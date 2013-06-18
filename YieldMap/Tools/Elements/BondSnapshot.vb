Namespace Tools.Elements
    Public Class BondSnapshot
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

        Private ReadOnly _disabledElements As New List(Of ICurveSnapshotElement)
        Public ReadOnly Property DisabledElements() As List(Of ICurveSnapshotElement) Implements ISnapshot.DisabledElements
            Get
                Return _disabledElements
            End Get
        End Property

        Private ReadOnly _enabledElements As New List(Of ICurveSnapshotElement)
        Public ReadOnly Property EnabledElements() As List(Of ICurveSnapshotElement) Implements ISnapshot.EnabledElements
            Get
                Return _enabledElements
            End Get
        End Property

        Private ReadOnly _synthetic As Boolean
        Public ReadOnly Property Synthetic As Boolean Implements ISnapshot.Synthetic
            Get
                Return _synthetic
            End Get
        End Property

        Public Sub New(ByVal bonds As List(Of Bond), ByVal items As List(Of PointOfCurve), ByVal syntCurve As List(Of SyntheticZcb), ByVal ansamble As Ansamble)
            _ansamble = ansamble
            _synthetic = syntCurve IsNot Nothing
            Dim lst = If(syntCurve IsNot Nothing, syntCurve.Cast(Of Bond).ToList(), bonds)
            For Each bond In lst
                Dim mainQuote = bond.QuotesAndYields.Main
                If mainQuote Is Nothing Then Continue For
                If bond.Enabled Then
                    _enabledElements.Add(New BondCurveSnapshotElement(bond.MetaData.RIC, bond.Label, mainQuote.Yield, mainQuote.Duration, mainQuote.Price, bond.QuotesAndYields.MaxPriorityField, mainQuote.YieldAtDate))
                Else
                    _disabledElements.Add(New BondCurveSnapshotElement(bond.MetaData.RIC, bond.Label, mainQuote.Yield, mainQuote.Duration, mainQuote.Price, bond.QuotesAndYields.MaxPriorityField, mainQuote.YieldAtDate))
                End If
            Next
            _enabledElements.Sort()
            _disabledElements.Sort()
            _current = items

            For Each ord In From q In Ordinate.Spreads Where _ansamble.Benchmarks.HasOrd(q)
                Dim tmp = ord
                _spreads(tmp) = (From bond In lst
                    Let mainQuote = Bond.QuotesAndYields.Main
                    Where mainQuote IsNot Nothing
                    Let vle = tmp.GetValue(mainQuote)
                    Where vle.HasValue
                    Select New JustPoint(mainQuote.Duration, tmp.GetValue(mainQuote), Nothing)).Cast(Of PointOfCurve).ToList()
                _spreads(tmp).Sort()
            Next ord
        End Sub
    End Class
End Namespace