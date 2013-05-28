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

        Private _ySource As YSource = YSource.Yield
        Public Property YSource() As YSource
            Get
                Return _ySource
            End Get
            Set(ByVal value As YSource)
                _ySource = value
                Recalculate()
            End Set
        End Property

        Private WithEvents _items As New GroupContainer
        Public ReadOnly Property Items As GroupContainer
            Get
                Return _items
            End Get
        End Property

        Private WithEvents _swapCurves As New SwapCurveContainer
        Public ReadOnly Property SwapCurves() As SwapCurveContainer
            Get
                Return _swapCurves
            End Get
        End Property

        Default Public ReadOnly Property Data(ByVal id As Long) As Group
            Get
                Return _items(id)
            End Get
        End Property

        ' todo develop a ChangeableContainer
        Public ReadOnly Property Benchmarks() As Dictionary(Of YSource, IChangeable)
            Get
                ' todo
                Return New Dictionary(Of YSource, IChangeable)()
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
    End Class
End Namespace