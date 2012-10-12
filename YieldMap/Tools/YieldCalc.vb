﻿Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Globalization
Imports AdfinXAnalyticsFunctions
Imports System.ComponentModel
Imports YieldMap.Forms.ChartForm
Imports NLog
Imports System.Text.RegularExpressions

Namespace Tools
    Public Class YieldToWhatConverter
        Inherits TypeConverter
        Public Overrides Function CanConvertFrom(ByVal context As ITypeDescriptorContext, ByVal sourceType As Type) As Boolean
            Return sourceType = GetType(String)
        End Function

        Public Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object) As Object
            If TypeOf value Is String Then
                Return CType(value.ToString(), YieldToWhat)
            End If
            Return Nothing
        End Function
    End Class

    ''' <summary>
    ''' Basic support for different kinds of yield. Java enums would work <b>much</b> better
    ''' </summary>
    ''' <remarks></remarks>
    <TypeConverter(GetType(YieldToWhatConverter))>
    Public Class YieldToWhat
        Implements IComparable(Of YieldToWhat)
        Public Shared Put = New YieldToWhat("Put", "YTP", MarkerStyle.Triangle)
        Public Shared [Call] = New YieldToWhat("Call", "YTC", MarkerStyle.Star5)
        Public Shared Maturity = New YieldToWhat("Maturity", "YTM", MarkerStyle.Circle)

        Private ReadOnly _name As String
        Private ReadOnly _abbr As String
        Private ReadOnly _markerStyle As MarkerStyle

        Private Sub New(ByVal name As String, ByVal abbr As String, ByVal markerStyle As MarkerStyle)
            _name = name
            _abbr = abbr
            _markerStyle = markerStyle
        End Sub

        Public ReadOnly Property Abbr As String
            Get
                Return _abbr
            End Get
        End Property

        Public ReadOnly Property Name As String
            Get
                Return _name
            End Get
        End Property

        Public ReadOnly Property MarkerStyle As MarkerStyle
            Get
                Return _markerStyle
            End Get
        End Property

        Public Function CompareTo(ByVal other As YieldToWhat) As Integer Implements IComparable(Of YieldToWhat).CompareTo
            Return _name.CompareTo(other.Name)
        End Function

        Public Overrides Function ToString() As String
            Return _name
        End Function

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If TypeOf obj Is YieldToWhat Then
                Dim yk = CType(obj, YieldToWhat)
                Return yk.Name = Name
            Else
                Return False
            End If
        End Function

        Public Shared Function TryParse(Of TEnum)(ByVal name As String, ByRef toWhat As TEnum) As Boolean
            'todo i could do it with attributes and reflection
            If name = Put.Name Then
                toWhat = Put
                Return True
            End If
            If name = [Call].Name Then
                toWhat = [Call]
                Return True
            End If
            If name = Maturity.Name Then
                toWhat = Maturity
                Return True
            End If
            Return False
        End Function

        Public Shared Narrowing Operator CType(x As String) As YieldToWhat
            Dim res As YieldToWhat
            If TryParse(x, res) Then
                Return res
            Else
                Return Nothing
            End If
        End Operator

        Public Shared Function GetValues() As Array
            'todo i could do it with attributes and reflection
            Return {Put, [Call], Maturity}
        End Function

        Public Shared Function Parse(ByVal toWhatStr As String) As YieldToWhat
            If toWhatStr = Put.Name Then Return Put
            If toWhatStr = [Call].Name Then Return [Call]
            If toWhatStr = Maturity.Name Then Return Maturity
            Return Nothing
        End Function
    End Class

    ''' <summary>
    ''' Contains information on one yield: it's value, it's kind and date to which it was calculated
    ''' </summary>
    ''' <remarks></remarks>
    Public Class YieldStructure
        Implements IComparable(Of YieldStructure)
        Public ToWhat As YieldToWhat
        Public Yield As Double
        Public YieldDate As DateTime

        Public Function CompareTo(ByVal other As YieldStructure) As Integer Implements IComparable(Of YieldStructure).CompareTo
            Return IIf(Yield < other.Yield, -1, 1)
        End Function

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If TypeOf obj Is YieldStructure Then
                Dim ys = CType(obj, YieldStructure)
                Return ys.ToWhat.Equals(ToWhat) And ys.YieldDate = YieldDate
            Else
                Return False
            End If
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("[{0:P2} @ {1}]", Yield, ToWhat.ToString())
        End Function
    End Class

    ''' <summary>
    ''' Shared yield calc functions
    ''' </summary>
    ''' <remarks></remarks>
    Friend Module YieldCalc
        Private ReadOnly Logger As Logger = Commons.GetLogger(GetType(YieldCalc))
        Private ReadOnly BondModule As New AdxBondModule
        Public Function QuoteDescription(price As Double, yield As Double, duration As Double, yieldToWhat As YieldToWhat) As String
            Return String.Format("P [{0:F4}], Y [{1:P2}] {2}, D [{3:F2}]",
                                 price, yield, yieldToWhat.Abbr, duration)
        End Function

        Public Function ZSpread(ByVal rateArray As Array, descr As BondPointDescr) As Double?
            '    'Dim rateArrayString = ""
            '    'For i = 0 To rateArray.GetUpperBound(0)
            '    '    rateArrayString += String.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0:ddMMMyy} {1:F4} ", rateArray.GetValue(i, 0), rateArray.GetValue(i, 1))
            '    'Next
            '    'Logger.Trace("ZSpread({0}, {1})", info.RIC, rateArrayString)
            If descr.CalcPrice > 0 Then
                Dim settleDate = BondModule.BdSettle(DateTime.Today, descr.PaymentStructure)
                Return BondModule.AdBondSpread(settleDate, rateArray, descr.CalcPrice / 100.0, descr.Maturity, descr.Coupon / 100.0, descr.PaymentStructure, "ZCTYPE:RATE IM:LIX RM:YC", "", "")
            Else
                Return Nothing
            End If
        End Function

        Public Function CalcYield(ByRef price As Double, ByVal dt As DateTime, descr As BondPointDescr) As Tuple(Of Double, Double, YieldStructure)
            Logger.Trace("CalcYield({0}, {1})", price, descr.RIC)

            If descr.PaymentStream Is Nothing Then
                descr.PaymentStream = New BondPayments(descr.IssueDate, descr.Maturity, descr.PaymentStructure, descr.Coupon)
                Logger.Trace("Adding bond payments for {0}: {1}", descr.ShortName, descr.PaymentStream)
            End If

            Dim coupon = descr.PaymentStream.GetCouponByDate(dt)
            Dim settleDate = BondModule.BdSettle(dt, descr.PaymentStructure)
            Logger.Trace("Coupon: {0}, settleDate: {1}, maturity: {2}", coupon, settleDate, descr.Maturity)
            Dim bondYield As Array = BondModule.AdBondYield(settleDate, price / 100, descr.Maturity, coupon, descr.PaymentStructure, descr.RateStructure, "")
            Dim bestYield = ParseBondYield(bondYield).Max
            Logger.Trace("best Yield: {0}", bestYield)

            Dim bondDeriv As Array = BondModule.AdBondDeriv(settleDate, bestYield.Yield, descr.Maturity, coupon, 0, descr.PaymentStructure, Regex.Replace(descr.RateStructure, "YT[A-Z]", bestYield.ToWhat.Abbr), "", "")
