Imports DbManager
Imports Settings
Imports Uitls

Namespace Tools.Elements
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

        Protected Overrides Sub NotifyChanged()
            If Ansamble.YSource = YSource.Yield Then
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

                    y = description.GetYield()
                    If x > 0 And y > 0 Then result.Add(New BondCurveItem(x, y, bnd, description.BackColor, description.Yld.ToWhat, description.MarkerStyle, bnd.Label))
                Next
                result.Sort()

                NotifyUpdated(result)
            ElseIf Ansamble.YSource.Belongs(YSource.ASWSpread, YSource.OASpread, YSource.ZSpread, YSource.PointSpread) Then
                ' todo plotting spreads
            Else
                Logger.Warn("Unknown spread type {0}", Ansamble.YSource)
            End If
        End Sub
    End Class
End Namespace