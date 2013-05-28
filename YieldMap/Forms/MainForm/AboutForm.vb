Imports System.Reflection

Namespace Forms.MainForm
    Public Class AboutForm
        Private Sub AboutFormLoad(sender As Object, e As EventArgs) Handles MyBase.Load
            BuildVerLabel.Text = Assembly.GetAssembly(GetType(AboutForm)).GetName().Version.ToString()
            OsVerLabel.Text = Environment.OSVersion.VersionString
        End Sub

        'Private Shared Sub SendReportButtonClick(sender As Object, e As EventArgs) Handles SendReportButton.Click
        '    SendErrorReport("Yield Map Info", GetEnvironment())
        'End Sub

        Private Sub CloseButtonClick(sender As Object, e As EventArgs) Handles CloseButton.Click
            Close()
        End Sub
    End Class
End Namespace