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
    End Enum

    Public Class HistoryLoadManager_v2
        Private Shared ReadOnly Logger As Logger = GetLogger(GetType(HistoryLoadManager_v2))
        Private WithEvents _historyManager As AdxRtHistory = Eikon.SDK.CreateAdxRtHistory()

        Private _ric As String

        Public Delegate Sub NewDataDelegate(ByVal ric As String, ByVal datastatus As RT_DataStatus, ByVal data As Dictionary(Of Date, HistoricalItem))
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


        'Public Sub New(ByVal manager As AdxRtHistory)
        '    Logger.Debug("New(manager)")
        '    _historyManager = manager
        'End Sub

        Public Sub StartTask(ByVal item As String, ByVal fields As String, ByVal since As Date, ByVal till As Date, Optional ByVal frequency As String = "D")
            Logger.Warn("StartTask({0})", item)
            Try
                _ric = item
                With _historyManager
                    .Source = "IDN"
                    .Mode = String.Format(
                        CultureInfo.CreateSpecificCulture("en-US"),
                        "FRQ:{0} HEADER:YES START:{1:ddMMMyy} END:{2:ddMMMyy}", frequency, since, till).ToUpper()
                    .ItemName = item
                    .RequestHistory(fields)
                    .ErrorMode = AdxErrorMode.EXCEPTION

                    If .Data IsNot Nothing Then
                        ParseData()
                    End If
                End With
            Catch ex As Exception
                Logger.ErrorException("Error in history downloader", ex)
                Logger.Error("Exception = {0}", ex.ToString())
            End Try
        End Sub

        Private Sub OnNewData(ByVal datastatus As RT_DataStatus) Handles _historyManager.OnUpdate
            Logger.Debug("OnNewData({0})", _ric)
            ThreadPool.QueueUserWorkItem(
                Sub(state)
                    Logger.Trace("state({0})={1}", _ric, datastatus.ToString())
                    If datastatus = RT_DataStatus.RT_DS_FULL Then
                        ParseData()
                    Else
                        Logger.Warn("Data Status is {0}; will omit the data. Error message is {1}", datastatus, _historyManager.ErrorString)
                        RaiseEvent HistoricalData(_ric, datastatus, Nothing)
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
                            Return str +
                                Environment.NewLine +
                                String.Format("{0:dd/MM/yy} -> [{1}]", item.Key, item.Value)
                        End Function),
                    _ric)
                Else
                    Logger.Warn("Will return nothing for {0}", _ric)
                End If
                RaiseEvent HistoricalData(Me, _ric, _historyManager.DataStatus, res)

            Catch ex As Exception
                Logger.ErrorException("Failed to parse historical data", ex)
                Logger.Error("Exception = {0}", ex.ToString())
                RaiseEvent HistoricalData(Me, _ric, _historyManager.DataStatus, Nothing) ' todo when  i kill program this bullshit fails
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
End Namespace