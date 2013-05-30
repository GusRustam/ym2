Namespace Tools.Elements
    Public Class Ansamble
        Private Shared ReadOnly Identities As New HashSet(Of Long)
        Public Event GroupCleared As Action(Of Group)
        Public Event SwapCleared As Action(Of SwapCurve)

        Public Shared Sub ReleaseID(ByVal id As Long)
            Identities.Remove(id)
        End Sub

        Public Shared Function GenerateID() As Long
            Dim rnd = New Random()
            Dim num As Integer
            Do
                num = CLng(Math.Round((89.9999 * rnd.NextDouble() + 10) * 10000))
            Loop While Identities.Contains(num)
            Identities.Add(num)
            Return num
        End Function

        Private _xSource As XSource
        Public Property XSource() As XSource
            Get
                Return _xSource
            End Get
            Set(ByVal value As XSource)
                _xSource = value
                Recalculate()
            End Set
        End Property

        Private _ySource As OrdinateBase = Yield
        Public Property YSource() As OrdinateBase
            Get
                Return _ySource
            End Get
            Set(ByVal value As OrdinateBase)
                _ySource = value
                Recalculate()
            End Set
        End Property

        ' todo catch updates by type and do recalculations
        Private WithEvents _items As New GroupContainer
        Public ReadOnly Property Items As GroupContainer
            Get
                Return _items
            End Get
        End Property

        ' todo catch updates by type and do recalculations
        Private WithEvents _swapCurves As New SwapCurveContainer
        Public ReadOnly Property SwapCurves() As SwapCurveContainer
            Get
                Return _swapCurves
            End Get
        End Property

        Default Public ReadOnly Property Data(ByVal id As Long) As IChangeable
            Get
                Return If(_items.Exists(id), Items(id), _swapCurves(id))
            End Get
        End Property

        Private WithEvents _benchmarks As New BenchmarkContainer
        Private _eventsFrozen As Boolean

        Public ReadOnly Property Benchmarks() As BenchmarkContainer
            Get
                Return _benchmarks
            End Get
        End Property

        Public Sub Recalculate()
            For Each item As KeyValuePair(Of Long, BondCurve) In _items
                item.Value.Recalculate()
            Next
            For Each item As SwapCurve In _swapCurves
                item.Recalculate()
            Next
        End Sub

        Private Sub GroupsCleared(ByVal obj As Group) Handles _items.Cleared
            RaiseEvent GroupCleared(obj)
        End Sub

        Private Sub SwapCurveCleared(ByVal obj As SwapCurve) Handles _swapCurves.Cleared
            RaiseEvent SwapCleared(obj)
        End Sub

        Public Sub Cleanup()
            Items.Cleanup()
            SwapCurves.Cleanup()
        End Sub

        Private Sub Benchmarks_ClearBmk(obj As IOrdinate) Handles _benchmarks.ClearBmk
            FreezeEvents()
            For Each item As KeyValuePair(Of Long, Group) In _items
                Dim bondCurve = TryCast(item.Value, BondCurve)
                If bondCurve IsNot Nothing Then bondCurve.ClearSpread(obj)
            Next
            For Each item As SwapCurve In _swapCurves
                item.ClearSpread(obj)
            Next
            UnfreezeEvents()
        End Sub

        Private Sub Benchmarks_NewBmk(obj As IOrdinate) Handles _benchmarks.NewBmk
            FreezeEvents()
            For Each item As KeyValuePair(Of Long, Group) In _items
                Dim bondCurve = TryCast(item.Value, BondCurve)
                If bondCurve IsNot Nothing Then bondCurve.SetSpread(obj)
            Next
            For Each item As SwapCurve In _swapCurves
                item.SetSpread(obj)
            Next
            UnfreezeEvents()
        End Sub

        Private Sub UnfreezeEvents()
            _eventsFrozen = false
            For Each item As KeyValuePair(Of Long, Group) In _items
                item.Value.UnfreezeEvents()
            Next
            For Each item As SwapCurve In _swapCurves
                item.UnfreezeEvents()
            Next
        End Sub

        Private Sub FreezeEvents()
            _eventsFrozen = True
            For Each item As KeyValuePair(Of Long, Group) In _items
                item.Value.FreezeEvents()
            Next
            For Each item As SwapCurve In _swapCurves
                item.FreezeEvents()
            Next
        End Sub
    End Class
End Namespace