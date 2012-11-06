Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class UnhandledExcForm
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
            Me.MainPanel = New System.Windows.Forms.TableLayoutPanel()
            Me.ErrorTextBox = New System.Windows.Forms.TextBox()
            Me.CloseButton = New System.Windows.Forms.Button()
            Me.SendErrorReportButton = New System.Windows.Forms.Button()
            Me.MainPanel.SuspendLayout()
            Me.SuspendLayout()
            '
            'MainPanel
            '
            Me.MainPanel.ColumnCount = 2
            Me.MainPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.MainPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.MainPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.MainPanel.Controls.Add(Me.ErrorTextBox, 0, 0)
            Me.MainPanel.Controls.Add(Me.CloseButton, 1, 1)
            Me.MainPanel.Controls.Add(Me.SendErrorReportButton, 0, 1)
            Me.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill
            Me.MainPanel.Location = New System.Drawing.Point(0, 0)
            Me.MainPanel.Name = "MainPanel"
            Me.MainPanel.RowCount = 2
            Me.MainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.MainPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            Me.MainPanel.Size = New System.Drawing.Size(692, 485)
            Me.MainPanel.TabIndex = 0
            '
            'ErrorTextBox
            '
            Me.MainPanel.SetColumnSpan(Me.ErrorTextBox, 2)
            Me.ErrorTextBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ErrorTextBox.Location = New System.Drawing.Point(3, 3)
            Me.ErrorTextBox.Multiline = True
            Me.ErrorTextBox.Name = "ErrorTextBox"
            Me.ErrorTextBox.Size = New System.Drawing.Size(686, 449)
            Me.ErrorTextBox.TabIndex = 2
            '
            'CloseButton
            '
            Me.CloseButton.Dock = System.Windows.Forms.DockStyle.Right
            Me.CloseButton.Location = New System.Drawing.Point(614, 458)
            Me.CloseButton.Name = "CloseButton"
            Me.CloseButton.Size = New System.Drawing.Size(75, 24)
            Me.CloseButton.TabIndex = 5
            Me.CloseButton.Text = "Close"
            Me.CloseButton.UseVisualStyleBackColor = True
            '
            'SendErrorReportButton
            '
            Me.SendErrorReportButton.Location = New System.Drawing.Point(3, 458)
            Me.SendErrorReportButton.Name = "SendErrorReportButton"
            Me.SendErrorReportButton.Size = New System.Drawing.Size(176, 23)
            Me.SendErrorReportButton.TabIndex = 6
            Me.SendErrorReportButton.Text = "Send error report to developer"
            Me.SendErrorReportButton.UseVisualStyleBackColor = True
            '
            'UnhandledExcForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(692, 485)
            Me.Controls.Add(Me.MainPanel)
            Me.Name = "UnhandledExcForm"
            Me.Text = "Unhandled exception"
            Me.MainPanel.ResumeLayout(False)
            Me.MainPanel.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents MainPanel As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents ErrorTextBox As System.Windows.Forms.TextBox
        Friend WithEvents CloseButton As System.Windows.Forms.Button
        Friend WithEvents SendErrorReportButton As System.Windows.Forms.Button
    End Class
End Namespace