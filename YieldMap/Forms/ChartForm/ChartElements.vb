Imports System.Drawing
Imports System.Reflection
Imports YieldMap.Commons
Imports YieldMap.Tools
Imports YieldMap.Curves

Namespace Forms.ChartForm
#Region "I. Enumerations"
    Friend Enum YieldSource
        Realtime
        Historical
        Synthetic
    End Enum

    Friend Enum QuoteSource
        Bid
        Ask
        Last
        Hist
    End Enum

    Friend Enum GroupType
        Chain
        List
        Bond
    End Enum

    Friend Class TaskDescription
        Public Name As String
        Public Field As String
        Public RICs As New List(Of String)
    End Class
#End Region

#Region "II. Groups and ansamble"
    Friend Class VisualizableAnsamble
        Public Groups As New List(Of VisualizableGroup)

        Public Function GetAllRICs() As List(Of String)
            Dim list = Groups.Select(Function(group) group.Elements.Keys.ToList()).SelectMany(Function(ric) ric).ToList()
            Return list
        End Function

        Public Function PrepareTasks() As List(Of TaskDescription)
            Dim result As New List(Of TaskDescription)
            Groups.ForEach(
                Sub(group)
                    result.AddRange(From elem In group.GetListByFields()
                                    Select New TaskDescription With {
                                        .Name = String.Format("{0} ({1})", group.Name, elem.Key),
                                        .RICs = elem.Value,
                                        .Field = elem.Key
                                    })
                End Sub)
            Return result
        End Function

        ''' <summary>
        ''' Get a group containing specified bond 
        ''' </summary>
        ''' <param name="instrument">RIC of bond</param>
        ''' <returns>FIRST group which contains specified element</returns>
        ''' <remarks>There might be several groups which contain that element, 
        ''' but they are arranged according to VisualizableGroup sorting rules</remarks>
        Public Function GetInstrumentGroup(ByVal instrument As String) As VisualizableGroup
            Dim grp = Groups.Where(Function(group) group.Elements.Any(Function(elem) elem.Key = instrument)).ToList()
            grp.Sort()
            Return grp.First
        End Function

        Public Function GetSeriesName(ByVal instrument As String) As String
            Return GetInstrumentGroup(instrument).Name
        End Function

        Public Function GetBondDescription(ByVal instrument As String) As BondPointDescr
            Return GetInstrumentGroup(instrument).Elements(instrument)
        End Function

        Public Function GetColor(ByVal instrument As String) As Color
            Return Color.FromName(GetInstrumentGroup(instrument).Color)
        End Function

        Public Sub Cleanup()
            Groups.ForEach(Sub(group) group.Cleanup())
            Groups.Clear()
        End Sub

        Public Function GetBondDescription(ByVal seriesName As String, ByVal instrument As String) As BondPointDescr
            Return Groups.First(Function(grp) grp.Name = seriesName).Elements.First(Function(elem) elem.Key = instrument).Value
        End Function

        Public Function ContainsRIC(ByVal instrument As String) As Boolean
            Return Groups.Where(Function(group) group.Elements.Any(Function(elem) elem.Key = instrument)).Count > 0
        End Function
    End Class

    ''' <summary>
    ''' Represents separate series on the chart
    ''' </summary>
    ''' <remarks></remarks>
    Friend Class VisualizableGroup
        Implements IComparable(Of VisualizableGroup)

        Public Group As GroupType
        Public Name As String
        Public BidField As String
        Public AskField As String
        Public LastField As String
        Public HistField As String
        Public Currency As String
        Public RicStructure As String
        Public Brokers As New List(Of String)

        Public Elements As New Dictionary(Of String, BondPointDescr) 'ric -> datapoint
        Public FromID As Long
        Private _color As String

        Public Property Color() As String
            Get
                Return _color
            End Get
            Set(ByVal value As String)
                _color = value
            End Set
        End Property

        Public Function GetListByFields() As Dictionary(Of String, List(Of String)) 'field -> ric list
            Dim distFields = Elements.Values.Select(Function(elem) elem.SelectedQuote).Distinct.ToList
            ' ReSharper fails when xxx inlined
            Dim xxx = distFields.Select(
                Function(fld)
                    Select Case fld
                        Case QuoteSource.Bid
                            Return BidField
                        Case QuoteSource.Ask
                            Return AskField
                        Case QuoteSource.Hist
                            Return HistField
                        Case Else
                            Return LastField
                    End Select
                End Function)
            Return xxx.ToDictionary(Of String, List(Of String)) _
                        (Function(field) field, Function(field) (From el In Elements.Values Select el.RIC).ToList())
        End Function

        Public Function CompareTo(ByVal other As VisualizableGroup) As Integer Implements IComparable(Of VisualizableGroup).CompareTo
            Return Group.CompareTo(other.Group)
        End Function

        Public Sub Cleanup()
            Elements.Clear()
        End Sub
    End Class

    Friend Class ColorElement
        Implements IComparable(Of ColorElement)

        Private ReadOnly _color As String
        Private ReadOnly _whereFrom As Object

        Public Sub New(ByVal color As String, ByVal whereFrom As Object)
            _color = color
            _whereFrom = whereFrom
        End Sub

        Public ReadOnly Property Color As String
            Get
                Return IIf(_color <> "", _color, "Gray")
            End Get
        End Property

        Public ReadOnly Property WhereFrom As Object
            Get
                Return _whereFrom
            End Get
        End Property

        Public Function CompareTo(ByVal other As ColorElement) As Integer Implements IComparable(Of ColorElement).CompareTo
            If WhereFrom = other.WhereFrom Then Return 0
            If WhereFrom = "list" Then Return 1
            If WhereFrom = "chain" Then
                If other.WhereFrom = "list" Then
                    Return -1
                ElseIf other.WhereFrom = "chain" Then
                    Return 0
                Else
                    Return 1
                End If
            End If
            Return -1
        End Function
    End Class
#End Region

#Region "III. Spreads"
    Friend Class SpreadContainer
        Private _currentMode As SpreadMode = SpreadMode.Yield
        Public Property CurrentMode As SpreadMode
            Get
                Return _currentMode
            End Get
            Set(value As SpreadMode)
                RaiseEvent ModeSelected(value, _currentMode)
                _currentMode = value
            End Set
        End Property

        Public Event ModeSelected As Action(Of SpreadMode, SpreadMode)
        Public Event SpreadUpdated As Action(Of SpreadMode, SpreadMode)

        Public Benchmarks As New Dictionary(Of SpreadMode, ICurve)

        Public Function GetQuote(ByVal theTag As DataPointDescr, Optional ByVal mode As SpreadMode = Nothing) As Double?
            Dim fieldName = If(mode IsNot Nothing, mode.Name, _currentMode.ToString())
            If fieldName = "Yield" Then
                Return theTag.Yld.Yield
            Else
                If theTag IsNot Nothing AndAlso theTag.IsValid Then
                    Dim value As Object
                    Dim fieldInfo = GetType(DataPointDescr).GetField(fieldName)
                    If fieldInfo IsNot Nothing Then
                        value = fieldInfo.GetValue(theTag)
                    Else
                        Dim propertyInfo = GetType(DataPointDescr).GetProperty(fieldName)
                        value = propertyInfo.GetValue(theTag, Nothing)
                    End If
                    If value IsNot Nothing Then Return CDbl(value)
                End If
                Return Nothing
            End If
        End Function

        Public Sub SetQuote(ByVal theTag As DataPointDescr, ByVal value As Double?, Optional ByVal mode As SpreadMode = Nothing)
            Dim fieldName = If(mode IsNot Nothing, mode.Name, _currentMode)
            If theTag IsNot Nothing AndAlso theTag.IsValid Then
                Dim fieldInfo = GetType(DataPointDescr).GetField(fieldName)
                If fieldInfo IsNot Nothing Then
                    fieldInfo.SetValue(theTag, value)
                Else
                    Dim propertyInfo = GetType(DataPointDescr).GetProperty(fieldName)
                    propertyInfo.SetValue(theTag, value, Nothing)
                End If
            End If
        End Sub

        Public Function CalculateSpreads(ByVal descr As DataPointDescr, Optional ByVal mode As SpreadMode = Nothing) As Double?
            If mode Is Nothing Then ' calculate all spreads
                If Benchmarks.ContainsKey(SpreadMode.ZSpread) Then CalculateZSpread(descr)
                If Benchmarks.ContainsKey(SpreadMode.ASWSpread) Then CalculateASWSpread(descr)
                If Benchmarks.ContainsKey(SpreadMode.PointSpread) Then CalculateISpread(descr)
                Return GetQuote(descr)
            Else ' calculate chosen spread
                If mode.Equals(SpreadMode.PointSpread) And Benchmarks.ContainsKey(SpreadMode.PointSpread) Then
                    CalculateISpread(descr)
                ElseIf mode.Equals(SpreadMode.ZSpread) And Benchmarks.ContainsKey(SpreadMode.ZSpread) Then
                    CalculateZSpread(descr)
                ElseIf mode.Equals(SpreadMode.ASWSpread) And Benchmarks.ContainsKey(SpreadMode.ASWSpread) Then
                    CalculateASWSpread(descr)
                ElseIf Not mode.Equals(SpreadMode.Yield) Then
                    SetQuote(descr, Nothing, mode)
                End If
                Return GetQuote(descr, mode)
            End If
        End Function

        Private Sub CalculateISpread(ByVal descr As DataPointDescr)
            Dim iSpreadMainCurve = Benchmarks(SpreadMode.PointSpread)
            If TypeOf descr Is BondPointDescr Then
                Dim tag As BondPointDescr = CType(descr, BondPointDescr)
                tag.PointSpread = ISpread(iSpreadMainCurve.ToArray(), tag)
            ElseIf TypeOf descr Is BidAskPointDescr And Benchmarks.ContainsKey(SpreadMode.ZSpread) Then
                Dim data = CType(descr, BidAskPointDescr)
                Dim tag As BondPointDescr = New BondPointDescr(data.BondTag)
                tag.CalcPrice = data.Price
                data.PointSpread = ISpread(iSpreadMainCurve.ToArray(), tag)
            End If
        End Sub

        Private Sub CalculateASWSpread(ByVal descr As DataPointDescr)
            Dim aswSpreadMainCurve = Benchmarks(SpreadMode.ASWSpread)
            Dim bmk = CType(aswSpreadMainCurve, IAssetSwapBenchmark)
            If TypeOf descr Is BondPointDescr Then
                Dim tag As BondPointDescr = CType(descr, BondPointDescr)
                tag.ASWSpread = ASWSpread(aswSpreadMainCurve.ToArray(), bmk.FloatLegStructure, bmk.FloatingPointValue, tag)
            ElseIf TypeOf descr Is BidAskPointDescr And Benchmarks.ContainsKey(SpreadMode.ASWSpread) Then
                Dim data = CType(descr, BidAskPointDescr)
                Dim tag As BondPointDescr = New BondPointDescr(data.BondTag)
                tag.CalcPrice = data.Price
                data.ASWSpread = ASWSpread(aswSpreadMainCurve.ToArray(), bmk.FloatLegStructure, bmk.FloatingPointValue, tag)
            End If
        End Sub

        Private Sub CalculateZSpread(ByVal descr As DataPointDescr)
            Dim zSpreadMainCurve = Benchmarks(SpreadMode.ZSpread)
            If TypeOf descr Is BondPointDescr Then
                Dim tag As BondPointDescr = CType(descr, BondPointDescr)
                tag.ZSpread = ZSpread(zSpreadMainCurve.ToArray(), tag)
            ElseIf TypeOf descr Is BidAskPointDescr And Benchmarks.ContainsKey(SpreadMode.ZSpread) Then
                Dim data = CType(descr, BidAskPointDescr)
                Dim tag As BondPointDescr = New BondPointDescr(data.BondTag)
                tag.CalcPrice = data.Price
                data.ZSpread = ZSpread(zSpreadMainCurve.ToArray(), tag)
            End If
        End Sub

        Public Sub CleanupCurve(ByVal curveName As String)
            Dim modes = (From keyValue In Benchmarks Where keyValue.Value.GetName() = curveName Select keyValue.Key).ToList()
            modes.ForEach(Sub(mode)
                              Benchmarks.Remove(mode)
                              RaiseEvent SpreadUpdated(mode, _currentMode)
                          End Sub)
            If modes.Contains(CurrentMode) Then CurrentMode = SpreadMode.Yield
        End Sub

        Public Sub UpdateCurve(ByVal curveName As String)
            Dim modes = (From keyValue In Benchmarks Where keyValue.Value.GetName() = curveName Select keyValue.Key).ToList()
            modes.ForEach(Sub(mode) RaiseEvent SpreadUpdated(mode, _currentMode))
        End Sub

        Public Sub SetBenchmark(ByVal mode As SpreadMode, ByVal curve As ICurve)
            If Not Benchmarks.ContainsKey(mode) Then
                Benchmarks.Add(mode, curve)
            Else
                Benchmarks(mode) = curve
            End If
            RaiseEvent SpreadUpdated(mode, _currentMode)
        End Sub
    End Class

    Public Class SpreadMode
        Public Shared Yield As New SpreadMode("Yield", True)
        Public Shared PointSpread As New SpreadMode("PointSpread", True)
        Public Shared ZSpread As New SpreadMode("ZSpread", True)
        Public Shared OASpread As New SpreadMode("OASpread", False)
        Public Shared ASWSpread As New SpreadMode("ASWSpread", True)

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If TypeOf obj Is SpreadMode Then
                Return _name = obj.Name
            Else
                Return False
            End If
        End Function

        Public Overrides Function ToString() As String
            Return _name
        End Function

        Private ReadOnly _name As String
        Private ReadOnly _enabled As Boolean

        Private Sub New(ByVal mode As String, ByVal enabled As Boolean)
            _name = mode
            _enabled = enabled
        End Sub

        Public Shared Operator =(ByVal mode1 As SpreadMode, ByVal mode2 As SpreadMode)
            Return mode1.Name = mode2.Name
        End Operator

        Public Shared Operator <>(ByVal mode1 As SpreadMode, ByVal mode2 As SpreadMode)
            Return mode1.Name <> mode2.Name
        End Operator

        Public Shared Function IsEnabled(name As String)
            Dim staticFields = GetType(SpreadMode).GetFields(BindingFlags.Instance Or BindingFlags.Public)
            Return (From info In staticFields Let element = CType(info.GetValue(Nothing), SpreadMode) Select element.Name = name And element.Enabled).FirstOrDefault()
        End Function

        Public ReadOnly Property Enabled As Boolean
            Get
                Return _enabled
            End Get
        End Property

        Public ReadOnly Property Name As String
            Get
                Return _name
            End Get
        End Property

        Public Shared Function FromString(ByVal name As String) As SpreadMode
            Dim staticFields = GetType(SpreadMode).GetFields()
            Return (From info In staticFields Let element = CType(info.GetValue(Nothing), SpreadMode) Where element.Name = name And element.Enabled Select element).FirstOrDefault()
        End Function
    End Class
#End Region

#Region "IV. Points descriptions"
    Friend Class DataPointDescr
        Implements IComparable(Of DataPointDescr)
        Public Const AroundZero As Double = 0.0001

        Public Overridable ReadOnly Property IsValid As Boolean
            Get
                Return False
            End Get
        End Property
        Public IsVisible As Boolean

        Public Yld As New YieldStructure
        Public Duration As Double
        Public Convexity As Double
        Public PVBP As Double
        Public YieldAtDate As Date

        Public Overridable Property PointSpread As Double?
        Public Overridable Property ZSpread As Double?
        Public Overridable Property OASpread As Double?
        Public Overridable Property ASWSpread As Double?

        Public Function CompareTo(ByVal other As DataPointDescr) As Integer Implements IComparable(Of DataPointDescr).CompareTo
            If Duration < other.Duration Then
                Return -1
            Else
                Return 1
            End If
        End Function

        Public Overrides Function ToString() As String
            Return String.Format("{0:P2} {1:F2}", Yld, Duration)
        End Function

        Public Function Fits(ByVal minX As Double, ByVal minY As Double, ByVal maxX As Double, ByVal maxY As Double) As Boolean
            Return IsValid And (Duration >= minX And Duration <= maxX And Yld.Yield >= minY And Yld.Yield <= maxY)
        End Function
    End Class

    Friend Class BondPointDescr
        Inherits DataPointDescr

        Public RIC As String
        Public ShortName As String
        Public Label As String

        Public Maturity As Date
        Public Coupon As Double

        Public YieldSource As YieldSource
        'Public YieldAtDate As DateTime
        'Public YieldToDate As DateTime

        'Public ToWhat As YieldToWhat

        Public CalcPrice As Double

        Public SelectedQuote As QuoteSource

        Public IssuerID As Integer
        Public SeriesName As String
        Public PaymentStructure As String
        Public PaymentStream As BondPayments
        Public RateStructure As String
        Public IssueDate As Date

        Public Sub New()

        End Sub

        Public Sub New(ByVal descr As BondPointDescr)
            With descr
                RIC = .RIC
                ShortName = .ShortName
                Label = .Label
                Maturity = .Maturity
                Coupon = .Coupon
                YieldSource = .YieldSource
                YieldAtDate = .YieldAtDate
                Yld = descr.Yld
                CalcPrice = .CalcPrice
                SelectedQuote = .SelectedQuote
                IssuerID = .IssuerID
                SeriesName = .SeriesName
                PaymentStructure = .PaymentStructure
                PaymentStream = New BondPayments(.PaymentStream)
                RateStructure = .RateStructure
                IssueDate = .IssueDate
            End With
        End Sub

        Public Overrides Property PointSpread As Double?
        Public Overrides Property ZSpread As Double?
        Public Overrides Property OASpread As Double?
        Public Overrides Property ASWSpread As Double?

        Public Overrides ReadOnly Property IsValid() As Boolean
            Get
                Return CalcPrice > AroundZero And Yld.Yield > MinYield / 100.0 And Yld.Yield < MaxYield / 100.0 And Duration > MinDur And Duration < MaxDur
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return RIC
        End Function

        Public Function ToLongString() As String
            Return String.Format("{0}; Last {1:F2}; {2:P2}/{3:F2}", RIC, CalcPrice, Yld, Duration)
        End Function
    End Class

    Friend Class BidAskPointDescr
        Inherits DataPointDescr
        Private _bidAsk As String
        Private _bondTag As BondPointDescr
        Public Price As Double

        Public Overrides ReadOnly Property IsValid() As Boolean
            Get
                Return Yld.Yield > MinYield / 100.0 And Yld.Yield < MaxYield / 100.0 And Duration > MinDur And Duration < MaxDur
            End Get
        End Property

        Public Property BidAsk() As String
            Get
                Return _bidAsk
            End Get
            Set(ByVal value As String)
                _bidAsk = value
            End Set
        End Property

        Public Property BondTag() As BondPointDescr
            Get
                Return _bondTag
            End Get
            Set(ByVal value As BondPointDescr)
                _bondTag = value
            End Set
        End Property
    End Class

    Friend Class MoneyMarketPointDescr
        Inherits DataPointDescr

        Public Overrides ReadOnly Property IsValid() As Boolean
            Get
                Return True
            End Get
        End Property

        Public FullName As String
        Public YieldCurveName As String

        Public Overrides Property ZSpread As Double?
            Get
                Return Nothing
            End Get
            Set(ByVal value As Double?)
            End Set
        End Property
        Public Overrides Property OASpread As Double?
            Get
                Return Nothing
            End Get
            Set(ByVal value As Double?)
            End Set
        End Property
        Public Overrides Property ASWSpread As Double?
            Get
                Return Nothing
            End Get
            Set(ByVal value As Double?)
            End Set
        End Property
        Public SwpCurve As SwapCurve
    End Class

    Friend Class HistCurvePointDescr
        Inherits DataPointDescr

        Public Overrides ReadOnly Property IsValid() As Boolean
            Get
                Return True
            End Get
        End Property

        Public HistCurveName As String
        Public RIC As String
        'Public YieldAtDate As DateTime
        Public YieldToDate As DateTime
        Public BaseBondName As String

        Public Overrides Function ToString() As String
            Return RIC
        End Function
    End Class
#End Region

#Region "V. Curves descriptions"
    Friend MustInherit Class SeriesDescr
        Public Name As String
    End Class

    Friend Class BondPointsSeries
        Inherits SeriesDescr

        Public Event SelectedPointChanged As Action(Of String, Integer?)

        Public Color As Color
        Private _selectedPointIndex As Integer?

        Public Property SelectedPointIndex As Integer?
            Get
                Return _selectedPointIndex
            End Get
            Set(value As Integer?)
                _selectedPointIndex = value
                RaiseEvent SelectedPointChanged(Name, value)
            End Set
        End Property

        Public Sub ResetSelection()
            _selectedPointIndex = Nothing
        End Sub
    End Class

    Friend Class SwapCurveSeries
        Inherits SeriesDescr
        Public SwpCurve As SwapCurve
    End Class

    Friend Class HistCurveSeries
        Inherits SeriesDescr
    End Class

    Friend Class BondYieldCurveSeries
        Inherits SeriesDescr
        Public BondCurve As YieldCurve
    End Class

    Friend Class BidAskSeries
        Inherits SeriesDescr
    End Class
#End Region
End Namespace