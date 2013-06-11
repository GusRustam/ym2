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
        Public Event Updated As Action(Of List(Of CurveItem))
        Public Event UpdatedSpread As Action(Of List(Of CurveItem), IOrdinate)

        Public MustOverride Sub Recalculate() Implements IChangeable.Recalculate
        Public MustOverride Sub RecalculateTotal() Implements IChangeable.RecalculateTotal
        Public MustOverride Sub Recalculate(ByVal ord As IOrdinate) Implements IChangeable.Recalculate
        'Protected MustOverride Sub UpdateSpreads()


        Public YieldMode As String ' todo currently unused

        Protected Nm As String
        Public ReadOnly Property Name() As String Implements INamed.Name
            Get
                Return Nm
            End Get
        End Property

        Friend BondFields As FieldContainer
        Public PortfolioID As Long


        Private ReadOnly _ansamble As Ansamble
        Public ReadOnly Property Ansamble() As Ansamble
            Get
                Return _ansamble
            End Get
        End Property

        Private _eventsFrozen As Boolean = False

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

        Protected Overridable Sub NotifyUpdated(ByVal curveItems As List(Of CurveItem))
            If Not _eventsFrozen Then RaiseEvent Updated(curveItems)
        End Sub

        Protected Sub NotifyUpdatedSpread(ByVal curveItems As List(Of CurveItem), ord As IOrdinate)
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

        Public ReadOnly Property DisabledElements() As List(Of Bond)
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
            Dim rics As List(Of String) = (From elem In _elements Select elem.MetaData.RIC).ToList()
            If rics.Count = 0 Then Return
            _quoteLoader.AddItems(rics, BondFields.AllNames)
        End Sub

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

                    If fieldsAndValues.ContainsKey(BondFields.Fields.Volume) Then
                        bond.TodayVolume = fieldsAndValues(BondFields.Fields.Volume)
                        RaiseEvent Volume(bond)
                    End If

                    For Each fieldName In fieldsAndValues.Keys
                        If BondFields.IsPriceByName(fieldName) AndAlso fieldsAndValues(fieldName) > 0 Then
                            Dim fieldValue = fieldsAndValues(fieldName)
                            Try
                                HandleNewQuote(bond, BondFields.XmlName(fieldName), fieldValue, Date.Today)
                                Dim bid = BondFields.Fields.Bid
                                Dim ask = BondFields.Fields.Ask
                                If fieldName.Belongs(bid, ask) Then
                                    Dim bidPrice As Double
                                    Dim xmlBid = BondFields.XmlName(bid)
                                    If bond.QuotesAndYields.Has(xmlBid) Then
                                        bidPrice = bond.QuotesAndYields(xmlBid).Price
                                    End If
                                    Dim askPrice As Double
                                    Dim xmlAsk = BondFields.XmlName(ask)
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

                                    If midPrice > 0 Then HandleNewQuote(bond, BondFields.XmlName(BondFields.Fields.Mid), midPrice, Date.Today)
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
                descr.BackColor = BondFields.Fields.BackColor(xmlName)
                descr.MarkerStyle = BondFields.Fields.MarkerStyle(xmlName)
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

        Public Sub AddRics(ByVal rics As IEnumerable(Of String))
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

        Private Sub OnCustomCustomPrice(ByVal bond As Bond, ByVal price As Double)
            HandleNewQuote(bond, BondFields.XmlName(bond.Fields.Custom), price, Today)
        End Sub

        Protected Sub New(ByVal ansamble As Ansamble)
            _ansamble = ansamble
        End Sub

        Public Sub Disable(ByVal ric As String)
            FreezeEvents()
            For Each item In (From elem In _elements Where elem.MetaData.RIC = ric)
                item.Enabled = False
            Next
            UnfreezeEvents()
        End Sub

        Public Sub Disable(ByVal rics As List(Of String))
            FreezeEvents()
            For Each item In (From elem In _elements Where rics.Contains(elem.MetaData.RIC))
                item.Enabled = False
            Next
            UnfreezeEvents()
        End Sub

        Public Sub Enable(ByVal ric As String)
            FreezeEvents()
            For Each item In (From elem In _elements Where elem.MetaData.RIC = ric)
                item.Enabled = True
            Next
            UnfreezeEvents()
        End Sub

        Public Sub Enable(ByVal rics As List(Of String))
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
    End Class
End Namespace