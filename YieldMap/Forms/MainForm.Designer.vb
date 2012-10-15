
Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class MainForm
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
            Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
            Me.ConnectButton = New System.Windows.Forms.ToolStripButton()
            Me.YieldMapButton = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
            Me.DatabaseButton = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
            Me.SettingsButton = New System.Windows.Forms.ToolStripButton()
            Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
            Me.StatusPicture = New System.Windows.Forms.ToolStripStatusLabel()
            Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
            Me.ToolStrip1.SuspendLayout()
            Me.StatusStrip1.SuspendLayout()
            Me.SuspendLayout()
            '
            'ToolStrip1
            '
            Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConnectButton, Me.YieldMapButton, Me.ToolStripSeparator1, Me.DatabaseButton, Me.ToolStripSeparator2, Me.SettingsButton})
            Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
            Me.ToolStrip1.Name = "ToolStrip1"
            Me.ToolStrip1.Size = New System.Drawing.Size(792, 39)
            Me.ToolStrip1.Stretch = True
            Me.ToolStrip1.TabIndex = 1
            Me.ToolStrip1.Text = "ToolStrip1"
            '
            'ConnectButton
            '
            Me.ConnectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.ConnectButton.Image = Global.YieldMap.My.Resources.Resources.PlugLarge
            Me.ConnectButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
            Me.ConnectButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.ConnectButton.Name = "ConnectButton"
            Me.ConnectButton.Size = New System.Drawing.Size(36, 36)
            Me.ConnectButton.Text = "ConnectButton"
            Me.ConnectButton.ToolTipText = "Connect to Eikon"
            '
            'YieldMapButton
            '
            Me.YieldMapButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.YieldMapButton.Enabled = False
            Me.YieldMapButton.Image = Global.YieldMap.My.Resources.Resources.ChartBubbleLarge
            Me.YieldMapButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
            Me.YieldMapButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.YieldMapButton.Name = "YieldMapButton"
            Me.YieldMapButton.Size = New System.Drawing.Size(36, 36)
            Me.YieldMapButton.Text = "ToolStripButton2"
            Me.YieldMapButton.ToolTipText = "Yield map"
            '
            'ToolStripSeparator1
            '
            Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
            Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 39)
            '
            'DatabaseButton
            '
            Me.DatabaseButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.DatabaseButton.Image = Global.YieldMap.My.Resources.Resources.DatabaseLarge
            Me.DatabaseButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
            Me.DatabaseButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.DatabaseButton.Name = "DatabaseButton"
            Me.DatabaseButton.Size = New System.Drawing.Size(36, 36)
            Me.DatabaseButton.Text = "DatabaseButton"
            Me.DatabaseButton.ToolTipText = "Database manager"
            '
            'ToolStripSeparator2
            '
            Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
            Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 39)
            '
            'SettingsButton
            '
            Me.SettingsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.SettingsButton.Image = Global.YieldMap.My.Resources.Resources.SettingsLarge
            Me.SettingsButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
            Me.SettingsButton.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.SettingsButton.Name = "SettingsButton"
            Me.SettingsButton.Size = New System.Drawing.Size(36, 36)
            Me.SettingsButton.Text = "SettingsButton"
            Me.SettingsButton.ToolTipText = "Settings"
            '
            'StatusStrip1
            '
            Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusPicture, Me.StatusLabel})
            Me.StatusStrip1.Location = New System.Drawing.Point(0, 551)
            Me.StatusStrip1.Name = "StatusStrip1"
            Me.StatusStrip1.Size = New System.Drawing.Size(792, 22)
            Me.StatusStrip1.TabIndex = 3
            Me.StatusStrip1.Text = "StatusStrip1"
            '
            'StatusPicture
            '
            Me.StatusPicture.Image = Global.YieldMap.My.Resources.Resources.Red
            Me.StatusPicture.Name = "StatusPicture"
            Me.StatusPicture.Size = New System.Drawing.Size(16, 17)
            '
            'StatusLabel
            '
            Me.StatusLabel.Name = "StatusLabel"
            Me.StatusLabel.Size = New System.Drawing.Size(0, 17)
            '
            'MainForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(792, 573)
            Me.Controls.Add(Me.StatusStrip1)
            Me.Controls.Add(Me.ToolStrip1)
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.IsMdiContainer = True
            Me.Name = "MainForm"
            Me.Text = "Thomson Reuters Yield Map"
            Me.ToolStrip1.ResumeLayout(False)
            Me.ToolStrip1.PerformLayout()
            Me.StatusStrip1.ResumeLayout(False)
            Me.StatusStrip1.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
        Friend WithEvents ConnectButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents YieldMapButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents DatabaseButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents SettingsButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
        Friend WithEvents StatusPicture As System.Windows.Forms.ToolStripStatusLabel
        Friend WithEvents StatusLabel As System.Windows.Forms.ToolStripStatusLabel

    End Class
End Namespace