Imports AdfinXRtLib
Imports System.Globalization
Imports System.Threading
Imports YieldMap.Commons
Imports NLog

Namespace Tools.History
    Public Class HistoryTaskDescr
        Public Item As String
        Public Fields As List(Of String)
        Public InterestingFields As List(Of String)
        Public StartDate As DateTime
        Public EndDate As DateTime
        Public Frequency As String

        Public SingleValue As Boolean
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

        Public Function GetPropByName(propName As String) As Double?
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


        Public Sub SetPropByValue(propName As String, propValue As String)
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

    Public Class HistoryLoadManager
        Private Shared ReadOnly Logger As Logger = GetLogger(GetType(HistoryLoadManager))
        Private _historyManager As AdxRtHistory

        Private _ric As String
        Private _interestingFields As List(Of String)
        Private _success As Boolean = False
        Private _finished As Boolean = False

        Public ReadOnly Property Success As Boolean
            Get
                Return _success
            End Get
        End Property
        Public ReadOnly Property Finished As Boolean
            Get
                Return _finished
            End Get
        End Property

        Public Delegate Sub NewDataDelegate(ByVal hst As HistoryLoadManager, ByVal ric As String, ByVal datastatus As RT_DataStatus, ByVal data As Dictionary(Of Date, HistoricalItem))
        Public Event NewData As NewDataDelegate

        Public Sub New(ByVal descr As HistoryTaskDescr, ByVal handler As NewDataDelegate)
            Logger.Debug("New(descr)")
            _historyManager = New AdxRtHistory
            StartTask(descr, handler)
        End Sub

        Public Sub New(manager As AdxRtHistory)
            Logger.Debug("New(manager)")
            _historyManager = manager
        End Sub

        Public Sub StartTask(ByVal descr As HistoryTaskDescr, ByVal handler As NewDataDelegate)
            Logger.Warn("StartTask({0})", descr.Item)
            _interestingFields = descr.InterestingFields
            Try
                _ric = descr.Item
                If descr.SingleValue Then
                    _historyManager.Mode = String.Format(CultureInfo.CreateSpecificCulture("en-US"),
                        "NBEVENTS:1 FRQ:{0} HEADER:YES START:{1:ddMMMyy}", descr.Frequency, descr.StartDate).ToUpper()
                Else
                    _historyManager.Mode = String.Format(CultureInfo.CreateSpecificCulture("en-US"),
                        "FRQ:{0} HEADER:YES START:{1:ddMMMyy} END:{2:ddMMMyy}", descr.Frequency, descr.StartDate, descr.EndDate).ToUpper()
                End If
                With _historyManager
                    .Source = "IDN"
                    .ItemName = descr.Item
                    .RequestHistory(descr.Fields.Aggregate(Function(str, elem) str + ", " + elem))
                    AddHandler NewData, handler
                    .ErrorMode = AdxErrorMode.EXCEPTION
                    If .Data IsNot Nothing Then
                        ParseData()
                    Else
                        AddHandler .OnUpdate, AddressOf OnNewData
                    End If
                End With
                _success = True
            Catch ex As Exception
                Logger.ErrorException("Error in history downloader", ex)
                Logger.Error("Exception = {0}", ex.ToString())
            End Try
        End Sub

        Private Sub OnNewData(ByVal datastatus As RT_DataStatus)
            Logger.Debug("OnNewData({0})", _ric)
            ThreadPool.QueueUserWorkItem(
                Sub(state)
                    Logger.Trace("state({0})={1}", _ric, datastatus.ToString())
                    If datastatus = RT_DataStatus.RT_DS_FULL Then
                        ParseData()
                    Else
                        Logger.Warn("Data Status is {0}; will omit the data. Error message is {1}", datastatus, _historyManager.ErrorString)
                        RaiseEvent NewData(Me, _ric, datastatus, Nothing)
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
                        If _interestingFields.Contains(itemName) Then
                            Try
                                bhd.SetPropByValue(itemName, propValue)
                                Logger.Trace("Adding {0} -> {1}", itemName, propValue)
                            Catch ex As Exception
                                Logger.Trace("Failed adding {0} -> {1}", itemName, propValue)
                            End Try
                        End If
                    Next
                    res.Add([date], bhd)

                Next
                If res.Count > 0 Then
                    Logger.Debug("Will return {0} for {1}",
                        res.Aggregate("",
                        Function(str, item)
                            Return str + Environment.NewLine + String.Format("{0:dd/MM/yy} -> [{1}]", item.Key, item.Value)
                        End Function),
                    _ric)
                Else
                    Logger.Warn("Will return nothing for {0}", _ric)
                End If
                RaiseEvent NewData(Me, _ric, _historyManager.DataStatus, res)

            Catch ex As Exception
                Logger.ErrorException("Failed to parse historical data", ex)
                Logger.Error("Exception = {0}", ex.ToString())
                RaiseEvent NewData(Me, _ric, _historyManager.DataStatus, Nothing)
            Finally
                _finished = True
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
End NameSpace