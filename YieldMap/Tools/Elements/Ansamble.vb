Namespace Tools.Elements
    Public Class Ansamble
        Private Shared ReadOnly Identities As New HashSet(Of Long)

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

        Private _chartSpreadType As SpreadType = SpreadType.Yield
        Public Property ChartSpreadType() As SpreadType
            Get
                Return _chartSpreadType
            End Get
            Set(ByVal value As SpreadType)
                _chartSpreadType = value
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
            ' todo something might have changed, I dunno what
        End Sub

        Private Sub _curves_Cleared(ByVal obj As BondCurve) Handles _curves.Cleared
            RaiseEvent CurveCleared(obj)
        End Sub

        Public Event CurveCleared As Action(Of BondCurve)
        Public Event CurveQuote As Action(Of BaseGroup)
        Public Event GroupCleared As Action(Of Group)
        Public Event BondQuote As Action(Of Bond)
        Public Event BondRemoved As Action(Of Group, String)
        Public Event BondVolume As Action(Of Bond)

        Private Sub _curves_Quote(ByVal obj As Bond) Handles _curves.Quote
            RaiseEvent CurveQuote(obj.Parent)
        End Sub

        Private Sub _groups_Cleared(ByVal obj As Group) Handles _groups.Cleared
            RaiseEvent GroupCleared(obj)
        End Sub

        Private Sub _groups_Quote(ByVal obj As Bond) Handles _groups.Quote
            RaiseEvent BondQuote(obj)
        End Sub

        Private Sub _groups_RemovedItem(ByVal arg1 As Group, ByVal arg2 As String) Handles _groups.RemovedItem
            RaiseEvent BondRemoved(arg1, arg2)
        End Sub

        Private Sub _groups_Volume(ByVal obj As Bond) Handles _groups.Volume
            RaiseEvent BondVolume(obj)
        End Sub
    End Class
End NameSpace