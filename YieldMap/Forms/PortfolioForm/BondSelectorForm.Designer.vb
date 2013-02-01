Namespace Forms.PortfolioForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class BondSelectorForm
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
            Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Me.IssuerBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.BondsDataSet = New YieldMap.BondsDataSet()
            Me.BondDescriptionsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.IssuerTableAdapter = New YieldMap.BondsDataSetTableAdapters.issuerTableAdapter()
            Me.BondDescriptionsTableAdapter = New YieldMap.BondsDataSetTableAdapters.BondDescriptionsTableAdapter()
            Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
            Me.BondListDGV = New System.Windows.Forms.DataGridView()
            Me._ricCol = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.BondshortnameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnDescr = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.RicDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnCurrency = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.issname = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnIssued = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.issue_size = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.coupon = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.MaturitydateDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnNextPutDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnNextCallDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
            Me.Button2 = New System.Windows.Forms.Button()
            Me.OkButton = New System.Windows.Forms.Button()
            Me.IncludeCB = New System.Windows.Forms.CheckBox()
            Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
            Me.IssuerTextBox = New System.Windows.Forms.TextBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.RICTextBox = New System.Windows.Forms.TextBox()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.SelectColumnsCMS = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.SelectColumnsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            CType(Me.IssuerBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.BondsDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.BondDescriptionsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TableLayoutPanel1.SuspendLayout()
            CType(Me.BondListDGV, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TableLayoutPanel2.SuspendLayout()
            Me.TableLayoutPanel3.SuspendLayout()
            Me.SelectColumnsCMS.SuspendLayout()
            Me.SuspendLayout()
            '
            'IssuerBindingSource
            '
            Me.IssuerBindingSource.DataMember = "issuer"
            Me.IssuerBindingSource.DataSource = Me.BondsDataSet
            '
            'BondsDataSet
            '
            Me.BondsDataSet.DataSetName = "BondsDataSet"
            Me.BondsDataSet.EnforceConstraints = False
            Me.BondsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'BondDescriptionsBindingSource
            '
            Me.BondDescriptionsBindingSource.DataMember = "BondDescriptions"
            Me.BondDescriptionsBindingSource.DataSource = Me.BondsDataSet
            '
            'IssuerTableAdapter
            '
            Me.IssuerTableAdapter.ClearBeforeFill = True
            '
            'BondDescriptionsTableAdapter
            '
            Me.BondDescriptionsTableAdapter.ClearBeforeFill = True
            '
            'TableLayoutPanel1
            '
            Me.TableLayoutPanel1.ColumnCount = 1
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel1.Controls.Add(Me.BondListDGV, 0, 1)
            Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 0, 2)
            Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel3, 0, 0)
            Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
            Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
            Me.TableLayoutPanel1.RowCount = 3
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TableLayoutPanel1.Size = New System.Drawing.Size(738, 526)
            Me.TableLayoutPanel1.TabIndex = 10
            '
            'BondListDGV
            '
            Me.BondListDGV.AllowUserToAddRows = False
            Me.BondListDGV.AllowUserToDeleteRows = False
            Me.BondListDGV.AutoGenerateColumns = False
            Me.BondListDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.BondListDGV.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me._ricCol, Me.BondshortnameDataGridViewTextBoxColumn, Me.ColumnDescr, Me.RicDataGridViewTextBoxColumn, Me.ColumnCurrency, Me.issname, Me.ColumnIssued, Me.issue_size, Me.coupon, Me.MaturitydateDataGridViewTextBoxColumn, Me.ColumnNextPutDate, Me.ColumnNextCallDate})
            Me.BondListDGV.DataSource = Me.BondDescriptionsBindingSource
            Me.BondListDGV.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BondListDGV.Location = New System.Drawing.Point(3, 53)
            Me.BondListDGV.Name = "BondListDGV"
            Me.BondListDGV.ReadOnly = True
            Me.BondListDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.BondListDGV.Size = New System.Drawing.Size(732, 445)
            Me.BondListDGV.TabIndex = 7
            '
            '_ricCol
            '
            Me._ricCol.DataPropertyName = "ric"
            Me._ricCol.HeaderText = "XXX"
            Me._ricCol.Name = "_ricCol"
            Me._ricCol.ReadOnly = True
            Me._ricCol.Visible = False
            '
            'BondshortnameDataGridViewTextBoxColumn
            '
            Me.BondshortnameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.BondshortnameDataGridViewTextBoxColumn.DataPropertyName = "bondshortname"
            Me.BondshortnameDataGridViewTextBoxColumn.HeaderText = "Bond"
            Me.BondshortnameDataGridViewTextBoxColumn.Name = "BondshortnameDataGridViewTextBoxColumn"
            Me.BondshortnameDataGridViewTextBoxColumn.ReadOnly = True
            Me.BondshortnameDataGridViewTextBoxColumn.Width = 57
            '
            'ColumnDescr
            '
            Me.ColumnDescr.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.ColumnDescr.DataPropertyName = "descr"
            Me.ColumnDescr.HeaderText = "Description"
            Me.ColumnDescr.Name = "ColumnDescr"
            Me.ColumnDescr.ReadOnly = True
            '
            'RicDataGridViewTextBoxColumn
            '
            Me.RicDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.RicDataGridViewTextBoxColumn.DataPropertyName = "ric"
            Me.RicDataGridViewTextBoxColumn.HeaderText = "RIC"
            Me.RicDataGridViewTextBoxColumn.Name = "RicDataGridViewTextBoxColumn"
            Me.RicDataGridViewTextBoxColumn.ReadOnly = True
            Me.RicDataGridViewTextBoxColumn.Width = 50
            '
            'ColumnCurrency
            '
            Me.ColumnCurrency.DataPropertyName = "currency"
            Me.ColumnCurrency.HeaderText = "Currency"
            Me.ColumnCurrency.Name = "ColumnCurrency"
            Me.ColumnCurrency.ReadOnly = True
            '
            'issname
            '
            Me.issname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.issname.DataPropertyName = "issname"
            Me.issname.HeaderText = "Issuer"
            Me.issname.Name = "issname"
            Me.issname.ReadOnly = True
            Me.issname.Width = 60
            '
            'ColumnIssued
            '
            Me.ColumnIssued.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.ColumnIssued.DataPropertyName = "issuedate"
            Me.ColumnIssued.HeaderText = "Issued"
            Me.ColumnIssued.Name = "ColumnIssued"
            Me.ColumnIssued.ReadOnly = True
            Me.ColumnIssued.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
            Me.ColumnIssued.Width = 63
            '
            'issue_size
            '
            Me.issue_size.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.issue_size.DataPropertyName = "issue_size"
            DataGridViewCellStyle1.Format = "N0"
            DataGridViewCellStyle1.NullValue = Nothing
            Me.issue_size.DefaultCellStyle = DataGridViewCellStyle1
            Me.issue_size.HeaderText = "Size"
            Me.issue_size.Name = "issue_size"
            Me.issue_size.ReadOnly = True
            Me.issue_size.Width = 52
            '
            'coupon
            '
            Me.coupon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.coupon.DataPropertyName = "coupon"
            DataGridViewCellStyle2.Format = "N2"
            Me.coupon.DefaultCellStyle = DataGridViewCellStyle2
            Me.coupon.HeaderText = "Coupon"
            Me.coupon.Name = "coupon"
            Me.coupon.ReadOnly = True
            Me.coupon.Width = 69
            '
            'MaturitydateDataGridViewTextBoxColumn
            '
            Me.MaturitydateDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.MaturitydateDataGridViewTextBoxColumn.DataPropertyName = "maturitydate"
            Me.MaturitydateDataGridViewTextBoxColumn.HeaderText = "Maturity"
            Me.MaturitydateDataGridViewTextBoxColumn.Name = "MaturitydateDataGridViewTextBoxColumn"
            Me.MaturitydateDataGridViewTextBoxColumn.ReadOnly = True
            Me.MaturitydateDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
            Me.MaturitydateDataGridViewTextBoxColumn.Width = 69
            '
            'ColumnNextPutDate
            '
            Me.ColumnNextPutDate.DataPropertyName = "nextputdate"
            Me.ColumnNextPutDate.HeaderText = "Next Put"
            Me.ColumnNextPutDate.Name = "ColumnNextPutDate"
            Me.ColumnNextPutDate.ReadOnly = True
            '
            'ColumnNextCallDate
            '
            Me.ColumnNextCallDate.DataPropertyName = "nextcalldate"
            Me.ColumnNextCallDate.HeaderText = "Next Call"
            Me.ColumnNextCallDate.Name = "ColumnNextCallDate"
            Me.ColumnNextCallDate.ReadOnly = True
            '
            'TableLayoutPanel2
            '
            Me.TableLayoutPanel2.ColumnCount = 3
            Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            Me.TableLayoutPanel2.Controls.Add(Me.Button2, 0, 0)
            Me.TableLayoutPanel2.Controls.Add(Me.OkButton, 0, 0)
            Me.TableLayoutPanel2.Controls.Add(Me.IncludeCB, 0, 0)
            Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 501)
            Me.TableLayoutPanel2.Margin = New System.Windows.Forms.Padding(0)
            Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
            Me.TableLayoutPanel2.RowCount = 1
            Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel2.Size = New System.Drawing.Size(738, 25)
            Me.TableLayoutPanel2.TabIndex = 8
            '
            'Button2
            '
            Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.Button2.Dock = System.Windows.Forms.DockStyle.Right
            Me.Button2.Location = New System.Drawing.Point(611, 0)
            Me.Button2.Margin = New System.Windows.Forms.Padding(0)
            Me.Button2.Name = "Button2"
            Me.Button2.Size = New System.Drawing.Size(127, 25)
            Me.Button2.TabIndex = 11
            Me.Button2.Text = "Cancel"
            Me.Button2.UseVisualStyleBackColor = True
            '
            'OkButton
            '
            Me.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.OkButton.Dock = System.Windows.Forms.DockStyle.Right
            Me.OkButton.Location = New System.Drawing.Point(365, 0)
            Me.OkButton.Margin = New System.Windows.Forms.Padding(0)
            Me.OkButton.Name = "OkButton"
            Me.OkButton.Size = New System.Drawing.Size(127, 25)
            Me.OkButton.TabIndex = 10
            Me.OkButton.Text = "Add selected"
            Me.OkButton.UseVisualStyleBackColor = True
            '
            'IncludeCB
            '
            Me.IncludeCB.AutoSize = True
            Me.IncludeCB.Checked = True
            Me.IncludeCB.CheckState = System.Windows.Forms.CheckState.Checked
            Me.IncludeCB.Location = New System.Drawing.Point(3, 3)
            Me.IncludeCB.Name = "IncludeCB"
            Me.IncludeCB.Size = New System.Drawing.Size(61, 17)
            Me.IncludeCB.TabIndex = 9
            Me.IncludeCB.Text = "Include"
            Me.IncludeCB.UseVisualStyleBackColor = True
            '
            'TableLayoutPanel3
            '
            Me.TableLayoutPanel3.ColumnCount = 4
            Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
            Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TableLayoutPanel3.Controls.Add(Me.IssuerTextBox, 1, 0)
            Me.TableLayoutPanel3.Controls.Add(Me.Label1, 0, 0)
            Me.TableLayoutPanel3.Controls.Add(Me.RICTextBox, 0, 1)
            Me.TableLayoutPanel3.Controls.Add(Me.Label2, 0, 1)
            Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel3.Location = New System.Drawing.Point(0, 0)
            Me.TableLayoutPanel3.Margin = New System.Windows.Forms.Padding(0)
            Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
            Me.TableLayoutPanel3.RowCount = 2
            Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel3.Size = New System.Drawing.Size(738, 50)
            Me.TableLayoutPanel3.TabIndex = 9
            '
            'IssuerTextBox
            '
            Me.IssuerTextBox.Dock = System.Windows.Forms.DockStyle.Top
            Me.IssuerTextBox.Location = New System.Drawing.Point(53, 3)
            Me.IssuerTextBox.Name = "IssuerTextBox"
            Me.IssuerTextBox.Size = New System.Drawing.Size(223, 20)
            Me.IssuerTextBox.TabIndex = 11
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(3, 3)
            Me.Label1.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(35, 13)
            Me.Label1.TabIndex = 5
            Me.Label1.Text = "Issuer"
            '
            'RICTextBox
            '
            Me.RICTextBox.Dock = System.Windows.Forms.DockStyle.Top
            Me.RICTextBox.Location = New System.Drawing.Point(53, 28)
            Me.RICTextBox.Name = "RICTextBox"
            Me.RICTextBox.Size = New System.Drawing.Size(223, 20)
            Me.RICTextBox.TabIndex = 10
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(3, 28)
            Me.Label2.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(25, 13)
            Me.Label2.TabIndex = 7
            Me.Label2.Text = "RIC"
            '
            'SelectColumnsCMS
            '
            Me.SelectColumnsCMS.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SelectColumnsToolStripMenuItem})
            Me.SelectColumnsCMS.Name = "SelectColumnsCMS"
            Me.SelectColumnsCMS.Size = New System.Drawing.Size(168, 48)
            '
            'SelectColumnsToolStripMenuItem
            '
            Me.SelectColumnsToolStripMenuItem.Name = "SelectColumnsToolStripMenuItem"
            Me.SelectColumnsToolStripMenuItem.Size = New System.Drawing.Size(167, 22)
            Me.SelectColumnsToolStripMenuItem.Text = "Select columns..."
            '
            'BondSelectorForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(738, 526)
            Me.Controls.Add(Me.TableLayoutPanel1)
            Me.Name = "BondSelectorForm"
            Me.Text = "Select a bond"
            CType(Me.IssuerBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.BondsDataSet, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.BondDescriptionsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TableLayoutPanel1.ResumeLayout(False)
            CType(Me.BondListDGV, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TableLayoutPanel2.ResumeLayout(False)
            Me.TableLayoutPanel2.PerformLayout()
            Me.TableLayoutPanel3.ResumeLayout(False)
            Me.TableLayoutPanel3.PerformLayout()
            Me.SelectColumnsCMS.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents BondsDataSet As YieldMap.BondsDataSet
        Friend WithEvents IssuerBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents IssuerTableAdapter As YieldMap.BondsDataSetTableAdapters.issuerTableAdapter
        Friend WithEvents BondDescriptionsBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents BondDescriptionsTableAdapter As YieldMap.BondsDataSetTableAdapters.BondDescriptionsTableAdapter
        Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents BondListDGV As System.Windows.Forms.DataGridView
        Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents Button2 As System.Windows.Forms.Button
        Friend WithEvents OkButton As System.Windows.Forms.Button
        Friend WithEvents IncludeCB As System.Windows.Forms.CheckBox
        Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents IssuerTextBox As System.Windows.Forms.TextBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents RICTextBox As System.Windows.Forms.TextBox
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents SelectColumnsCMS As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents SelectColumnsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents _ricCol As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents BondshortnameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnDescr As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents RicDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnCurrency As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents issname As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnIssued As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents issue_size As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents coupon As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents MaturitydateDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnNextPutDate As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnNextCallDate As System.Windows.Forms.DataGridViewTextBoxColumn
    End Class
End Namespace