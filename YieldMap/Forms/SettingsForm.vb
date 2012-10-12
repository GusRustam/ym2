Imports YieldMap.Commons
Imports NLog

Namespace Forms
    Public Class SettingsForm
        Private Sub SettingsFormLoad(sender As System.Object, e As EventArgs) Handles MyBase.Load
            Select Case LoggingLevel
                Case LogLevel.Trace : LogTraceRadioButton.Checked = True
                Case LogLevel.Debug : LogDebugRadioButton.Checked = True
                Case LogLevel.Info : LogInfoRadioButton.Checked = True
                Case LogLevel.Warn : LogWarnRadioButton.Checked = True
                Case LogLevel.Error : LogErrRadioButton.Checked = True
                Case LogLevel.Off : LogNoneRadioButton.Checked = True
            End Select

            MaxYieldTextBox.Text = MaxYield
            MaxDurTextBox.Text = MaxDur
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

            SetLoggingLevel()
            SettingsManager.LogLevel = LoggingLevel

            Dim minYldTxtx = MinYieldTextBox.Text
            If minYldTxtx.Length > 0 Then
                If IsNumeric(minYldTxtx) Then
                    MinYield = Double.Parse(minYldTxtx)
                Else
                    MsgBox("minimum Yield is not a number. Set a numeric value or empty the cell in order to save")
                    Exit Sub
                End If
            End If


            Dim maxYldTxtx = MaxYieldTextBox.Text
            If maxYldTxtx.Length > 0 Then
                If IsNumeric(maxYldTxtx) Then
                    MaxYield = Double.Parse(maxYldTxtx)
                Else
                    MsgBox("Maximum Yield is not a number. Set a numeric value or empty the cell in order to save")
                    Exit Sub
                End If
            End If

            Dim minDurTxt = MinDurTextBox.Text
            If minDurTxt.Length > 0 Then
                If IsNumeric(minDurTxt) Then
                    MinDur = Double.Parse(minDurTxt)
                Else
                    MsgBox("minimum Duration is not a number. Set a numeric value or empty the cell in order to save")
                    Exit Sub
                End If
            End If

            Dim maxDurTxt = MaxDurTextBox.Text
            If maxDurTxt.Length > 0 Then
                If IsNumeric(maxDurTxt) Then
                    MaxDur = Double.Parse(maxDurTxt)
                Else
                    MsgBox("Maximum Duration is not a number. Set a numeric value or empty the cell in order to save")
                    Exit Sub
                End If
            End If
            Close()
        End Sub
    End Class
End Namespace