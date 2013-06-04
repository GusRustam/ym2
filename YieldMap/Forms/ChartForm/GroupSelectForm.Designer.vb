Namespace Forms.ChartForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class GroupSelectForm
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
            Me.ExistingGroupsListBox = New System.Windows.Forms.ListBox()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.NewGroupTextBox = New System.Windows.Forms.TextBox()
            Me.OkButton = New System.Windows.Forms.Button()
            Me.CancelButton = New System.Windows.Forms.Button()
            Me.ExistingGroupRadioButton = New System.Windows.Forms.RadioButton()
            Me.NewGroupRadioButton = New System.Windows.Forms.RadioButton()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.FieldsLayoutComboBox = New System.Windows.Forms.ComboBox()
            Me.ColorsComboBox = New System.Windows.Forms.ComboBox()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.RandomColorButton = New System.Windows.Forms.Button()
            Me.SuspendLayout()
            '
            'ExistingGroupsListBox
            '
            Me.ExistingGroupsListBox.FormattingEnabled = True
            Me.ExistingGroupsListBox.Location = New System.Drawing.Point(5, 25)
            Me.ExistingGroupsListBox.Name = "ExistingGroupsListBox"
            Me.ExistingGroupsListBox.Size = New System.Drawing.Size(249, 82)
            Me.ExistingGroupsListBox.TabIndex = 1
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
            Me.Label3.Location = New System.Drawing.Point(120, 110)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(25, 13)
            Me.Label3.TabIndex = 0
            Me.Label3.Text = "OR"
            '
            'NewGroupTextBox
            '
            Me.NewGroupTextBox.Enabled = False
            Me.NewGroupTextBox.Location = New System.Drawing.Point(5, 142)
            Me.NewGroupTextBox.Name = "NewGroupTextBox"
            Me.NewGroupTextBox.Size = New System.Drawing.Size(249, 20)
            Me.NewGroupTextBox.TabIndex = 2
            '
            'OkButton
            '
            Me.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.OkButton.Location = New System.Drawing.Point(5, 273)
            Me.OkButton.Name = "OkButton"
            Me.OkButton.Size = New System.Drawing.Size(75, 23)
            Me.OkButton.TabIndex = 3
            Me.OkButton.Text = "Ok"
            Me.OkButton.UseVisualStyleBackColor = True
            '
            'CancelButton
            '
            Me.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.CancelButton.Location = New System.Drawing.Point(179, 273)
            Me.CancelButton.Name = "CancelButton"
            Me.CancelButton.Size = New System.Drawing.Size(75, 23)
            Me.CancelButton.TabIndex = 3
            Me.CancelButton.Text = "Cancel"
            Me.CancelButton.UseVisualStyleBackColor = True
            '
            'ExistingGroupRadioButton
            '
            Me.ExistingGroupRadioButton.AutoSize = True
            Me.ExistingGroupRadioButton.Checked = True
            Me.ExistingGroupRadioButton.Location = New System.Drawing.Point(5, 2)
            Me.ExistingGroupRadioButton.Name = "ExistingGroupRadioButton"
            Me.ExistingGroupRadioButton.Size = New System.Drawing.Size(106, 17)
            Me.ExistingGroupRadioButton.TabIndex = 4
            Me.ExistingGroupRadioButton.TabStop = True
            Me.ExistingGroupRadioButton.Text = "To existing group"
            Me.ExistingGroupRadioButton.UseVisualStyleBackColor = True
            '
            'NewGroupRadioButton
            '
            Me.NewGroupRadioButton.AutoSize = True
            Me.NewGroupRadioButton.Location = New System.Drawing.Point(5, 119)
            Me.NewGroupRadioButton.Name = "NewGroupRadioButton"
            Me.NewGroupRadioButton.Size = New System.Drawing.Size(100, 17)
            Me.NewGroupRadioButton.TabIndex = 5
            Me.NewGroupRadioButton.Text = "To a new group"
            Me.NewGroupRadioButton.UseVisualStyleBackColor = True
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(2, 175)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(65, 13)
            Me.Label1.TabIndex = 6
            Me.Label1.Text = "Fields layout"
            '
            'FieldsLayoutComboBox
            '
            Me.FieldsLayoutComboBox.Enabled = False
            Me.FieldsLayoutComboBox.FormattingEnabled = True
            Me.FieldsLayoutComboBox.Location = New System.Drawing.Point(73, 172)
            Me.FieldsLayoutComboBox.Name = "FieldsLayoutComboBox"
            Me.FieldsLayoutComboBox.Size = New System.Drawing.Size(181, 21)
            Me.FieldsLayoutComboBox.TabIndex = 7
            '
            'ColorsComboBox
            '
            Me.ColorsComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
            Me.ColorsComboBox.Enabled = False
            Me.ColorsComboBox.FormattingEnabled = True
            Me.ColorsComboBox.Location = New System.Drawing.Point(73, 199)
            Me.ColorsComboBox.Name = "ColorsComboBox"
            Me.ColorsComboBox.Size = New System.Drawing.Size(181, 21)
            Me.ColorsComboBox.TabIndex = 8
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(2, 199)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(31, 13)
            Me.Label2.TabIndex = 6
            Me.Label2.Text = "Color"
            '
            'RandomColorButton
            '
            Me.RandomColorButton.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.RandomColorButton.Location = New System.Drawing.Point(179, 226)
            Me.RandomColorButton.Name = "RandomColorButton"
            Me.RandomColorButton.Size = New System.Drawing.Size(75, 23)
            Me.RandomColorButton.TabIndex = 3
            Me.RandomColorButton.Text = "Random"
            Me.RandomColorButton.UseVisualStyleBackColor = True
            '
            'GroupSelectForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(266, 308)
            Me.Controls.Add(Me.ColorsComboBox)
            Me.Controls.Add(Me.FieldsLayoutComboBox)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.NewGroupRadioButton)
            Me.Controls.Add(Me.ExistingGroupRadioButton)
            Me.Controls.Add(Me.CancelButton)
            Me.Controls.Add(Me.RandomColorButton)
            Me.Controls.Add(Me.OkButton)
            Me.Controls.Add(Me.NewGroupTextBox)
            Me.Controls.Add(Me.ExistingGroupsListBox)
            Me.Controls.Add(Me.Label3)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Name = "GroupSelectForm"
            Me.Text = "Adding bond to chart..."
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents ExistingGroupsListBox As System.Windows.Forms.ListBox
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents NewGroupTextBox As System.Windows.Forms.TextBox
        Friend WithEvents OkButton As System.Windows.Forms.Button
        Friend WithEvents CancelButton As System.Windows.Forms.Button
        Friend WithEvents ExistingGroupRadioButton As System.Windows.Forms.RadioButton
        Friend WithEvents NewGroupRadioButton As System.Windows.Forms.RadioButton
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents FieldsLayoutComboBox As System.Windows.Forms.ComboBox
        Friend WithEvents ColorsComboBox As System.Windows.Forms.ComboBox
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents RandomColorButton As System.Windows.Forms.Button
    End Class
End Namespace