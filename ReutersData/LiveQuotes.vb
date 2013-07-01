Imports AdfinXRtLib
Imports NLog
Imports CommonController
Imports System.Runtime.InteropServices

Public Class LiveQuotes
    Implements IDisposable

    Public Delegate Sub OnNewData(ByVal ricFieldValue As Dictionary(Of String, Dictionary(Of String, Double)))
    Public Event NewData As OnNewData

    Private WithEvents _shutdownManager As ShutdownController = ShutdownController.Instance

    Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(LiveQuotes))
    Private WithEvents _listManager As AdxRtList = Eikon.Sdk.CreateAdxRtList()

    Public Sub CancelAll()
        Try
            _listManager.UnregisterAllItems()
            _listManager.StopUpdates()
        Catch ex As Exception
            Logger.ErrorException("Failed to unregister items", ex)
            Logger.Error("Exception = {0}", ex.ToString())
        End Try
    End Sub

    Public Sub CancelItem(ByVal ric As String)
        Try
            _listManager.UnregisterItems(ric)
        Catch ex As Exception
            Logger.ErrorException("Failed to unregister items", ex)
            Logger.Error("Exception = {0}", ex.ToString())
        End Try
    End Sub

    Public Sub CancelItems(ByVal rics As List(Of String))
        Try
            _listManager.UnregisterItems(String.Join(",", rics))
        Catch ex As Exception
            Logger.ErrorException("Failed to unregister items", ex)
            Logger.Error("Exception = {0}", ex.ToString())
        End Try
    End Sub

    Public Sub AddItems(ByVal items As List(Of String), Optional ByVal fields As List(Of String) = Nothing)
        Dim itms = items.Aggregate(Function(str, elem) str & "," & elem)
        Dim flds = If(fields IsNot Nothing, fields.Aggregate(Function(str, elem) str & "," & elem), "")
        Logger.Debug("AddItems({0}, {1})", itms, flds)
        For Each item In From elem In items Where Not _listManager.IsRegisteredItem(elem)
            Try
                _listManager.RegisterItems(item, flds)
            Catch ex As Exception
                Logger.WarnException("Failed to start loading items", ex)
                Logger.Warn("Exception = {0}", ex.ToString())
            End Try
        Next
        _listManager.StartUpdates(RT_RunMode.RT_MODE_ONTIME_IF_UPDATED)
    End Sub

    Public Sub New()
        Logger.Debug("New()")
        With _listManager
            .Source = "IDN"
            .DebugLevel = RT_DebugLevel.RT_DEBUG_IMMEDIATE
            .Mode = String.Format("FRQ:{0}s", 10)
            .StartUpdates(RT_RunMode.RT_MODE_ONTIME_IF_UPDATED)
        End With
    End Sub

    Private Shared Sub InsertIntoRfv(ByRef rfv As Dictionary(Of String, Dictionary(Of String, Double)), ByVal ric As String, ByVal field As String, ByVal value As Double?)
        If Not rfv.ContainsKey(ric) Then rfv.Add(ric, New Dictionary(Of String, Double))
        Dim fv As Dictionary(Of String, Double) = rfv(ric)
        If Not fv.ContainsKey(field) Then fv.Add(field, value)
    End Sub

    Private Sub OnTimeHandler() Handles _listManager.OnTime
        Logger.Trace("OnTimeHandler()")
        Dim taskRicFieldValue As New Dictionary(Of String, Dictionary(Of String, Double)) ' RIC -> Field -> Value
        Try
            Dim items As Array = _listManager.ListItems(RT_ItemRowView.RT_IRV_UPDATED, RT_ItemColumnView.RT_ICV_STATUS)
            For k = items.GetLowerBound(0) To items.GetUpperBound(0)
                Try
                    Dim aItemName As String = items.GetValue(k, 0)

                    Dim status = _listManager.ItemStatus(aItemName)
                    If {RT_ItemStatus.RT_ITEM_OK, RT_ItemStatus.RT_ITEM_DELAYED}.Contains(status) Then
                        Dim fields As Array = _listManager.ListFields(aItemName, RT_FieldRowView.RT_FRV_UPDATED, RT_FieldColumnView.RT_FCV_VALUE)
                        If fields Is Nothing Then Continue For

                        For i = fields.GetLowerBound(0) To fields.GetUpperBound(0)
                            Dim val = fields.GetValue(i, 1)
                            Dim field = fields.GetValue(i, 0)
                            Try
                                Dim stts = _listManager.FieldStatus(aItemName, field)
                                If stts = RT_FieldStatus.RT_FIELD_OK Then
                                    If IsNumeric(val) Then
                                        InsertIntoRfv(taskRicFieldValue, aItemName, field, CDbl(val))
                                    End If
                                Else
                                    Logger.Warn("Field {0} with value {1} of item {2} status is {3}, skipping", field, val, aItemName, stts)
                                End If
                            Catch ex As Exception
                                Logger.WarnException(String.Format("Failed to read field/value {0}/{1}", field, val), ex)
                                Logger.Warn("Exception = {0}", ex.ToString())
                            End Try
                        Next i
                    Else
                        Logger.Warn("Item {0} status is {1}, skipping", aItemName, status)
                    End If

                Catch invOpEx As InvalidOperationException
                    Logger.Trace("rank is {0}", items.Rank)
                    Logger.ErrorException("Invalid operation", invOpEx)
                Catch ex As Exception
                    Logger.ErrorException("Exception in updates handler", ex)
                    Logger.Error("Exception = {0}", ex.ToString())
                End Try
            Next
            RaiseEvent NewData(taskRicFieldValue)
        Catch ex As Exception
            Logger.Error("Failed to retrieve realtime data: {0}", ex.ToString())
            Logger.ErrorException("Exception: ", ex)
        End Try
    End Sub

    Private Shared Sub OnStatusChangeHandler(ByVal aListstatus As RT_ListStatus, ByVal aSourcestatus As RT_SourceStatus, ByVal aRunmode As RT_RunMode) Handles _listManager.OnStatusChange
        Logger.Info("OnStatusChangeHandler({0}, {1}, {2})", aListstatus.ToString(), aSourcestatus.ToString(), aRunmode.ToString())
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Logger.Debug("Dispose()")
        _listManager.UnregisterAllItems()
        _listManager.CloseAllLinks()
        _listManager = Nothing
    End Sub

    Private Sub ShutdownNow() Handles _shutdownManager.ShutdownNow
        Logger.Warn("Shutdown()")
        If _listManager Is Nothing Then Return
        CancelAll()
        Marshal.ReleaseComObject(_listManager)
        _listManager = Nothing
    End Sub
End Class