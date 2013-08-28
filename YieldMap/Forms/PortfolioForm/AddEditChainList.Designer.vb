Namespace Forms.PortfolioForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class AddEditChainList
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
            Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
            Me.CancelButton = New System.Windows.Forms.Button()
            Me.OkButton = New System.Windows.Forms.Button()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.Label6 = New System.Windows.Forms.Label()
            Me.NameTextBox = New System.Windows.Forms.TextBox()
            Me.FieldLayoutComboBox = New System.Windows.Forms.ComboBox()
            Me.CurveCheckBox = New System.Windows.Forms.CheckBox()
            Me.EnabledCheckBox = New System.Windows.Forms.CheckBox()
            Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
            Me.ColorComboBox = New System.Windows.Forms.ComboBox()
            Me.ColorBox = New System.Windows.Forms.PictureBox()
            Me.RandomColorButton = New System.Windows.Forms.Button()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel()
            Me.ChainRadioButton = New System.Windows.Forms.RadioButton()
            Me.ListRadioButton = New System.Windows.Forms.RadioButton()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.FlowLayoutPanel3 = New System.Windows.Forms.FlowLayoutPanel()
            Me.ConditionTextBox = New System.Windows.Forms.TextBox()
            Me.EditConditionButton = New System.Windows.Forms.Button()
            Me.QueryRadioButton = New System.Windows.Forms.RadioButton()
            Me.ChainRicTextBox = New System.Windows.Forms.ComboBox()
            Me.TableLayoutPanel1.SuspendLayout()
            Me.FlowLayoutPanel1.SuspendLayout()
            CType(Me.ColorBox, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.FlowLayoutPanel2.SuspendLayout()
            Me.FlowLayoutPanel3.SuspendLayout()
            Me.SuspendLayout()
            '
            'TableLayoutPanel1
            '
            Me.TableLayoutPanel1.ColumnCount = 2
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel1.Controls.Add(Me.CancelButton, 1, 9)
            Me.TableLayoutPanel1.Controls.Add(Me.OkButton, 0, 9)
            Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 1)
            Me.TableLayoutPanel1.Controls.Add(Me.Label2, 0, 2)
            Me.TableLayoutPanel1.Controls.Add(Me.Label3, 0, 3)
            Me.TableLayoutPanel1.Controls.Add(Me.Label4, 0, 4)
            Me.TableLayoutPanel1.Controls.Add(Me.Label5, 0, 5)
            Me.TableLayoutPanel1.Controls.Add(Me.Label6, 0, 6)
            Me.TableLayoutPanel1.Controls.Add(Me.NameTextBox, 1, 1)
            Me.TableLayoutPanel1.Controls.Add(Me.FieldLayoutComboBox, 1, 2)
            Me.TableLayoutPanel1.Controls.Add(Me.CurveCheckBox, 1, 4)
            Me.TableLayoutPanel1.Controls.Add(Me.EnabledCheckBox, 1, 5)
            Me.TableLayoutPanel1.Controls.Add(Me.FlowLayoutPanel1, 1, 3)
            Me.TableLayoutPanel1.Controls.Add(Me.Label7, 0, 0)
            Me.TableLayoutPanel1.Controls.Add(Me.FlowLayoutPanel2, 1, 0)
            Me.TableLayoutPanel1.Controls.Add(Me.Label8, 0, 7)
            Me.TableLayoutPanel1.Controls.Add(Me.FlowLayoutPanel3, 1, 7)
            Me.TableLayoutPanel1.Controls.Add(Me.ChainRicTextBox, 1, 6)
            Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
            Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
            Me.TableLayoutPanel1.RowCount = 10
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0001!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0001!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0001!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0001!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0001!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0001!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0001!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0001!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.999102!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0001!))
            Me.TableLayoutPanel1.Size = New System.Drawing.Size(461, 273)
            Me.TableLayoutPanel1.TabIndex = 0
            '
            'CancelButton
            '
            Me.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.CancelButton.Location = New System.Drawing.Point(123, 246)
            Me.CancelButton.Name = "CancelButton"
            Me.CancelButton.Size = New System.Drawing.Size(75, 20)
            Me.CancelButton.TabIndex = 8
            Me.CancelButton.Text = "Cancel"
            Me.CancelButton.UseVisualStyleBackColor = True
            '
            'OkButton
            '
            Me.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.OkButton.Location = New System.Drawing.Point(3, 246)
            Me.OkButton.Name = "OkButton"
            Me.OkButton.Size = New System.Drawing.Size(75, 20)
            Me.OkButton.TabIndex = 7
            Me.OkButton.Text = "Ok"
            Me.OkButton.UseVisualStyleBackColor = True
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(3, 30)
            Me.Label1.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(35, 13)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "Name"
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(3, 57)
            Me.Label2.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(65, 13)
            Me.Label2.TabIndex = 1
            Me.Label2.Text = "Fields layout"
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(3, 84)
            Me.Label3.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(31, 13)
            Me.Label3.TabIndex = 2
            Me.Label3.Text = "Color"
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(3, 111)
            Me.Label4.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(76, 13)
            Me.Label4.TabIndex = 3
            Me.Label4.Text = "Can plot curve"
            '
            'Label5
            '
            Me.Label5.AutoSize = True
            Me.Label5.Location = New System.Drawing.Point(3, 138)
            Me.Label5.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(46, 13)
            Me.Label5.TabIndex = 4
            Me.Label5.Text = "Enabled"
            '
            'Label6
            '
            Me.Label6.AutoSize = True
            Me.Label6.Location = New System.Drawing.Point(3, 165)
            Me.Label6.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New System.Drawing.Size(55, 13)
            Me.Label6.TabIndex = 9
            Me.Label6.Text = "Chain RIC"
            '
            'NameTextBox
            '
            Me.NameTextBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.NameTextBox.Location = New System.Drawing.Point(123, 30)
            Me.NameTextBox.Name = "NameTextBox"
            Me.NameTextBox.Size = New System.Drawing.Size(335, 20)
            Me.NameTextBox.TabIndex = 10
            '
            'FieldLayoutComboBox
            '
            Me.FieldLayoutComboBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FieldLayoutComboBox.FormattingEnabled = True
            Me.FieldLayoutComboBox.Location = New System.Drawing.Point(123, 57)
            Me.FieldLayoutComboBox.Name = "FieldLayoutComboBox"
            Me.FieldLayoutComboBox.Size = New System.Drawing.Size(335, 21)
            Me.FieldLayoutComboBox.TabIndex = 11
            '
            'CurveCheckBox
            '
            Me.CurveCheckBox.AutoSize = True
            Me.CurveCheckBox.Location = New System.Drawing.Point(123, 111)
            Me.CurveCheckBox.Name = "CurveCheckBox"
            Me.CurveCheckBox.Size = New System.Drawing.Size(15, 14)
            Me.CurveCheckBox.TabIndex = 13
            Me.CurveCheckBox.UseVisualStyleBackColor = True
            '
            'EnabledCheckBox
            '
            Me.EnabledCheckBox.AutoSize = True
            Me.EnabledCheckBox.Checked = True
            Me.EnabledCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
            Me.EnabledCheckBox.Location = New System.Drawing.Point(123, 138)
            Me.EnabledCheckBox.Name = "EnabledCheckBox"
            Me.EnabledCheckBox.Size = New System.Drawing.Size(15, 14)
            Me.EnabledCheckBox.TabIndex = 14
            Me.EnabledCheckBox.UseVisualStyleBackColor = True
            '
            'FlowLayoutPanel1
            '
            Me.FlowLayoutPanel1.Controls.Add(Me.ColorComboBox)
            Me.FlowLayoutPanel1.Controls.Add(Me.ColorBox)
            Me.FlowLayoutPanel1.Controls.Add(Me.RandomColorButton)
            Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FlowLayoutPanel1.Location = New System.Drawing.Point(120, 81)
            Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
            Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
            Me.FlowLayoutPanel1.Size = New System.Drawing.Size(341, 27)
            Me.FlowLayoutPanel1.TabIndex = 16
            '
            'ColorComboBox
            '
            Me.ColorComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
            Me.ColorComboBox.FormattingEnabled = True
            Me.ColorComboBox.Location = New System.Drawing.Point(3, 3)
            Me.ColorComboBox.Name = "ColorComboBox"
            Me.ColorComboBox.Size = New System.Drawing.Size(222, 21)
            Me.ColorComboBox.TabIndex = 12
            '
            'ColorBox
            '
            Me.ColorBox.Location = New System.Drawing.Point(231, 3)
            Me.ColorBox.Name = "ColorBox"
            Me.ColorBox.Size = New System.Drawing.Size(25, 21)
            Me.ColorBox.TabIndex = 14
            Me.ColorBox.TabStop = False
            '
            'RandomColorButton
            '
            Me.RandomColorButton.Location = New System.Drawing.Point(262, 3)
            Me.RandomColorButton.Name = "RandomColorButton"
            Me.RandomColorButton.Size = New System.Drawing.Size(75, 20)
            Me.RandomColorButton.TabIndex = 13
            Me.RandomColorButton.Text = "Random"
            Me.RandomColorButton.UseVisualStyleBackColor = True
            '
            'Label7
            '
            Me.Label7.AutoSize = True
            Me.Label7.Location = New System.Drawing.Point(3, 3)
            Me.Label7.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
            Me.Label7.Name = "Label7"
            Me.Label7.Size = New System.Drawing.Size(33, 13)
            Me.Label7.TabIndex = 17
            Me.Label7.Text = "What"
            '
            'FlowLayoutPanel2
            '
            Me.FlowLayoutPanel2.Controls.Add(Me.ChainRadioButton)
            Me.FlowLayoutPanel2.Controls.Add(Me.ListRadioButton)
            Me.FlowLayoutPanel2.Controls.Add(Me.QueryRadioButton)
            Me.FlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FlowLayoutPanel2.Location = New System.Drawing.Point(120, 0)
            Me.FlowLayoutPanel2.Margin = New System.Windows.Forms.Padding(0)
            Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
            Me.FlowLayoutPanel2.Size = New System.Drawing.Size(341, 27)
            Me.FlowLayoutPanel2.TabIndex = 18
            '
            'ChainRadioButton
            '
            Me.ChainRadioButton.AutoSize = True
            Me.ChainRadioButton.Checked = True
            Me.ChainRadioButton.Location = New System.Drawing.Point(3, 3)
            Me.ChainRadioButton.Name = "ChainRadioButton"
            Me.ChainRadioButton.Size = New System.Drawing.Size(52, 17)
            Me.ChainRadioButton.TabIndex = 0
            Me.ChainRadioButton.TabStop = True
            Me.ChainRadioButton.Text = "Chain"
            Me.ChainRadioButton.UseVisualStyleBackColor = True
            '
            'ListRadioButton
            '
            Me.ListRadioButton.AutoSize = True
            Me.ListRadioButton.Location = New System.Drawing.Point(61, 3)
            Me.ListRadioButton.Name = "ListRadioButton"
            Me.ListRadioButton.Size = New System.Drawing.Size(41, 17)
            Me.ListRadioButton.TabIndex = 1
            Me.ListRadioButton.TabStop = True
            Me.ListRadioButton.Text = "List"
            Me.ListRadioButton.UseVisualStyleBackColor = True
            '
            'Label8
            '
            Me.Label8.AutoSize = True
            Me.Label8.Location = New System.Drawing.Point(3, 192)
            Me.Label8.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
            Me.Label8.Name = "Label8"
            Me.Label8.Size = New System.Drawing.Size(51, 13)
            Me.Label8.TabIndex = 19
            Me.Label8.Text = "Condition"
            '
            'FlowLayoutPanel3
            '
            Me.FlowLayoutPanel3.Controls.Add(Me.ConditionTextBox)
            Me.FlowLayoutPanel3.Controls.Add(Me.EditConditionButton)
            Me.FlowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FlowLayoutPanel3.Location = New System.Drawing.Point(120, 189)
            Me.FlowLayoutPanel3.Margin = New System.Windows.Forms.Padding(0)
            Me.FlowLayoutPanel3.Name = "FlowLayoutPanel3"
            Me.FlowLayoutPanel3.Size = New System.Drawing.Size(341, 27)
            Me.FlowLayoutPanel3.TabIndex = 20
            '
            'ConditionTextBox
            '
            Me.ConditionTextBox.Location = New System.Drawing.Point(3, 3)
            Me.ConditionTextBox.Name = "ConditionTextBox"
            Me.ConditionTextBox.ReadOnly = True
            Me.ConditionTextBox.Size = New System.Drawing.Size(207, 20)
            Me.ConditionTextBox.TabIndex = 0
            '
            'EditConditionButton
            '
            Me.EditConditionButton.Location = New System.Drawing.Point(216, 3)
            Me.EditConditionButton.Name = "EditConditionButton"
            Me.EditConditionButton.Size = New System.Drawing.Size(122, 20)
            Me.EditConditionButton.TabIndex = 1
            Me.EditConditionButton.Text = "Edit..."
            Me.EditConditionButton.UseVisualStyleBackColor = True
            '
            'QueryRadioButton
            '
            Me.QueryRadioButton.AutoSize = True
            Me.QueryRadioButton.Location = New System.Drawing.Point(108, 3)
            Me.QueryRadioButton.Name = "QueryRadioButton"
            Me.QueryRadioButton.Size = New System.Drawing.Size(53, 17)
            Me.QueryRadioButton.TabIndex = 2
            Me.QueryRadioButton.TabStop = True
            Me.QueryRadioButton.Text = "Query"
            Me.QueryRadioButton.UseVisualStyleBackColor = True
            '
            'ChainRicTextBox
            '
            Me.ChainRicTextBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ChainRicTextBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
            Me.ChainRicTextBox.FormattingEnabled = True
            Me.ChainRicTextBox.Location = New System.Drawing.Point(123, 165)
            Me.ChainRicTextBox.Name = "ChainRicTextBox"
            Me.ChainRicTextBox.Size = New System.Drawing.Size(335, 21)
            Me.ChainRicTextBox.TabIndex = 21
            '
            'AddEditChainList
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(461, 273)
            Me.Controls.Add(Me.TableLayoutPanel1)
            Me.MinimumSize = New System.Drawing.Size(469, 229)
            Me.Name = "AddEditChainList"
            Me.Text = "AddEditChainList"
            Me.TableLayoutPanel1.ResumeLayout(False)
            Me.TableLayoutPanel1.PerformLayout()
            Me.FlowLayoutPanel1.ResumeLayout(False)
            CType(Me.ColorBox, System.ComponentModel.ISupportInitialize).EndInit()
            Me.FlowLayoutPanel2.ResumeLayout(False)
            Me.FlowLayoutPanel2.PerformLayout()
            Me.FlowLayoutPanel3.ResumeLayout(False)
            Me.FlowLayoutPanel3.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents CancelButton As System.Windows.Forms.Button
        Friend WithEvents OkButton As System.Windows.Forms.Button
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents NameTextBox As System.Windows.Forms.TextBox
        Friend WithEvents FieldLayoutComboBox As System.Windows.Forms.ComboBox
        Friend WithEvents CurveCheckBox As System.Windows.Forms.CheckBox
        Friend WithEvents EnabledCheckBox As System.Windows.Forms.CheckBox
        Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents ColorComboBox As System.Windows.Forms.ComboBox
        Friend WithEvents RandomColorButton As System.Windows.Forms.Button
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents FlowLayoutPanel2 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents ChainRadioButton As System.Windows.Forms.RadioButton
        Friend WithEvents ListRadioButton As System.Windows.Forms.RadioButton
        Friend WithEvents ColorBox As System.Windows.Forms.PictureBox
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents FlowLayoutPanel3 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents ConditionTextBox As System.Windows.Forms.TextBox
        Friend WithEvents EditConditionButton As System.Windows.Forms.Button
        Friend WithEvents QueryRadioButton As System.Windows.Forms.RadioButton
        Friend WithEvents ChainRicTextBox As System.Windows.Forms.ComboBox
    End Class
End Namespace