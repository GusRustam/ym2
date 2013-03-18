Imports System.ComponentModel

Namespace Bonds

    Public Class PaymentException
        Inherits Exception

        Public Sub New()
        End Sub

        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(ByVal message As String, ByVal innerException As Exception)
            MyBase.New(message, innerException)
        End Sub
    End Class
    Public Class BondPayments
        Private ReadOnly _issueDate As Date
        Private ReadOnly _matDate As Date?
        Private ReadOnly _payments As New LinkedList(Of Tuple(Of Date, Double))

        Sub New(ByVal issueDate As Date, ByVal matDate As Date?)
            _issueDate = issueDate
            _matDate = matDate
        End Sub

        Public Sub AddPayment(ByVal dt As Date, ByVal cpn As Double)
            _payments.AddLast(Tuple.Create(dt, cpn))
        End Sub

        Public Function GetCoupon(ByVal dt As Date) As Double
            If dt < _issueDate Then
                Throw New PaymentException(String.Format("Requested date {0:dd/MMM/yy} is less then issue date {1:dd/MMM/yy}", dt, _issueDate))
            ElseIf dt > _matDate Then
                Throw New PaymentException(String.Format("Requested date {0:dd/MMM/yy} is greater then maturity date {1:dd/MMM/yy}", dt, _matDate))
            End If
            Dim item = _payments.First
            While item.Next IsNot Nothing And item.Value.Item1 < dt
                item = item.Next
            End While
            Return item.Value.Item2 / 100
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

        ' todo user-defined labels, combined from fields and formatted as necessary (???)
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

        <DisplayName("Ric")>
        Public ReadOnly Property RIC As String
            Get
                Return _ric
            End Get
        End Property

        <DisplayName("Name")>
        Public ReadOnly Property ShortName As String
            Get
                Return _shortName
            End Get
        End Property

        <Browsable(False)>
        Public ReadOnly Property Label As String
            Get
                Return _label
            End Get
        End Property

        <DisplayName("Maturity date")>
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

        <Browsable(False)>
        Public ReadOnly Property PaymentStructure As String
            Get
                Return _paymentStructure
            End Get
        End Property

        <Browsable(False)>
        Public ReadOnly Property RateStructure As String
            Get
                Return _rateStructure
            End Get
        End Property

        <DisplayName("Issue date")>
        Public ReadOnly Property IssueDate As Date
            Get
                Return _issueDate
            End Get
        End Property

        <Browsable(False)>
        Public ReadOnly Property Label1 As String
            Get
                Return _label1
            End Get
        End Property

        <Browsable(False)>
        Public ReadOnly Property Label2 As String
            Get
                Return _label2
            End Get
        End Property

        <Browsable(False)>
        Public ReadOnly Property Label3 As String
            Get
                Return _label3
            End Get
        End Property

        <Browsable(False)>
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