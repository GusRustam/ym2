Imports AdfinXRtLib
Imports System
Imports NLog
Imports YieldMap.Commons

Namespace Tools.Lists
    Public Class ListTaskDescr
        Public Name As String
        Public Items As List(Of String)
        Public Fields As List(Of String)

        Public Function GetItemArray() As Object()
            Dim itemList() As Object
            ReDim itemList(0 To Items.Count - 1)
            For i = 0 To Items.Count - 1
                itemList(i) = Items(i)
            Next
            Return itemList
        End Function

        Public Function GetFieldArray() As Object()
            Dim fieldList() As Object
            ReDim fieldList(0 To Fields.Count - 1)
            For i = 0 To Fields.Count - 1
                fieldList(i) = Fields(i)
            Next
            Return fieldList
        End Function
    End Class

    Public Class ListLoadManager
        Implements IDisposable

        Private Shared ReadOnly Logger As Logger = GetLogger(GetType(ListLoadManager))
        Private WithEvents _listManager As New AdxRtList

        Private ReadOnly _activeTasks As New Dictionary(Of String, ListTaskDescr)

        Public Event OnNewData As Action(Of Dictionary(Of String, Dictionary(Of String, Dictionary(Of String, Double?))))
        Public Sub DiscardTask(ByVal taskName As String)
            Logger.Debug("DiscardTask({0})", taskName)
            If _activeTasks.ContainsKey(taskName) Then
                Dim descr As ListTaskDescr = _activeTasks(taskName)
                _listManager.UnregisterItems(descr.GetItemArray())
                _activeTasks.Remove(taskName)
            End If
        End Sub

        Public Function StartNewTask(descr As ListTaskDescr) As Boolean
            Logger.Debug("StartNewTask({0})", descr.Name)
            Try
                If Not _activeTasks.ContainsKey(descr.Name) Then
                    _activeTasks.Add(descr.Name, descr)
                    _listManager.RegisterItems(descr.GetItemArray(), descr.GetFieldArray())
                End If
                Return True
            Catch ex As Exception
                Logger.WarnException("Failed to start loading items", ex)
                Return False
            End Try
        End Function

        Public Sub New()
            Logger.Debug("New()")
            With _listManager
                .Source = "IDN"
                .DebugLevel = RT_DebugLevel.RT_DEBUG_IMMEDIATE
                .Mode = String.Format("FRQ:{0}s", RefreshInterval)
                .StartUpdates(RT_RunMode.RT_MODE_ONTIME_IF_UPDATED)
            End With
        End Sub

        Private Shared Sub InsertIntoTRFV(ByRef trfv As Dictionary(Of String, Dictionary(Of String, Dictionary(Of String, Double?))), ByVal task As String, ByVal ric As String, ByVal field As String, ByVal value As Double?)
            If Not trfv.ContainsKey(Task) Then trfv.Add(Task, New Dictionary(Of String, Dictionary(Of String, Double?)))
            Dim rfv As Dictionary(Of String, Dictionary(Of String, Double?)) = trfv(Task)
            If Not rfv.ContainsKey(Ric) Then rfv.Add(Ric, New Dictionary(Of String, Double?))
            Dim fv As Dictionary(Of String, Double?) = rfv(ric)
            If Not fv.ContainsKey(field) Then fv.Add(field, value)
        End Sub

        Private Sub OnTimeHandler() Handles _listManager.OnTime
            Logger.Trace("OnTimeHandler()")
            Dim taskRicFieldValue As New Dictionary(Of String, Dictionary(Of String, Dictionary(Of String, Double?))) 'Task -> RIC -> Field -> Value

            Try
                Dim items As Array = _listManager.ListItems(RT_ItemRowView.RT_IRV_UPDATED, RT_ItemColumnView.RT_ICV_STATUS)
                For k = items.GetLowerBound(0) To items.GetUpperBound(0)
                    Try
                        Dim aItemName As String = items.GetValue(k, 0)
                        Dim relevantTasks = _activeTasks.Where(Function(item) item.Value.Items.Contains(aItemName)).Select(Function(item) item.Key).Distinct.ToList()

                        Dim fields As Array = _listManager.ListFields(aItemName, RT_FieldRowView.RT_FRV_UPDATED, RT_FieldColumnView.RT_FCV_VALUE)
                        For Each task As String In relevantTasks
                            For i = fields.GetLowerBound(0) To fields.GetUpperBound(0)
                                Dim val = fields.GetValue(i, 1)
                                If IsNumeric(val) Then
                                    InsertIntoTRFV(taskRicFieldValue, task, aItemName, fields.GetValue(i, 0), CDbl(val))
                                Else
                                    InsertIntoTRFV(taskRicFieldValue, task, aItemName, fields.GetValue(i, 0), Nothing)
                                End If
                            Next i
                        Next
                    Catch invOpEx As InvalidOperationException
                        Logger.Trace("rank is {0}", items.Rank)
                        Logger.ErrorException("Invalid operation", invOpEx)
                    Catch ex As Exception
                        Logger.ErrorException("Exception in updates handler", ex)
                        Logger.Error("Exception = {0}", ex.ToString())
                    End Try
                Next
                RaiseEvent OnNewData(taskRicFieldValue)
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