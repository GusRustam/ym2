Imports AdfinXRtLib
Imports YieldMap.Commons
Imports NLog
Imports System.Threading

Namespace Tools.Chains
    Public Class ChainItemsData 
        Inherits EventArgs
        Public Handled As Boolean = False
        Public ChainName AS String
        Private ReadOnly _failed As Boolean
        Private ReadOnly _listItems As New List(Of String)
        
        Sub New(ByVal failed As Boolean, ByVal listItems As List(Of String), ByVal chainName As String)
            _failed = failed
            _listItems = listItems
            Me.ChainName = chainName
        End Sub

        Public ReadOnly Property ListItems() As List(Of String)
            Get
                Return _listItems
            End Get
        End Property

        Public ReadOnly Property Failed() As Boolean
            Get
                Return _failed
            End Get
        End Property
    End Class 

    ''' <summary>
    ''' Этот класс отвечает за выполнение загрузки одного набора данных
    ''' Поскольку загрузка осуществляется асинхронно, и, к тому же, может зависать,
    ''' этот класс запускает отдельный поток (читатель), и выделяет ему определенный 
    ''' интервал времени. Если в течение этого интервала читатель не успевает получить
    ''' данные, он уничтожается и никаких данных не возвращается.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ChainHandler
        Private Shared ReadOnly Logger As Logger  = GetLogger(GetType(ChainHandler))

        Private ReadOnly _chainName As String
        Private ReadOnly _params As String
        Private ReadOnly _timeOut As Integer
        
        Private ReadOnly _loaderThread As Thread = new Thread(AddressOf StartLoadingData)
        Private ReadOnly _lockObject As New Object
        Private ReadOnly _listItems As New List(Of String)

        Private ReadOnly _chainManager As AdxRtChain = Eikon.SDK.CreateAdxRtChain()
        Private _failed As Boolean = False
        
        Public Delegate Sub DataEventHandler(ByRef sender As Object, ByVal e As ChainItemsData)

        ''' <summary>
        ''' Событие, обозначающее, что по данной цепочке поступили данные
        ''' </summary>
        ''' <remarks></remarks>
        Public Event OnData As DataEventHandler
        
        Public Sub New(ByVal chainMan As AdxRtChain, ByVal chainName As String, ByVal params As String, ByVal timeOut As Integer)
            _chainManager = chainMan
            _chainName = chainName
            _params = params
            _timeOut = timeOut
        End Sub
        
        Public Property Failed() As Boolean
            Get
                Return _failed
            End Get
            Set (ByVal value As Boolean)
                _failed = value
            End Set
        End Property
        
        Public ReadOnly Property Items() As List(Of String)
            Get
                Return _listItems
            End Get
        End Property

        ''' <summary>
        ''' Запускает поток - читатель данных из данной цепочки
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub DoLoadList()
            Logger.Debug("DoLoadList()")
            
            _loaderThread.Start()
            _loaderThread.IsBackground = True
        End Sub

        Private Sub StartLoadingData()
            Logger.Debug("StartLoadingData()")
            Try
                ' Запрашиваем данные и ставим обработчик события получения данных
                With _chainManager
                    .Source = "IDN"
                    .ItemName = _chainName
                    .Mode = _params
                    .RequestChain()
                    AddHandler .OnUpdate, AddressOf Chain_OnUpdate
                End With

                ' Запускаем слушатель событий на заданный срок
                SyncLock (_lockObject) ' Захватываем объект блокировки
                    Monitor.Pulse(_lockObject) ' Передаем управление следющему ожидающему потоку 

                    ' Освободить нас должен обработчик события "Поступили свежие данные"
                    If Not Monitor.Wait(_lockObject, TimeSpan.FromSeconds(_timeOut)) Then
                        Logger.Trace("No data arrived after timeout of {0} seconds", _timeOut)
                        ' Не дождались!
                        Failed = True
                        RemoveHandler _chainManager.OnUpdate, AddressOf Chain_OnUpdate
                    Else
                        Logger.Trace("Data have arrived")
                    End If
                End SyncLock

                ' Если подвисли, то
                If Failed Then
                    SyncLock (_listItems)
                        _listItems.Clear() ' Уберем все лишнее 
                    End SyncLock
                End If
                RaiseEvent OnData(Me, New ChainItemsData(Failed, _listItems, _chainName))
            Catch ex As Exception
                Logger.ErrorException("Failed to start loading chain", ex)
                Logger.Error("Exception = {0}", ex.ToString())
                Failed = True
                RaiseEvent OnData(Me, New ChainItemsData(Failed, Nothing, _chainName))
            End Try

        End Sub

        Private Sub Chain_OnUpdate(ByVal datastatus As RT_DataStatus)
            Logger.Debug("ChainManager_OnUpdate()")
            
            ' Если все данные на месте, то
            If dataStatus = RT_DataStatus.RT_DS_FULL Then
                SyncLock (_listItems)
                    _listItems.Clear
                End SyncLock
                Dim data As Array = _chainManager.Data
                If data IsNot Nothing Then
                    Logger.Trace("data -> {0}, rank is {1}", data, data.Rank)
                    ' Сохраняем полученные данные в массив
                    For i As Integer = data.GetLowerBound(0) To data.GetUpperBound(0)
                        If Failed Then
                            Logger.Trace("It seems I'm being stopped by timeout")
                            Exit For
                        End If
                        Dim val As String = data.GetValue(i)
                        SyncLock (_listItems)
                            _listItems.Add(val)
                        End SyncLock
                        Logger.Trace("data({0}) is {1}", i, val)
                    Next
                End If
            End If

            SyncLock (_lockObject)
                Monitor.Pulse(_lockObject)
            End SyncLock
        End Sub
        
    End Class
End NameSpace