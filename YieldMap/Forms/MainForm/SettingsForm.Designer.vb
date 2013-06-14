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
            Me.components = New System.ComponentModel.Container()
            Dim Label11 As System.Windows.Forms.Label
            Dim Label4 As System.Windows.Forms.Label
            Dim Label15 As System.Windows.Forms.Label
            Dim Label7 As System.Windows.Forms.Label
            Dim Label5 As System.Windows.Forms.Label
            Dim Label6 As System.Windows.Forms.Label
            Dim Label17 As System.Windows.Forms.Label
            Dim Label16 As System.Windows.Forms.Label
            Dim Label13 As System.Windows.Forms.Label
            Dim Label18 As System.Windows.Forms.Label
            Dim Label19 As System.Windows.Forms.Label
            Me.MainTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
            Me.SaveSettingsButton = New System.Windows.Forms.Button()
            Me.MainTabControl = New System.Windows.Forms.TabControl()
            Me.MainChartPage = New System.Windows.Forms.TabPage()
            Me.ClearOtherCurvesCheckBox = New System.Windows.Forms.CheckBox()
            Me.ClearBondCurvesCheckBox = New System.Windows.Forms.CheckBox()
            Me.ClearPointsCheckBox = New System.Windows.Forms.CheckBox()
            Me.YieldCalcModeCB = New System.Windows.Forms.ComboBox()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.ShowPointSizeCheckBox = New System.Windows.Forms.CheckBox()
            Me.MidIfBothCB = New System.Windows.Forms.CheckBox()
            Me.ShowBidAskCheckBox = New System.Windows.Forms.CheckBox()
            Me.ViewportTP = New System.Windows.Forms.TabPage()
            Me.Panel2 = New System.Windows.Forms.Panel()
            Me.MaxYStrictCB = New System.Windows.Forms.CheckBox()
            Me.MinYStrictCB = New System.Windows.Forms.CheckBox()
            Me.MaxYieldTextBox = New System.Windows.Forms.MaskedTextBox()
            Me.MinYieldTextBox = New System.Windows.Forms.MaskedTextBox()
            Me.MinSpreadTextBox = New System.Windows.Forms.MaskedTextBox()
            Me.MaxSpreadTextBox = New System.Windows.Forms.MaskedTextBox()
            Me.Panel1 = New System.Windows.Forms.Panel()
            Me.MaxXStrictCB = New System.Windows.Forms.CheckBox()
            Me.MinXStrictCB = New System.Windows.Forms.CheckBox()
            Me.MaxDurTextBox = New System.Windows.Forms.MaskedTextBox()
            Me.MinDurTextBox = New System.Windows.Forms.MaskedTextBox()
            Me.FieldPriorityTabPage = New System.Windows.Forms.TabPage()
            Me.DownButton = New System.Windows.Forms.Button()
            Me.MoveToHiddenButton = New System.Windows.Forms.Button()
            Me.MoveToShownButton = New System.Windows.Forms.Button()
            Me.UpButton = New System.Windows.Forms.Button()
            Me.HiddenFieldsListBox = New System.Windows.Forms.ListBox()
            Me.FieldsPriorityLB = New System.Windows.Forms.ListBox()
            Me.Label9 = New System.Windows.Forms.Label()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.MainGeneralTabPage = New System.Windows.Forms.TabPage()
            Me.LoadRicsButton = New System.Windows.Forms.Button()
            Me.LoadRicsCB = New System.Windows.Forms.CheckBox()
            Me.Label10 = New System.Windows.Forms.Label()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.ChartWindowCheckBox = New System.Windows.Forms.CheckBox()
            Me.MainWindowCheckBox = New System.Windows.Forms.CheckBox()
            Me.MainLoadColumnsPage = New System.Windows.Forms.TabPage()
            Me.Panel4 = New System.Windows.Forms.Panel()
            Me.FirstLevelRB = New System.Windows.Forms.RadioButton()
            Me.FirstDateRB = New System.Windows.Forms.RadioButton()
            Me.Label12 = New System.Windows.Forms.Label()
            Me.AllColumnsCB = New System.Windows.Forms.CheckBox()
            Me.Label14 = New System.Windows.Forms.Label()
            Me.ColumnsCLB = New System.Windows.Forms.CheckedListBox()
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
            Me.ErrProv = New System.Windows.Forms.ErrorProvider(Me.components)
            Me.NumInterpPointsTB = New System.Windows.Forms.TextBox()
            Label11 = New System.Windows.Forms.Label()
            Label4 = New System.Windows.Forms.Label()
            Label15 = New System.Windows.Forms.Label()
            Label7 = New System.Windows.Forms.Label()
            Label5 = New System.Windows.Forms.Label()
            Label6 = New System.Windows.Forms.Label()
            Label17 = New System.Windows.Forms.Label()
            Label16 = New System.Windows.Forms.Label()
            Label13 = New System.Windows.Forms.Label()
            Label18 = New System.Windows.Forms.Label()
            Label19 = New System.Windows.Forms.Label()
            Me.MainTableLayoutPanel.SuspendLayout()
            Me.MainTabControl.SuspendLayout()
            Me.MainChartPage.SuspendLayout()
            Me.ViewportTP.SuspendLayout()
            Me.Panel2.SuspendLayout()
            Me.Panel1.SuspendLayout()
            Me.FieldPriorityTabPage.SuspendLayout()
            Me.MainGeneralTabPage.SuspendLayout()
            Me.MainLoadColumnsPage.SuspendLayout()
            Me.Panel4.SuspendLayout()
            Me.MainLogPage.SuspendLayout()
            CType(Me.ErrProv, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'Label11
            '
            Label11.AutoSize = True
            Label11.Location = New System.Drawing.Point(7, 38)
            Label11.Name = "Label11"
            Label11.Size = New System.Drawing.Size(148, 13)
            Label11.TabIndex = 44
            Label11.Text = "When select a portfolio, clear:"
            '
            'Label4
            '
            Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Label4.AutoSize = True
            Label4.Location = New System.Drawing.Point(8, 12)
            Label4.Margin = New System.Windows.Forms.Padding(6, 6, 0, 0)
            Label4.Name = "Label4"
            Label4.Size = New System.Drawing.Size(135, 13)
            Label4.TabIndex = 41
            Label4.Text = "Chart viewport default limits"
            '
            'Label15
            '
            Label15.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Label15.AutoSize = True
            Label15.Location = New System.Drawing.Point(264, 12)
            Label15.Margin = New System.Windows.Forms.Padding(6, 6, 0, 0)
            Label15.Name = "Label15"
            Label15.Size = New System.Drawing.Size(32, 13)
            Label15.TabIndex = 53
            Label15.Text = "years"
            '
            'Label7
            '
            Label7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Label7.AutoSize = True
            Label7.Location = New System.Drawing.Point(12, 15)
            Label7.Margin = New System.Windows.Forms.Padding(6, 6, 0, 0)
            Label7.Name = "Label7"
            Label7.Size = New System.Drawing.Size(47, 13)
            Label7.TabIndex = 52
            Label7.Text = "Duration"
            '
            'Label5
            '
            Label5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Label5.AutoSize = True
            Label5.Location = New System.Drawing.Point(12, 41)
            Label5.Margin = New System.Windows.Forms.Padding(6, 6, 0, 0)
            Label5.Name = "Label5"
            Label5.Size = New System.Drawing.Size(30, 13)
            Label5.TabIndex = 55
            Label5.Text = "Yield"
            '
            'Label6
            '
            Label6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Label6.AutoSize = True
            Label6.Location = New System.Drawing.Point(12, 10)
            Label6.Margin = New System.Windows.Forms.Padding(6, 6, 0, 0)
            Label6.Name = "Label6"
            Label6.Size = New System.Drawing.Size(41, 13)
            Label6.TabIndex = 58
            Label6.Text = "Spread"
            '
            'Label17
            '
            Label17.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Label17.AutoSize = True
            Label17.Location = New System.Drawing.Point(264, 44)
            Label17.Margin = New System.Windows.Forms.Padding(6, 6, 0, 0)
            Label17.Name = "Label17"
            Label17.Size = New System.Drawing.Size(15, 13)
            Label17.TabIndex = 56
            Label17.Text = "%"
            '
            'Label16
            '
            Label16.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Label16.AutoSize = True
            Label16.Location = New System.Drawing.Point(264, 10)
            Label16.Margin = New System.Windows.Forms.Padding(6, 6, 0, 0)
            Label16.Name = "Label16"
            Label16.Size = New System.Drawing.Size(25, 13)
            Label16.TabIndex = 57
            Label16.Text = "b.p."
            '
            'Label13
            '
            Label13.AutoSize = True
            Label13.Location = New System.Drawing.Point(12, 74)
            Label13.Name = "Label13"
            Label13.Size = New System.Drawing.Size(68, 13)
            Label13.TabIndex = 59
            Label13.Text = "Set strict limit"
            '
            'Label18
            '
            Label18.AutoSize = True
            Label18.Location = New System.Drawing.Point(12, 44)
            Label18.Name = "Label18"
            Label18.Size = New System.Drawing.Size(68, 13)
            Label18.TabIndex = 61
            Label18.Text = "Set strict limit"
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
            Me.MainTableLayoutPanel.Size = New System.Drawing.Size(444, 272)
            Me.MainTableLayoutPanel.TabIndex = 5
            '
            'SaveSettingsButton
            '
            Me.SaveSettingsButton.Location = New System.Drawing.Point(3, 245)
            Me.SaveSettingsButton.Name = "SaveSettingsButton"
            Me.SaveSettingsButton.Size = New System.Drawing.Size(155, 24)
            Me.SaveSettingsButton.TabIndex = 6
            Me.SaveSettingsButton.Text = "Save and close"
            Me.SaveSettingsButton.UseVisualStyleBackColor = True
            '
            'MainTabControl
            '
            Me.MainTableLayoutPanel.SetColumnSpan(Me.MainTabControl, 2)
            Me.MainTabControl.Controls.Add(Me.MainChartPage)
            Me.MainTabControl.Controls.Add(Me.ViewportTP)
            Me.MainTabControl.Controls.Add(Me.FieldPriorityTabPage)
            Me.MainTabControl.Controls.Add(Me.MainGeneralTabPage)
            Me.MainTabControl.Controls.Add(Me.MainLoadColumnsPage)
            Me.MainTabControl.Controls.Add(Me.MainLogPage)
            Me.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill
            Me.MainTabControl.Location = New System.Drawing.Point(3, 3)
            Me.MainTabControl.Name = "MainTabControl"
            Me.MainTabControl.SelectedIndex = 0
            Me.MainTabControl.Size = New System.Drawing.Size(438, 236)
            Me.MainTabControl.TabIndex = 5
            '
            'MainChartPage
            '
            Me.MainChartPage.Controls.Add(Me.NumInterpPointsTB)
            Me.MainChartPage.Controls.Add(Label19)
            Me.MainChartPage.Controls.Add(Label11)
            Me.MainChartPage.Controls.Add(Me.ClearOtherCurvesCheckBox)
            Me.MainChartPage.Controls.Add(Me.ClearBondCurvesCheckBox)
            Me.MainChartPage.Controls.Add(Me.ClearPointsCheckBox)
            Me.MainChartPage.Controls.Add(Me.YieldCalcModeCB)
            Me.MainChartPage.Controls.Add(Me.Label8)
            Me.MainChartPage.Controls.Add(Me.ShowPointSizeCheckBox)
            Me.MainChartPage.Controls.Add(Me.MidIfBothCB)
            Me.MainChartPage.Controls.Add(Me.ShowBidAskCheckBox)
            Me.MainChartPage.Location = New System.Drawing.Point(4, 22)
            Me.MainChartPage.Name = "MainChartPage"
            Me.MainChartPage.Padding = New System.Windows.Forms.Padding(3)
            Me.MainChartPage.Size = New System.Drawing.Size(430, 210)
            Me.MainChartPage.TabIndex = 2
            Me.MainChartPage.Text = "Chart"
            Me.MainChartPage.UseVisualStyleBackColor = True
            '
            'ClearOtherCurvesCheckBox
            '
            Me.ClearOtherCurvesCheckBox.AutoSize = True
            Me.ClearOtherCurvesCheckBox.Location = New System.Drawing.Point(19, 123)
            Me.ClearOtherCurvesCheckBox.Name = "ClearOtherCurvesCheckBox"
            Me.ClearOtherCurvesCheckBox.Size = New System.Drawing.Size(87, 17)
            Me.ClearOtherCurvesCheckBox.TabIndex = 43
            Me.ClearOtherCurvesCheckBox.Text = "Other curves"
            Me.ClearOtherCurvesCheckBox.UseVisualStyleBackColor = True
            '
            'ClearBondCurvesCheckBox
            '
            Me.ClearBondCurvesCheckBox.AutoSize = True
            Me.ClearBondCurvesCheckBox.Location = New System.Drawing.Point(19, 92)
            Me.ClearBondCurvesCheckBox.Name = "ClearBondCurvesCheckBox"
            Me.ClearBondCurvesCheckBox.Size = New System.Drawing.Size(86, 17)
            Me.ClearBondCurvesCheckBox.TabIndex = 43
            Me.ClearBondCurvesCheckBox.Text = "Bond curves"
            Me.ClearBondCurvesCheckBox.UseVisualStyleBackColor = True
            '
            'ClearPointsCheckBox
            '
            Me.ClearPointsCheckBox.AutoSize = True
            Me.ClearPointsCheckBox.Location = New System.Drawing.Point(19, 63)
            Me.ClearPointsCheckBox.Name = "ClearPointsCheckBox"
            Me.ClearPointsCheckBox.Size = New System.Drawing.Size(101, 17)
            Me.ClearPointsCheckBox.TabIndex = 43
            Me.ClearPointsCheckBox.Text = "Separate bonds"
            Me.ClearPointsCheckBox.UseVisualStyleBackColor = True
            '
            'YieldCalcModeCB
            '
            Me.YieldCalcModeCB.FormattingEnabled = True
            Me.YieldCalcModeCB.Items.AddRange(New Object() {"YTM", "YTW", "YTB", "YTA"})
            Me.YieldCalcModeCB.Location = New System.Drawing.Point(330, 35)
            Me.YieldCalcModeCB.Name = "YieldCalcModeCB"
            Me.YieldCalcModeCB.Size = New System.Drawing.Size(73, 21)
            Me.YieldCalcModeCB.TabIndex = 42
            '
            'Label8
            '
            Me.Label8.AutoSize = True
            Me.Label8.Location = New System.Drawing.Point(211, 38)
            Me.Label8.Name = "Label8"
            Me.Label8.Size = New System.Drawing.Size(113, 13)
            Me.Label8.TabIndex = 41
            Me.Label8.Text = "Yield calculation mode"
            '
            'ShowPointSizeCheckBox
            '
            Me.ShowPointSizeCheckBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.ShowPointSizeCheckBox.AutoSize = True
            Me.ShowPointSizeCheckBox.Location = New System.Drawing.Point(214, 12)
            Me.ShowPointSizeCheckBox.Margin = New System.Windows.Forms.Padding(0)
            Me.ShowPointSizeCheckBox.Name = "ShowPointSizeCheckBox"
            Me.ShowPointSizeCheckBox.Size = New System.Drawing.Size(257, 17)
            Me.ShowPointSizeCheckBox.TabIndex = 6
            Me.ShowPointSizeCheckBox.Text = "Set point size depending on today's trade volume"
            Me.ShowPointSizeCheckBox.UseVisualStyleBackColor = True
            Me.ShowPointSizeCheckBox.Visible = False
            '
            'MidIfBothCB
            '
            Me.MidIfBothCB.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.MidIfBothCB.AutoSize = True
            Me.MidIfBothCB.Location = New System.Drawing.Point(3, 176)
            Me.MidIfBothCB.Margin = New System.Windows.Forms.Padding(0)
            Me.MidIfBothCB.Name = "MidIfBothCB"
            Me.MidIfBothCB.Size = New System.Drawing.Size(269, 17)
            Me.MidIfBothCB.TabIndex = 5
            Me.MidIfBothCB.Text = "Calculate MID only if both BID and ASK are present"
            Me.MidIfBothCB.UseVisualStyleBackColor = True
            '
            'ShowBidAskCheckBox
            '
            Me.ShowBidAskCheckBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.ShowBidAskCheckBox.AutoSize = True
            Me.ShowBidAskCheckBox.Location = New System.Drawing.Point(8, 12)
            Me.ShowBidAskCheckBox.Margin = New System.Windows.Forms.Padding(0)
            Me.ShowBidAskCheckBox.Name = "ShowBidAskCheckBox"
            Me.ShowBidAskCheckBox.Size = New System.Drawing.Size(192, 17)
            Me.ShowBidAskCheckBox.TabIndex = 5
            Me.ShowBidAskCheckBox.Text = "Show bid-ask yields on mouse over"
            Me.ShowBidAskCheckBox.UseVisualStyleBackColor = True
            '
            'ViewportTP
            '
            Me.ViewportTP.Controls.Add(Me.Panel2)
            Me.ViewportTP.Controls.Add(Me.Panel1)
            Me.ViewportTP.Controls.Add(Label4)
            Me.ViewportTP.Location = New System.Drawing.Point(4, 22)
            Me.ViewportTP.Name = "ViewportTP"
            Me.ViewportTP.Padding = New System.Windows.Forms.Padding(3)
            Me.ViewportTP.Size = New System.Drawing.Size(430, 210)
            Me.ViewportTP.TabIndex = 5
            Me.ViewportTP.Text = "Viewport"
            Me.ViewportTP.UseVisualStyleBackColor = True
            '
            'Panel2
            '
            Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.Panel2.Controls.Add(Me.MaxYStrictCB)
            Me.Panel2.Controls.Add(Me.MinYStrictCB)
            Me.Panel2.Controls.Add(Label13)
            Me.Panel2.Controls.Add(Me.MaxYieldTextBox)
            Me.Panel2.Controls.Add(Label5)
            Me.Panel2.Controls.Add(Label6)
            Me.Panel2.Controls.Add(Me.MinYieldTextBox)
            Me.Panel2.Controls.Add(Me.MinSpreadTextBox)
            Me.Panel2.Controls.Add(Me.MaxSpreadTextBox)
            Me.Panel2.Controls.Add(Label17)
            Me.Panel2.Controls.Add(Label16)
            Me.Panel2.Location = New System.Drawing.Point(11, 99)
            Me.Panel2.Name = "Panel2"
            Me.Panel2.Size = New System.Drawing.Size(335, 97)
            Me.Panel2.TabIndex = 52
            '
            'MaxYStrictCB
            '
            Me.MaxYStrictCB.AutoSize = True
            Me.MaxYStrictCB.Location = New System.Drawing.Point(240, 73)
            Me.MaxYStrictCB.Name = "MaxYStrictCB"
            Me.MaxYStrictCB.Size = New System.Drawing.Size(15, 14)
            Me.MaxYStrictCB.TabIndex = 60
            Me.MaxYStrictCB.UseVisualStyleBackColor = True
            '
            'MinYStrictCB
            '
            Me.MinYStrictCB.AutoSize = True
            Me.MinYStrictCB.Location = New System.Drawing.Point(157, 74)
            Me.MinYStrictCB.Name = "MinYStrictCB"
            Me.MinYStrictCB.Size = New System.Drawing.Size(15, 14)
            Me.MinYStrictCB.TabIndex = 60
            Me.MinYStrictCB.UseVisualStyleBackColor = True
            '
            'MaxYieldTextBox
            '
            Me.MaxYieldTextBox.Location = New System.Drawing.Point(178, 41)
            Me.MaxYieldTextBox.Name = "MaxYieldTextBox"
            Me.MaxYieldTextBox.Size = New System.Drawing.Size(77, 20)
            Me.MaxYieldTextBox.TabIndex = 51
            '
            'MinYieldTextBox
            '
            Me.MinYieldTextBox.Location = New System.Drawing.Point(95, 41)
            Me.MinYieldTextBox.Name = "MinYieldTextBox"
            Me.MinYieldTextBox.Size = New System.Drawing.Size(77, 20)
            Me.MinYieldTextBox.TabIndex = 52
            '
            'MinSpreadTextBox
            '
            Me.MinSpreadTextBox.Location = New System.Drawing.Point(95, 10)
            Me.MinSpreadTextBox.Name = "MinSpreadTextBox"
            Me.MinSpreadTextBox.Size = New System.Drawing.Size(77, 20)
            Me.MinSpreadTextBox.TabIndex = 53
            '
            'MaxSpreadTextBox
            '
            Me.MaxSpreadTextBox.Location = New System.Drawing.Point(178, 10)
            Me.MaxSpreadTextBox.Name = "MaxSpreadTextBox"
            Me.MaxSpreadTextBox.Size = New System.Drawing.Size(77, 20)
            Me.MaxSpreadTextBox.TabIndex = 54
            '
            'Panel1
            '
            Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.Panel1.Controls.Add(Me.MaxXStrictCB)
            Me.Panel1.Controls.Add(Me.MinXStrictCB)
            Me.Panel1.Controls.Add(Label18)
            Me.Panel1.Controls.Add(Me.MaxDurTextBox)
            Me.Panel1.Controls.Add(Me.MinDurTextBox)
            Me.Panel1.Controls.Add(Label15)
            Me.Panel1.Controls.Add(Label7)
            Me.Panel1.Location = New System.Drawing.Point(11, 28)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(335, 65)
            Me.Panel1.TabIndex = 51
            '
            'MaxXStrictCB
            '
            Me.MaxXStrictCB.AutoSize = True
            Me.MaxXStrictCB.Location = New System.Drawing.Point(240, 41)
            Me.MaxXStrictCB.Name = "MaxXStrictCB"
            Me.MaxXStrictCB.Size = New System.Drawing.Size(15, 14)
            Me.MaxXStrictCB.TabIndex = 62
            Me.MaxXStrictCB.UseVisualStyleBackColor = True
            '
            'MinXStrictCB
            '
            Me.MinXStrictCB.AutoSize = True
            Me.MinXStrictCB.Location = New System.Drawing.Point(157, 42)
            Me.MinXStrictCB.Name = "MinXStrictCB"
            Me.MinXStrictCB.Size = New System.Drawing.Size(15, 14)
            Me.MinXStrictCB.TabIndex = 63
            Me.MinXStrictCB.UseVisualStyleBackColor = True
            '
            'MaxDurTextBox
            '
            Me.MaxDurTextBox.Location = New System.Drawing.Point(178, 12)
            Me.MaxDurTextBox.Name = "MaxDurTextBox"
            Me.MaxDurTextBox.Size = New System.Drawing.Size(77, 20)
            Me.MaxDurTextBox.TabIndex = 51
            '
            'MinDurTextBox
            '
            Me.MinDurTextBox.Location = New System.Drawing.Point(95, 12)
            Me.MinDurTextBox.Name = "MinDurTextBox"
            Me.MinDurTextBox.Size = New System.Drawing.Size(77, 20)
            Me.MinDurTextBox.TabIndex = 50
            '
            'FieldPriorityTabPage
            '
            Me.FieldPriorityTabPage.Controls.Add(Me.DownButton)
            Me.FieldPriorityTabPage.Controls.Add(Me.MoveToHiddenButton)
            Me.FieldPriorityTabPage.Controls.Add(Me.MoveToShownButton)
            Me.FieldPriorityTabPage.Controls.Add(Me.UpButton)
            Me.FieldPriorityTabPage.Controls.Add(Me.HiddenFieldsListBox)
            Me.FieldPriorityTabPage.Controls.Add(Me.FieldsPriorityLB)
            Me.FieldPriorityTabPage.Controls.Add(Me.Label9)
            Me.FieldPriorityTabPage.Controls.Add(Me.Label3)
            Me.FieldPriorityTabPage.Location = New System.Drawing.Point(4, 22)
            Me.FieldPriorityTabPage.Name = "FieldPriorityTabPage"
            Me.FieldPriorityTabPage.Padding = New System.Windows.Forms.Padding(3)
            Me.FieldPriorityTabPage.Size = New System.Drawing.Size(430, 210)
            Me.FieldPriorityTabPage.TabIndex = 4
            Me.FieldPriorityTabPage.Text = "Field priorities"
            Me.FieldPriorityTabPage.UseVisualStyleBackColor = True
            '
            'DownButton
            '
            Me.DownButton.Location = New System.Drawing.Point(152, 61)
            Me.DownButton.Name = "DownButton"
            Me.DownButton.Size = New System.Drawing.Size(47, 23)
            Me.DownButton.TabIndex = 47
            Me.DownButton.Text = "Down"
            Me.DownButton.UseVisualStyleBackColor = True
            '
            'MoveToHiddenButton
            '
            Me.MoveToHiddenButton.Location = New System.Drawing.Point(152, 119)
            Me.MoveToHiddenButton.Name = "MoveToHiddenButton"
            Me.MoveToHiddenButton.Size = New System.Drawing.Size(47, 23)
            Me.MoveToHiddenButton.TabIndex = 48
            Me.MoveToHiddenButton.Text = ">>"
            Me.MoveToHiddenButton.UseVisualStyleBackColor = True
            '
            'MoveToShownButton
            '
            Me.MoveToShownButton.Location = New System.Drawing.Point(152, 90)
            Me.MoveToShownButton.Name = "MoveToShownButton"
            Me.MoveToShownButton.Size = New System.Drawing.Size(47, 23)
            Me.MoveToShownButton.TabIndex = 48
            Me.MoveToShownButton.Text = "<<"
            Me.MoveToShownButton.UseVisualStyleBackColor = True
            '
            'UpButton
            '
            Me.UpButton.Location = New System.Drawing.Point(152, 33)
            Me.UpButton.Name = "UpButton"
            Me.UpButton.Size = New System.Drawing.Size(47, 23)
            Me.UpButton.TabIndex = 48
            Me.UpButton.Text = "Up"
            Me.UpButton.UseVisualStyleBackColor = True
            '
            'HiddenFieldsListBox
            '
            Me.HiddenFieldsListBox.FormattingEnabled = True
            Me.HiddenFieldsListBox.Location = New System.Drawing.Point(205, 34)
            Me.HiddenFieldsListBox.Name = "HiddenFieldsListBox"
            Me.HiddenFieldsListBox.Size = New System.Drawing.Size(140, 160)
            Me.HiddenFieldsListBox.TabIndex = 46
            '
            'FieldsPriorityLB
            '
            Me.FieldsPriorityLB.FormattingEnabled = True
            Me.FieldsPriorityLB.Location = New System.Drawing.Point(6, 34)
            Me.FieldsPriorityLB.Name = "FieldsPriorityLB"
            Me.FieldsPriorityLB.Size = New System.Drawing.Size(140, 160)
            Me.FieldsPriorityLB.TabIndex = 46
            '
            'Label9
            '
            Me.Label9.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.Label9.AutoSize = True
            Me.Label9.Location = New System.Drawing.Point(202, 12)
            Me.Label9.Margin = New System.Windows.Forms.Padding(0)
            Me.Label9.Name = "Label9"
            Me.Label9.Size = New System.Drawing.Size(135, 13)
            Me.Label9.TabIndex = 45
            Me.Label9.Text = "Fields not shown by default"
            Me.Label9.Visible = False
            '
            'Label3
            '
            Me.Label3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(3, 12)
            Me.Label3.Margin = New System.Windows.Forms.Padding(0)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(70, 13)
            Me.Label3.TabIndex = 45
            Me.Label3.Text = "Fields priority:"
            Me.Label3.Visible = False
            '
            'MainGeneralTabPage
            '
            Me.MainGeneralTabPage.Controls.Add(Me.LoadRicsButton)
            Me.MainGeneralTabPage.Controls.Add(Me.LoadRicsCB)
            Me.MainGeneralTabPage.Controls.Add(Me.Label10)
            Me.MainGeneralTabPage.Controls.Add(Me.Label2)
            Me.MainGeneralTabPage.Controls.Add(Me.ChartWindowCheckBox)
            Me.MainGeneralTabPage.Controls.Add(Me.MainWindowCheckBox)
            Me.MainGeneralTabPage.Location = New System.Drawing.Point(4, 22)
            Me.MainGeneralTabPage.Name = "MainGeneralTabPage"
            Me.MainGeneralTabPage.Padding = New System.Windows.Forms.Padding(3)
            Me.MainGeneralTabPage.Size = New System.Drawing.Size(430, 210)
            Me.MainGeneralTabPage.TabIndex = 1
            Me.MainGeneralTabPage.Text = "General"
            Me.MainGeneralTabPage.UseVisualStyleBackColor = True
            '
            'LoadRicsButton
            '
            Me.LoadRicsButton.Location = New System.Drawing.Point(148, 80)
            Me.LoadRicsButton.Name = "LoadRicsButton"
            Me.LoadRicsButton.Size = New System.Drawing.Size(210, 23)
            Me.LoadRicsButton.TabIndex = 3
            Me.LoadRicsButton.Text = "Save settings and load contributed rics"
            Me.LoadRicsButton.UseVisualStyleBackColor = True
            Me.LoadRicsButton.Visible = False
            '
            'LoadRicsCB
            '
            Me.LoadRicsCB.AutoSize = True
            Me.LoadRicsCB.Location = New System.Drawing.Point(10, 84)
            Me.LoadRicsCB.Name = "LoadRicsCB"
            Me.LoadRicsCB.Size = New System.Drawing.Size(132, 17)
            Me.LoadRicsCB.TabIndex = 2
            Me.LoadRicsCB.Text = "Load contributed RICs"
            Me.LoadRicsCB.UseVisualStyleBackColor = True
            '
            'Label10
            '
            Me.Label10.AutoSize = True
            Me.Label10.Location = New System.Drawing.Point(7, 120)
            Me.Label10.Name = "Label10"
            Me.Label10.Size = New System.Drawing.Size(417, 39)
            Me.Label10.TabIndex = 1
            Me.Label10.Text = "If you want to start using contributed rics immediately, please set the above che" & _
        "ckbox " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "and press button ""Save settings and load contributed rics"". Otherwise it" & _
        " will take effect " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "after restart"
            Me.Label10.Visible = False
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
            Me.ChartWindowCheckBox.Location = New System.Drawing.Point(20, 51)
            Me.ChartWindowCheckBox.Name = "ChartWindowCheckBox"
            Me.ChartWindowCheckBox.Size = New System.Drawing.Size(106, 17)
            Me.ChartWindowCheckBox.TabIndex = 0
            Me.ChartWindowCheckBox.Text = "On chart window"
            Me.ChartWindowCheckBox.UseVisualStyleBackColor = True
            '
            'MainWindowCheckBox
            '
            Me.MainWindowCheckBox.AutoSize = True
            Me.MainWindowCheckBox.Location = New System.Drawing.Point(20, 28)
            Me.MainWindowCheckBox.Name = "MainWindowCheckBox"
            Me.MainWindowCheckBox.Size = New System.Drawing.Size(104, 17)
            Me.MainWindowCheckBox.TabIndex = 0
            Me.MainWindowCheckBox.Text = "On main window"
            Me.MainWindowCheckBox.UseVisualStyleBackColor = True
            '
            'MainLoadColumnsPage
            '
            Me.MainLoadColumnsPage.Controls.Add(Me.Panel4)
            Me.MainLoadColumnsPage.Location = New System.Drawing.Point(4, 22)
            Me.MainLoadColumnsPage.Name = "MainLoadColumnsPage"
            Me.MainLoadColumnsPage.Padding = New System.Windows.Forms.Padding(3)
            Me.MainLoadColumnsPage.Size = New System.Drawing.Size(430, 210)
            Me.MainLoadColumnsPage.TabIndex = 3
            Me.MainLoadColumnsPage.Text = "Bond selection"
            Me.MainLoadColumnsPage.UseVisualStyleBackColor = True
            '
            'Panel4
            '
            Me.Panel4.Controls.Add(Me.FirstLevelRB)
            Me.Panel4.Controls.Add(Me.FirstDateRB)
            Me.Panel4.Controls.Add(Me.Label12)
            Me.Panel4.Controls.Add(Me.AllColumnsCB)
            Me.Panel4.Controls.Add(Me.Label14)
            Me.Panel4.Controls.Add(Me.ColumnsCLB)
            Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
            Me.Panel4.Location = New System.Drawing.Point(3, 3)
            Me.Panel4.Margin = New System.Windows.Forms.Padding(0)
            Me.Panel4.Name = "Panel4"
            Me.Panel4.Size = New System.Drawing.Size(424, 204)
            Me.Panel4.TabIndex = 0
            '
            'FirstLevelRB
            '
            Me.FirstLevelRB.AutoSize = True
            Me.FirstLevelRB.Location = New System.Drawing.Point(174, 49)
            Me.FirstLevelRB.Name = "FirstLevelRB"
            Me.FirstLevelRB.Size = New System.Drawing.Size(102, 17)
            Me.FirstLevelRB.TabIndex = 5
            Me.FirstLevelRB.TabStop = True
            Me.FirstLevelRB.Text = "Level, then date"
            Me.FirstLevelRB.UseVisualStyleBackColor = True
            '
            'FirstDateRB
            '
            Me.FirstDateRB.AutoSize = True
            Me.FirstDateRB.Location = New System.Drawing.Point(174, 26)
            Me.FirstDateRB.Name = "FirstDateRB"
            Me.FirstDateRB.Size = New System.Drawing.Size(100, 17)
            Me.FirstDateRB.TabIndex = 4
            Me.FirstDateRB.TabStop = True
            Me.FirstDateRB.Text = "Date, then level"
            Me.FirstDateRB.UseVisualStyleBackColor = True
            '
            'Label12
            '
            Me.Label12.AutoSize = True
            Me.Label12.Location = New System.Drawing.Point(171, 3)
            Me.Label12.Name = "Label12"
            Me.Label12.Size = New System.Drawing.Size(101, 13)
            Me.Label12.TabIndex = 3
            Me.Label12.Text = "Rating sorting mode"
            '
            'AllColumnsCB
            '
            Me.AllColumnsCB.AutoSize = True
            Me.AllColumnsCB.Location = New System.Drawing.Point(2, 22)
            Me.AllColumnsCB.Name = "AllColumnsCB"
            Me.AllColumnsCB.Size = New System.Drawing.Size(79, 17)
            Me.AllColumnsCB.TabIndex = 2
            Me.AllColumnsCB.Text = "All columns"
            Me.AllColumnsCB.UseVisualStyleBackColor = True
            '
            'Label14
            '
            Me.Label14.AutoSize = True
            Me.Label14.Location = New System.Drawing.Point(2, 3)
            Me.Label14.Margin = New System.Windows.Forms.Padding(3)
            Me.Label14.Name = "Label14"
            Me.Label14.Size = New System.Drawing.Size(90, 13)
            Me.Label14.TabIndex = 1
            Me.Label14.Text = "Columns to show:"
            '
            'ColumnsCLB
            '
            Me.ColumnsCLB.FormattingEnabled = True
            Me.ColumnsCLB.Location = New System.Drawing.Point(2, 45)
            Me.ColumnsCLB.Name = "ColumnsCLB"
            Me.ColumnsCLB.Size = New System.Drawing.Size(146, 154)
            Me.ColumnsCLB.TabIndex = 0
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
            Me.MainLogPage.Size = New System.Drawing.Size(430, 210)
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
            Me.TheCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.TheCancelButton.Location = New System.Drawing.Point(286, 245)
            Me.TheCancelButton.Name = "TheCancelButton"
            Me.TheCancelButton.Size = New System.Drawing.Size(155, 24)
            Me.TheCancelButton.TabIndex = 7
            Me.TheCancelButton.Text = "Cancel"
            Me.TheCancelButton.UseVisualStyleBackColor = True
            '
            'ErrProv
            '
            Me.ErrProv.ContainerControl = Me
            '
            'Label19
            '
            Label19.AutoSize = True
            Label19.Location = New System.Drawing.Point(6, 150)
            Label19.Name = "Label19"
            Label19.Size = New System.Drawing.Size(192, 13)
            Label19.TabIndex = 44
            Label19.Text = "Number of points for curve interpolation"
            '
            'NumInterpPointsTB
            '
            Me.NumInterpPointsTB.Location = New System.Drawing.Point(204, 147)
            Me.NumInterpPointsTB.Name = "NumInterpPointsTB"
            Me.NumInterpPointsTB.Size = New System.Drawing.Size(100, 20)
            Me.NumInterpPointsTB.TabIndex = 45
            '
            'SettingsForm
            '
            Me.AcceptButton = Me.SaveSettingsButton
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.TheCancelButton
            Me.ClientSize = New System.Drawing.Size(444, 272)
            Me.Controls.Add(Me.MainTableLayoutPanel)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(450, 300)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(450, 263)
            Me.Name = "SettingsForm"
            Me.Text = "Settings"
            Me.MainTableLayoutPanel.ResumeLayout(False)
            Me.MainTabControl.ResumeLayout(False)
            Me.MainChartPage.ResumeLayout(False)
            Me.MainChartPage.PerformLayout()
            Me.ViewportTP.ResumeLayout(False)
            Me.ViewportTP.PerformLayout()
            Me.Panel2.ResumeLayout(False)
            Me.Panel2.PerformLayout()
            Me.Panel1.ResumeLayout(False)
            Me.Panel1.PerformLayout()
            Me.FieldPriorityTabPage.ResumeLayout(False)
            Me.FieldPriorityTabPage.PerformLayout()
            Me.MainGeneralTabPage.ResumeLayout(False)
            Me.MainGeneralTabPage.PerformLayout()
            Me.MainLoadColumnsPage.ResumeLayout(False)
            Me.Panel4.ResumeLayout(False)
            Me.Panel4.PerformLayout()
            Me.MainLogPage.ResumeLayout(False)
            Me.MainLogPage.PerformLayout()
            CType(Me.ErrProv, System.ComponentModel.ISupportInitialize).EndInit()
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
        Friend WithEvents MainLoadColumnsPage As System.Windows.Forms.TabPage
        Friend WithEvents Panel4 As System.Windows.Forms.Panel
        Friend WithEvents ColumnsCLB As System.Windows.Forms.CheckedListBox
        Friend WithEvents AllColumnsCB As System.Windows.Forms.CheckBox
        Friend WithEvents Label14 As System.Windows.Forms.Label
        Friend WithEvents ShowPointSizeCheckBox As System.Windows.Forms.CheckBox
        Friend WithEvents ShowBidAskCheckBox As System.Windows.Forms.CheckBox
        Friend WithEvents YieldCalcModeCB As System.Windows.Forms.ComboBox
        Friend WithEvents Label8 As System.Windows.Forms.Label
        Friend WithEvents FieldPriorityTabPage As System.Windows.Forms.TabPage
        Friend WithEvents DownButton As System.Windows.Forms.Button
        Friend WithEvents MoveToHiddenButton As System.Windows.Forms.Button
        Friend WithEvents MoveToShownButton As System.Windows.Forms.Button
        Friend WithEvents UpButton As System.Windows.Forms.Button
        Friend WithEvents HiddenFieldsListBox As System.Windows.Forms.ListBox
        Friend WithEvents FieldsPriorityLB As System.Windows.Forms.ListBox
        Friend WithEvents Label9 As System.Windows.Forms.Label
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents MidIfBothCB As System.Windows.Forms.CheckBox
        Friend WithEvents LoadRicsCB As System.Windows.Forms.CheckBox
        Friend WithEvents LoadRicsButton As System.Windows.Forms.Button
        Friend WithEvents Label10 As System.Windows.Forms.Label
        Friend WithEvents ClearPointsCheckBox As System.Windows.Forms.CheckBox
        Friend WithEvents ClearOtherCurvesCheckBox As System.Windows.Forms.CheckBox
        Friend WithEvents ClearBondCurvesCheckBox As System.Windows.Forms.CheckBox
        Friend WithEvents FirstLevelRB As System.Windows.Forms.RadioButton
        Friend WithEvents FirstDateRB As System.Windows.Forms.RadioButton
        Friend WithEvents Label12 As System.Windows.Forms.Label
        Friend WithEvents ViewportTP As System.Windows.Forms.TabPage
        Friend WithEvents Panel2 As System.Windows.Forms.Panel
        Friend WithEvents MaxYStrictCB As System.Windows.Forms.CheckBox
        Friend WithEvents MinYStrictCB As System.Windows.Forms.CheckBox
        Friend WithEvents MaxYieldTextBox As System.Windows.Forms.MaskedTextBox
        Friend WithEvents MinYieldTextBox As System.Windows.Forms.MaskedTextBox
        Friend WithEvents MinSpreadTextBox As System.Windows.Forms.MaskedTextBox
        Friend WithEvents MaxSpreadTextBox As System.Windows.Forms.MaskedTextBox
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents MaxXStrictCB As System.Windows.Forms.CheckBox
        Friend WithEvents MinXStrictCB As System.Windows.Forms.CheckBox
        Friend WithEvents MaxDurTextBox As System.Windows.Forms.MaskedTextBox
        Friend WithEvents MinDurTextBox As System.Windows.Forms.MaskedTextBox
        Friend WithEvents ErrProv As System.Windows.Forms.ErrorProvider
        Friend WithEvents NumInterpPointsTB As System.Windows.Forms.TextBox
    End Class
End Namespace