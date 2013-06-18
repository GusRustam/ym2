Namespace Tools.Elements
    Public Class SwapCurveSnapshotElement
        Inherits CurveSnapshotElement

        Public ReadOnly Property RIC() As String
            Get
                Return TheRIC
            End Get
        End Property

        Public ReadOnly Property Duration() As String
            Get
                Return String.Format("{0:F2}", Dur)
            End Get
        End Property

        Public Sub New(ByVal ric As String, ByVal duration As Double, yld As Double)
            MyBase.New(ric, duration, yld)
        End Sub
    End Class
End NameSpace