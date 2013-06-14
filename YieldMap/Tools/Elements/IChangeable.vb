Namespace Tools.Elements
    Public Interface IChangeable
        Inherits INamed
        Event Cleared As Action
        Event Updated As Action(Of List(Of CurveItem))
        Event UpdatedSpread As Action(Of List(Of CurveItem), IOrdinate)
        Sub Cleanup()
        Sub Recalculate()
        Sub Recalculate(ByVal ord As IOrdinate)
        Sub Subscribe()
        Sub FreezeEvents()
        Sub UnfreezeEvents()
        Sub UnfreezeEventsQuiet()
        Sub RecalculateTotal()
    End Interface
End NameSpace