Imports AdfinXRtLib
Imports System.Globalization
Imports System.Threading
Imports NLog
Imports Settings
Imports System.Threading.Tasks
Imports CommonController
Imports System.Runtime.InteropServices

Public Class RawHistoricalItem
    Private ReadOnly _items As New Dictionary(Of String, String)

    Default Public Property Data(ByVal key As String) As String
        Get
            Return _items(key)
        End Get
        Set(ByVal value As String)
            _items(key) = value
        End Set
    End Property

    Public ReadOnly Property Has(ByVal key As String) As Boolean
        Get
            Return _items.ContainsKey(key)
        End Get
    End Property

    Public ReadOnly Property Keys() As List(Of String)
        Get
            Return _items.Keys.ToList()
        End Get
    End Property
End Class

Public Class HistoricalItem
    Public AltOpen As Double
    Public Open As Double
    Public High As Double
    Public Low As Double
    Public Close As Double
    Public VWAP As Double

    Public Volume As Double
    Public Value As Double
    Public MarketValue As Double

    Public CloseYield As Double

    Public Bid As Double
    Public Ask As Double

    Public ReadOnly Property Mid() As Double
        Get
            If Bid > 0 And Ask > 0 Then
                Return (Bid + Ask) / 2
            ElseIf Bid > 0 Then
                Return Bid
            ElseIf Ask > 0 Then
                Return Ask
            Else
                Return 0
            End If
        End Get
    End Property

    Public Function GetPropByName(ByVal propName As String) As Double?
        Dim res As Double? = Nothing
        Try
            Select Case propName.ToUpper()
                Case "OPEN" : res = Open
                Case "ALT OPEN" : res = AltOpen
                Case "HIGH" : res = High
                Case "LOW" : res = Low
                Case "CLOSE" : res = Close
                Case "VWAP" : res = VWAP
                Case "VOLUME" : res = Volume
                Case "MARKET VALUE" : res = MarketValue
                Case "CLOSE YIELD" : res = CloseYield
                Case "BID" : res = Bid
                Case "ASK" : res = Ask
                Case "VALUE" : res = Value
            End Select
        Catch ex As Exception
            res = Nothing
        End Try
        Return res
    End Function

    Public Sub SetPropByValue(ByVal propName As String, ByVal propValue As String)
        Dim val = CDbl(propValue)
        Select Case propName
            Case "OPEN" : Open = val
            Case "ALT OPEN" : AltOpen = val
            Case "HIGH" : High = val
            Case "LOW" : Low = val
            Case "CLOSE" : Close = val
            Case "VWAP" : VWAP = val
            Case "VOLUME" : Volume = val
            Case "MARKET VALUE" : MarketValue = val
            Case "CLOSE YIELD" : CloseYield = val
            Case "BID" : Bid = val
            Case "ASK" : Ask = val
            Case "VALUE" : Value = val
        End Select
    End Sub

    Public Overrides Function ToString() As String
        Dim res As String = ""
        res += IIf(AltOpen > 0, String.Format("[AltOpen] = {0:F2}; ", AltOpen), "")
        res += IIf(Open > 0, String.Format("[Open] = {0:F2}; ", Open), "")
        res += IIf(High > 0, String.Format("[High] = {0:F2}; ", High), "")
        res += IIf(Low > 0, String.Format("[Low] = {0:F2}; ", Low), "")
        res += IIf(Close > 0, String.Format("[Close] = {0:F2}; ", Close), "")
        res += IIf(VWAP > 0, String.Format("[VWAP] = {0:F2}; ", VWAP), "")
        res += IIf(Volume > 0, String.Format("[Volume] = {0:F2}; ", Volume), "")
        res += IIf(MarketValue > 0, String.Format("[MarketValue] = {0:F2}; ", MarketValue), "")
        res += IIf(CloseYield > 0, String.Format("[CloseYield] = {0:F2}; ", CloseYield), "")
        res += IIf(Bid > 0, String.Format("[Bid] = {0:F2}; ", Bid), "")
        res += IIf(Ask > 0, String.Format("[Ask] = {0:F2}", Ask), "")
        res += IIf(Value > 0, String.Format("[Value] = {0:F2}", Value), "")
        Return res
    End Function

    Public Function SomePrice() As Boolean
        Dim arr = {Open, High, Low, Close, AltOpen, VWAP, Value}
        Return arr.Any(Function(elem) elem > 0)
    End Function
End Class

