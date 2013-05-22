Namespace Tools
    Public MustInherit Class BasePointDescription
        Implements IComparable(Of BasePointDescription)

        Public Duration As Double
        Public Price As Double
        Public YieldAtDate As Date
        Public PointSpread As Double?
        Public ZSpread As Double?
        Public ASWSpread As Double?
        Public OASpread As Double?

        Public MustOverride Function GetYield() As Double?

        Public Function CompareTo(ByVal other As BasePointDescription) As Integer Implements IComparable(Of BasePointDescription).CompareTo
            If other IsNot Nothing Then
                If Duration < other.Duration Then
                    Return -1
                ElseIf Duration > other.Duration Then
                    Return 1
                Else
                    Return 0
                End If
            Else
                Return 0
            End If
        End Function
    End Class

    Public Class SwapPointDescription
        Inherits BasePointDescription

        Public Yield As Double?
        Public Overrides Function GetYield() As Double?
            Return Yield
        End Function

        Private ReadOnly _ric As String
        Public ReadOnly Property RIC As String
            Get
                Return _ric
            End Get
        End Property

        Public Sub New(ByVal ric As String)
            _ric = ric
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("{0} {1:P2}:{2:F2}", RIC, Yield / 100, Duration)
        End Function
    End Class

    Public Class BondPointDescription
        Inherits BasePointDescription

        Public ParentBond As Bond

        Public Yld As New YieldStructure
        Public Convexity As Double
        Public PVBP As Double

        Public BackColor As String
        Public MarkerStyle As String

        Public Overrides Function GetYield() As Double?
            Return Yld.Yield
        End Function
    End Class

    Public Class HistPointDescription
        Inherits BondPointDescription
    End Class

End Namespace