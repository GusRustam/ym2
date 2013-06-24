Imports DbManager
Imports Uitls

Namespace Tools.Elements
    Public Class BondGroup
        Inherits Group

        Public Sub New(ByVal ans As Ansamble, ByVal port As PortfolioSource, ByVal portfolioStructure As PortfolioStructure)
            MyBase.new(ans)
            Dim source = TryCast(port.Source, Source)

            If source Is Nothing Then
                Logger.Warn("Unsupported source {0}", source)
                Throw New InvalidOperationException(String.Format("Unsupported source {0}", source))
            End If

            Nm = If(port.Name <> "", port.Name, source.Name)
            PortfolioID = source.ID
            BondFields = source.Fields.Realtime.AsContainer()
            Color = If(port.Color <> "", port.Color, source.Color)


            AddRics(portfolioStructure.Rics(port))
        End Sub

        Public Sub New(ByVal ans As Ansamble, ByVal name As String, ByVal rics As List(Of String), ByVal clr As String, ByVal fields As FieldSet)
            MyBase.new(ans)
            Nm = name
            PortfolioID = -1 ' ??
            BondFields = fields.Realtime.AsContainer()
            Color = clr
            AddRics(rics)
        End Sub

        Public Overrides Sub RecalculateTotal()
            For Each bnd In AllElements
                For Each q In bnd.QuotesAndYields
                    HandleNewQuote(bnd, q, bnd.QuotesAndYields(q).Price, bnd.QuotesAndYields(q).YieldAtDate, False)
                Next
            Next
            Recalculate()
        End Sub

        Public Overrides Sub Recalculate(ByVal ord As IOrdinate)
            NotifyUpdatedSpread(UpdateSpreads(ord), ord)
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
    End Class
End Namespace