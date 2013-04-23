Imports DbManager.Bonds
Imports DbManager
Imports Uitls

Namespace Forms.PortfolioForm
    Public Class AddPortfolioSource
        Public ReadOnly Property CustomName() As String
            Get
                Return CustomNameTB.Text
            End Get
        End Property

        Public ReadOnly Property CustomColor() As String
            Get
                Return If(CustomColorCB.SelectedItem IsNot Nothing, CustomColorCB.SelectedItem.ToString(), "")
            End Get
        End Property

        Public ReadOnly Property Condition() As String
            Get
                Dim cond = ConditionTB.Text
                Dim parser As New FilterParser
                Try
                    parser.SetFilter(cond)
                Catch ex As ParserException
                    
                    Return ""
                End Try
                Return cond
            End Get
        End Property

        Public Structure NewSourceDescr
            Public Src As SourceBase
            Public CustomColor As String
            Public CustomName As String
            Public Condition As String
            Public Include As Boolean
        End Structure

        Public ReadOnly Property Data() As NewSourceDescr
            Get
                Dim res As New NewSourceDescr
                res.CustomColor = CustomColor
                res.CustomName = CustomName
                res.Condition = Condition
                res.Include = (MainTabControl.SelectedTab.Name <> ChainOrListTP.Name AndAlso CustomBondsRB.Checked) OrElse IncludeCB.Checked
                res.Src = If(MainTabControl.SelectedTab.Name = ChainOrListTP.Name, ChainsListsLB.SelectedItem, BondsDGV.SelectedRows(0).DataBoundItem)
                Return res
            End Get
        End Property

        Private Sub MainTabControl_TabIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MainTabControl.TabIndexChanged, MainTabControl.SelectedIndexChanged
            RefreshSupplementaryFields()
        End Sub

        Private Sub AddEditPortfolioSource_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            For Each clr In Utils.GetColorList()
                CustomColorCB.Items.Add(clr)
            Next
            RefreshChainListList()
            BondsDGV.DataSource = If(IndBondsRB.Checked, Nothing, PortfolioManager.Instance.CustomBondsView())
        End Sub

        Private Sub BondsDGV_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles BondsDGV.CellClick
            RefreshSupplementaryFields()
        End Sub

        Private Sub RandomColorButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles RandomColorB.Click
            CustomColorCB.SelectedIndex = New Random().NextDouble() * CustomColorCB.Items.Count
        End Sub

        Private Sub ChainsListsLB_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ChainsListsLB.SelectedIndexChanged
            RefreshSupplementaryFields()
        End Sub

        Private Sub ColorComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CustomColorCB.SelectedIndexChanged
            If CustomColorCB.SelectedIndex < 0 Then
                SampleColorPB.BackColor = Color.White
            Else
                SampleColorPB.BackColor = Color.FromName(CustomColorCB.SelectedItem)
            End If
        End Sub

        Private Sub RefreshChainListList()
            ChainsListsLB.DataSource = If(ShowChainsRB.Checked,
                      PortfolioManager.Instance.ChainsView,
                      PortfolioManager.Instance.UserListsView)
        End Sub

        Private Sub RefreshSupplementaryFields()
            If MainTabControl.SelectedTab.Name = ChainOrListTP.Name Then
                ConditionTB.Enabled = True
                IncludeCB.Enabled = True
            Else
                ConditionTB.Enabled = False
                IncludeCB.Enabled = False
            End If
        End Sub

        Private Sub OkButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OkButton.Click
            If MainTabControl.SelectedTab.Name = ChainOrListTP.Name AndAlso ChainsListsLB.SelectedIndex < 0 Then
                MessageBox.Show("Please select chain or list to show", "Cannot perform an operation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf MainTabControl.SelectedTab.Name = IndividualAndCustomBondsTP.Name AndAlso BondsDGV.SelectedRows.Count <= 0 Then
                MessageBox.Show("Please select some bonds or custom bonds", "Cannot perform an operation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                DialogResult = DialogResult.OK
                Close()
            End If
        End Sub

        Private Sub CustomColorCB_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles CustomColorCB.DrawItem
            Dim g As Graphics = e.Graphics
            Dim r As Rectangle = e.Bounds
            If e.Index > 0 Then
                Dim txt As String = CustomColorCB.Items(e.Index)
                g.DrawString(txt, CustomColorCB.Font, Brushes.Black, r.X, r.Top)
                Dim m = g.MeasureString(txt, CustomColorCB.Font)
                Dim c As Color = Color.FromName(txt)
                g.FillRectangle(New SolidBrush(c), r.X + m.Width + 10, r.Y + 2, r.Width - m.Width - 15, r.Height - 6)
                g.DrawRectangle(New Pen(New SolidBrush(Color.Black)), r.X + m.Width + 10, r.Y + 2, r.Width - m.Width - 15, r.Height - 6)
            End If
        End Sub

        Private Sub ShowChainsRB_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ShowChainsRB.CheckedChanged
            RefreshChainListList()
            RefreshSupplementaryFields()
        End Sub

        Private Sub IncludeCB_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles IncludeCB.CheckedChanged
            IncludeCB.Text = If(IncludeCB.Checked, "Yes", "No")
        End Sub

        Private Sub ConditionTB_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles ConditionTB.Enter, ConditionTB.MouseClick
            Dim frm As New ParserErrorForm
            If frm.ShowDialog() = DialogResult.OK Then ConditionTB.Text = frm.ConditionTB.Text
        End Sub
    End Class
End Namespace