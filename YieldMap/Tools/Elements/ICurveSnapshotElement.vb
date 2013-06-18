Namespace Tools.Elements
    Public Interface ICurveSnapshotElement
        Inherits IComparable(Of ICurveSnapshotElement)
        ReadOnly Property RIC() As String
        ReadOnly Property Dur() As Double
        ReadOnly Property Yld() As Double
    End Interface
End NameSpace