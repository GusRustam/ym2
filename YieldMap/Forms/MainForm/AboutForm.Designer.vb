Namespace Forms.MainForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class AboutForm
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
            Me.PictureBox1 = New System.Windows.Forms.PictureBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.BuildVerLabel = New System.Windows.Forms.Label()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.OsVerLabel = New System.Windows.Forms.Label()
            Me.SendReportButton = New System.Windows.Forms.Button()
            Me.CloseButton = New System.Windows.Forms.Button()
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'PictureBox1
            '
            Me.PictureBox1.Image = Global.YieldMap.My.Resources.Resources.logo
            Me.PictureBox1.InitialImage = Global.YieldMap.My.Resources.Resources.logo
            Me.PictureBox1.Location = New System.Drawing.Point(15, 107)
            Me.PictureBox1.Name = "PictureBox1"
            Me.PictureBox1.Size = New System.Drawing.Size(532, 186)
            Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.PictureBox1.TabIndex = 0
            Me.PictureBox1.TabStop = False
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
            Me.Label1.Location = New System.Drawing.Point(12, 9)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(235, 20)
            Me.Label1.TabIndex = 1
            Me.Label1.Text = "Thomson Reuters Yield Map"
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(12, 33)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(70, 13)
            Me.Label2.TabIndex = 2
            Me.Label2.Text = "Build version:"
            '
            'BuildVerLabel
            '
            Me.BuildVerLabel.AutoSize = True
            Me.BuildVerLabel.Location = New System.Drawing.Point(88, 33)
            Me.BuildVerLabel.Name = "BuildVerLabel"
            Me.BuildVerLabel.Size = New System.Drawing.Size(108, 13)
            Me.BuildVerLabel.TabIndex = 3
            Me.BuildVerLabel.Text = "=== build version ==="
            '
            'Label4
            '
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(14, 314)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(182, 13)
            Me.Label4.TabIndex = 2
            Me.Label4.Text = "Copyright by Thomson Reuters, 2012"
            '
            'Label5
            '
            Me.Label5.AutoSize = True
            Me.Label5.Location = New System.Drawing.Point(13, 46)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(63, 13)
            Me.Label5.TabIndex = 2
            Me.Label5.Text = "OS Version:"
            '
            'OsVerLabel
            '
            Me.OsVerLabel.AutoSize = True
            Me.OsVerLabel.Location = New System.Drawing.Point(88, 46)
            Me.OsVerLabel.Name = "OsVerLabel"
            Me.OsVerLabel.Size = New System.Drawing.Size(98, 13)
            Me.OsVerLabel.TabIndex = 3
            Me.OsVerLabel.Text = "===OS version ==="
            '
            'SendReportButton
            '
            Me.SendReportButton.Location = New System.Drawing.Point(407, 12)
            Me.SendReportButton.Name = "SendReportButton"
            Me.SendReportButton.Size = New System.Drawing.Size(137, 23)
            Me.SendReportButton.TabIndex = 4
            Me.SendReportButton.Text = "Send report to developer"
            Me.SendReportButton.UseVisualStyleBackColor = True
            '
            'CloseButton
            '
            Me.CloseButton.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.CloseButton.Location = New System.Drawing.Point(407, 304)
            Me.CloseButton.Name = "CloseButton"
            Me.CloseButton.Size = New System.Drawing.Size(137, 23)
            Me.CloseButton.TabIndex = 1
            Me.CloseButton.Text = "Close"
            Me.CloseButton.UseVisualStyleBackColor = True
            '
            'AboutForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(556, 339)
            Me.Controls.Add(Me.SendReportButton)
            Me.Controls.Add(Me.CloseButton)
            Me.Controls.Add(Me.OsVerLabel)
            Me.Controls.Add(Me.BuildVerLabel)
            Me.Controls.Add(Me.Label4)
            Me.Controls.Add(Me.Label5)
            Me.Controls.Add(Me.Label2)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.PictureBox1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Name = "AboutForm"
            Me.Text = "About Yield Map"
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents BuildVerLabel As System.Windows.Forms.Label
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents OsVerLabel As System.Windows.Forms.Label
        Friend WithEvents SendReportButton As System.Windows.Forms.Button
        Friend WithEvents CloseButton As System.Windows.Forms.Button
    End Class
End Namespace