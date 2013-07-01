Imports AdfinXAnalyticsFunctions
Imports DbManager
Imports System.Text.RegularExpressions
Imports ReutersData
Imports Settings
Imports Uitls

Namespace Tools.Elements
    Public Class ChainCurve
        Inherits Group
        Implements ICurve

        Private WithEvents _chainLoader As Chain = New Chain
        Private ReadOnly _src As ChainCurveSrc
        Private ReadOnly _dates As IAdxDateModule = Eikon.Sdk.CreateAdxDateModule()
        Private ReadOnly _bonds As IAdxBondModule = Eikon.Sdk.CreateAdxBondModule()

        Public Sub New(ByVal ansamble As Ansamble, ByVal src As ChainCurveSrc)
            MyBase.New(ansamble, src.Fields)
            Color = src.Color
            Nm = src.Name

            ' Loading curve items (getDefaultRics returns chain ric and I am loading items)
            _chainLoader.StartChains(src.GetDefaultRics(), src.Skip)
            _src = src
        End Sub
        Private ReadOnly _lastCurve As New Dictionary(Of IOrdinate, List(Of PointOfCurve))

        Public Overrides Sub Recalculate(ByVal ord As IOrdinate)
            ' yield can't be a source for benchmark
            If ord = Yield Then Throw New InvalidOperationException()
            _lastCurve(ord) = UpdateSpreads(ord)
            NotifyUpdatedSpread(_lastCurve(ord), ord)
        End Sub

        Private Function UpdateCurveShape() As List(Of PointOfCurve)
            Dim result As New List(Of PointOfCurve)
            
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

                y = description.Yield
                If x > 0 And y > 0 Then result.Add(New PointOfBondCurve(x, y, bnd, description.BackColor, description.Yld.ToWhat, description.MarkerStyle, bnd.Label))
            Next
            result.Sort()

            Return result
        End Function

        Public Overrides Sub Recalculate()
            _lastCurve(Yield) = UpdateCurveShape()

            For Each ord In Spreads
                _lastCurve(ord) = UpdateSpreads(ord)
            Next
            If Ansamble.YSource = Yield Then
                NotifyUpdated(_lastCurve(Yield))
            ElseIf Ansamble.YSource.Belongs(AswSpread, OaSpread, ZSpread, PointSpread) Then
                If _lastCurve.ContainsKey(Ansamble.YSource) Then NotifyUpdated(_lastCurve(Ansamble.YSource))
            Else
                Logger.Warn("Unknown spread type {0}", Ansamble.YSource)
            End If
        End Sub

        Private Function UpdateSpreads(ByVal ord As IOrdinate) As List(Of PointOfCurve)
            SetSpread(ord)
            Dim res = New List(Of PointOfCurve)(
                        From item In AllElements
                        From quoteName In item.QuotesAndYields
                        Let q = item.QuotesAndYields(quoteName)
                        Let theY = ord.GetValue(q)
                        Where theY.HasValue AndAlso item.QuotesAndYields.Main IsNot Nothing AndAlso quoteName = item.QuotesAndYields.Main.QuoteName
                        Select New JustPoint(q.Duration, theY, Me))
            res.Sort()
            Return res
        End Function

        Public ReadOnly Property Snapshot() As ISnapshot Implements ICurve.Snapshot
            Get
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property Formula() As String Implements ICurve.Formula
            Get
                Return ""
            End Get
        End Property

        Public ReadOnly Property CanBootstrap() As Boolean Implements ICurve.CanBootstrap
            Get
                Return False
            End Get
        End Property

        Public Property Bootstrapped() As Boolean Implements ICurve.Bootstrapped
            Get
                Return False
            End Get
            Set(value As Boolean)

            End Set
        End Property

        Public Sub Bootstrap() Implements ICurve.Bootstrap
        End Sub

        Public ReadOnly Property IsSynthetic() As Boolean Implements ICurve.IsSynthetic
            Get
                Return False
            End Get
        End Property


        Public Sub ClearSpread(ByVal ySource As OrdinateBase) Implements ICurve.ClearSpread
            For Each qy In From item In AllElements From quoteName In item.QuotesAndYields Select item.QuotesAndYields(quoteName)
                ySource.ClearValue(qy)
            Next
        End Sub

        Public Sub SetSpread(ByVal ySource As OrdinateBase) Implements ICurve.SetSpread
            If Ansamble.Benchmarks.Keys.Contains(ySource) AndAlso Ansamble.Benchmarks(ySource) <> Me Then
                For Each qy In From item In AllElements From quoteName In item.QuotesAndYields Select item.QuotesAndYields(quoteName)
                    ySource.SetValue(qy, Ansamble.Benchmarks(ySource))
                Next
            Else
                For Each qy In From item In AllElements From quoteName In item.QuotesAndYields Select item.QuotesAndYields(quoteName)
                    ySource.ClearValue(qy)
                Next
            End If
        End Sub

        Public Function RateArray() As Array Implements ICurve.RateArray
            If Not _lastCurve.ContainsKey(Yield) Then Return Nothing
            Dim list = (From elem In _lastCurve(Yield) Select New XY(elem.TheX, elem.TheY)).ToList()
            list.Sort()
            Dim len = list.Count - 1
            Dim res(0 To len, 1) As Object
            For i = 0 To len
                res(i, 0) = DateTime.Today.AddDays(TimeSpan.FromDays(list(i).X * 365).TotalDays)
                res(i, 1) = list(i).Y
            Next
            Return res
        End Function

        Public Overrides Sub RecalculateTotal()
            'Dim tmp = New List(Of Bond)(AllElements)
            For Each bnd In AllElements
                For Each q In bnd.QuotesAndYields
                    HandleNewQuote(bnd, q, bnd.QuotesAndYields(q).Yld.Yield * 100, bnd.QuotesAndYields(q).YieldAtDate, False)
                Next
            Next
            Recalculate()
        End Sub

        Private ReadOnly _terms As New Dictionary(Of String, Double)

        Private Sub OnChainData(ric As String, data As Dictionary(Of String, List(Of String)), done As Boolean) Handles _chainLoader.Chain
            ' Parsing rics, extracting terms, creating ZCB's and storing them
            Dim rics = data(ric)
            Dim rex As New Regex(_src.Pattern)
            For Each rc In rics
                Dim mtch = rex.Match(rc)
                If mtch.Success Then
                    Dim trm = mtch.Groups("term").Value
                    If trm.Trim() <> "ON" Then
                        Dim dt = _dates.DfAddPeriod("RUS", Today, trm, "")
                        _terms(rc) = Utils.FromExcelSerialDate(dt.GetValue(1, 1)).Subtract(Today).Days / 365
                    Else
                        _terms(rc) = 1 / 365
                    End If
                    AllElements.Add(New SyntheticZcb(Me, Today, 0, _terms(rc), Name, rc))
                Else
                    Logger.Warn("Failed to parse ric {0}", rc)
                End If
            Next

            If AllElements.Any Then
                Subscribe()
            Else
                Logger.Error("Failed to find any suitable ric in chain {0}", ric)
            End If
        End Sub

        Private Function GetZcbPrice(bond As SyntheticZcb, yield As Double) As Double
            Dim dur = _terms(bond.MetaData.Ric)
            Dim newMat = Today.AddDays(dur * 365 / (1 + yield * dur))
            bond.MetaData.Maturity = newMat

            Dim paymentStructure = bond.MetaData.PaymentStructure
            Dim rateStructure = bond.MetaData.RateStructure

            Dim settleDate = _bonds.BdSettle(Today, paymentStructure)
            Dim priceObject As Array = _bonds.AdBondPrice(settleDate, yield, bond.MetaData.Maturity, 0, 0, paymentStructure, Regex.Replace(rateStructure, "YT[A-Z]", SettingsManager.Instance.YieldCalcMode), "", "RES:BDPRICE")
            Return 100 * priceObject.GetValue(1)
        End Function


        Protected Overrides Sub HandleNewQuote(ByRef bond As Bond, ByVal xmlName As String, ByVal fieldVal As Double?, ByVal calcDate As Date, Optional _
                                        ByVal recalc As Boolean = True)
            'AllElements.Remove(bond)
            'bond = New SyntheticZcb(Me, Today, fieldVal / 100, _terms(bond.MetaData.Ric), Name, bond.MetaData.Ric)
            'AllElements.Add(bond)

            Dim zcbPrice = GetZcbPrice(bond, fieldVal / 100)
            If Not bond.QuotesAndYields.Contains(xmlName) Then
                Dim descr As New BondPointDescription(xmlName)
                descr.BackColor = BondFields.Fields.BackColor(xmlName)
                descr.MarkerStyle = BondFields.Fields.MarkerStyle(xmlName)
                descr.ParentBond = bond
                descr.Yield(Today) = zcbPrice

                bond.QuotesAndYields(xmlName) = descr
            Else
                bond.QuotesAndYields(xmlName).Yield(Today) = zcbPrice
            End If

            If recalc Then Recalculate()
        End Sub

        Private Shared Sub OnChainFailed(arg1 As String, arg2 As Exception, arg3 As Boolean) Handles _chainLoader.Failed
            Logger.Error("Failed to load chain {0}", arg1)
        End Sub
    End Class
End Namespace