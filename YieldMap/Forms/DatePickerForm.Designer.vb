Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class DatePickerForm
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
            Me.TheCalendar = New System.Windows.Forms.MonthCalendar()
            Me.OkButton = New System.Windows.Forms.Button()
            Me.CancelButton = New System.Windows.Forms.Button()
            Me.SuspendLayout()
            '
            'TheCalendar
            '
            Me.TheCalendar.CalendarDimensions = New System.Drawing.Size(3, 1)
            Me.TheCalendar.Location = New System.Drawing.Point(12, 9)
            Me.TheCalendar.MaxDate = New Date(2012, 9, 6, 0, 0, 0, 0)
            Me.TheCalendar.MaxSelectionCount = 1
            Me.TheCalendar.Name = "TheCalendar"
            Me.TheCalendar.TabIndex = 0
            '
            'OkButton
            '
            Me.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.OkButton.Location = New System.Drawing.Point(12, 187)
            Me.OkButton.Name = "OkButton"
            Me.OkButton.Size = New System.Drawing.Size(75, 23)
            Me.OkButton.TabIndex = 1
            Me.OkButton.Text = "Ok"
            Me.OkButton.UseVisualStyleBackColor = True
            '
            'CancelButton
            '
            Me.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.CancelButton.Location = New System.Drawing.Point(416, 187)
            Me.CancelButton.Name = "CancelButton"
            Me.CancelButton.Size = New System.Drawing.Size(75, 23)
            Me.CancelButton.TabIndex = 2
            Me.CancelButton.Text = "Cancel"
            Me.CancelButton.UseVisualStyleBackColor = True
            '
            'DatePickerForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(507, 220)
            Me.Controls.Add(Me.CancelButton)
            Me.Controls.Add(Me.OkButton)
            Me.Controls.Add(Me.TheCalendar)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Name = "DatePickerForm"
            Me.Text = "Pick a date"
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents TheCalendar As System.Windows.Forms.MonthCalendar
        Friend WithEvents OkButton As System.Windows.Forms.Button
        Friend WithEvents CancelButton As System.Windows.Forms.Button
    End Class
End Namespace