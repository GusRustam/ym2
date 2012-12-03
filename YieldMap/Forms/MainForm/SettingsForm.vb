Imports System.Text.RegularExpressions
Imports YieldMap.Commons
Imports NLog

Namespace Forms.MainForm
    Public Class SettingsForm
        Private Sub SettingsFormLoad(sender As System.Object, e As EventArgs) Handles MyBase.Load
            Select Case LoggingLevel
                Case LogLevel.Trace : LogTraceRadioButton.Checked = True
                Case LogLevel.Debug : LogDebugRadioButton.Checked = True
                Case LogLevel.Info : LogInfoRadioButton.Checked = True
                Case LogLevel.Warn : LogWarnRadioButton.Checked = True
                Case LogLevel.Error : LogErrRadioButton.Checked = True
                Case LogLevel.Fatal : LogFatalRadioButton.Checked = True
                Case LogLevel.Off : LogNoneRadioButton.Checked = True
            End Select

            MinYieldTextBox.Text = If(MinYield.HasValue, MinYield, "")
            MaxYieldTextBox.Text = If(MaxYield.HasValue, MaxYield, "")

            MinDurTextBox.Text = If(MinDur.HasValue, MinDur, "")
            MaxDurTextBox.Text = If(MaxDur.HasValue, MaxDur, "")

            MinSpreadTextBox.Text = If(MinSpread.HasValue, MinSpread, "")
            MaxSpreadTextBox.Text = If(MaxSpread.HasValue, MaxSpread, "")

            ShowBidAskCheckBox.Checked = ShowBidAsk
            ShowPointSizeCheckBox.Checked = ShowPointSize

            UseLastRadioButton.Checked = (DefaultField = "LAST")
            UseVWAPRadioButton.Checked = Not UseLastRadioButton.Checked

            MainWindowCheckBox.Checked = ShowMainToolBar
            ChartWindowCheckBox.Checked = ShowChartToolBar
        End Sub

        Private Sub CancelButtonClick(sender As System.Object, e As EventArgs) Handles TheCancelButton.Click
            Close()
        End Sub

        Private Sub SaveSettingsButtonClick(sender As System.Object, e As EventArgs) Handles SaveSettingsButton.Click
            If LogTraceRadioButton.Checked Then
                LoggingLevel = LogLevel.Trace
            ElseIf LogDebugRadioButton.Checked Then
                LoggingLevel = LogLevel.Debug
            ElseIf LogInfoRadioButton.Checked Then
                LoggingLevel = LogLevel.Info
            ElseIf LogWarnRadioButton.Checked Then
                LoggingLevel = LogLevel.Warn
            ElseIf LogErrRadioButton.Checked Then
                LoggingLevel = LogLevel.Error
            ElseIf LogNoneRadioButton.Checked Then
                LoggingLevel = LogLevel.Off
            End If

            ResetLoggers()
            SettingsManager.LogLevel = LoggingLevel

            MinYield = ParseDouble(MinYieldTextBox.Text)
            MaxYield = ParseDouble(MaxYieldTextBox.Text)
            MinDur = ParseDouble(MinDurTextBox.Text)
            MaxDur = ParseDouble(MaxDurTextBox.Text)
            MinSpread = ParseDouble(MinSpreadTextBox.Text)
            MaxSpread = ParseDouble(MaxSpreadTextBox.Text)

            ShowBidAsk = ShowBidAskCheckBox.Checked
            ShowPointSize = ShowPointSizeCheckBox.Checked

            DefaultField = If(UseLastRadioButton.Checked, "LAST", "VWAP")

            ShowMainToolBar = MainWindowCheckBox.Checked
            ShowChartToolBar = ChartWindowCheckBox.Checked

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
    End Class
End Namespace