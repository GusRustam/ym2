Imports AdfinXAnalyticsFunctions
Imports NLog

Namespace Tools
    Public Class BondPayments
        Private Shared ReadOnly Logger As Logger = Commons.GetLogger(GetType(BondPayments))
        Private ReadOnly _adFinBond As New AdxBondModule
        Private ReadOnly _adFinCommon As New AdxDateModule
        
        Private ReadOnly _issueDate As DateTime

        Public ReadOnly Property IssueDate() As Date
            Get
                Return _issueDate
            End Get
        End Property

        Public ReadOnly Property MaturityDate() As Date
            Get
                Return _maturityDate
            End Get
        End Property

        Public ReadOnly Property Cpns() As List(Of Double)
            Get
                Return _cpns
            End Get
        End Property

        Public ReadOnly Property Redempts() As List(Of Double)
            Get
                Return _redempts
            End Get
        End Property

        Public ReadOnly Property CpnPmts() As List(Of Double)
            Get
                Return _cpnPmts
            End Get
        End Property

        Public ReadOnly Property FaceValue() As List(Of Double)
            Get
                Return _faceValue
            End Get
        End Property

        Public ReadOnly Property Dates() As LinkedList(Of Date)
            Get
                Return _dates
            End Get
        End Property

        Private ReadOnly _maturityDate As DateTime
        
        Private ReadOnly _dates As New LinkedList(Of DateTime)
        Private ReadOnly _faceValue As New List(Of Double)

        Private ReadOnly _cpnPmts As New List(Of Double)
        Private ReadOnly _redempts As New List(Of Double)
        Private ReadOnly _cpns As New List(Of Double)

        Public Sub New(ByVal issueDate As Date, ByVal maturityDate As Date, ByVal bondStructure As String, coupon As Double)
            _issueDate = issueDate
            _maturityDate = maturityDate

            Dim payments As Array = _adFinBond.BdCashflows(issueDate, maturityDate, coupon / 100.0, bondStructure, "RET:A100 IAC")

            _dates.AddLast(issueDate)
            _faceValue.Add(1)

            _cpnPmts.Add(0)
            _redempts.Add(0)

            For i = payments.GetLowerBound(0) To payments.GetUpperBound(0)
                If payments.GetValue(i, 1) Is Nothing Then Exit For

                _dates.AddLast(Commons.FromExcelSerialDate(CInt(payments.GetValue(i, 1))))
                _cpnPmts.Add(CSng(payments.GetValue(i, 2)))
                _redempts.Add(CSng(payments.GetValue(i, 3)))

                Dim prevFaceValue = _faceValue.Last
                _faceValue.Add(prevFaceValue - _redempts.Last)

                Dim daysCount As Integer = _adFinCommon.DfCountDays(_dates.Last.Previous.Value, _dates.Last.Value, "DCB:AA DMC:F")
                _cpns.Add(_cpnPmts.Last / (prevFaceValue * daysCount / 365))
            Next
            _cpns.Add(0)
        End Sub

        Public Sub New(ByVal paymentStream As BondPayments)
            With paymentStream
                _issueDate = .IssueDate
                _maturityDate = .MaturityDate
                _dates = New LinkedList(Of DateTime)(.Dates)
                _faceValue = New List(Of Double)(.FaceValue)
                _cpnPmts = New List(Of Double)(.CpnPmts)
                _redempts = New List(Of Double)(.Redempts)
                _cpns = New List(Of Double)(.Cpns)
            End With
        End Sub

        Public Function GetCouponByDate(calcDate As DateTime) As Double
            If calcDate >= _issueDate And calcDate < _maturityDate Then
                Dim i As Integer
                For i = 0 To _dates.Count - 1
                    If (calcDate >= _dates(i) And calcDate < _dates(i + 1)) Then Exit For
                Next
                Logger.Trace("Coupon rate is {0:F6}", _cpns(i))
                Return _cpns(i)
            Else
                Throw New InvalidOperationException(
                    String.Format("Calculation date {0:dd-MM-yyyy} must have been between issue date {1:dd-MM-yyyy} and maturity date {2:dd-MM-yyyy}",
                                  calcDate, _issueDate, _maturityDate))
            End If
        End Function
        
        Public Overrides Function ToString() As String
            Dim res As String = "Payments are:" + Environment.NewLine
            for i = 0 To _dates.Count-1
                res += String.Format("{0:dd-MM-yy} | {1:F4} | {2:P} | {3:F4} | {4:F4}" + Environment.NewLine, _dates(i), _cpnPmts(i), _cpns(i), _redempts(i), _faceValue(i))
            Next
            Return res
        End Function
    End Class
End NameSpace