Public Class HistoryBlock
    Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(HistoryBlock))
    Public Class DataCube
        Private ReadOnly _rfd As New Dictionary(Of Date, Dictionary(Of String, Dictionary(Of String, String)))
        Private ReadOnly _rics As List(Of String)
        Private ReadOnly _fields As List(Of String)
        Private ReadOnly _dates As List(Of Date)

        Public Sub New(ByVal rics As List(Of String), ByVal fields As List(Of String), ByVal since As Date, ByVal till As Date)
            _rics = New List(Of String)(rics)
            _fields = New List(Of String)(fields)
            _dates = New List(Of Date)
            For Each dt As DateTime In Enumerable.Range(0, (till - since).Days).Select(Function(i) since.AddDays(i))
                _dates.Add(dt)
                _rfd(dt) = New Dictionary(Of String, Dictionary(Of String, String))
                For Each ric In rics
                    _rfd(dt)(ric) = New Dictionary(Of String, String)
                    For Each field In fields
                        _rfd(dt)(ric)(field) = ""
                    Next
                Next
            Next dt
        End Sub

        Public Sub Add(ByVal ric As String, ByVal field As String, ByVal dt As Date, ByVal val As String)
            _rfd(dt)(ric)(field) = val
        End Sub

        Public ReadOnly Property RicDate(ByVal ric As String, ByVal dt As Date) As Dictionary(Of String, String)
            Get
                Return New Dictionary(Of String, String)(_rfd(dt)(ric))
            End Get
        End Property

        Public ReadOnly Property RicField(ByVal ric As String, ByVal field As String) As Dictionary(Of Date, String)
            Get
                Dim res As New Dictionary(Of Date, String)
                For Each item In _rfd
                    Dim dt = item.Key
                    Dim rfv = item.Value
                    For Each fieldVal In From elem In rfv Where elem.Key = ric Select elem.Value
                        res.Add(dt, fieldVal(field))
                    Next
                Next
                Return res
            End Get
        End Property

        Public ReadOnly Property FieldDate(ByVal field As String, ByVal dt As Date) As Dictionary(Of String, String)
            Get
                Dim res As New Dictionary(Of String, String)
                For Each item In (From elem As KeyValuePair(Of String, Dictionary(Of String, String)) In _rfd(dt))
                    Dim ric = item.Key
                    Dim fv = item.Value
                    res.Add(ric, fv(field))
                Next
                Return res
            End Get
        End Property

        Public ReadOnly Property Rics() As List(Of String)
            Get
                Return _rics
            End Get
        End Property

        Public ReadOnly Property Fields() As List(Of String)
            Get
                Return _fields
            End Get
        End Property

        Public ReadOnly Property Dates() As List(Of Date)
            Get
                Return _dates
            End Get
        End Property

        Public ReadOnly Property RicData(ByVal ric As String) As Dictionary(Of Date, Dictionary(Of String, String))
            Get
                Dim res As New Dictionary(Of Date, Dictionary(Of String, String))
                For Each dt In _dates
                    res(dt) = _rfd(dt)(ric)
                Next
                Return res
            End Get
        End Property

        Public ReadOnly Property RicData2(ByVal ric As String) As Dictionary(Of String, Dictionary(Of Date, String))
            Get
                Dim res As New Dictionary(Of String, Dictionary(Of Date, String))
                For Each field In _fields
                    res(field) = New Dictionary(Of Date, String)
                    For Each dt In _dates
                        res(field)(dt) = _rfd(dt)(ric)(field)
                    Next
                Next
                Return res
            End Get
        End Property
    End Class

    Private _result As DataCube

    Public Event History As Action(Of DataCube)

    Private _fields As List(Of String)
    Private _since As Date
    Private _till As Date

    Private _countdown As CountdownEvent
    Private WithEvents _shutdownManager As ShutdownController = ShutdownController.Instance
    Private Shared ReadOnly _historyManagers As New List(Of AdxRtHistory)

    Private Sub ShutdownNow() Handles _shutdownManager.ShutdownNow
        For Each hM In _historyManagers
            Marshal.ReleaseComObject(hM)
            hM = Nothing
        Next
        _historyManagers.Clear()
    End Sub

    Private Sub WaiterThread()
        Logger.Info("WaiterThread()")
        If Not _countdown.Wait(TimeSpan.FromSeconds(30)) Then
            Logger.Warn("{0} threads are still working", _countdown.CurrentCount)
            RaiseEvent History(Nothing)
        Else
            Logger.Info("Ok!")
            RaiseEvent History(_result)
        End If
    End Sub

    Private Sub RicLoaderThread(ByVal item As String)
        Dim historyManager As AdxRtHistory = Eikon.Sdk.CreateAdxRtHistory()
        _historyManagers.Add(historyManager)

        AddHandler historyManager.OnUpdate, Sub() ParseData(historyManager, item)
        Dim ric = item
        If item(0) = "/" Then ric = item.Substring(1)
        Logger.Warn("StartTask({0})", ric)
        Try
            With historyManager
                .ErrorMode = AdxErrorMode.EXCEPTION
                .Source = SettingsManager.Instance.ReutersDataSource
                .Mode = String.Format(
                    CultureInfo.CreateSpecificCulture("en-US"),
                    "FRQ:{0} HEADER:YES START:{1:ddMMMyy} END:{2:ddMMMyy}", "D", _since, _till).ToUpper()
                .ItemName = ric
                .RequestHistory(String.Join(",", _fields))
                If .Data IsNot Nothing Then ParseData(historyManager, ric)
            End With
        Catch ex As Exception
            Logger.ErrorException("Error in history downloader", ex)
            Logger.Error("Exception = {0}", ex.ToString())
        End Try
    End Sub

    Private Sub ParseData(ByVal historyManager As AdxRtHistory, ByVal ric As String)
        Try
            Dim data As Array = historyManager.Data

            Dim firstRow = data.GetLowerBound(0)
            Dim lastRow = data.GetUpperBound(0)
            Dim firstColumn = data.GetLowerBound(1)
            Dim lastColumn = data.GetUpperBound(1)

            For col = firstColumn + 1 To lastColumn
                Dim theValue = data.GetValue(0, col)
                If Not IsDate(theValue) Then Continue For

                Dim dt = CDate(theValue)
                SyncLock _result
                    For row = firstRow To lastRow
                        Dim itemName = data.GetValue(row, 0).ToString()
                        Dim propValue = data.GetValue(row, col)
                        Try
                            If IsNumeric(propValue) Or IsDate(propValue) Then
                                Logger.Trace("Added {0} -> {1}", itemName, propValue)
                                _result.Add(ric, itemName, dt, propValue)
                            Else
                                Logger.Trace("Skipped {0} -> {1}", itemName, propValue)
                            End If
                        Catch ex As Exception
                            Logger.Trace("Failed adding {0} -> {1} at {2:dd/MM/yy} for ric {3}", itemName, propValue, dt, ric)
                            Logger.Trace("exception = {0}", ex.ToString())
                        End Try
                    Next
                End SyncLock
            Next
        Catch ex As Exception
            Logger.ErrorException("Failed to parse historical data", ex)
            Logger.Error("Exception = {0}", ex.ToString())
        Finally
            Try
                _countdown.Signal()
                historyManager.FlushData()
            Catch ex As Exception
                Logger.WarnException("Failed to stop task", ex)
                Logger.Warn("Exception = {0}", ex.ToString())
            End Try
        End Try
    End Sub

    Public Sub Load(ByVal rics As List(Of String), ByVal fields As List(Of String), ByVal since As Date, ByVal till As Date)
        ' Some general-purpose fields (could use clojures instead)
        _fields = fields
        _since = since
        _till = till

        ' main tool to wait all my threads
        _countdown = New CountdownEvent(rics.Count())

        ' wut to return
        _result = New DataCube(rics, fields, since, till)

        ' data collector
        Dim waiter = New Thread(AddressOf WaiterThread)
        waiter.Start()

        ' individual rics
        Parallel.ForEach(rics, AddressOf RicLoaderThread)
    End Sub
