﻿Imports System.Windows.Forms
Imports DbManager.Bonds
Imports YieldMap.Forms.MainForm

Namespace Forms.PortfolioForm
    Public Class BondSelectorForm
        Private _bonds As New BondView

        Private _canExclude As Boolean = False
        Public WriteOnly Property CanExclude As Boolean
            Set(ByVal value As Boolean)
                _canExclude = value
            End Set
        End Property

        Private _selectedRICs As New List(Of String)
        Public ReadOnly Property SelectedRICs As List(Of String)
            Get
                Return _selectedRICs
            End Get
        End Property

        Private Sub BondSelectorFormLoad(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            RefreshList()
            RefreshGrid()
            RefreshColumns()
            IncludeCB.Visible = _canExclude
        End Sub

        Private Sub RefreshColumns()
            ' todo choosing columns
            'Dim selectedFields = BondSelectorVisibleColumns.Split(",")
            'If selectedFields.Contains("ALL") Then
            '    For Each column As DataGridViewColumn In BondListDGV.Columns
            '        column.Visible = True
            '    Next
            'Else
            '    For Each column As DataGridViewColumn In BondListDGV.Columns
            '        column.Visible = selectedFields.Contains(column.DataPropertyName)
            '    Next
            'End If
            'BondListDGV.Columns(0).Visible = False
        End Sub

        Private Sub RefreshList()
            'Dim rics = (From row In BondsLoader.Instance.GetBondsTable() Select row.ric).ToList()
            BondListDGV.DataSource = _bonds.Items 'BondsData.Instance.GetBondInfo(rics)
            '
            ' todo 1) сортировка и статическая фильтрация
            ' todo 2) для строк предусмотреть оператор LIKE
            ' todo 3) для рейтингов предусмотреть сортировку и отбор (например, квадратные скобки)
            ' todo 4) выполнение распарсенного представления и динамическая фильтрация
            '
            'IssuerTableAdapter.Fill(BondsDataSet.issuer)


            'Dim accs = New AutoCompleteStringCollection()

            'For Each aName In BondsDataSet.issuer.Select(Function(row As BondsDataSet.issuerRow) row.shortname)
            '    accs.Add(aName)
            'Next

            'IssuerCB.AutoCompleteCustomSource = accs
        End Sub

        Private Sub RefreshGrid()
            Dim strFitler As String = ""

            If IssuerTextBox.Text <> "" Then
                strFitler = String.Format("$issuerName Like ""{0}""", IssuerTextBox.Text)
            End If
            If RICTextBox.Text <> "" Then
                strFitler = If(strFitler <> "", strFitler & " AND ", "") & String.Format("$ric LIKE ""{0}""", RICTextBox.Text)
            End If

            _bonds.SetFilter(strFitler)
            RefreshList()
        End Sub

        Private Sub OkButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles OkButton.Click
            _selectedRICs = (From aRow As DataGridViewRow In BondListDGV.SelectedRows Select CStr(aRow.Cells(0).Value)).ToList
        End Sub

        Private Sub IncludeCBCheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles IncludeCB.CheckedChanged
            IncludeCB.Text = IIf(IncludeCB.Checked, "Include", "Exclude")
        End Sub

        Private Sub FilterTextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles IssuerTextBox.TextChanged, RICTextBox.TextChanged
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
                If BondListDGV.Columns(e.ColumnIndex).HeaderText = "Maturity date" Then
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
            'If anOrder <> SortOrder.None Then
            '    BondDescriptionsBindingSource.Sort = " " & nm & " " & dir
            'Else
            '    BondDescriptionsBindingSource.Sort = ""
            'End If
            Return anOrder
        End Function

        Private Sub SelectColumnsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SelectColumnsToolStripMenuItem.Click
            Dim sf As New SettingsForm
            sf.MainLoadColumnsPage.Select()
            sf.ShowDialog()
            RefreshColumns()
        End Sub
    End Class
End Namespace