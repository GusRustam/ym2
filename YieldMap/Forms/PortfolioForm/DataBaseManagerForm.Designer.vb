Namespace Forms.PortfolioForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class DataBaseManagerForm
        Inherits System.Windows.Forms.Form

        'Форма переопределяет dispose для очистки списка компонентов.
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

        'Является обязательной для конструктора форм Windows Forms
        Private components As System.ComponentModel.IContainer

        'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
        'Для ее изменения используйте конструктор форм Windows Form.  
        'Не изменяйте ее в редакторе исходного кода.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Me.MainTabControl = New System.Windows.Forms.TabControl()
            Me.PorfolioPage = New System.Windows.Forms.TabPage()
            Me.PorfolioElementsDGV = New System.Windows.Forms.DataGridView()
            Me.PidDataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.BidDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.BondshortnameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.DataGridViewTextBoxColumn12 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.IncludeDataGridViewCheckBoxColumn3 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
            Me.PortfolioUnitedBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.BondsDataSet = New YieldMap.BondsDataSet()
            Me.RemoveBondButton = New System.Windows.Forms.Button()
            Me.AddBondButton = New System.Windows.Forms.Button()
            Me.RemoveHawserButton = New System.Windows.Forms.Button()
            Me.AddHawserButton = New System.Windows.Forms.Button()
            Me.RemoveChainButton = New System.Windows.Forms.Button()
            Me.AddChainButton = New System.Windows.Forms.Button()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.ConstPortLabel = New System.Windows.Forms.Label()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.PortBondDGV = New System.Windows.Forms.DataGridView()
            Me.pid = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ric = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.IncludeDataGridViewCheckBoxColumn2 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
            Me.PortfolioByBondsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.PortUserListDGV = New System.Windows.Forms.DataGridView()
            Me.PidDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.hid = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.HawsernameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.DataGridViewTextBoxColumn11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.IncludeDataGridViewCheckBoxColumn1 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
            Me.HawsersInPortfolioBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.PortChainsDGV = New System.Windows.Forms.DataGridView()
            Me.PidDataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.cid = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ChainnameDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.color = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.IncludeDataGridViewCheckBoxColumn4 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
            Me.ChainsInPortfolioBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.Label2 = New System.Windows.Forms.Label()
            Me.RenamePortfolioButton = New System.Windows.Forms.Button()
            Me.AddPortfolioButton = New System.Windows.Forms.Button()
            Me.DeletePortfolioButton = New System.Windows.Forms.Button()
            Me.PortfoliosListBox = New System.Windows.Forms.ListBox()
            Me.PortfolioBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.UDL_Page = New System.Windows.Forms.TabPage()
            Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
            Me.ListOfList = New System.Windows.Forms.DataGridView()
            Me.IdDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.HawsernameDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColorDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnHawserBid = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnHawserAsk = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnHawserLast = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnHawserHist = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnListCurve = New System.Windows.Forms.DataGridViewCheckBoxColumn()
            Me.HawserBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.ConstituentsDGW = New System.Windows.Forms.DataGridView()
            Me.HawseridDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.RICDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.BondidDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.BondDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.HawserDataBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel()
            Me.AddListButton = New System.Windows.Forms.Button()
            Me.RenameButton = New System.Windows.Forms.Button()
            Me.DeleteListButton = New System.Windows.Forms.Button()
            Me.FlowLayoutPanel3 = New System.Windows.Forms.FlowLayoutPanel()
            Me.AddItemButton = New System.Windows.Forms.Button()
            Me.RemoveItemButton = New System.Windows.Forms.Button()
            Me.TabPage1 = New System.Windows.Forms.TabPage()
            Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
            Me.ChainsDGV = New System.Windows.Forms.DataGridView()
            Me.IdDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ChainnameDataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.DescrDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColorDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnChainBid = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnChainAsk = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnChainLast = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnChainHist = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnChainCurve = New System.Windows.Forms.DataGridViewCheckBoxColumn()
            Me.ChainBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
            Me.AddChain = New System.Windows.Forms.Button()
            Me.EditChain = New System.Windows.Forms.Button()
            Me.RemoveChain = New System.Windows.Forms.Button()
            Me.TabPage2 = New System.Windows.Forms.TabPage()
            Me.ReloadBondsButton = New System.Windows.Forms.Button()
            Me.DbUpdatedLabel = New System.Windows.Forms.Label()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.CidDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ChainnameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.IncludeDataGridViewCheckBoxColumn = New System.Windows.Forms.DataGridViewCheckBoxColumn()
            Me.CloseButton = New System.Windows.Forms.Button()
            Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.HawserTableAdapter = New YieldMap.BondsDataSetTableAdapters.hawserTableAdapter()
            Me.HawserDataTableAdapter = New YieldMap.BondsDataSetTableAdapters.HawserDataTableAdapter()
            Me.PortfolioTableAdapter = New YieldMap.BondsDataSetTableAdapters.portfolioTableAdapter()
            Me._portfolioByBondsTableAdapter = New YieldMap.BondsDataSetTableAdapters._portfolioByBondsTableAdapter()
            Me.ChainsInPortfolioTableAdapter = New YieldMap.BondsDataSetTableAdapters.ChainsInPortfolioTableAdapter()
            Me.HawsersInPortfolioTableAdapter = New YieldMap.BondsDataSetTableAdapters.HawsersInPortfolioTableAdapter()
            Me.PortfolioUnitedTableAdapter = New YieldMap.BondsDataSetTableAdapters.PortfolioUnitedTableAdapter()
            Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.DataGridViewTextBoxColumn8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.DataGridViewTextBoxColumn9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.DataGridViewTextBoxColumn10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ChainTableAdapter = New YieldMap.BondsDataSetTableAdapters.chainTableAdapter()
            Me.MessageListBox = New System.Windows.Forms.ListBox()
            Me.MainTabControl.SuspendLayout()
            Me.PorfolioPage.SuspendLayout()
            CType(Me.PorfolioElementsDGV, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PortfolioUnitedBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.BondsDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PortBondDGV, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PortfolioByBondsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PortUserListDGV, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.HawsersInPortfolioBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PortChainsDGV, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.ChainsInPortfolioBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.PortfolioBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.UDL_Page.SuspendLayout()
            Me.TableLayoutPanel2.SuspendLayout()
            CType(Me.ListOfList, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.HawserBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.ConstituentsDGW, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.HawserDataBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.FlowLayoutPanel2.SuspendLayout()
            Me.FlowLayoutPanel3.SuspendLayout()
            Me.TabPage1.SuspendLayout()
            Me.TableLayoutPanel1.SuspendLayout()
            CType(Me.ChainsDGV, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.ChainBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.FlowLayoutPanel1.SuspendLayout()
            Me.TabPage2.SuspendLayout()
            Me.SuspendLayout()
            '
            'MainTabControl
            '
            Me.MainTabControl.Controls.Add(Me.PorfolioPage)
            Me.MainTabControl.Controls.Add(Me.UDL_Page)
            Me.MainTabControl.Controls.Add(Me.TabPage1)
            Me.MainTabControl.Controls.Add(Me.TabPage2)
            Me.MainTabControl.Location = New System.Drawing.Point(0, 0)
            Me.MainTabControl.Name = "MainTabControl"
            Me.MainTabControl.SelectedIndex = 0
            Me.MainTabControl.Size = New System.Drawing.Size(907, 565)
            Me.MainTabControl.TabIndex = 0
            '
            'PorfolioPage
            '
            Me.PorfolioPage.Controls.Add(Me.PorfolioElementsDGV)
            Me.PorfolioPage.Controls.Add(Me.RemoveBondButton)
            Me.PorfolioPage.Controls.Add(Me.AddBondButton)
            Me.PorfolioPage.Controls.Add(Me.RemoveHawserButton)
            Me.PorfolioPage.Controls.Add(Me.AddHawserButton)
            Me.PorfolioPage.Controls.Add(Me.RemoveChainButton)
            Me.PorfolioPage.Controls.Add(Me.AddChainButton)
            Me.PorfolioPage.Controls.Add(Me.Label5)
            Me.PorfolioPage.Controls.Add(Me.Label4)
            Me.PorfolioPage.Controls.Add(Me.ConstPortLabel)
            Me.PorfolioPage.Controls.Add(Me.Label3)
            Me.PorfolioPage.Controls.Add(Me.PortBondDGV)
            Me.PorfolioPage.Controls.Add(Me.PortUserListDGV)
            Me.PorfolioPage.Controls.Add(Me.PortChainsDGV)
            Me.PorfolioPage.Controls.Add(Me.Label2)
            Me.PorfolioPage.Controls.Add(Me.RenamePortfolioButton)
            Me.PorfolioPage.Controls.Add(Me.AddPortfolioButton)
            Me.PorfolioPage.Controls.Add(Me.DeletePortfolioButton)
            Me.PorfolioPage.Controls.Add(Me.PortfoliosListBox)
            Me.PorfolioPage.Location = New System.Drawing.Point(4, 22)
            Me.PorfolioPage.Name = "PorfolioPage"
            Me.PorfolioPage.Padding = New System.Windows.Forms.Padding(3)
            Me.PorfolioPage.Size = New System.Drawing.Size(899, 539)
            Me.PorfolioPage.TabIndex = 3
            Me.PorfolioPage.Text = "Porfolios"
            Me.PorfolioPage.UseVisualStyleBackColor = True
            '
            'PorfolioElementsDGV
            '
            Me.PorfolioElementsDGV.AllowUserToAddRows = False
            Me.PorfolioElementsDGV.AllowUserToDeleteRows = False
            Me.PorfolioElementsDGV.AutoGenerateColumns = False
            Me.PorfolioElementsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.PorfolioElementsDGV.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.PidDataGridViewTextBoxColumn3, Me.BidDataGridViewTextBoxColumn, Me.BondshortnameDataGridViewTextBoxColumn, Me.DataGridViewTextBoxColumn12, Me.IncludeDataGridViewCheckBoxColumn3})
            Me.PorfolioElementsDGV.DataSource = Me.PortfolioUnitedBindingSource
            Me.PorfolioElementsDGV.Location = New System.Drawing.Point(593, 26)
            Me.PorfolioElementsDGV.Name = "PorfolioElementsDGV"
            Me.PorfolioElementsDGV.ReadOnly = True
            Me.PorfolioElementsDGV.RowHeadersVisible = False
            Me.PorfolioElementsDGV.Size = New System.Drawing.Size(288, 472)
            Me.PorfolioElementsDGV.TabIndex = 14
            '
            'PidDataGridViewTextBoxColumn3
            '
            Me.PidDataGridViewTextBoxColumn3.DataPropertyName = "pid"
            Me.PidDataGridViewTextBoxColumn3.HeaderText = "pid"
            Me.PidDataGridViewTextBoxColumn3.Name = "PidDataGridViewTextBoxColumn3"
            Me.PidDataGridViewTextBoxColumn3.ReadOnly = True
            Me.PidDataGridViewTextBoxColumn3.Visible = False
            '
            'BidDataGridViewTextBoxColumn
            '
            Me.BidDataGridViewTextBoxColumn.DataPropertyName = "bid"
            Me.BidDataGridViewTextBoxColumn.HeaderText = "bid"
            Me.BidDataGridViewTextBoxColumn.Name = "BidDataGridViewTextBoxColumn"
            Me.BidDataGridViewTextBoxColumn.ReadOnly = True
            Me.BidDataGridViewTextBoxColumn.Visible = False
            '
            'BondshortnameDataGridViewTextBoxColumn
            '
            Me.BondshortnameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.BondshortnameDataGridViewTextBoxColumn.DataPropertyName = "bondshortname"
            Me.BondshortnameDataGridViewTextBoxColumn.HeaderText = "Bond"
            Me.BondshortnameDataGridViewTextBoxColumn.Name = "BondshortnameDataGridViewTextBoxColumn"
            Me.BondshortnameDataGridViewTextBoxColumn.ReadOnly = True
            '
            'DataGridViewTextBoxColumn12
            '
            Me.DataGridViewTextBoxColumn12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.DataGridViewTextBoxColumn12.DataPropertyName = "color"
            Me.DataGridViewTextBoxColumn12.HeaderText = "Color"
            Me.DataGridViewTextBoxColumn12.Name = "DataGridViewTextBoxColumn12"
            Me.DataGridViewTextBoxColumn12.ReadOnly = True
            Me.DataGridViewTextBoxColumn12.Width = 56
            '
            'IncludeDataGridViewCheckBoxColumn3
            '
            Me.IncludeDataGridViewCheckBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.IncludeDataGridViewCheckBoxColumn3.DataPropertyName = "include"
            Me.IncludeDataGridViewCheckBoxColumn3.HeaderText = "Included"
            Me.IncludeDataGridViewCheckBoxColumn3.Name = "IncludeDataGridViewCheckBoxColumn3"
            Me.IncludeDataGridViewCheckBoxColumn3.ReadOnly = True
            Me.IncludeDataGridViewCheckBoxColumn3.Width = 54
            '
            'PortfolioUnitedBindingSource
            '
            Me.PortfolioUnitedBindingSource.DataMember = "PortfolioUnited"
            Me.PortfolioUnitedBindingSource.DataSource = Me.BondsDataSet
            Me.PortfolioUnitedBindingSource.Sort = "bondshortname"
            '
            'BondsDataSet
            '
            Me.BondsDataSet.DataSetName = "BondsDataSet"
            Me.BondsDataSet.EnforceConstraints = False
            Me.BondsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'RemoveBondButton
            '
            Me.RemoveBondButton.Image = Global.YieldMap.My.Resources.Resources.delete
            Me.RemoveBondButton.Location = New System.Drawing.Point(541, 470)
            Me.RemoveBondButton.Name = "RemoveBondButton"
            Me.RemoveBondButton.Size = New System.Drawing.Size(35, 28)
            Me.RemoveBondButton.TabIndex = 13
            Me.RemoveBondButton.UseVisualStyleBackColor = True
            '
            'AddBondButton
            '
            Me.AddBondButton.Image = Global.YieldMap.My.Resources.Resources.add
            Me.AddBondButton.Location = New System.Drawing.Point(541, 436)
            Me.AddBondButton.Name = "AddBondButton"
            Me.AddBondButton.Size = New System.Drawing.Size(35, 28)
            Me.AddBondButton.TabIndex = 13
            Me.AddBondButton.UseVisualStyleBackColor = True
            '
            'RemoveHawserButton
            '
            Me.RemoveHawserButton.Image = Global.YieldMap.My.Resources.Resources.delete
            Me.RemoveHawserButton.Location = New System.Drawing.Point(541, 303)
            Me.RemoveHawserButton.Name = "RemoveHawserButton"
            Me.RemoveHawserButton.Size = New System.Drawing.Size(35, 28)
            Me.RemoveHawserButton.TabIndex = 13
            Me.RemoveHawserButton.UseVisualStyleBackColor = True
            '
            'AddHawserButton
            '
            Me.AddHawserButton.Image = Global.YieldMap.My.Resources.Resources.add
            Me.AddHawserButton.Location = New System.Drawing.Point(541, 269)
            Me.AddHawserButton.Name = "AddHawserButton"
            Me.AddHawserButton.Size = New System.Drawing.Size(35, 28)
            Me.AddHawserButton.TabIndex = 13
            Me.AddHawserButton.UseVisualStyleBackColor = True
            '
            'RemoveChainButton
            '
            Me.RemoveChainButton.Image = Global.YieldMap.My.Resources.Resources.delete
            Me.RemoveChainButton.Location = New System.Drawing.Point(541, 136)
            Me.RemoveChainButton.Name = "RemoveChainButton"
            Me.RemoveChainButton.Size = New System.Drawing.Size(35, 28)
            Me.RemoveChainButton.TabIndex = 13
            Me.RemoveChainButton.UseVisualStyleBackColor = True
            '
            'AddChainButton
            '
            Me.AddChainButton.Image = Global.YieldMap.My.Resources.Resources.add
            Me.AddChainButton.Location = New System.Drawing.Point(541, 102)
            Me.AddChainButton.Name = "AddChainButton"
            Me.AddChainButton.Size = New System.Drawing.Size(35, 28)
            Me.AddChainButton.TabIndex = 13
            Me.AddChainButton.UseVisualStyleBackColor = True
            '
            'Label5
            '
            Me.Label5.AutoSize = True
            Me.Label5.Location = New System.Drawing.Point(228, 343)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(84, 13)
            Me.Label5.TabIndex = 12
            Me.Label5.Text = "Individual bonds"
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(228, 176)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(87, 13)
            Me.Label4.TabIndex = 12
            Me.Label4.Text = "User-defined lists"
            '
            'ConstPortLabel
            '
            Me.ConstPortLabel.AutoSize = True
            Me.ConstPortLabel.Location = New System.Drawing.Point(590, 7)
            Me.ConstPortLabel.Name = "ConstPortLabel"
            Me.ConstPortLabel.Size = New System.Drawing.Size(101, 13)
            Me.ConstPortLabel.TabIndex = 12
            Me.ConstPortLabel.Text = "Current constituents"
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(228, 7)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(39, 13)
            Me.Label3.TabIndex = 12
            Me.Label3.Text = "Chains"
            '
            'PortBondDGV
            '
            Me.PortBondDGV.AllowUserToAddRows = False
            Me.PortBondDGV.AllowUserToDeleteRows = False
            Me.PortBondDGV.AutoGenerateColumns = False
            Me.PortBondDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.PortBondDGV.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.pid, Me.ric, Me.IncludeDataGridViewCheckBoxColumn2})
            Me.PortBondDGV.DataSource = Me.PortfolioByBondsBindingSource
            Me.PortBondDGV.Location = New System.Drawing.Point(231, 360)
            Me.PortBondDGV.Name = "PortBondDGV"
            Me.PortBondDGV.ReadOnly = True
            Me.PortBondDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.PortBondDGV.Size = New System.Drawing.Size(303, 138)
            Me.PortBondDGV.TabIndex = 11
            '
            'pid
            '
            Me.pid.DataPropertyName = "pid"
            Me.pid.HeaderText = "pid"
            Me.pid.Name = "pid"
            Me.pid.ReadOnly = True
            Me.pid.Visible = False
            '
            'ric
            '
            Me.ric.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.ric.DataPropertyName = "ric"
            Me.ric.HeaderText = "Name"
            Me.ric.Name = "ric"
            Me.ric.ReadOnly = True
            '
            'IncludeDataGridViewCheckBoxColumn2
            '
            Me.IncludeDataGridViewCheckBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.IncludeDataGridViewCheckBoxColumn2.DataPropertyName = "include"
            Me.IncludeDataGridViewCheckBoxColumn2.HeaderText = "Included"
            Me.IncludeDataGridViewCheckBoxColumn2.Name = "IncludeDataGridViewCheckBoxColumn2"
            Me.IncludeDataGridViewCheckBoxColumn2.ReadOnly = True
            Me.IncludeDataGridViewCheckBoxColumn2.Width = 54
            '
            'PortfolioByBondsBindingSource
            '
            Me.PortfolioByBondsBindingSource.DataMember = "_portfolioByBonds"
            Me.PortfolioByBondsBindingSource.DataSource = Me.BondsDataSet
            '
            'PortUserListDGV
            '
            Me.PortUserListDGV.AllowUserToAddRows = False
            Me.PortUserListDGV.AllowUserToDeleteRows = False
            Me.PortUserListDGV.AutoGenerateColumns = False
            Me.PortUserListDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.PortUserListDGV.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.PidDataGridViewTextBoxColumn, Me.hid, Me.HawsernameDataGridViewTextBoxColumn, Me.DataGridViewTextBoxColumn11, Me.IncludeDataGridViewCheckBoxColumn1})
            Me.PortUserListDGV.DataSource = Me.HawsersInPortfolioBindingSource
            Me.PortUserListDGV.Location = New System.Drawing.Point(231, 193)
            Me.PortUserListDGV.Name = "PortUserListDGV"
            Me.PortUserListDGV.ReadOnly = True
            Me.PortUserListDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.PortUserListDGV.Size = New System.Drawing.Size(303, 138)
            Me.PortUserListDGV.TabIndex = 11
            '
            'PidDataGridViewTextBoxColumn
            '
            Me.PidDataGridViewTextBoxColumn.DataPropertyName = "pid"
            Me.PidDataGridViewTextBoxColumn.HeaderText = "pid"
            Me.PidDataGridViewTextBoxColumn.Name = "PidDataGridViewTextBoxColumn"
            Me.PidDataGridViewTextBoxColumn.ReadOnly = True
            Me.PidDataGridViewTextBoxColumn.Visible = False
            '
            'hid
            '
            Me.hid.DataPropertyName = "hid"
            Me.hid.HeaderText = "hid"
            Me.hid.Name = "hid"
            Me.hid.ReadOnly = True
            Me.hid.Visible = False
            '
            'HawsernameDataGridViewTextBoxColumn
            '
            Me.HawsernameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.HawsernameDataGridViewTextBoxColumn.DataPropertyName = "hawser_name"
            Me.HawsernameDataGridViewTextBoxColumn.HeaderText = "Name"
            Me.HawsernameDataGridViewTextBoxColumn.Name = "HawsernameDataGridViewTextBoxColumn"
            Me.HawsernameDataGridViewTextBoxColumn.ReadOnly = True
            '
            'DataGridViewTextBoxColumn11
            '
            Me.DataGridViewTextBoxColumn11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.DataGridViewTextBoxColumn11.DataPropertyName = "color"
            Me.DataGridViewTextBoxColumn11.HeaderText = "Color"
            Me.DataGridViewTextBoxColumn11.Name = "DataGridViewTextBoxColumn11"
            Me.DataGridViewTextBoxColumn11.ReadOnly = True
            Me.DataGridViewTextBoxColumn11.Width = 56
            '
            'IncludeDataGridViewCheckBoxColumn1
            '
            Me.IncludeDataGridViewCheckBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.IncludeDataGridViewCheckBoxColumn1.DataPropertyName = "include"
            Me.IncludeDataGridViewCheckBoxColumn1.HeaderText = "Included"
            Me.IncludeDataGridViewCheckBoxColumn1.Name = "IncludeDataGridViewCheckBoxColumn1"
            Me.IncludeDataGridViewCheckBoxColumn1.ReadOnly = True
            Me.IncludeDataGridViewCheckBoxColumn1.Width = 54
            '
            'HawsersInPortfolioBindingSource
            '
            Me.HawsersInPortfolioBindingSource.DataMember = "HawsersInPortfolio"
            Me.HawsersInPortfolioBindingSource.DataSource = Me.BondsDataSet
            '
            'PortChainsDGV
            '
            Me.PortChainsDGV.AllowUserToAddRows = False
            Me.PortChainsDGV.AllowUserToDeleteRows = False
            Me.PortChainsDGV.AutoGenerateColumns = False
            Me.PortChainsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.PortChainsDGV.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.PidDataGridViewTextBoxColumn2, Me.cid, Me.ChainnameDataGridViewTextBoxColumn1, Me.color, Me.IncludeDataGridViewCheckBoxColumn4})
            Me.PortChainsDGV.DataSource = Me.ChainsInPortfolioBindingSource
            Me.PortChainsDGV.Location = New System.Drawing.Point(231, 26)
            Me.PortChainsDGV.Name = "PortChainsDGV"
            Me.PortChainsDGV.ReadOnly = True
            Me.PortChainsDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.PortChainsDGV.Size = New System.Drawing.Size(303, 138)
            Me.PortChainsDGV.TabIndex = 11
            '
            'PidDataGridViewTextBoxColumn2
            '
            Me.PidDataGridViewTextBoxColumn2.DataPropertyName = "pid"
            Me.PidDataGridViewTextBoxColumn2.HeaderText = "pid"
            Me.PidDataGridViewTextBoxColumn2.Name = "PidDataGridViewTextBoxColumn2"
            Me.PidDataGridViewTextBoxColumn2.ReadOnly = True
            Me.PidDataGridViewTextBoxColumn2.Visible = False
            '
            'cid
            '
            Me.cid.DataPropertyName = "cid"
            Me.cid.HeaderText = "cid"
            Me.cid.Name = "cid"
            Me.cid.ReadOnly = True
            Me.cid.Visible = False
            '
            'ChainnameDataGridViewTextBoxColumn1
            '
            Me.ChainnameDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.ChainnameDataGridViewTextBoxColumn1.DataPropertyName = "chain_name"
            Me.ChainnameDataGridViewTextBoxColumn1.HeaderText = "Name"
            Me.ChainnameDataGridViewTextBoxColumn1.Name = "ChainnameDataGridViewTextBoxColumn1"
            Me.ChainnameDataGridViewTextBoxColumn1.ReadOnly = True
            '
            'color
            '
            Me.color.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.color.DataPropertyName = "color"
            Me.color.HeaderText = "Color"
            Me.color.Name = "color"
            Me.color.ReadOnly = True
            Me.color.Width = 56
            '
            'IncludeDataGridViewCheckBoxColumn4
            '
            Me.IncludeDataGridViewCheckBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.IncludeDataGridViewCheckBoxColumn4.DataPropertyName = "include"
            Me.IncludeDataGridViewCheckBoxColumn4.HeaderText = "Included"
            Me.IncludeDataGridViewCheckBoxColumn4.Name = "IncludeDataGridViewCheckBoxColumn4"
            Me.IncludeDataGridViewCheckBoxColumn4.ReadOnly = True
            Me.IncludeDataGridViewCheckBoxColumn4.Width = 54
            '
            'ChainsInPortfolioBindingSource
            '
            Me.ChainsInPortfolioBindingSource.DataMember = "ChainsInPortfolio"
            Me.ChainsInPortfolioBindingSource.DataSource = Me.BondsDataSet
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(6, 3)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(50, 13)
            Me.Label2.TabIndex = 10
            Me.Label2.Text = "Portfolios"
            '
            'RenamePortfolioButton
            '
            Me.RenamePortfolioButton.Image = Global.YieldMap.My.Resources.Resources.pencil
            Me.RenamePortfolioButton.Location = New System.Drawing.Point(79, 504)
            Me.RenamePortfolioButton.Name = "RenamePortfolioButton"
            Me.RenamePortfolioButton.Size = New System.Drawing.Size(63, 29)
            Me.RenamePortfolioButton.TabIndex = 8
            Me.RenamePortfolioButton.UseVisualStyleBackColor = True
            '
            'AddPortfolioButton
            '
            Me.AddPortfolioButton.Image = Global.YieldMap.My.Resources.Resources.add
            Me.AddPortfolioButton.Location = New System.Drawing.Point(9, 504)
            Me.AddPortfolioButton.Name = "AddPortfolioButton"
            Me.AddPortfolioButton.Size = New System.Drawing.Size(63, 29)
            Me.AddPortfolioButton.TabIndex = 9
            Me.AddPortfolioButton.UseVisualStyleBackColor = True
            '
            'DeletePortfolioButton
            '
            Me.DeletePortfolioButton.Image = Global.YieldMap.My.Resources.Resources.delete
            Me.DeletePortfolioButton.Location = New System.Drawing.Point(149, 504)
            Me.DeletePortfolioButton.Name = "DeletePortfolioButton"
            Me.DeletePortfolioButton.Size = New System.Drawing.Size(63, 29)
            Me.DeletePortfolioButton.TabIndex = 7
            Me.DeletePortfolioButton.UseVisualStyleBackColor = True
            '
            'PortfoliosListBox
            '
            Me.PortfoliosListBox.DataSource = Me.PortfolioBindingSource
            Me.PortfoliosListBox.DisplayMember = "portfolio_name"
            Me.PortfoliosListBox.FormattingEnabled = True
            Me.PortfoliosListBox.Location = New System.Drawing.Point(8, 26)
            Me.PortfoliosListBox.Name = "PortfoliosListBox"
            Me.PortfoliosListBox.Size = New System.Drawing.Size(204, 472)
            Me.PortfoliosListBox.TabIndex = 6
            Me.PortfoliosListBox.ValueMember = "id"
            '
            'PortfolioBindingSource
            '
            Me.PortfolioBindingSource.DataMember = "portfolio"
            Me.PortfolioBindingSource.DataSource = Me.BondsDataSet
            '
            'UDL_Page
            '
            Me.UDL_Page.AutoScroll = True
            Me.UDL_Page.Controls.Add(Me.TableLayoutPanel2)
            Me.UDL_Page.Location = New System.Drawing.Point(4, 22)
            Me.UDL_Page.Name = "UDL_Page"
            Me.UDL_Page.Padding = New System.Windows.Forms.Padding(3)
            Me.UDL_Page.Size = New System.Drawing.Size(899, 539)
            Me.UDL_Page.TabIndex = 2
            Me.UDL_Page.Text = "User defined lists"
            Me.UDL_Page.UseVisualStyleBackColor = True
            '
            'TableLayoutPanel2
            '
            Me.TableLayoutPanel2.ColumnCount = 3
            Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.0!))
            Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
            Me.TableLayoutPanel2.Controls.Add(Me.ListOfList, 0, 0)
            Me.TableLayoutPanel2.Controls.Add(Me.ConstituentsDGW, 2, 0)
            Me.TableLayoutPanel2.Controls.Add(Me.FlowLayoutPanel2, 0, 1)
            Me.TableLayoutPanel2.Controls.Add(Me.FlowLayoutPanel3, 2, 1)
            Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 3)
            Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
            Me.TableLayoutPanel2.RowCount = 2
            Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            Me.TableLayoutPanel2.Size = New System.Drawing.Size(893, 533)
            Me.TableLayoutPanel2.TabIndex = 7
            '
            'ListOfList
            '
            Me.ListOfList.AllowUserToAddRows = False
            Me.ListOfList.AllowUserToDeleteRows = False
            Me.ListOfList.AutoGenerateColumns = False
            Me.ListOfList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.ListOfList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IdDataGridViewTextBoxColumn1, Me.HawsernameDataGridViewTextBoxColumn1, Me.ColorDataGridViewTextBoxColumn1, Me.ColumnHawserBid, Me.ColumnHawserAsk, Me.ColumnHawserLast, Me.ColumnHawserHist, Me.ColumnListCurve})
            Me.ListOfList.DataSource = Me.HawserBindingSource
            Me.ListOfList.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ListOfList.Location = New System.Drawing.Point(3, 3)
            Me.ListOfList.MultiSelect = False
            Me.ListOfList.Name = "ListOfList"
            Me.ListOfList.ReadOnly = True
            Me.ListOfList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.ListOfList.Size = New System.Drawing.Size(517, 497)
            Me.ListOfList.TabIndex = 6
            '
            'IdDataGridViewTextBoxColumn1
            '
            Me.IdDataGridViewTextBoxColumn1.DataPropertyName = "id"
            Me.IdDataGridViewTextBoxColumn1.HeaderText = "id"
            Me.IdDataGridViewTextBoxColumn1.Name = "IdDataGridViewTextBoxColumn1"
            Me.IdDataGridViewTextBoxColumn1.ReadOnly = True
            Me.IdDataGridViewTextBoxColumn1.Visible = False
            '
            'HawsernameDataGridViewTextBoxColumn1
            '
            Me.HawsernameDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.HawsernameDataGridViewTextBoxColumn1.DataPropertyName = "hawser_name"
            Me.HawsernameDataGridViewTextBoxColumn1.HeaderText = "Name"
            Me.HawsernameDataGridViewTextBoxColumn1.Name = "HawsernameDataGridViewTextBoxColumn1"
            Me.HawsernameDataGridViewTextBoxColumn1.ReadOnly = True
            '
            'ColorDataGridViewTextBoxColumn1
            '
            Me.ColorDataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.ColorDataGridViewTextBoxColumn1.DataPropertyName = "color"
            Me.ColorDataGridViewTextBoxColumn1.HeaderText = "Color"
            Me.ColorDataGridViewTextBoxColumn1.Name = "ColorDataGridViewTextBoxColumn1"
            Me.ColorDataGridViewTextBoxColumn1.ReadOnly = True
            Me.ColorDataGridViewTextBoxColumn1.Width = 56
            '
            'ColumnHawserBid
            '
            Me.ColumnHawserBid.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.ColumnHawserBid.DataPropertyName = "bid_field"
            Me.ColumnHawserBid.HeaderText = "Bid"
            Me.ColumnHawserBid.Name = "ColumnHawserBid"
            Me.ColumnHawserBid.ReadOnly = True
            Me.ColumnHawserBid.Width = 47
            '
            'ColumnHawserAsk
            '
            Me.ColumnHawserAsk.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.ColumnHawserAsk.DataPropertyName = "ask_field"
            Me.ColumnHawserAsk.HeaderText = "Ask"
            Me.ColumnHawserAsk.Name = "ColumnHawserAsk"
            Me.ColumnHawserAsk.ReadOnly = True
            Me.ColumnHawserAsk.Width = 50
            '
            'ColumnHawserLast
            '
            Me.ColumnHawserLast.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.ColumnHawserLast.DataPropertyName = "last_field"
            Me.ColumnHawserLast.HeaderText = "Last"
            Me.ColumnHawserLast.Name = "ColumnHawserLast"
            Me.ColumnHawserLast.ReadOnly = True
            Me.ColumnHawserLast.Width = 52
            '
            'ColumnHawserHist
            '
            Me.ColumnHawserHist.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.ColumnHawserHist.DataPropertyName = "hist_field"
            Me.ColumnHawserHist.HeaderText = "History"
            Me.ColumnHawserHist.Name = "ColumnHawserHist"
            Me.ColumnHawserHist.ReadOnly = True
            Me.ColumnHawserHist.Width = 64
            '
            'ColumnListCurve
            '
            Me.ColumnListCurve.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.ColumnListCurve.DataPropertyName = "curve"
            Me.ColumnListCurve.HeaderText = "Curve"
            Me.ColumnListCurve.Name = "ColumnListCurve"
            Me.ColumnListCurve.ReadOnly = True
            Me.ColumnListCurve.Width = 41
            '
            'HawserBindingSource
            '
            Me.HawserBindingSource.DataMember = "hawser"
            Me.HawserBindingSource.DataSource = Me.BondsDataSet
            '
            'ConstituentsDGW
            '
            Me.ConstituentsDGW.AllowUserToAddRows = False
            Me.ConstituentsDGW.AllowUserToDeleteRows = False
            Me.ConstituentsDGW.AutoGenerateColumns = False
            Me.ConstituentsDGW.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.ConstituentsDGW.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.HawseridDataGridViewTextBoxColumn, Me.RICDataGridViewTextBoxColumn, Me.BondidDataGridViewTextBoxColumn, Me.BondDataGridViewTextBoxColumn})
            Me.ConstituentsDGW.DataSource = Me.HawserDataBindingSource
            Me.ConstituentsDGW.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ConstituentsDGW.Location = New System.Drawing.Point(546, 3)
            Me.ConstituentsDGW.Name = "ConstituentsDGW"
            Me.ConstituentsDGW.ReadOnly = True
            Me.ConstituentsDGW.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.ConstituentsDGW.Size = New System.Drawing.Size(344, 497)
            Me.ConstituentsDGW.TabIndex = 1
            '
            'HawseridDataGridViewTextBoxColumn
            '
            Me.HawseridDataGridViewTextBoxColumn.DataPropertyName = "hawser_id"
            Me.HawseridDataGridViewTextBoxColumn.HeaderText = "hawser_id"
            Me.HawseridDataGridViewTextBoxColumn.Name = "HawseridDataGridViewTextBoxColumn"
            Me.HawseridDataGridViewTextBoxColumn.ReadOnly = True
            Me.HawseridDataGridViewTextBoxColumn.Visible = False
            '
            'RICDataGridViewTextBoxColumn
            '
            Me.RICDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.RICDataGridViewTextBoxColumn.DataPropertyName = "RIC"
            Me.RICDataGridViewTextBoxColumn.HeaderText = "RIC"
            Me.RICDataGridViewTextBoxColumn.Name = "RICDataGridViewTextBoxColumn"
            Me.RICDataGridViewTextBoxColumn.ReadOnly = True
            '
            'BondidDataGridViewTextBoxColumn
            '
            Me.BondidDataGridViewTextBoxColumn.DataPropertyName = "bond_id"
            Me.BondidDataGridViewTextBoxColumn.HeaderText = "bond_id"
            Me.BondidDataGridViewTextBoxColumn.Name = "BondidDataGridViewTextBoxColumn"
            Me.BondidDataGridViewTextBoxColumn.ReadOnly = True
            Me.BondidDataGridViewTextBoxColumn.Visible = False
            '
            'BondDataGridViewTextBoxColumn
            '
            Me.BondDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.BondDataGridViewTextBoxColumn.DataPropertyName = "Bond"
            Me.BondDataGridViewTextBoxColumn.HeaderText = "Bond"
            Me.BondDataGridViewTextBoxColumn.Name = "BondDataGridViewTextBoxColumn"
            Me.BondDataGridViewTextBoxColumn.ReadOnly = True
            '
            'HawserDataBindingSource
            '
            Me.HawserDataBindingSource.DataMember = "HawserData"
            Me.HawserDataBindingSource.DataSource = Me.BondsDataSet
            '
            'FlowLayoutPanel2
            '
            Me.FlowLayoutPanel2.Controls.Add(Me.AddListButton)
            Me.FlowLayoutPanel2.Controls.Add(Me.RenameButton)
            Me.FlowLayoutPanel2.Controls.Add(Me.DeleteListButton)
            Me.FlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FlowLayoutPanel2.Location = New System.Drawing.Point(0, 503)
            Me.FlowLayoutPanel2.Margin = New System.Windows.Forms.Padding(0)
            Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
            Me.FlowLayoutPanel2.Size = New System.Drawing.Size(523, 30)
            Me.FlowLayoutPanel2.TabIndex = 7
            '
            'AddListButton
            '
            Me.AddListButton.Image = Global.YieldMap.My.Resources.Resources.add
            Me.AddListButton.Location = New System.Drawing.Point(3, 3)
            Me.AddListButton.Name = "AddListButton"
            Me.AddListButton.Size = New System.Drawing.Size(42, 27)
            Me.AddListButton.TabIndex = 5
            Me.AddListButton.UseVisualStyleBackColor = True
            '
            'RenameButton
            '
            Me.RenameButton.Image = Global.YieldMap.My.Resources.Resources.pencil
            Me.RenameButton.Location = New System.Drawing.Point(51, 3)
            Me.RenameButton.Name = "RenameButton"
            Me.RenameButton.Size = New System.Drawing.Size(42, 27)
            Me.RenameButton.TabIndex = 5
            Me.RenameButton.UseVisualStyleBackColor = True
            '
            'DeleteListButton
            '
            Me.DeleteListButton.Image = Global.YieldMap.My.Resources.Resources.delete
            Me.DeleteListButton.Location = New System.Drawing.Point(99, 3)
            Me.DeleteListButton.Name = "DeleteListButton"
            Me.DeleteListButton.Size = New System.Drawing.Size(42, 27)
            Me.DeleteListButton.TabIndex = 4
            Me.DeleteListButton.UseVisualStyleBackColor = True
            '
            'FlowLayoutPanel3
            '
            Me.FlowLayoutPanel3.Controls.Add(Me.AddItemButton)
            Me.FlowLayoutPanel3.Controls.Add(Me.RemoveItemButton)
            Me.FlowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FlowLayoutPanel3.Location = New System.Drawing.Point(543, 503)
            Me.FlowLayoutPanel3.Margin = New System.Windows.Forms.Padding(0)
            Me.FlowLayoutPanel3.Name = "FlowLayoutPanel3"
            Me.FlowLayoutPanel3.Size = New System.Drawing.Size(350, 30)
            Me.FlowLayoutPanel3.TabIndex = 8
            '
            'AddItemButton
            '
            Me.AddItemButton.Image = Global.YieldMap.My.Resources.Resources.add
            Me.AddItemButton.Location = New System.Drawing.Point(3, 3)
            Me.AddItemButton.Name = "AddItemButton"
            Me.AddItemButton.Size = New System.Drawing.Size(42, 27)
            Me.AddItemButton.TabIndex = 5
            Me.AddItemButton.UseVisualStyleBackColor = True
            '
            'RemoveItemButton
            '
            Me.RemoveItemButton.Enabled = False
            Me.RemoveItemButton.Image = Global.YieldMap.My.Resources.Resources.delete
            Me.RemoveItemButton.Location = New System.Drawing.Point(51, 3)
            Me.RemoveItemButton.Name = "RemoveItemButton"
            Me.RemoveItemButton.Size = New System.Drawing.Size(42, 27)
            Me.RemoveItemButton.TabIndex = 5
            Me.RemoveItemButton.UseVisualStyleBackColor = True
            '
            'TabPage1
            '
            Me.TabPage1.Controls.Add(Me.TableLayoutPanel1)
            Me.TabPage1.Location = New System.Drawing.Point(4, 22)
            Me.TabPage1.Name = "TabPage1"
            Me.TabPage1.Size = New System.Drawing.Size(899, 539)
            Me.TabPage1.TabIndex = 4
            Me.TabPage1.Text = "Chains"
            Me.TabPage1.UseVisualStyleBackColor = True
            '
            'TableLayoutPanel1
            '
            Me.TableLayoutPanel1.ColumnCount = 1
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel1.Controls.Add(Me.ChainsDGV, 0, 0)
            Me.TableLayoutPanel1.Controls.Add(Me.FlowLayoutPanel1, 0, 1)
            Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
            Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
            Me.TableLayoutPanel1.RowCount = 2
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TableLayoutPanel1.Size = New System.Drawing.Size(899, 539)
            Me.TableLayoutPanel1.TabIndex = 1
            '
            'ChainsDGV
            '
            Me.ChainsDGV.AllowUserToAddRows = False
            Me.ChainsDGV.AllowUserToDeleteRows = False
            Me.ChainsDGV.AutoGenerateColumns = False
            Me.ChainsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.ChainsDGV.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IdDataGridViewTextBoxColumn, Me.ChainnameDataGridViewTextBoxColumn2, Me.DescrDataGridViewTextBoxColumn, Me.ColorDataGridViewTextBoxColumn, Me.ColumnChainBid, Me.ColumnChainAsk, Me.ColumnChainLast, Me.ColumnChainHist, Me.ColumnChainCurve})
            Me.ChainsDGV.DataSource = Me.ChainBindingSource
            Me.ChainsDGV.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ChainsDGV.Location = New System.Drawing.Point(3, 3)
            Me.ChainsDGV.Name = "ChainsDGV"
            Me.ChainsDGV.ReadOnly = True
            Me.ChainsDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.ChainsDGV.Size = New System.Drawing.Size(893, 508)
            Me.ChainsDGV.TabIndex = 1
            '
            'IdDataGridViewTextBoxColumn
            '
            Me.IdDataGridViewTextBoxColumn.DataPropertyName = "id"
            Me.IdDataGridViewTextBoxColumn.HeaderText = "id"
            Me.IdDataGridViewTextBoxColumn.Name = "IdDataGridViewTextBoxColumn"
            Me.IdDataGridViewTextBoxColumn.ReadOnly = True
            Me.IdDataGridViewTextBoxColumn.Visible = False
            '
            'ChainnameDataGridViewTextBoxColumn2
            '
            Me.ChainnameDataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.ChainnameDataGridViewTextBoxColumn2.DataPropertyName = "chain_name"
            Me.ChainnameDataGridViewTextBoxColumn2.HeaderText = "RIC"
            Me.ChainnameDataGridViewTextBoxColumn2.Name = "ChainnameDataGridViewTextBoxColumn2"
            Me.ChainnameDataGridViewTextBoxColumn2.ReadOnly = True
            Me.ChainnameDataGridViewTextBoxColumn2.Width = 50
            '
            'DescrDataGridViewTextBoxColumn
            '
            Me.DescrDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.DescrDataGridViewTextBoxColumn.DataPropertyName = "descr"
            Me.DescrDataGridViewTextBoxColumn.HeaderText = "Description"
            Me.DescrDataGridViewTextBoxColumn.Name = "DescrDataGridViewTextBoxColumn"
            Me.DescrDataGridViewTextBoxColumn.ReadOnly = True
            '
            'ColorDataGridViewTextBoxColumn
            '
            Me.ColorDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.ColorDataGridViewTextBoxColumn.DataPropertyName = "color"
            Me.ColorDataGridViewTextBoxColumn.HeaderText = "Color"
            Me.ColorDataGridViewTextBoxColumn.Name = "ColorDataGridViewTextBoxColumn"
            Me.ColorDataGridViewTextBoxColumn.ReadOnly = True
            Me.ColorDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
            Me.ColorDataGridViewTextBoxColumn.Width = 56
            '
            'ColumnChainBid
            '
            Me.ColumnChainBid.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.ColumnChainBid.DataPropertyName = "bid_field"
            Me.ColumnChainBid.HeaderText = "Bid"
            Me.ColumnChainBid.Name = "ColumnChainBid"
            Me.ColumnChainBid.ReadOnly = True
            Me.ColumnChainBid.Width = 47
            '
            'ColumnChainAsk
            '
            Me.ColumnChainAsk.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.ColumnChainAsk.DataPropertyName = "ask_field"
            Me.ColumnChainAsk.HeaderText = "Ask"
            Me.ColumnChainAsk.Name = "ColumnChainAsk"
            Me.ColumnChainAsk.ReadOnly = True
            Me.ColumnChainAsk.Width = 50
            '
            'ColumnChainLast
            '
            Me.ColumnChainLast.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.ColumnChainLast.DataPropertyName = "last_field"
            Me.ColumnChainLast.HeaderText = "Last"
            Me.ColumnChainLast.Name = "ColumnChainLast"
            Me.ColumnChainLast.ReadOnly = True
            Me.ColumnChainLast.Width = 52
            '
            'ColumnChainHist
            '
            Me.ColumnChainHist.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.ColumnChainHist.DataPropertyName = "hist_field"
            Me.ColumnChainHist.HeaderText = "History"
            Me.ColumnChainHist.Name = "ColumnChainHist"
            Me.ColumnChainHist.ReadOnly = True
            Me.ColumnChainHist.Width = 64
            '
            'ColumnChainCurve
            '
            Me.ColumnChainCurve.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.ColumnChainCurve.DataPropertyName = "curve"
            Me.ColumnChainCurve.HeaderText = "Curve"
            Me.ColumnChainCurve.Name = "ColumnChainCurve"
            Me.ColumnChainCurve.ReadOnly = True
            Me.ColumnChainCurve.Width = 41
            '
            'ChainBindingSource
            '
            Me.ChainBindingSource.DataMember = "chain"
            Me.ChainBindingSource.DataSource = Me.BondsDataSet
            '
            'FlowLayoutPanel1
            '
            Me.FlowLayoutPanel1.Controls.Add(Me.AddChain)
            Me.FlowLayoutPanel1.Controls.Add(Me.EditChain)
            Me.FlowLayoutPanel1.Controls.Add(Me.RemoveChain)
            Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 514)
            Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
            Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
            Me.FlowLayoutPanel1.Size = New System.Drawing.Size(899, 25)
            Me.FlowLayoutPanel1.TabIndex = 2
            '
            'AddChain
            '
            Me.AddChain.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.AddChain.Image = Global.YieldMap.My.Resources.Resources.add
            Me.AddChain.Location = New System.Drawing.Point(3, 0)
            Me.AddChain.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
            Me.AddChain.Name = "AddChain"
            Me.AddChain.Size = New System.Drawing.Size(54, 25)
            Me.AddChain.TabIndex = 15
            Me.AddChain.UseVisualStyleBackColor = True
            '
            'EditChain
            '
            Me.EditChain.Anchor = System.Windows.Forms.AnchorStyles.Right
            Me.EditChain.Image = Global.YieldMap.My.Resources.Resources.pencil
            Me.EditChain.Location = New System.Drawing.Point(63, 0)
            Me.EditChain.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
            Me.EditChain.Name = "EditChain"
            Me.EditChain.Size = New System.Drawing.Size(54, 25)
            Me.EditChain.TabIndex = 14
            Me.EditChain.UseVisualStyleBackColor = True
            '
            'RemoveChain
            '
            Me.RemoveChain.Anchor = System.Windows.Forms.AnchorStyles.Left
            Me.RemoveChain.Image = Global.YieldMap.My.Resources.Resources.delete
            Me.RemoveChain.Location = New System.Drawing.Point(123, 0)
            Me.RemoveChain.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
            Me.RemoveChain.Name = "RemoveChain"
            Me.RemoveChain.Size = New System.Drawing.Size(54, 25)
            Me.RemoveChain.TabIndex = 13
            Me.RemoveChain.UseVisualStyleBackColor = True
            '
            'TabPage2
            '
            Me.TabPage2.Controls.Add(Me.MessageListBox)
            Me.TabPage2.Controls.Add(Me.ReloadBondsButton)
            Me.TabPage2.Controls.Add(Me.DbUpdatedLabel)
            Me.TabPage2.Controls.Add(Me.Label1)
            Me.TabPage2.Location = New System.Drawing.Point(4, 22)
            Me.TabPage2.Name = "TabPage2"
            Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
            Me.TabPage2.Size = New System.Drawing.Size(899, 539)
            Me.TabPage2.TabIndex = 5
            Me.TabPage2.Text = "Database"
            Me.TabPage2.UseVisualStyleBackColor = True
            '
            'ReloadBondsButton
            '
            Me.ReloadBondsButton.Location = New System.Drawing.Point(11, 170)
            Me.ReloadBondsButton.Name = "ReloadBondsButton"
            Me.ReloadBondsButton.Size = New System.Drawing.Size(220, 23)
            Me.ReloadBondsButton.TabIndex = 2
            Me.ReloadBondsButton.Text = "Reload bond descriptions"
            Me.ReloadBondsButton.UseVisualStyleBackColor = True
            '
            'DbUpdatedLabel
            '
            Me.DbUpdatedLabel.AutoSize = True
            Me.DbUpdatedLabel.Location = New System.Drawing.Point(86, 13)
            Me.DbUpdatedLabel.Name = "DbUpdatedLabel"
            Me.DbUpdatedLabel.Size = New System.Drawing.Size(113, 13)
            Me.DbUpdatedLabel.TabIndex = 1
            Me.DbUpdatedLabel.Text = "== last update date =="
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(8, 13)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(72, 13)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "Last updated:"
            '
            'CidDataGridViewTextBoxColumn
            '
            Me.CidDataGridViewTextBoxColumn.DataPropertyName = "cid"
            Me.CidDataGridViewTextBoxColumn.HeaderText = "cid"
            Me.CidDataGridViewTextBoxColumn.Name = "CidDataGridViewTextBoxColumn"
            Me.CidDataGridViewTextBoxColumn.ReadOnly = True
            Me.CidDataGridViewTextBoxColumn.Visible = False
            '
            'ChainnameDataGridViewTextBoxColumn
            '
            Me.ChainnameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.ChainnameDataGridViewTextBoxColumn.DataPropertyName = "chain_name"
            Me.ChainnameDataGridViewTextBoxColumn.HeaderText = "Name"
            Me.ChainnameDataGridViewTextBoxColumn.Name = "ChainnameDataGridViewTextBoxColumn"
            Me.ChainnameDataGridViewTextBoxColumn.ReadOnly = True
            '
            'IncludeDataGridViewCheckBoxColumn
            '
            Me.IncludeDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.IncludeDataGridViewCheckBoxColumn.DataPropertyName = "include"
            Me.IncludeDataGridViewCheckBoxColumn.HeaderText = "Include"
            Me.IncludeDataGridViewCheckBoxColumn.Name = "IncludeDataGridViewCheckBoxColumn"
            Me.IncludeDataGridViewCheckBoxColumn.ReadOnly = True
            '
            'CloseButton
            '
            Me.CloseButton.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.CloseButton.Location = New System.Drawing.Point(12, 583)
            Me.CloseButton.Name = "CloseButton"
            Me.CloseButton.Size = New System.Drawing.Size(122, 23)
            Me.CloseButton.TabIndex = 5
            Me.CloseButton.Text = "Close"
            Me.CloseButton.UseVisualStyleBackColor = True
            '
            'DataGridViewTextBoxColumn1
            '
            Me.DataGridViewTextBoxColumn1.DataPropertyName = "Bond"
            Me.DataGridViewTextBoxColumn1.HeaderText = "Bond"
            Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
            '
            'DataGridViewTextBoxColumn2
            '
            Me.DataGridViewTextBoxColumn2.DataPropertyName = "bondshortname"
            Me.DataGridViewTextBoxColumn2.HeaderText = "bondshortname"
            Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
            '
            'DataGridViewTextBoxColumn3
            '
            Me.DataGridViewTextBoxColumn3.DataPropertyName = "[bondshortname]"
            Me.DataGridViewTextBoxColumn3.HeaderText = "Bond"
            Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
            '
            'DataGridViewTextBoxColumn4
            '
            Me.DataGridViewTextBoxColumn4.DataPropertyName = "Bond"
            Me.DataGridViewTextBoxColumn4.HeaderText = "Bond"
            Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
            Me.DataGridViewTextBoxColumn4.ReadOnly = True
            '
            'DataGridViewTextBoxColumn5
            '
            Me.DataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.DataGridViewTextBoxColumn5.DataPropertyName = "bondshortname"
            Me.DataGridViewTextBoxColumn5.HeaderText = "Bond"
            Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
            Me.DataGridViewTextBoxColumn5.ReadOnly = True
            '
            'DataGridViewTextBoxColumn6
            '
            Me.DataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.DataGridViewTextBoxColumn6.DataPropertyName = "[bondshortname]"
            Me.DataGridViewTextBoxColumn6.HeaderText = "Bond"
            Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
            Me.DataGridViewTextBoxColumn6.ReadOnly = True
            '
            'HawserTableAdapter
            '
            Me.HawserTableAdapter.ClearBeforeFill = True
            '
            'HawserDataTableAdapter
            '
            Me.HawserDataTableAdapter.ClearBeforeFill = True
            '
            'PortfolioTableAdapter
            '
            Me.PortfolioTableAdapter.ClearBeforeFill = True
            '
            '_portfolioByBondsTableAdapter
            '
            Me._portfolioByBondsTableAdapter.ClearBeforeFill = True
            '
            'ChainsInPortfolioTableAdapter
            '
            Me.ChainsInPortfolioTableAdapter.ClearBeforeFill = True
            '
            'HawsersInPortfolioTableAdapter
            '
            Me.HawsersInPortfolioTableAdapter.ClearBeforeFill = True
            '
            'PortfolioUnitedTableAdapter
            '
            Me.PortfolioUnitedTableAdapter.ClearBeforeFill = True
            '
            'DataGridViewTextBoxColumn7
            '
            Me.DataGridViewTextBoxColumn7.DataPropertyName = "Bond"
            Me.DataGridViewTextBoxColumn7.HeaderText = "Bond"
            Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
            '
            'DataGridViewTextBoxColumn8
            '
            Me.DataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.DataGridViewTextBoxColumn8.DataPropertyName = "bondshortname"
            Me.DataGridViewTextBoxColumn8.HeaderText = "Bond"
            Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
            '
            'DataGridViewTextBoxColumn9
            '
            Me.DataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.DataGridViewTextBoxColumn9.DataPropertyName = "bondshortname"
            Me.DataGridViewTextBoxColumn9.HeaderText = "Bond"
            Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
            '
            'DataGridViewTextBoxColumn10
            '
            Me.DataGridViewTextBoxColumn10.DataPropertyName = "Bond"
            Me.DataGridViewTextBoxColumn10.HeaderText = "Bond"
            Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
            '
            'ChainTableAdapter
            '
            Me.ChainTableAdapter.ClearBeforeFill = True
            '
            'MessageListBox
            '
            Me.MessageListBox.FormattingEnabled = True
            Me.MessageListBox.Location = New System.Drawing.Point(10, 30)
            Me.MessageListBox.Name = "MessageListBox"
            Me.MessageListBox.Size = New System.Drawing.Size(431, 134)
            Me.MessageListBox.TabIndex = 3
            '
            'DataBaseManagerForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(921, 618)
            Me.Controls.Add(Me.MainTabControl)
            Me.Controls.Add(Me.CloseButton)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.ImeMode = System.Windows.Forms.ImeMode.[On]
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(200, 200)
            Me.Name = "DataBaseManagerForm"
            Me.Text = "Custom lists and portfolios"
            Me.MainTabControl.ResumeLayout(False)
            Me.PorfolioPage.ResumeLayout(False)
            Me.PorfolioPage.PerformLayout()
            CType(Me.PorfolioElementsDGV, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PortfolioUnitedBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.BondsDataSet, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PortBondDGV, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PortfolioByBondsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PortUserListDGV, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.HawsersInPortfolioBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PortChainsDGV, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.ChainsInPortfolioBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.PortfolioBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            Me.UDL_Page.ResumeLayout(False)
            Me.TableLayoutPanel2.ResumeLayout(False)
            CType(Me.ListOfList, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.HawserBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.ConstituentsDGW, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.HawserDataBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            Me.FlowLayoutPanel2.ResumeLayout(False)
            Me.FlowLayoutPanel3.ResumeLayout(False)
            Me.TabPage1.ResumeLayout(False)
            Me.TableLayoutPanel1.ResumeLayout(False)
            CType(Me.ChainsDGV, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.ChainBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            Me.FlowLayoutPanel1.ResumeLayout(False)
            Me.TabPage2.ResumeLayout(False)
            Me.TabPage2.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents MainTabControl As System.Windows.Forms.TabControl
        Friend WithEvents UDL_Page As System.Windows.Forms.TabPage
        Friend WithEvents ConstituentsDGW As System.Windows.Forms.DataGridView
        Friend WithEvents BondsDataSet As YieldMap.BondsDataSet
        Friend WithEvents HawserBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents HawserTableAdapter As YieldMap.BondsDataSetTableAdapters.hawserTableAdapter
        Friend WithEvents AddListButton As System.Windows.Forms.Button
        Friend WithEvents DeleteListButton As System.Windows.Forms.Button
        Friend WithEvents RemoveItemButton As System.Windows.Forms.Button
        Friend WithEvents AddItemButton As System.Windows.Forms.Button
        Friend WithEvents HawserDataBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents HawserDataTableAdapter As YieldMap.BondsDataSetTableAdapters.HawserDataTableAdapter
        Friend WithEvents RenameButton As System.Windows.Forms.Button
        Friend WithEvents CloseButton As System.Windows.Forms.Button
        Friend WithEvents PorfolioPage As System.Windows.Forms.TabPage
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents RenamePortfolioButton As System.Windows.Forms.Button
        Friend WithEvents AddPortfolioButton As System.Windows.Forms.Button
        Friend WithEvents DeletePortfolioButton As System.Windows.Forms.Button
        Friend WithEvents PortfoliosListBox As System.Windows.Forms.ListBox
        Friend WithEvents PortBondDGV As System.Windows.Forms.DataGridView
        Friend WithEvents PortUserListDGV As System.Windows.Forms.DataGridView
        Friend WithEvents PortChainsDGV As System.Windows.Forms.DataGridView
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents ConstPortLabel As System.Windows.Forms.Label
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents PortfolioBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents PortfolioTableAdapter As YieldMap.BondsDataSetTableAdapters.portfolioTableAdapter
        Friend WithEvents PortfolioByBondsBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents _portfolioByBondsTableAdapter As YieldMap.BondsDataSetTableAdapters._portfolioByBondsTableAdapter
        Friend WithEvents ChainsInPortfolioBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents ChainsInPortfolioTableAdapter As YieldMap.BondsDataSetTableAdapters.ChainsInPortfolioTableAdapter
        Friend WithEvents BidDataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents HawsersInPortfolioBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents HawsersInPortfolioTableAdapter As YieldMap.BondsDataSetTableAdapters.HawsersInPortfolioTableAdapter
        Friend WithEvents RemoveBondButton As System.Windows.Forms.Button
        Friend WithEvents AddBondButton As System.Windows.Forms.Button
        Friend WithEvents RemoveHawserButton As System.Windows.Forms.Button
        Friend WithEvents AddHawserButton As System.Windows.Forms.Button
        Friend WithEvents RemoveChainButton As System.Windows.Forms.Button
        Friend WithEvents AddChainButton As System.Windows.Forms.Button
        Friend WithEvents PorfolioElementsDGV As System.Windows.Forms.DataGridView
        Friend WithEvents PortfolioUnitedBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents PortfolioUnitedTableAdapter As YieldMap.BondsDataSetTableAdapters.PortfolioUnitedTableAdapter
        Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents CidDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ChainnameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IncludeDataGridViewCheckBoxColumn As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents BondshortnameDataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DataGridViewTextBoxColumn5 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DataGridViewTextBoxColumn6 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DataGridViewTextBoxColumn7 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DataGridViewTextBoxColumn8 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
        Friend WithEvents DataGridViewTextBoxColumn9 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DataGridViewTextBoxColumn10 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ChainBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents ChainTableAdapter As YieldMap.BondsDataSetTableAdapters.chainTableAdapter
        Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents ChainsDGV As System.Windows.Forms.DataGridView
        Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents AddChain As System.Windows.Forms.Button
        Friend WithEvents EditChain As System.Windows.Forms.Button
        Friend WithEvents RemoveChain As System.Windows.Forms.Button
        Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents ListOfList As System.Windows.Forms.DataGridView
        Friend WithEvents FlowLayoutPanel2 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents FlowLayoutPanel3 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents HawseridDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents RICDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents BondidDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents BondDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents pid As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ric As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IncludeDataGridViewCheckBoxColumn2 As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents PidDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents hid As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents HawsernameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DataGridViewTextBoxColumn11 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IncludeDataGridViewCheckBoxColumn1 As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents PidDataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents cid As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ChainnameDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents color As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IncludeDataGridViewCheckBoxColumn4 As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents PidDataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents BidDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents BondshortnameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DataGridViewTextBoxColumn12 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IncludeDataGridViewCheckBoxColumn3 As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents IdDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ChainnameDataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents DescrDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColorDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnChainBid As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnChainAsk As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnChainLast As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnChainHist As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnChainCurve As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents IdDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents HawsernameDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColorDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnHawserBid As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnHawserAsk As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnHawserLast As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnHawserHist As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnListCurve As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
        Friend WithEvents ReloadBondsButton As System.Windows.Forms.Button
        Friend WithEvents DbUpdatedLabel As System.Windows.Forms.Label
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents MessageListBox As System.Windows.Forms.ListBox
    End Class
End Namespace