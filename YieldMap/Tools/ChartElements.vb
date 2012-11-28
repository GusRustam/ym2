Imports System.Drawing
Imports System.Reflection
Imports AdfinXRtLib
Imports NLog
Imports YieldMap.Tools.History
Imports YieldMap.Tools.Lists
Imports YieldMap.Commons
Imports YieldMap.Curves

Namespace Tools
#Region "I. Enumerations"
    Public Enum YieldSource
        Realtime
        Historical
        Synthetic
    End Enum

    Public Enum QuoteSource
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

        Public Event AllQuotes As Action(Of List(Of VisualizableBond))
        Public Event Quote As Action(Of VisualizableBond, String)
        Public Event Volume As Action(Of VisualizableBond)
        Public Event Clear As Action(Of VisualizableGroup)

        Public ReadOnly Property SpreadBmk() As SpreadContainer
            Get
                Return _spreadBmk
            End Get
        End Property

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
            _groups.ForEach(Sub(group)
                                group.Cleanup()
                                RemoveHandler group.Quote, AddressOf OnBondQuote
                            End Sub)
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
            AddHandler group.Quote, AddressOf OnBondQuote
            AddHandler group.AllQuotes, AddressOf OnBondAllQuotes
            AddHandler group.Volume, AddressOf OnBondVolume
            AddHandler group.Clear, AddressOf OnGroupClear
        End Sub

        Private Sub OnBondAllQuotes(ByVal obj As List(Of VisualizableBond))
            RaiseEvent AllQuotes(obj)
        End Sub

        Private Sub OnGroupClear(ByVal obj As VisualizableGroup)
            RaiseEvent Clear(obj)
        End Sub

        Private Sub OnBondVolume(ByVal obj As VisualizableBond)
            RaiseEvent Volume(obj)
        End Sub

        Private Sub OnBondQuote(bond As VisualizableBond, field As String)
            RaiseEvent Quote(bond, field)
        End Sub

        Public Sub New(bmk As SpreadContainer)
            _spreadBmk = bmk
        End Sub

        Public Sub CalcAllSpreads(ByVal type As SpreadType, ByRef calculation As BondPointDescription, ByVal metaData As DataBaseBondDescription)
            _spreadBmk.CalcAllSpreads(calculation, metaData, type)
        End Sub

        Public Sub CalcAllSpreads(ByRef calculation As BondPointDescription, ByVal metaData As DataBaseBondDescription)
            For i = 0 To _spreadBmk.Benchmarks.Keys.Count
                _spreadBmk.CalcAllSpreads(calculation, metaData, _spreadBmk.Benchmarks.Keys(i))
            Next
        End Sub

        Public Function GetGroup(ByVal seriesName As String) As VisualizableGroup
            Return _groups.First(Function(grp) grp.SeriesName = seriesName)
        End Function

        Public Sub RecalculateByType(ByVal type As SpreadType)
            _groups.ForEach(Sub(group) group.RecalculateByType(Type))
        End Sub

        Public Sub CleanupByType(ByVal type As SpreadType)
            _groups.ForEach(Sub(group) group.CleanupByType(type))
        End Sub

        Public Sub CleanupSpread(ByVal type As SpreadType, ByRef descr As BasePointDescription)
            _spreadBmk.CleanupSpread(descr, type)
        End Sub
    End Class

    Public Class VisualizableBond
        Private _selectedQuote As String
        Private ReadOnly _parentGroup As VisualizableGroup
        Private ReadOnly _metaData As DataBaseBondDescription
        Private ReadOnly _quotesAndYields As New Dictionary(Of String, BondPointDescription)
        Public TodayVolume As Double

        Sub New(ByVal parentGroup As VisualizableGroup, ByVal selectedQuote As String, ByVal metaData As DataBaseBondDescription)
            _parentGroup = parentGroup
            Me.SelectedQuote = selectedQuote
            _metaData = metaData
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
                _selectedQuote = value
                'Recalculate()
            End Set
        End Property

        Public ReadOnly Property MetaData As DataBaseBondDescription
            Get
                Return _metaData
            End Get
        End Property

        Public ReadOnly Property QuotesAndYields As Dictionary(Of String, BondPointDescription)
            Get
                Return _quotesAndYields
            End Get
        End Property

        Public Sub RecalculateByType(type As SpreadType)
            If _quotesAndYields.ContainsKey(_selectedQuote) Then
                _parentGroup.Ansamble.CalcAllSpreads(type, _quotesAndYields(_selectedQuote), _metaData)
            End If
        End Sub

        Public Sub CleanupByType(ByVal type As SpreadType)
            If _quotesAndYields.ContainsKey(_selectedQuote) Then
                _parentGroup.Ansamble.CleanupSpread(type, _quotesAndYields(_selectedQuote))
            End If
        End Sub
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
        Public Event Clear As Action(Of VisualizableGroup)
        Public Event Volume As Action(Of VisualizableBond)
        Public Event AllQuotes As Action(Of List(Of VisualizableBond))

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

        Public Property Color() As String
            Get
                Return _color
            End Get
            Set(ByVal value As String)
                _color = value
            End Set
        End Property

        Public ReadOnly Property Ansamble() As VisualizableAnsamble
            Get
                Return _ansamble
            End Get
        End Property

        Public Function CompareTo(ByVal other As VisualizableGroup) As Integer Implements IComparable(Of VisualizableGroup).CompareTo
            Return Group.CompareTo(other.Group)
        End Function

        Public Sub Cleanup()
            _quoteLoader.DiscardTask(Id)
            _elements.Clear()
            RaiseEvent Clear(Me)
        End Sub

        Public Sub StartLoadingLiveData()
            Dim descr = New ListTaskDescr() With {
                    .Items = _elements.Keys.ToList(),
                    .Fields = {BidField, AskField, LastField, VolumeField, VWAPField}.ToList(),
                    .Name = Id,
                    .Descr = SeriesName}

            _quoteLoader.StartNewTask(descr)
        End Sub

        Private Sub QuoteLoaderOnNewData(data As Dictionary(Of String, Dictionary(Of String, Dictionary(Of String, Double)))) Handles _quoteLoader.OnNewData
            Logger.Trace("QuoteLoaderOnNewData()")
            ' data is a collection of Task -> RIC -> Field -> Value
            ' I must calculate all yields and spreads and fire an event
            If Not data.Keys.Contains(Id) Then Return
            Dim quotes = data(Id)

            For Each instrAndFields As KeyValuePair(Of String, Dictionary(Of String, Double)) In quotes
                Try
                    Dim instrument As String = instrAndFields.Key
                    Dim fieldsAndValues As Dictionary(Of String, Double) = instrAndFields.Value

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

        Private Sub HandleQuote(ByRef bondDataPoint As VisualizableBond, ByVal fieldName As String, ByVal fieldVal As Double?, ByVal calcDate As Date)
            Dim calculation As New BondPointDescription
            calculation.Price = fieldVal
            calculation.YieldSource = If(calcDate = Date.Today, YieldSource.Realtime, YieldSource.Historical)
            CalculateYields(calcDate, bondDataPoint.MetaData, calculation)
            _ansamble.CalcAllSpreads(calculation, bondDataPoint.MetaData)
            bondDataPoint.QuotesAndYields(fieldName) = calculation
            If bondDataPoint.SelectedQuote = fieldName Then RaiseEvent Quote(bondDataPoint, fieldName)
        End Sub

        Private Sub DoLoadHistory(ByVal bondDataPoint As DataBaseBondDescription, fieldName As String)
            If Not {LastField, VWAPField}.Contains(fieldName) Then Exit Sub
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
        End Sub

        Private Sub OnHistoricalQuotes(ByVal hst As HistoryLoadManager, ByVal ric As String, ByVal datastatus As RT_DataStatus, ByVal data As Dictionary(Of Date, HistoricalItem))
            Logger.Trace("OnHistoricalQuotes({0})", ric)
            If datastatus = RT_DataStatus.RT_DS_FULL Then
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

        Public Function GetElement(ByVal ric As String) As VisualizableBond
            Return _elements(ric)
        End Function

        Public Sub RecalculateByType(type As SpreadType)
            _elements.Keys.ToList().ForEach(
                Sub(ric)
                    Dim elem = _elements(ric)
                    If elem.QuotesAndYields.ContainsKey(elem.SelectedQuote) Then
                        elem.RecalculateByType(type)
                    End If
                End Sub)
            If _ansamble.SpreadBmk.CurrentType = type Then RaiseEvent AllQuotes(_elements.Values.ToList())
        End Sub

        Public Sub CleanupByType(ByVal type As SpreadType)
            _elements.Keys.ToList().ForEach(
                Sub(ric)
                    Dim elem = _elements(ric)
                    If elem.QuotesAndYields.ContainsKey(elem.SelectedQuote) Then
                        elem.CleanupByType(type)
                    End If
                End Sub)
        End Sub
    End Class
