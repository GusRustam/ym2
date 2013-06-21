Imports DbManager
Imports Uitls

Namespace Tools.Elements
    'todo much common methods with BondGroup and even BondCurve
    Public Class CustomBondGroup
        Inherits Group

        Public Sub New(ByVal ansamble As Ansamble, ByVal port As PortfolioSource, ByVal portfolioStructure As PortfolioStructure)
            MyBase.New(ansamble)

            Nm = If(port.Name <> "", port.Name, port.Source.Name)
            PortfolioID = port.Source.ID
            Color = If(port.Color <> "", port.Color, port.Source.Color)
            Dim flds = New FieldSet("Custom bonds")
            BondFields = flds.Realtime.AsContainer()
            AddRics(portfolioStructure.Rics(port))
        End Sub

        Public Overrides Sub Subscribe()
            For Each item In Elements
                item.SetCustomPrice(100)
            Next
        End Sub

        Public Overrides Sub AddRics(ByVal rics As IEnumerable(Of String))
            For Each ric In rics
                Dim cstmBond = CustomBond.LoadByCode(ric)
                If cstmBond IsNot Nothing Then
                    Dim bond = New CustomCouponBond(Me, cstmBond)
                    AddHandler bond.Changed, Sub() If Not EventsFrozen Then RecalculateTotal()
                    AddHandler bond.CustomPrice, AddressOf OnCustomCustomPrice
                    AllElements.Add(bond)
                Else
                    Logger.Error("No description for bond {0} found", ric)
                End If
            Next
        End Sub

        Public Overrides Sub Recalculate()
            Dim res As New Dictionary(Of IOrdinate, List(Of PointOfCurve))
            res(Yield) = UpdatePoints()
            For Each ord In Spreads
                res(ord) = UpdateSpreads(ord)
            Next

            If Ansamble.YSource = Yield Then
                NotifyUpdated(res(Yield))
            ElseIf Ansamble.YSource.Belongs(AswSpread, OaSpread, ZSpread, PointSpread) Then
                If res.ContainsKey(Ansamble.YSource) Then NotifyUpdated(res(Ansamble.YSource))
            Else
                Logger.Warn("Unknown spread type {0}", Ansamble.YSource)
            End If
        End Sub

        Public Sub SetSpread(ByVal ySource As OrdinateBase)
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

        Private Function UpdateSpreads(ByVal ordinate As IOrdinate) As List(Of PointOfCurve)
            SetSpread(ordinate)
            Dim res = New List(Of PointOfCurve)(
                        From item In AllElements
                        From quoteName In item.QuotesAndYields
                        Let q = item.QuotesAndYields(quoteName)
                        Let theY = ordinate.GetValue(q)
                        Where theY.HasValue AndAlso item.QuotesAndYields.Main IsNot Nothing AndAlso quoteName = item.QuotesAndYields.Main.QuoteName
                        Select New PointOfBondCurve(q.Duration, theY, q.ParentBond,
                                                q.BackColor, q.Yld.ToWhat, q.MarkerStyle,
                                                q.ParentBond.Label))
            res.Sort()
            Return res
        End Function

        Private Function UpdatePoints() As List(Of PointOfCurve)
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

        Public Overrides Sub Recalculate(ByVal ord As IOrdinate)
            NotifyUpdatedSpread(UpdateSpreads(ord), ord)
        End Sub

        Public Overrides Sub RecalculateTotal()
            For Each bnd In AllElements
                For Each q In bnd.QuotesAndYields
                    HandleNewQuote(bnd, q, bnd.QuotesAndYields(q).Price, bnd.QuotesAndYields(q).YieldAtDate, False)
                Next
            Next
            Recalculate()
        End Sub
    End Class
End Namespace