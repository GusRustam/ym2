Imports AdfinXRtLib
Imports System.Globalization
Imports System.Threading
Imports YieldMap.Commons
Imports NLog

Namespace Tools.History
    Public Enum HistoryStatus
        Full
        Null
        Failed
        TimeOut
        Part
        None ' a special case
    End Enum

    Public Enum LoaderErrReason
        None
        Timeout
        DataStatus
        Exception
    End Enum

    Public Structure LoaderStatus
        Public Finished As Boolean
        Public Err As Boolean
        Public Reason As LoaderErrReason

        Public Sub New(ByVal finished As Boolean, ByVal err As Boolean, ByVal reason As LoaderErrReason)
            Me.Finished = finished
            Me.Err = err
            Me.Reason = reason
        End Sub
    End Structure

    Public Class HistoryLoadManager_v2
        Private Shared ReadOnly Logger As Logger = GetLogger(GetType(HistoryLoadManager_v2))
        Private WithEvents _historyManager As AdxRtHistory = Eikon.SDK.CreateAdxRtHistory()

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

        Public Delegate Sub NewDataDelegate(ByVal ric As String, ByVal status As LoaderStatus, ByVal hstatus As HistoryStatus, ByVal data As Dictionary(Of Date, HistoricalItem))
        Public Event HistoricalData As NewDataDelegate

        Public Shared Function ParseDataStatus(ByVal status As RT_DataStatus) As HistoryStatus
            Select Case status
                Case RT_DataStatus.RT_DS_FULL : Return HistoryStatus.Full
                Case RT_DataStatus.RT_DS_NULL_EMPTY : Return HistoryStatus.Null
                Case RT_DataStatus.RT_DS_NULL_ERROR : Return HistoryStatus.Failed
                Case RT_DataStatus.RT_DS_NULL_TIMEOUT : Return HistoryStatus.Failed
                Case RT_DataStatus.RT_DS_PARTIAL : Return HistoryStatus.Part
                Case Else : Return Nothing
            End Select
        End Function

        Public Sub StartTask(ByVal item As String, ByVal fields As String, ByVal since As Date, ByVal till As Date, Optional ByVal frequency As String = "D", Optional ByVal timeOut As Integer = 30)
            Logger.Warn("StartTask({0})", item)
            Try
                Err = False
                Finished = False
                _ric = item
                With _historyManager
                    .ErrorMode = AdxErrorMode.EXCEPTION
                    .Source = "IDN"
                    .Mode = String.Format(
                        CultureInfo.CreateSpecificCulture("en-US"),
                        "FRQ:{0} HEADER:YES START:{1:ddMMMyy} END:{2:ddMMMyy}", frequency, since, till).ToUpper()
                    .ItemName = item
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
                                       RaiseEvent HistoricalData(item, New LoaderStatus(Finished, Err, LoaderErrReason.Timeout), HistoryStatus.None, Nothing)
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
                        ' why raise empty event? todo: only if U want to update status
                        ' RaiseEvent HistoricalData(_ric, New LoaderStatus(Finished, Err, LoaderErrReason.DataStatus), datastatus, Nothing)
                    End If
                End Sub
            )
        End Sub

        Private Sub ParseData()
            Try
                Dim res As New Dictionary(Of DateTime, HistoricalItem)

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

                    Dim [date] = CDate(theValue)
                    Dim bhd As New HistoricalItem
                    For row = firstRow + 1 To lastRow
                        Dim itemName = data.GetValue(row, 0).ToString()
                        Dim propValue = data.GetValue(row, col)
                        Try
                            bhd.SetPropByValue(itemName, propValue)
                            Logger.Trace("Adding {0} -> {1}", itemName, propValue)
                        Catch ex As Exception
                            Logger.Trace("Failed adding {0} -> {1}", itemName, propValue)
                        End Try
                    Next
                    res.Add([date], bhd)

                Next
                If res.Count > 0 Then
                    Logger.Debug("Will return {0} for {1}",
                        res.Aggregate("",
                        Function(str, item)
                            Return str +
                                Environment.NewLine +
                                String.Format("{0:dd/MM/yy} -> [{1}]", item.Key, item.Value)
                        End Function),
                    _ric)
                Else
                    Logger.Warn("Will return nothing for {0}", _ric)
                End If
                Err = False
                Finished = True
                RaiseEvent HistoricalData(_ric, New LoaderStatus(Finished, Err, LoaderErrReason.None), ParseDataStatus(_historyManager.DataStatus), res)
            Catch ex As Exception
                Logger.ErrorException("Failed to parse historical data", ex)
                Logger.Error("Exception = {0}", ex.ToString())
                Err = True
                Finished = True
                Try
                    If _historyManager IsNot Nothing Then
                        RaiseEvent HistoricalData(_ric, New LoaderStatus(Finished, Err, LoaderErrReason.Exception), ParseDataStatus(_historyManager.DataStatus), Nothing) ' todo when  i kill program this bullshit fails
                    Else
                        RaiseEvent HistoricalData(_ric, New LoaderStatus(Finished, Err, LoaderErrReason.Exception), HistoryStatus.None, Nothing) ' todo when  i kill program this bullshit fails
                    End If
                Catch ex1 As Exception
                    Logger.ErrorException("Failed to stop task", ex)
                    Logger.Error("Exception = {0}", ex.ToString())
                End Try
            Finally
                Try
                    RemoveHandler _historyManager.OnUpdate, AddressOf OnNewData
                    _historyManager.FlushData()
                    _historyManager = Nothing
                Catch ex As Exception
                    Logger.WarnException("Failed to stop task", ex)
                    Logger.Warn("Exception = {0}", ex.ToString())
                End Try
            End Try
        End Sub
    End Class
End Namespace