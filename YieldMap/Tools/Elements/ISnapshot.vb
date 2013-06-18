Namespace Tools.Elements
    Public Interface ISnapshot
        ReadOnly Property Spreads() As Dictionary(Of IOrdinate, List(Of PointOfCurve))
        ReadOnly Property Current() As List(Of PointOfCurve)
        ReadOnly Property DisabledElements() As List(Of ICurveSnapshotElement)
        ReadOnly Property EnabledElements() As List(Of ICurveSnapshotElement)
        ReadOnly Property Synthetic As Boolean
    End Interface
End NameSpace