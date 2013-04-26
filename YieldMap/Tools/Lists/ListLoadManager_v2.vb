Imports AdfinXRtLib
Imports System
Imports NLog
Imports ReutersData

Namespace Tools.Lists
    Public Enum ItemInfo
        ExistingItems
        InvalidItems
    End Enum

    Public Enum FieldStatus
        Undefined
        Unknown
        Invalid
    End Enum

    Public Enum ItemStatus
        Stale
        Invalid
        Unknown
        NotPermissioned
        Duplicate
    End Enum

    Public Class WrongItemsInfo
        Public WrongItems As Dictionary(Of ItemInfo, List(Of Tuple(Of String, ItemStatus)))
        Public WrongFields As Dictionary(Of String, List(Of Tuple(Of String, FieldStatus)))
    End Class

    Public Class ListLoadManager_v2
        Implements IDisposable


        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(ListLoadManager_v2))
        Private WithEvents _listManager As AdxRtList = Eikon.SDK.CreateAdxRtList()

        Public Event OnNewData As Action(Of Dictionary(Of String, Dictionary(Of String, Double)), WrongItemsInfo)

        Public Sub CancelAll()
            Try
                _listManager.UnregisterAllItems()
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

        Public Function AddItems(ByVal items As List(Of String), Optional ByVal fields As List(Of String) = Nothing) As WrongItemsInfo
            Dim itms = items.Aggregate(Function(str, elem) str & "," & elem)
            Dim flds = If(fields IsNot Nothing, fields.Aggregate(Function(str, elem) str & "," & elem), "")
            Logger.Debug("StartNewTask({0}, {1})", itms, flds)
            Try
                Dim wrongItems As New Dictionary(Of ItemInfo, List(Of Tuple(Of String, ItemStatus)))
                Dim wrongFields As New Dictionary(Of String, List(Of Tuple(Of String, FieldStatus)))
                For Each item In items
                    If Not _listManager.IsRegisteredItem(item) Then
                        _listManager.RegisterItems(item, flds)
                    Else
                        wrongItems(ItemInfo.ExistingItems).Add(Tuple.Create(item, ItemStatus.Duplicate))
                    End If
                Next
                Return New WrongItemsInfo() With {.WrongFields = wrongFields, .WrongItems = wrongItems}
            Catch ex As Exception
                Logger.WarnException("Failed to start loading items", ex)
                Logger.Warn("Exception = {0}", ex.ToString())
            End Try
            Return Nothing
        End Function

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

        Private Function AddItemStatusInfo(ByRef wrongItems As Dictionary(Of ItemInfo, List(Of Tuple(Of String, ItemStatus))), ByVal name As String, ByVal status As RT_ItemStatus) As Boolean
            Dim badItems = {RT_ItemStatus.RT_ITEM_INVALID, RT_ItemStatus.RT_ITEM_NOT_PERMISSIONED,
                            RT_ItemStatus.RT_ITEM_STALE, RT_ItemStatus.RT_ITEM_UNKNOWN}

            If badItems.Contains(status) Then
                Dim theStatus As ItemStatus
                Select Case status
                    Case RT_ItemStatus.RT_ITEM_INVALID : theStatus = ItemStatus.Invalid
                    Case RT_ItemStatus.RT_ITEM_NOT_PERMISSIONED : theStatus = ItemStatus.Unknown
                    Case RT_ItemStatus.RT_ITEM_STALE : theStatus = ItemStatus.Stale
                    Case RT_ItemStatus.RT_ITEM_UNKNOWN : theStatus = ItemStatus.Unknown
                End Select

                If Not wrongItems.ContainsKey(ItemInfo.InvalidItems) Then
                    wrongItems.Add(ItemInfo.InvalidItems, New List(Of Tuple(Of String, ItemStatus)))
                End If

                wrongItems(ItemInfo.InvalidItems).Add(Tuple.Create(name, theStatus))
                Return True
            End If
            Return False
        End Function

        Private Sub AddFieldStatusInfo(ByRef wrongFields As Dictionary(Of String, List(Of Tuple(Of String, FieldStatus))), ByVal name As String, ByVal field As String, ByVal status As RT_FieldStatus)
            Dim badItems = {RT_FieldStatus.RT_FIELD_INVALID,
                            RT_FieldStatus.RT_FIELD_UNKNOWN,
                            RT_FieldStatus.RT_FIELD_UNDEFINED}

            If badItems.Contains(status) Then
                Dim theStatus As FieldStatus
                Select Case status
                    Case RT_FieldStatus.RT_FIELD_INVALID : theStatus = FieldStatus.Invalid
                    Case RT_FieldStatus.RT_FIELD_UNKNOWN : theStatus = FieldStatus.Unknown
                    Case RT_FieldStatus.RT_FIELD_UNDEFINED : theStatus = FieldStatus.Undefined
                End Select

                If Not wrongFields.ContainsKey(name) Then
                    wrongFields.Add(name, New List(Of Tuple(Of String, FieldStatus)))
                End If

                wrongFields(name).Add(Tuple.Create(field.ToString(), theStatus))
            End If
        End Sub

        Private Sub OnTimeHandler() Handles _listManager.OnTime
            Logger.Trace("OnTimeHandler()")
            Dim taskRicFieldValue As New Dictionary(Of String, Dictionary(Of String, Double)) ' RIC -> Field -> Value
            Dim wrongItems As New Dictionary(Of ItemInfo, List(Of Tuple(Of String, ItemStatus)))
            Dim wrongFields As New Dictionary(Of String, List(Of Tuple(Of String, FieldStatus)))
            Try
                Dim items As Array = _listManager.ListItems(RT_ItemRowView.RT_IRV_UPDATED, RT_ItemColumnView.RT_ICV_STATUS)
                For k = items.GetLowerBound(0) To items.GetUpperBound(0)
                    Try
                        Dim aItemName As String = items.GetValue(k, 0)

                        Dim status = _listManager.ItemStatus(aItemName)
                        If AddItemStatusInfo(wrongItems, aItemName, status) Then
                            '     _listManager.UnregisterItems(aItemName)
                        Else
                            Dim fields As Array = _listManager.ListFields(aItemName, RT_FieldRowView.RT_FRV_UPDATED, RT_FieldColumnView.RT_FCV_VALUE)
                            If fields Is Nothing Then Continue For

                            For i = fields.GetLowerBound(0) To fields.GetUpperBound(0)
                                Dim val = fields.GetValue(i, 1)
                                Dim field = fields.GetValue(i, 0)
                                Try
                                    Dim stts = _listManager.FieldStatus(aItemName, field)
                                    AddFieldStatusInfo(wrongFields, aItemName, field, stts)
                                    If IsNumeric(val) Then
                                        InsertIntoRfv(taskRicFieldValue, aItemName, field, CDbl(val))
                                    End If
                                Catch ex As Exception
                                    AddFieldStatusInfo(wrongFields, aItemName, field, RT_FieldStatus.RT_FIELD_UNKNOWN)
                                End Try
                            Next i
                        End If
                    Catch invOpEx As InvalidOperationException
                        Logger.Trace("rank is {0}", items.Rank)
                        Logger.ErrorException("Invalid operation", invOpEx)
                    Catch ex As Exception
                        Logger.ErrorException("Exception in updates handler", ex)
                        Logger.Error("Exception = {0}", ex.ToString())
                    End Try
                Next
                RaiseEvent OnNewData(taskRicFieldValue, New WrongItemsInfo() With {.WrongFields = wrongFields, .WrongItems = wrongItems})
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
        End Sub
    End Class
End Namespace