Imports System.Reflection
Imports YieldMap.Commons

Namespace Forms.MainForm
    Public Class AboutForm
        Private Sub AboutFormLoad(sender As Object, e As EventArgs) Handles MyBase.Load
            BuildVerLabel.Text = Assembly.GetAssembly(GetType(Common)).GetName().Version.ToString()
            OsVerLabel.Text = Environment.OSVersion.VersionString
        End Sub

        Private Shared Sub SetLogPropsButtonClick(sender As Object, e As EventArgs) Handles SetLogPropsButton.Click
            Dim sf = New SettingsForm
            sf.ShowDialog()
        End Sub

        Private Shared Sub SendReportButtonClick(sender As Object, e As EventArgs) Handles SendReportButton.Click
            SendErrorReport("Yield Map Info", GetEnvironment())
        End Sub

        Private Sub CloseButtonClick(sender As Object, e As EventArgs) Handles CloseButton.Click
            Close()
        End Sub
    End Class
End Namespace