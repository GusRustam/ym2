Imports System.Diagnostics.Contracts

Namespace Bonds

    Public Class BondPayments
        Private _issueDate As Date
        Private _matDate As Date?
        Private _payments As List(Of Tuple(Of Date, Double))

        Public Function GetCoupon(ByVal dt As Date) As Double
            Contract.Requires(dt < Date.Today)
            Return 0 ' todo wrong!
        End Function
    End Class

    Public Class BondDescription
        Private ReadOnly _ric As String
        Private ReadOnly _shortName As String
        Private ReadOnly _label As String

        Private ReadOnly _maturity As Date
        Private ReadOnly _coupon As Double

        Private ReadOnly _paymentStructure As String
        Private ReadOnly _rateStructure As String
        Private ReadOnly _issueDate As Date
        Private ReadOnly _label1 As String
        Private ReadOnly _label2 As String
        Private ReadOnly _label3 As String
        Private ReadOnly _label4 As String

        Sub New(ByVal ric As String, ByVal shortName As String, ByVal label As String, ByVal maturity As Date, ByVal coupon As Double, ByVal paymentStructure As String, ByVal rateStructure As String, ByVal issueDate As Date, ByVal label1 As String, ByVal label2 As String, ByVal label3 As String, ByVal label4 As String)
            _ric = ric
            _shortName = shortName
            _label = label
            _maturity = maturity
            _coupon = coupon
            _paymentStructure = paymentStructure
            _rateStructure = rateStructure
            _issueDate = issueDate
            _label1 = label1
            _label2 = label2
            _label3 = label3
            _label4 = label4
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

        Public ReadOnly Property Label1 As String
            Get
                Return _label1
            End Get
        End Property

        Public ReadOnly Property Label2 As String
            Get
                Return _label2
            End Get
        End Property

        Public ReadOnly Property Label3 As String
            Get
                Return _label3
            End Get
        End Property

        Public ReadOnly Property Label4 As String
            Get
                Return _label4
            End Get
        End Property

        Public Function GetCouponByDate(ByVal dt As Date) As Double
            Return BondsData.Instance.GetBondPayments(_ric).GetCoupon(dt)
        End Function
    End Class
End Namespace