Imports DbManager.Bonds
Imports DbManager
Imports NLog
Imports ReutersData
Imports Settings
Imports Uitls

Namespace Tools.Elements
    ''' <summary>
    ''' Represents separate series on the chart
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class Group
        Inherits Identifyable
        Implements IChangeable

        Protected Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(Group))

        Public Event Cleared As Action Implements IChangeable.Cleared
        Public Event Volume As Action(Of Bond)
        Public Event Updated As Action(Of List(Of PointOfCurve)) Implements IChangeable.Updated
        Public Event UpdatedSpread As Action(Of List(Of PointOfCurve), IOrdinate) Implements IChangeable.UpdatedSpread

        Public MustOverride Sub Recalculate() Implements IChangeable.Recalculate
        Public MustOverride Sub RecalculateTotal() Implements IChangeable.RecalculateTotal
        Public MustOverride Sub Recalculate(ByVal ord As IOrdinate) Implements IChangeable.Recalculate

        Protected Nm As String
        Public ReadOnly Property Name() As String Implements INamed.Name
            Get
                Return Nm
            End Get
        End Property

        Private ReadOnly _histFields As FieldContainer

        Private _groupDate As Date = Today
        Public Property GroupDate() As Date Implements IChangeable.GroupDate
            Get
                Return _groupDate
            End Get
            Set(ByVal value As Date)
                If _groupDate <> value Then
                    _groupDate = value
                    Subscribe()
                End If
            End Set
        End Property

        Private ReadOnly _bondFields As FieldContainer
        Public ReadOnly Property BondFields() As FieldContainer
            Get
                Return _bondFields
            End Get
        End Property

        Public PortfolioID As Long


        Private ReadOnly _ansamble As Ansamble
        Public ReadOnly Property Ansamble() As Ansamble
            Get
                Return _ansamble
            End Get
        End Property

        Private _eventsFrozen As Boolean = False

        Public ReadOnly Property EventsFrozen() As Boolean
            Get
                Return _eventsFrozen
            End Get
        End Property

        Public Sub FreezeEvents() Implements IChangeable.FreezeEvents
            _eventsFrozen = True
        End Sub

        Public Sub UnfreezeEvents() Implements IChangeable.UnfreezeEvents
            _eventsFrozen = False
            Recalculate()
        End Sub

        Public Sub UnfreezeEventsQuiet() Implements IChangeable.UnfreezeEventsQuiet
            _eventsFrozen = False
            If Ansamble.YSource <> Yield Then Recalculate(Ansamble.YSource)
        End Sub

        Protected Overridable Sub NotifyUpdated(ByVal curveItems As List(Of PointOfCurve))
            If Not _eventsFrozen Then RaiseEvent Updated(curveItems)
        End Sub

        Protected Sub NotifyUpdatedSpread(ByVal curveItems As List(Of PointOfCurve), ord As IOrdinate)
            If Not _eventsFrozen Then RaiseEvent UpdatedSpread(curveItems, ord)
        End Sub

        Private ReadOnly _elements As New List(Of Bond) 'ric -> datapoint
        Public ReadOnly Property Elements() As List(Of Bond)
            Get
                Return (From elem In _elements Where elem.Enabled).ToList()
            End Get
        End Property

        Public ReadOnly Property AllElements() As List(Of Bond)
            Get
                Return _elements
            End Get
        End Property

        Public ReadOnly Property DisabledElements() As List(Of Bond) Implements IChangeable.DisabledElements
            Get
                Return (From elem In _elements Where Not elem.Enabled).ToList()
            End Get
        End Property

        Private WithEvents _quoteLoader As New LiveQuotes
        Protected ReadOnly Property QuoteLoader() As LiveQuotes
            Get
                Return _quoteLoader
            End Get
        End Property

        Private _color As String

        Public Property Color() As String
            Get
                Return _color
            End Get
            Set(ByVal value As String)
                _color = value
            End Set
        End Property

        Public Sub Cleanup() Implements IChangeable.Cleanup
            _quoteLoader.CancelAll()
            _elements.Clear()
            RaiseEvent Cleared()
        End Sub

        Public Overridable Sub Subscribe() Implements IChangeable.Subscribe
            'Dim rics As List(Of String) = (From elem In _elements Select elem.MetaData.RIC).ToList()
            'If rics.Count = 0 Then Return
            '_quoteLoader.AddItems(rics, BondFields.AllNames)
            Dim rics As List(Of String) = (From elem In AllElements Select elem.MetaData.Ric).ToList()
            If rics.Count = 0 Then Return
            If GroupDate = Today Then
                QuoteLoader.AddItems(rics, _bondFields.AllNames)
            Else
                QuoteLoader.CancelAll()
                Dim historyBlock As New HistoryBlock
                AddHandler historyBlock.History, AddressOf OnHistory
                historyBlock.Load(rics, _histFields.AllNames, GroupDate.AddDays(-10), GroupDate)
            End If
        End Sub


        Private Sub OnHistory(ByVal obj As HistoryBlock.DataCube)
            If obj Is Nothing Then
                GroupDate = Today
            Else
                ' doing some cleanup
                For Each elem In AllElements
                    elem.QuotesAndYields.Clear()
                Next
                ' parsing historical data
                For Each ric In obj.Rics
                    ParseHistory(ric, obj.RicData2(ric))
                Next
            End If
        End Sub

        Private Sub ParseHistory(ByVal ric As String, ByVal rawData As Dictionary(Of String, Dictionary(Of Date, String)))
            If rawData Is Nothing Then
                Logger.Error("No data on bond {0}", ric)
                Return
            End If
            Dim bonds = (From elem In AllElements Where elem.MetaData.RIC = ric)
            If Not bonds.Any Then
                Logger.Warn("Instrument {0} does not belong to serie {1}", ric, Name)
                Return
            End If
            Dim bond = bonds.First()

            Dim fieldsDescription As FieldsDescription = _histFields.Fields
            If rawData.ContainsKey(fieldsDescription.Last) Then
                ParseHistoricalItem(rawData, fieldsDescription.Last, bond)
            End If
            If rawData.ContainsKey(fieldsDescription.Bid) Or rawData.ContainsKey(fieldsDescription.Ask) Then
                Dim bidData = ParseHistoricalItem(rawData, fieldsDescription.Bid, bond)
                Dim askData = ParseHistoricalItem(rawData, fieldsDescription.Ask, bond)
                If bidData IsNot Nothing AndAlso askData IsNot Nothing AndAlso bidData.Item1 = askData.Item1 Then
                    Dim mid = (bidData.Item2 + askData.Item2) / 2
                    HandleNewQuote(bond, _histFields.XmlName(_histFields.Fields.Mid), mid, bidData.Item1)
                ElseIf (bidData IsNot Nothing Or askData IsNot Nothing) And Not SettingsManager.Instance.MidIfBoth Then
                    If bidData IsNot Nothing Then
                        HandleNewQuote(bond, _histFields.XmlName(_histFields.Fields.Mid), bidData.Item2, bidData.Item1)
                    Else
                        HandleNewQuote(bond, _histFields.XmlName(_histFields.Fields.Mid), askData.Item2, askData.Item1)
                    End If
                End If
            End If
        End Sub


        Private Function ParseHistoricalItem(ByVal rawData As Dictionary(Of String, Dictionary(Of Date, String)), ByVal field As String, ByVal bond As Bond) As Tuple(Of Date, Double)
            Dim datVal = rawData(field)
            Dim dates = (From key In datVal.Keys Where IsNumeric(datVal(key))).ToList()
            If dates.Any Then
                Dim maxdate = dates.Max
                HandleNewQuote(bond, _histFields.XmlName(field), datVal(maxdate), maxdate)
                Return Tuple.Create(maxdate, CDbl(rawData(field)(maxdate)))
            End If
            Return Nothing
        End Function

        Private Sub OnQuotes(ByVal data As Dictionary(Of String, Dictionary(Of String, Double))) Handles _quoteLoader.NewData
            Logger.Trace("QuoteLoaderOnNewData()")
            For Each instrAndFields As KeyValuePair(Of String, Dictionary(Of String, Double)) In data
                Try
                    Dim instrument As String = instrAndFields.Key
                    Dim fieldsAndValues As Dictionary(Of String, Double) = instrAndFields.Value

                    ' checking if this bond is allowed to show up
                    Dim bonds = (From elem In _elements Where elem.MetaData.RIC = instrument)
                    If Not bonds.Any Then
                        Logger.Warn("Instrument {0} does not belong to current serie", instrument)
                        Continue For
                    End If

                    ' now update data point
                    Dim bond = bonds.First()

                    If fieldsAndValues.ContainsKey(_bondFields.Fields.Volume) Then
                        bond.TodayVolume = fieldsAndValues(_bondFields.Fields.Volume)
                        RaiseEvent Volume(bond)
                    End If

                    For Each fieldName In fieldsAndValues.Keys
                        If _bondFields.IsPriceByName(fieldName) AndAlso fieldsAndValues(fieldName) > 0 Then
                            Dim fieldValue = fieldsAndValues(fieldName)
                            Try
                                HandleNewQuote(bond, _bondFields.XmlName(fieldName), fieldValue, Date.Today)
                                Dim bid = _bondFields.Fields.Bid
                                Dim ask = _bondFields.Fields.Ask
                                If fieldName.Belongs(bid, ask) Then
                                    Dim bidPrice As Double
                                    Dim xmlBid = _bondFields.XmlName(bid)
                                    If bond.QuotesAndYields.Has(xmlBid) Then
                                        bidPrice = bond.QuotesAndYields(xmlBid).Price
                                    End If
                                    Dim askPrice As Double
                                    Dim xmlAsk = _bondFields.XmlName(ask)
                                    If bond.QuotesAndYields.Has(xmlAsk) Then
                                        askPrice = bond.QuotesAndYields(xmlAsk).Price
                                    End If
                                    Dim midPrice As Double
                                    If bidPrice > 0 And askPrice > 0 Then
                                        midPrice = (bidPrice + askPrice) / 2
                                    ElseIf Not SettingsManager.Instance.MidIfBoth Then
                                        If bidPrice > 0 Then
                                            midPrice = bidPrice
                                        ElseIf askPrice > 0 Then
                                            midPrice = askPrice
                                        End If
                                    End If

                                    If midPrice > 0 Then HandleNewQuote(bond, _bondFields.XmlName(_bondFields.Fields.Mid), midPrice, Date.Today)
                                End If
                            Catch ex As Exception
                                Logger.WarnException("Failed to plot the point", ex)
                                Logger.Warn("Exception = {0}", ex.ToString())
                            End Try
                        End If
                    Next
                Catch ex As Exception
                    Logger.WarnException("Got exception", ex)
                    Logger.Warn("Exception = {0}", ex.ToString())
                End Try
            Next
        End Sub

        Protected Sub HandleNewQuote(ByRef bond As Bond, ByVal xmlName As String, ByVal fieldVal As Double?, ByVal calcDate As Date, Optional _
                                        ByVal recalc As Boolean = True)
            If Not bond.QuotesAndYields.Contains(xmlName) Then
                Dim descr As New BondPointDescription(xmlName)
                descr.BackColor = _bondFields.Fields.BackColor(xmlName)
                descr.MarkerStyle = _bondFields.Fields.MarkerStyle(xmlName)
                descr.ParentBond = bond
                descr.Yield(calcDate) = fieldVal ' we won't calc right here

                bond.QuotesAndYields(xmlName) = descr
            Else
                bond.QuotesAndYields(xmlName).Yield(calcDate) = fieldVal
            End If

            If recalc Then Recalculate()
        End Sub

        Public Function HasRic(ByVal instrument As String) As Boolean
            Return _elements.Any(Function(elem) elem.MetaData.RIC = instrument)
        End Function

        Public Overridable Sub AddRics(ByVal rics As IEnumerable(Of String))
            For Each ric In rics
                Dim descr = BondsData.Instance.GetBondInfo(ric)
                If descr IsNot Nothing Then
                    Dim bond = New Bond(Me, descr)
                    AddHandler bond.Changed, Sub() If Not _eventsFrozen Then RecalculateTotal()
                    AddHandler bond.CustomPrice, AddressOf OnCustomCustomPrice
                    _elements.Add(bond)
                Else
                    Logger.Error("No description for bond {0} found", ric)
                End If
            Next
        End Sub

        Protected Sub OnCustomCustomPrice(ByVal bond As Bond, ByVal price As Double)
            HandleNewQuote(bond, _bondFields.XmlName(bond.Fields.Custom), price, Today)
        End Sub

        Protected Sub New(ByVal ansamble As Ansamble, fields As FieldSet)
            _ansamble = ansamble
            _histFields = fields.History.AsContainer()
            _bondFields = fields.Realtime.AsContainer()
        End Sub

        Public Sub Disable(ByVal ric As String) Implements IChangeable.Disable
            FreezeEvents()
            For Each item In (From elem In _elements Where elem.MetaData.RIC = ric)
                item.Enabled = False
            Next
            UnfreezeEvents()
        End Sub

        Public Sub Disable(ByVal rics As List(Of String)) Implements IChangeable.Disable
            FreezeEvents()
            For Each ric In rics
                Dim rc = ric
                For Each item In (From elem In _elements Where elem.MetaData.RIC = rc)
                    item.Enabled = False
                Next
            Next
            UnfreezeEvents()
        End Sub

        Public Sub Enable(ByVal rics As List(Of String)) Implements IChangeable.Enable
            FreezeEvents()
            For Each item In (From elem In _elements Where rics.Contains(elem.MetaData.RIC))
                item.Enabled = True
            Next
            UnfreezeEvents()
        End Sub

        Public WriteOnly Property LabelsOn As Boolean
            Set(ByVal value As Boolean)
                FreezeEvents()
                For Each elem In _elements
                    elem.LabelEnabled = value
                Next
                UnfreezeEvents()
            End Set
        End Property

        Public Sub ToggleLabels()
            FreezeEvents()
            For Each elem In _elements
                elem.ToggleLabel()
            Next
            UnfreezeEvents()
        End Sub

        Public Sub SetLabelMode(ByVal mode As LabelMode)
            FreezeEvents()
            For Each elem In _elements
                elem.LabelMode = mode
            Next
            UnfreezeEvents()
        End Sub

        Public Function Bonds(Optional ByVal clause As Func(Of Bond, Boolean) = Nothing) As IEnumerable(Of Bond)
            If clause IsNot Nothing Then
                Return From elem In _elements Where clause(elem)
            Else
                Return _elements
            End If
        End Function

        Public Function Bonds(ByVal clause As Func(Of BondMetadata, Boolean)) As IEnumerable(Of Bond)
            Return From elem In _elements Where clause(elem.MetaData)
        End Function

        Public Sub SetYieldMode(ByVal mode As String)
            FreezeEvents()
            _elements.ForEach(Sub(elem) elem.YieldMode = mode)
            UnfreezeEvents()
        End Sub
    End Class
End Namespace