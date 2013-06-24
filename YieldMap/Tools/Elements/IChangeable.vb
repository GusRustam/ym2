Namespace Tools.Elements
    Public Interface IChangeable
        Inherits INamed
        Event Cleared As Action
        Event Updated As Action(Of List(Of PointOfCurve))
        Event UpdatedSpread As Action(Of List(Of PointOfCurve), IOrdinate)
        ReadOnly Property DisabledElements() As List(Of Bond)
        Property GroupDate() As Date
        Sub Cleanup()
        Sub Recalculate()
        Sub Recalculate(ByVal ord As IOrdinate)
        Sub Subscribe()
        Sub FreezeEvents()
        Sub UnfreezeEvents()
        Sub UnfreezeEventsQuiet()
        Sub RecalculateTotal()
        Sub Disable(ByVal ric As String)
        Sub Disable(ByVal rics As List(Of String))
        Sub Enable(ByVal rics As List(Of String))
    End Interface
End Namespace