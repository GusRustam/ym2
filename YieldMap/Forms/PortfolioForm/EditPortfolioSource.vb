Imports DbManager
Imports Uitls

Namespace Forms.PortfolioForm
    Public Class EditPortfolioSource
        Private ReadOnly _thePortfolio As Source

        Public ReadOnly Property Portfolio As Source
            Get
                Return _thePortfolio
            End Get
        End Property

        Public Property CustomName() As String
            Get
                Return CustomNameTB.Text
            End Get
            Set(ByVal value As String)
                CustomNameTB.Text = value
            End Set
        End Property

        Public Property CustomColor() As String
            Get
                Return CustomColorCB.SelectedText
            End Get
            Set(ByVal value As String)
                CustomColorCB.SelectedItem = value
            End Set
        End Property

        Public Property Condition() As String
            Get
                Return ConditionTB.Text
            End Get
            Set(ByVal value As String)
                ConditionTB.Text = value
            End Set
        End Property

        Sub New(ByVal p As Source)
            InitializeComponent()
            _thePortfolio = p
        End Sub

        Private Sub RandomColorButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles RandomColorB.Click
            CustomColorCB.SelectedIndex = New Random().NextDouble() * CustomColorCB.Items.Count
        End Sub

        Private Sub ColorComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CustomColorCB.SelectedIndexChanged
            If CustomColorCB.SelectedIndex < 0 Then
                SampleColorPB.BackColor = Color.White
            Else
                SampleColorPB.BackColor = Color.FromName(CustomColorCB.SelectedItem)
            End If
        End Sub

        Private Sub AddEditPortfolioSource_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Shown
            For Each clr In Utils.GetColorList()
                CustomColorCB.Items.Add(clr)
            Next

            If TypeOf Portfolio Is Chain OrElse TypeOf Portfolio Is UserList Then
                MainTabControl.SelectedTab = ChainOrListTP

                IndividualAndCustomBondsTP.Visible = False
                IndividualAndCustomBondsTP.Enabled = False

                ChainOrListTP.Visible = True
                ShowChainsRB.Checked = TypeOf Portfolio Is Chain
                ShowListsRB.Checked = Not ShowChainsRB.Checked
                ShowChainsRB.Enabled = ShowChainsRB.Checked
                ShowListsRB.Enabled = Not ShowChainsRB.Checked

                ChainsListsLB.DataSource =
                 If(ShowChainsRB.Checked,
                     (From item In PortfolioManager.Instance.ChainsView Select New IdName(Of String)(item.ID, item.Name)).ToList(),
                     (From item In PortfolioManager.Instance.UserListsView Select New IdName(Of String)(item.ID, item.Name)).ToList())

                Dim itms = (From elem In ChainsListsLB.Items Let id = CType(elem, IdName(Of String)).Id Where id = Portfolio.ID Select elem).ToList()
                If itms.Any() Then ChainsListsLB.SelectedItem = itms.First()

            ElseIf TypeOf Portfolio Is CustomBond OrElse TypeOf Portfolio Is RegularBond Then
                MainTabControl.SelectedTab = IndividualAndCustomBondsTP

                IndividualAndCustomBondsTP.Visible = True

                ChainOrListTP.Visible = False
                ChainOrListTP.Enabled = False

                IndBondsRB.Checked = TypeOf Portfolio Is RegularBond
                IndBondsRB.Enabled = IndBondsRB.Checked
                CustomBondsRB.Checked = Not IndBondsRB.Checked
                CustomBondsRB.Enabled = Not IndBondsRB.Checked

                ' ReSharper disable ConditionalTernaryEqualBranch
                BondsDGV.DataSource = If(IndBondsRB.Checked, Nothing, Nothing)
                ' ReSharper restore ConditionalTernaryEqualBranch
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

        Private Sub ClearCustomColorB_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ClearCustomColorB.Click
            CustomColorCB.SelectedIndex = -1
        End Sub
    End Class
End Namespace