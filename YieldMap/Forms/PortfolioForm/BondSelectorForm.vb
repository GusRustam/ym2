Imports System.Windows.Forms
Imports DbManager.Bonds
Imports System.Reflection
Imports NLog
Imports Settings
Imports YieldMap.Forms.MainForm

Namespace Forms.PortfolioForm
    Public Class BondSelectorForm
        Private ReadOnly _bonds As New BondView
        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(BondSelectorForm))

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
            Dim selectedFields = SettingsManager.Instance.BondSelectorVisibleColumns.Split(",")
            If selectedFields.Contains("ALL") Then
                For Each column As DataGridViewColumn In BondListDGV.Columns
                    column.Visible = True
                Next
            Else
                For Each column As DataGridViewColumn In BondListDGV.Columns
                    column.Visible = selectedFields.Contains(column.DataPropertyName)
                Next
            End If
            BondListDGV.Columns(0).Visible = False
        End Sub

        Private Sub RefreshList()
            BondListDGV.DataSource = _bonds.Items
        End Sub

        Private Sub RefreshGrid()
            Dim strFitler As String = ""

            If IssuerTextBox.Text <> "" Then
                strFitler = String.Format("$issuerName Like ""{0}""", IssuerTextBox.Text)
            End If
            If RICTextBox.Text <> "" Then
                strFitler = If(strFitler <> "", strFitler & " AND ", "") & String.Format("$ric LIKE ""{0}""", RICTextBox.Text)
            End If
            Try
                _bonds.SetFilter(strFitler)
                RefreshList()
            Catch ex As ParserException
                Logger.ErrorException("Failed to set filtering", ex)
                Logger.Error("Exception = {0}", ex.ToString())
                MessageBox.Show(String.Format("Failed to apply filtering, message is {0}", ex.ToString()), "Filtering error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
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

        Private Sub BondListDGV_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As DataGridViewCellMouseEventArgs) Handles BondListDGV.ColumnHeaderMouseClick
            If e.Button = MouseButtons.Left Then
                Dim order As SortDirection
                Select Case BondListDGV.Columns(e.ColumnIndex).HeaderCell.SortGlyphDirection
                    Case SortDirection.Asc
                        order = SortDirection.Desc
                    Case SortDirection.Desc
                        order = SortDirection.None
                    Case SortDirection.None
                        order = SortDirection.Asc
                End Select
                Dim propertyName = BondListDGV.Columns(e.ColumnIndex).DataPropertyName
                Dim sortableFields = (From prop In GetType(BondDescription).GetProperties(BindingFlags.Public Or BindingFlags.Instance)
                                 Where prop.GetCustomAttributes(GetType(SortableAttribute), False).Any
                                 Select prop.Name).ToList()
                If sortableFields.Contains(propertyName) Then
                    _bonds.SetSort(propertyName, order)
                    RefreshList()
                    BondListDGV.Columns(e.ColumnIndex).HeaderCell.SortGlyphDirection = GetSortOrder(order)
                End If
            ElseIf e.Button = MouseButtons.Right Then
                SelectColumnsCMS.Show(MousePosition)
            End If
        End Sub

        Private Shared Function GetSortOrder(ByVal sortDirection As SortDirection) As SortOrder
            If sortDirection = sortDirection.None Then Return SortOrder.None
            If sortDirection = sortDirection.Asc Then Return SortOrder.Ascending
            Return SortOrder.Descending
        End Function

        Private Sub SelectColumnsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SelectColumnsToolStripMenuItem.Click
            Dim sf As New SettingsForm
            sf.MainLoadColumnsPage.Select()
            sf.ShowDialog()
            RefreshColumns()
        End Sub
    End Class
End Namespace