Imports DbManager
Imports Settings
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

            YieldMode = SettingsManager.Instance.YieldCalcMode

            AddRics(portfolioStructure.Rics(port))
        End Sub

        Public Overrides Sub Recalculate(ByVal ord As IOrdinate)
            RecalculateSpread(ord)
        End Sub

        Private Function RecalculateSpread(ByVal ordinate As IOrdinate) As List(Of CurveItem)
            SetSpread(ordinate)
            'For Each qy In From item In AllElements From quoteName In item.QuotesAndYields Select item.QuotesAndYields(quoteName)
            '    res.Add(New PointCurveItem(qy.Duration, ord.GetValue(qy), Me))
            'Next
            Dim res = New List(Of CurveItem)(From item In AllElements
                                                               From quoteName In item.QuotesAndYields
                                                               Let q = item.QuotesAndYields(quoteName)
                                                               Select New BondCurveItem(q.Duration, ordinate.GetValue(q), q.ParentBond, q.BackColor, q.Yld.ToWhat, q.MarkerStyle, q.ParentBond.Label))
            res.Sort()
            Return res
        End Function

        Public Sub SetSpread(ByVal ySource As OrdinateBase)
            If Ansamble.Benchmarks(ySource) = Me Then
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
            If Ansamble.YSource = Yield Then
                NotifyUpdated(RecalculateYield())
            ElseIf Ansamble.YSource.Belongs(AswSpread, OaSpread, ZSpread, PointSpread) Then
                NotifyUpdated(RecalculateSpread(Ansamble.YSource))
            Else
                Logger.Warn("Unknown spread type {0}", Ansamble.YSource)
            End If
        End Sub

        Private Function RecalculateYield() As List(Of CurveItem)
            Dim result As New List(Of CurveItem)

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
                If x > 0 And y > 0 Then result.Add(New BondCurveItem(x, y, bnd, description.BackColor, description.Yld.ToWhat, description.MarkerStyle, bnd.Label))
            Next
            result.Sort()
            Return result
        End Function
    End Class
End Namespace