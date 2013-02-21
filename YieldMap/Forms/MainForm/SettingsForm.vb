Imports System.Text.RegularExpressions
Imports NLog
Imports Settings

Namespace Forms.MainForm
    Public Class SettingsForm
        Private Sub SettingsFormLoad(sender As System.Object, e As EventArgs) Handles MyBase.Load
            Select Case SettingsManager.LogLevel
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

            
            MainWindowCheckBox.Checked = ShowMainToolBar
            ChartWindowCheckBox.Checked = ShowChartToolBar

            Dim selectedFields = BondSelectorVisibleColumns.Split(",")
            ColumnsCLB.Items.Clear()
            For Each nd As NameDescr In NameDescr.AllNames
                Dim idx = ColumnsCLB.Items.Add(nd)
                If selectedFields.Contains(nd.Field) Then
                    ColumnsCLB.SetItemCheckState(idx, CheckState.Checked)
                End If
            Next
            If selectedFields.Contains("ALL") Then
                AllColumnsCB.Checked = True
                SetAllCheckState()
            End If
        End Sub

        Private Sub CancelButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles TheCancelButton.Click
            Close()
        End Sub

        Private Class NameDescr
            Public Shared ReadOnly AllNames = {New NameDescr("Bond name", "bondshortname"),
                               New NameDescr("Description", "descr"),
                               New NameDescr("RIC", "ric"),
                               New NameDescr("Issuer", "issname"),
                               New NameDescr("Issue date", "issuedate"),
                               New NameDescr("Issue size", "issue_size"),
                               New NameDescr("Maturity", "maturitydate"),
                               New NameDescr("Coupon", "coupon"),
                               New NameDescr("Currency", "currency"),
                               New NameDescr("Next put", "nextputdate"),
                               New NameDescr("Next call", "nextcalldate")}

            Private ReadOnly _descr As String
            Public ReadOnly Field As String

            Private Sub New(ByVal descr As String, ByVal field As String)
                _descr = descr
                Me.Field = field
            End Sub

            Public Overrides Function ToString() As String
                Return _descr
            End Function
        End Class

        Private Sub SaveSettingsButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles SaveSettingsButton.Click
            If LogTraceRadioButton.Checked Then
                SettingsManager.LogLevel = LogLevel.Trace
            ElseIf LogDebugRadioButton.Checked Then
                SettingsManager.LogLevel = LogLevel.Debug
            ElseIf LogInfoRadioButton.Checked Then
                SettingsManager.LogLevel = LogLevel.Info
            ElseIf LogWarnRadioButton.Checked Then
                SettingsManager.LogLevel = LogLevel.Warn
            ElseIf LogErrRadioButton.Checked Then
                SettingsManager.LogLevel = LogLevel.Error
            ElseIf LogNoneRadioButton.Checked Then
                SettingsManager.LogLevel = LogLevel.Off
            End If

            Logging.ResetLoggers()
            SettingsManager.LogLevel = SettingsManager.LogLevel

            MinYield = ParseDouble(MinYieldTextBox.Text)
            MaxYield = ParseDouble(MaxYieldTextBox.Text)
            MinDur = ParseDouble(MinDurTextBox.Text)
            MaxDur = ParseDouble(MaxDurTextBox.Text)
            MinSpread = ParseDouble(MinSpreadTextBox.Text)
            MaxSpread = ParseDouble(MaxSpreadTextBox.Text)

            ShowBidAsk = ShowBidAskCheckBox.Checked
            ShowPointSize = ShowPointSizeCheckBox.Checked

            ShowMainToolBar = MainWindowCheckBox.Checked
            ShowChartToolBar = ChartWindowCheckBox.Checked

            Dim columnsString As String = ""

            If AllColumnsCB.CheckState = CheckState.Checked Then
                columnsString = "ALL"
            Else
                For i = 0 To ColumnsCLB.Items.Count - 1
                    If ColumnsCLB.GetItemCheckState(i) = CheckState.Checked Then
                        columnsString = columnsString & CType(ColumnsCLB.Items(i), NameDescr).Field & ","
                    End If
                Next
            End If
            If AllColumnsCB.CheckState <> CheckState.Checked And columnsString <> "" Then
                columnsString = columnsString.Substring(0, columnsString.Length() - 1)
            End If
            BondSelectorVisibleColumns = columnsString

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