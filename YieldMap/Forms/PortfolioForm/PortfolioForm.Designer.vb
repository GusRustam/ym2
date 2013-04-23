Namespace Forms.PortfolioForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class PortfolioForm
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim Label1 As System.Windows.Forms.Label
            Dim Label9 As System.Windows.Forms.Label
            Dim Label4 As System.Windows.Forms.Label
            Dim Label10 As System.Windows.Forms.Label
            Dim Label11 As System.Windows.Forms.Label
            Dim Label6 As System.Windows.Forms.Label
            Dim Label14 As System.Windows.Forms.Label
            Dim Label12 As System.Windows.Forms.Label
            Dim Label5 As System.Windows.Forms.Label
            Dim Label15 As System.Windows.Forms.Label
            Dim Label16 As System.Windows.Forms.Label
            Dim DataGridViewCellStyle19 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle20 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle21 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle22 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle23 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle24 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle25 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle26 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle27 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle28 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle29 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle30 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle31 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle32 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle33 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle34 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle35 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle36 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Me.MainTabControl = New System.Windows.Forms.TabControl()
            Me.PortfoliosPage = New System.Windows.Forms.TabPage()
            Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
            Me.PortfolioTree = New System.Windows.Forms.TreeView()
            Me.TabControl1 = New System.Windows.Forms.TabControl()
            Me.TabPage2 = New System.Windows.Forms.TabPage()
            Me.TableLayoutPanel6 = New System.Windows.Forms.TableLayoutPanel()
            Me.FlowLayoutPanel10 = New System.Windows.Forms.FlowLayoutPanel()
            Me.AddChainListButton = New System.Windows.Forms.Button()
            Me.RemoveChainListButton = New System.Windows.Forms.Button()
            Me.EditChainListButton = New System.Windows.Forms.Button()
            Me.PortfolioChainsListsGrid = New System.Windows.Forms.DataGridView()
            Me.FlowLayoutPanel9 = New System.Windows.Forms.FlowLayoutPanel()
            Me.ChainsCB = New System.Windows.Forms.CheckBox()
            Me.ListsCB = New System.Windows.Forms.CheckBox()
            Me.TabPage3 = New System.Windows.Forms.TabPage()
            Me.TableLayoutPanel7 = New System.Windows.Forms.TableLayoutPanel()
            Me.PortfolioItemsGrid = New System.Windows.Forms.DataGridView()
            Me.FlowLayoutPanel7 = New System.Windows.Forms.FlowLayoutPanel()
            Me.AllRB = New System.Windows.Forms.RadioButton()
            Me.SeparateRB = New System.Windows.Forms.RadioButton()
            Me.ChainsPage = New System.Windows.Forms.TabPage()
            Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
            Me.TableLayoutPanel8 = New System.Windows.Forms.TableLayoutPanel()
            Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
            Me.AddCLButton = New System.Windows.Forms.Button()
            Me.EditCLButton = New System.Windows.Forms.Button()
            Me.DeleteCLButton = New System.Windows.Forms.Button()
            Me.ReloadChainButton = New System.Windows.Forms.Button()
            Me.ChainsListsGrid = New System.Windows.Forms.DataGridView()
            Me.FlowLayoutPanel3 = New System.Windows.Forms.FlowLayoutPanel()
            Me.ChainsButton = New System.Windows.Forms.RadioButton()
            Me.ListsButton = New System.Windows.Forms.RadioButton()
            Me.TableLayoutPanel9 = New System.Windows.Forms.TableLayoutPanel()
            Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel()
            Me.AddItemsButton = New System.Windows.Forms.Button()
            Me.DeleteItemsButton = New System.Windows.Forms.Button()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.ChainListItemsGrid = New System.Windows.Forms.DataGridView()
            Me.CurvesPage = New System.Windows.Forms.TabPage()
            Me.TableLayoutPanel5 = New System.Windows.Forms.TableLayoutPanel()
            Me.CustomBondsPage = New System.Windows.Forms.TabPage()
            Me.BondsSC = New System.Windows.Forms.SplitContainer()
            Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
            Me.CustomBondsList = New System.Windows.Forms.DataGridView()
            Me.AnnuityCB = New System.Windows.Forms.CheckBox()
            Me.FixedRateTB = New System.Windows.Forms.TextBox()
            Me.FrequencyCB = New System.Windows.Forms.ComboBox()
            Me.CouponScheduleDGV = New System.Windows.Forms.DataGridView()
            Me.AmortScheduleDGV = New System.Windows.Forms.DataGridView()
            Me.UnspecifiedIssueDateCB = New System.Windows.Forms.CheckBox()
            Me.PerpetualCB = New System.Windows.Forms.CheckBox()
            Me.SaveButton = New System.Windows.Forms.Button()
            Me.IssueDateDTP = New System.Windows.Forms.DateTimePicker()
            Me.MaturityDTP = New System.Windows.Forms.DateTimePicker()
            Me.RecalculateButton = New System.Windows.Forms.Button()
            Me.RandomColorButton = New System.Windows.Forms.Button()
            Me.CustomBondColorPB = New System.Windows.Forms.PictureBox()
            Me.Label13 = New System.Windows.Forms.Label()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.CustomBondColorCB = New System.Windows.Forms.ComboBox()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.CashFlowsDGV = New System.Windows.Forms.DataGridView()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.OptionsDGV = New System.Windows.Forms.DataGridView()
            Me.MessagesTB = New System.Windows.Forms.TextBox()
            Me.OtherRulesML = New System.Windows.Forms.TextBox()
            Me.FieldsPage = New System.Windows.Forms.TabPage()
            Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
            Me.FieldLayoutsListBox = New System.Windows.Forms.ListBox()
            Me.FieldsListBox = New System.Windows.Forms.ListBox()
            Me.FieldsGrid = New System.Windows.Forms.DataGridView()
            Me.DataPage = New System.Windows.Forms.TabPage()
            Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
            Me.BondsTableView = New System.Windows.Forms.DataGridView()
            Me.TableChooserList = New System.Windows.Forms.ListBox()
            Me.FlowLayoutPanel4 = New System.Windows.Forms.FlowLayoutPanel()
            Me.CleanupDataButton = New System.Windows.Forms.Button()
            Me.ReloadDataButton = New System.Windows.Forms.Button()
            Me.PortTreeCM = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.AddToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
            Me.RenameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ChainsListsCMS = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.AddCLTSMI = New System.Windows.Forms.ToolStripMenuItem()
            Me.EditCLTSMI = New System.Windows.Forms.ToolStripMenuItem()
            Me.DeleteCLTSMI = New System.Windows.Forms.ToolStripMenuItem()
            Me.ReloadCLTSMI = New System.Windows.Forms.ToolStripMenuItem()
            Me.CustomBondListCMS = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.AddNewCustomBondTSMI = New System.Windows.Forms.ToolStripMenuItem()
            Me.DeleteCustomBondTSMI = New System.Windows.Forms.ToolStripMenuItem()
            Me.RenameCustomBondTSMI = New System.Windows.Forms.ToolStripMenuItem()
            Me.EditManCB = New System.Windows.Forms.CheckBox()
            Label1 = New System.Windows.Forms.Label()
            Label9 = New System.Windows.Forms.Label()
            Label4 = New System.Windows.Forms.Label()
            Label10 = New System.Windows.Forms.Label()
            Label11 = New System.Windows.Forms.Label()
            Label6 = New System.Windows.Forms.Label()
            Label14 = New System.Windows.Forms.Label()
            Label12 = New System.Windows.Forms.Label()
            Label5 = New System.Windows.Forms.Label()
            Label15 = New System.Windows.Forms.Label()
            Label16 = New System.Windows.Forms.Label()
            Me.MainTabControl.SuspendLayout()
            Me.PortfoliosPage.SuspendLayout()
            Me.TableLayoutPanel2.SuspendLayout()
            Me.TabControl1.SuspendLayout()
            Me.TabPage2.SuspendLayout()
            Me.TableLayoutPanel6.SuspendLayout()
            Me.FlowLayoutPanel10.SuspendLayout()
            CType(Me.PortfolioChainsListsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.FlowLayoutPanel9.SuspendLayout()
            Me.TabPage3.SuspendLayout()
            Me.TableLayoutPanel7.SuspendLayout()
            CType(Me.PortfolioItemsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.FlowLayoutPanel7.SuspendLayout()
            Me.ChainsPage.SuspendLayout()
            CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SplitContainer1.Panel1.SuspendLayout()
            Me.SplitContainer1.Panel2.SuspendLayout()
            Me.SplitContainer1.SuspendLayout()
            Me.TableLayoutPanel8.SuspendLayout()
            Me.FlowLayoutPanel1.SuspendLayout()
            CType(Me.ChainsListsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.FlowLayoutPanel3.SuspendLayout()
            Me.TableLayoutPanel9.SuspendLayout()
            Me.FlowLayoutPanel2.SuspendLayout()
            CType(Me.ChainListItemsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.CurvesPage.SuspendLayout()
            Me.CustomBondsPage.SuspendLayout()
            CType(Me.BondsSC, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.BondsSC.Panel1.SuspendLayout()
            Me.BondsSC.Panel2.SuspendLayout()
            Me.BondsSC.SuspendLayout()
            Me.TableLayoutPanel3.SuspendLayout()
            CType(Me.CustomBondsList, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.CouponScheduleDGV, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.AmortScheduleDGV, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.CustomBondColorPB, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.CashFlowsDGV, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.OptionsDGV, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.FieldsPage.SuspendLayout()
            Me.TableLayoutPanel4.SuspendLayout()
            CType(Me.FieldsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.DataPage.SuspendLayout()
            Me.TableLayoutPanel1.SuspendLayout()
            CType(Me.BondsTableView, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.FlowLayoutPanel4.SuspendLayout()
            Me.PortTreeCM.SuspendLayout()
            Me.ChainsListsCMS.SuspendLayout()
            Me.CustomBondListCMS.SuspendLayout()
            Me.SuspendLayout()
            '
            'Label1
            '
            Label1.AutoSize = True
            Label1.Location = New System.Drawing.Point(3, 10)
            Label1.Margin = New System.Windows.Forms.Padding(3, 10, 3, 3)
            Label1.Name = "Label1"
            Label1.Size = New System.Drawing.Size(74, 12)
            Label1.TabIndex = 0
            Label1.Text = "Custom bonds"
            '
            'Label9
            '
            Label9.AutoSize = True
            Label9.Location = New System.Drawing.Point(156, 34)
            Label9.Name = "Label9"
            Label9.Size = New System.Drawing.Size(16, 13)
            Label9.TabIndex = 38
            Label9.Text = "or"
            '
            'Label4
            '
            Label4.AutoSize = True
            Label4.Location = New System.Drawing.Point(-1, 427)
            Label4.Name = "Label4"
            Label4.Size = New System.Drawing.Size(53, 13)
            Label4.TabIndex = 27
            Label4.Text = "Structure "
            '
            'Label10
            '
            Label10.AutoSize = True
            Label10.Location = New System.Drawing.Point(296, 194)
            Label10.Name = "Label10"
            Label10.Size = New System.Drawing.Size(31, 13)
            Label10.TabIndex = 42
            Label10.Text = "Color"
            '
            'Label11
            '
            Label11.AutoSize = True
            Label11.Location = New System.Drawing.Point(296, 426)
            Label11.Name = "Label11"
            Label11.Size = New System.Drawing.Size(55, 13)
            Label11.TabIndex = 27
            Label11.Text = "Messages"
            '
            'Label6
            '
            Label6.AutoSize = True
            Label6.Location = New System.Drawing.Point(-1, 270)
            Label6.Name = "Label6"
            Label6.Size = New System.Drawing.Size(110, 13)
            Label6.TabIndex = 48
            Label6.Text = "Amortization schedule"
            '
            'Label14
            '
            Label14.AutoSize = True
            Label14.Location = New System.Drawing.Point(156, 11)
            Label14.Name = "Label14"
            Label14.Size = New System.Drawing.Size(16, 13)
            Label14.TabIndex = 38
            Label14.Text = "or"
            '
            'Label12
            '
            Label12.AutoSize = True
            Label12.Location = New System.Drawing.Point(-1, 100)
            Label12.Name = "Label12"
            Label12.Size = New System.Drawing.Size(53, 13)
            Label12.TabIndex = 53
            Label12.Text = "Fixed rate"
            '
            'Label5
            '
            Label5.AutoSize = True
            Label5.Location = New System.Drawing.Point(-1, 73)
            Label5.Name = "Label5"
            Label5.Size = New System.Drawing.Size(98, 13)
            Label5.TabIndex = 51
            Label5.Text = "Payment frequency"
            '
            'Label15
            '
            Label15.AutoSize = True
            Label15.Location = New System.Drawing.Point(-1, 125)
            Label15.Name = "Label15"
            Label15.Size = New System.Drawing.Size(90, 13)
            Label15.TabIndex = 48
            Label15.Text = "Coupon schedule"
            '
            'Label16
            '
            Label16.AutoSize = True
            Label16.Location = New System.Drawing.Point(111, 270)
            Label16.Name = "Label16"
            Label16.Size = New System.Drawing.Size(16, 13)
            Label16.TabIndex = 55
            Label16.Text = "or"
            '
            'MainTabControl
            '
            Me.MainTabControl.Controls.Add(Me.PortfoliosPage)
            Me.MainTabControl.Controls.Add(Me.ChainsPage)
            Me.MainTabControl.Controls.Add(Me.CurvesPage)
            Me.MainTabControl.Controls.Add(Me.CustomBondsPage)
            Me.MainTabControl.Controls.Add(Me.FieldsPage)
            Me.MainTabControl.Controls.Add(Me.DataPage)
            Me.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill
            Me.MainTabControl.Location = New System.Drawing.Point(0, 0)
            Me.MainTabControl.Name = "MainTabControl"
            Me.MainTabControl.SelectedIndex = 0
            Me.MainTabControl.Size = New System.Drawing.Size(913, 586)
            Me.MainTabControl.TabIndex = 0
            '
            'PortfoliosPage
            '
            Me.PortfoliosPage.Controls.Add(Me.TableLayoutPanel2)
            Me.PortfoliosPage.Location = New System.Drawing.Point(4, 22)
            Me.PortfoliosPage.Name = "PortfoliosPage"
            Me.PortfoliosPage.Padding = New System.Windows.Forms.Padding(3)
            Me.PortfoliosPage.Size = New System.Drawing.Size(905, 560)
            Me.PortfoliosPage.TabIndex = 0
            Me.PortfoliosPage.Text = "Portfolios"
            Me.PortfoliosPage.UseVisualStyleBackColor = True
            '
            'TableLayoutPanel2
            '
            Me.TableLayoutPanel2.ColumnCount = 2
            Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.0!))
            Me.TableLayoutPanel2.Controls.Add(Me.PortfolioTree, 0, 0)
            Me.TableLayoutPanel2.Controls.Add(Me.TabControl1, 1, 0)
            Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 3)
            Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
            Me.TableLayoutPanel2.RowCount = 1
            Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel2.Size = New System.Drawing.Size(899, 554)
            Me.TableLayoutPanel2.TabIndex = 0
            '
            'PortfolioTree
            '
            Me.PortfolioTree.AllowDrop = True
            Me.PortfolioTree.Dock = System.Windows.Forms.DockStyle.Fill
            Me.PortfolioTree.Location = New System.Drawing.Point(3, 3)
            Me.PortfolioTree.Name = "PortfolioTree"
            Me.PortfolioTree.Size = New System.Drawing.Size(218, 548)
            Me.PortfolioTree.TabIndex = 0
            '
            'TabControl1
            '
            Me.TabControl1.Controls.Add(Me.TabPage2)
            Me.TabControl1.Controls.Add(Me.TabPage3)
            Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TabControl1.Location = New System.Drawing.Point(227, 3)
            Me.TabControl1.Name = "TabControl1"
            Me.TabControl1.SelectedIndex = 0
            Me.TabControl1.Size = New System.Drawing.Size(669, 548)
            Me.TabControl1.TabIndex = 6
            '
            'TabPage2
            '
            Me.TabPage2.Controls.Add(Me.TableLayoutPanel6)
            Me.TabPage2.Location = New System.Drawing.Point(4, 22)
            Me.TabPage2.Name = "TabPage2"
            Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage2.Size = New System.Drawing.Size(661, 522)
            Me.TabPage2.TabIndex = 0
            Me.TabPage2.Text = "Chains and lists"
            Me.TabPage2.UseVisualStyleBackColor = True
            '
            'TableLayoutPanel6
            '
            Me.TableLayoutPanel6.ColumnCount = 1
            Me.TableLayoutPanel6.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel6.Controls.Add(Me.FlowLayoutPanel10, 0, 2)
            Me.TableLayoutPanel6.Controls.Add(Me.PortfolioChainsListsGrid, 0, 1)
            Me.TableLayoutPanel6.Controls.Add(Me.FlowLayoutPanel9, 0, 0)
            Me.TableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel6.Location = New System.Drawing.Point(3, 3)
            Me.TableLayoutPanel6.Name = "TableLayoutPanel6"
            Me.TableLayoutPanel6.RowCount = 3
            Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel6.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            Me.TableLayoutPanel6.Size = New System.Drawing.Size(655, 516)
            Me.TableLayoutPanel6.TabIndex = 0
            '
            'FlowLayoutPanel10
            '
            Me.FlowLayoutPanel10.Controls.Add(Me.AddChainListButton)
            Me.FlowLayoutPanel10.Controls.Add(Me.RemoveChainListButton)
            Me.FlowLayoutPanel10.Controls.Add(Me.EditChainListButton)
            Me.FlowLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FlowLayoutPanel10.Location = New System.Drawing.Point(3, 489)
            Me.FlowLayoutPanel10.Name = "FlowLayoutPanel10"
            Me.FlowLayoutPanel10.Size = New System.Drawing.Size(649, 24)
            Me.FlowLayoutPanel10.TabIndex = 7
            '
            'AddChainListButton
            '
            Me.AddChainListButton.Enabled = False
            Me.AddChainListButton.Location = New System.Drawing.Point(3, 3)
            Me.AddChainListButton.Name = "AddChainListButton"
            Me.AddChainListButton.Size = New System.Drawing.Size(36, 21)
            Me.AddChainListButton.TabIndex = 0
            Me.AddChainListButton.Text = "Add"
            Me.AddChainListButton.UseVisualStyleBackColor = True
            '
            'RemoveChainListButton
            '
            Me.RemoveChainListButton.Enabled = False
            Me.RemoveChainListButton.Location = New System.Drawing.Point(45, 3)
            Me.RemoveChainListButton.Name = "RemoveChainListButton"
            Me.RemoveChainListButton.Size = New System.Drawing.Size(56, 21)
            Me.RemoveChainListButton.TabIndex = 1
            Me.RemoveChainListButton.Text = "Remove"
            Me.RemoveChainListButton.UseVisualStyleBackColor = True
            '
            'EditChainListButton
            '
            Me.EditChainListButton.Enabled = False
            Me.EditChainListButton.Location = New System.Drawing.Point(107, 3)
            Me.EditChainListButton.Name = "EditChainListButton"
            Me.EditChainListButton.Size = New System.Drawing.Size(43, 21)
            Me.EditChainListButton.TabIndex = 4
            Me.EditChainListButton.Text = "Edit"
            Me.EditChainListButton.UseVisualStyleBackColor = True
            '
            'PortfolioChainsListsGrid
            '
            Me.PortfolioChainsListsGrid.AllowUserToAddRows = False
            Me.PortfolioChainsListsGrid.AllowUserToDeleteRows = False
            Me.PortfolioChainsListsGrid.AllowUserToResizeColumns = False
            DataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.PortfolioChainsListsGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle19
            Me.PortfolioChainsListsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            DataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Window
            DataGridViewCellStyle20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.ControlText
            DataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.PortfolioChainsListsGrid.DefaultCellStyle = DataGridViewCellStyle20
            Me.PortfolioChainsListsGrid.Dock = System.Windows.Forms.DockStyle.Fill
            Me.PortfolioChainsListsGrid.Location = New System.Drawing.Point(3, 33)
            Me.PortfolioChainsListsGrid.Name = "PortfolioChainsListsGrid"
            DataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle21.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle21.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.PortfolioChainsListsGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle21
            Me.PortfolioChainsListsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.PortfolioChainsListsGrid.Size = New System.Drawing.Size(649, 450)
            Me.PortfolioChainsListsGrid.TabIndex = 6
            '
            'FlowLayoutPanel9
            '
            Me.FlowLayoutPanel9.Controls.Add(Me.ChainsCB)
            Me.FlowLayoutPanel9.Controls.Add(Me.ListsCB)
            Me.FlowLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FlowLayoutPanel9.Location = New System.Drawing.Point(3, 3)
            Me.FlowLayoutPanel9.Name = "FlowLayoutPanel9"
            Me.FlowLayoutPanel9.Size = New System.Drawing.Size(649, 24)
            Me.FlowLayoutPanel9.TabIndex = 4
            '
            'ChainsCB
            '
            Me.ChainsCB.AutoSize = True
            Me.ChainsCB.Checked = True
            Me.ChainsCB.CheckState = System.Windows.Forms.CheckState.Checked
            Me.ChainsCB.Location = New System.Drawing.Point(3, 3)
            Me.ChainsCB.Name = "ChainsCB"
            Me.ChainsCB.Size = New System.Drawing.Size(58, 17)
            Me.ChainsCB.TabIndex = 0
            Me.ChainsCB.Text = "Chains"
            Me.ChainsCB.UseVisualStyleBackColor = True
            '
            'ListsCB
            '
            Me.ListsCB.AutoSize = True
            Me.ListsCB.Checked = True
            Me.ListsCB.CheckState = System.Windows.Forms.CheckState.Checked
            Me.ListsCB.Location = New System.Drawing.Point(67, 3)
            Me.ListsCB.Name = "ListsCB"
            Me.ListsCB.Size = New System.Drawing.Size(47, 17)
            Me.ListsCB.TabIndex = 1
            Me.ListsCB.Text = "Lists"
            Me.ListsCB.UseVisualStyleBackColor = True
            '
            'TabPage3
            '
            Me.TabPage3.Controls.Add(Me.TableLayoutPanel7)
            Me.TabPage3.Location = New System.Drawing.Point(4, 22)
            Me.TabPage3.Name = "TabPage3"
            Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage3.Size = New System.Drawing.Size(661, 522)
            Me.TabPage3.TabIndex = 1
            Me.TabPage3.Text = "Items"
            Me.TabPage3.UseVisualStyleBackColor = True
            '
            'TableLayoutPanel7
            '
            Me.TableLayoutPanel7.ColumnCount = 1
            Me.TableLayoutPanel7.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel7.Controls.Add(Me.PortfolioItemsGrid, 0, 1)
            Me.TableLayoutPanel7.Controls.Add(Me.FlowLayoutPanel7, 0, 0)
            Me.TableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel7.Location = New System.Drawing.Point(3, 3)
            Me.TableLayoutPanel7.Name = "TableLayoutPanel7"
            Me.TableLayoutPanel7.RowCount = 2
            Me.TableLayoutPanel7.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            Me.TableLayoutPanel7.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel7.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TableLayoutPanel7.Size = New System.Drawing.Size(655, 516)
            Me.TableLayoutPanel7.TabIndex = 0
            '
            'PortfolioItemsGrid
            '
            DataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle22.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle22.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.PortfolioItemsGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle22
            Me.PortfolioItemsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            DataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle23.BackColor = System.Drawing.SystemColors.Window
            DataGridViewCellStyle23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle23.ForeColor = System.Drawing.SystemColors.ControlText
            DataGridViewCellStyle23.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle23.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle23.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.PortfolioItemsGrid.DefaultCellStyle = DataGridViewCellStyle23
            Me.PortfolioItemsGrid.Dock = System.Windows.Forms.DockStyle.Fill
            Me.PortfolioItemsGrid.Location = New System.Drawing.Point(3, 33)
            Me.PortfolioItemsGrid.Name = "PortfolioItemsGrid"
            DataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle24.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle24.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle24.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle24.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle24.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.PortfolioItemsGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle24
            Me.PortfolioItemsGrid.Size = New System.Drawing.Size(649, 480)
            Me.PortfolioItemsGrid.TabIndex = 7
            '
            'FlowLayoutPanel7
            '
            Me.FlowLayoutPanel7.Controls.Add(Me.AllRB)
            Me.FlowLayoutPanel7.Controls.Add(Me.SeparateRB)
            Me.FlowLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FlowLayoutPanel7.Location = New System.Drawing.Point(3, 3)
            Me.FlowLayoutPanel7.Name = "FlowLayoutPanel7"
            Me.FlowLayoutPanel7.Size = New System.Drawing.Size(649, 24)
            Me.FlowLayoutPanel7.TabIndex = 6
            '
            'AllRB
            '
            Me.AllRB.AutoSize = True
            Me.AllRB.Checked = True
            Me.AllRB.Location = New System.Drawing.Point(3, 3)
            Me.AllRB.Name = "AllRB"
            Me.AllRB.Size = New System.Drawing.Size(63, 17)
            Me.AllRB.TabIndex = 0
            Me.AllRB.TabStop = True
            Me.AllRB.Text = "All items"
            Me.AllRB.UseVisualStyleBackColor = True
            '
            'SeparateRB
            '
            Me.SeparateRB.AutoSize = True
            Me.SeparateRB.Location = New System.Drawing.Point(72, 3)
            Me.SeparateRB.Name = "SeparateRB"
            Me.SeparateRB.Size = New System.Drawing.Size(184, 17)
            Me.SeparateRB.TabIndex = 1
            Me.SeparateRB.Text = "Included and excluded separately"
            Me.SeparateRB.UseVisualStyleBackColor = True
            '
            'ChainsPage
            '
            Me.ChainsPage.Controls.Add(Me.SplitContainer1)
            Me.ChainsPage.Location = New System.Drawing.Point(4, 22)
            Me.ChainsPage.Name = "ChainsPage"
            Me.ChainsPage.Padding = New System.Windows.Forms.Padding(3)
            Me.ChainsPage.Size = New System.Drawing.Size(905, 560)
            Me.ChainsPage.TabIndex = 2
            Me.ChainsPage.Text = "Chains and lists"
            Me.ChainsPage.UseVisualStyleBackColor = True
            '
            'SplitContainer1
            '
            Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.SplitContainer1.Location = New System.Drawing.Point(3, 3)
            Me.SplitContainer1.Name = "SplitContainer1"
            '
            'SplitContainer1.Panel1
            '
            Me.SplitContainer1.Panel1.Controls.Add(Me.TableLayoutPanel8)
            '
            'SplitContainer1.Panel2
            '
            Me.SplitContainer1.Panel2.Controls.Add(Me.TableLayoutPanel9)
            Me.SplitContainer1.Size = New System.Drawing.Size(899, 554)
            Me.SplitContainer1.SplitterDistance = 299
            Me.SplitContainer1.TabIndex = 12
            '
            'TableLayoutPanel8
            '
            Me.TableLayoutPanel8.ColumnCount = 1
            Me.TableLayoutPanel8.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel8.Controls.Add(Me.FlowLayoutPanel1, 0, 2)
            Me.TableLayoutPanel8.Controls.Add(Me.ChainsListsGrid, 0, 1)
            Me.TableLayoutPanel8.Controls.Add(Me.FlowLayoutPanel3, 0, 0)
            Me.TableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel8.Location = New System.Drawing.Point(0, 0)
            Me.TableLayoutPanel8.Name = "TableLayoutPanel8"
            Me.TableLayoutPanel8.RowCount = 3
            Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TableLayoutPanel8.Size = New System.Drawing.Size(299, 554)
            Me.TableLayoutPanel8.TabIndex = 11
            '
            'FlowLayoutPanel1
            '
            Me.FlowLayoutPanel1.Controls.Add(Me.AddCLButton)
            Me.FlowLayoutPanel1.Controls.Add(Me.EditCLButton)
            Me.FlowLayoutPanel1.Controls.Add(Me.DeleteCLButton)
            Me.FlowLayoutPanel1.Controls.Add(Me.ReloadChainButton)
            Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 534)
            Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
            Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
            Me.FlowLayoutPanel1.Size = New System.Drawing.Size(299, 20)
            Me.FlowLayoutPanel1.TabIndex = 11
            '
            'AddCLButton
            '
            Me.AddCLButton.Location = New System.Drawing.Point(0, 0)
            Me.AddCLButton.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
            Me.AddCLButton.Name = "AddCLButton"
            Me.AddCLButton.Size = New System.Drawing.Size(54, 20)
            Me.AddCLButton.TabIndex = 0
            Me.AddCLButton.Text = "Add"
            Me.AddCLButton.UseVisualStyleBackColor = True
            '
            'EditCLButton
            '
            Me.EditCLButton.Location = New System.Drawing.Point(57, 0)
            Me.EditCLButton.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
            Me.EditCLButton.Name = "EditCLButton"
            Me.EditCLButton.Size = New System.Drawing.Size(54, 20)
            Me.EditCLButton.TabIndex = 1
            Me.EditCLButton.Text = "Edit"
            Me.EditCLButton.UseVisualStyleBackColor = True
            '
            'DeleteCLButton
            '
            Me.DeleteCLButton.Location = New System.Drawing.Point(114, 0)
            Me.DeleteCLButton.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
            Me.DeleteCLButton.Name = "DeleteCLButton"
            Me.DeleteCLButton.Size = New System.Drawing.Size(54, 20)
            Me.DeleteCLButton.TabIndex = 2
            Me.DeleteCLButton.Text = "Delete"
            Me.DeleteCLButton.UseVisualStyleBackColor = True
            '
            'ReloadChainButton
            '
            Me.ReloadChainButton.Location = New System.Drawing.Point(171, 0)
            Me.ReloadChainButton.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
            Me.ReloadChainButton.Name = "ReloadChainButton"
            Me.ReloadChainButton.Size = New System.Drawing.Size(54, 20)
            Me.ReloadChainButton.TabIndex = 3
            Me.ReloadChainButton.Text = "Reload"
            Me.ReloadChainButton.UseVisualStyleBackColor = True
            '
            'ChainsListsGrid
            '
            Me.ChainsListsGrid.AllowUserToAddRows = False
            Me.ChainsListsGrid.AllowUserToDeleteRows = False
            Me.ChainsListsGrid.AllowUserToResizeRows = False
            DataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle25.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle25.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle25.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle25.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.ChainsListsGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle25
            Me.ChainsListsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            DataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle26.BackColor = System.Drawing.SystemColors.Window
            DataGridViewCellStyle26.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle26.ForeColor = System.Drawing.SystemColors.ControlText
            DataGridViewCellStyle26.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle26.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.ChainsListsGrid.DefaultCellStyle = DataGridViewCellStyle26
            Me.ChainsListsGrid.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ChainsListsGrid.Location = New System.Drawing.Point(3, 28)
            Me.ChainsListsGrid.Name = "ChainsListsGrid"
            DataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle27.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle27.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle27.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle27.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle27.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle27.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.ChainsListsGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle27
            Me.ChainsListsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.ChainsListsGrid.Size = New System.Drawing.Size(293, 503)
            Me.ChainsListsGrid.TabIndex = 10
            '
            'FlowLayoutPanel3
            '
            Me.FlowLayoutPanel3.Controls.Add(Me.ChainsButton)
            Me.FlowLayoutPanel3.Controls.Add(Me.ListsButton)
            Me.FlowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top
            Me.FlowLayoutPanel3.Location = New System.Drawing.Point(0, 0)
            Me.FlowLayoutPanel3.Margin = New System.Windows.Forms.Padding(0)
            Me.FlowLayoutPanel3.Name = "FlowLayoutPanel3"
            Me.FlowLayoutPanel3.Size = New System.Drawing.Size(299, 20)
            Me.FlowLayoutPanel3.TabIndex = 9
            '
            'ChainsButton
            '
            Me.ChainsButton.AutoSize = True
            Me.ChainsButton.Checked = True
            Me.ChainsButton.Location = New System.Drawing.Point(0, 0)
            Me.ChainsButton.Margin = New System.Windows.Forms.Padding(0)
            Me.ChainsButton.Name = "ChainsButton"
            Me.ChainsButton.Size = New System.Drawing.Size(57, 17)
            Me.ChainsButton.TabIndex = 8
            Me.ChainsButton.TabStop = True
            Me.ChainsButton.Text = "Chains"
            Me.ChainsButton.UseVisualStyleBackColor = True
            '
            'ListsButton
            '
            Me.ListsButton.AutoSize = True
            Me.ListsButton.Location = New System.Drawing.Point(57, 0)
            Me.ListsButton.Margin = New System.Windows.Forms.Padding(0)
            Me.ListsButton.Name = "ListsButton"
            Me.ListsButton.Size = New System.Drawing.Size(46, 17)
            Me.ListsButton.TabIndex = 9
            Me.ListsButton.Text = "Lists"
            Me.ListsButton.UseVisualStyleBackColor = True
            '
            'TableLayoutPanel9
            '
            Me.TableLayoutPanel9.ColumnCount = 1
            Me.TableLayoutPanel9.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel9.Controls.Add(Me.FlowLayoutPanel2, 0, 2)
            Me.TableLayoutPanel9.Controls.Add(Me.Label2, 0, 0)
            Me.TableLayoutPanel9.Controls.Add(Me.ChainListItemsGrid, 0, 1)
            Me.TableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel9.Location = New System.Drawing.Point(0, 0)
            Me.TableLayoutPanel9.Name = "TableLayoutPanel9"
            Me.TableLayoutPanel9.RowCount = 3
            Me.TableLayoutPanel9.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TableLayoutPanel9.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel9.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TableLayoutPanel9.Size = New System.Drawing.Size(596, 554)
            Me.TableLayoutPanel9.TabIndex = 12
            '
            'FlowLayoutPanel2
            '
            Me.FlowLayoutPanel2.Controls.Add(Me.AddItemsButton)
            Me.FlowLayoutPanel2.Controls.Add(Me.DeleteItemsButton)
            Me.FlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FlowLayoutPanel2.Location = New System.Drawing.Point(0, 534)
            Me.FlowLayoutPanel2.Margin = New System.Windows.Forms.Padding(0)
            Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
            Me.FlowLayoutPanel2.Size = New System.Drawing.Size(596, 20)
            Me.FlowLayoutPanel2.TabIndex = 7
            '
            'AddItemsButton
            '
            Me.AddItemsButton.Location = New System.Drawing.Point(0, 0)
            Me.AddItemsButton.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
            Me.AddItemsButton.Name = "AddItemsButton"
            Me.AddItemsButton.Size = New System.Drawing.Size(75, 20)
            Me.AddItemsButton.TabIndex = 1
            Me.AddItemsButton.Text = "Add"
            Me.AddItemsButton.UseVisualStyleBackColor = True
            '
            'DeleteItemsButton
            '
            Me.DeleteItemsButton.Location = New System.Drawing.Point(78, 0)
            Me.DeleteItemsButton.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
            Me.DeleteItemsButton.Name = "DeleteItemsButton"
            Me.DeleteItemsButton.Size = New System.Drawing.Size(75, 20)
            Me.DeleteItemsButton.TabIndex = 3
            Me.DeleteItemsButton.Text = "Delete"
            Me.DeleteItemsButton.UseVisualStyleBackColor = True
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(3, 0)
            Me.Label2.Name = "Label2"
            Me.Label2.Padding = New System.Windows.Forms.Padding(0, 3, 0, 0)
            Me.Label2.Size = New System.Drawing.Size(32, 16)
            Me.Label2.TabIndex = 4
            Me.Label2.Text = "Items"
            '
            'ChainListItemsGrid
            '
            Me.ChainListItemsGrid.AllowUserToAddRows = False
            Me.ChainListItemsGrid.AllowUserToDeleteRows = False
            Me.ChainListItemsGrid.AllowUserToResizeRows = False
            DataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle28.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle28.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle28.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle28.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle28.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.ChainListItemsGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle28
            Me.ChainListItemsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            DataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle29.BackColor = System.Drawing.SystemColors.Window
            DataGridViewCellStyle29.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle29.ForeColor = System.Drawing.SystemColors.ControlText
            DataGridViewCellStyle29.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle29.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle29.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.ChainListItemsGrid.DefaultCellStyle = DataGridViewCellStyle29
            Me.ChainListItemsGrid.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ChainListItemsGrid.Location = New System.Drawing.Point(3, 28)
            Me.ChainListItemsGrid.Name = "ChainListItemsGrid"
            DataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle30.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle30.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle30.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle30.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle30.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle30.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.ChainListItemsGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle30
            Me.ChainListItemsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.ChainListItemsGrid.Size = New System.Drawing.Size(590, 503)
            Me.ChainListItemsGrid.TabIndex = 3
            '
            'CurvesPage
            '
            Me.CurvesPage.Controls.Add(Me.TableLayoutPanel5)
            Me.CurvesPage.Location = New System.Drawing.Point(4, 22)
            Me.CurvesPage.Name = "CurvesPage"
            Me.CurvesPage.Padding = New System.Windows.Forms.Padding(3)
            Me.CurvesPage.Size = New System.Drawing.Size(905, 560)
            Me.CurvesPage.TabIndex = 5
            Me.CurvesPage.Text = "Curves"
            Me.CurvesPage.UseVisualStyleBackColor = True
            '
            'TableLayoutPanel5
            '
            Me.TableLayoutPanel5.ColumnCount = 2
            Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel5.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel5.Location = New System.Drawing.Point(3, 3)
            Me.TableLayoutPanel5.Name = "TableLayoutPanel5"
            Me.TableLayoutPanel5.RowCount = 2
            Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel5.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel5.Size = New System.Drawing.Size(899, 554)
            Me.TableLayoutPanel5.TabIndex = 0
            '
            'CustomBondsPage
            '
            Me.CustomBondsPage.Controls.Add(Me.BondsSC)
            Me.CustomBondsPage.Location = New System.Drawing.Point(4, 22)
            Me.CustomBondsPage.Name = "CustomBondsPage"
            Me.CustomBondsPage.Size = New System.Drawing.Size(905, 560)
            Me.CustomBondsPage.TabIndex = 6
            Me.CustomBondsPage.Text = "Custom bonds"
            Me.CustomBondsPage.UseVisualStyleBackColor = True
            '
            'BondsSC
            '
            Me.BondsSC.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BondsSC.Location = New System.Drawing.Point(0, 0)
            Me.BondsSC.Name = "BondsSC"
            '
            'BondsSC.Panel1
            '
            Me.BondsSC.Panel1.Controls.Add(Me.TableLayoutPanel3)
            '
            'BondsSC.Panel2
            '
            Me.BondsSC.Panel2.Controls.Add(Me.EditManCB)
            Me.BondsSC.Panel2.Controls.Add(Me.AnnuityCB)
            Me.BondsSC.Panel2.Controls.Add(Label16)
            Me.BondsSC.Panel2.Controls.Add(Me.FixedRateTB)
            Me.BondsSC.Panel2.Controls.Add(Label12)
            Me.BondsSC.Panel2.Controls.Add(Label5)
            Me.BondsSC.Panel2.Controls.Add(Me.FrequencyCB)
            Me.BondsSC.Panel2.Controls.Add(Me.CouponScheduleDGV)
            Me.BondsSC.Panel2.Controls.Add(Label15)
            Me.BondsSC.Panel2.Controls.Add(Label6)
            Me.BondsSC.Panel2.Controls.Add(Me.AmortScheduleDGV)
            Me.BondsSC.Panel2.Controls.Add(Me.UnspecifiedIssueDateCB)
            Me.BondsSC.Panel2.Controls.Add(Me.PerpetualCB)
            Me.BondsSC.Panel2.Controls.Add(Me.SaveButton)
            Me.BondsSC.Panel2.Controls.Add(Me.IssueDateDTP)
            Me.BondsSC.Panel2.Controls.Add(Me.MaturityDTP)
            Me.BondsSC.Panel2.Controls.Add(Me.RecalculateButton)
            Me.BondsSC.Panel2.Controls.Add(Me.RandomColorButton)
            Me.BondsSC.Panel2.Controls.Add(Me.CustomBondColorPB)
            Me.BondsSC.Panel2.Controls.Add(Me.Label13)
            Me.BondsSC.Panel2.Controls.Add(Label14)
            Me.BondsSC.Panel2.Controls.Add(Me.Label7)
            Me.BondsSC.Panel2.Controls.Add(Label9)
            Me.BondsSC.Panel2.Controls.Add(Me.CustomBondColorCB)
            Me.BondsSC.Panel2.Controls.Add(Label10)
            Me.BondsSC.Panel2.Controls.Add(Me.Label8)
            Me.BondsSC.Panel2.Controls.Add(Me.CashFlowsDGV)
            Me.BondsSC.Panel2.Controls.Add(Me.Label3)
            Me.BondsSC.Panel2.Controls.Add(Me.OptionsDGV)
            Me.BondsSC.Panel2.Controls.Add(Me.MessagesTB)
            Me.BondsSC.Panel2.Controls.Add(Label11)
            Me.BondsSC.Panel2.Controls.Add(Me.OtherRulesML)
            Me.BondsSC.Panel2.Controls.Add(Label4)
            Me.BondsSC.Size = New System.Drawing.Size(905, 560)
            Me.BondsSC.SplitterDistance = 288
            Me.BondsSC.TabIndex = 0
            '
            'TableLayoutPanel3
            '
            Me.TableLayoutPanel3.ColumnCount = 1
            Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel3.Controls.Add(Me.CustomBondsList, 0, 1)
            Me.TableLayoutPanel3.Controls.Add(Label1, 0, 0)
            Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel3.Location = New System.Drawing.Point(0, 0)
            Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
            Me.TableLayoutPanel3.RowCount = 2
            Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel3.Size = New System.Drawing.Size(288, 560)
            Me.TableLayoutPanel3.TabIndex = 0
            '
            'CustomBondsList
            '
            Me.CustomBondsList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
            Me.CustomBondsList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.CustomBondsList.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CustomBondsList.Location = New System.Drawing.Point(3, 28)
            Me.CustomBondsList.Name = "CustomBondsList"
            Me.CustomBondsList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.CustomBondsList.Size = New System.Drawing.Size(282, 529)
            Me.CustomBondsList.TabIndex = 1
            '
            'AnnuityCB
            '
            Me.AnnuityCB.AutoSize = True
            Me.AnnuityCB.Location = New System.Drawing.Point(133, 269)
            Me.AnnuityCB.Name = "AnnuityCB"
            Me.AnnuityCB.Size = New System.Drawing.Size(60, 17)
            Me.AnnuityCB.TabIndex = 56
            Me.AnnuityCB.Text = "annuity"
            Me.AnnuityCB.UseVisualStyleBackColor = True
            '
            'FixedRateTB
            '
            Me.FixedRateTB.Location = New System.Drawing.Point(117, 97)
            Me.FixedRateTB.Name = "FixedRateTB"
            Me.FixedRateTB.Size = New System.Drawing.Size(100, 20)
            Me.FixedRateTB.TabIndex = 54
            '
            'FrequencyCB
            '
            Me.FrequencyCB.FormattingEnabled = True
            Me.FrequencyCB.Items.AddRange(New Object() {"1", "2", "4", "12", "Y", "SQMA", "28D", "90D", "91D", "92D", "182D", "364D", "90D", "180D", "365D", "S0", "R1", "R2", "R4"})
            Me.FrequencyCB.Location = New System.Drawing.Point(117, 70)
            Me.FrequencyCB.Name = "FrequencyCB"
            Me.FrequencyCB.Size = New System.Drawing.Size(100, 21)
            Me.FrequencyCB.TabIndex = 52
            '
            'CouponScheduleDGV
            '
            Me.CouponScheduleDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
            Me.CouponScheduleDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.CouponScheduleDGV.Location = New System.Drawing.Point(2, 141)
            Me.CouponScheduleDGV.Name = "CouponScheduleDGV"
            Me.CouponScheduleDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.CouponScheduleDGV.Size = New System.Drawing.Size(279, 114)
            Me.CouponScheduleDGV.TabIndex = 50
            '
            'AmortScheduleDGV
            '
            Me.AmortScheduleDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
            Me.AmortScheduleDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.AmortScheduleDGV.Location = New System.Drawing.Point(2, 288)
            Me.AmortScheduleDGV.Name = "AmortScheduleDGV"
            Me.AmortScheduleDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.AmortScheduleDGV.Size = New System.Drawing.Size(279, 136)
            Me.AmortScheduleDGV.TabIndex = 49
            '
            'UnspecifiedIssueDateCB
            '
            Me.UnspecifiedIssueDateCB.AutoSize = True
            Me.UnspecifiedIssueDateCB.Location = New System.Drawing.Point(178, 9)
            Me.UnspecifiedIssueDateCB.Name = "UnspecifiedIssueDateCB"
            Me.UnspecifiedIssueDateCB.Size = New System.Drawing.Size(80, 17)
            Me.UnspecifiedIssueDateCB.TabIndex = 46
            Me.UnspecifiedIssueDateCB.Text = "unspecified"
            Me.UnspecifiedIssueDateCB.UseVisualStyleBackColor = True
            '
            'PerpetualCB
            '
            Me.PerpetualCB.AutoSize = True
            Me.PerpetualCB.Location = New System.Drawing.Point(178, 33)
            Me.PerpetualCB.Name = "PerpetualCB"
            Me.PerpetualCB.Size = New System.Drawing.Size(70, 17)
            Me.PerpetualCB.TabIndex = 46
            Me.PerpetualCB.Text = "perpetual"
            Me.PerpetualCB.UseVisualStyleBackColor = True
            '
            'SaveButton
            '
            Me.SaveButton.Location = New System.Drawing.Point(530, 539)
            Me.SaveButton.Name = "SaveButton"
            Me.SaveButton.Size = New System.Drawing.Size(75, 21)
            Me.SaveButton.TabIndex = 47
            Me.SaveButton.Text = "Save"
            Me.SaveButton.UseVisualStyleBackColor = True
            '
            'IssueDateDTP
            '
            Me.IssueDateDTP.CustomFormat = "dd/MM/yyyy"
            Me.IssueDateDTP.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.IssueDateDTP.Location = New System.Drawing.Point(67, 6)
            Me.IssueDateDTP.Name = "IssueDateDTP"
            Me.IssueDateDTP.Size = New System.Drawing.Size(86, 20)
            Me.IssueDateDTP.TabIndex = 45
            '
            'MaturityDTP
            '
            Me.MaturityDTP.CustomFormat = "dd/MM/yyyy"
            Me.MaturityDTP.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.MaturityDTP.Location = New System.Drawing.Point(67, 30)
            Me.MaturityDTP.Name = "MaturityDTP"
            Me.MaturityDTP.Size = New System.Drawing.Size(86, 20)
            Me.MaturityDTP.TabIndex = 45
            '
            'RecalculateButton
            '
            Me.RecalculateButton.Location = New System.Drawing.Point(449, 539)
            Me.RecalculateButton.Name = "RecalculateButton"
            Me.RecalculateButton.Size = New System.Drawing.Size(75, 21)
            Me.RecalculateButton.TabIndex = 46
            Me.RecalculateButton.Text = "Recalculate"
            Me.RecalculateButton.UseVisualStyleBackColor = True
            '
            'RandomColorButton
            '
            Me.RandomColorButton.Location = New System.Drawing.Point(550, 187)
            Me.RandomColorButton.Name = "RandomColorButton"
            Me.RandomColorButton.Size = New System.Drawing.Size(55, 23)
            Me.RandomColorButton.TabIndex = 45
            Me.RandomColorButton.Text = "Random"
            Me.RandomColorButton.UseVisualStyleBackColor = True
            '
            'CustomBondColorPB
            '
            Me.CustomBondColorPB.Location = New System.Drawing.Point(506, 189)
            Me.CustomBondColorPB.Name = "CustomBondColorPB"
            Me.CustomBondColorPB.Size = New System.Drawing.Size(38, 21)
            Me.CustomBondColorPB.TabIndex = 44
            Me.CustomBondColorPB.TabStop = False
            '
            'Label13
            '
            Me.Label13.AutoSize = True
            Me.Label13.Location = New System.Drawing.Point(-1, 11)
            Me.Label13.Name = "Label13"
            Me.Label13.Size = New System.Drawing.Size(56, 13)
            Me.Label13.TabIndex = 39
            Me.Label13.Text = "Issue date"
            '
            'Label7
            '
            Me.Label7.AutoSize = True
            Me.Label7.Location = New System.Drawing.Point(-1, 33)
            Me.Label7.Name = "Label7"
            Me.Label7.Size = New System.Drawing.Size(57, 13)
            Me.Label7.TabIndex = 39
            Me.Label7.Text = "Matures at"
            '
            'CustomBondColorCB
            '
            Me.CustomBondColorCB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
            Me.CustomBondColorCB.FormattingEnabled = True
            Me.CustomBondColorCB.Location = New System.Drawing.Point(333, 189)
            Me.CustomBondColorCB.Name = "CustomBondColorCB"
            Me.CustomBondColorCB.Size = New System.Drawing.Size(167, 21)
            Me.CustomBondColorCB.TabIndex = 43
            '
            'Label8
            '
            Me.Label8.AutoSize = True
            Me.Label8.Location = New System.Drawing.Point(296, 215)
            Me.Label8.Name = "Label8"
            Me.Label8.Size = New System.Drawing.Size(58, 13)
            Me.Label8.TabIndex = 40
            Me.Label8.Text = "Cash flows"
            '
            'CashFlowsDGV
            '
            Me.CashFlowsDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
            Me.CashFlowsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.CashFlowsDGV.Location = New System.Drawing.Point(299, 232)
            Me.CashFlowsDGV.Name = "CashFlowsDGV"
            Me.CashFlowsDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.CashFlowsDGV.Size = New System.Drawing.Size(306, 192)
            Me.CashFlowsDGV.TabIndex = 38
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(296, 9)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(95, 13)
            Me.Label3.TabIndex = 34
            Me.Label3.Text = "Embedded options"
            '
            'OptionsDGV
            '
            Me.OptionsDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
            Me.OptionsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.OptionsDGV.Location = New System.Drawing.Point(299, 26)
            Me.OptionsDGV.Name = "OptionsDGV"
            Me.OptionsDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.OptionsDGV.Size = New System.Drawing.Size(306, 157)
            Me.OptionsDGV.TabIndex = 35
            '
            'MessagesTB
            '
            Me.MessagesTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.MessagesTB.Location = New System.Drawing.Point(299, 441)
            Me.MessagesTB.Multiline = True
            Me.MessagesTB.Name = "MessagesTB"
            Me.MessagesTB.ReadOnly = True
            Me.MessagesTB.Size = New System.Drawing.Size(306, 89)
            Me.MessagesTB.TabIndex = 28
            '
            'OtherRulesML
            '
            Me.OtherRulesML.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.OtherRulesML.Location = New System.Drawing.Point(2, 443)
            Me.OtherRulesML.Multiline = True
            Me.OtherRulesML.Name = "OtherRulesML"
            Me.OtherRulesML.ReadOnly = True
            Me.OtherRulesML.Size = New System.Drawing.Size(279, 114)
            Me.OtherRulesML.TabIndex = 28
            '
            'FieldsPage
            '
            Me.FieldsPage.Controls.Add(Me.TableLayoutPanel4)
            Me.FieldsPage.Location = New System.Drawing.Point(4, 22)
            Me.FieldsPage.Name = "FieldsPage"
            Me.FieldsPage.Padding = New System.Windows.Forms.Padding(3)
            Me.FieldsPage.Size = New System.Drawing.Size(905, 560)
            Me.FieldsPage.TabIndex = 4
            Me.FieldsPage.Text = "Fields"
            Me.FieldsPage.UseVisualStyleBackColor = True
            '
            'TableLayoutPanel4
            '
            Me.TableLayoutPanel4.ColumnCount = 3
            Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            Me.TableLayoutPanel4.Controls.Add(Me.FieldLayoutsListBox, 1, 0)
            Me.TableLayoutPanel4.Controls.Add(Me.FieldsListBox, 0, 0)
            Me.TableLayoutPanel4.Controls.Add(Me.FieldsGrid, 2, 0)
            Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel4.Location = New System.Drawing.Point(3, 3)
            Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
            Me.TableLayoutPanel4.RowCount = 1
            Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 554.0!))
            Me.TableLayoutPanel4.Size = New System.Drawing.Size(899, 554)
            Me.TableLayoutPanel4.TabIndex = 0
            '
            'FieldLayoutsListBox
            '
            Me.FieldLayoutsListBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FieldLayoutsListBox.FormattingEnabled = True
            Me.FieldLayoutsListBox.Location = New System.Drawing.Point(302, 3)
            Me.FieldLayoutsListBox.Name = "FieldLayoutsListBox"
            Me.FieldLayoutsListBox.Size = New System.Drawing.Size(293, 548)
            Me.FieldLayoutsListBox.TabIndex = 5
            '
            'FieldsListBox
            '
            Me.FieldsListBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FieldsListBox.FormattingEnabled = True
            Me.FieldsListBox.Location = New System.Drawing.Point(3, 3)
            Me.FieldsListBox.Name = "FieldsListBox"
            Me.FieldsListBox.Size = New System.Drawing.Size(293, 548)
            Me.FieldsListBox.TabIndex = 0
            '
            'FieldsGrid
            '
            DataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle31.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle31.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle31.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle31.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle31.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle31.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.FieldsGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle31
            Me.FieldsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            DataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle32.BackColor = System.Drawing.SystemColors.Window
            DataGridViewCellStyle32.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle32.ForeColor = System.Drawing.SystemColors.ControlText
            DataGridViewCellStyle32.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle32.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle32.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.FieldsGrid.DefaultCellStyle = DataGridViewCellStyle32
            Me.FieldsGrid.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FieldsGrid.Location = New System.Drawing.Point(601, 3)
            Me.FieldsGrid.Name = "FieldsGrid"
            DataGridViewCellStyle33.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle33.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle33.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle33.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle33.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle33.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle33.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.FieldsGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle33
            Me.FieldsGrid.Size = New System.Drawing.Size(295, 548)
            Me.FieldsGrid.TabIndex = 2
            '
            'DataPage
            '
            Me.DataPage.Controls.Add(Me.TableLayoutPanel1)
            Me.DataPage.Location = New System.Drawing.Point(4, 22)
            Me.DataPage.Name = "DataPage"
            Me.DataPage.Padding = New System.Windows.Forms.Padding(3)
            Me.DataPage.Size = New System.Drawing.Size(905, 560)
            Me.DataPage.TabIndex = 1
            Me.DataPage.Text = "Data"
            Me.DataPage.UseVisualStyleBackColor = True
            '
            'TableLayoutPanel1
            '
            Me.TableLayoutPanel1.ColumnCount = 2
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.07692!))
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.92308!))
            Me.TableLayoutPanel1.Controls.Add(Me.BondsTableView, 1, 0)
            Me.TableLayoutPanel1.Controls.Add(Me.TableChooserList, 0, 0)
            Me.TableLayoutPanel1.Controls.Add(Me.FlowLayoutPanel4, 0, 1)
            Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 3)
            Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
            Me.TableLayoutPanel1.RowCount = 2
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TableLayoutPanel1.Size = New System.Drawing.Size(899, 554)
            Me.TableLayoutPanel1.TabIndex = 1
            '
            'BondsTableView
            '
            Me.BondsTableView.AllowUserToAddRows = False
            Me.BondsTableView.AllowUserToDeleteRows = False
            Me.BondsTableView.AllowUserToOrderColumns = True
            Me.BondsTableView.AllowUserToResizeRows = False
            DataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle34.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle34.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle34.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle34.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle34.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle34.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.BondsTableView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle34
            Me.BondsTableView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            DataGridViewCellStyle35.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle35.BackColor = System.Drawing.SystemColors.Window
            DataGridViewCellStyle35.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle35.ForeColor = System.Drawing.SystemColors.ControlText
            DataGridViewCellStyle35.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle35.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle35.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.BondsTableView.DefaultCellStyle = DataGridViewCellStyle35
            Me.BondsTableView.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BondsTableView.Location = New System.Drawing.Point(210, 3)
            Me.BondsTableView.Name = "BondsTableView"
            DataGridViewCellStyle36.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle36.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle36.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle36.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle36.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle36.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle36.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.BondsTableView.RowHeadersDefaultCellStyle = DataGridViewCellStyle36
            Me.TableLayoutPanel1.SetRowSpan(Me.BondsTableView, 2)
            Me.BondsTableView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.BondsTableView.Size = New System.Drawing.Size(686, 548)
            Me.BondsTableView.TabIndex = 1
            '
            'TableChooserList
            '
            Me.TableChooserList.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableChooserList.FormattingEnabled = True
            Me.TableChooserList.Items.AddRange(New Object() {"Bonds", "Coupons", "FRNs", "Issue ratings", "Issuer ratings", "Rics"})
            Me.TableChooserList.Location = New System.Drawing.Point(3, 3)
            Me.TableChooserList.Name = "TableChooserList"
            Me.TableChooserList.Size = New System.Drawing.Size(201, 523)
            Me.TableChooserList.TabIndex = 2
            '
            'FlowLayoutPanel4
            '
            Me.FlowLayoutPanel4.Controls.Add(Me.CleanupDataButton)
            Me.FlowLayoutPanel4.Controls.Add(Me.ReloadDataButton)
            Me.FlowLayoutPanel4.Location = New System.Drawing.Point(0, 529)
            Me.FlowLayoutPanel4.Margin = New System.Windows.Forms.Padding(0)
            Me.FlowLayoutPanel4.Name = "FlowLayoutPanel4"
            Me.FlowLayoutPanel4.Size = New System.Drawing.Size(196, 25)
            Me.FlowLayoutPanel4.TabIndex = 3
            '
            'CleanupDataButton
            '
            Me.CleanupDataButton.Location = New System.Drawing.Point(0, 0)
            Me.CleanupDataButton.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
            Me.CleanupDataButton.Name = "CleanupDataButton"
            Me.CleanupDataButton.Size = New System.Drawing.Size(75, 23)
            Me.CleanupDataButton.TabIndex = 0
            Me.CleanupDataButton.Text = "Cleanup"
            Me.CleanupDataButton.UseVisualStyleBackColor = True
            '
            'ReloadDataButton
            '
            Me.ReloadDataButton.Location = New System.Drawing.Point(78, 0)
            Me.ReloadDataButton.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
            Me.ReloadDataButton.Name = "ReloadDataButton"
            Me.ReloadDataButton.Size = New System.Drawing.Size(75, 23)
            Me.ReloadDataButton.TabIndex = 1
            Me.ReloadDataButton.Text = "Reload"
            Me.ReloadDataButton.UseVisualStyleBackColor = True
            '
            'PortTreeCM
            '
            Me.PortTreeCM.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddToolStripMenuItem, Me.ToolStripSeparator1, Me.RenameToolStripMenuItem, Me.DeleteToolStripMenuItem})
            Me.PortTreeCM.Name = "PortTreeCM"
            Me.PortTreeCM.Size = New System.Drawing.Size(114, 76)
            '
            'AddToolStripMenuItem
            '
            Me.AddToolStripMenuItem.Name = "AddToolStripMenuItem"
            Me.AddToolStripMenuItem.Size = New System.Drawing.Size(113, 22)
            Me.AddToolStripMenuItem.Text = "Add..."
            '
            'ToolStripSeparator1
            '
            Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
            Me.ToolStripSeparator1.Size = New System.Drawing.Size(110, 6)
            '
            'RenameToolStripMenuItem
            '
            Me.RenameToolStripMenuItem.Name = "RenameToolStripMenuItem"
            Me.RenameToolStripMenuItem.Size = New System.Drawing.Size(113, 22)
            Me.RenameToolStripMenuItem.Text = "Rename"
            '
            'DeleteToolStripMenuItem
            '
            Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
            Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(113, 22)
            Me.DeleteToolStripMenuItem.Text = "Delete"
            '
            'ChainsListsCMS
            '
            Me.ChainsListsCMS.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddCLTSMI, Me.EditCLTSMI, Me.DeleteCLTSMI, Me.ReloadCLTSMI})
            Me.ChainsListsCMS.Name = "ChainsListsCMS"
            Me.ChainsListsCMS.Size = New System.Drawing.Size(136, 92)
            '
            'AddCLTSMI
            '
            Me.AddCLTSMI.Name = "AddCLTSMI"
            Me.AddCLTSMI.Size = New System.Drawing.Size(135, 22)
            Me.AddCLTSMI.Text = "Add..."
            '
            'EditCLTSMI
            '
            Me.EditCLTSMI.Name = "EditCLTSMI"
            Me.EditCLTSMI.Size = New System.Drawing.Size(135, 22)
            Me.EditCLTSMI.Text = "Edit..."
            '
            'DeleteCLTSMI
            '
            Me.DeleteCLTSMI.Name = "DeleteCLTSMI"
            Me.DeleteCLTSMI.Size = New System.Drawing.Size(135, 22)
            Me.DeleteCLTSMI.Text = "Delete..."
            '
            'ReloadCLTSMI
            '
            Me.ReloadCLTSMI.Name = "ReloadCLTSMI"
            Me.ReloadCLTSMI.Size = New System.Drawing.Size(135, 22)
            Me.ReloadCLTSMI.Text = "Reload items"
            '
            'CustomBondListCMS
            '
            Me.CustomBondListCMS.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddNewCustomBondTSMI, Me.DeleteCustomBondTSMI, Me.RenameCustomBondTSMI})
            Me.CustomBondListCMS.Name = "CustomBondListCMS"
            Me.CustomBondListCMS.Size = New System.Drawing.Size(129, 70)
            '
            'AddNewCustomBondTSMI
            '
            Me.AddNewCustomBondTSMI.Name = "AddNewCustomBondTSMI"
            Me.AddNewCustomBondTSMI.Size = New System.Drawing.Size(128, 22)
            Me.AddNewCustomBondTSMI.Text = "Add new..."
            '
            'DeleteCustomBondTSMI
            '
            Me.DeleteCustomBondTSMI.Name = "DeleteCustomBondTSMI"
            Me.DeleteCustomBondTSMI.Size = New System.Drawing.Size(128, 22)
            Me.DeleteCustomBondTSMI.Text = "Delete"
            '
            'RenameCustomBondTSMI
            '
            Me.RenameCustomBondTSMI.Name = "RenameCustomBondTSMI"
            Me.RenameCustomBondTSMI.Size = New System.Drawing.Size(128, 22)
            Me.RenameCustomBondTSMI.Text = "Rename..."
            '
            'EditManCB
            '
            Me.EditManCB.AutoSize = True
            Me.EditManCB.Location = New System.Drawing.Point(299, 542)
            Me.EditManCB.Name = "EditManCB"
            Me.EditManCB.Size = New System.Drawing.Size(88, 17)
            Me.EditManCB.TabIndex = 57
            Me.EditManCB.Text = "Edit manually"
            Me.EditManCB.UseVisualStyleBackColor = True
            '
            'PortfolioForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(913, 586)
            Me.Controls.Add(Me.MainTabControl)
            Me.Name = "PortfolioForm"
            Me.Text = "Portfolio manager"
            Me.MainTabControl.ResumeLayout(False)
            Me.PortfoliosPage.ResumeLayout(False)
            Me.TableLayoutPanel2.ResumeLayout(False)
            Me.TabControl1.ResumeLayout(False)
            Me.TabPage2.ResumeLayout(False)
            Me.TableLayoutPanel6.ResumeLayout(False)
            Me.FlowLayoutPanel10.ResumeLayout(False)
            CType(Me.PortfolioChainsListsGrid, System.ComponentModel.ISupportInitialize).EndInit()
            Me.FlowLayoutPanel9.ResumeLayout(False)
            Me.FlowLayoutPanel9.PerformLayout()
            Me.TabPage3.ResumeLayout(False)
            Me.TableLayoutPanel7.ResumeLayout(False)
            CType(Me.PortfolioItemsGrid, System.ComponentModel.ISupportInitialize).EndInit()
            Me.FlowLayoutPanel7.ResumeLayout(False)
            Me.FlowLayoutPanel7.PerformLayout()
            Me.ChainsPage.ResumeLayout(False)
            Me.SplitContainer1.Panel1.ResumeLayout(False)
            Me.SplitContainer1.Panel2.ResumeLayout(False)
            CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.SplitContainer1.ResumeLayout(False)
            Me.TableLayoutPanel8.ResumeLayout(False)
            Me.FlowLayoutPanel1.ResumeLayout(False)
            CType(Me.ChainsListsGrid, System.ComponentModel.ISupportInitialize).EndInit()
            Me.FlowLayoutPanel3.ResumeLayout(False)
            Me.FlowLayoutPanel3.PerformLayout()
            Me.TableLayoutPanel9.ResumeLayout(False)
            Me.TableLayoutPanel9.PerformLayout()
            Me.FlowLayoutPanel2.ResumeLayout(False)
            CType(Me.ChainListItemsGrid, System.ComponentModel.ISupportInitialize).EndInit()
            Me.CurvesPage.ResumeLayout(False)
            Me.CustomBondsPage.ResumeLayout(False)
            Me.BondsSC.Panel1.ResumeLayout(False)
            Me.BondsSC.Panel2.ResumeLayout(False)
            Me.BondsSC.Panel2.PerformLayout()
            CType(Me.BondsSC, System.ComponentModel.ISupportInitialize).EndInit()
            Me.BondsSC.ResumeLayout(False)
            Me.TableLayoutPanel3.ResumeLayout(False)
            Me.TableLayoutPanel3.PerformLayout()
            CType(Me.CustomBondsList, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.CouponScheduleDGV, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.AmortScheduleDGV, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.CustomBondColorPB, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.CashFlowsDGV, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.OptionsDGV, System.ComponentModel.ISupportInitialize).EndInit()
            Me.FieldsPage.ResumeLayout(False)
            Me.TableLayoutPanel4.ResumeLayout(False)
            CType(Me.FieldsGrid, System.ComponentModel.ISupportInitialize).EndInit()
            Me.DataPage.ResumeLayout(False)
            Me.TableLayoutPanel1.ResumeLayout(False)
            CType(Me.BondsTableView, System.ComponentModel.ISupportInitialize).EndInit()
            Me.FlowLayoutPanel4.ResumeLayout(False)
            Me.PortTreeCM.ResumeLayout(False)
            Me.ChainsListsCMS.ResumeLayout(False)
            Me.CustomBondListCMS.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents MainTabControl As System.Windows.Forms.TabControl
        Friend WithEvents PortfoliosPage As System.Windows.Forms.TabPage
        Friend WithEvents DataPage As System.Windows.Forms.TabPage
        Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents BondsTableView As System.Windows.Forms.DataGridView
        Friend WithEvents TableChooserList As System.Windows.Forms.ListBox
        Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents PortfolioTree As System.Windows.Forms.TreeView
        Friend WithEvents ChainsPage As System.Windows.Forms.TabPage
        Friend WithEvents FieldsPage As System.Windows.Forms.TabPage
        Friend WithEvents FlowLayoutPanel4 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents CleanupDataButton As System.Windows.Forms.Button
        Friend WithEvents ReloadDataButton As System.Windows.Forms.Button
        Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents FieldsListBox As System.Windows.Forms.ListBox
        Friend WithEvents FieldsGrid As System.Windows.Forms.DataGridView
        Friend WithEvents PortTreeCM As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents AddToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents RenameToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents DeleteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
        Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
        Friend WithEvents TableLayoutPanel6 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents FlowLayoutPanel10 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents AddChainListButton As System.Windows.Forms.Button
        Friend WithEvents RemoveChainListButton As System.Windows.Forms.Button
        Friend WithEvents EditChainListButton As System.Windows.Forms.Button
        Friend WithEvents PortfolioChainsListsGrid As System.Windows.Forms.DataGridView
        Friend WithEvents FlowLayoutPanel9 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents ChainsCB As System.Windows.Forms.CheckBox
        Friend WithEvents ListsCB As System.Windows.Forms.CheckBox
        Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
        Friend WithEvents TableLayoutPanel7 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents PortfolioItemsGrid As System.Windows.Forms.DataGridView
        Friend WithEvents FlowLayoutPanel7 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents AllRB As System.Windows.Forms.RadioButton
        Friend WithEvents SeparateRB As System.Windows.Forms.RadioButton
        Friend WithEvents CurvesPage As System.Windows.Forms.TabPage
        Friend WithEvents TableLayoutPanel5 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents CustomBondsPage As System.Windows.Forms.TabPage
        Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
        Friend WithEvents TableLayoutPanel8 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents AddCLButton As System.Windows.Forms.Button
        Friend WithEvents EditCLButton As System.Windows.Forms.Button
        Friend WithEvents DeleteCLButton As System.Windows.Forms.Button
        Friend WithEvents ChainsListsGrid As System.Windows.Forms.DataGridView
        Friend WithEvents FlowLayoutPanel3 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents ChainsButton As System.Windows.Forms.RadioButton
        Friend WithEvents ListsButton As System.Windows.Forms.RadioButton
        Friend WithEvents TableLayoutPanel9 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents FlowLayoutPanel2 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents AddItemsButton As System.Windows.Forms.Button
        Friend WithEvents DeleteItemsButton As System.Windows.Forms.Button
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents ChainListItemsGrid As System.Windows.Forms.DataGridView
        Friend WithEvents FieldLayoutsListBox As System.Windows.Forms.ListBox
        Friend WithEvents ReloadChainButton As System.Windows.Forms.Button
        Friend WithEvents ChainsListsCMS As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents AddCLTSMI As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents EditCLTSMI As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents DeleteCLTSMI As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ReloadCLTSMI As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents BondsSC As System.Windows.Forms.SplitContainer
        Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents CustomBondsList As System.Windows.Forms.DataGridView
        Friend WithEvents PerpetualCB As System.Windows.Forms.CheckBox
        Friend WithEvents MaturityDTP As System.Windows.Forms.DateTimePicker
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents CashFlowsDGV As System.Windows.Forms.DataGridView
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents OptionsDGV As System.Windows.Forms.DataGridView
        Friend WithEvents OtherRulesML As System.Windows.Forms.TextBox
        Friend WithEvents CustomBondListCMS As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents AddNewCustomBondTSMI As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents DeleteCustomBondTSMI As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents RenameCustomBondTSMI As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents RandomColorButton As System.Windows.Forms.Button
        Friend WithEvents CustomBondColorPB As System.Windows.Forms.PictureBox
        Friend WithEvents CustomBondColorCB As System.Windows.Forms.ComboBox
        Friend WithEvents MessagesTB As System.Windows.Forms.TextBox
        Friend WithEvents RecalculateButton As System.Windows.Forms.Button
        Friend WithEvents SaveButton As System.Windows.Forms.Button
        Friend WithEvents AmortScheduleDGV As System.Windows.Forms.DataGridView
        Friend WithEvents UnspecifiedIssueDateCB As System.Windows.Forms.CheckBox
        Friend WithEvents IssueDateDTP As System.Windows.Forms.DateTimePicker
        Friend WithEvents Label13 As System.Windows.Forms.Label
        Friend WithEvents FixedRateTB As System.Windows.Forms.TextBox
        Friend WithEvents FrequencyCB As System.Windows.Forms.ComboBox
        Friend WithEvents CouponScheduleDGV As System.Windows.Forms.DataGridView
        Friend WithEvents AnnuityCB As System.Windows.Forms.CheckBox
        Friend WithEvents EditManCB As System.Windows.Forms.CheckBox
    End Class
End Namespace