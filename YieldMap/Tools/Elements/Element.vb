Imports AdfinXAnalyticsFunctions
Imports DbManager.Bonds
Imports System.Text.RegularExpressions
Imports NLog
Imports ReutersData
Imports Settings
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

        Public MustOverride Property Duration As Double
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

        Public Overrides Property Duration() As Double
            Get
                Return _duration
            End Get
            Set(value As Double)
                _duration = value
            End Set
        End Property

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
        Private _duration As Double

        Public ReadOnly Property Ric As String
            Get
                Return _ric
            End Get
        End Property

        Public Sub New(ByVal ric As String)
            _ric = ric
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("{0} {1:P2}:{2:F2}", Ric, _yield / 100, Duration)
        End Function
    End Class

    Public Class YieldContainer
        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(YieldContainer))
        Private ReadOnly _yields As New List(Of YieldStructure)
        Private ReadOnly _best As YieldStructure

        Public ReadOnly Property Yields() As List(Of YieldStructure)
            Get
                Return _yields
            End Get
        End Property

        Public ReadOnly Property Best() As YieldStructure
            Get
                Return _best
            End Get
        End Property

        Public Sub New(ByVal bondYield As Array, ByVal spread As Double)
            For j = bondYield.GetLowerBound(0) To bondYield.GetUpperBound(0)
                Dim yield = CSng(bondYield.GetValue(j, 1))
                Dim itsDate = Utils.FromExcelSerialDate(bondYield.GetValue(j, 2))
                Logger.Trace("Parsing line: {0:P2} {1:dd-MMM-yy} {2} {3}", yield, itsDate, bondYield.GetValue(j, 3).ToString(), bondYield.GetValue(j, 4).ToString())
                Dim toWhat As YieldToWhat = YieldToWhat.Call
                If Not YieldToWhat.TryParse(bondYield.GetValue(j, 4).ToString(), toWhat) Then
                    toWhat = YieldToWhat.Maturity
                End If
                Dim yieldDescr = New YieldStructure With {.Yield = yield + spread, .YieldToDate = itsDate, .ToWhat = toWhat}
                _yields.Add(yieldDescr)
            Next
            _best = _yields.Max
        End Sub


        Public Sub New(ByVal fieldVal As Double?)
            _yields.Add(New YieldStructure With {.Yield = fieldVal, .YieldToDate = Today, .ToWhat = YieldToWhat.Maturity})
            _best = _yields(0)
        End Sub

        Public Sub AddDerivatives(ByVal i As Integer, ByVal bondDeriv As Array)
            ' 1. Price                 - Price of the bond
            ' 2. Option Free Price     - Option free price of the bond
            ' 3. Volatility            - Modified Duration of the bond (see Modified Duration), or its price sensitivity to movements in yield. 
            '                            This is only a measure of fixed income sensitivity, and should not be confused with volatility in the 
            '                            sense of option pricing, or volatility of a random market price.
            ' 4. PVBP                  - Price variation per basis point (see PVBP)
            ' 5. Duration              - Duration (see Duration)
            ' 6. Average(Life)         - Average life (see Average Life)
            ' 7. Convexity             - Convexity (see Convexity)
            ' 8. YTW/YTB date          - Yield to worst/yield to best date
            If bondDeriv IsNot Nothing Then
                _yields(i).ModDuration = bondDeriv.GetValue(1, 3)
                _yields(i).Pvbp = bondDeriv.GetValue(1, 4)
                _yields(i).Duration = bondDeriv.GetValue(1, 5)
                _yields(i).AverageLife = bondDeriv.GetValue(1, 6) 'non-macauley duration
                _yields(i).Convexity = bondDeriv.GetValue(1, 7)
            Else
                _yields(i).ModDuration = 0
                _yields(i).Pvbp = 0
                _yields(i).Duration = 0
                _yields(i).AverageLife = 0
                _yields(i).Convexity = 0
            End If


        End Sub

    End Class

    Public Class BondPointDescription
        Inherits BasePointDescription
        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(BondPointDescription))
        Private ReadOnly _bondModule As AdxBondModule = Eikon.Sdk.CreateAdxBondModule()

        Private ReadOnly _quoteName As String

        Public BackColor As String
        Public MarkerStyle As String
        Public ParentBond As Bond

        Private _yields As YieldContainer

        Public ReadOnly Property Yld As YieldStructure
            Get
                Return _yields.Best
            End Get
        End Property

        Public Overrides Property Duration() As Double
            Get
                Return _yields.Best.Duration
            End Get
            Set(value As Double)
                ' todo  do nothing
            End Set
        End Property


        Public ReadOnly Property Convexity As Double
            Get
                Return Yld.Convexity
            End Get
        End Property

        Public ReadOnly Property Pvbp As Double
            Get
                Return Yld.Pvbp
            End Get
        End Property

        Public ReadOnly Property AverageLife() As Double
            Get
                Return Yld.AverageLife
            End Get
        End Property

        Public ReadOnly Property ModDuration() As Double
            Get
                Return Yld.ModDuration
            End Get
        End Property

        Sub New(ByVal quoteName As String)
            _quoteName = quoteName
        End Sub

        Private Sub CalculateYields(ByVal prc As Double)
            Price = prc
            Dim dscr = ParentBond.MetaData
            Logger.Trace("CalculateYields({0}, {1})", Price, dscr.RIC)

            Dim coupon = ParentBond.Coupon(YieldAtDate)
            Dim settleDate = _bondModule.BdSettle(YieldAtDate, dscr.PaymentStructure)
            Logger.Trace("Coupon: {0}, settleDate: {1}, maturity: {2}", coupon, Utils.FromExcelSerialDate(settleDate), dscr.Maturity)

            Dim yieldCalcMode = ParentBond.YieldMode
            Dim rateStructure = If(Not yieldCalcMode.Belongs("Default", ""), Regex.Replace(dscr.RateStructure, "YT[A-Z]", yieldCalcMode), dscr.RateStructure)
            Dim bondYield As Array = _bondModule.AdBondYield(settleDate, Price / 100, dscr.Maturity, coupon, dscr.PaymentStructure, rateStructure, "")

            'Dim bondYield As Array = _bondModule.AdBondYield(settleDate, Price / 100, dscr.Maturity, coupon, dscr.PaymentStructure, Regex.Replace(rateStructure, "YT[A-Z]", SettingsManager.Instance.YieldCalcMode), "")
            _yields = New YieldContainer(bondYield, ParentBond.UserDefinedSpread(Ordinate.Yield))

            For i = 0 To _yields.Yields.Count() - 1
                Try
                    Dim bondDeriv As Array = _bondModule.AdBondDeriv(settleDate, _yields.Yields(i).Yield, dscr.Maturity, coupon, 0, dscr.PaymentStructure,
                                                 Regex.Replace(dscr.RateStructure, "YT[A-Z]", _yields.Yields(i).ToWhat.Abbr),
                                                 "", "")
                    _yields.AddDerivatives(i, bondDeriv)
                Catch ex As Exception
                    _yields.AddDerivatives(i, Nothing)
                End Try
            Next
        End Sub

        Public Overrides Sub ClearYield()
        End Sub

        Public Overrides Property Yield(Optional ByVal dt As Date? = Nothing) As Double?
            Get
                Return Yld.Yield
            End Get
            Set(value As Double?)
                Logger.Trace("Yield({0}, {1:ddMMyy}, {2})", ParentBond.MetaData.Ric, dt, value)
                If dt.HasValue Then YieldAtDate = dt.Value
                CalculateYields(value)
            End Set
        End Property

        Public ReadOnly Property QuoteName As String
            Get
                Return _quoteName
            End Get
        End Property

        'Public Sub SetYield(ByVal fieldVal As Double?, ByVal dur As Double)
        '    _yields = New YieldContainer(fieldVal)
        '    _yields.Yields(0).Duration = dur
        '    _yields.Yields(0).AverageLife = dur / (1 + fieldVal)
        'End Sub
    End Class
End Namespace