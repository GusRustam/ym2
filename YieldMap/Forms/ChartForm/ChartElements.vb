Imports System.Drawing
Imports System.Reflection
Imports AdfinXRtLib
Imports NLog
Imports YieldMap.Tools.History
Imports YieldMap.Tools.Lists
Imports YieldMap.Commons
Imports YieldMap.Tools
Imports YieldMap.Curves

Namespace Forms.ChartForm
#Region "I. Enumerations"
    Public Enum YieldSource
        Realtime
        Historical
        Synthetic
    End Enum

    Public Enum QuoteSource 'todo get rid, it's too individual
        Bid
        Ask
        Last
        Hist
    End Enum

    Public Enum GroupType
        Chain
        List
        Bond
    End Enum
#End Region

#Region "II. Groups and ansamble"
    Public Class VisualizableAnsamble
        Private ReadOnly _groups As New List(Of VisualizableGroup)

        Private ReadOnly _spreadBmk As SpreadContainer

        Public Event Quote As Action(Of VisualizableBond, String)

        ''' <summary>
        ''' Get a group containing specified bond 
        ''' </summary>
        ''' <param name="instrument">RIC of bond</param>
        ''' <returns>FIRST group which contains specified element</returns>
        ''' <remarks>There might be several groups which contain that element, 
        ''' but they are arranged according to VisualizableGroup sorting rules</remarks>
        Public Function GetInstrumentGroup(ByVal instrument As String) As VisualizableGroup
            Dim grp = _groups.Where(Function(group) group.HasRic(instrument)).ToList()
            grp.Sort()
            Return grp.First
        End Function

        Public Function GetSeriesName(ByVal instrument As String) As String
            Return GetInstrumentGroup(instrument).SeriesName
        End Function

        Public Function GetColor(ByVal instrument As String) As Color
            Return Color.FromName(GetInstrumentGroup(instrument).Color)
        End Function

        Public Sub Cleanup()
            _groups.ForEach(Sub(group) group.Cleanup())
            _groups.Clear()
        End Sub

        Public Function ContainsRIC(ByVal instrument As String) As Boolean
            Return _groups.Any(Function(group) group.HasRic(instrument))
        End Function

        Public Sub StartLoadingLiveData()
            _groups.ForEach(Sub(grp) grp.StartLoadingLiveData())
        End Sub

        Public Sub AddGroup(ByVal group As VisualizableGroup)
            _groups.Add(group)
            AddHandler group.Quote, AddressOf OnGroupQuote
        End Sub

        Private Sub OnGroupQuote(bond As VisualizableBond, field As String)
            RaiseEvent Quote(bond, field)
        End Sub

        Public Sub New(bmk As SpreadContainer)
            _spreadBmk = bmk
        End Sub

        Public Sub CalcAllSpreads(ByVal calculation As CalculatedYield, ByVal metaData As DataBaseBondDescription)
            _spreadBmk.CalcAllSpreads(calculation, metaData)
        End Sub

        Public Function GetGroup(ByVal seriesName As String) As VisualizableGroup
            Return _groups.First(Function(grp) grp.SeriesName = seriesName)
        End Function
    End Class

    Public Class VisualizableBond
        Private _selectedQuote As String
        Private ReadOnly _parentGroup As VisualizableGroup
        Private ReadOnly _metaData As DataBaseBondDescription
        Private ReadOnly _quotesAndYields As Dictionary(Of String, CalculatedYield)
        Public TodayVolume As Double

        Sub New(ByVal parentGroup As VisualizableGroup, ByVal selectedQuote As String, ByVal metaData As DataBaseBondDescription)
            _parentGroup = parentGroup
            Me.SelectedQuote = selectedQuote
            _metaData = metaData
            _quotesAndYields = New Dictionary(Of String, CalculatedYield)
        End Sub

        Public ReadOnly Property ParentGroup As VisualizableGroup
            Get
                Return _parentGroup
            End Get
        End Property

        Public Property SelectedQuote As String
            Get
                Return _selectedQuote
            End Get
            Set(value As String)
                ' todo raise event with new data if necessary
                _selectedQuote = value
            End Set
        End Property

        Public ReadOnly Property MetaData As DataBaseBondDescription
            Get
                Return _metaData
            End Get
        End Property

        Public ReadOnly Property QuotesAndYields As Dictionary(Of String, CalculatedYield)
            Get
                Return _quotesAndYields
            End Get
        End Property
    End Class

    ''' <summary>
    ''' Represents separate series on the chart
    ''' </summary>
    ''' <remarks></remarks>
    Public Class VisualizableGroup
        Implements IComparable(Of VisualizableGroup)
        Private Shared ReadOnly Logger As Logger = GetLogger(GetType(VisualizableGroup))

        Private ReadOnly _ansamble As VisualizableAnsamble

        Public Event Quote As Action(Of VisualizableBond, String)
        Public Event Volume As Action(Of VisualizableBond)

        Public Group As GroupType
        Public SeriesName As String
        Public Id As String = Guid.NewGuid().ToString()
        Public BidField As String
        Public AskField As String
        Public LastField As String
        Public HistField As String
        Public VolumeField As String = "VOLUME"
        Public VWAPField As String = "VWAP"
        Public Currency As String
        Public RicStructure As String
        Public Brokers As New List(Of String)

        Private ReadOnly _elements As New Dictionary(Of String, VisualizableBond) 'ric -> datapoint
        Public PortfolioID As Long
        Private _color As String

        Private WithEvents _quoteLoader As New ListLoadManager
        Private ReadOnly _historyLoaders As New Dictionary(Of String, HistoryLoadManager)


        Public Property Color() As String
            Get
                Return _color
            End Get
            Set(ByVal value As String)
                _color = value
            End Set
        End Property

        Public Function CompareTo(ByVal other As VisualizableGroup) As Integer Implements IComparable(Of VisualizableGroup).CompareTo
            Return Group.CompareTo(other.Group)
        End Function

        Public Sub Cleanup()
            _quoteLoader.DiscardTask(Id)
            _elements.Clear()
        End Sub

        Public Sub StartLoadingLiveData()
            Dim descr = New ListTaskDescr() With {
                    .Items = _elements.Keys.ToList(),
                    .Fields = {BidField, AskField, LastField, VolumeField, VWAPField}.ToList(),
                    .Name = Id,
                    .Descr = SeriesName}

            _quoteLoader.StartNewTask(descr)
        End Sub

        Private Sub QuoteLoaderOnNewData(data As Dictionary(Of String, Dictionary(Of String, Dictionary(Of String, Double?)))) Handles _quoteLoader.OnNewData
            Logger.Trace("QuoteLoaderOnNewData()")
            ' data is a collection of Task -> RIC -> Field -> Value
            ' I must calculate all yields and spreads and fire an event
            If Not data.Keys.Contains(Id) Then Return
            Dim quotes = data(Id)

            For Each instrAndFields As KeyValuePair(Of String, Dictionary(Of String, Double?)) In quotes
                Try
                    Dim instrument As String = instrAndFields.Key
                    Dim fieldsAndValues As Dictionary(Of String, Double?) = instrAndFields.Value

                    ' checking if this bond is allowed to show up
                    If Not _elements.Keys.Contains(instrument) Then
                        Logger.Warn("Unknown instrument {0} in series {1}", instrument, SeriesName)
                        Continue For
                    End If

                    ' now update data point
                    Dim bondDataPoint = _elements(instrument)

                    If fieldsAndValues.ContainsKey(VolumeField) Then
                        bondDataPoint.TodayVolume = fieldsAndValues(VolumeField)
                        RaiseEvent Volume(bondDataPoint)
                    End If

                    For Each fieldName In {BidField, AskField, LastField, VWAPField}
                        If fieldsAndValues.ContainsKey(fieldName) AndAlso fieldsAndValues(fieldName) > 0 Then
                            Dim fieldValue = fieldsAndValues(fieldName)
                            Try
                                HandleQuote(bondDataPoint, fieldName, fieldValue, Date.Today)
                            Catch ex As Exception
                                Logger.WarnException("Failed to plot the point", ex)
                                Logger.Warn("Exception = {0}", ex.ToString())
                            End Try
                        Else
                            If Not bondDataPoint.QuotesAndYields.ContainsKey(fieldName) Then
                                DoLoadHistory(bondDataPoint.MetaData, fieldName)
                            End If
                        End If
                    Next
                Catch ex As Exception
                    Logger.WarnException("Got exception", ex)
                    Logger.Warn("Exception = {0}", ex.ToString())
                End Try
            Next
        End Sub

        Private Sub HandleQuote(ByVal bondDataPoint As VisualizableBond, ByVal fieldName As String, ByVal fieldVal As Double?, ByVal calcDate As Date)
            Dim calculation As New CalculatedYield
            calculation.Price = fieldVal
            calculation.YieldSource = If(calcDate = Date.Today, YieldSource.Realtime, YieldSource.Historical)
            CalculateYields(calcDate, bondDataPoint.MetaData, calculation)
            _ansamble.CalcAllSpreads(calculation, bondDataPoint.MetaData)
            bondDataPoint.QuotesAndYields(fieldName) = calculation
            If bondDataPoint.SelectedQuote = fieldName Then RaiseEvent Quote(bondDataPoint, fieldName)
        End Sub


        Private Sub DoLoadHistory(ByVal bondDataPoint As DataBaseBondDescription, fieldName As String)
            If Not {LastField, VWAPField}.Contains(fieldName) Then Exit Sub

            If _historyLoaders.Any(Function(elem) elem.Key = bondDataPoint.RIC) Then
                _historyLoaders(bondDataPoint.RIC).StopTask()
                _historyLoaders.Remove(bondDataPoint.RIC)
            End If

            Logger.Debug("Will load {0}", bondDataPoint.RIC)

            Dim historyTaskDescr = New HistoryTaskDescr() With {
                    .Item = bondDataPoint.RIC,
                    .StartDate = DateTime.Today.AddDays(-10),
                    .EndDate = DateTime.Today,
                    .Fields = {"DATE", "CLOSE", "VWAP"}.ToList,
                    .Frequency = "D",
                    .InterestingFields = {"DATE", "CLOSE", "VWAP"}.ToList()
            }
            Dim hst = New HistoryLoadManager(Eikon.SDK.CreateAdxRtHistory())
            hst.StartTask(historyTaskDescr, AddressOf OnHistoricalQuotes)
            If hst.Success Then
                Logger.Info("Successfully added task for {0}", historyTaskDescr.Item)
                _historyLoaders.Add(bondDataPoint.RIC, hst)
            End If
        End Sub

        Private Sub OnHistoricalQuotes(ByVal hst As HistoryLoadManager, ByVal ric As String, ByVal datastatus As RT_DataStatus, ByVal data As Dictionary(Of Date, HistoricalItem))
            Logger.Trace("OnHistoricalQuotes({0})", ric)
            If datastatus = RT_DataStatus.RT_DS_FULL Then
                RemoveHandler hst.NewData, AddressOf OnHistoricalQuotes

                If data Is Nothing OrElse data.Count <= 0 Then
                    Logger.Info("No data on {0} arrived", ric)
                    Return
                End If

                Dim maxdate As Date, maxElem As HistoricalItem
                Try
                    maxdate = data.Where(Function(kvp) kvp.Value.SomePrice()).Select(Function(kvp) kvp.Key).Max
                    maxElem = data(maxdate)
                Catch ex As Exception
                    Logger.Info("Failed to retreive max date for {0}", ric)
                    Return
                End Try

                ' checking if this bond is allowed to show up
                If Not _elements.Keys.Contains(ric) Then
                    Logger.Warn("Unknown instrument {0} in series {1}", ric, SeriesName)
                    Return
                End If

                ' now update data point
                Dim bondDataPoint = _elements(ric)

                If maxElem.Close > 0 Then
                    HandleQuote(bondDataPoint, LastField, maxElem.Close, maxdate)
                End If

                If maxElem.VWAP > 0 Then
                    HandleQuote(bondDataPoint, VWAPField, maxElem.Close, maxdate)
                End If
            End If
            If datastatus <> RT_DataStatus.RT_DS_PARTIAL Then
                SyncLock (_historyLoaders)
                    If _historyLoaders.Any(Function(elem) elem.Key = ric) Then
                        _historyLoaders(ric).StopTask()
                        _historyLoaders.Remove(ric)
                    End If
                End SyncLock
            End If
        End Sub

        Public Function HasRic(ByVal instrument As String) As Boolean
            Return _elements.Any(Function(elem) elem.Key = instrument)
        End Function

        Public Sub AddElement(ByVal ric As String, ByVal descr As DataBaseBondDescription)
            _elements.Add(ric, New VisualizableBond(Me, LastField, descr))
        End Sub

        Public Sub New(ansamble As VisualizableAnsamble)
            _ansamble = ansamble
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
    Public Class SpreadContainer
        Private _currentMode As SpreadMode = SpreadMode.Yield
        Public Property CurrentMode As SpreadMode
            Get
                Return _currentMode
            End Get
            Set(value As SpreadMode)
                _currentMode = value
                RaiseEvent ModeSelected(value, _currentMode)
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

        Public Sub CalcAllSpreads(ByRef descr As CalculatedYield, ByVal data As DataBaseBondDescription)
            If Benchmarks.ContainsKey(SpreadMode.ZSpread) Then CalcZSpread(descr, data)
            If Benchmarks.ContainsKey(SpreadMode.ASWSpread) Then CalcASWSpread(descr, data)
            If Benchmarks.ContainsKey(SpreadMode.PointSpread) Then CalcISpread(descr)
        End Sub

        Private Sub CalcISpread(ByRef descr As CalculatedYield)
            CalcPntSprd(Benchmarks(SpreadMode.PointSpread).ToArray(), descr)
        End Sub

        Private Sub CalcASWSpread(ByRef descr As CalculatedYield, ByVal data As DataBaseBondDescription)
            Dim aswSpreadMainCurve = Benchmarks(SpreadMode.ASWSpread)
            Dim bmk = CType(aswSpreadMainCurve, IAssetSwapBenchmark)
            CalcASWSprd(aswSpreadMainCurve.ToArray(), bmk.FloatLegStructure, bmk.FloatingPointValue, descr, data)
        End Sub

        Private Sub CalcZSpread(ByRef descr As CalculatedYield, ByVal data As DataBaseBondDescription)
            CalcZSprd(Benchmarks(SpreadMode.ZSpread).ToArray(), descr, data)
        End Sub

        Public Sub CleanupCurve(ByVal curve As ICurve)
            Dim modes = (From keyValue In Benchmarks Where keyValue.Value.GetName() = curve.GetName() Select keyValue.Key).ToList()
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

        Public Function GetQt(ByVal calc As CalculatedYield) As Double?
            Dim fieldName = _currentMode.ToString()
            If fieldName = "Yield" Then
                Return calc.Yld.Yield
            Else
                Dim value As Object
                Dim fieldInfo = GetType(CalculatedYield).GetField(fieldName)
                If fieldInfo IsNot Nothing Then
                    value = fieldInfo.GetValue(calc)
                Else
                    Dim propertyInfo = GetType(DataPointDescr).GetProperty(fieldName)
                    value = propertyInfo.GetValue(calc, Nothing)
                End If
                If value IsNot Nothing Then Return CDbl(value)
            End If
            Return Nothing
        End Function
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
    Public Class DataPointDescr
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

    'Friend Class HistCurvePointDescr
    '    Inherits DataPointDescr

    '    Public Overrides ReadOnly Property IsValid() As Boolean
    '        Get
    '            Return True
    '        End Get
    '    End Property

    '    Public HistCurveName As String
    '    Public RIC As String
    '    Public BondTag As BondPointDescr
    '    Public Price As Double

    '    Public Overrides Function ToString() As String
    '        Return RIC
    '    End Function
    'End Class
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

    Friend Class BidAskSeries
        Inherits SeriesDescr
    End Class
#End Region
End Namespace