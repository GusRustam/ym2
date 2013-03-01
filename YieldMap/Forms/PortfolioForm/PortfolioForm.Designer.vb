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
            Me.MainTabControl = New System.Windows.Forms.TabControl()
            Me.PortfoliosPage = New System.Windows.Forms.TabPage()
            Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
            Me.PortfolioTree = New System.Windows.Forms.TreeView()
            Me.PortfolioItemsGrid = New System.Windows.Forms.DataGridView()
            Me.ChainsPage = New System.Windows.Forms.TabPage()
            Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
            Me.ChainsListsGrid = New System.Windows.Forms.DataGridView()
            Me.ChainListItemsGrid = New System.Windows.Forms.DataGridView()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
            Me.AddCLButton = New System.Windows.Forms.Button()
            Me.EditCLButton = New System.Windows.Forms.Button()
            Me.ReloadCLButton = New System.Windows.Forms.Button()
            Me.DeleteCLButton = New System.Windows.Forms.Button()
            Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel()
            Me.AddItemsButton = New System.Windows.Forms.Button()
            Me.DeleteItemsButton = New System.Windows.Forms.Button()
            Me.FlowLayoutPanel3 = New System.Windows.Forms.FlowLayoutPanel()
            Me.ChainsButton = New System.Windows.Forms.RadioButton()
            Me.ListsButton = New System.Windows.Forms.RadioButton()
            Me.FieldsPage = New System.Windows.Forms.TabPage()
            Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
            Me.ListBox1 = New System.Windows.Forms.ListBox()
            Me.FlowLayoutPanel5 = New System.Windows.Forms.FlowLayoutPanel()
            Me.Button9 = New System.Windows.Forms.Button()
            Me.Button10 = New System.Windows.Forms.Button()
            Me.DataGridView4 = New System.Windows.Forms.DataGridView()
            Me.DataPage = New System.Windows.Forms.TabPage()
            Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
            Me.BondsTableView = New System.Windows.Forms.DataGridView()
            Me.TableChooserList = New System.Windows.Forms.ListBox()
            Me.FlowLayoutPanel4 = New System.Windows.Forms.FlowLayoutPanel()
            Me.Button7 = New System.Windows.Forms.Button()
            Me.Button8 = New System.Windows.Forms.Button()
            Me.MainTabControl.SuspendLayout()
            Me.PortfoliosPage.SuspendLayout()
            Me.TableLayoutPanel2.SuspendLayout()
            CType(Me.PortfolioItemsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.ChainsPage.SuspendLayout()
            Me.TableLayoutPanel3.SuspendLayout()
            CType(Me.ChainsListsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.ChainListItemsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.FlowLayoutPanel1.SuspendLayout()
            Me.FlowLayoutPanel2.SuspendLayout()
            Me.FlowLayoutPanel3.SuspendLayout()
            Me.FieldsPage.SuspendLayout()
            Me.TableLayoutPanel4.SuspendLayout()
            Me.FlowLayoutPanel5.SuspendLayout()
            CType(Me.DataGridView4, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.DataPage.SuspendLayout()
            Me.TableLayoutPanel1.SuspendLayout()
            CType(Me.BondsTableView, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.FlowLayoutPanel4.SuspendLayout()
            Me.SuspendLayout()
            '
            'MainTabControl
            '
            Me.MainTabControl.Controls.Add(Me.PortfoliosPage)
            Me.MainTabControl.Controls.Add(Me.ChainsPage)
            Me.MainTabControl.Controls.Add(Me.FieldsPage)
            Me.MainTabControl.Controls.Add(Me.DataPage)
            Me.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill
            Me.MainTabControl.Location = New System.Drawing.Point(0, 0)
            Me.MainTabControl.Name = "MainTabControl"
            Me.MainTabControl.SelectedIndex = 0
            Me.MainTabControl.Size = New System.Drawing.Size(866, 498)
            Me.MainTabControl.TabIndex = 0
            '
            'PortfoliosPage
            '
            Me.PortfoliosPage.Controls.Add(Me.TableLayoutPanel2)
            Me.PortfoliosPage.Location = New System.Drawing.Point(4, 22)
            Me.PortfoliosPage.Name = "PortfoliosPage"
            Me.PortfoliosPage.Padding = New System.Windows.Forms.Padding(3)
            Me.PortfoliosPage.Size = New System.Drawing.Size(858, 472)
            Me.PortfoliosPage.TabIndex = 0
            Me.PortfoliosPage.Text = "Portfolios"
            Me.PortfoliosPage.UseVisualStyleBackColor = True
            '
            'TableLayoutPanel2
            '
            Me.TableLayoutPanel2.ColumnCount = 2
            Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel2.Controls.Add(Me.PortfolioTree, 0, 0)
            Me.TableLayoutPanel2.Controls.Add(Me.PortfolioItemsGrid, 1, 0)
            Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 3)
            Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
            Me.TableLayoutPanel2.RowCount = 1
            Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel2.Size = New System.Drawing.Size(852, 466)
            Me.TableLayoutPanel2.TabIndex = 0
            '
            'PortfolioTree
            '
            Me.PortfolioTree.Dock = System.Windows.Forms.DockStyle.Fill
            Me.PortfolioTree.Location = New System.Drawing.Point(3, 3)
            Me.PortfolioTree.Name = "PortfolioTree"
            Me.PortfolioTree.Size = New System.Drawing.Size(420, 460)
            Me.PortfolioTree.TabIndex = 0
            '
            'PortfolioItemsGrid
            '
            Me.PortfolioItemsGrid.AllowUserToAddRows = False
            Me.PortfolioItemsGrid.AllowUserToDeleteRows = False
            Me.PortfolioItemsGrid.AllowUserToResizeColumns = False
            Me.PortfolioItemsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.PortfolioItemsGrid.Dock = System.Windows.Forms.DockStyle.Fill
            Me.PortfolioItemsGrid.Location = New System.Drawing.Point(429, 3)
            Me.PortfolioItemsGrid.Name = "PortfolioItemsGrid"
            Me.PortfolioItemsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.PortfolioItemsGrid.Size = New System.Drawing.Size(420, 460)
            Me.PortfolioItemsGrid.TabIndex = 1
            '
            'ChainsPage
            '
            Me.ChainsPage.Controls.Add(Me.TableLayoutPanel3)
            Me.ChainsPage.Location = New System.Drawing.Point(4, 22)
            Me.ChainsPage.Name = "ChainsPage"
            Me.ChainsPage.Padding = New System.Windows.Forms.Padding(3)
            Me.ChainsPage.Size = New System.Drawing.Size(858, 472)
            Me.ChainsPage.TabIndex = 2
            Me.ChainsPage.Text = "Chains and lists"
            Me.ChainsPage.UseVisualStyleBackColor = True
            '
            'TableLayoutPanel3
            '
            Me.TableLayoutPanel3.ColumnCount = 2
            Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel3.Controls.Add(Me.ChainsListsGrid, 0, 1)
            Me.TableLayoutPanel3.Controls.Add(Me.ChainListItemsGrid, 1, 1)
            Me.TableLayoutPanel3.Controls.Add(Me.Label2, 1, 0)
            Me.TableLayoutPanel3.Controls.Add(Me.FlowLayoutPanel1, 0, 2)
            Me.TableLayoutPanel3.Controls.Add(Me.FlowLayoutPanel2, 1, 2)
            Me.TableLayoutPanel3.Controls.Add(Me.FlowLayoutPanel3, 0, 0)
            Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel3.Location = New System.Drawing.Point(3, 3)
            Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
            Me.TableLayoutPanel3.RowCount = 3
            Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TableLayoutPanel3.Size = New System.Drawing.Size(852, 466)
            Me.TableLayoutPanel3.TabIndex = 0
            '
            'ChainsListsGrid
            '
            Me.ChainsListsGrid.AllowUserToAddRows = False
            Me.ChainsListsGrid.AllowUserToDeleteRows = False
            Me.ChainsListsGrid.AllowUserToOrderColumns = True
            Me.ChainsListsGrid.AllowUserToResizeColumns = False
            Me.ChainsListsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.ChainsListsGrid.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ChainsListsGrid.Location = New System.Drawing.Point(3, 23)
            Me.ChainsListsGrid.Name = "ChainsListsGrid"
            Me.ChainsListsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.ChainsListsGrid.Size = New System.Drawing.Size(420, 420)
            Me.ChainsListsGrid.TabIndex = 0
            '
            'ChainListItemsGrid
            '
            Me.ChainListItemsGrid.AllowUserToAddRows = False
            Me.ChainListItemsGrid.AllowUserToDeleteRows = False
            Me.ChainListItemsGrid.AllowUserToResizeColumns = False
            Me.ChainListItemsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.ChainListItemsGrid.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ChainListItemsGrid.Location = New System.Drawing.Point(429, 23)
            Me.ChainListItemsGrid.Name = "ChainListItemsGrid"
            Me.ChainListItemsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.ChainListItemsGrid.Size = New System.Drawing.Size(420, 420)
            Me.ChainListItemsGrid.TabIndex = 1
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(429, 0)
            Me.Label2.Name = "Label2"
            Me.Label2.Padding = New System.Windows.Forms.Padding(0, 3, 0, 0)
            Me.Label2.Size = New System.Drawing.Size(32, 16)
            Me.Label2.TabIndex = 3
            Me.Label2.Text = "Items"
            '
            'FlowLayoutPanel1
            '
            Me.FlowLayoutPanel1.Controls.Add(Me.AddCLButton)
            Me.FlowLayoutPanel1.Controls.Add(Me.EditCLButton)
            Me.FlowLayoutPanel1.Controls.Add(Me.ReloadCLButton)
            Me.FlowLayoutPanel1.Controls.Add(Me.DeleteCLButton)
            Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 446)
            Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
            Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
            Me.FlowLayoutPanel1.Size = New System.Drawing.Size(426, 20)
            Me.FlowLayoutPanel1.TabIndex = 5
            '
            'AddCLButton
            '
            Me.AddCLButton.Location = New System.Drawing.Point(0, 0)
            Me.AddCLButton.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
            Me.AddCLButton.Name = "AddCLButton"
            Me.AddCLButton.Size = New System.Drawing.Size(75, 20)
            Me.AddCLButton.TabIndex = 0
            Me.AddCLButton.Text = "Add"
            Me.AddCLButton.UseVisualStyleBackColor = True
            '
            'EditCLButton
            '
            Me.EditCLButton.Location = New System.Drawing.Point(78, 0)
            Me.EditCLButton.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
            Me.EditCLButton.Name = "EditCLButton"
            Me.EditCLButton.Size = New System.Drawing.Size(75, 20)
            Me.EditCLButton.TabIndex = 1
            Me.EditCLButton.Text = "Edit"
            Me.EditCLButton.UseVisualStyleBackColor = True
            '
            'ReloadCLButton
            '
            Me.ReloadCLButton.Location = New System.Drawing.Point(156, 0)
            Me.ReloadCLButton.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
            Me.ReloadCLButton.Name = "ReloadCLButton"
            Me.ReloadCLButton.Size = New System.Drawing.Size(75, 20)
            Me.ReloadCLButton.TabIndex = 3
            Me.ReloadCLButton.Text = "Reload"
            Me.ReloadCLButton.UseVisualStyleBackColor = True
            '
            'DeleteCLButton
            '
            Me.DeleteCLButton.Location = New System.Drawing.Point(234, 0)
            Me.DeleteCLButton.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
            Me.DeleteCLButton.Name = "DeleteCLButton"
            Me.DeleteCLButton.Size = New System.Drawing.Size(75, 20)
            Me.DeleteCLButton.TabIndex = 2
            Me.DeleteCLButton.Text = "Delete"
            Me.DeleteCLButton.UseVisualStyleBackColor = True
            '
            'FlowLayoutPanel2
            '
            Me.FlowLayoutPanel2.Controls.Add(Me.AddItemsButton)
            Me.FlowLayoutPanel2.Controls.Add(Me.DeleteItemsButton)
            Me.FlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FlowLayoutPanel2.Location = New System.Drawing.Point(426, 446)
            Me.FlowLayoutPanel2.Margin = New System.Windows.Forms.Padding(0)
            Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
            Me.FlowLayoutPanel2.Size = New System.Drawing.Size(426, 20)
            Me.FlowLayoutPanel2.TabIndex = 6
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
            'FlowLayoutPanel3
            '
            Me.FlowLayoutPanel3.Controls.Add(Me.ChainsButton)
            Me.FlowLayoutPanel3.Controls.Add(Me.ListsButton)
            Me.FlowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FlowLayoutPanel3.Location = New System.Drawing.Point(0, 0)
            Me.FlowLayoutPanel3.Margin = New System.Windows.Forms.Padding(0)
            Me.FlowLayoutPanel3.Name = "FlowLayoutPanel3"
            Me.FlowLayoutPanel3.Size = New System.Drawing.Size(426, 20)
            Me.FlowLayoutPanel3.TabIndex = 7
            '
            'ChainsButton
            '
            Me.ChainsButton.AutoSize = True
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
            Me.ListsButton.TabStop = True
            Me.ListsButton.Text = "Lists"
            Me.ListsButton.UseVisualStyleBackColor = True
            '
            'FieldsPage
            '
            Me.FieldsPage.Controls.Add(Me.TableLayoutPanel4)
            Me.FieldsPage.Location = New System.Drawing.Point(4, 22)
            Me.FieldsPage.Name = "FieldsPage"
            Me.FieldsPage.Padding = New System.Windows.Forms.Padding(3)
            Me.FieldsPage.Size = New System.Drawing.Size(858, 472)
            Me.FieldsPage.TabIndex = 4
            Me.FieldsPage.Text = "Fields"
            Me.FieldsPage.UseVisualStyleBackColor = True
            '
            'TableLayoutPanel4
            '
            Me.TableLayoutPanel4.ColumnCount = 2
            Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel4.Controls.Add(Me.ListBox1, 0, 0)
            Me.TableLayoutPanel4.Controls.Add(Me.FlowLayoutPanel5, 0, 1)
            Me.TableLayoutPanel4.Controls.Add(Me.DataGridView4, 1, 0)
            Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel4.Location = New System.Drawing.Point(3, 3)
            Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
            Me.TableLayoutPanel4.RowCount = 2
            Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TableLayoutPanel4.Size = New System.Drawing.Size(852, 466)
            Me.TableLayoutPanel4.TabIndex = 0
            '
            'ListBox1
            '
            Me.ListBox1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ListBox1.FormattingEnabled = True
            Me.ListBox1.Location = New System.Drawing.Point(3, 3)
            Me.ListBox1.Name = "ListBox1"
            Me.ListBox1.Size = New System.Drawing.Size(420, 435)
            Me.ListBox1.TabIndex = 0
            '
            'FlowLayoutPanel5
            '
            Me.FlowLayoutPanel5.Controls.Add(Me.Button9)
            Me.FlowLayoutPanel5.Controls.Add(Me.Button10)
            Me.FlowLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FlowLayoutPanel5.Location = New System.Drawing.Point(0, 441)
            Me.FlowLayoutPanel5.Margin = New System.Windows.Forms.Padding(0)
            Me.FlowLayoutPanel5.Name = "FlowLayoutPanel5"
            Me.FlowLayoutPanel5.Size = New System.Drawing.Size(426, 25)
            Me.FlowLayoutPanel5.TabIndex = 1
            '
            'Button9
            '
            Me.Button9.Location = New System.Drawing.Point(0, 0)
            Me.Button9.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
            Me.Button9.Name = "Button9"
            Me.Button9.Size = New System.Drawing.Size(75, 23)
            Me.Button9.TabIndex = 0
            Me.Button9.Text = "Add"
            Me.Button9.UseVisualStyleBackColor = True
            '
            'Button10
            '
            Me.Button10.Location = New System.Drawing.Point(78, 0)
            Me.Button10.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
            Me.Button10.Name = "Button10"
            Me.Button10.Size = New System.Drawing.Size(75, 23)
            Me.Button10.TabIndex = 1
            Me.Button10.Text = "Remove"
            Me.Button10.UseVisualStyleBackColor = True
            '
            'DataGridView4
            '
            Me.DataGridView4.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.DataGridView4.Dock = System.Windows.Forms.DockStyle.Fill
            Me.DataGridView4.Location = New System.Drawing.Point(429, 3)
            Me.DataGridView4.Name = "DataGridView4"
            Me.DataGridView4.Size = New System.Drawing.Size(420, 435)
            Me.DataGridView4.TabIndex = 2
            '
            'DataPage
            '
            Me.DataPage.Controls.Add(Me.TableLayoutPanel1)
            Me.DataPage.Location = New System.Drawing.Point(4, 22)
            Me.DataPage.Name = "DataPage"
            Me.DataPage.Padding = New System.Windows.Forms.Padding(3)
            Me.DataPage.Size = New System.Drawing.Size(858, 472)
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
            Me.TableLayoutPanel1.Size = New System.Drawing.Size(852, 466)
            Me.TableLayoutPanel1.TabIndex = 1
            '
            'BondsTableView
            '
            Me.BondsTableView.AllowUserToAddRows = False
            Me.BondsTableView.AllowUserToDeleteRows = False
            Me.BondsTableView.AllowUserToOrderColumns = True
            Me.BondsTableView.AllowUserToResizeRows = False
            Me.BondsTableView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.BondsTableView.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BondsTableView.Location = New System.Drawing.Point(199, 3)
            Me.BondsTableView.Name = "BondsTableView"
            Me.TableLayoutPanel1.SetRowSpan(Me.BondsTableView, 2)
            Me.BondsTableView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.BondsTableView.Size = New System.Drawing.Size(650, 460)
            Me.BondsTableView.TabIndex = 1
            '
            'TableChooserList
            '
            Me.TableChooserList.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableChooserList.FormattingEnabled = True
            Me.TableChooserList.Items.AddRange(New Object() {"Bonds", "Coupons", "FRNs", "Issue ratings", "Issuer ratings", "Rics"})
            Me.TableChooserList.Location = New System.Drawing.Point(3, 3)
            Me.TableChooserList.Name = "TableChooserList"
            Me.TableChooserList.Size = New System.Drawing.Size(190, 435)
            Me.TableChooserList.TabIndex = 2
            '
            'FlowLayoutPanel4
            '
            Me.FlowLayoutPanel4.Controls.Add(Me.Button7)
            Me.FlowLayoutPanel4.Controls.Add(Me.Button8)
            Me.FlowLayoutPanel4.Location = New System.Drawing.Point(0, 441)
            Me.FlowLayoutPanel4.Margin = New System.Windows.Forms.Padding(0)
            Me.FlowLayoutPanel4.Name = "FlowLayoutPanel4"
            Me.FlowLayoutPanel4.Size = New System.Drawing.Size(196, 25)
            Me.FlowLayoutPanel4.TabIndex = 3
            '
            'Button7
            '
            Me.Button7.Location = New System.Drawing.Point(0, 0)
            Me.Button7.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
            Me.Button7.Name = "Button7"
            Me.Button7.Size = New System.Drawing.Size(75, 23)
            Me.Button7.TabIndex = 0
            Me.Button7.Text = "Cleanup"
            Me.Button7.UseVisualStyleBackColor = True
            '
            'Button8
            '
            Me.Button8.Location = New System.Drawing.Point(78, 0)
            Me.Button8.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
            Me.Button8.Name = "Button8"
            Me.Button8.Size = New System.Drawing.Size(75, 23)
            Me.Button8.TabIndex = 1
            Me.Button8.Text = "Reload"
            Me.Button8.UseVisualStyleBackColor = True
            '
            'PortfolioForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(866, 498)
            Me.Controls.Add(Me.MainTabControl)
            Me.Name = "PortfolioForm"
            Me.Text = "Portfolio manager"
            Me.MainTabControl.ResumeLayout(False)
            Me.PortfoliosPage.ResumeLayout(False)
            Me.TableLayoutPanel2.ResumeLayout(False)
            CType(Me.PortfolioItemsGrid, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ChainsPage.ResumeLayout(False)
            Me.TableLayoutPanel3.ResumeLayout(False)
            Me.TableLayoutPanel3.PerformLayout()
            CType(Me.ChainsListsGrid, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.ChainListItemsGrid, System.ComponentModel.ISupportInitialize).EndInit()
            Me.FlowLayoutPanel1.ResumeLayout(False)
            Me.FlowLayoutPanel2.ResumeLayout(False)
            Me.FlowLayoutPanel3.ResumeLayout(False)
            Me.FlowLayoutPanel3.PerformLayout()
            Me.FieldsPage.ResumeLayout(False)
            Me.TableLayoutPanel4.ResumeLayout(False)
            Me.FlowLayoutPanel5.ResumeLayout(False)
            CType(Me.DataGridView4, System.ComponentModel.ISupportInitialize).EndInit()
            Me.DataPage.ResumeLayout(False)
            Me.TableLayoutPanel1.ResumeLayout(False)
            CType(Me.BondsTableView, System.ComponentModel.ISupportInitialize).EndInit()
            Me.FlowLayoutPanel4.ResumeLayout(False)
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
        Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents ChainsListsGrid As System.Windows.Forms.DataGridView
        Friend WithEvents ChainListItemsGrid As System.Windows.Forms.DataGridView
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents FlowLayoutPanel2 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents AddCLButton As System.Windows.Forms.Button
        Friend WithEvents EditCLButton As System.Windows.Forms.Button
        Friend WithEvents ReloadCLButton As System.Windows.Forms.Button
        Friend WithEvents DeleteCLButton As System.Windows.Forms.Button
        Friend WithEvents AddItemsButton As System.Windows.Forms.Button
        Friend WithEvents DeleteItemsButton As System.Windows.Forms.Button
        Friend WithEvents FlowLayoutPanel3 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents ChainsButton As System.Windows.Forms.RadioButton
        Friend WithEvents ListsButton As System.Windows.Forms.RadioButton
        Friend WithEvents PortfolioItemsGrid As System.Windows.Forms.DataGridView
        Friend WithEvents FlowLayoutPanel4 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents Button7 As System.Windows.Forms.Button
        Friend WithEvents Button8 As System.Windows.Forms.Button
        Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
        Friend WithEvents FlowLayoutPanel5 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents Button9 As System.Windows.Forms.Button
        Friend WithEvents Button10 As System.Windows.Forms.Button
        Friend WithEvents DataGridView4 As System.Windows.Forms.DataGridView
    End Class
End Namespace