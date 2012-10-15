Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class SettingsForm
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
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.LogTraceRadioButton = New System.Windows.Forms.RadioButton()
        Me.LogDebugRadioButton = New System.Windows.Forms.RadioButton()
        Me.LogInfoRadioButton = New System.Windows.Forms.RadioButton()
        Me.LogWarnRadioButton = New System.Windows.Forms.RadioButton()
        Me.LogErrRadioButton = New System.Windows.Forms.RadioButton()
        Me.LogNoneRadioButton = New System.Windows.Forms.RadioButton()
        Me.SaveSettingsButton = New System.Windows.Forms.Button()
        Me.TheCancelButton = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.MaxYieldTextBox = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.MaxDurTextBox = New System.Windows.Forms.TextBox()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.MinYieldTextBox = New System.Windows.Forms.TextBox()
            Me.MinDurTextBox = New System.Windows.Forms.TextBox()
            Me.GroupBox2.SuspendLayout()
            Me.SuspendLayout()
            '
            'GroupBox2
            '
            Me.GroupBox2.Controls.Add(Me.LogTraceRadioButton)
            Me.GroupBox2.Controls.Add(Me.LogDebugRadioButton)
            Me.GroupBox2.Controls.Add(Me.LogInfoRadioButton)
            Me.GroupBox2.Controls.Add(Me.LogWarnRadioButton)
            Me.GroupBox2.Controls.Add(Me.LogErrRadioButton)
            Me.GroupBox2.Controls.Add(Me.LogNoneRadioButton)
            Me.GroupBox2.Location = New System.Drawing.Point(13, 12)
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.Size = New System.Drawing.Size(155, 94)
            Me.GroupBox2.TabIndex = 0
            Me.GroupBox2.TabStop = False
            Me.GroupBox2.Text = "Logging level"
            '
            'LogTraceRadioButton
            '
            Me.LogTraceRadioButton.AutoSize = True
            Me.LogTraceRadioButton.Location = New System.Drawing.Point(94, 65)
            Me.LogTraceRadioButton.Name = "LogTraceRadioButton"
            Me.LogTraceRadioButton.Size = New System.Drawing.Size(53, 17)
            Me.LogTraceRadioButton.TabIndex = 0
            Me.LogTraceRadioButton.TabStop = True
            Me.LogTraceRadioButton.Text = "Trace"
            Me.LogTraceRadioButton.UseVisualStyleBackColor = True
            '
            'LogDebugRadioButton
            '
            Me.LogDebugRadioButton.AutoSize = True
            Me.LogDebugRadioButton.Location = New System.Drawing.Point(94, 42)
            Me.LogDebugRadioButton.Name = "LogDebugRadioButton"
            Me.LogDebugRadioButton.Size = New System.Drawing.Size(57, 17)
            Me.LogDebugRadioButton.TabIndex = 0
            Me.LogDebugRadioButton.TabStop = True
            Me.LogDebugRadioButton.Text = "Debug"
            Me.LogDebugRadioButton.UseVisualStyleBackColor = True
            '
            'LogInfoRadioButton
            '
            Me.LogInfoRadioButton.AutoSize = True
            Me.LogInfoRadioButton.Location = New System.Drawing.Point(94, 19)
            Me.LogInfoRadioButton.Name = "LogInfoRadioButton"
            Me.LogInfoRadioButton.Size = New System.Drawing.Size(43, 17)
            Me.LogInfoRadioButton.TabIndex = 0
            Me.LogInfoRadioButton.TabStop = True
            Me.LogInfoRadioButton.Text = "Info"
            Me.LogInfoRadioButton.UseVisualStyleBackColor = True
            '
            'LogWarnRadioButton
            '
            Me.LogWarnRadioButton.AutoSize = True
            Me.LogWarnRadioButton.Location = New System.Drawing.Point(6, 65)
            Me.LogWarnRadioButton.Name = "LogWarnRadioButton"
            Me.LogWarnRadioButton.Size = New System.Drawing.Size(51, 17)
            Me.LogWarnRadioButton.TabIndex = 0
            Me.LogWarnRadioButton.TabStop = True
            Me.LogWarnRadioButton.Text = "Warn"
            Me.LogWarnRadioButton.UseVisualStyleBackColor = True
            '
            'LogErrRadioButton
            '
            Me.LogErrRadioButton.AutoSize = True
            Me.LogErrRadioButton.Location = New System.Drawing.Point(6, 42)
            Me.LogErrRadioButton.Name = "LogErrRadioButton"
            Me.LogErrRadioButton.Size = New System.Drawing.Size(47, 17)
            Me.LogErrRadioButton.TabIndex = 0
            Me.LogErrRadioButton.TabStop = True
            Me.LogErrRadioButton.Text = "Error"
            Me.LogErrRadioButton.UseVisualStyleBackColor = True
            '
            'LogNoneRadioButton
            '
            Me.LogNoneRadioButton.AutoSize = True
            Me.LogNoneRadioButton.Location = New System.Drawing.Point(6, 19)
            Me.LogNoneRadioButton.Name = "LogNoneRadioButton"
            Me.LogNoneRadioButton.Size = New System.Drawing.Size(51, 17)
            Me.LogNoneRadioButton.TabIndex = 0
            Me.LogNoneRadioButton.TabStop = True
            Me.LogNoneRadioButton.Text = "None"
            Me.LogNoneRadioButton.UseVisualStyleBackColor = True
            '
            'SaveSettingsButton
            '
            Me.SaveSettingsButton.Location = New System.Drawing.Point(12, 118)
            Me.SaveSettingsButton.Name = "SaveSettingsButton"
            Me.SaveSettingsButton.Size = New System.Drawing.Size(155, 29)
            Me.SaveSettingsButton.TabIndex = 1
            Me.SaveSettingsButton.Text = "Save"
            Me.SaveSettingsButton.UseVisualStyleBackColor = True
            '
            'TheCancelButton
            '
            Me.TheCancelButton.Location = New System.Drawing.Point(12, 153)
            Me.TheCancelButton.Name = "TheCancelButton"
            Me.TheCancelButton.Size = New System.Drawing.Size(155, 29)
            Me.TheCancelButton.TabIndex = 1
            Me.TheCancelButton.Text = "Cancel"
            Me.TheCancelButton.UseVisualStyleBackColor = True
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(9, 420)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(75, 13)
            Me.Label1.TabIndex = 2
            Me.Label1.Text = "Maximum yield"
            '
            'MaxYieldTextBox
            '
            Me.MaxYieldTextBox.Location = New System.Drawing.Point(101, 417)
            Me.MaxYieldTextBox.Name = "MaxYieldTextBox"
            Me.MaxYieldTextBox.Size = New System.Drawing.Size(66, 20)
            Me.MaxYieldTextBox.TabIndex = 3
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(9, 488)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(92, 13)
            Me.Label2.TabIndex = 2
            Me.Label2.Text = "Maximum duration"
            '
            'MaxDurTextBox
            '
            Me.MaxDurTextBox.Location = New System.Drawing.Point(101, 485)
            Me.MaxDurTextBox.Name = "MaxDurTextBox"
            Me.MaxDurTextBox.Size = New System.Drawing.Size(67, 20)
            Me.MaxDurTextBox.TabIndex = 3
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(9, 394)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(72, 13)
            Me.Label3.TabIndex = 2
            Me.Label3.Text = "Minimum yield"
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(9, 462)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(89, 13)
            Me.Label4.TabIndex = 2
            Me.Label4.Text = "Minimum duration"
            '
            'MinYieldTextBox
            '
            Me.MinYieldTextBox.Location = New System.Drawing.Point(101, 391)
            Me.MinYieldTextBox.Name = "MinYieldTextBox"
            Me.MinYieldTextBox.Size = New System.Drawing.Size(66, 20)
            Me.MinYieldTextBox.TabIndex = 3
            '
            'MinDurTextBox
            '
            Me.MinDurTextBox.Location = New System.Drawing.Point(101, 459)
            Me.MinDurTextBox.Name = "MinDurTextBox"
            Me.MinDurTextBox.Size = New System.Drawing.Size(66, 20)
            Me.MinDurTextBox.TabIndex = 3
            '
            'SettingsForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(186, 191)
            Me.Controls.Add(Me.MinDurTextBox)
            Me.Controls.Add(Me.MaxDurTextBox)
            Me.Controls.Add(Me.MinYieldTextBox)
            Me.Controls.Add(Me.MaxYieldTextBox)
            Me.Controls.Add(Me.Label4)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.Label3)
            Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TheCancelButton)
        Me.Controls.Add(Me.SaveSettingsButton)
        Me.Controls.Add(Me.GroupBox2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "SettingsForm"
        Me.Text = "Settings"
        Me.GroupBox2.ResumeLayout(false)
        Me.GroupBox2.PerformLayout
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
        Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
        Friend WithEvents LogTraceRadioButton As System.Windows.Forms.RadioButton
        Friend WithEvents LogDebugRadioButton As System.Windows.Forms.RadioButton
        Friend WithEvents LogWarnRadioButton As System.Windows.Forms.RadioButton
        Friend WithEvents LogErrRadioButton As System.Windows.Forms.RadioButton
        Friend WithEvents LogNoneRadioButton As System.Windows.Forms.RadioButton
        Friend WithEvents SaveSettingsButton As System.Windows.Forms.Button
        Friend WithEvents TheCancelButton As System.Windows.Forms.Button
        Friend WithEvents LogInfoRadioButton As System.Windows.Forms.RadioButton
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents MaxYieldTextBox As System.Windows.Forms.TextBox
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents MaxDurTextBox As System.Windows.Forms.TextBox
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents MinYieldTextBox As System.Windows.Forms.TextBox
        Friend WithEvents MinDurTextBox As System.Windows.Forms.TextBox
    End Class
End NameSpace