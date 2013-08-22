Imports System.Windows.Forms
Imports DbManager.Bonds
Imports System.Reflection
Imports NLog
Imports System.Text.RegularExpressions
Imports Settings
Imports YieldMap.Forms.MainForm

Namespace Forms
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

        Private WithEvents _settings As SettingsManager = SettingsManager.Instance
        Private Sub OnSortMode(ByVal dateFirst As Boolean) Handles _settings.RatingSortModeChanged
            If _bonds.Sorted Then
                RefreshList()
                BondListDGV.Columns(_bonds.SortFieldName).HeaderCell.SortGlyphDirection = _bonds.SortOrder
            End If
        End Sub

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
        End Sub

        Private Sub RefreshList()
            BondListDGV.DataSource = _bonds.Items
        End Sub

        Sub AppendFilter(ByVal tb As TextBox, ByVal fieldName As String, ByRef filter As String)
            Dim newText = tb.Text
            If newText <> "" Then
                ' ReSharper disable UnusedVariable
                Try
                    Dim x As New Regex(tb.Text)
                    filter = If(filter <> "", filter & " AND ", "") & String.Format("${0} LIKE ""{1}""", fieldName, newText)
                Catch ex As Exception
                    filter = If(filter <> "", filter & " AND ", "") & String.Format("${0} = ""{1}""", fieldName, newText)
                End Try
                ' ReSharper restore UnusedVariable
            End If
        End Sub

        Private Sub RefreshGrid()
            Dim strFilter = ""
            AppendFilter(IssuerTextBox, "issuerName", strFilter)
            AppendFilter(RICTextBox, "ric", strFilter)
            AppendFilter(SectorTextBox, "industry", strFilter)
            AppendFilter(CurrencyTextBox, "currency", strFilter)

            Try
                _bonds.SetFilter(strFilter)
                RefreshList()
            Catch ex As ParserException
                Logger.ErrorException("Failed to set filtering", ex)
                Logger.Error("Exception = {0}", ex.ToString())
                MessageBox.Show(String.Format("Failed to apply filtering, message is {0}", ex.ToString()), "Filtering error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
        End Sub

        Private Sub OkButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles OkButton.Click
            _selectedRICs = (From aRow As DataGridViewRow In BondListDGV.SelectedRows Select CStr(aRow.Cells("RIC").Value)).ToList
        End Sub

        Private Sub IncludeCBCheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles IncludeCB.CheckedChanged
            IncludeCB.Text = IIf(IncludeCB.Checked, "Include", "Exclude")
        End Sub

        Private Sub FilterTextChanged(ByVal sender As Object, ByVal e As EventArgs) _
            Handles IssuerTextBox.TextChanged, RICTextBox.TextChanged, CurrencyTextBox.TextChanged, SectorTextBox.TextChanged

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
                Dim sortableFields = (From prop In GetType(BondMetadata).GetProperties(BindingFlags.Public Or BindingFlags.Instance)
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

        Private Sub SelectColumnsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SelectColumnsToolStripMenuItem.Click, SettingsButton.Click
            Controller.SettingsManager(SettingsForm.MainLoadColumnsPage.Name)
            RefreshColumns()
        End Sub

    End Class
End Namespace