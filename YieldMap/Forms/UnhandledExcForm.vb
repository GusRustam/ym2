Imports System.IO
Imports YieldMap.Commons

Namespace Forms
    Public Class UnhandledExcForm
        Private Sub CloseButtonClick(sender As Object, e As EventArgs) Handles CloseButton.Click
            Close()
        End Sub

        Private Sub SendErrorReportButtonClick(sender As Object, e As EventArgs) Handles SendErrorReportButton.Click
            Try
                Dim logName = GetMyPath() + "\" + LogFileName
                Dim mail As New MAPI
                mail.AddRecipientTo("rustam.guseynov@thomsonreuters.com")
                If File.Exists(logName) Then mail.AddAttachment(logName)
                mail.SendMailPopup("Yield Map Error Report", ErrorTextBox.Text)
            Catch ex As Exception
                Clipboard.SetText(ErrorTextBox.Text)
                Process.Start("mailto:rustam.guseynov@thomsonreuters.com?subject=YieldMap%20Error&body=---Paste%20error%20info%20here---")
            End Try
        End Sub
    End Class
End Namespace