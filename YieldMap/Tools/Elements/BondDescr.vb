Imports System.Windows.Forms
Imports NLog

Namespace Tools.Elements
    Class BondDescrComparer
        Implements IComparer(Of BondDescr)

        Private ReadOnly _memberName As String
        Private ReadOnly _sortOrder As SortOrder = SortOrder.None

        Sub New(ByVal memberName As String, ByVal sortOrder As SortOrder)
            _memberName = memberName
            _sortOrder = sortOrder
        End Sub

        Public Function Compare(ByVal x As BondDescr, ByVal y As BondDescr) As Integer Implements IComparer(Of BondDescr).Compare
            Dim res As Integer
            Dim mult = IIf(_sortOrder = SortOrder.Ascending, 1, -1)
            Dim nameOrder As Integer = String.Compare(x.Name, y.Name, StringComparison.Ordinal)
            Select Case _memberName
                Case "RIC" : res = mult * String.Compare(x.RIC, y.RIC, StringComparison.Ordinal)
                Case "Name" : res = mult * nameOrder
                Case Else
                    Dim srt As Integer
                    Select Case _memberName
                        Case "Maturity" : srt = x.Maturity.CompareTo(y.Maturity)
                        Case "BondYield" : srt = x.BondYield.CompareTo(y.BondYield)
                        Case "Duration" : srt = x.Duration.CompareTo(y.Duration)
                        Case "Convexity" : srt = x.Convexity.CompareTo(y.Convexity)
                        Case "ToWhat" : srt = x.ToWhat.CompareTo(y.ToWhat)
                        Case "Price" : srt = x.Price.CompareTo(y.Price)
                        Case "Convexity" : srt = x.Convexity.CompareTo(y.Convexity)
                        Case "QuoteDate" : srt = x.QuoteDate.CompareTo(y.QuoteDate)
                        Case "Coupon" : srt = x.Coupon.CompareTo(y.Coupon)
                        Case Else : srt = 0
                    End Select
                    res = mult * IIf(srt <> 0, srt, nameOrder)
            End Select
            Return res
        End Function
    End Class

    ''' <summary>
    ''' Currenly used to show bonds in a table
    ''' </summary>
    ''' <remarks></remarks>
    Public Class BondDescr
        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(BondDescr))

        Private _ric As String
        Private _name As String
        Private _maturity As Date
        Private _bondYield As Double
        Private _duration As Double
        Private _convexity As Double
        Private _toWhat As YieldToWhat
        Private _price As Double
        Private _quote As String
        Private _live As Boolean
        Private _quoteDate As Date
        Private _coupon As Double

        Public Sub New()
            _quoteDate = Date.Today
            _live = True
            _toWhat = YieldToWhat.Maturity
        End Sub

        Public Sub New(ByVal ric As String, ByVal name As String, ByVal maturity As Date, ByVal bondYield As Double, ByVal duration As Double, ByVal convxity As Double, ByVal toWhat As YieldToWhat, ByVal price As Double, ByVal quote As String, ByVal live As Boolean, ByVal quoteDate As Date)
            _ric = ric
            _name = name
            _maturity = maturity
            _bondYield = bondYield
            _duration = duration
            _convexity = convxity
            _toWhat = toWhat
            _price = price
            _quote = quote
            _quoteDate = quoteDate
            _live = live
        End Sub

        ' this constructor uses only important data
        Public Sub New(ByVal ric As String, ByVal name As String, ByVal toWhat As YieldToWhat, ByVal quote As String, ByVal live As Boolean, ByVal quoteDate As Date)
            _ric = ric
            _name = name
            _toWhat = toWhat
            _quote = quote
            _quoteDate = quoteDate
            _live = live
        End Sub

        Public Property RIC As String
            Get
                Return _ric
            End Get
            Set(ByVal value As String)
                _ric = value
            End Set
        End Property

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property Maturity As Date
            Get
                Return _maturity
            End Get
            Set(ByVal value As Date)
                _maturity = value
            End Set
        End Property

        Public Property BondYield As Double
            Get
                Return _bondYield
            End Get
            Set(ByVal value As Double)
                _bondYield = value
            End Set
        End Property

        Public Property Duration As Double
            Get
                Return _duration
            End Get
            Set(ByVal value As Double)
                _duration = value
            End Set
        End Property

        Public Property Convexity As Double
            Get
                Return _convexity
            End Get
            Set(ByVal value As Double)
                _convexity = value
            End Set
        End Property

        Public Property ToWhat As YieldToWhat
            Get
                Return _toWhat
            End Get
            Set(ByVal value As YieldToWhat)
                _toWhat = value
            End Set
        End Property

        Public Property Price As Double
            Get
                Return _price
            End Get
            Set(ByVal value As Double)
                _price = value
            End Set
        End Property

        Public Property Quote As String
            Get
                Return _quote
            End Get
            Set(ByVal value As String)
                _quote = value
            End Set
        End Property

        Public Property Live As Boolean
            Get
                Return _live
            End Get
            Set(ByVal value As Boolean)
                _live = value
            End Set
        End Property

        Public Property QuoteDate As Date
            Get
                Return _quoteDate
            End Get
            Set(ByVal value As Date)
                _quoteDate = value
            End Set
        End Property

        Public Property Coupon() As Double
            Get
                Return _coupon
            End Get
            Set(ByVal value As Double)
                _coupon = value
            End Set
        End Property

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If Not TypeOf obj Is BondDescr Then Return False
            Dim bond = CType(obj, BondDescr)
            Return _
                (bond.RIC = RIC Or bond.Name = Name) And
                bond.Live = Live And
                bond.Quote.Equals(Quote) And
                bond.QuoteDate = QuoteDate And
                bond.ToWhat.Equals(ToWhat)
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("RIC:{0}|Name:{1}|Quote:{2}|Date:{3:dd/MM/yy}|ToWhat:{4}", RIC, Name, Quote.ToString(), QuoteDate, ToWhat.ToString())
        End Function

        Public Shared Function FromString(ByVal str As String) As BondDescr
            Logger.Trace("FromString({0})", str)
            Try
                Dim data = str.Split("|")
                If data.Count() <> 7 Then Return Nothing

                Dim ric = data(0).Split(":")(1)
                Dim name = data(1).Split(":")(1)
                Dim live = Boolean.Parse(data(3).Split(":")(1))
                Dim quote = data(4).Split(":")(1)
                Dim quoteDateStr = Date.Parse(data(5).Split(":")(1))
                Dim toWhatStr = YieldToWhat.Parse(data(6).Split(":")(1))
                Dim descr = New BondDescr(ric, name, toWhatStr, quote, live, quoteDateStr)

                Logger.Trace("Bond = {0}", descr.ToString())

                Return descr
            Catch ex As Exception
                Logger.WarnException("Failed parsing bond", ex)
                Logger.Warn("Exception = {0}", ex.ToString())
            End Try
            Return Nothing
        End Function
    End Class
End Namespace