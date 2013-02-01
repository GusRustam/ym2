Imports System.Drawing
Imports System.Reflection
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
    Public Class Ansamble
        Private ReadOnly _groups As New List(Of Group)

        Public ReadOnly Property Groups As List(Of Group)
            Get
                Return _groups
            End Get
        End Property

        Private ReadOnly _spreadBmk As SpreadContainer

        Public Event AllQuotes As Action(Of List(Of Bond))
        Public Event RemovedItem As Action(Of Group, String)
        Public Event Quote As Action(Of Bond, String)
        Public Event Volume As Action(Of Bond)
        Public Event Clear As Action(Of Group)

        Public ReadOnly Property SpreadBmk() As SpreadContainer
            Get
                Return _spreadBmk
            End Get
        End Property

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


        Public Sub StartLoadingLiveData()
            _groups.ForEach(Sub(grp) grp.StartAll())
        End Sub

        Public Sub AddGroup(ByVal group As Group)
            _groups.Add(group)
            AddHandler group.Quote, AddressOf OnBondQuote
            AddHandler group.RemovedItem, AddressOf OnRemovedItem
            AddHandler group.AllQuotes, AddressOf OnBondAllQuotes
            AddHandler group.Volume, AddressOf OnBondVolume
            AddHandler group.Clear, AddressOf OnGroupClear
        End Sub

        Private Sub OnRemovedItem(ByVal grp As Group, ByVal ric As String)
            RaiseEvent RemovedItem(grp, ric)
        End Sub

        Private Sub OnBondAllQuotes(ByVal obj As List(Of Bond))
            RaiseEvent AllQuotes(obj)
        End Sub

        Private Sub OnGroupClear(ByVal obj As Group)
            RaiseEvent Clear(obj)
        End Sub

        Private Sub OnBondVolume(ByVal obj As Bond)
            RaiseEvent Volume(obj)
        End Sub

        Private Sub OnBondQuote(ByVal bond As Bond, ByVal field As String)
            RaiseEvent Quote(bond, field)
        End Sub

        Public Sub New(ByVal bmk As SpreadContainer)
            _spreadBmk = bmk
        End Sub

