Namespace Forms
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
            Dim Label3 As System.Windows.Forms.Label
            Dim Label4 As System.Windows.Forms.Label
            Dim Label5 As System.Windows.Forms.Label
            Dim Label6 As System.Windows.Forms.Label
            Dim Label7 As System.Windows.Forms.Label
            Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
            Me.BondListDGV = New System.Windows.Forms.DataGridView()
            Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
            Me.Button2 = New System.Windows.Forms.Button()
            Me.OkButton = New System.Windows.Forms.Button()
            Me.IncludeCB = New System.Windows.Forms.CheckBox()
            Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
            Me.SectorTextBox = New System.Windows.Forms.TextBox()
            Me.CurrencyTextBox = New System.Windows.Forms.TextBox()
            Me.SettingsButton = New System.Windows.Forms.Button()
            Me.IssuerTextBox = New System.Windows.Forms.TextBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.RICTextBox = New System.Windows.Forms.TextBox()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.SelectColumnsCMS = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.SelectColumnsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.BorrowerTextBox = New System.Windows.Forms.TextBox()
            Me.SubIndTextBox = New System.Windows.Forms.TextBox()
            Me.IssCntTextBox = New System.Windows.Forms.TextBox()
            Me.BorCntTextBox = New System.Windows.Forms.TextBox()
            Label3 = New System.Windows.Forms.Label()
            Label4 = New System.Windows.Forms.Label()
            Label5 = New System.Windows.Forms.Label()
            Label6 = New System.Windows.Forms.Label()
            Label7 = New System.Windows.Forms.Label()
            Me.TableLayoutPanel1.SuspendLayout()
            CType(Me.BondListDGV, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TableLayoutPanel2.SuspendLayout()
            Me.TableLayoutPanel3.SuspendLayout()
            Me.SelectColumnsCMS.SuspendLayout()
            Me.SuspendLayout()
            '
            'Label3
            '
            Label3.AutoSize = True
            Label3.Location = New System.Drawing.Point(207, 3)
            Label3.Margin = New System.Windows.Forms.Padding(3)
            Label3.Name = "Label3"
            Label3.Size = New System.Drawing.Size(49, 13)
            Label3.TabIndex = 14
            Label3.Text = "Currency"
            '
            'Label4
            '
            Label4.AutoSize = True
            Label4.Location = New System.Drawing.Point(207, 27)
            Label4.Margin = New System.Windows.Forms.Padding(3)
            Label4.Name = "Label4"
            Label4.Size = New System.Drawing.Size(44, 13)
            Label4.TabIndex = 15
            Label4.Text = "Industry"
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
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TableLayoutPanel1.Size = New System.Drawing.Size(738, 526)
            Me.TableLayoutPanel1.TabIndex = 10
            '
            'BondListDGV
            '
            Me.BondListDGV.AllowUserToAddRows = False
            Me.BondListDGV.AllowUserToDeleteRows = False
            Me.BondListDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.BondListDGV.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BondListDGV.Location = New System.Drawing.Point(3, 78)
            Me.BondListDGV.Name = "BondListDGV"
            Me.BondListDGV.ReadOnly = True
            Me.BondListDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.BondListDGV.Size = New System.Drawing.Size(732, 420)
            Me.BondListDGV.TabIndex = 7
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
            Me.TableLayoutPanel3.ColumnCount = 7
            Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
            Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00062!))
            Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
            Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00062!))
            Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
            Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.99813!))
            Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00062!))
            Me.TableLayoutPanel3.Controls.Add(Me.SectorTextBox, 3, 1)
            Me.TableLayoutPanel3.Controls.Add(Me.CurrencyTextBox, 3, 0)
            Me.TableLayoutPanel3.Controls.Add(Label4, 2, 1)
            Me.TableLayoutPanel3.Controls.Add(Me.SettingsButton, 6, 0)
            Me.TableLayoutPanel3.Controls.Add(Me.IssuerTextBox, 1, 0)
            Me.TableLayoutPanel3.Controls.Add(Me.Label1, 0, 0)
            Me.TableLayoutPanel3.Controls.Add(Me.RICTextBox, 1, 2)
            Me.TableLayoutPanel3.Controls.Add(Label3, 2, 0)
            Me.TableLayoutPanel3.Controls.Add(Me.Label2, 0, 2)
            Me.TableLayoutPanel3.Controls.Add(Label5, 2, 2)
            Me.TableLayoutPanel3.Controls.Add(Label6, 4, 0)
            Me.TableLayoutPanel3.Controls.Add(Label7, 4, 1)
            Me.TableLayoutPanel3.Controls.Add(Me.Label8, 0, 1)
            Me.TableLayoutPanel3.Controls.Add(Me.BorrowerTextBox, 1, 1)
            Me.TableLayoutPanel3.Controls.Add(Me.SubIndTextBox, 3, 2)
            Me.TableLayoutPanel3.Controls.Add(Me.IssCntTextBox, 5, 0)
            Me.TableLayoutPanel3.Controls.Add(Me.BorCntTextBox, 5, 1)
            Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel3.Location = New System.Drawing.Point(0, 0)
            Me.TableLayoutPanel3.Margin = New System.Windows.Forms.Padding(0)
            Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
            Me.TableLayoutPanel3.RowCount = 3
            Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            Me.TableLayoutPanel3.Size = New System.Drawing.Size(738, 75)
            Me.TableLayoutPanel3.TabIndex = 9
            '
            'SectorTextBox
            '
            Me.SectorTextBox.Dock = System.Windows.Forms.DockStyle.Top
            Me.SectorTextBox.Location = New System.Drawing.Point(287, 27)
            Me.SectorTextBox.Name = "SectorTextBox"
            Me.SectorTextBox.Size = New System.Drawing.Size(118, 20)
            Me.SectorTextBox.TabIndex = 17
            '
            'CurrencyTextBox
            '
            Me.CurrencyTextBox.Dock = System.Windows.Forms.DockStyle.Top
            Me.CurrencyTextBox.Location = New System.Drawing.Point(287, 3)
            Me.CurrencyTextBox.Name = "CurrencyTextBox"
            Me.CurrencyTextBox.Size = New System.Drawing.Size(118, 20)
            Me.CurrencyTextBox.TabIndex = 16
            '
            'SettingsButton
            '
            Me.SettingsButton.Dock = System.Windows.Forms.DockStyle.Right
            Me.SettingsButton.Location = New System.Drawing.Point(663, 0)
            Me.SettingsButton.Margin = New System.Windows.Forms.Padding(0)
            Me.SettingsButton.Name = "SettingsButton"
            Me.SettingsButton.Size = New System.Drawing.Size(75, 24)
            Me.SettingsButton.TabIndex = 13
            Me.SettingsButton.Text = "Settings..."
            Me.SettingsButton.UseVisualStyleBackColor = True
            '
            'IssuerTextBox
            '
            Me.IssuerTextBox.Dock = System.Windows.Forms.DockStyle.Top
            Me.IssuerTextBox.Location = New System.Drawing.Point(83, 3)
            Me.IssuerTextBox.Name = "IssuerTextBox"
            Me.IssuerTextBox.Size = New System.Drawing.Size(118, 20)
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
            Me.RICTextBox.Location = New System.Drawing.Point(83, 51)
            Me.RICTextBox.Name = "RICTextBox"
            Me.RICTextBox.Size = New System.Drawing.Size(118, 20)
            Me.RICTextBox.TabIndex = 10
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(3, 51)
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
            Me.SelectColumnsCMS.Size = New System.Drawing.Size(173, 26)
            '
            'SelectColumnsToolStripMenuItem
            '
            Me.SelectColumnsToolStripMenuItem.Name = "SelectColumnsToolStripMenuItem"
            Me.SelectColumnsToolStripMenuItem.Size = New System.Drawing.Size(172, 22)
            Me.SelectColumnsToolStripMenuItem.Text = "Choose columns..."
            '
            'Label5
            '
            Label5.AutoSize = True
            Label5.Location = New System.Drawing.Point(207, 51)
            Label5.Margin = New System.Windows.Forms.Padding(3)
            Label5.Name = "Label5"
            Label5.Size = New System.Drawing.Size(63, 13)
            Label5.TabIndex = 18
            Label5.Text = "SubIndustry"
            '
            'Label6
            '
            Label6.AutoSize = True
            Label6.Location = New System.Drawing.Point(411, 3)
            Label6.Margin = New System.Windows.Forms.Padding(3)
            Label6.Name = "Label6"
            Label6.Size = New System.Drawing.Size(62, 13)
            Label6.TabIndex = 18
            Label6.Text = "Iss. Country"
            '
            'Label7
            '
            Label7.AutoSize = True
            Label7.Location = New System.Drawing.Point(411, 27)
            Label7.Margin = New System.Windows.Forms.Padding(3)
            Label7.Name = "Label7"
            Label7.Size = New System.Drawing.Size(65, 13)
            Label7.TabIndex = 18
            Label7.Text = "Bor. Country"
            '
            'Label8
            '
            Me.Label8.AutoSize = True
            Me.Label8.Location = New System.Drawing.Point(3, 27)
            Me.Label8.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
            Me.Label8.Name = "Label8"
            Me.Label8.Size = New System.Drawing.Size(49, 13)
            Me.Label8.TabIndex = 5
            Me.Label8.Text = "Borrower"
            '
            'BorrowerTextBox
            '
            Me.BorrowerTextBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BorrowerTextBox.Location = New System.Drawing.Point(83, 27)
            Me.BorrowerTextBox.Name = "BorrowerTextBox"
            Me.BorrowerTextBox.Size = New System.Drawing.Size(118, 20)
            Me.BorrowerTextBox.TabIndex = 19
            '
            'SubIndTextBox
            '
            Me.SubIndTextBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.SubIndTextBox.Location = New System.Drawing.Point(287, 51)
            Me.SubIndTextBox.Name = "SubIndTextBox"
            Me.SubIndTextBox.Size = New System.Drawing.Size(118, 20)
            Me.SubIndTextBox.TabIndex = 20
            '
            'IssCntTextBox
            '
            Me.IssCntTextBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.IssCntTextBox.Location = New System.Drawing.Point(491, 3)
            Me.IssCntTextBox.Name = "IssCntTextBox"
            Me.IssCntTextBox.Size = New System.Drawing.Size(118, 20)
            Me.IssCntTextBox.TabIndex = 21
            '
            'BorCntTextBox
            '
            Me.BorCntTextBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BorCntTextBox.Location = New System.Drawing.Point(491, 27)
            Me.BorCntTextBox.Name = "BorCntTextBox"
            Me.BorCntTextBox.Size = New System.Drawing.Size(118, 20)
            Me.BorCntTextBox.TabIndex = 22
            '
            'BondSelectorForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(738, 526)
            Me.Controls.Add(Me.TableLayoutPanel1)
            Me.Name = "BondSelectorForm"
            Me.Text = "Select a bond"
            Me.TableLayoutPanel1.ResumeLayout(False)
            CType(Me.BondListDGV, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TableLayoutPanel2.ResumeLayout(False)
            Me.TableLayoutPanel2.PerformLayout()
            Me.TableLayoutPanel3.ResumeLayout(False)
            Me.TableLayoutPanel3.PerformLayout()
            Me.SelectColumnsCMS.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        'Friend WithEvents BondsDataSet As YieldMap.BondsDataSet
        'Friend WithEvents IssuerTableAdapter As YieldMap.BondsDataSetTableAdapters.issuerTableAdapter
        'Friend WithEvents BondDescriptionsTableAdapter As YieldMap.BondsDataSetTableAdapters.BondDescriptionsTableAdapter
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
        Friend WithEvents BondshortnameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents RicDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents MaturitydateDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents SettingsButton As System.Windows.Forms.Button
        Friend WithEvents SectorTextBox As System.Windows.Forms.TextBox
        Friend WithEvents CurrencyTextBox As System.Windows.Forms.TextBox
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents BorrowerTextBox As System.Windows.Forms.TextBox
        Friend WithEvents SubIndTextBox As System.Windows.Forms.TextBox
        Friend WithEvents IssCntTextBox As System.Windows.Forms.TextBox
        Friend WithEvents BorCntTextBox As System.Windows.Forms.TextBox
    End Class
End Namespace