Namespace Forms.PortfolioForm
    Public Class AddPortfolioForm
        Public EditMode As Boolean
        Public ItIsPortfolio As Boolean

        Private Sub AddPortfolioForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            Text = If(EditMode, "Rename item", "Add item")
            ItsFolder.Checked = Not ItIsPortfolio
            ItsPortfolio.Checked = ItIsPortfolio
            ItsFolder.Enabled = Not EditMode
            ItsPortfolio.Enabled = Not EditMode
        End Sub

        Private Sub OkButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OkButton.Click
            If NewName.Text = "" Then
                MessageBox.Show("Cannot use empty name", Text)
            Else
                DialogResult = DialogResult.OK
                Close()
            End If
        End Sub

        Private Sub AddPortfolioForm_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) Handles MyBase.KeyUp, NewName.KeyUp
            If e.KeyCode = Keys.Escape Then
                DialogResult = DialogResult.OK
                Close()
            End If
        End Sub
    End Class
End Namespace