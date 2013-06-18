Imports System.ComponentModel

Namespace Tools.Elements
    Public MustInherit Class CurveSnapshotElement
        Implements ICurveSnapshotElement
        Private ReadOnly _ric As String
        Private ReadOnly _duration As Double
        Private ReadOnly _yield As Double

        Public Function CompareTo(ByVal other As ICurveSnapshotElement) As Integer Implements IComparable(Of ICurveSnapshotElement).CompareTo
            Return _duration.CompareTo(other.Dur)
        End Function

        <Browsable(False)>
        Protected ReadOnly Property TheRIC() As String Implements ICurveSnapshotElement.RIC
            Get
                Return _ric
            End Get
        End Property

        <Browsable(False)>
        Protected ReadOnly Property Dur() As Double Implements ICurveSnapshotElement.Dur
            Get
                Return _duration
            End Get
        End Property

        <Browsable(False)>
        Protected ReadOnly Property Yld() As Double Implements ICurveSnapshotElement.Yld
            Get
                Return _yield
            End Get
        End Property

        Public Sub New(ByVal ric As String, ByVal duration As Double, ByVal [yield] As Double)
            _yield = yield
            _ric = ric
            _duration = duration
        End Sub
    End Class
End NameSpace