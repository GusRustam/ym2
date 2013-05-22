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
    Public MustInherit Class BaseGroup
        Inherits Identifyable
        Protected Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(BaseGroup))

        Private ReadOnly _ansamble As Ansamble

        Public Event Quote As Action(Of Bond)
        Public Event RemovedItem As Action(Of BaseGroup, String)
        Public Event Clear As Action(Of BaseGroup)
        Public Event Volume As Action(Of Bond)

        Public YieldMode As String ' todo currently unused
        Friend BondFields As FieldContainer
        Public SeriesName As String

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

        Public PortfolioID As Long

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

        Public ReadOnly Property Ansamble() As Ansamble
            Get
                Return _ansamble
            End Get
        End Property

        Public Sub Cleanup()
            _quoteLoader.CancelAll()
            _elements.Clear()
            RaiseEvent Clear(Me)
        End Sub

        Public Overridable Sub StartAll()
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
                        Logger.Warn("Instrument {0} does not belong to serie {1}", instrument, SeriesName)
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
                                HandleQuote(bond, BondFields.XmlName(fieldName), fieldValue, Date.Today)
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

                                    If midPrice > 0 Then HandleQuote(bond, BondFields.XmlName(BondFields.Fields.Mid), midPrice, Date.Today)
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

        Protected Sub HandleQuote(ByRef bondDataPoint As Bond, ByVal xmlName As String, ByVal fieldVal As Double?, ByVal calcDate As Date)
            Dim calculation As New BondPointDescription
            'Dim xmlName = BondFields.XmlName(fieldName)
            calculation.BackColor = BondFields.Fields.BackColor(xmlName)
            calculation.MarkerStyle = BondFields.Fields.MarkerStyle(xmlName)
            calculation.Price = fieldVal
            CalculateYields(calcDate, bondDataPoint.MetaData, calculation) ' todo add userDefinedSpread

            bondDataPoint.QuotesAndYields(xmlName) = calculation
            NotifyQuote(bondDataPoint)
        End Sub

        Public Function HasRic(ByVal instrument As String) As Boolean
            Return _elements.Any(Function(elem) elem.MetaData.RIC = instrument)
        End Function

        Public Sub AddRics(ByVal rics As IEnumerable(Of String))
            For Each ric In rics
                Dim descr = BondsData.Instance.GetBondInfo(ric)
                If descr IsNot Nothing Then
                    _elements.Add(New Bond(Me, descr))
                Else
                    Logger.Error("No description for bond {0} found", ric)
                End If
            Next
        End Sub

        Protected Sub New(ByVal ansamble As Ansamble)
            _ansamble = ansamble
        End Sub

        Friend Function GetBond(ByVal ric As String) As Bond
            Dim bonds = (From item In _elements Where item.MetaData.RIC = ric).ToList()
            Return If(bonds.Any, bonds.First, Nothing)
        End Function

        Public Overridable Sub NotifyQuote(ByVal bond As Bond)
            RaiseEvent Quote(bond)
        End Sub

        Public Overridable Sub NotifyRemoved(ByVal bond As Bond)
            RaiseEvent RemovedItem(Me, bond.MetaData.RIC)
        End Sub

        Public Sub Annihilate()
            _elements.Clear()
            _ansamble.BondCurves.Remove(Identity)
            _ansamble.Groups.Remove(Identity)
            RaiseEvent Clear(Me)
        End Sub

        Public Sub Disable(ByVal ric As String)
            For Each item In (From elem In _elements Where elem.MetaData.RIC = ric)
                item.Enabled = False
                NotifyRemoved(item)
            Next
        End Sub

        Public Sub Disable(ByVal rics As List(Of String))
            For Each item In (From elem In _elements Where rics.Contains(elem.MetaData.RIC))
                item.Enabled = False
                NotifyRemoved(item)
            Next
        End Sub

        Public Sub Enable(ByVal ric As String)
            For Each item In (From elem In _elements Where elem.MetaData.RIC = ric)
                item.Enabled = True
                NotifyQuote(item)
            Next
        End Sub

        Public Sub Enable(ByVal rics As List(Of String))
            For Each item In (From elem In _elements Where rics.Contains(elem.MetaData.RIC))
                item.Enabled = True
                NotifyQuote(item)
            Next
        End Sub
    End Class
End NameSpace