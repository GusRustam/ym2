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
            Me.ShowAllCB = New System.Windows.Forms.CheckBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.IssuerCB = New System.Windows.Forms.ComboBox()
            Me.IssuerBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.BondsDataSet = New YieldMap.BondsDataSet()
            Me.BondListDGV = New System.Windows.Forms.DataGridView()
            Me.BondshortnameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.RicDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.issname = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.issue_size = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.coupon = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.MaturitydateDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.IssueridDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.BondDescriptionsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.IssuerTableAdapter = New YieldMap.BondsDataSetTableAdapters.issuerTableAdapter()
            Me.BondDescriptionsTableAdapter = New YieldMap.BondsDataSetTableAdapters.BondDescriptionsTableAdapter()
            Me.OkButton = New System.Windows.Forms.Button()
            Me.Button2 = New System.Windows.Forms.Button()
            Me.IncludeCB = New System.Windows.Forms.CheckBox()
            CType(Me.IssuerBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.BondsDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.BondListDGV, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.BondDescriptionsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'ShowAllCB
            '
            Me.ShowAllCB.AutoSize = True
            Me.ShowAllCB.Checked = True
            Me.ShowAllCB.CheckState = System.Windows.Forms.CheckState.Checked
            Me.ShowAllCB.Location = New System.Drawing.Point(241, 14)
            Me.ShowAllCB.Name = "ShowAllCB"
            Me.ShowAllCB.Size = New System.Drawing.Size(73, 17)
            Me.ShowAllCB.TabIndex = 5
            Me.ShowAllCB.Text = "any issuer"
            Me.ShowAllCB.UseVisualStyleBackColor = True
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(13, 15)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(35, 13)
            Me.Label1.TabIndex = 4
            Me.Label1.Text = "Issuer"
            '
            'IssuerCB
            '
            Me.IssuerCB.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
            Me.IssuerCB.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
            Me.IssuerCB.DataSource = Me.IssuerBindingSource
            Me.IssuerCB.DisplayMember = "shortname"
            Me.IssuerCB.Enabled = False
            Me.IssuerCB.FormattingEnabled = True
            Me.IssuerCB.Location = New System.Drawing.Point(54, 12)
            Me.IssuerCB.Name = "IssuerCB"
            Me.IssuerCB.Size = New System.Drawing.Size(171, 21)
            Me.IssuerCB.TabIndex = 3
            Me.IssuerCB.ValueMember = "id"
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
            'BondListDGV
            '
            Me.BondListDGV.AllowUserToAddRows = False
            Me.BondListDGV.AllowUserToDeleteRows = False
            Me.BondListDGV.AutoGenerateColumns = False
            Me.BondListDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.BondListDGV.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.BondshortnameDataGridViewTextBoxColumn, Me.RicDataGridViewTextBoxColumn, Me.issname, Me.issue_size, Me.coupon, Me.MaturitydateDataGridViewTextBoxColumn, Me.IssueridDataGridViewTextBoxColumn})
            Me.BondListDGV.DataSource = Me.BondDescriptionsBindingSource
            Me.BondListDGV.Location = New System.Drawing.Point(16, 39)
            Me.BondListDGV.Name = "BondListDGV"
            Me.BondListDGV.ReadOnly = True
            Me.BondListDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.BondListDGV.Size = New System.Drawing.Size(710, 409)
            Me.BondListDGV.TabIndex = 6
            '
            'BondshortnameDataGridViewTextBoxColumn
            '
            Me.BondshortnameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.BondshortnameDataGridViewTextBoxColumn.DataPropertyName = "bondshortname"
            Me.BondshortnameDataGridViewTextBoxColumn.HeaderText = "Bond"
            Me.BondshortnameDataGridViewTextBoxColumn.Name = "BondshortnameDataGridViewTextBoxColumn"
            Me.BondshortnameDataGridViewTextBoxColumn.ReadOnly = True
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
            'issname
            '
            Me.issname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.issname.DataPropertyName = "issname"
            Me.issname.HeaderText = "Issuer"
            Me.issname.Name = "issname"
            Me.issname.ReadOnly = True
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
            Me.MaturitydateDataGridViewTextBoxColumn.Width = 69
            '
            'IssueridDataGridViewTextBoxColumn
            '
            Me.IssueridDataGridViewTextBoxColumn.DataPropertyName = "issuer_id"
            Me.IssueridDataGridViewTextBoxColumn.HeaderText = "issuer_id"
            Me.IssueridDataGridViewTextBoxColumn.Name = "IssueridDataGridViewTextBoxColumn"
            Me.IssueridDataGridViewTextBoxColumn.ReadOnly = True
            Me.IssueridDataGridViewTextBoxColumn.Visible = False
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
            'OkButton
            '
            Me.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.OkButton.Location = New System.Drawing.Point(447, 455)
            Me.OkButton.Name = "OkButton"
            Me.OkButton.Size = New System.Drawing.Size(127, 23)
            Me.OkButton.TabIndex = 7
            Me.OkButton.Text = "Add selected"
            Me.OkButton.UseVisualStyleBackColor = True
            '
            'Button2
            '
            Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.Button2.Location = New System.Drawing.Point(599, 455)
            Me.Button2.Name = "Button2"
            Me.Button2.Size = New System.Drawing.Size(127, 23)
            Me.Button2.TabIndex = 7
            Me.Button2.Text = "Cancel"
            Me.Button2.UseVisualStyleBackColor = True
            '
            'IncludeCB
            '
            Me.IncludeCB.AutoSize = True
            Me.IncludeCB.Checked = True
            Me.IncludeCB.CheckState = System.Windows.Forms.CheckState.Checked
            Me.IncludeCB.Location = New System.Drawing.Point(16, 455)
            Me.IncludeCB.Name = "IncludeCB"
            Me.IncludeCB.Size = New System.Drawing.Size(61, 17)
            Me.IncludeCB.TabIndex = 8
            Me.IncludeCB.Text = "Include"
            Me.IncludeCB.UseVisualStyleBackColor = True
            '
            'BondSelectorForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(738, 505)
            Me.Controls.Add(Me.IncludeCB)
            Me.Controls.Add(Me.Button2)
            Me.Controls.Add(Me.OkButton)
            Me.Controls.Add(Me.BondListDGV)
            Me.Controls.Add(Me.ShowAllCB)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.IssuerCB)
            Me.Name = "BondSelectorForm"
            Me.Text = "Select a bond"
            CType(Me.IssuerBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.BondsDataSet, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.BondListDGV, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.BondDescriptionsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents ShowAllCB As System.Windows.Forms.CheckBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents IssuerCB As System.Windows.Forms.ComboBox
        Friend WithEvents BondListDGV As System.Windows.Forms.DataGridView
        Friend WithEvents BondsDataSet As YieldMap.BondsDataSet
        Friend WithEvents IssuerBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents IssuerTableAdapter As YieldMap.BondsDataSetTableAdapters.issuerTableAdapter
        Friend WithEvents BondDescriptionsBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents BondDescriptionsTableAdapter As YieldMap.BondsDataSetTableAdapters.BondDescriptionsTableAdapter
        Friend WithEvents OkButton As System.Windows.Forms.Button
        Friend WithEvents Button2 As System.Windows.Forms.Button
        Friend WithEvents IncludeCB As System.Windows.Forms.CheckBox
        Friend WithEvents BondshortnameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents RicDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents issname As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents issue_size As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents coupon As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents MaturitydateDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents IssueridDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    End Class
End Namespace