Imports YieldMap.Curves
Imports YieldMap.Forms.ChartForm

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

        Public RIC As String
        Public Yield As Double?
        Public SwpCurve As SwapCurve

        Public Overrides Function GetYield() As Double?
            Return Yield
        End Function
        Public Sub New()
        End Sub

        Public Sub New(ByVal elem As SwapPointDescription)
            With elem
                RIC = .RIC
                Me.Yield = .Yield
                Duration = .Duration
                Price = .Price
                PointSpread = .PointSpread
                ZSpread = .ZSpread
                ASWSpread = .ASWSpread
            End With

        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("{0} {1:P2}:{2:F2}", RIC, Yield / 100, Duration)
        End Function
    End Class

    Public Class BondPointDescription
        Inherits BasePointDescription

        Public ParentBond As VisualizableBond

        Public Yld As New YieldStructure
        Public Convexity As Double
        Public PVBP As Double

        Public YieldSource As YieldSource
        Public Overrides Function GetYield() As Double?
            Return Yld.Yield
        End Function
    End Class

    Public Class DataBaseBondDescription
        Private ReadOnly _ric As String
        Private ReadOnly _shortName As String
        Private ReadOnly _label As String

        Private ReadOnly _maturity As Date
        Private ReadOnly _coupon As Double

        Private ReadOnly _paymentStructure As String
        Private ReadOnly _paymentStream As BondPayments
        Private ReadOnly _rateStructure As String
        Private ReadOnly _issueDate As Date

        Sub New(ByVal ric As String, ByVal shortName As String, ByVal label As String, ByVal maturity As Date, ByVal coupon As Double, ByVal paymentStructure As String, ByVal rateStructure As String, ByVal issueDate As Date)
            _ric = ric
            _shortName = shortName
            _label = label
            _maturity = maturity
            _coupon = coupon
            _paymentStructure = paymentStructure
            _rateStructure = rateStructure
            _issueDate = issueDate
            _paymentStream = New BondPayments(_issueDate, _maturity, _paymentStructure, _coupon)
        End Sub

        Public ReadOnly Property RIC As String
            Get
                Return _ric
            End Get
        End Property

        Public ReadOnly Property ShortName As String
            Get
                Return _shortName
            End Get
        End Property

        Public ReadOnly Property Label As String
            Get
                Return _label
            End Get
        End Property

        Public ReadOnly Property Maturity As Date
            Get
                Return _maturity
            End Get
        End Property

        Public ReadOnly Property Coupon As Double
            Get
                Return _coupon
            End Get
        End Property

        Public ReadOnly Property PaymentStructure As String
            Get
                Return _paymentStructure
            End Get
        End Property

        Public ReadOnly Property PaymentStream As BondPayments
            Get
                Return _paymentStream
            End Get
        End Property

        Public ReadOnly Property RateStructure As String
            Get
                Return _rateStructure
            End Get
        End Property

        Public ReadOnly Property IssueDate As Date
            Get
                Return _issueDate
            End Get
        End Property
    End Class
End Namespace