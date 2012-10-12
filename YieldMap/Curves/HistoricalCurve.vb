Imports NLog
Imports YieldMap.Tools.History

Namespace Curves
    Class HistoricalCurvesContainer
        Private Shared ReadOnly Logger As Logger = Commons.GetLogger(GetType(HistoricalCurvesContainer))
        Public Event CurveRemoved As Action(Of String)

        Private ReadOnly _rics As New HashSet(Of String)
        Private ReadOnly _newDataHandler As HistoryLoadManager.NewDataDelegate
        Private _hst As HistoryLoadManager
        Private ReadOnly _removeEventHandler As Action(Of String)

        Public Sub New(ByVal newDataHandler As HistoryLoadManager.NewDataDelegate, _
                       ByVal removeEventHandler As Action(Of String))
            _removeEventHandler = removeEventHandler
            AddHandler CurveRemoved, removeEventHandler
            _newDataHandler = newDataHandler
        End Sub

        Function HasRIC(ByVal ric As String) As Boolean
            Return _rics.Contains(ric)
        End Function

        Sub RemoveCurve(ByVal name As String)
            Logger.Info("Will remove curve {0}", name)
            _rics.Remove(name)
            RaiseEvent CurveRemoved(name)
        End Sub

        Sub AddCurve(ByVal ric As String, ByVal taskDescr As HistoryTaskDescr)
            _hst = New HistoryLoadManager(taskDescr, _newDataHandler)
            If _hst.Success Then
                _rics.Add(ric)
            End If
        End Sub

        Public Sub Clear()
            While _rics.Any()
                RemoveCurve(_rics.First())
            End While
            RemoveHandler CurveRemoved, _removeEventHandler
        End Sub
    End Class
End Namespace