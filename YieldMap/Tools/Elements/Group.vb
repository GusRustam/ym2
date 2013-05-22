Imports DbManager
Imports Settings

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
    End Class
End NameSpace