#End Region

#Region "III. Spreads"
    Public Class SpreadContainer
        Public Benchmarks As New Dictionary(Of SpreadType, SwapCurve)

        Public Sub CalcAllSpreads(ByRef descr As BasePointDescription, Optional ByVal data As DataBaseBondDescription = Nothing, Optional ByVal type As SpreadType = Nothing)
            If type IsNot Nothing Then
                If data IsNot Nothing Then
                    If type = SpreadType.ZSpread AndAlso Benchmarks.ContainsKey(SpreadType.ZSpread) Then
                        CalcZSprd(Benchmarks(SpreadType.ZSpread).ToArray(), descr, data)
                    End If
                    If type = SpreadType.ASWSpread AndAlso Benchmarks.ContainsKey(SpreadType.ASWSpread) Then
                        Dim aswSpreadMainCurve = Benchmarks(SpreadType.ASWSpread)
                        Dim bmk = CType(aswSpreadMainCurve, IAssetSwapBenchmark)
                        CalcASWSprd(aswSpreadMainCurve.ToArray(), bmk.FloatLegStructure, bmk.FloatingPointValue, descr, data)
                    End If
                End If
                If type = SpreadType.PointSpread AndAlso Benchmarks.ContainsKey(SpreadType.PointSpread) Then
                    CalcPntSprd(Benchmarks(SpreadType.PointSpread).ToArray(), descr)
                End If
            Else
                If data IsNot Nothing Then
                    If Benchmarks.ContainsKey(SpreadType.ZSpread) Then
                        CalcZSprd(Benchmarks(SpreadType.ZSpread).ToArray(), descr, data)
                    End If
                    If Benchmarks.ContainsKey(SpreadType.ASWSpread) Then
                        Dim aswSpreadMainCurve = Benchmarks(SpreadType.ASWSpread)
                        Dim bmk = CType(aswSpreadMainCurve, IAssetSwapBenchmark)
                        CalcASWSprd(aswSpreadMainCurve.ToArray(), bmk.FloatLegStructure, bmk.FloatingPointValue, descr, data)
                    End If
                End If
                If Benchmarks.ContainsKey(SpreadType.PointSpread) Then
                    CalcPntSprd(Benchmarks(SpreadType.PointSpread).ToArray(), descr)
                End If
            End If
        End Sub

        Public Function GetQt(ByVal calc As BasePointDescription) As Double?
            Dim fieldName = _currentType.ToString()
            If fieldName = "Yield" Then
                Return calc.GetYield()
            Else
                Dim value As Object
                Dim fieldInfo = GetType(BondPointDescription).GetField(fieldName)
                If fieldInfo IsNot Nothing Then
                    value = fieldInfo.GetValue(calc)
                Else
                    Dim propertyInfo = GetType(BasePointDescription).GetProperty(fieldName)
                    value = propertyInfo.GetValue(calc, Nothing)
                End If
                If value IsNot Nothing Then Return CDbl(value)
            End If
            Return Nothing
        End Function

        Private _currentType As SpreadType = SpreadType.Yield
        Public Property CurrentType As SpreadType
            Get
                Return _currentType
            End Get
            Set(value As SpreadType)
                Dim oldType = _currentType
                _currentType = value
                RaiseEvent TypeSelected(_currentType, oldType)
            End Set
        End Property

        Public Event TypeSelected As Action(Of SpreadType, SpreadType)
        Public Event BenchmarkRemoved As Action(Of SpreadType)
        Public Event BenchmarkUpdated As Action(Of SpreadType)

        Public Sub OnCurveRemoved(ByVal curve As SwapCurve)
            Dim types = (From keyValue In Benchmarks Where keyValue.Value.GetName() = curve.GetName() Select keyValue.Key).ToList()
            types.ForEach(Sub(type)
                              Benchmarks.Remove(type)
                              RaiseEvent BenchmarkRemoved(type)
                          End Sub)
            If types.Contains(CurrentType) Then CurrentType = SpreadType.Yield
        End Sub


        Public Sub UpdateCurve(ByVal curveName As String)
            Dim types = (From keyValue In Benchmarks Where keyValue.Value.GetName() = curveName Select keyValue.Key).ToList()
            types.ForEach(Sub(type) RaiseEvent BenchmarkUpdated(type))
        End Sub

        Public Sub AddType(ByVal type As SpreadType, ByVal curve As SwapCurve)
            If Benchmarks.ContainsKey(type) Then
                Benchmarks.Remove(type)
            End If
            Benchmarks.Add(type, curve)
            RaiseEvent BenchmarkUpdated(type)
        End Sub

        Public Sub RemoveType(ByVal type As SpreadType)
            If Benchmarks.ContainsKey(type) Then
                RaiseEvent BenchmarkRemoved(type)
                Benchmarks.Remove(type)
            End If
            If CurrentType = type Then CurrentType = SpreadType.Yield
        End Sub

        Public Sub CleanupSpread(ByRef descr As BasePointDescription, ByVal type As SpreadType)
            If type = SpreadType.ZSpread Then
                descr.ZSpread = Nothing
            ElseIf type = SpreadType.ASWSpread Then
                descr.ASWSpread = Nothing
            ElseIf type = SpreadType.PointSpread Then
                descr.PointSpread = Nothing
            End If
        End Sub
    End Class

    Public Class SpreadType
        Public Shared Yield As New SpreadType("Yield", True)
        Public Shared PointSpread As New SpreadType("PointSpread", True)
        Public Shared ZSpread As New SpreadType("ZSpread", True)
        Public Shared OASpread As New SpreadType("OASpread", False)
        Public Shared ASWSpread As New SpreadType("ASWSpread", True)

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If TypeOf obj Is SpreadType Then
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

        Public Shared Operator =(ByVal mode1 As SpreadType, ByVal mode2 As SpreadType)
            If mode1 Is Nothing OrElse mode2 Is Nothing Then Return False
            Return mode1.Name = mode2.Name
        End Operator

        Public Shared Operator <>(ByVal mode1 As SpreadType, ByVal mode2 As SpreadType)
            If mode1 Is Nothing OrElse mode2 Is Nothing Then Return False
            Return mode1.Name <> mode2.Name
        End Operator

        Public Shared Function IsEnabled(name As String)
            Dim staticFields = GetType(SpreadType).GetFields(BindingFlags.Instance Or BindingFlags.Public)
            Return (From info In staticFields Let element = CType(info.GetValue(Nothing), SpreadType) Select element.Name = name And element.Enabled).FirstOrDefault()
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

        Public Shared Function FromString(ByVal name As String) As SpreadType
            Dim staticFields = GetType(SpreadType).GetFields()
            Return (From info In staticFields Let element = CType(info.GetValue(Nothing), SpreadType) Where element.Name = name And element.Enabled Select element).FirstOrDefault()
        End Function
    End Class
#End Region

#Region "IV. Points descriptions"
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
#End Region
End Namespace