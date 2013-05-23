Namespace Tools.Elements
    Public Class Ansamble
        Private Shared ReadOnly Identities As New HashSet(Of Long)
        Public Event Cleared As Action(Of BaseGroup)

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

        Private WithEvents _groups As New GroupContainer(Of Group)
        Public ReadOnly Property Groups As GroupContainer(Of Group)
            Get
                Return _groups
            End Get
        End Property

        Private WithEvents _curves As New GroupContainer(Of BondCurve)
        Public ReadOnly Property BondCurves As GroupContainer(Of BondCurve)
            Get
                Return _curves
            End Get
        End Property

        Public Sub Recalculate()
            For Each item As KeyValuePair(Of Long, BondCurve) In _curves
                item.Value.Recalculate()
            Next
            For Each item As KeyValuePair(Of Long, Group) In _groups
                item.Value.Recalculate()
            Next
        End Sub

        Private Sub GroupsCleared(ByVal obj As BaseGroup) Handles _curves.Cleared, _groups.Cleared
            RaiseEvent Cleared(obj)
        End Sub
    End Class
End NameSpace