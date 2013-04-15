Imports System.Text.RegularExpressions
Imports DbManager.Bonds
Imports NLog
Imports Settings

Namespace Forms.MainForm
    Public Class SettingsForm
        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(SettingsForm))
        Private Shared ReadOnly Settings As SettingsManager = SettingsManager.Instance

        Private Sub SettingsFormLoad(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.Load
            Select Case Settings.LogLevel
                Case LogLevel.Trace : LogTraceRadioButton.Checked = True
                Case LogLevel.Debug : LogDebugRadioButton.Checked = True
                Case LogLevel.Info : LogInfoRadioButton.Checked = True
                Case LogLevel.Warn : LogWarnRadioButton.Checked = True
                Case LogLevel.Error : LogErrRadioButton.Checked = True
                Case LogLevel.Fatal : LogFatalRadioButton.Checked = True
                Case LogLevel.Off : LogNoneRadioButton.Checked = True
            End Select

            MinYieldTextBox.Text = If(Settings.MinYield.HasValue, Settings.MinYield, "")
            MaxYieldTextBox.Text = If(Settings.MaxYield.HasValue, Settings.MaxYield, "")

            MinDurTextBox.Text = If(Settings.MinDur.HasValue, Settings.MinDur, "")
            MaxDurTextBox.Text = If(Settings.MaxDur.HasValue, Settings.MaxDur, "")

            MinSpreadTextBox.Text = If(Settings.MinSpread.HasValue, Settings.MinSpread, "")
            MaxSpreadTextBox.Text = If(Settings.MaxSpread.HasValue, Settings.MaxSpread, "")

            ShowBidAskCheckBox.Checked = Settings.ShowBidAsk
            ShowPointSizeCheckBox.Checked = Settings.ShowPointSize


            MainWindowCheckBox.Checked = Settings.ShowMainToolBar
            ChartWindowCheckBox.Checked = Settings.ShowChartToolBar

            Dim selectedFields = Settings.BondSelectorVisibleColumns.Split(",")
            ColumnsCLB.Items.Clear()
            For Each field In BondDescription.GetSelectableFields()
                Dim idx = ColumnsCLB.Items.Add(field)
                ColumnsCLB.SetItemCheckState(idx, If(selectedFields.Contains(field), CheckState.Checked, CheckState.Unchecked))
            Next
            If selectedFields.Contains("ALL") Then
                AllColumnsCB.Checked = True
                SetAllCheckState()
            End If
        End Sub

        Private Sub CancelButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles TheCancelButton.Click
            Close()
        End Sub

        Private Sub SaveSettingsButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles SaveSettingsButton.Click
            If LogTraceRadioButton.Checked Then
                Settings.LogLevel = LogLevel.Trace
            ElseIf LogDebugRadioButton.Checked Then
                Settings.LogLevel = LogLevel.Debug
            ElseIf LogInfoRadioButton.Checked Then
                Settings.LogLevel = LogLevel.Info
            ElseIf LogWarnRadioButton.Checked Then
                Settings.LogLevel = LogLevel.Warn
            ElseIf LogErrRadioButton.Checked Then
                Settings.LogLevel = LogLevel.Error
            ElseIf LogNoneRadioButton.Checked Then
                Settings.LogLevel = LogLevel.Off
            End If

            ' Logging.ResetLoggers()
            'SettingsManager.LogLevel = SettingsManager.LogLevel
            Logging.LoggingLevel = Settings.LogLevel
            Logger.Log(Settings.LogLevel, "Log level set to {0}", Settings.LogLevel)

            Settings.MinYield = ParseDouble(MinYieldTextBox.Text)
            Settings.MaxYield = ParseDouble(MaxYieldTextBox.Text)
            Settings.MinDur = ParseDouble(MinDurTextBox.Text)
            Settings.MaxDur = ParseDouble(MaxDurTextBox.Text)
            Settings.MinSpread = ParseDouble(MinSpreadTextBox.Text)
            Settings.MaxSpread = ParseDouble(MaxSpreadTextBox.Text)

            Settings.ShowBidAsk = ShowBidAskCheckBox.Checked
            Settings.ShowPointSize = ShowPointSizeCheckBox.Checked

            Settings.ShowMainToolBar = MainWindowCheckBox.Checked
            Settings.ShowChartToolBar = ChartWindowCheckBox.Checked

            Dim columnsString As String = ""

            If AllColumnsCB.CheckState = CheckState.Checked Then
                columnsString = "ALL"
            Else
                For i = 0 To ColumnsCLB.Items.Count - 1
                    If ColumnsCLB.GetItemCheckState(i) = CheckState.Checked Then
                        columnsString = columnsString & ColumnsCLB.Items(i).ToString() & ","
                    End If
                Next
            End If
            If AllColumnsCB.CheckState <> CheckState.Checked And columnsString <> "" Then
                columnsString = columnsString.Substring(0, columnsString.Length() - 1)
            End If
            Settings.BondSelectorVisibleColumns = columnsString

            Close()
        End Sub

        Private Shared Function ParseDouble(ByVal txt As String) As Double?
            Dim regex As New Regex("^(?<number>[\-0-9]+)")
            Dim match As Match
            Dim result As Double? = Nothing

            match = regex.Match(txt)

            If match.Success Then
                txt = match.Groups("number").Value
                result = If(IsNumeric(txt), Double.Parse(txt), Nothing)
            End If
            Return result
        End Function

        Private Sub AllColumnsCB_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles AllColumnsCB.Click
            SetAllCheckState()
        End Sub

        Private Sub SetAllCheckState()
            If AllColumnsCB.CheckState <> CheckState.Indeterminate Then
                For i As Integer = 0 To ColumnsCLB.Items.Count - 1
                    ColumnsCLB.SetItemCheckState(i, AllColumnsCB.CheckState)
                Next
            End If
        End Sub

        Private Sub ColumnsCLB_SelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ColumnsCLB.SelectedValueChanged
            RefreshAllBox()
        End Sub

        Private Sub RefreshAllBox()
            Dim allSelected = True
            Dim noneSelected = True
            For i As Integer = 0 To ColumnsCLB.Items.Count - 1
                Dim cs = ColumnsCLB.GetItemCheckState(i)
                allSelected = allSelected And (cs = CheckState.Checked)
                noneSelected = noneSelected And (cs <> CheckState.Checked)
                If Not allSelected And Not noneSelected Then Exit For
            Next
            If allSelected Then
                AllColumnsCB.CheckState = CheckState.Checked
            ElseIf noneSelected Then
                AllColumnsCB.Checked = CheckState.Unchecked
            Else
                AllColumnsCB.CheckState = CheckState.Indeterminate
            End If
        End Sub
    End Class
End Namespace