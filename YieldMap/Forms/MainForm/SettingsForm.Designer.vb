Namespace Forms.MainForm
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
            Me.MainTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
            Me.SaveSettingsButton = New System.Windows.Forms.Button()
            Me.MainTabControl = New System.Windows.Forms.TabControl()
            Me.MainChartPage = New System.Windows.Forms.TabPage()
            Me.ViewportPanel = New System.Windows.Forms.TableLayoutPanel()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.UseVWAPRadioButton = New System.Windows.Forms.RadioButton()
            Me.UseLastRadioButton = New System.Windows.Forms.RadioButton()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.ShowPointSizeCheckBox = New System.Windows.Forms.CheckBox()
            Me.ShowBidAskCheckBox = New System.Windows.Forms.CheckBox()
            Me.Panel1 = New System.Windows.Forms.Panel()
            Me.MaxYieldTextBox = New System.Windows.Forms.MaskedTextBox()
            Me.MinYieldTextBox = New System.Windows.Forms.MaskedTextBox()
            Me.Label9 = New System.Windows.Forms.Label()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.Panel2 = New System.Windows.Forms.Panel()
            Me.MaxSpreadTextBox = New System.Windows.Forms.MaskedTextBox()
            Me.MinSpreadTextBox = New System.Windows.Forms.MaskedTextBox()
            Me.Label10 = New System.Windows.Forms.Label()
            Me.Label11 = New System.Windows.Forms.Label()
            Me.Label6 = New System.Windows.Forms.Label()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.Panel3 = New System.Windows.Forms.Panel()
            Me.MaxDurTextBox = New System.Windows.Forms.MaskedTextBox()
            Me.MinDurTextBox = New System.Windows.Forms.MaskedTextBox()
            Me.Label12 = New System.Windows.Forms.Label()
            Me.Label13 = New System.Windows.Forms.Label()
            Me.MainGeneralTabPage = New System.Windows.Forms.TabPage()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.ChartWindowCheckBox = New System.Windows.Forms.CheckBox()
            Me.MainWindowCheckBox = New System.Windows.Forms.CheckBox()
            Me.MainLogPage = New System.Windows.Forms.TabPage()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.LogTraceRadioButton = New System.Windows.Forms.RadioButton()
            Me.LogDebugRadioButton = New System.Windows.Forms.RadioButton()
            Me.LogInfoRadioButton = New System.Windows.Forms.RadioButton()
            Me.LogWarnRadioButton = New System.Windows.Forms.RadioButton()
            Me.LogFatalRadioButton = New System.Windows.Forms.RadioButton()
            Me.LogErrRadioButton = New System.Windows.Forms.RadioButton()
            Me.LogNoneRadioButton = New System.Windows.Forms.RadioButton()
            Me.TheCancelButton = New System.Windows.Forms.Button()
            Me.MainTableLayoutPanel.SuspendLayout()
            Me.MainTabControl.SuspendLayout()
            Me.MainChartPage.SuspendLayout()
            Me.ViewportPanel.SuspendLayout()
            Me.Panel1.SuspendLayout()
            Me.Panel2.SuspendLayout()
            Me.Panel3.SuspendLayout()
            Me.MainGeneralTabPage.SuspendLayout()
            Me.MainLogPage.SuspendLayout()
            Me.SuspendLayout()
            '
            'MainTableLayoutPanel
            '
            Me.MainTableLayoutPanel.ColumnCount = 2
            Me.MainTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.MainTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.MainTableLayoutPanel.Controls.Add(Me.SaveSettingsButton, 0, 1)
            Me.MainTableLayoutPanel.Controls.Add(Me.MainTabControl, 0, 0)
            Me.MainTableLayoutPanel.Controls.Add(Me.TheCancelButton, 1, 1)
            Me.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
            Me.MainTableLayoutPanel.Location = New System.Drawing.Point(0, 0)
            Me.MainTableLayoutPanel.Name = "MainTableLayoutPanel"
            Me.MainTableLayoutPanel.RowCount = 2
            Me.MainTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.MainTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            Me.MainTableLayoutPanel.Size = New System.Drawing.Size(602, 273)
            Me.MainTableLayoutPanel.TabIndex = 5
            '
            'SaveSettingsButton
            '
            Me.SaveSettingsButton.Location = New System.Drawing.Point(3, 246)
            Me.SaveSettingsButton.Name = "SaveSettingsButton"
            Me.SaveSettingsButton.Size = New System.Drawing.Size(155, 24)
            Me.SaveSettingsButton.TabIndex = 6
            Me.SaveSettingsButton.Text = "Save"
            Me.SaveSettingsButton.UseVisualStyleBackColor = True
            '
            'MainTabControl
            '
            Me.MainTableLayoutPanel.SetColumnSpan(Me.MainTabControl, 2)
            Me.MainTabControl.Controls.Add(Me.MainChartPage)
            Me.MainTabControl.Controls.Add(Me.MainGeneralTabPage)
            Me.MainTabControl.Controls.Add(Me.MainLogPage)
            Me.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill
            Me.MainTabControl.Location = New System.Drawing.Point(3, 3)
            Me.MainTabControl.Name = "MainTabControl"
            Me.MainTabControl.SelectedIndex = 0
            Me.MainTabControl.Size = New System.Drawing.Size(596, 237)
            Me.MainTabControl.TabIndex = 5
            '
            'MainChartPage
            '
            Me.MainChartPage.Controls.Add(Me.ViewportPanel)
            Me.MainChartPage.Location = New System.Drawing.Point(4, 22)
            Me.MainChartPage.Name = "MainChartPage"
            Me.MainChartPage.Padding = New System.Windows.Forms.Padding(3)
            Me.MainChartPage.Size = New System.Drawing.Size(588, 211)
            Me.MainChartPage.TabIndex = 2
            Me.MainChartPage.Text = "Chart"
            Me.MainChartPage.UseVisualStyleBackColor = True
            '
            'ViewportPanel
            '
            Me.ViewportPanel.ColumnCount = 3
            Me.ViewportPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            Me.ViewportPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            Me.ViewportPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            Me.ViewportPanel.Controls.Add(Me.Label5, 0, 4)
            Me.ViewportPanel.Controls.Add(Me.Label4, 0, 3)
            Me.ViewportPanel.Controls.Add(Me.UseVWAPRadioButton, 2, 2)
            Me.ViewportPanel.Controls.Add(Me.UseLastRadioButton, 1, 2)
            Me.ViewportPanel.Controls.Add(Me.Label3, 0, 2)
            Me.ViewportPanel.Controls.Add(Me.ShowPointSizeCheckBox, 0, 1)
            Me.ViewportPanel.Controls.Add(Me.ShowBidAskCheckBox, 0, 0)
            Me.ViewportPanel.Controls.Add(Me.Panel1, 0, 5)
            Me.ViewportPanel.Controls.Add(Me.Panel2, 1, 5)
            Me.ViewportPanel.Controls.Add(Me.Label6, 1, 4)
            Me.ViewportPanel.Controls.Add(Me.Label7, 2, 4)
            Me.ViewportPanel.Controls.Add(Me.Panel3, 2, 5)
            Me.ViewportPanel.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ViewportPanel.Location = New System.Drawing.Point(3, 3)
            Me.ViewportPanel.Name = "ViewportPanel"
            Me.ViewportPanel.RowCount = 6
            Me.ViewportPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.ViewportPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.ViewportPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.ViewportPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.ViewportPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.ViewportPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.ViewportPanel.Size = New System.Drawing.Size(582, 205)
            Me.ViewportPanel.TabIndex = 4
            '
            'Label5
            '
            Me.Label5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.Label5.AutoSize = True
            Me.Label5.Location = New System.Drawing.Point(6, 86)
            Me.Label5.Margin = New System.Windows.Forms.Padding(6, 6, 0, 0)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(71, 14)
            Me.Label5.TabIndex = 10
            Me.Label5.Text = "By yield (in %)"
            '
            'Label4
            '
            Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(6, 66)
            Me.Label4.Margin = New System.Windows.Forms.Padding(6, 6, 0, 0)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(135, 14)
            Me.Label4.TabIndex = 7
            Me.Label4.Text = "Chart viewport default limits"
            '
            'UseVWAPRadioButton
            '
            Me.UseVWAPRadioButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.UseVWAPRadioButton.AutoSize = True
            Me.UseVWAPRadioButton.Location = New System.Drawing.Point(388, 40)
            Me.UseVWAPRadioButton.Margin = New System.Windows.Forms.Padding(0)
            Me.UseVWAPRadioButton.Name = "UseVWAPRadioButton"
            Me.UseVWAPRadioButton.Size = New System.Drawing.Size(83, 20)
            Me.UseVWAPRadioButton.TabIndex = 5
            Me.UseVWAPRadioButton.TabStop = True
            Me.UseVWAPRadioButton.Text = "VWAP price"
            Me.UseVWAPRadioButton.UseVisualStyleBackColor = True
            '
            'UseLastRadioButton
            '
            Me.UseLastRadioButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.UseLastRadioButton.AutoSize = True
            Me.UseLastRadioButton.Location = New System.Drawing.Point(194, 40)
            Me.UseLastRadioButton.Margin = New System.Windows.Forms.Padding(0)
            Me.UseLastRadioButton.Name = "UseLastRadioButton"
            Me.UseLastRadioButton.Size = New System.Drawing.Size(98, 20)
            Me.UseLastRadioButton.TabIndex = 4
            Me.UseLastRadioButton.TabStop = True
            Me.UseLastRadioButton.Text = "Last trade price"
            Me.UseLastRadioButton.UseVisualStyleBackColor = True
            '
            'Label3
            '
            Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(0, 40)
            Me.Label3.Margin = New System.Windows.Forms.Padding(0)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(113, 20)
            Me.Label3.TabIndex = 3
            Me.Label3.Text = "Field to use by default:"
            '
            'ShowPointSizeCheckBox
            '
            Me.ShowPointSizeCheckBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.ShowPointSizeCheckBox.AutoSize = True
            Me.ViewportPanel.SetColumnSpan(Me.ShowPointSizeCheckBox, 2)
            Me.ShowPointSizeCheckBox.Location = New System.Drawing.Point(0, 20)
            Me.ShowPointSizeCheckBox.Margin = New System.Windows.Forms.Padding(0)
            Me.ShowPointSizeCheckBox.Name = "ShowPointSizeCheckBox"
            Me.ShowPointSizeCheckBox.Size = New System.Drawing.Size(257, 20)
            Me.ShowPointSizeCheckBox.TabIndex = 2
            Me.ShowPointSizeCheckBox.Text = "Set point size depending on today's trade volume"
            Me.ShowPointSizeCheckBox.UseVisualStyleBackColor = True
            '
            'ShowBidAskCheckBox
            '
            Me.ShowBidAskCheckBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.ShowBidAskCheckBox.AutoSize = True
            Me.ViewportPanel.SetColumnSpan(Me.ShowBidAskCheckBox, 2)
            Me.ShowBidAskCheckBox.Location = New System.Drawing.Point(0, 0)
            Me.ShowBidAskCheckBox.Margin = New System.Windows.Forms.Padding(0)
            Me.ShowBidAskCheckBox.Name = "ShowBidAskCheckBox"
            Me.ShowBidAskCheckBox.Size = New System.Drawing.Size(192, 20)
            Me.ShowBidAskCheckBox.TabIndex = 1
            Me.ShowBidAskCheckBox.Text = "Show bid-ask yields on mouse over"
            Me.ShowBidAskCheckBox.UseVisualStyleBackColor = True
            '
            'Panel1
            '
            Me.Panel1.Controls.Add(Me.MaxYieldTextBox)
            Me.Panel1.Controls.Add(Me.MinYieldTextBox)
            Me.Panel1.Controls.Add(Me.Label9)
            Me.Panel1.Controls.Add(Me.Label8)
            Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.Panel1.Location = New System.Drawing.Point(3, 103)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(188, 99)
            Me.Panel1.TabIndex = 8
            '
            'MaxYieldTextBox
            '
            Me.MaxYieldTextBox.Location = New System.Drawing.Point(45, 53)
            Me.MaxYieldTextBox.Mask = "00 %"
            Me.MaxYieldTextBox.Name = "MaxYieldTextBox"
            Me.MaxYieldTextBox.Size = New System.Drawing.Size(77, 20)
            Me.MaxYieldTextBox.TabIndex = 1
            '
            'MinYieldTextBox
            '
            Me.MinYieldTextBox.Location = New System.Drawing.Point(45, 23)
            Me.MinYieldTextBox.Mask = "00 %"
            Me.MinYieldTextBox.Name = "MinYieldTextBox"
            Me.MinYieldTextBox.Size = New System.Drawing.Size(77, 20)
            Me.MinYieldTextBox.TabIndex = 1
            '
            'Label9
            '
            Me.Label9.AutoSize = True
            Me.Label9.Location = New System.Drawing.Point(9, 56)
            Me.Label9.Name = "Label9"
            Me.Label9.Size = New System.Drawing.Size(20, 13)
            Me.Label9.TabIndex = 0
            Me.Label9.Text = "To"
            '
            'Label8
            '
            Me.Label8.AutoSize = True
            Me.Label8.Location = New System.Drawing.Point(9, 26)
            Me.Label8.Name = "Label8"
            Me.Label8.Size = New System.Drawing.Size(30, 13)
            Me.Label8.TabIndex = 0
            Me.Label8.Text = "From"
            '
            'Panel2
            '
            Me.Panel2.Controls.Add(Me.MaxSpreadTextBox)
            Me.Panel2.Controls.Add(Me.MinSpreadTextBox)
            Me.Panel2.Controls.Add(Me.Label10)
            Me.Panel2.Controls.Add(Me.Label11)
            Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.Panel2.Location = New System.Drawing.Point(197, 103)
            Me.Panel2.Name = "Panel2"
            Me.Panel2.Size = New System.Drawing.Size(188, 99)
            Me.Panel2.TabIndex = 9
            '
            'MaxSpreadTextBox
            '
            Me.MaxSpreadTextBox.Location = New System.Drawing.Point(55, 53)
            Me.MaxSpreadTextBox.Mask = "#0000 b.p."
            Me.MaxSpreadTextBox.Name = "MaxSpreadTextBox"
            Me.MaxSpreadTextBox.Size = New System.Drawing.Size(77, 20)
            Me.MaxSpreadTextBox.TabIndex = 1
            '
            'MinSpreadTextBox
            '
            Me.MinSpreadTextBox.Location = New System.Drawing.Point(55, 23)
            Me.MinSpreadTextBox.Mask = "#0000 b.p."
            Me.MinSpreadTextBox.Name = "MinSpreadTextBox"
            Me.MinSpreadTextBox.Size = New System.Drawing.Size(77, 20)
            Me.MinSpreadTextBox.TabIndex = 1
            '
            'Label10
            '
            Me.Label10.AutoSize = True
            Me.Label10.Location = New System.Drawing.Point(19, 26)
            Me.Label10.Name = "Label10"
            Me.Label10.Size = New System.Drawing.Size(30, 13)
            Me.Label10.TabIndex = 0
            Me.Label10.Text = "From"
            '
            'Label11
            '
            Me.Label11.AutoSize = True
            Me.Label11.Location = New System.Drawing.Point(19, 56)
            Me.Label11.Name = "Label11"
            Me.Label11.Size = New System.Drawing.Size(20, 13)
            Me.Label11.TabIndex = 0
            Me.Label11.Text = "To"
            '
            'Label6
            '
            Me.Label6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.Label6.AutoSize = True
            Me.Label6.Location = New System.Drawing.Point(200, 86)
            Me.Label6.Margin = New System.Windows.Forms.Padding(6, 6, 0, 0)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New System.Drawing.Size(129, 14)
            Me.Label6.TabIndex = 10
            Me.Label6.Text = "By spread (in basis points)"
            '
            'Label7
            '
            Me.Label7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.Label7.AutoSize = True
            Me.Label7.Location = New System.Drawing.Point(394, 86)
            Me.Label7.Margin = New System.Windows.Forms.Padding(6, 6, 0, 0)
            Me.Label7.Name = "Label7"
            Me.Label7.Size = New System.Drawing.Size(105, 14)
            Me.Label7.TabIndex = 10
            Me.Label7.Text = "By duration (in years)"
            '
            'Panel3
            '
            Me.Panel3.Controls.Add(Me.MaxDurTextBox)
            Me.Panel3.Controls.Add(Me.MinDurTextBox)
            Me.Panel3.Controls.Add(Me.Label12)
            Me.Panel3.Controls.Add(Me.Label13)
            Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
            Me.Panel3.Location = New System.Drawing.Point(391, 103)
            Me.Panel3.Name = "Panel3"
            Me.Panel3.Size = New System.Drawing.Size(188, 99)
            Me.Panel3.TabIndex = 11
            '
            'MaxDurTextBox
            '
            Me.MaxDurTextBox.Location = New System.Drawing.Point(53, 53)
            Me.MaxDurTextBox.Mask = "00 yrs"
            Me.MaxDurTextBox.Name = "MaxDurTextBox"
            Me.MaxDurTextBox.Size = New System.Drawing.Size(77, 20)
            Me.MaxDurTextBox.TabIndex = 1
            '
            'MinDurTextBox
            '
            Me.MinDurTextBox.Location = New System.Drawing.Point(53, 23)
            Me.MinDurTextBox.Mask = "00 yrs"
            Me.MinDurTextBox.Name = "MinDurTextBox"
            Me.MinDurTextBox.Size = New System.Drawing.Size(77, 20)
            Me.MinDurTextBox.TabIndex = 1
            '
            'Label12
            '
            Me.Label12.AutoSize = True
            Me.Label12.Location = New System.Drawing.Point(17, 56)
            Me.Label12.Name = "Label12"
            Me.Label12.Size = New System.Drawing.Size(20, 13)
            Me.Label12.TabIndex = 0
            Me.Label12.Text = "To"
            '
            'Label13
            '
            Me.Label13.AutoSize = True
            Me.Label13.Location = New System.Drawing.Point(17, 26)
            Me.Label13.Name = "Label13"
            Me.Label13.Size = New System.Drawing.Size(30, 13)
            Me.Label13.TabIndex = 0
            Me.Label13.Text = "From"
            '
            'MainGeneralTabPage
            '
            Me.MainGeneralTabPage.Controls.Add(Me.Label2)
            Me.MainGeneralTabPage.Controls.Add(Me.ChartWindowCheckBox)
            Me.MainGeneralTabPage.Controls.Add(Me.MainWindowCheckBox)
            Me.MainGeneralTabPage.Location = New System.Drawing.Point(4, 22)
            Me.MainGeneralTabPage.Name = "MainGeneralTabPage"
            Me.MainGeneralTabPage.Padding = New System.Windows.Forms.Padding(3)
            Me.MainGeneralTabPage.Size = New System.Drawing.Size(588, 211)
            Me.MainGeneralTabPage.TabIndex = 1
            Me.MainGeneralTabPage.Text = "General"
            Me.MainGeneralTabPage.UseVisualStyleBackColor = True
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(7, 7)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(125, 13)
            Me.Label2.TabIndex = 1
            Me.Label2.Text = "Show toolstrip by default:"
            '
            'ChartWindowCheckBox
            '
            Me.ChartWindowCheckBox.AutoSize = True
            Me.ChartWindowCheckBox.Location = New System.Drawing.Point(12, 52)
            Me.ChartWindowCheckBox.Name = "ChartWindowCheckBox"
            Me.ChartWindowCheckBox.Size = New System.Drawing.Size(90, 17)
            Me.ChartWindowCheckBox.TabIndex = 0
            Me.ChartWindowCheckBox.Text = "Chart window"
            Me.ChartWindowCheckBox.UseVisualStyleBackColor = True
            '
            'MainWindowCheckBox
            '
            Me.MainWindowCheckBox.AutoSize = True
            Me.MainWindowCheckBox.Location = New System.Drawing.Point(12, 29)
            Me.MainWindowCheckBox.Name = "MainWindowCheckBox"
            Me.MainWindowCheckBox.Size = New System.Drawing.Size(88, 17)
            Me.MainWindowCheckBox.TabIndex = 0
            Me.MainWindowCheckBox.Text = "Main window"
            Me.MainWindowCheckBox.UseVisualStyleBackColor = True
            '
            'MainLogPage
            '
            Me.MainLogPage.Controls.Add(Me.Label1)
            Me.MainLogPage.Controls.Add(Me.LogTraceRadioButton)
            Me.MainLogPage.Controls.Add(Me.LogDebugRadioButton)
            Me.MainLogPage.Controls.Add(Me.LogInfoRadioButton)
            Me.MainLogPage.Controls.Add(Me.LogWarnRadioButton)
            Me.MainLogPage.Controls.Add(Me.LogFatalRadioButton)
            Me.MainLogPage.Controls.Add(Me.LogErrRadioButton)
            Me.MainLogPage.Controls.Add(Me.LogNoneRadioButton)
            Me.MainLogPage.Location = New System.Drawing.Point(4, 22)
            Me.MainLogPage.Name = "MainLogPage"
            Me.MainLogPage.Padding = New System.Windows.Forms.Padding(3)
            Me.MainLogPage.Size = New System.Drawing.Size(588, 211)
            Me.MainLogPage.TabIndex = 0
            Me.MainLogPage.Text = "Logging"
            Me.MainLogPage.UseVisualStyleBackColor = True
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(6, 13)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(419, 13)
            Me.Label1.TabIndex = 7
            Me.Label1.Text = "Choose granularity of logging (the finer the granularity, the slower the applicat" & _
        "ion works):"
            '
            'LogTraceRadioButton
            '
            Me.LogTraceRadioButton.AutoSize = True
            Me.LogTraceRadioButton.Location = New System.Drawing.Point(9, 37)
            Me.LogTraceRadioButton.Name = "LogTraceRadioButton"
            Me.LogTraceRadioButton.Size = New System.Drawing.Size(139, 17)
            Me.LogTraceRadioButton.TabIndex = 4
            Me.LogTraceRadioButton.Text = "Trace (very detailed log)"
            Me.LogTraceRadioButton.UseVisualStyleBackColor = True
            '
            'LogDebugRadioButton
            '
            Me.LogDebugRadioButton.AutoSize = True
            Me.LogDebugRadioButton.Location = New System.Drawing.Point(9, 60)
            Me.LogDebugRadioButton.Name = "LogDebugRadioButton"
            Me.LogDebugRadioButton.Size = New System.Drawing.Size(312, 17)
            Me.LogDebugRadioButton.TabIndex = 5
            Me.LogDebugRadioButton.Text = "Debug (minimum necessary to find the reason of the problem)"
            Me.LogDebugRadioButton.UseVisualStyleBackColor = True
            '
            'LogInfoRadioButton
            '
            Me.LogInfoRadioButton.AutoSize = True
            Me.LogInfoRadioButton.Location = New System.Drawing.Point(9, 83)
            Me.LogInfoRadioButton.Name = "LogInfoRadioButton"
            Me.LogInfoRadioButton.Size = New System.Drawing.Size(199, 17)
            Me.LogInfoRadioButton.TabIndex = 6
            Me.LogInfoRadioButton.Text = "Info (stores info only on main actions)"
            Me.LogInfoRadioButton.UseVisualStyleBackColor = True
            '
            'LogWarnRadioButton
            '
            Me.LogWarnRadioButton.AutoSize = True
            Me.LogWarnRadioButton.Checked = True
            Me.LogWarnRadioButton.Location = New System.Drawing.Point(9, 106)
            Me.LogWarnRadioButton.Name = "LogWarnRadioButton"
            Me.LogWarnRadioButton.Size = New System.Drawing.Size(281, 17)
            Me.LogWarnRadioButton.TabIndex = 1
            Me.LogWarnRadioButton.TabStop = True
            Me.LogWarnRadioButton.Text = "Warn (serious problems and minor unexpected events)"
            Me.LogWarnRadioButton.UseVisualStyleBackColor = True
            '
            'LogFatalRadioButton
            '
            Me.LogFatalRadioButton.AutoSize = True
            Me.LogFatalRadioButton.Location = New System.Drawing.Point(9, 152)
            Me.LogFatalRadioButton.Name = "LogFatalRadioButton"
            Me.LogFatalRadioButton.Size = New System.Drawing.Size(217, 17)
            Me.LogFatalRadioButton.TabIndex = 2
            Me.LogFatalRadioButton.Text = "Fatal (only appliction crashes are logged)"
            Me.LogFatalRadioButton.UseVisualStyleBackColor = True
            '
            'LogErrRadioButton
            '
            Me.LogErrRadioButton.AutoSize = True
            Me.LogErrRadioButton.Location = New System.Drawing.Point(9, 129)
            Me.LogErrRadioButton.Name = "LogErrRadioButton"
            Me.LogErrRadioButton.Size = New System.Drawing.Size(209, 17)
            Me.LogErrRadioButton.TabIndex = 2
            Me.LogErrRadioButton.Text = "Error (only serious problems are logged)"
            Me.LogErrRadioButton.UseVisualStyleBackColor = True
            '
            'LogNoneRadioButton
            '
            Me.LogNoneRadioButton.AutoSize = True
            Me.LogNoneRadioButton.Location = New System.Drawing.Point(9, 175)
            Me.LogNoneRadioButton.Name = "LogNoneRadioButton"
            Me.LogNoneRadioButton.Size = New System.Drawing.Size(230, 17)
            Me.LogNoneRadioButton.TabIndex = 3
            Me.LogNoneRadioButton.Text = "None (highest performance, no log created)"
            Me.LogNoneRadioButton.UseVisualStyleBackColor = True
            '
            'TheCancelButton
            '
            Me.TheCancelButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.TheCancelButton.Location = New System.Drawing.Point(444, 246)
            Me.TheCancelButton.Name = "TheCancelButton"
            Me.TheCancelButton.Size = New System.Drawing.Size(155, 24)
            Me.TheCancelButton.TabIndex = 7
            Me.TheCancelButton.Text = "Cancel"
            Me.TheCancelButton.UseVisualStyleBackColor = True
            '
            'SettingsForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(602, 273)
            Me.Controls.Add(Me.MainTableLayoutPanel)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Name = "SettingsForm"
            Me.Text = "Settings"
            Me.MainTableLayoutPanel.ResumeLayout(False)
            Me.MainTabControl.ResumeLayout(False)
            Me.MainChartPage.ResumeLayout(False)
            Me.ViewportPanel.ResumeLayout(False)
            Me.ViewportPanel.PerformLayout()
            Me.Panel1.ResumeLayout(False)
            Me.Panel1.PerformLayout()
            Me.Panel2.ResumeLayout(False)
            Me.Panel2.PerformLayout()
            Me.Panel3.ResumeLayout(False)
            Me.Panel3.PerformLayout()
            Me.MainGeneralTabPage.ResumeLayout(False)
            Me.MainGeneralTabPage.PerformLayout()
            Me.MainLogPage.ResumeLayout(False)
            Me.MainLogPage.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents MainTableLayoutPanel As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents SaveSettingsButton As System.Windows.Forms.Button
        Friend WithEvents MainTabControl As System.Windows.Forms.TabControl
        Friend WithEvents MainGeneralTabPage As System.Windows.Forms.TabPage
        Friend WithEvents MainLogPage As System.Windows.Forms.TabPage
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents LogTraceRadioButton As System.Windows.Forms.RadioButton
        Friend WithEvents LogDebugRadioButton As System.Windows.Forms.RadioButton
        Friend WithEvents LogInfoRadioButton As System.Windows.Forms.RadioButton
        Friend WithEvents LogWarnRadioButton As System.Windows.Forms.RadioButton
        Friend WithEvents LogFatalRadioButton As System.Windows.Forms.RadioButton
        Friend WithEvents LogErrRadioButton As System.Windows.Forms.RadioButton
        Friend WithEvents LogNoneRadioButton As System.Windows.Forms.RadioButton
        Friend WithEvents TheCancelButton As System.Windows.Forms.Button
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents ChartWindowCheckBox As System.Windows.Forms.CheckBox
        Friend WithEvents MainWindowCheckBox As System.Windows.Forms.CheckBox
        Friend WithEvents MainChartPage As System.Windows.Forms.TabPage
        Friend WithEvents ViewportPanel As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents UseVWAPRadioButton As System.Windows.Forms.RadioButton
        Friend WithEvents UseLastRadioButton As System.Windows.Forms.RadioButton
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents ShowPointSizeCheckBox As System.Windows.Forms.CheckBox
        Friend WithEvents ShowBidAskCheckBox As System.Windows.Forms.CheckBox
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents MaxYieldTextBox As System.Windows.Forms.MaskedTextBox
        Friend WithEvents MinYieldTextBox As System.Windows.Forms.MaskedTextBox
        Friend WithEvents Label9 As System.Windows.Forms.Label
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents MaxSpreadTextBox As System.Windows.Forms.MaskedTextBox
        Friend WithEvents MinSpreadTextBox As System.Windows.Forms.MaskedTextBox
        Friend WithEvents Label10 As System.Windows.Forms.Label
        Friend WithEvents Label11 As System.Windows.Forms.Label
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents Panel3 As System.Windows.Forms.Panel
        Friend WithEvents MaxDurTextBox As System.Windows.Forms.MaskedTextBox
        Friend WithEvents MinDurTextBox As System.Windows.Forms.MaskedTextBox
        Friend WithEvents Label12 As System.Windows.Forms.Label
        Friend WithEvents Label13 As System.Windows.Forms.Label
    End Class
End Namespace