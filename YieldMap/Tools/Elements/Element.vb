Imports AdfinXAnalyticsFunctions
Imports DbManager.Bonds
Imports System.Text.RegularExpressions
Imports NLog
Imports ReutersData
Imports Uitls

Namespace Tools.Elements
    ''' <summary>
    ''' Source of chart X axis
    ''' </summary>
    Public Enum XSource
        Duration
        Maturity
    End Enum

    ''' <summary>
    ''' Points labeling mode
    ''' </summary>
    ''' <remarks>have to create custom labels</remarks>
    Public Enum LabelMode
        IssuerAndSeries
        IssuerCpnMat
        Description
        SeriesOnly
    End Enum

    ''' <summary>
    ''' This class stores information on which point was last MouseOvered on chart.
    ''' It is used as a tag for series on chart and has an event to notify which point is currently on selection
    ''' </summary>
    ''' <remarks></remarks>
    Friend Class BondSetSeries
        Public Name As String
        Public Event SelectedPointChanged As Action(Of String, Integer?)

        Public Color As Color
        Private _selectedPointIndex As Integer?

        Public Property SelectedPointIndex As Integer?
            Get
                Return _selectedPointIndex
            End Get
            Set(ByVal value As Integer?)
                _selectedPointIndex = value
                RaiseEvent SelectedPointChanged(Name, value)
            End Set
        End Property

        Public Sub ResetSelection()
            _selectedPointIndex = Nothing
        End Sub
    End Class

    ''' <summary>
    ''' Used as a tag for history point. 
    ''' Contains short info on bond for which the history was loaded and id of line on chart
    ''' todo add reference to base bond instead of Ric, Descr and Meta
    ''' todo kill history when bond is unloaded or hidden (this means I have to catch bond events)
    ''' </summary>
    ''' <remarks></remarks>
    Friend Structure HistoryPointTag
        Public Ric As String
        Public Descr As BondPointDescription
        Public Meta As BondMetadata
        Public SeriesId As Guid
    End Structure

    ''' <summary>
    ''' Contains calculated point parameters
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class BasePointDescription
        Implements IComparable(Of BasePointDescription)

        Public Duration As Double
        Public Price As Double
        Public YieldAtDate As Date
        Public PointSpread As Double?
        Public ZSpread As Double?
        Public ASWSpread As Double?
        Public OASpread As Double?

        Public MustOverride Sub ClearYield()
        Public MustOverride Property Yield(Optional ByVal dt As Date? = Nothing) As Double?

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

        Private _yield As Double?

        Public Overrides Sub ClearYield()
            _yield = Nothing
        End Sub

        Public Overrides Property Yield(Optional ByVal dt As Date? = Nothing) As Double?
            Get
                Return _yield
            End Get
            Set(value As Double?)
                If dt.HasValue Then YieldAtDate = dt.Value
                _yield = value
            End Set
        End Property

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
            Return String.Format("{0} {1:P2}:{2:F2}", RIC, _yield / 100, Duration)
        End Function
    End Class

    Public Class BondPointDescription
        Inherits BasePointDescription
        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(BondPointDescription))
        Private ReadOnly _bondModule As AdxBondModule = Eikon.Sdk.CreateAdxBondModule()

        Public ParentBond As Bond

        Public Yld As New YieldStructure
        Public Convexity As Double
        Public PVBP As Double

        Public BackColor As String
        Public MarkerStyle As String

        Private Shared Function ParseBondYield(ByVal bondYield As Array) As List(Of YieldStructure)
            Dim res As New List(Of YieldStructure)

            For j = bondYield.GetLowerBound(0) To bondYield.GetUpperBound(0)
                Dim yield = CSng(bondYield.GetValue(j, 1))
                Dim itsDate = Utils.FromExcelSerialDate(bondYield.GetValue(j, 2))
                Logger.Trace("Parsing line: {0:P2} {1:dd-MMM-yy} {2} {3}", yield, itsDate, bondYield.GetValue(j, 3).ToString(), bondYield.GetValue(j, 4).ToString())
                Dim toWhat As YieldToWhat
                If Not YieldToWhat.TryParse(bondYield.GetValue(j, 4).ToString(), toWhat) Then
                    toWhat = YieldToWhat.Maturity
                End If
                Dim yieldDescr = New YieldStructure With {.Yield = yield, .YieldToDate = itsDate, .ToWhat = toWhat}
                res.Add(yieldDescr)
            Next
            Return res
        End Function

        Private Sub CalculateYields(ByVal prc As Double)
            Price = prc
            Dim dscr = ParentBond.MetaData
            Logger.Trace("CalculateYields({0}, {1})", Price, dscr.RIC)

            Dim coupon = BondsData.Instance.GetBondPayments(dscr.RIC).GetCoupon(YieldAtDate)
            Dim settleDate = _bondModule.BdSettle(YieldAtDate, dscr.PaymentStructure)
            Logger.Trace("Coupon: {0}, settleDate: {1}, maturity: {2}", coupon, settleDate, dscr.Maturity)
            Dim bondYield As Array = _bondModule.AdBondYield(settleDate, price / 100, dscr.Maturity, coupon, dscr.PaymentStructure, dscr.RateStructure, "")
            Dim bestYield = ParseBondYield(bondYield).Max
            Logger.Trace("best Yield: {0}", bestYield)

            ' todo no best yield! (or else, best yield and other yields too), and todo modified duration

            Dim bondDeriv As Array = _bondModule.AdBondDeriv(settleDate, bestYield.Yield, dscr.Maturity, coupon, 0, dscr.PaymentStructure, Regex.Replace(dscr.RateStructure, "YT[A-Z]", bestYield.ToWhat.Abbr), "", "")

            Duration = bondDeriv.GetValue(1, 5)
            Convexity = bondDeriv.GetValue(1, 7)
            PVBP = bondDeriv.GetValue(1, 4)

            bestYield.Yield += ParentBond.UsedDefinedSpread
            Yld = bestYield
        End Sub

        Public Overrides Sub ClearYield()
            Yld = New YieldStructure()
        End Sub

        Public Overrides Property Yield(Optional ByVal dt As Date? = Nothing) As Double?
            Get
                Return Yld.Yield
            End Get
            Set(value As Double?)
                If dt.HasValue Then YieldAtDate = dt.Value
                CalculateYields(value)
            End Set
        End Property
    End Class
End Namespace