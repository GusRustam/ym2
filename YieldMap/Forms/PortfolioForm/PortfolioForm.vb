Imports DbManager.Bonds

Namespace Forms.PortfolioForm
    Public Class PortfolioForm
        Private Sub PortfolioForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            BondsTableView.DataSource = BondsLoader.Instance.GetBondsTable()
        End Sub

        Private Sub TableChooserList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TableChooserList.SelectedIndexChanged
            If TableChooserList.SelectedIndex >= 0 Then
                Select Case TableChooserList.Items(TableChooserList.SelectedIndex).ToString()
                    Case "Bonds" : BondsTableView.DataSource = BondsLoader.Instance.GetBondsTable()
                    Case "Coupons" : BondsTableView.DataSource = BondsLoader.Instance.GetCouponsTable()
                    Case "FRNs" : BondsTableView.DataSource = BondsLoader.Instance.GetFRNsTable()
                    Case "Issue ratings" : BondsTableView.DataSource = BondsLoader.Instance.GetIssueRatingsTable()
                    Case "Issuer ratings" : BondsTableView.DataSource = BondsLoader.Instance.GetIssuerRatingsTable()
                    Case "Rics" : BondsTableView.DataSource = BondsLoader.Instance.GetAllRicsTable()
                End Select
            End If
        End Sub
    End Class
End Namespace