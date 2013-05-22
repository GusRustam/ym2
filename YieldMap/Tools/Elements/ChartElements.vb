Imports System.Reflection
Imports DbManager.Bonds
Imports YieldMap.Forms.TableForm
Imports YieldMap.Curves

Namespace Tools.Elements
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
                    grp.Elements.ForEach(
                        Sub(elem)
                            Dim res As New BondDescr
                            res.RIC = elem.MetaData.RIC
                            res.Name = elem.MetaData.ShortName
                            res.Maturity = elem.MetaData.Maturity
                            res.Coupon = elem.MetaData.Coupon
                            Dim fieldName = elem.QuotesAndYields.MaxPriorityField
                            Dim quote = elem.QuotesAndYields(fieldName)
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

        Public Function FindBond(ByVal ric As String) As Bond
            For Each kvp In From elem In _groups Where elem.Value.HasRic(ric)
                Return kvp.Value.GetBond(ric)
            Next
            Return Nothing
        End Function
    End Class

    ' todo custom label
    Public Enum LabelMode
        IssuerAndSeries
        IssuerCpnMat
        Description
        SeriesOnly
    End Enum

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

End Namespace