End Class

Public Class History
    Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(History))
    Private WithEvents _historyManager As AdxRtHistory = Eikon.Sdk.CreateAdxRtHistory()
    Private WithEvents _shutdownManager As ShutdownController = ShutdownController.Instance

    Private _ric As String
    Private _error As Boolean
    Private _finished As Boolean
    Private ReadOnly _lock As New Object

    Public Property Finished As Boolean
        Get
            Return _finished
        End Get
        Set(ByVal value As Boolean)
            SyncLock (_lock)
                _finished = value
            End SyncLock
        End Set
    End Property

    Public Property Err As Boolean
        Get
            Return _error
        End Get
        Set(ByVal value As Boolean)
            SyncLock (_lock)
                _error = value
            End SyncLock
        End Set
    End Property

    Public Delegate Sub NewDataDelegate(ByVal ric As String, ByVal data As Dictionary(Of Date, HistoricalItem), ByVal rawData As Dictionary(Of DateTime, RawHistoricalItem))
    Public Event HistoricalData As NewDataDelegate

    Public Sub StartTask(ByVal item As String, ByVal fields As String, ByVal since As Date, ByVal till As Date, Optional ByVal frequency As String = "D", Optional ByVal timeOut As Integer = 30)
        Dim ric = item
        If item(0) = "/" Then ric = item.Substring(1)
        Logger.Warn("StartTask({0})", ric)
        Try
            Err = False
            Finished = False
            _ric = item
            With _historyManager
                .ErrorMode = AdxErrorMode.EXCEPTION
                .Source = SettingsManager.Instance.ReutersDataSource
                .Mode = String.Format(
                    CultureInfo.CreateSpecificCulture("en-US"),
                    "FRQ:{0} HEADER:YES START:{1:ddMMMyy} END:{2:ddMMMyy}", frequency, since, till).ToUpper()
                .ItemName = ric
                .RequestHistory(fields)
                If .Data IsNot Nothing Then
                    ParseData()
                    Finished = True
                Else
                    Dim waiterAndRunnerThread = New Thread(New ThreadStart(
                        Sub()
                            Logger.Trace("{0} waiter started", item)
                            Thread.Sleep(TimeSpan.FromSeconds(timeOut))
                            If Not Finished Then
                                Logger.Warn("{0} waiter finihed with error", item)
                                Err = True
                                Finished = True
                                RaiseEvent HistoricalData(item, Nothing, Nothing)
                            Else
                                Logger.Info("{0} waiter has finished successfully", item)
                            End If
                        End Sub))
                    waiterAndRunnerThread.Start()
                End If
            End With
        Catch ex As Exception
            Logger.ErrorException("Error in history downloader", ex)
            Logger.Error("Exception = {0}", ex.ToString())
        End Try
    End Sub

    Private Sub OnNewData(ByVal datastatus As RT_DataStatus) Handles _historyManager.OnUpdate
        Logger.Debug("OnNewData({0})", _ric)
        If Finished Then
            Logger.Warn("Data arrived after timeout)", _ric)
            Return
        End If
        ThreadPool.QueueUserWorkItem(
            Sub(state)
                Logger.Trace("state({0})={1}", _ric, datastatus.ToString())
                If datastatus = RT_DataStatus.RT_DS_FULL Then
                    ParseData()
                Else
                    Logger.Warn(String.Format("{2}: Data Status is {0}; will omit the data. Error message is {1}", datastatus, _historyManager.ErrorString, _ric))
                End If
            End Sub)
    End Sub

    Private Sub ParseData()
        Try
            Dim res As New Dictionary(Of DateTime, HistoricalItem)
            Dim rawRes As New Dictionary(Of DateTime, RawHistoricalItem)

            Dim data As Array = _historyManager.Data

            Dim firstRow = data.GetLowerBound(0)
            Dim lastRow = data.GetUpperBound(0)
            Dim firstColumn = data.GetLowerBound(1)
            Dim lastColumn = data.GetUpperBound(1)

