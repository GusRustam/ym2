
Namespace Forms.MainForm
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
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
            Me.MainToolStrip = New System.Windows.Forms.ToolStrip()
            Me.ConnectButton = New System.Windows.Forms.ToolStripButton()
            Me.YieldMapButton = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
            Me.DatabaseButton = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
            Me.SettingsButton = New System.Windows.Forms.ToolStripButton()
            Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
            Me.TileHorTSB = New System.Windows.Forms.ToolStripButton()
            Me.TileVerTSB = New System.Windows.Forms.ToolStripButton()
            Me.AboutTSMI = New System.Windows.Forms.ToolStripButton()
            Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
            Me.StatusPicture = New System.Windows.Forms.ToolStripStatusLabel()
            Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
            Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
            Me.InitEventLabel = New System.Windows.Forms.ToolStripStatusLabel()
            Me.CMS = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.LogSettingsTSMI = New System.Windows.Forms.ToolStripMenuItem()
            Me.ShowLogTSMI = New System.Windows.Forms.ToolStripMenuItem()
            Me.RaiseExcTSMI = New System.Windows.Forms.ToolStripMenuItem()
            Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
            Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ConnectTSMI = New System.Windows.Forms.ToolStripMenuItem()
            Me.NewChartTSMI = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
            Me.ExitTSMI = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.DatabaseManagerTSMI = New System.Windows.Forms.ToolStripMenuItem()
            Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolbarTSMI = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
            Me.TileWindowsHorizontallyTSMI = New System.Windows.Forms.ToolStripMenuItem()
            Me.TileVerticallyTSMI = New System.Windows.Forms.ToolStripMenuItem()
            Me.CascadeTSMI = New System.Windows.Forms.ToolStripMenuItem()
            Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
            Me.AboutMenuTSMI = New System.Windows.Forms.ToolStripMenuItem()
            Me.MainToolStrip.SuspendLayout()
            Me.StatusStrip1.SuspendLayout()
            Me.CMS.SuspendLayout()
            Me.MenuStrip1.SuspendLayout()
            Me.SuspendLayout()
            '
            'MainToolStrip
            '
            Me.MainToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConnectButton, Me.YieldMapButton, Me.ToolStripSeparator1, Me.DatabaseButton, Me.ToolStripSeparator2, Me.SettingsButton, Me.ToolStripSeparator3, Me.TileHorTSB, Me.TileVerTSB, Me.AboutTSMI})
            Me.MainToolStrip.Location = New System.Drawing.Point(0, 24)
            Me.MainToolStrip.Name = "MainToolStrip"
            Me.MainToolStrip.Size = New System.Drawing.Size(792, 39)
            Me.MainToolStrip.Stretch = True
            Me.MainToolStrip.TabIndex = 1
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
            Me.ToolStripSeparator2.Visible = False
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
            Me.SettingsButton.Visible = False
            '
            'ToolStripSeparator3
            '
            Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
            Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 39)
            '
            'TileHorTSB
            '
            Me.TileHorTSB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.TileHorTSB.Image = Global.YieldMap.My.Resources.Resources.TileHor
            Me.TileHorTSB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
            Me.TileHorTSB.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.TileHorTSB.Name = "TileHorTSB"
            Me.TileHorTSB.Size = New System.Drawing.Size(36, 36)
            Me.TileHorTSB.Text = "Tile horizontally"
            '
            'TileVerTSB
            '
            Me.TileVerTSB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.TileVerTSB.Image = Global.YieldMap.My.Resources.Resources.TileVer
            Me.TileVerTSB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
            Me.TileVerTSB.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.TileVerTSB.Name = "TileVerTSB"
            Me.TileVerTSB.Size = New System.Drawing.Size(36, 36)
            Me.TileVerTSB.Text = "ToolStripButton2"
            Me.TileVerTSB.ToolTipText = "Tile vertically"
            '
            'AboutTSMI
            '
            Me.AboutTSMI.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
            Me.AboutTSMI.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.AboutTSMI.Image = Global.YieldMap.My.Resources.Resources.about
            Me.AboutTSMI.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
            Me.AboutTSMI.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.AboutTSMI.Name = "AboutTSMI"
            Me.AboutTSMI.Size = New System.Drawing.Size(36, 36)
            Me.AboutTSMI.Text = "About Yield Map"
            '
            'StatusStrip1
            '
            Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusPicture, Me.StatusLabel, Me.ToolStripStatusLabel1, Me.InitEventLabel})
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
            'ToolStripStatusLabel1
            '
            Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
            Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(28, 17)
            Me.ToolStripStatusLabel1.Text = "       "
            '
            'InitEventLabel
            '
            Me.InitEventLabel.Name = "InitEventLabel"
            Me.InitEventLabel.Size = New System.Drawing.Size(0, 17)
            '
            'CMS
            '
            Me.CMS.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LogSettingsTSMI, Me.ShowLogTSMI, Me.RaiseExcTSMI})
            Me.CMS.Name = "CMS"
            Me.CMS.Size = New System.Drawing.Size(207, 70)
            '
            'LogSettingsTSMI
            '
            Me.LogSettingsTSMI.Name = "LogSettingsTSMI"
            Me.LogSettingsTSMI.Size = New System.Drawing.Size(206, 22)
            Me.LogSettingsTSMI.Text = "Logging settings"
            '
            'ShowLogTSMI
            '
            Me.ShowLogTSMI.Name = "ShowLogTSMI"
            Me.ShowLogTSMI.Size = New System.Drawing.Size(206, 22)
            Me.ShowLogTSMI.Text = "Show log"
            '
            'RaiseExcTSMI
            '
            Me.RaiseExcTSMI.Name = "RaiseExcTSMI"
            Me.RaiseExcTSMI.Size = New System.Drawing.Size(206, 22)
            Me.RaiseExcTSMI.Text = "Send report to developer"
            '
            'MenuStrip1
            '
            Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ToolsToolStripMenuItem, Me.ViewToolStripMenuItem, Me.HelpToolStripMenuItem})
            Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
            Me.MenuStrip1.Name = "MenuStrip1"
            Me.MenuStrip1.Size = New System.Drawing.Size(792, 24)
            Me.MenuStrip1.TabIndex = 5
            Me.MenuStrip1.Text = "MenuStrip1"
            '
            'FileToolStripMenuItem
            '
            Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConnectTSMI, Me.NewChartTSMI, Me.ToolStripMenuItem1, Me.ExitTSMI})
            Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
            Me.FileToolStripMenuItem.Size = New System.Drawing.Size(35, 20)
            Me.FileToolStripMenuItem.Text = "File"
            '
            'ConnectTSMI
            '
            Me.ConnectTSMI.Name = "ConnectTSMI"
            Me.ConnectTSMI.Size = New System.Drawing.Size(166, 22)
            Me.ConnectTSMI.Text = "Connect to Eikon"
            '
            'NewChartTSMI
            '
            Me.NewChartTSMI.Enabled = False
            Me.NewChartTSMI.Name = "NewChartTSMI"
            Me.NewChartTSMI.Size = New System.Drawing.Size(166, 22)
            Me.NewChartTSMI.Text = "New chart..."
            '
            'ToolStripMenuItem1
            '
            Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
            Me.ToolStripMenuItem1.Size = New System.Drawing.Size(163, 6)
            '
            'ExitTSMI
            '
            Me.ExitTSMI.Name = "ExitTSMI"
            Me.ExitTSMI.Size = New System.Drawing.Size(166, 22)
            Me.ExitTSMI.Text = "Exit"
            '
            'ToolsToolStripMenuItem
            '
            Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DatabaseManagerTSMI, Me.SettingsToolStripMenuItem})
            Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
            Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
            Me.ToolsToolStripMenuItem.Text = "Tools"
            '
            'DatabaseManagerTSMI
            '
            Me.DatabaseManagerTSMI.Name = "DatabaseManagerTSMI"
            Me.DatabaseManagerTSMI.Size = New System.Drawing.Size(188, 22)
            Me.DatabaseManagerTSMI.Text = "Database manager..."
            '
            'SettingsToolStripMenuItem
            '
            Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
            Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
            Me.SettingsToolStripMenuItem.Text = "Settings..."
            '
            'ViewToolStripMenuItem
            '
            Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolbarTSMI, Me.ToolStripSeparator4, Me.TileWindowsHorizontallyTSMI, Me.TileVerticallyTSMI, Me.CascadeTSMI})
            Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
            Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(41, 20)
            Me.ViewToolStripMenuItem.Text = "View"
            '
            'ToolbarTSMI
            '
            Me.ToolbarTSMI.Checked = True
            Me.ToolbarTSMI.CheckOnClick = True
            Me.ToolbarTSMI.CheckState = System.Windows.Forms.CheckState.Checked
            Me.ToolbarTSMI.Name = "ToolbarTSMI"
            Me.ToolbarTSMI.Size = New System.Drawing.Size(159, 22)
            Me.ToolbarTSMI.Text = "Toolbar"
            '
            'ToolStripSeparator4
            '
            Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
            Me.ToolStripSeparator4.Size = New System.Drawing.Size(156, 6)
            '
            'TileWindowsHorizontallyTSMI
            '
            Me.TileWindowsHorizontallyTSMI.Name = "TileWindowsHorizontallyTSMI"
            Me.TileWindowsHorizontallyTSMI.Size = New System.Drawing.Size(159, 22)
            Me.TileWindowsHorizontallyTSMI.Text = "Tile horizontally"
            '
            'TileVerticallyTSMI
            '
            Me.TileVerticallyTSMI.Name = "TileVerticallyTSMI"
            Me.TileVerticallyTSMI.Size = New System.Drawing.Size(159, 22)
            Me.TileVerticallyTSMI.Text = "Tile vertically"
            '
            'CascadeTSMI
            '
            Me.CascadeTSMI.Name = "CascadeTSMI"
            Me.CascadeTSMI.Size = New System.Drawing.Size(159, 22)
            Me.CascadeTSMI.Text = "Cascade"
            '
            'HelpToolStripMenuItem
            '
            Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutMenuTSMI})
            Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
            Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(40, 20)
            Me.HelpToolStripMenuItem.Text = "Help"
            '
            'AboutMenuTSMI
            '
            Me.AboutMenuTSMI.Name = "AboutMenuTSMI"
            Me.AboutMenuTSMI.Size = New System.Drawing.Size(152, 22)
            Me.AboutMenuTSMI.Text = "About..."
            '
            'MainForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(792, 573)
            Me.Controls.Add(Me.StatusStrip1)
            Me.Controls.Add(Me.MainToolStrip)
            Me.Controls.Add(Me.MenuStrip1)
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.IsMdiContainer = True
            Me.MainMenuStrip = Me.MenuStrip1
            Me.Name = "MainForm"
            Me.Text = "Thomson Reuters Yield Map"
            Me.MainToolStrip.ResumeLayout(False)
            Me.MainToolStrip.PerformLayout()
            Me.StatusStrip1.ResumeLayout(False)
            Me.StatusStrip1.PerformLayout()
            Me.CMS.ResumeLayout(False)
            Me.MenuStrip1.ResumeLayout(False)
            Me.MenuStrip1.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents MainToolStrip As System.Windows.Forms.ToolStrip
        Friend WithEvents ConnectButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents YieldMapButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents DatabaseButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents SettingsButton As System.Windows.Forms.ToolStripButton
        Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
        Friend WithEvents StatusPicture As System.Windows.Forms.ToolStripStatusLabel
        Friend WithEvents StatusLabel As System.Windows.Forms.ToolStripStatusLabel
        Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
        Friend WithEvents InitEventLabel As System.Windows.Forms.ToolStripStatusLabel
        Friend WithEvents CMS As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents LogSettingsTSMI As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents TileHorTSB As System.Windows.Forms.ToolStripButton
        Friend WithEvents TileVerTSB As System.Windows.Forms.ToolStripButton
        Friend WithEvents RaiseExcTSMI As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ShowLogTSMI As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AboutTSMI As System.Windows.Forms.ToolStripButton
        Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
        Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ConnectTSMI As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ExitTSMI As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents DatabaseManagerTSMI As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents SettingsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents AboutMenuTSMI As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ViewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolbarTSMI As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents NewChartTSMI As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents TileWindowsHorizontallyTSMI As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents TileVerticallyTSMI As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents CascadeTSMI As System.Windows.Forms.ToolStripMenuItem

    End Class
End Namespace