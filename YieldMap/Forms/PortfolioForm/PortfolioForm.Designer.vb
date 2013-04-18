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
            Dim Label5 As System.Windows.Forms.Label
            Dim Label9 As System.Windows.Forms.Label
            Dim Label6 As System.Windows.Forms.Label
            Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim Label4 As System.Windows.Forms.Label
            Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle17 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle18 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim Label10 As System.Windows.Forms.Label
            Dim Label11 As System.Windows.Forms.Label
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
            Me.Panel2 = New System.Windows.Forms.Panel()
            Me.PerpetualCB = New System.Windows.Forms.CheckBox()
            Me.BulletPaymentDTP = New System.Windows.Forms.DateTimePicker()
            Me.MaturityDTP = New System.Windows.Forms.DateTimePicker()
            Me.BulletPaymentRB = New System.Windows.Forms.RadioButton()
            Me.AmortScheduleRB = New System.Windows.Forms.RadioButton()
            Me.AmortScheduleDGV = New System.Windows.Forms.DataGridView()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.Panel1 = New System.Windows.Forms.Panel()
            Me.FixedCouponRB = New System.Windows.Forms.RadioButton()
            Me.FrequencyCB = New System.Windows.Forms.ComboBox()
            Me.CouponScheduleRB = New System.Windows.Forms.RadioButton()
            Me.CouponScheduleDGV = New System.Windows.Forms.DataGridView()
            Me.CashFlowsDGV = New System.Windows.Forms.DataGridView()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.OptionsDGV = New System.Windows.Forms.DataGridView()
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
            Me.CustomBondColorCB = New System.Windows.Forms.ComboBox()
            Me.CustomBondColorPB = New System.Windows.Forms.PictureBox()
            Me.RandomColorButton = New System.Windows.Forms.Button()
            Me.MessagesTB = New System.Windows.Forms.TextBox()
            Me.RecalculateButton = New System.Windows.Forms.Button()
            Label1 = New System.Windows.Forms.Label()
            Label5 = New System.Windows.Forms.Label()
            Label9 = New System.Windows.Forms.Label()
            Label6 = New System.Windows.Forms.Label()
            Label4 = New System.Windows.Forms.Label()
            Label10 = New System.Windows.Forms.Label()
            Label11 = New System.Windows.Forms.Label()
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
            Me.Panel2.SuspendLayout()
            CType(Me.AmortScheduleDGV, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.Panel1.SuspendLayout()
            CType(Me.CouponScheduleDGV, System.ComponentModel.ISupportInitialize).BeginInit()
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
            CType(Me.CustomBondColorPB, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'Label1
            '
            Label1.AutoSize = True
            Label1.Location = New System.Drawing.Point(3, 3)
            Label1.Margin = New System.Windows.Forms.Padding(3)
            Label1.Name = "Label1"
            Label1.Size = New System.Drawing.Size(74, 13)
            Label1.TabIndex = 0
            Label1.Text = "Custom bonds"
            '
            'Label5
            '
            Label5.AutoSize = True
            Label5.Location = New System.Drawing.Point(106, 15)
            Label5.Name = "Label5"
            Label5.Size = New System.Drawing.Size(54, 13)
            Label5.TabIndex = 30
            Label5.Text = "frequency"
            '
            'Label9
            '
            Label9.AutoSize = True
            Label9.Location = New System.Drawing.Point(8, 209)
            Label9.Name = "Label9"
            Label9.Size = New System.Drawing.Size(16, 13)
            Label9.TabIndex = 38
            Label9.Text = "or"
            '
            'Label6
            '
            Label6.AutoSize = True
            Label6.Location = New System.Drawing.Point(111, 20)
            Label6.Name = "Label6"
            Label6.Size = New System.Drawing.Size(16, 13)
            Label6.TabIndex = 40
            Label6.Text = "at"
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
            DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.PortfolioChainsListsGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
            Me.PortfolioChainsListsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
            DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
            DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.PortfolioChainsListsGrid.DefaultCellStyle = DataGridViewCellStyle2
            Me.PortfolioChainsListsGrid.Dock = System.Windows.Forms.DockStyle.Fill
            Me.PortfolioChainsListsGrid.Location = New System.Drawing.Point(3, 33)
            Me.PortfolioChainsListsGrid.Name = "PortfolioChainsListsGrid"
            DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.PortfolioChainsListsGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
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
            DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.PortfolioItemsGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
            Me.PortfolioItemsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
            DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
            DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.PortfolioItemsGrid.DefaultCellStyle = DataGridViewCellStyle5
            Me.PortfolioItemsGrid.Dock = System.Windows.Forms.DockStyle.Fill
            Me.PortfolioItemsGrid.Location = New System.Drawing.Point(3, 33)
            Me.PortfolioItemsGrid.Name = "PortfolioItemsGrid"
            DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.PortfolioItemsGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle6
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
            DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.ChainsListsGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle7
            Me.ChainsListsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window
            DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText
            DataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.ChainsListsGrid.DefaultCellStyle = DataGridViewCellStyle8
            Me.ChainsListsGrid.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ChainsListsGrid.Location = New System.Drawing.Point(3, 28)
            Me.ChainsListsGrid.Name = "ChainsListsGrid"
            DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.ChainsListsGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle9
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
            DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.ChainListItemsGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle10
            Me.ChainListItemsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window
            DataGridViewCellStyle11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText
            DataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.ChainListItemsGrid.DefaultCellStyle = DataGridViewCellStyle11
            Me.ChainListItemsGrid.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ChainListItemsGrid.Location = New System.Drawing.Point(3, 28)
            Me.ChainListItemsGrid.Name = "ChainListItemsGrid"
            DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.ChainListItemsGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle12
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
            Me.BondsSC.Panel2.Controls.Add(Me.RecalculateButton)
            Me.BondsSC.Panel2.Controls.Add(Me.RandomColorButton)
            Me.BondsSC.Panel2.Controls.Add(Me.CustomBondColorPB)
            Me.BondsSC.Panel2.Controls.Add(Me.CustomBondColorCB)
            Me.BondsSC.Panel2.Controls.Add(Label10)
            Me.BondsSC.Panel2.Controls.Add(Me.Panel2)
            Me.BondsSC.Panel2.Controls.Add(Me.Label8)
            Me.BondsSC.Panel2.Controls.Add(Me.Panel1)
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
            Me.TableLayoutPanel3.Controls.Add(Label1, 0, 0)
            Me.TableLayoutPanel3.Controls.Add(Me.CustomBondsList, 0, 1)
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
            Me.CustomBondsList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.CustomBondsList.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CustomBondsList.Location = New System.Drawing.Point(3, 28)
            Me.CustomBondsList.Name = "CustomBondsList"
            Me.CustomBondsList.Size = New System.Drawing.Size(282, 529)
            Me.CustomBondsList.TabIndex = 1
            '
            'Panel2
            '
            Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.Panel2.Controls.Add(Me.PerpetualCB)
            Me.Panel2.Controls.Add(Me.BulletPaymentDTP)
            Me.Panel2.Controls.Add(Me.MaturityDTP)
            Me.Panel2.Controls.Add(Me.BulletPaymentRB)
            Me.Panel2.Controls.Add(Me.AmortScheduleRB)
            Me.Panel2.Controls.Add(Me.AmortScheduleDGV)
            Me.Panel2.Controls.Add(Label9)
            Me.Panel2.Controls.Add(Me.Label7)
            Me.Panel2.Controls.Add(Label6)
            Me.Panel2.Location = New System.Drawing.Point(14, 195)
            Me.Panel2.Name = "Panel2"
            Me.Panel2.Size = New System.Drawing.Size(279, 240)
            Me.Panel2.TabIndex = 41
            '
            'PerpetualCB
            '
            Me.PerpetualCB.AutoSize = True
            Me.PerpetualCB.Location = New System.Drawing.Point(29, 208)
            Me.PerpetualCB.Name = "PerpetualCB"
            Me.PerpetualCB.Size = New System.Drawing.Size(98, 17)
            Me.PerpetualCB.TabIndex = 46
            Me.PerpetualCB.Text = "Perpetual bond"
            Me.PerpetualCB.UseVisualStyleBackColor = True
            '
            'BulletPaymentDTP
            '
            Me.BulletPaymentDTP.CustomFormat = "dd/MM/yyyy"
            Me.BulletPaymentDTP.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.BulletPaymentDTP.Location = New System.Drawing.Point(134, 16)
            Me.BulletPaymentDTP.Name = "BulletPaymentDTP"
            Me.BulletPaymentDTP.Size = New System.Drawing.Size(126, 20)
            Me.BulletPaymentDTP.TabIndex = 44
            '
            'MaturityDTP
            '
            Me.MaturityDTP.CustomFormat = "dd/MM/yyyy"
            Me.MaturityDTP.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.MaturityDTP.Location = New System.Drawing.Point(71, 182)
            Me.MaturityDTP.Name = "MaturityDTP"
            Me.MaturityDTP.Size = New System.Drawing.Size(86, 20)
            Me.MaturityDTP.TabIndex = 45
            '
            'BulletPaymentRB
            '
            Me.BulletPaymentRB.AutoSize = True
            Me.BulletPaymentRB.Checked = True
            Me.BulletPaymentRB.Location = New System.Drawing.Point(11, 18)
            Me.BulletPaymentRB.Name = "BulletPaymentRB"
            Me.BulletPaymentRB.Size = New System.Drawing.Size(94, 17)
            Me.BulletPaymentRB.TabIndex = 41
            Me.BulletPaymentRB.TabStop = True
            Me.BulletPaymentRB.Text = "Bullet payment"
            Me.BulletPaymentRB.UseVisualStyleBackColor = True
            '
            'AmortScheduleRB
            '
            Me.AmortScheduleRB.AutoSize = True
            Me.AmortScheduleRB.Location = New System.Drawing.Point(11, 41)
            Me.AmortScheduleRB.Name = "AmortScheduleRB"
            Me.AmortScheduleRB.Size = New System.Drawing.Size(128, 17)
            Me.AmortScheduleRB.TabIndex = 42
            Me.AmortScheduleRB.Text = "Amortization schedule"
            Me.AmortScheduleRB.UseVisualStyleBackColor = True
            '
            'AmortScheduleDGV
            '
            Me.AmortScheduleDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.AmortScheduleDGV.Location = New System.Drawing.Point(11, 64)
            Me.AmortScheduleDGV.Name = "AmortScheduleDGV"
            Me.AmortScheduleDGV.Size = New System.Drawing.Size(249, 112)
            Me.AmortScheduleDGV.TabIndex = 43
            '
            'Label7
            '
            Me.Label7.AutoSize = True
            Me.Label7.Location = New System.Drawing.Point(8, 186)
            Me.Label7.Name = "Label7"
            Me.Label7.Size = New System.Drawing.Size(57, 13)
            Me.Label7.TabIndex = 39
            Me.Label7.Text = "Matures at"
            '
            'Label8
            '
            Me.Label8.AutoSize = True
            Me.Label8.Location = New System.Drawing.Point(306, 233)
            Me.Label8.Name = "Label8"
            Me.Label8.Size = New System.Drawing.Size(58, 13)
            Me.Label8.TabIndex = 40
            Me.Label8.Text = "Cash flows"
            '
            'Panel1
            '
            Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.Panel1.Controls.Add(Me.FixedCouponRB)
            Me.Panel1.Controls.Add(Label5)
            Me.Panel1.Controls.Add(Me.FrequencyCB)
            Me.Panel1.Controls.Add(Me.CouponScheduleRB)
            Me.Panel1.Controls.Add(Me.CouponScheduleDGV)
            Me.Panel1.Location = New System.Drawing.Point(14, 13)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(279, 176)
            Me.Panel1.TabIndex = 39
            '
            'FixedCouponRB
            '
            Me.FixedCouponRB.AutoSize = True
            Me.FixedCouponRB.Checked = True
            Me.FixedCouponRB.Location = New System.Drawing.Point(11, 13)
            Me.FixedCouponRB.Name = "FixedCouponRB"
            Me.FixedCouponRB.Size = New System.Drawing.Size(89, 17)
            Me.FixedCouponRB.TabIndex = 27
            Me.FixedCouponRB.TabStop = True
            Me.FixedCouponRB.Text = "Fixed coupon"
            Me.FixedCouponRB.UseVisualStyleBackColor = True
            '
            'FrequencyCB
            '
            Me.FrequencyCB.FormattingEnabled = True
            Me.FrequencyCB.Location = New System.Drawing.Point(166, 12)
            Me.FrequencyCB.Name = "FrequencyCB"
            Me.FrequencyCB.Size = New System.Drawing.Size(94, 21)
            Me.FrequencyCB.TabIndex = 31
            '
            'CouponScheduleRB
            '
            Me.CouponScheduleRB.AutoSize = True
            Me.CouponScheduleRB.Location = New System.Drawing.Point(11, 36)
            Me.CouponScheduleRB.Name = "CouponScheduleRB"
            Me.CouponScheduleRB.Size = New System.Drawing.Size(108, 17)
            Me.CouponScheduleRB.TabIndex = 28
            Me.CouponScheduleRB.Text = "Coupon schedule"
            Me.CouponScheduleRB.UseVisualStyleBackColor = True
            '
            'CouponScheduleDGV
            '
            Me.CouponScheduleDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.CouponScheduleDGV.Location = New System.Drawing.Point(11, 59)
            Me.CouponScheduleDGV.Name = "CouponScheduleDGV"
            Me.CouponScheduleDGV.Size = New System.Drawing.Size(249, 97)
            Me.CouponScheduleDGV.TabIndex = 29
            '
            'CashFlowsDGV
            '
            Me.CashFlowsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.CashFlowsDGV.Location = New System.Drawing.Point(309, 249)
            Me.CashFlowsDGV.Name = "CashFlowsDGV"
            Me.CashFlowsDGV.Size = New System.Drawing.Size(296, 159)
            Me.CashFlowsDGV.TabIndex = 38
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(309, 12)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(43, 13)
            Me.Label3.TabIndex = 34
            Me.Label3.Text = "Options"
            '
            'OptionsDGV
            '
            Me.OptionsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.OptionsDGV.Location = New System.Drawing.Point(309, 28)
            Me.OptionsDGV.Name = "OptionsDGV"
            Me.OptionsDGV.Size = New System.Drawing.Size(296, 145)
            Me.OptionsDGV.TabIndex = 35
            '
            'OtherRulesML
            '
            Me.OtherRulesML.Location = New System.Drawing.Point(14, 464)
            Me.OtherRulesML.Multiline = True
            Me.OtherRulesML.Name = "OtherRulesML"
            Me.OtherRulesML.Size = New System.Drawing.Size(279, 73)
            Me.OtherRulesML.TabIndex = 28
            '
            'Label4
            '
            Label4.AutoSize = True
            Label4.Location = New System.Drawing.Point(11, 448)
            Label4.Name = "Label4"
            Label4.Size = New System.Drawing.Size(78, 13)
            Label4.TabIndex = 27
            Label4.Text = "Additional rules"
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
            DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.FieldsGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle13
            Me.FieldsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window
            DataGridViewCellStyle14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText
            DataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.FieldsGrid.DefaultCellStyle = DataGridViewCellStyle14
            Me.FieldsGrid.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FieldsGrid.Location = New System.Drawing.Point(601, 3)
            Me.FieldsGrid.Name = "FieldsGrid"
            DataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.FieldsGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle15
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
            DataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.BondsTableView.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle16
            Me.BondsTableView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            DataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Window
            DataGridViewCellStyle17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.ControlText
            DataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.BondsTableView.DefaultCellStyle = DataGridViewCellStyle17
            Me.BondsTableView.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BondsTableView.Location = New System.Drawing.Point(210, 3)
            Me.BondsTableView.Name = "BondsTableView"
            DataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            DataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            DataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.BondsTableView.RowHeadersDefaultCellStyle = DataGridViewCellStyle18
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
            'Label10
            '
            Label10.AutoSize = True
            Label10.Location = New System.Drawing.Point(306, 192)
            Label10.Name = "Label10"
            Label10.Size = New System.Drawing.Size(67, 13)
            Label10.TabIndex = 42
            Label10.Text = "Default color"
            '
            'CustomBondColorCB
            '
            Me.CustomBondColorCB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
            Me.CustomBondColorCB.FormattingEnabled = True
            Me.CustomBondColorCB.Location = New System.Drawing.Point(379, 189)
            Me.CustomBondColorCB.Name = "CustomBondColorCB"
            Me.CustomBondColorCB.Size = New System.Drawing.Size(121, 21)
            Me.CustomBondColorCB.TabIndex = 43
            '
            'CustomBondColorPB
            '
            Me.CustomBondColorPB.Location = New System.Drawing.Point(506, 189)
            Me.CustomBondColorPB.Name = "CustomBondColorPB"
            Me.CustomBondColorPB.Size = New System.Drawing.Size(38, 21)
            Me.CustomBondColorPB.TabIndex = 44
            Me.CustomBondColorPB.TabStop = False
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
            'Label11
            '
            Label11.AutoSize = True
            Label11.Location = New System.Drawing.Point(306, 448)
            Label11.Name = "Label11"
            Label11.Size = New System.Drawing.Size(55, 13)
            Label11.TabIndex = 27
            Label11.Text = "Messages"
            '
            'MessagesTB
            '
            Me.MessagesTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.MessagesTB.Location = New System.Drawing.Point(309, 464)
            Me.MessagesTB.Multiline = True
            Me.MessagesTB.Name = "MessagesTB"
            Me.MessagesTB.ReadOnly = True
            Me.MessagesTB.Size = New System.Drawing.Size(296, 73)
            Me.MessagesTB.TabIndex = 28
            '
            'RecalculateButton
            '
            Me.RecalculateButton.Location = New System.Drawing.Point(530, 414)
            Me.RecalculateButton.Name = "RecalculateButton"
            Me.RecalculateButton.Size = New System.Drawing.Size(75, 21)
            Me.RecalculateButton.TabIndex = 46
            Me.RecalculateButton.Text = "Recalculate"
            Me.RecalculateButton.UseVisualStyleBackColor = True
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
            Me.Panel2.ResumeLayout(False)
            Me.Panel2.PerformLayout()
            CType(Me.AmortScheduleDGV, System.ComponentModel.ISupportInitialize).EndInit()
            Me.Panel1.ResumeLayout(False)
            Me.Panel1.PerformLayout()
            CType(Me.CouponScheduleDGV, System.ComponentModel.ISupportInitialize).EndInit()
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
            CType(Me.CustomBondColorPB, System.ComponentModel.ISupportInitialize).EndInit()
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
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents PerpetualCB As System.Windows.Forms.CheckBox
        Friend WithEvents BulletPaymentDTP As System.Windows.Forms.DateTimePicker
        Friend WithEvents MaturityDTP As System.Windows.Forms.DateTimePicker
        Friend WithEvents BulletPaymentRB As System.Windows.Forms.RadioButton
        Friend WithEvents AmortScheduleRB As System.Windows.Forms.RadioButton
        Friend WithEvents AmortScheduleDGV As System.Windows.Forms.DataGridView
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents FixedCouponRB As System.Windows.Forms.RadioButton
        Friend WithEvents FrequencyCB As System.Windows.Forms.ComboBox
        Friend WithEvents CouponScheduleRB As System.Windows.Forms.RadioButton
        Friend WithEvents CouponScheduleDGV As System.Windows.Forms.DataGridView
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
    End Class
End Namespace