Imports System.Windows.Forms
Imports YieldMap.Forms.MainForm
Imports YieldMap.Commons

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
            RefreshColumns()
            IncludeCB.Visible = _canExclude
        End Sub

        Private Sub RefreshColumns()
            Dim selectedFields = BondSelectorVisibleColumns.Split(",")
            If selectedFields.Contains("ALL") Then
                For Each column As DataGridViewColumn In BondListDGV.Columns
                    column.Visible = True
                Next
            Else
                For Each column As DataGridViewColumn In BondListDGV.Columns
                    column.Visible = selectedFields.Contains(column.DataPropertyName)
                Next
            End If
        End Sub

        Private Sub RefreshList()
            IssuerTableAdapter.Fill(BondsDataSet.issuer)
            'Dim accs = New AutoCompleteStringCollection()

            'For Each aName In BondsDataSet.issuer.Select(Function(row As BondsDataSet.issuerRow) row.shortname)
            '    accs.Add(aName)
            'Next

            'IssuerCB.AutoCompleteCustomSource = accs
        End Sub

        Private Sub RefreshGrid()
            Dim strFitler As String = ""

            If IssuerTextBox.Text <> "" Then
                strFitler = String.Format("issname LIKE '{0}%'", IssuerTextBox.Text)
            End If
            If RICTextBox.Text <> "" Then
                strFitler = If(strFitler <> "", strFitler & " AND ", "") & String.Format("ric LIKE '{0}%'", RICTextBox.Text)
            End If

            BondDescriptionsBindingSource.Filter = strFitler

            AddHandler BondDescriptionsTableAdapter.Adapter.FillError, AddressOf Commons.SkipInvalidRows
            BondDescriptionsTableAdapter.Fill(BondsDataSet.BondDescriptions)
            RemoveHandler BondDescriptionsTableAdapter.Adapter.FillError, AddressOf Commons.SkipInvalidRows
        End Sub

        Private Sub OkButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles OkButton.Click
            _selectedRICs = (From aRow As DataGridViewRow In BondListDGV.SelectedRows Select CStr(aRow.Cells(1).Value)).ToList
        End Sub

        Private Sub IncludeCBCheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles IncludeCB.CheckedChanged
            IncludeCB.Text = IIf(IncludeCB.Checked, "Include", "Exclude")
        End Sub

        Private Sub FilterTextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RICTextBox.TextAlignChanged, IssuerTextBox.TextChanged
            RefreshGrid()
        End Sub

        Private _matOrder As SortOrder
        Private _issOrder As SortOrder
        Private _nextPutOrder As SortOrder
        Private _nextCallOrder As SortOrder

        Private Sub BondListDGV_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As DataGridViewCellMouseEventArgs) Handles BondListDGV.ColumnHeaderMouseClick
            If e.Button = MouseButtons.Left Then
                Dim sorted As Boolean = True
                Dim order As SortOrder
                If BondListDGV.Columns(e.ColumnIndex).HeaderText = "Maturity" Then
                    order = SetDateSort(_matOrder, "matsort")
                ElseIf BondListDGV.Columns(e.ColumnIndex).HeaderText = "Issued" Then
                    order = SetDateSort(_issOrder, "isssort")
                ElseIf BondListDGV.Columns(e.ColumnIndex).HeaderText = "Next Put" Then
                    order = SetDateSort(_nextPutOrder, "nextputsort")
                ElseIf BondListDGV.Columns(e.ColumnIndex).HeaderText = "Next Call" Then
                    order = SetDateSort(_nextCallOrder, "nextcallsort")
                Else
                    sorted = False
                End If
                If sorted Then BondListDGV.Columns(e.ColumnIndex).HeaderCell.SortGlyphDirection = order
            ElseIf e.Button = MouseButtons.Right Then
                SelectColumnsCMS.Show(MousePosition)
            End If
        End Sub

        Private Function SetDateSort(ByRef anOrder As SortOrder, ByVal nm As String) As SortOrder
            Dim dir As String
            Select Case anOrder
                Case SortOrder.None
                    anOrder = SortOrder.Ascending
                    dir = "asc"
                Case SortOrder.Ascending
                    anOrder = SortOrder.Descending
                    dir = "desc"
                Case SortOrder.Descending
                    anOrder = SortOrder.None
                    dir = ""
            End Select
            If anOrder <> SortOrder.None Then
                BondDescriptionsBindingSource.Sort = " " & nm & " " & dir
            Else
                BondDescriptionsBindingSource.Sort = ""
            End If
            Return anOrder
        End Function

        Private Sub SelectColumnsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectColumnsToolStripMenuItem.Click
            Dim sf As New SettingsForm
            sf.MainLoadColumnsPage.Select()
            sf.ShowDialog()
            RefreshColumns()
        End Sub
    End Class
End Namespace