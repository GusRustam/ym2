Imports DbManager.Bonds

Namespace Forms.PortfolioForm
    Public Class ParserErrorForm

        Private Sub ParserErrorForm_Load(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.Load
            Dim fields As List(Of String) = FilterHelper.GetFilterableFields(Of BondDescription)()
            VariablesLB.DataSource = fields
        End Sub

    End Class
End Namespace