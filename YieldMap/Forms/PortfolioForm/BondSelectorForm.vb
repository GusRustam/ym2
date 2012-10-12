
Imports System.Data
Imports System.Windows.Forms

Namespace Forms.PortfolioForm
    Public Class BondSelectorForm
        Private _canExclude As Boolean = False
        Public WriteOnly Property CanExclude As Boolean
            Set(value As Boolean)
                _canExclude = value
            End Set
        End Property

        Private _selectedRICs As New List(Of String)
        Public ReadOnly Property SelectedRICs As List(Of String)
            Get
                Return _selectedRICs
            End Get
        End Property

        Private Sub BondSelectorFormLoad(sender As System.Object, e As EventArgs) Handles MyBase.Load
            RefreshList()
            RefreshGrid()
            IncludeCB.Visible = _canExclude
        End Sub

        Private Sub RefreshList()
            IssuerTableAdapter.Fill(BondsDataSet.issuer)
            Dim accs = New AutoCompleteStringCollection()

            For Each aName In BondsDataSet.issuer.Select(Function(row As BondsDataSet.issuerRow) row.shortname)
                accs.Add(aName)
            Next

            IssuerCB.AutoCompleteCustomSource = accs
        End Sub

        Private Sub RefreshGrid()
            If IssuerCB.Enabled And IssuerCB.SelectedIndex >= 0 Then
                BondDescriptionsBindingSource.Filter = String.Format("issuer_id = {0:D}", CType(IssuerCB.SelectedItem, DataRowView)("id"))
            Else
                BondDescriptionsBindingSource.Filter = ""
            End If
            AddHandler BondDescriptionsTableAdapter.Adapter.FillError, AddressOf Commons.SkipInvalidRows
            BondDescriptionsTableAdapter.Fill(BondsDataSet.BondDescriptions)
            RemoveHandler BondDescriptionsTableAdapter.Adapter.FillError, AddressOf Commons.SkipInvalidRows
        End Sub

        Private Sub ShowAllCBCheckedChanged(sender As System.Object, e As EventArgs) Handles ShowAllCB.CheckedChanged
            IssuerCB.Enabled = Not ShowAllCB.Checked
            RefreshList()
            RefreshGrid()
        End Sub

        Private Sub IssuerCBSelectedIndexChanged(sender As System.Object, e As EventArgs) Handles IssuerCB.SelectedIndexChanged
            RefreshGrid()
        End Sub

        Private Sub OkButtonClick(sender As System.Object, e As EventArgs) Handles OkButton.Click
            _selectedRICs = (From aRow As DataGridViewRow In BondListDGV.SelectedRows Select CStr(aRow.Cells(1).Value)).ToList
        End Sub

        Private Sub IncludeCBCheckedChanged(sender As System.Object, e As EventArgs) Handles IncludeCB.CheckedChanged
            IncludeCB.Text = IIf(IncludeCB.Checked, "Include", "Exclude")
        End Sub
    End Class
End Namespace