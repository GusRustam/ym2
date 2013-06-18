Namespace Tools.Elements
    Public Class CurveSnapshot
        Implements ISnapshot
        Private ReadOnly _ansamble As Ansamble

        Private ReadOnly _spreads As New Dictionary(Of IOrdinate, List(Of PointOfCurve))
        Public ReadOnly Property Spreads() As Dictionary(Of IOrdinate, List(Of PointOfCurve)) Implements ISnapshot.Spreads
            Get
                Return _spreads
            End Get
        End Property

        Private ReadOnly _current As List(Of PointOfCurve)
        Public ReadOnly Property Current() As List(Of PointOfCurve) Implements ISnapshot.Current
            Get
                Return _current
            End Get
        End Property

        Public ReadOnly Property DisabledElements() As List(Of ICurveSnapshotElement) Implements ISnapshot.DisabledElements
            Get
                Return New List(Of ICurveSnapshotElement)()
            End Get
        End Property

        Public ReadOnly Property EnabledElements() As List(Of ICurveSnapshotElement) Implements ISnapshot.EnabledElements
            Get
                Return New List(Of ICurveSnapshotElement)()
            End Get
        End Property

        Public ReadOnly Property Synthetic As Boolean Implements ISnapshot.Synthetic
            Get
                Return False
            End Get
        End Property

        Public Sub New(lastCurve As Dictionary(Of IOrdinate, List(Of PointOfCurve)), ByVal ansamble As Ansamble)
            _ansamble = ansamble
            _current = lastCurve(Yield)
            For Each ord In From q In Ordinate.Spreads
                Where _ansamble.Benchmarks.HasOrd(q) AndAlso lastCurve.ContainsKey(q)
                _spreads(ord) = lastCurve(ord)
                _spreads(ord).Sort()
            Next
        End Sub
    End Class
End NameSpace