#Region "Groups - these are methods that must be encapsulateds"
        ' todo these are methods that must be encapsulated
        Public Sub RemovePoint(ByVal ric As String)
            While _groups.Any(Function(grp) grp.HasRic(ric))
                Dim g = _groups.First(Function(grp) grp.HasRic(ric))
                g.RemoveRic(ric)
            End While
        End Sub

        Public Function GetGroupById(ByVal id As String) As Group
            Return _groups.First(Function(grp) grp.Id = id)
        End Function

        Public Function GetGroupList() As Dictionary(Of Guid, String)
            Dim res As New Dictionary(Of Guid, String)
            _groups.ForEach(Sub(grp) res.Add(Guid.Parse(grp.Id), grp.SeriesName))
            Return res
        End Function

        Public Function GetGroup(ByVal id As Guid) As Group
            Return _groups.First(Function(grp) grp.Id = id.ToString())
        End Function

        Public Function ContainsRic(ByVal instrument As String) As Boolean
            Return _groups.Any(Function(group) group.HasRic(instrument))
        End Function

        ''' <summary>
        ''' Get a group containing specified bond 
        ''' </summary>
        ''' <param name="instrument">RIC of bond</param>
        ''' <returns>FIRST group which contains specified element</returns>
        ''' <remarks>There might be several groups which contain that element, 
        ''' but they are arranged according to VisualizableGroup sorting rules</remarks>
        Public Function GetInstrumentGroup(ByVal instrument As String) As Group
            Dim grp = _groups.Where(Function(group) group.HasRic(instrument)).ToList()
            grp.Sort()
            Return grp.First
        End Function

        Public Function GetSeriesName(ByVal instrument As String) As String
            Return GetInstrumentGroup(instrument).SeriesName
        End Function
#End Region

        Public Sub RecalculateByType(ByVal type As SpreadType)
            _groups.ForEach(Sub(group) group.RecalculateByType(type))
        End Sub

        Public Sub CleanupByType(ByVal type As SpreadType)
            _groups.ForEach(Sub(group) group.CleanupByType(type))
        End Sub

        Public Sub CleanupSpread(ByVal type As SpreadType, ByRef descr As BasePointDescription)
            _spreadBmk.CleanupSpread(descr, type)
        End Sub

        Public Sub RemoveGroup(ByVal groupId As String)
            While _groups.Any(Function(grp) grp.Id = groupId)
                Dim g = _groups.First(Function(grp) grp.Id = groupId)
                g.Cleanup()
                _groups.Remove(g)
            End While
        End Sub

        Public Function HasGroupById(ByVal id As String) As Boolean
            Return _groups.Any(Function(grp) grp.Id = id)
        End Function
    End Class

    Public Class Bond
        Private _selectedQuote As String
        Private ReadOnly _parentGroup As Group
        Private ReadOnly _metaData As DataBaseBondDescription
        Private ReadOnly _quotesAndYields As New Dictionary(Of String, BondPointDescription)
        Public TodayVolume As Double

        Private _labelMode As LabelMode = LabelMode.IssuerAndSeries
        Public Property LabelMode As LabelMode
            Get
                Return _labelMode
            End Get
            Set(ByVal value As LabelMode)
                _labelMode = value
            End Set
        End Property

        Sub New(ByVal parentGroup As Group, ByVal selectedQuote As String, ByVal metaData As DataBaseBondDescription)
            _parentGroup = parentGroup
            Me.SelectedQuote = selectedQuote
            _metaData = metaData
        End Sub

        Public ReadOnly Property ParentGroup As Group
            Get
                Return _parentGroup
            End Get
        End Property

        Public Property SelectedQuote As String
            Get
                Return _selectedQuote
            End Get
            Set(ByVal value As String)
                _selectedQuote = value
                ParentGroup.NotifyQuote(Me)
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

        Public Sub RecalculateByType(ByVal type As SpreadType)
            If _quotesAndYields.ContainsKey(_selectedQuote) Then
                _parentGroup.Ansamble.SpreadBmk.CalcAllSpreads(_quotesAndYields(_selectedQuote), _metaData, type)
            End If
        End Sub

        Public Sub CleanupByType(ByVal type As SpreadType)
            If _quotesAndYields.ContainsKey(_selectedQuote) Then
                _parentGroup.Ansamble.CleanupSpread(type, _quotesAndYields(_selectedQuote))
            End If
        End Sub

        Function GetFieldByKey(ByVal key As String) As String
            If ParentGroup.BidField = key Then Return "BID"
            If ParentGroup.AskField = key Then Return "ASK"
            If ParentGroup.LastField = key Then Return "LAST"
            If ParentGroup.HistField = key Then Return "CLOSE"
            If ParentGroup.VwapField = key Then Return "VWAP"
            Return key
        End Function

    End Class

    Public Enum LabelMode
        IssuerAndSeries
        ShortName
        Description
        SeriesOnly
    End Enum

    ''' <summary>
    ''' Represents separate series on the chart
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Group
        Implements IComparable(Of Group)
        Private Shared ReadOnly Logger As Logger = GetLogger(GetType(Group))

        Private ReadOnly _ansamble As Ansamble

        Public Event Quote As Action(Of Bond, String)
        Public Event RemovedItem As Action(Of Group, String)
        Public Event Clear As Action(Of Group)
        Public Event Volume As Action(Of Bond)
        Public Event AllQuotes As Action(Of List(Of Bond))
        Public Event LoadCriticalError As Action
        Public Event LoadError As Action(Of WrongItemsInfo)

        Public Group As GroupType
        Public SeriesName As String
        Public Id As String = Guid.NewGuid().ToString()
        Public BidField As String
        Public AskField As String
        Public LastField As String
        Public HistField As String
        Public VolumeField As String
        Public VwapField As String
        Public Currency As String
        Public RicStructure As String
        Public Brokers As New List(Of String)

        Private ReadOnly _elements As New Dictionary(Of String, Bond) 'ric -> datapoint
        Public ReadOnly Property Elements() As Dictionary(Of String, Bond)
            Get
                Return _elements
            End Get
        End Property

        Public PortfolioID As Long
        Private _color As String

        Private WithEvents _quoteLoader As New ListLoadManager_v2

        Public Property Color() As String
            Get
                Return _color
            End Get
            Set(ByVal value As String)
                _color = value
            End Set
        End Property

        Public ReadOnly Property Ansamble() As Ansamble
            Get
                Return _ansamble
            End Get
        End Property

        Public Function CompareTo(ByVal other As Group) As Integer Implements IComparable(Of Group).CompareTo
            Return Group.CompareTo(other.Group)
        End Function

        Public Sub Cleanup()
            _quoteLoader.CancelAll()
            _elements.Clear()
            RaiseEvent Clear(Me)
        End Sub

        Public Sub StartRics(ByVal rics As List(Of String))
            Dim res = _quoteLoader.AddItems(rics, GetAllKnownFields())
            If res Is Nothing Then
                RaiseEvent LoadCriticalError()
            Else
                If res.WrongFields.Any Or res.WrongItems.Any Then
                    RaiseEvent LoadError(res)
                End If
            End If
        End Sub

        Private Function GetAllKnownFields() As List(Of String)
            Return {BidField, AskField, LastField, VolumeField, VwapField}.
                Where(Function(fldName) fldName IsNot Nothing AndAlso fldName.Trim() <> "").ToList()
        End Function

        Private Function GetKnownPriceFields() As List(Of String)
            Return {BidField, AskField, LastField, VwapField}.
                Where(Function(fldName) fldName IsNot Nothing AndAlso fldName.Trim() <> "").ToList()
        End Function

        Public Sub StartAll()
            StartRics(_elements.Keys.ToList())
        End Sub

        Private Sub OnQuotes(ByVal data As Dictionary(Of String, Dictionary(Of String, Double)), ByVal errorInfo As WrongItemsInfo) Handles _quoteLoader.OnNewData
            Logger.Trace("QuoteLoaderOnNewData()")
            If errorInfo IsNot Nothing Then
                If errorInfo.WrongFields.Any Then
                    ' todo what? mark field as dead one for the corresponding bond
                End If
                If errorInfo.WrongItems.Any Then
                    If errorInfo.WrongItems.ContainsKey(ItemInfo.InvalidItems) Then
                        errorInfo.WrongItems(ItemInfo.InvalidItems).ForEach(
                            Sub(tuple)
                                Dim ric = tuple.Item1
                                Dim status = tuple.Item2
                                ' todo what? mark bond as dead
                            End Sub)
                    End If
                End If
            End If
            For Each instrAndFields As KeyValuePair(Of String, Dictionary(Of String, Double)) In data
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

                    For Each fieldName In GetKnownPriceFields()
                        If fieldsAndValues.ContainsKey(fieldName) AndAlso fieldsAndValues(fieldName) > 0 Then
                            Dim fieldValue = fieldsAndValues(fieldName)
                            Try
                                HandleQuote(bondDataPoint, fieldName, fieldValue, Date.Today)
                            Catch ex As Exception
                                Logger.WarnException("Failed to plot the point", ex)
                                Logger.Warn("Exception = {0}", ex.ToString())
                            End Try
                        Else
                            If fieldName = _elements(instrument).SelectedQuote And Not bondDataPoint.QuotesAndYields.ContainsKey(fieldName) Then
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

        Private Sub HandleQuote(ByRef bondDataPoint As Bond, ByVal fieldName As String, ByVal fieldVal As Double?, ByVal calcDate As Date)
            Dim calculation As New BondPointDescription
            calculation.Price = fieldVal
            calculation.YieldSource = If(calcDate = Date.Today, YieldSource.Realtime, YieldSource.Historical)
            CalculateYields(calcDate, bondDataPoint.MetaData, calculation)

            Dim container = _ansamble.SpreadBmk
            For i = 0 To container.Benchmarks.Keys.Count
                container.CalcAllSpreads(calculation, bondDataPoint.MetaData, container.Benchmarks.Keys(i))
            Next
            bondDataPoint.QuotesAndYields(fieldName) = calculation
            If bondDataPoint.SelectedQuote = fieldName Then RaiseEvent Quote(bondDataPoint, fieldName)
        End Sub

        Private Sub DoLoadHistory(ByVal bondDataPoint As DataBaseBondDescription, ByVal fieldName As String)
            If Not {LastField, VwapField}.Contains(fieldName) Then Exit Sub
            Logger.Debug("Will load {0}", bondDataPoint.RIC)

            Dim hst = New HistoryLoadManager_v2()
            AddHandler hst.HistoricalData, AddressOf OnHistoricalQuotes
            hst.StartTask(bondDataPoint.RIC, "DATE,CLOSE,VWAP", DateTime.Today.AddDays(-10), DateTime.Today)
        End Sub

        Private Sub OnHistoricalQuotes(ByVal ric As String, ByVal status As LoaderStatus, ByVal hstatus As HistoryStatus, ByVal data As Dictionary(Of Date, HistoricalItem))
            Logger.Trace("OnHistoricalQuotes({0})", ric)
            If Not status.Finished Or status.Err Then
                Logger.Warn(String.Format("{0} not finished or error!!!, hstatus = {1}, reason = {2}", ric, hstatus, status.Reason))
            End If
            If hstatus = HistoryStatus.Full Then
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
                    HandleQuote(bondDataPoint, VwapField, maxElem.Close, maxdate)
                End If
            End If
        End Sub

        Public Function HasRic(ByVal instrument As String) As Boolean
            Return _elements.Any(Function(elem) elem.Key = instrument)
        End Function

        Public Sub AddRic(ByVal ric As String, ByVal descr As DataBaseBondDescription, ByVal quote As String)
            _elements.Add(ric, New Bond(Me, quote, descr))
        End Sub

        Public Sub New(ByVal ansamble As Ansamble)
            _ansamble = ansamble
        End Sub

        Public Function GetElement(ByVal ric As String) As Bond
            Return _elements(ric)
        End Function

        Public Sub RecalculateByType(ByVal type As SpreadType)
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

        Public Sub RemoveRic(ByVal ric As String)
            _quoteLoader.CancelItem(ric)
            While _elements.Any(Function(elem) elem.Key = ric)
                _elements.Remove(ric)
            End While
            RaiseEvent RemovedItem(Me, ric)
        End Sub

        Public Sub NotifyQuote(ByVal bond As Bond)
            RaiseEvent Quote(bond, bond.SelectedQuote)
        End Sub
    End Class
#End Region

#Region "III. Spreads"
    Public Class SpreadContainer
        Public Benchmarks As New Dictionary(Of SpreadType, SwapCurve)

        Public Event TypeSelected As Action(Of SpreadType, SpreadType)
        Public Event BenchmarkRemoved As Action(Of SpreadType)
        Public Event BenchmarkUpdated As Action(Of SpreadType)

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
                    CalcPntSprd(Benchmarks(SpreadType.PointSpread).ToFittedArray(), descr)
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
                    CalcPntSprd(Benchmarks(SpreadType.PointSpread).ToFittedArray(), descr)
                End If
            End If
        End Sub

        Public Function GetActualQuote(ByVal calc As BasePointDescription) As Double?
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
    Friend Class HistoryPoint
        Public Ric As String
        Public Descr As HistPointDescription
        Public Meta As DataBaseBondDescription
        Public SeriesId As Guid
    End Class
#End Region

#Region "V. Curves descriptions"
    Friend Class BondSetSeries
        Public Name As String
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
#End Region
End Namespace