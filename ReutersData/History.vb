Imports AdfinXRtLib
Imports System.Globalization
Imports System.Threading
Imports NLog
Imports Settings

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

Public Class History
    Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(History))
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

    Public Delegate Sub NewDataDelegate(ByVal ric As String, ByVal data As Dictionary(Of Date, HistoricalItem), ByVal rawData As Dictionary(Of DateTime, RawHistoricalItem))
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
                    ' why raise empty event? todo: only if U want to update status
                    ' RaiseEvent HistoricalData(_ric, New LoaderStatus(Finished, Err, LoaderErrReason.DataStatus), datastatus, Nothing)
                End If
            End Sub
            )
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
End Class