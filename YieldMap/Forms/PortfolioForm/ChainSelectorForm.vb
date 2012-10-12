Imports System.Data

Namespace Forms.PortfolioForm

    Public Class ChainSelectorForm
        Private _selectedChains As New List(Of Integer)
        Public ReadOnly Property SelectedChains As List(Of Integer)
            Get
                Return _selectedChains
            End Get
        End Property

        Private Sub ChainSelectorFormLoad(sender As System.Object, e As EventArgs) Handles MyBase.Load
            ChainTableAdapter.Fill(BondsDataSet.chain)
        End Sub

        Private Sub IncludeCbCheckedChanged(sender As System.Object, e As EventArgs) Handles IncludeCB.CheckedChanged
            IncludeCB.Text = IIf(IncludeCB.Checked, "Include", "Exclude")
        End Sub

        Private Sub OkButtonClick(sender As System.Object, e As EventArgs) Handles OkButton.Click
            _selectedChains = (From elem As DataRowView In ChainListBox.SelectedItems Select CInt(elem("id"))).ToList
        End Sub
    End Class
End Namespace