Imports System.Data

Public Class HawserSelectorForm
    Private _selectedHawsers As New List(Of Integer)
    Public ReadOnly Property SelectedHawsers As List(Of Integer)
        Get
            Return _selectedHawsers
        End Get
    End Property

    Private Sub ChainSelectorForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        HawserTableAdapter.Fill(BondsDataSet.hawser)
    End Sub

    Private Sub IncludeCB_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles IncludeCB.CheckedChanged
        IncludeCB.Text = IIf(IncludeCB.Checked, "Include", "Exclude")
    End Sub

    Private Sub OkButton_Click(sender As System.Object, e As System.EventArgs) Handles OkButton.Click
        _selectedHawsers = (From elem As DataRowView In ChainListBox.SelectedItems Select CInt(elem("id"))).ToList
    End Sub
End Class