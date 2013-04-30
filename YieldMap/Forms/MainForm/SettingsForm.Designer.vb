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
            Me.YieldCalcModeCB = New System.Windows.Forms.ComboBox()
            Me.Label8 = New System.Windows.Forms.Label()
            Me.MaxYieldTextBox = New System.Windows.Forms.MaskedTextBox()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.Label6 = New System.Windows.Forms.Label()
            Me.MaxDurTextBox = New System.Windows.Forms.MaskedTextBox()
            Me.MinYieldTextBox = New System.Windows.Forms.MaskedTextBox()
            Me.MinSpreadTextBox = New System.Windows.Forms.MaskedTextBox()
            Me.MinDurTextBox = New System.Windows.Forms.MaskedTextBox()
            Me.MaxSpreadTextBox = New System.Windows.Forms.MaskedTextBox()
            Me.Label7 = New System.Windows.Forms.Label()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.ShowPointSizeCheckBox = New System.Windows.Forms.CheckBox()
            Me.ShowBidAskCheckBox = New System.Windows.Forms.CheckBox()
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
            Me.Label2 = New System.Windows.Forms.Label()
            Me.ChartWindowCheckBox = New System.Windows.Forms.CheckBox()
            Me.MainWindowCheckBox = New System.Windows.Forms.CheckBox()
            Me.MainLoadColumnsPage = New System.Windows.Forms.TabPage()
            Me.Panel4 = New System.Windows.Forms.Panel()
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
            Me.MainTableLayoutPanel.SuspendLayout()
            Me.MainTabControl.SuspendLayout()
            Me.MainChartPage.SuspendLayout()
            Me.FieldPriorityTabPage.SuspendLayout()
            Me.MainGeneralTabPage.SuspendLayout()
            Me.MainLoadColumnsPage.SuspendLayout()
            Me.Panel4.SuspendLayout()
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
            Me.MainTableLayoutPanel.Size = New System.Drawing.Size(544, 275)
            Me.MainTableLayoutPanel.TabIndex = 5
            '
            'SaveSettingsButton
            '
            Me.SaveSettingsButton.Location = New System.Drawing.Point(3, 248)
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
            Me.MainTabControl.Controls.Add(Me.FieldPriorityTabPage)
            Me.MainTabControl.Controls.Add(Me.MainGeneralTabPage)
            Me.MainTabControl.Controls.Add(Me.MainLoadColumnsPage)
            Me.MainTabControl.Controls.Add(Me.MainLogPage)
            Me.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill
            Me.MainTabControl.Location = New System.Drawing.Point(3, 3)
            Me.MainTabControl.Name = "MainTabControl"
            Me.MainTabControl.SelectedIndex = 0
            Me.MainTabControl.Size = New System.Drawing.Size(538, 239)
            Me.MainTabControl.TabIndex = 5
            '
            'MainChartPage
            '
            Me.MainChartPage.Controls.Add(Me.YieldCalcModeCB)
            Me.MainChartPage.Controls.Add(Me.Label8)
            Me.MainChartPage.Controls.Add(Me.MaxYieldTextBox)
            Me.MainChartPage.Controls.Add(Me.Label5)
            Me.MainChartPage.Controls.Add(Me.Label6)
            Me.MainChartPage.Controls.Add(Me.MaxDurTextBox)
            Me.MainChartPage.Controls.Add(Me.MinYieldTextBox)
            Me.MainChartPage.Controls.Add(Me.MinSpreadTextBox)
            Me.MainChartPage.Controls.Add(Me.MinDurTextBox)
            Me.MainChartPage.Controls.Add(Me.MaxSpreadTextBox)
            Me.MainChartPage.Controls.Add(Me.Label7)
            Me.MainChartPage.Controls.Add(Me.Label4)
            Me.MainChartPage.Controls.Add(Me.ShowPointSizeCheckBox)
            Me.MainChartPage.Controls.Add(Me.ShowBidAskCheckBox)
            Me.MainChartPage.Location = New System.Drawing.Point(4, 22)
            Me.MainChartPage.Name = "MainChartPage"
            Me.MainChartPage.Padding = New System.Windows.Forms.Padding(3)
            Me.MainChartPage.Size = New System.Drawing.Size(530, 213)
            Me.MainChartPage.TabIndex = 2
            Me.MainChartPage.Text = "Chart"
            Me.MainChartPage.UseVisualStyleBackColor = True
            '
            'YieldCalcModeCB
            '
            Me.YieldCalcModeCB.FormattingEnabled = True
            Me.YieldCalcModeCB.Items.AddRange(New Object() {"YTM", "YTW", "YTB", "YTA"})
            Me.YieldCalcModeCB.Location = New System.Drawing.Point(148, 150)
            Me.YieldCalcModeCB.Name = "YieldCalcModeCB"
            Me.YieldCalcModeCB.Size = New System.Drawing.Size(160, 21)
            Me.YieldCalcModeCB.TabIndex = 42
            '
            'Label8
            '
            Me.Label8.AutoSize = True
            Me.Label8.Location = New System.Drawing.Point(7, 153)
            Me.Label8.Name = "Label8"
            Me.Label8.Size = New System.Drawing.Size(113, 13)
            Me.Label8.TabIndex = 41
            Me.Label8.Text = "Yield calculation mode"
            '
            'MaxYieldTextBox
            '
            Me.MaxYieldTextBox.Location = New System.Drawing.Point(231, 124)
            Me.MaxYieldTextBox.Mask = "00 %"
            Me.MaxYieldTextBox.Name = "MaxYieldTextBox"
            Me.MaxYieldTextBox.Size = New System.Drawing.Size(77, 20)
            Me.MaxYieldTextBox.TabIndex = 32
            '
            'Label5
            '
            Me.Label5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.Label5.AutoSize = True
            Me.Label5.Location = New System.Drawing.Point(10, 124)
            Me.Label5.Margin = New System.Windows.Forms.Padding(6, 6, 0, 0)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(71, 13)
            Me.Label5.TabIndex = 38
            Me.Label5.Text = "By yield (in %)"
            '
            'Label6
            '
            Me.Label6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.Label6.AutoSize = True
            Me.Label6.Location = New System.Drawing.Point(10, 96)
            Me.Label6.Margin = New System.Windows.Forms.Padding(6, 6, 0, 0)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New System.Drawing.Size(129, 13)
            Me.Label6.TabIndex = 40
            Me.Label6.Text = "By spread (in basis points)"
            '
            'MaxDurTextBox
            '
            Me.MaxDurTextBox.Location = New System.Drawing.Point(231, 63)
            Me.MaxDurTextBox.Mask = "00 yrs"
            Me.MaxDurTextBox.Name = "MaxDurTextBox"
            Me.MaxDurTextBox.Size = New System.Drawing.Size(77, 20)
            Me.MaxDurTextBox.TabIndex = 34
            '
            'MinYieldTextBox
            '
            Me.MinYieldTextBox.Location = New System.Drawing.Point(148, 124)
            Me.MinYieldTextBox.Mask = "00 %"
            Me.MinYieldTextBox.Name = "MinYieldTextBox"
            Me.MinYieldTextBox.Size = New System.Drawing.Size(77, 20)
            Me.MinYieldTextBox.TabIndex = 35
            '
            'MinSpreadTextBox
            '
            Me.MinSpreadTextBox.Location = New System.Drawing.Point(148, 93)
            Me.MinSpreadTextBox.Mask = "#0000 b.p."
            Me.MinSpreadTextBox.Name = "MinSpreadTextBox"
            Me.MinSpreadTextBox.Size = New System.Drawing.Size(77, 20)
            Me.MinSpreadTextBox.TabIndex = 36
            '
            'MinDurTextBox
            '
            Me.MinDurTextBox.Location = New System.Drawing.Point(148, 63)
            Me.MinDurTextBox.Mask = "00 yrs"
            Me.MinDurTextBox.Name = "MinDurTextBox"
            Me.MinDurTextBox.Size = New System.Drawing.Size(77, 20)
            Me.MinDurTextBox.TabIndex = 33
            '
            'MaxSpreadTextBox
            '
            Me.MaxSpreadTextBox.Location = New System.Drawing.Point(231, 93)
            Me.MaxSpreadTextBox.Mask = "#0000 b.p."
            Me.MaxSpreadTextBox.Name = "MaxSpreadTextBox"
            Me.MaxSpreadTextBox.Size = New System.Drawing.Size(77, 20)
            Me.MaxSpreadTextBox.TabIndex = 37
            '
            'Label7
            '
            Me.Label7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.Label7.AutoSize = True
            Me.Label7.Location = New System.Drawing.Point(10, 70)
            Me.Label7.Margin = New System.Windows.Forms.Padding(6, 6, 0, 0)
            Me.Label7.Name = "Label7"
            Me.Label7.Size = New System.Drawing.Size(105, 13)
            Me.Label7.TabIndex = 39
            Me.Label7.Text = "By duration (in years)"
            '
            'Label4
            '
            Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(4, 41)
            Me.Label4.Margin = New System.Windows.Forms.Padding(6, 6, 0, 0)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(135, 13)
            Me.Label4.TabIndex = 11
            Me.Label4.Text = "Chart viewport default limits"
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
            Me.FieldPriorityTabPage.Size = New System.Drawing.Size(530, 213)
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
            Me.MoveToHiddenButton.Location = New System.Drawing.Point(281, 33)
            Me.MoveToHiddenButton.Name = "MoveToHiddenButton"
            Me.MoveToHiddenButton.Size = New System.Drawing.Size(47, 23)
            Me.MoveToHiddenButton.TabIndex = 48
            Me.MoveToHiddenButton.Text = ">>"
            Me.MoveToHiddenButton.UseVisualStyleBackColor = True
            '
            'MoveToShownButton
            '
            Me.MoveToShownButton.Location = New System.Drawing.Point(228, 33)
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
            Me.HiddenFieldsListBox.Location = New System.Drawing.Point(379, 34)
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
            Me.Label9.Location = New System.Drawing.Point(376, 18)
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
            Me.MainGeneralTabPage.Controls.Add(Me.Label2)
            Me.MainGeneralTabPage.Controls.Add(Me.ChartWindowCheckBox)
            Me.MainGeneralTabPage.Controls.Add(Me.MainWindowCheckBox)
            Me.MainGeneralTabPage.Location = New System.Drawing.Point(4, 22)
            Me.MainGeneralTabPage.Name = "MainGeneralTabPage"
            Me.MainGeneralTabPage.Padding = New System.Windows.Forms.Padding(3)
            Me.MainGeneralTabPage.Size = New System.Drawing.Size(530, 213)
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
            'MainLoadColumnsPage
            '
            Me.MainLoadColumnsPage.Controls.Add(Me.Panel4)
            Me.MainLoadColumnsPage.Location = New System.Drawing.Point(4, 22)
            Me.MainLoadColumnsPage.Name = "MainLoadColumnsPage"
            Me.MainLoadColumnsPage.Padding = New System.Windows.Forms.Padding(3)
            Me.MainLoadColumnsPage.Size = New System.Drawing.Size(530, 213)
            Me.MainLoadColumnsPage.TabIndex = 3
            Me.MainLoadColumnsPage.Text = "Columns"
            Me.MainLoadColumnsPage.UseVisualStyleBackColor = True
            '
            'Panel4
            '
            Me.Panel4.Controls.Add(Me.AllColumnsCB)
            Me.Panel4.Controls.Add(Me.Label14)
            Me.Panel4.Controls.Add(Me.ColumnsCLB)
            Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
            Me.Panel4.Location = New System.Drawing.Point(3, 3)
            Me.Panel4.Margin = New System.Windows.Forms.Padding(0)
            Me.Panel4.Name = "Panel4"
            Me.Panel4.Size = New System.Drawing.Size(524, 207)
            Me.Panel4.TabIndex = 0
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
            Me.Label14.Size = New System.Drawing.Size(142, 13)
            Me.Label14.TabIndex = 1
            Me.Label14.Text = "In bond selection form show:"
            '
            'ColumnsCLB
            '
            Me.ColumnsCLB.FormattingEnabled = True
            Me.ColumnsCLB.Items.AddRange(New Object() {"Bond name", "Description", "RIC", "Issuer", "Issue date", "Maturity", "Coupon", "Currency", "Next put", "Next call"})
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
            Me.MainLogPage.Size = New System.Drawing.Size(530, 213)
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
            Me.TheCancelButton.Location = New System.Drawing.Point(386, 248)
            Me.TheCancelButton.Name = "TheCancelButton"
            Me.TheCancelButton.Size = New System.Drawing.Size(155, 24)
            Me.TheCancelButton.TabIndex = 7
            Me.TheCancelButton.Text = "Cancel"
            Me.TheCancelButton.UseVisualStyleBackColor = True
            '
            'SettingsForm
            '
            Me.AcceptButton = Me.SaveSettingsButton
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.TheCancelButton
            Me.ClientSize = New System.Drawing.Size(544, 275)
            Me.Controls.Add(Me.MainTableLayoutPanel)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(550, 300)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(550, 263)
            Me.Name = "SettingsForm"
            Me.Text = "Settings"
            Me.MainTableLayoutPanel.ResumeLayout(False)
            Me.MainTabControl.ResumeLayout(False)
            Me.MainChartPage.ResumeLayout(False)
            Me.MainChartPage.PerformLayout()
            Me.FieldPriorityTabPage.ResumeLayout(False)
            Me.FieldPriorityTabPage.PerformLayout()
            Me.MainGeneralTabPage.ResumeLayout(False)
            Me.MainGeneralTabPage.PerformLayout()
            Me.MainLoadColumnsPage.ResumeLayout(False)
            Me.Panel4.ResumeLayout(False)
            Me.Panel4.PerformLayout()
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
        Friend WithEvents MainLoadColumnsPage As System.Windows.Forms.TabPage
        Friend WithEvents Panel4 As System.Windows.Forms.Panel
        Friend WithEvents ColumnsCLB As System.Windows.Forms.CheckedListBox
        Friend WithEvents AllColumnsCB As System.Windows.Forms.CheckBox
        Friend WithEvents Label14 As System.Windows.Forms.Label
        Friend WithEvents MaxYieldTextBox As System.Windows.Forms.MaskedTextBox
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents MaxDurTextBox As System.Windows.Forms.MaskedTextBox
        Friend WithEvents MinYieldTextBox As System.Windows.Forms.MaskedTextBox
        Friend WithEvents MinSpreadTextBox As System.Windows.Forms.MaskedTextBox
        Friend WithEvents MinDurTextBox As System.Windows.Forms.MaskedTextBox
        Friend WithEvents MaxSpreadTextBox As System.Windows.Forms.MaskedTextBox
        Friend WithEvents Label7 As System.Windows.Forms.Label
        Friend WithEvents Label4 As System.Windows.Forms.Label
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
    End Class
End Namespace