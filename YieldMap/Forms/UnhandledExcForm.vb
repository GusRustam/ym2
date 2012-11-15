Imports YieldMap.Commons

Namespace Forms
    Public Class UnhandledExcForm
        Private Sub CloseButtonClick(sender As Object, e As EventArgs) Handles CloseButton.Click
            Close()
        End Sub

        Private Sub SendErrorReportButtonClick(sender As Object, e As EventArgs) Handles SendErrorReportButton.Click
            SendErrorReport("Yield Map Error Report", ErrorTextBox.Text)
        End Sub
    End Class
End Namespace