#If DEBUG Then
            For row = firstRow To lastRow
                For col = firstColumn To lastColumn
                    Logger.Trace("({0},{1}) -> {2}", row, col, data.GetValue(row, col))
                Next
            Next
#End If

            For col = firstColumn + 1 To lastColumn
                Dim theValue = data.GetValue(0, col)
                If Not IsDate(theValue) Then Continue For

                Dim dt = CDate(theValue)
                Dim bhd As New HistoricalItem
                Dim thd As New RawHistoricalItem
                For row = firstRow + 1 To lastRow
                    Dim itemName = data.GetValue(row, 0).ToString()
                    Dim propValue = data.GetValue(row, col)
                    Try
                        bhd.SetPropByValue(itemName, propValue)
                        If IsNumeric(propValue) Or IsDate(propValue) Then thd(itemName) = propValue
                        Logger.Trace("Adding {0} -> {1}", itemName, propValue)
                    Catch ex As Exception
                        Logger.Trace("Failed adding {0} -> {1}", itemName, propValue)
                    End Try
                Next
                res.Add(dt, bhd)
                rawRes.Add(dt, thd)
            Next
            If res.Count > 0 Then
                Logger.Debug("Will return {0} for {1}",
                             res.Aggregate("", Function(str, item) String.Format("{0} {2:dd/MM/yy} -> [{3}]{1}", str, Environment.NewLine, item.Key, item.Value)),
                             _ric)
            Else
                Logger.Warn("Will return nothing for {0}", _ric)
            End If
            Err = False
            Finished = True
            RaiseEvent HistoricalData(_ric, res, rawRes)
        Catch ex As Exception
            Logger.ErrorException("Failed to parse historical data", ex)
            Logger.Error("Exception = {0}", ex.ToString())
            Err = True
            Finished = True
            Try
                RaiseEvent HistoricalData(_ric, Nothing, Nothing)
            Catch ex1 As Exception
                Logger.ErrorException("Failed to stop task", ex)
                Logger.Error("Exception = {0}", ex.ToString())
            End Try
        Finally
            Try
                _historyManager.FlushData()
                _historyManager = Nothing
            Catch ex As Exception
                Logger.WarnException("Failed to stop task", ex)
                Logger.Warn("Exception = {0}", ex.ToString())
            End Try
        End Try
    End Sub

    Private Sub ShutdownNow() Handles _shutdownManager.ShutdownNow
        Marshal.ReleaseComObject(_historyManager)
        _historyManager = Nothing
    End Sub
End Class