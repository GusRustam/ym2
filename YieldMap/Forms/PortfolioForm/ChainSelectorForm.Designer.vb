Namespace Forms.PortfolioForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ChainSelectorForm
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
            Me.BondsDataSet = New YieldMap.BondsDataSet()
            Me.ChainBindingSource = New System.Windows.Forms.BindingSource(Me.components)
            Me.ChainTableAdapter = New YieldMap.BondsDataSetTableAdapters.chainTableAdapter()
            Me.ChainListBox = New System.Windows.Forms.ListBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.IncludeCB = New System.Windows.Forms.CheckBox()
            Me.OkButton = New System.Windows.Forms.Button()
            Me.Button2 = New System.Windows.Forms.Button()
            CType(Me.BondsDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.ChainBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'BondsDataSet
            '
            Me.BondsDataSet.DataSetName = "BondsDataSet"
            Me.BondsDataSet.EnforceConstraints = False
            Me.BondsDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            'ChainBindingSource
            '
            Me.ChainBindingSource.DataMember = "chain"
            Me.ChainBindingSource.DataSource = Me.BondsDataSet
            '
            'ChainTableAdapter
            '
            Me.ChainTableAdapter.ClearBeforeFill = True
            '
            'ChainListBox
            '
            Me.ChainListBox.DataSource = Me.ChainBindingSource
            Me.ChainListBox.DisplayMember = "chain_name"
            Me.ChainListBox.FormattingEnabled = True
            Me.ChainListBox.Location = New System.Drawing.Point(12, 23)
            Me.ChainListBox.Name = "ChainListBox"
            Me.ChainListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
            Me.ChainListBox.Size = New System.Drawing.Size(282, 316)
            Me.ChainListBox.TabIndex = 0
            Me.ChainListBox.ValueMember = "id"
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(9, 7)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(130, 13)
            Me.Label1.TabIndex = 1
            Me.Label1.Text = "Select one or more chains"
            '
            'IncludeCB
            '
            Me.IncludeCB.AutoSize = True
            Me.IncludeCB.Checked = True
            Me.IncludeCB.CheckState = System.Windows.Forms.CheckState.Checked
            Me.IncludeCB.Location = New System.Drawing.Point(13, 346)
            Me.IncludeCB.Name = "IncludeCB"
            Me.IncludeCB.Size = New System.Drawing.Size(61, 17)
            Me.IncludeCB.TabIndex = 2
            Me.IncludeCB.Text = "Include"
            Me.IncludeCB.UseVisualStyleBackColor = True
            '
            'OkButton
            '
            Me.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.OkButton.Location = New System.Drawing.Point(138, 346)
            Me.OkButton.Name = "OkButton"
            Me.OkButton.Size = New System.Drawing.Size(75, 23)
            Me.OkButton.TabIndex = 3
            Me.OkButton.Text = "Ok"
            Me.OkButton.UseVisualStyleBackColor = True
            '
            'Button2
            '
            Me.Button2.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.Button2.Location = New System.Drawing.Point(219, 346)
            Me.Button2.Name = "Button2"
            Me.Button2.Size = New System.Drawing.Size(75, 23)
            Me.Button2.TabIndex = 3
            Me.Button2.Text = "Cancel"
            Me.Button2.UseVisualStyleBackColor = True
            '
            'ChainSelectorForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(306, 378)
            Me.Controls.Add(Me.Button2)
            Me.Controls.Add(Me.OkButton)
            Me.Controls.Add(Me.IncludeCB)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.ChainListBox)
            Me.Name = "ChainSelectorForm"
            Me.Text = "Select chain"
            CType(Me.BondsDataSet, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.ChainBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents BondsDataSet As YieldMap.BondsDataSet
        Friend WithEvents ChainBindingSource As System.Windows.Forms.BindingSource
        Friend WithEvents ChainTableAdapter As YieldMap.BondsDataSetTableAdapters.chainTableAdapter
        Friend WithEvents ChainListBox As System.Windows.Forms.ListBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents IncludeCB As System.Windows.Forms.CheckBox
        Friend WithEvents OkButton As System.Windows.Forms.Button
        Friend WithEvents Button2 As System.Windows.Forms.Button
    End Class
End Namespace