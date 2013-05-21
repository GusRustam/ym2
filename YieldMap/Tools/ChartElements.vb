Imports System.Drawing
Imports System.Reflection
Imports AdfinXAnalyticsFunctions
Imports System.ComponentModel
Imports DbManager
Imports DbManager.Bonds
Imports YieldMap.Tools.Estimation
Imports NLog
Imports ReutersData
Imports Settings
Imports Uitls
Imports YieldMap.Forms.TableForm
Imports YieldMap.Curves

Namespace Tools
#Region "I. Enumerations"
    'Public Enum QuoteSource
    '    Bid
    '    Ask
    '    Last
    '    Hist
    'End Enum

    Public Enum XSource
        Duration
        Maturity
    End Enum
#End Region

#Region "II. Groups and ansamble"
    Public MustInherit Class Identifyable
        Implements IEquatable(Of Identifyable)
        Private ReadOnly _identity As Long = Ansamble.GenerateID()

        Public ReadOnly Property Identity() As Long
            Get
                Return _identity
            End Get
        End Property

        Public Overloads Function Equals(ByVal other As Identifyable) As Boolean Implements IEquatable(Of Identifyable).Equals
            If ReferenceEquals(Nothing, other) Then Return False
            If ReferenceEquals(Me, other) Then Return True
            Return _identity = other._identity
        End Function

        Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
            If ReferenceEquals(Nothing, obj) Then Return False
            If ReferenceEquals(Me, obj) Then Return True
            If obj.GetType IsNot Me.GetType Then Return False
            Return Equals(DirectCast(obj, Identifyable))
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return _identity.GetHashCode
        End Function

        Public Shared Operator =(ByVal left As Identifyable, ByVal right As Identifyable) As Boolean
            Return Equals(left, right)
        End Operator

        Public Shared Operator <>(ByVal left As Identifyable, ByVal right As Identifyable) As Boolean
            Return Not Equals(left, right)
        End Operator

        Protected Overrides Sub Finalize()
            Ansamble.ReleaseID(_identity)
        End Sub
    End Class

    Friend Class InternalSpreadContainer
        Private _benchmarks As New Dictionary(Of SpreadType, BondCurve)
    End Class

    Public Class GroupContainer(Of T As BaseGroup)
        Private ReadOnly _groups As New Dictionary(Of Long, T)

        Public Event RemovedItem As Action(Of T, String)
        Public Event Quote As Action(Of Bond)
        Public Event Volume As Action(Of Bond)
        Public Event Cleared As Action(Of T)

        Default Public ReadOnly Property Data(ByVal id As Long) As T
            Get
                Return _groups(id)
            End Get
        End Property

        Public ReadOnly Property AsTable() As List(Of BondDescr)
            Get
                Dim result As New List(Of BondDescr)
                For Each grp In From kvp In _groups Select kvp.Value
                    grp.Elements.Keys.ToList().ForEach(
                        Sub(elem)
                            Dim res As New BondDescr
                            Dim point = grp.Elements(elem)
                            res.RIC = point.MetaData.RIC
                            res.Name = point.MetaData.ShortName
                            res.Maturity = point.MetaData.Maturity
                            res.Coupon = point.MetaData.Coupon
                            Dim fieldName = point.QuotesAndYields.MaxPriorityField
                            Dim quote = point.QuotesAndYields(fieldName)
                            If quote IsNot Nothing Then
                                res.Price = quote.Price
                                res.Quote = fieldName
                                res.QuoteDate = quote.YieldAtDate
                                res.State = BondDescr.StateType.Ok
                                res.ToWhat = quote.Yld.ToWhat
                                res.BondYield = quote.Yld.Yield
                                res.CalcMode = BondDescr.CalculationMode.SystemPrice
                                res.Convexity = quote.Convexity
                                res.Duration = quote.Duration
                                res.Live = quote.YieldAtDate = Date.Today
                            End If
                            result.Add(res)
                        End Sub)
                Next
                Return result
            End Get
        End Property

        Public Function Exists(ByVal id As Long) As Boolean
            Return _groups.Keys.Contains(id)
        End Function

        Public Sub Cleanup()
            For Each kvp In _groups
                kvp.Value.Cleanup()
            Next
            _groups.Clear()
        End Sub

        Public Sub Start()
            For Each kvp In _groups
                kvp.Value.StartAll()
            Next
        End Sub

        Public Sub Add(ByVal group As T)
            _groups.Add(group.Identity, group)
            AddHandler group.Clear, Sub(base As BaseGroup) RaiseEvent Cleared(base)
            AddHandler group.Quote, Sub(bond As Bond) RaiseEvent Quote(bond)
            AddHandler group.RemovedItem, Sub(grp As BaseGroup, ric As String) RaiseEvent RemovedItem(grp, ric)
            AddHandler group.Volume, Sub(bond As Bond) RaiseEvent Volume(bond)
        End Sub

        Public Sub Remove(ByVal id As Long)
            _groups.Remove(id)
        End Sub

        Public Function FindBond(ByVal ric As String) As Bond ' todo switch to ids instead of ric I believe?
            For Each kvp In From elem In _groups Where elem.Value.HasRic(ric)
                Return kvp.Value.GetBond(ric)
            Next
            Return Nothing
        End Function
    End Class

    Public Class Ansamble
        Private Shared ReadOnly Identities As New HashSet(Of Long)

        Public Shared Sub ReleaseID(ByVal id As Long)
            Identities.Remove(id)
        End Sub

        Public Shared Function GenerateID() As Long
            Dim rnd = New Random()
            Dim num As Integer
            Do
                num = CLng(Math.Round((89.9999 * rnd.NextDouble() + 10) * 10000))
            Loop While Identities.Contains(num)
            Identities.Add(num)
            Return num
        End Function

        Private _xSource As XSource
        Public Property XSource() As XSource
            Get
                Return _xSource
            End Get
            Set(ByVal value As XSource)
                _xSource = value
                ' todo recalculating, guys
            End Set
        End Property

        Private _chartSpreadType As SpreadType = SpreadType.Yield
        Public Property ChartSpreadType() As SpreadType
            Get
                Return _chartSpreadType
            End Get
            Set(ByVal value As SpreadType)
                _chartSpreadType = value
                ' todo recalculating, guys
            End Set
        End Property

        Private WithEvents _groups As New GroupContainer(Of Group)
        Public ReadOnly Property Groups As GroupContainer(Of Group)
            Get
                Return _groups
            End Get
        End Property

        Private WithEvents _curves As New GroupContainer(Of BondCurve)
        Public ReadOnly Property BondCurves As GroupContainer(Of BondCurve)
            Get
                Return _curves
            End Get
        End Property

        Public Sub Recalculate()
            ' todo something might have changed, I dunno what
        End Sub

        Private Sub _curves_Cleared(ByVal obj As BondCurve) Handles _curves.Cleared
            RaiseEvent CurveCleared(obj)
        End Sub

        Public Event CurveCleared As Action(Of BondCurve)
        Public Event CurveQuote As Action(Of BaseGroup)
        Public Event GroupCleared As Action(Of Group)
        Public Event BondQuote As Action(Of Bond)
        Public Event BondRemoved As Action(Of Group, String)
        Public Event BondVolume As Action(Of Bond)

        Private Sub _curves_Quote(ByVal obj As Bond) Handles _curves.Quote
            RaiseEvent CurveQuote(obj.Parent)
        End Sub

        Private Sub _groups_Cleared(ByVal obj As Group) Handles _groups.Cleared
            RaiseEvent GroupCleared(obj)
        End Sub

        Private Sub _groups_Quote(ByVal obj As Bond) Handles _groups.Quote
            RaiseEvent BondQuote(obj)
        End Sub

        Private Sub _groups_RemovedItem(ByVal arg1 As Group, ByVal arg2 As String) Handles _groups.RemovedItem
            RaiseEvent BondRemoved(arg1, arg2)
        End Sub

        Private Sub _groups_Volume(ByVal obj As Bond) Handles _groups.Volume
            RaiseEvent BondVolume(obj)
        End Sub
    End Class

    Public Class Bond
        Inherits Identifyable

        Private ReadOnly _parent As BaseGroup
        Private ReadOnly _metaData As BondDescription
        Public TodayVolume As Double

        Private _userSelectedQuote As String
        Public Property UserSelectedQuote As String
            Get
                Return _userSelectedQuote
            End Get
            Set(ByVal value As String)
                _userSelectedQuote = value
                Parent.NotifyQuote(Me)
            End Set
        End Property

        Private _usedDefinedSpread As Double

        Public Property UsedDefinedSpread() As Double
            Get
                Return _usedDefinedSpread
            End Get
            Set(ByVal value As Double)
                _usedDefinedSpread = value
                ' todo recalculate all! yields as spreads, fire event
            End Set
        End Property

        Private _labelMode As LabelMode = LabelMode.IssuerAndSeries

        Public Property LabelMode As LabelMode
            Get
                Return _labelMode
            End Get
            Set(ByVal value As LabelMode)
                _labelMode = value
            End Set
        End Property

        Sub New(ByVal parent As BaseGroup, ByVal metaData As BondDescription)
            _parent = parent
            _metaData = metaData
        End Sub

        Public ReadOnly Property Parent As BaseGroup
            Get
                Return _parent
            End Get
        End Property


        Public ReadOnly Property MetaData As BondDescription
            Get
                Return _metaData
            End Get
        End Property

        Public Class QyContainer
            Implements IEnumerable(Of String)
            Private ReadOnly _parent As Bond
            Private ReadOnly _quotesAndYields As New Dictionary(Of String, BondPointDescription)

            Sub New(ByVal parent As Bond)
                _parent = parent
            End Sub

            Default Public Property Val(ByVal key As String) As BondPointDescription
                Get
                    Return If(_quotesAndYields.ContainsKey(key), _quotesAndYields(key), Nothing)
                End Get
                Set(ByVal value As BondPointDescription)
                    _quotesAndYields(key) = value
                End Set
            End Property

            Public ReadOnly Property MaxPriorityField() As String
                Get
                    If _parent.UserSelectedQuote <> "" Then Return _parent.UserSelectedQuote
                    Dim forbiddenFields = SettingsManager.Instance.ForbiddenFields.Split(",")
                    Dim existingFields = (From item In _quotesAndYields.Keys
                                      Where _quotesAndYields(item).Price > 0 AndAlso Not forbiddenFields.Contains(item)).ToList()
                    Dim allowedFields = SettingsManager.Instance.FieldsPriority.Split(",")
                    If Not allowedFields.Any OrElse Not existingFields.Any Then Return ""

                    Dim i As Integer
                    For i = 0 To allowedFields.Count() - 1
                        If existingFields.Contains(allowedFields(i)) Then Return allowedFields(i)
                    Next
                    Return ""
                End Get
            End Property

            Public Function Has(ByVal key As String) As Boolean
                Return _quotesAndYields.ContainsKey(key)
            End Function

            Public Function IEnumerable_GetEnumerator() As IEnumerator(Of String) Implements IEnumerable(Of String).GetEnumerator
                Return _quotesAndYields.Keys.GetEnumerator()
            End Function

            Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
                Return _quotesAndYields.Keys.GetEnumerator()
            End Function

            Public ReadOnly Property Main() As BondPointDescription
                Get
                    Dim priorityField = MaxPriorityField
                    Return If(priorityField <> "", _quotesAndYields(priorityField), Nothing)
                End Get
            End Property
        End Class

        Private ReadOnly _quotesAndYields As New QyContainer(Me)
        Public ReadOnly Property QuotesAndYields As QyContainer
            Get
                Return _quotesAndYields
            End Get
        End Property

        Public ReadOnly Property Label() As String
            Get
                Dim lab As String
                Select Case LabelMode
                    Case LabelMode.IssuerAndSeries : lab = MetaData.Label1
                    Case LabelMode.IssuerCpnMat : lab = MetaData.Label2
                    Case LabelMode.Description : lab = MetaData.Label3
                    Case LabelMode.SeriesOnly : lab = MetaData.Label4
                End Select
                Label = lab
            End Get
        End Property

        Public ReadOnly Property Fields() As FieldsDescription
            Get
                Return Parent.BondFields.Fields
            End Get
        End Property

        Public Sub Annihilate()
            Parent.Elements.Remove(MetaData.RIC)
            Parent.NotifyRemoved(Me)
        End Sub
    End Class

    ' todo custom label
    Public Enum LabelMode
        IssuerAndSeries
        IssuerCpnMat
        Description
        SeriesOnly
    End Enum

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

        Private ReadOnly _elements As New Dictionary(Of String, Bond) 'ric -> datapoint
        Public ReadOnly Property Elements() As Dictionary(Of String, Bond)
            Get
                Return _elements
            End Get
        End Property

        Public PortfolioID As Long

        Private WithEvents _quoteLoader As New LiveQuotes

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

        Public Sub StartAll()
            Dim rics As List(Of String) = _elements.Keys.ToList()
            If rics.Count = 0 Then Return
            _quoteLoader.AddItems(rics, BondFields.AllNames)
        End Sub

        Public Sub SetCustomPrice(ByVal ric As String, ByVal price As Double)
            Dim bondDataPoint = _elements(ric)
            If price > 0 Then
                HandleQuote(bondDataPoint, BondFields.Fields.Custom, price, Date.Today)
                bondDataPoint.UserSelectedQuote = BondFields.Fields.Custom
            End If
        End Sub

        Private Sub OnQuotes(ByVal data As Dictionary(Of String, Dictionary(Of String, Double))) Handles _quoteLoader.NewData
            Logger.Trace("QuoteLoaderOnNewData()")
            For Each instrAndFields As KeyValuePair(Of String, Dictionary(Of String, Double)) In data
                Try
                    Dim instrument As String = instrAndFields.Key
                    Dim fieldsAndValues As Dictionary(Of String, Double) = instrAndFields.Value

                    ' checking if this bond is allowed to show up
                    If Not _elements.Keys.Contains(instrument) Then
                        Logger.Warn("Instrument {0} does not belong to serie {1}", instrument, SeriesName)
                        Continue For
                    End If

                    ' now update data point
                    Dim bondDataPoint = _elements(instrument)

                    If fieldsAndValues.ContainsKey(BondFields.Fields.Volume) Then
                        bondDataPoint.TodayVolume = fieldsAndValues(BondFields.Fields.Volume)
                        RaiseEvent Volume(bondDataPoint)
                    End If

                    For Each fieldName In fieldsAndValues.Keys
                        If BondFields.IsPriceByName(fieldName) AndAlso fieldsAndValues(fieldName) > 0 Then
                            Dim fieldValue = fieldsAndValues(fieldName)
                            Try
                                HandleQuote(bondDataPoint, fieldName, fieldValue, Date.Today)
                                Dim bid = BondFields.Fields.Bid
                                Dim ask = BondFields.Fields.Ask
                                If fieldName.Belongs(bid, ask) Then
                                    Dim bidPrice As Double
                                    Dim xmlBid = BondFields.XmlName(bid)
                                    If bondDataPoint.QuotesAndYields.Has(xmlBid) Then
                                        bidPrice = bondDataPoint.QuotesAndYields(xmlBid).Price
                                    End If
                                    Dim askPrice As Double
                                    Dim xmlAsk = BondFields.XmlName(ask)
                                    If bondDataPoint.QuotesAndYields.Has(xmlAsk) Then
                                        askPrice = bondDataPoint.QuotesAndYields(xmlAsk).Price
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

                                    If midPrice > 0 Then HandleQuote(bondDataPoint, BondFields.Fields.Mid, midPrice, Date.Today)
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

        Private Sub HandleQuote(ByRef bondDataPoint As Bond, ByVal fieldName As String, ByVal fieldVal As Double?, ByVal calcDate As Date)
            Dim calculation As New BondPointDescription
            Dim xmlName = BondFields.XmlName(fieldName)
            calculation.BackColor = BondFields.Fields.BackColor(xmlName)
            calculation.MarkerStyle = BondFields.Fields.MarkerStyle(xmlName)
            calculation.Price = fieldVal
            CalculateYields(calcDate, bondDataPoint.MetaData, calculation) ' todo add userDefinedSpread

            bondDataPoint.QuotesAndYields(xmlName) = calculation
            NotifyQuote(bondDataPoint)
        End Sub

        Public Function HasRic(ByVal instrument As String) As Boolean
            Return _elements.Any(Function(elem) elem.Key = instrument)
        End Function

        Public Sub AddRics(ByVal rics As IEnumerable(Of String))
            For Each ric In rics
                Dim descr = BondsData.Instance.GetBondInfo(ric)
                If descr IsNot Nothing Then
                    _elements.Add(ric, New Bond(Me, descr))
                Else
                    Logger.Error("No description for bond {0} found", ric)
                End If
            Next
        End Sub

        Protected Sub New(ByVal ansamble As Ansamble)
            _ansamble = ansamble
        End Sub

        Friend Function GetBond(ByVal ric As String) As Bond
            Return _elements(ric)
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
    End Class

    Public Class Group
        Inherits BaseGroup


        Public Sub New(ByVal ans As Ansamble, ByVal port As PortfolioSource, ByVal portfolioStructure As PortfolioStructure)
            MyBase.new(ans)
            Dim source = TryCast(port.Source, Source)

            If source Is Nothing Then
                Logger.Warn("Unsupported source {0}", source)
                Throw New InvalidOperationException(String.Format("Unsupported source {0}", source))
            End If

            SeriesName = If(port.Name <> "", port.Name, source.Name)
            PortfolioID = source.ID
            BondFields = source.Fields.Realtime.AsContainer()
            Color = If(port.Color <> "", port.Color, source.Color)

            YieldMode = SettingsManager.Instance.YieldCalcMode

            AddRics(portfolioStructure.Rics(port))
        End Sub
    End Class

    Public Class BondCurve
        Inherits BaseGroup

        Public MustInherit Class CurveItem
            Implements IComparable(Of CurveItem)
            Private ReadOnly _x As Double
            Private ReadOnly _y As Double
            Public ReadOnly Property X() As Double
                Get
                    Return _x
                End Get
            End Property

            Public ReadOnly Property Y() As Double
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

        Private _historicalDate As Date?
        Private _history As Boolean = False

        Public Event Updated As Action(Of List(Of CurveItem))

        Private _histFields As FieldContainer

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

        Private _lastCurve As List(Of CurveItem)

        Public Overrides Sub NotifyQuote(ByVal bond As Bond)
            If Ansamble.ChartSpreadType = SpreadType.Yield Then
                Dim result As New List(Of CurveItem)
                If _bootstrapped Then
                    Try
                        ' todo use date of curve instead of today
                        Dim data = (From elem In Elements.Values
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
                    For Each bnd In Elements.Values
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
                    Dim tmp = New List(Of CurveItem)(result)
                    Dim list As List(Of XY) = (From item In tmp Select New XY(item.X, item.Y)).ToList()
                    Dim apprXY = est.Approximate(list)
                    result = (From item In apprXY Select New PointCurveItem(item.X, item.Y, Me)).Cast(Of CurveItem).ToList()
                End If

                _lastCurve = New List(Of CurveItem)(result)
                RaiseEvent Updated(result)
            ElseIf Ansamble.ChartSpreadType.Belongs(SpreadType.ASWSpread, SpreadType.OASpread, SpreadType.ZSpread, SpreadType.PointSpread) Then
                ' todo plotting spreads
            Else
                Logger.Warn("Unknown spread type {0}", Ansamble.ChartSpreadType)
            End If
        End Sub

        Public Overrides Sub NotifyRemoved(ByVal bond As Bond)
            NotifyQuote(Nothing)
        End Sub

        Public Sub Bootstrap()
            Bootstrapped = Not Bootstrapped
        End Sub

        Public Function GetSnapshot() As BondCurveSnapshot
            Return New BondCurveSnapshot(Elements, _lastCurve)
        End Function

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

                Public ReadOnly Property Yield() As Double
                    Get
                        Return _yield
                    End Get
                End Property

                Public ReadOnly Property Duration() As Double
                    Get
                        Return _duration
                    End Get
                End Property

                Public ReadOnly Property Price() As Double
                    Get
                        Return _price
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

            Private ReadOnly _elements As List(Of BondCurveElement)
            Public ReadOnly Property Elements() As List(Of BondCurveElement)
                Get
                    Return _elements
                End Get
            End Property

            Public Sub New(ByVal bonds As Dictionary(Of String, Bond), ByVal items As List(Of CurveItem))
                _elements = New List(Of BondCurveElement)
                For Each bond In bonds.Values
                    Dim mainQuote = bond.QuotesAndYields.Main
                    If mainQuote Is Nothing Then Continue For
                    _elements.Add(New BondCurveElement(bond.MetaData.RIC, bond.Label, mainQuote.GetYield(), mainQuote.Duration, mainQuote.Price, bond.QuotesAndYields.MaxPriorityField))
                Next
                _elements.Sort()
                _current = New List(Of CurveItem)(items)
            End Sub
        End Class

        Public Sub SetFitMode(ByVal mode As String)
            Dim model = EstimationModel.FromName(mode)
            If model Is Nothing OrElse (EstModel IsNot Nothing AndAlso EstModel = model) Then
                EstModel = Nothing
                Return
            End If
            EstModel = model
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
            Set(ByVal value As SpreadType)
                Dim oldType = _currentType
                _currentType = value
                RaiseEvent TypeSelected(_currentType, oldType)
            End Set
        End Property

        Public Sub CalcAllSpreads(ByRef descr As BasePointDescription, Optional ByVal data As BondDescription = Nothing, Optional ByVal type As SpreadType = Nothing)
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

        Public Shared Function IsEnabled(ByVal name As String)
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
        Public Meta As BondDescription
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
            Set(ByVal value As Integer?)
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