Namespace Forms.PortfolioForm
    Public Class AddPortfolioForm
        Public EditMode As Boolean
        Private Sub AddPortfolioForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            Text = If(EditMode, "Rename item", "Add item")

        End Sub
    End Class
End Namespace