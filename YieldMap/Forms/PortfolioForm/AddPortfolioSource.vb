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
                Return CustomColorCB.SelectedText
            End Get
        End Property

        Public ReadOnly Property Condition() As String
            Get
                Return ConditionTB.Text
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
            ' ReSharper disable ConditionalTernaryEqualBranch
            BondsDGV.DataSource = If(IndBondsRB.Checked, Nothing, Nothing)
            ' ReSharper restore ConditionalTernaryEqualBranch
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
                     (From item In PortfolioManager.Instance.ChainsView Select New IdName(Of String)(item.ID, item.Name)).ToList(),
                     (From item In PortfolioManager.Instance.UserListsView Select New IdName(Of String)(item.ID, item.Name)).ToList())
        End Sub

        Private Sub RefreshSupplementaryFields()
            If MainTabControl.SelectedTab.Name = ChainOrListTP.Name Then
                CustomNameTB.Enabled = True
                CustomColorCB.Enabled = True
                RandomColorB.Enabled = True
                ConditionTB.Enabled = False
            Else
                CustomNameTB.Enabled = False
                CustomColorCB.Enabled = True
                RandomColorB.Enabled = True
                ConditionTB.Enabled = False
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
            RefreshChainListList
            RefreshSupplementaryFields()
        End Sub
    End Class
End Namespace