#If DEBUG Then
            Logger.Trace(" --- derivatives ---")
            For i = bondDeriv.GetLowerBound(0) To bondDeriv.GetUpperBound(0)
                For j = bondDeriv.GetLowerBound(0) To bondDeriv.GetUpperBound(1)
                    Logger.Trace("({0},{1}) -> {2}", i, j, bondDeriv.GetValue(i, j))
                Next
            Next
#End If
            Dim duration = bondDeriv.GetValue(1, 5)
            Dim convexity = bondDeriv.GetValue(1, 7)
            Logger.Trace("duration: {0}", duration)
            Return New Tuple(Of Double, Double, YieldStructure)(duration, convexity, bestYield)
        End Function

        Private Function ParseBondYield(ByVal bondYield As Array) As List(Of YieldStructure)
            Dim res As New List(Of YieldStructure)

            For j = bondYield.GetLowerBound(0) To bondYield.GetUpperBound(0)
                Dim yield = CSng(bondYield.GetValue(j, 1))
                Dim itsDate = Commons.FromExcelSerialDate(bondYield.GetValue(j, 2))
                Logger.Trace("Parsing line: {0:P2} {1:dd-MMM-yy} {2} {3}", yield, itsDate, bondYield.GetValue(j, 3).ToString(), bondYield.GetValue(j, 4).ToString())
                Dim toWhat As YieldToWhat
                If Not YieldToWhat.TryParse(bondYield.GetValue(j, 4).ToString(), toWhat) Then
                    toWhat = YieldToWhat.Maturity
                End If
                Dim yieldDescr = New YieldStructure With {.Yield = yield, .YieldDate = itsDate, .ToWhat = toWhat}
                res.Add(yieldDescr)
            Next
            Return res
        End Function
    End Module
End Namespace