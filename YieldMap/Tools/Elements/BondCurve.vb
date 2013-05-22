Imports System.ComponentModel
Imports AdfinXAnalyticsFunctions
Imports DbManager
Imports Settings
Imports Uitls
Imports YieldMap.Tools.Estimation
Imports ReutersData

Namespace Tools.Elements
    Public Class BondCurve
        Inherits BaseGroup

        Public MustInherit Class CurveItem
            Implements IComparable(Of CurveItem)
            Private ReadOnly _x As Double
            Private ReadOnly _y As Double

            Public ReadOnly Property X() As String
                Get
                    Return String.Format("{0:F2}", _x)
                End Get
            End Property

            Public ReadOnly Property Y() As String
                Get
                    Return String.Format("{0:P2}", _y)
                End Get
            End Property

            <Browsable(False)>
            Public ReadOnly Property TheX() As Double
                Get
                    Return _x
                End Get
            End Property

            <Browsable(False)>
            Public ReadOnly Property TheY() As Double
                Get
                    Return _y
                End Get
            End Property

            Public Function CompareTo(ByVal other As CurveItem) As Integer Implements IComparable(Of CurveItem).CompareTo
                Return _x.CompareTo(other._x)
            End Function

            Public Sub New(ByVal x As Double, ByVal y As Double)
                _x = x
                _y = y
            End Sub
        End Class

        Public Class BondCurveItem
            Inherits CurveItem
            Private ReadOnly _bond As Bond

            <Browsable(False)>
            Public ReadOnly Property Bond() As Bond
                Get
                    Return _bond
                End Get
            End Property

            Public ReadOnly Property Ric() As String
                Get
                    Return _bond.MetaData.RIC
                End Get
            End Property

            Public Sub New(ByVal x As Double, ByVal y As Double, ByVal bond As Bond)
                MyBase.new(x, y)
                _bond = bond
            End Sub

        End Class

        Public Class PointCurveItem
            Inherits CurveItem
            Private ReadOnly _curve As BondCurve

            <Browsable(False)>
            Public ReadOnly Property Curve() As BondCurve
                Get
                    Return _curve
                End Get
            End Property

            Public Sub New(ByVal x As Double, ByVal y As Double, ByVal curve As BondCurve)
                MyBase.New(x, y)
                _curve = curve
            End Sub
        End Class

        Public Class BondCurveSnapshot
            Public Class BondCurveElement
                Implements IComparable(Of BondCurveElement)
                Private ReadOnly _ric As String
                Private ReadOnly _descr As String
                Private ReadOnly _yield As Double
                Private ReadOnly _duration As Double
                Private ReadOnly _price As Double
                Private ReadOnly _quote As String

                Public Sub New(ByVal ric As String, ByVal descr As String, ByVal [yield] As Double, ByVal duration As Double, ByVal price As Double, ByVal quote As String)
                    _ric = ric
                    _descr = descr
                    _yield = yield
                    _duration = duration
                    _price = price
                    _quote = quote
                End Sub

                Public ReadOnly Property RIC() As String
                    Get
                        Return _ric
                    End Get
                End Property

                Public ReadOnly Property Descr() As String
                    Get
                        Return _descr
                    End Get
                End Property

                Public ReadOnly Property Yield() As String
                    Get
                        Return String.Format("{0:P2}", _yield)
                    End Get
                End Property

                Public ReadOnly Property Duration() As String
                    Get
                        Return String.Format("{0:F2}", _duration)
                    End Get
                End Property

                Public ReadOnly Property Price() As String
                    Get
                        Return String.Format("{0:F4}", _price)
                    End Get
                End Property

                Public ReadOnly Property Quote() As String
                    Get
                        Return _quote
                    End Get
                End Property

                Public Function CompareTo(ByVal other As BondCurveElement) As Integer Implements IComparable(Of BondCurveElement).CompareTo
                    Return _duration.CompareTo(other._duration)
                End Function
            End Class

            Private ReadOnly _current As List(Of CurveItem)
            Public ReadOnly Property Current() As List(Of CurveItem)
                Get
                    Return _current
                End Get
            End Property

            Private ReadOnly _disabledElements As New List(Of BondCurveElement)
            Public ReadOnly Property DisabledElements() As List(Of BondCurveElement)
                Get
                    Return _disabledElements
                End Get
            End Property

            Private ReadOnly _enabledElements As New List(Of BondCurveElement)
            Public ReadOnly Property EnabledElements() As List(Of BondCurveElement)
                Get
                    Return _enabledElements
                End Get
            End Property

            Public Sub New(ByVal bonds As List(Of Bond), ByVal items As List(Of CurveItem))
                For Each bond In bonds
                    Dim mainQuote = bond.QuotesAndYields.Main
                    If mainQuote Is Nothing Then Continue For
                    If bond.Enabled Then
                        _enabledElements.Add(New BondCurveElement(bond.MetaData.RIC, bond.Label, mainQuote.GetYield(), mainQuote.Duration, mainQuote.Price, bond.QuotesAndYields.MaxPriorityField))
                    Else
                        _disabledElements.Add(New BondCurveElement(bond.MetaData.RIC, bond.Label, mainQuote.GetYield(), mainQuote.Duration, mainQuote.Price, bond.QuotesAndYields.MaxPriorityField))
                    End If
                Next
                _enabledElements.Sort()
                _disabledElements.Sort()
                _current = New List(Of CurveItem)(items)
            End Sub
        End Class

        Private _date As Date = Today
        Public Property [Date]() As Date
            Get
                Return _date
            End Get
            Set(ByVal value As Date)
                If _date <> value Then
                    _date = value
                    StartAll()
                End If
            End Set
        End Property

        Public Overrides Sub StartAll()
            Dim rics As List(Of String) = (From elem In AllElements Select elem.MetaData.RIC).ToList()
            If rics.Count = 0 Then Return
            If _date = Today Then
                QuoteLoader.AddItems(rics, BondFields.AllNames)
            Else
                QuoteLoader.CancelAll()
                AllElements.ForEach(Sub(elem) elem.QuotesAndYields.Clear())
                For Each ric In rics
                    Dim histLoader As New History
                    AddHandler histLoader.HistoricalData, AddressOf OnHistoricalData
                    histLoader.StartTask(ric, String.Join(",", _histFields.AllNames), _date, _date)
                Next
            End If
        End Sub

        Public Event Updated As Action(Of List(Of CurveItem))

        Private ReadOnly _histFields As FieldContainer

        ' Last curve snapshot
        Private _lastCurve As List(Of CurveItem)

        Private _formula As String
        Public ReadOnly Property Formula() As String
            Get
                Return _formula
            End Get
        End Property

        Private _bootstrapped As Boolean
        Public Property Bootstrapped() As Boolean
            Get
                Return _bootstrapped
            End Get
            Set(ByVal value As Boolean)
                _bootstrapped = value
                NotifyQuote(Nothing)
            End Set
        End Property

        Private _estModel As EstimationModel
        Public Property EstModel() As EstimationModel
            Get
                Return _estModel
            End Get
            Set(ByVal value As EstimationModel)
                _estModel = value
                NotifyQuote(Nothing)
            End Set
        End Property

        Public Sub New(ByVal ansamble As Ansamble, ByVal src As Source)
            MyBase.new(ansamble)

            SeriesName = src.Name
            PortfolioID = src.ID
            BondFields = src.Fields.Realtime.AsContainer()
            Color = src.Color
            _histFields = src.Fields.History.AsContainer()

            YieldMode = SettingsManager.Instance.YieldCalcMode
            AddRics(src.GetDefaultRics())
        End Sub

        Private Sub OnHistoricalData(ByVal ric As String, ByVal data As Dictionary(Of Date, HistoricalItem), ByVal rawData As Dictionary(Of DateTime, RawHistoricalItem))
            If rawData Is Nothing Then
                Logger.Error("No data on bond {0}", ric)
                Return
            End If
            Dim bonds = (From elem In AllElements Where elem.MetaData.RIC = ric)
            If Not bonds.Any Then
                Logger.Warn("Instrument {0} does not belong to serie {1}", ric, SeriesName)
                Return
            End If
            Dim bond = bonds.First()

            If Not rawData.ContainsKey(_date) Then
                Logger.Warn("Instrument {0} has no necessary date {1:dd/MM/yyyy}", ric, _date)
                Return
            End If
            Dim quote = rawData(_date)
            Dim fieldsDescription As FieldsDescription = _histFields.Fields
            If quote.Has(fieldsDescription.Last) Then
                HandleQuote(bond, _histFields.XmlName(fieldsDescription.Last), quote(fieldsDescription.Last), _date)
            End If
            If quote.Has(fieldsDescription.Bid) Or quote.Has(fieldsDescription.Ask) Then
                If quote.Has(fieldsDescription.Bid) And quote.Has(fieldsDescription.Ask) Then
                    Dim bid = CDbl(quote(fieldsDescription.Bid))
                    HandleQuote(bond, _histFields.XmlName(fieldsDescription.Bid), bid, _date)
                    Dim ask = CDbl(quote(fieldsDescription.Ask))
                    HandleQuote(bond, _histFields.XmlName(fieldsDescription.Mid), ask, _date)
                    Dim mid = (bid + ask) / 2
                    HandleQuote(bond, _histFields.XmlName(fieldsDescription.Mid), mid, _date)
                ElseIf quote.Has(fieldsDescription.Bid) Then
                    Dim bid = CDbl(quote(fieldsDescription.Bid))
                    HandleQuote(bond, _histFields.XmlName(fieldsDescription.Bid), bid, _date)
                    If Not SettingsManager.Instance.MidIfBoth Then
                        HandleQuote(bond, _histFields.XmlName(fieldsDescription.Mid), bid, _date)
                    End If
                ElseIf quote.Has(fieldsDescription.Ask) Then
                    Dim ask = CDbl(quote(fieldsDescription.Ask))
                    HandleQuote(bond, _histFields.XmlName(fieldsDescription.Mid), ask, _date)
                    If Not SettingsManager.Instance.MidIfBoth Then
                        HandleQuote(bond, _histFields.XmlName(fieldsDescription.Mid), ask, _date)
                    End If
                End If
            End If
        End Sub

        Public Overrides Sub NotifyQuote(ByVal bond As Bond)
            If Ansamble.YSource = YSource.Yield Then
                Dim result As New List(Of CurveItem)
                If _bootstrapped Then
                    Try
                        ' todo use date of curve instead of today
                        Dim data = (From elem In Elements
                                Where elem.MetaData.IssueDate <= Today And
                                      elem.MetaData.Maturity > Today And
                                      elem.QuotesAndYields.Any()).ToList()

                        Dim params(0 To data.Count() - 1, 5) As Object
                        For i = 0 To data.Count - 1
                            Dim meta = data(i).MetaData
                            params(i, 0) = "B"
                            params(i, 1) = Today ' todo date
                            params(i, 2) = meta.Maturity
                            params(i, 3) = meta.GetCouponByDate(Today) ' todo date
                            params(i, 4) = data(i).QuotesAndYields.Main.Price / 100.0 ' todo local fields priorities???
                            params(i, 5) = meta.PaymentStructure
                        Next
                        Dim curveModule = New AdxYieldCurveModule

                        Dim termStructure As Array = curveModule.AdTermStructure(params, "RM:YC ZCTYPE:RATE IM:CUBX ND:DIS", Nothing)
                        For i = termStructure.GetLowerBound(0) To termStructure.GetUpperBound(0)
                            Dim matDate = Utils.FromExcelSerialDate(termStructure.GetValue(i, 1))
                            Dim dur = (matDate - Today).TotalDays / 365.0 ' todo date
                            Dim yld = termStructure.GetValue(i, 2)
                            If dur > 0 And yld > 0 Then
                                result.Add(New PointCurveItem(dur, yld, Me))
                            End If
                        Next
                    Catch ex As Exception
                        Logger.ErrorException("Failed to bootstrap", ex)
                        Logger.Error("Exception = {0}", ex.ToString())
                        Return
                    End Try
                Else
                    For Each bnd In Elements
                        Dim x As Double, y As Double
                        Dim description = bnd.QuotesAndYields.Main
                        If description Is Nothing Then Continue For

                        Select Case Ansamble.XSource
                            Case XSource.Duration
                                x = description.Duration
                            Case XSource.Maturity
                                x = (bnd.MetaData.Maturity.Value - Date.Today).Days / 365
                        End Select

                        y = description.GetYield()
                        If x > 0 And y > 0 Then result.Add(New BondCurveItem(x, y, bnd))
                    Next
                End If
                result.Sort()

                If _estModel IsNot Nothing Then
                    Dim est As New Estimator(_estModel)
                    _formula = est.GetFormula()
                    Dim tmp = New List(Of CurveItem)(result)
                    Dim list As List(Of XY) = (From item In tmp Select New XY(item.TheX, item.TheY)).ToList()
                    Dim apprXY = est.Approximate(list)
                    result = (From item In apprXY Select New PointCurveItem(item.X, item.Y, Me)).Cast(Of CurveItem).ToList()
                End If

                _lastCurve = New List(Of CurveItem)(result)
                RaiseEvent Updated(result)
            ElseIf Ansamble.YSource.Belongs(YSource.ASWSpread, YSource.OASpread, YSource.ZSpread, YSource.PointSpread) Then
                ' todo plotting spreads
            Else
                Logger.Warn("Unknown spread type {0}", Ansamble.YSource)
            End If
        End Sub

        Public Overrides Sub NotifyRemoved(ByVal bond As Bond)
            NotifyQuote(Nothing)
        End Sub

        Public Sub Bootstrap()
            Bootstrapped = Not Bootstrapped
        End Sub

        Public Function GetSnapshot() As BondCurveSnapshot
            Return New BondCurveSnapshot(AllElements, _lastCurve)
        End Function

        Public Sub SetFitMode(ByVal mode As String)
            Dim model = EstimationModel.FromName(mode)
            EstModel = If(model Is Nothing OrElse (EstModel IsNot Nothing AndAlso EstModel = model), Nothing, model)
        End Sub

    End Class
End NameSpace