Namespace Forms
    Public Class DatePickerForm
        Private Sub DatePickerFormLoad(sender As System.Object, e As EventArgs) Handles MyBase.Load
            TheCalendar.MaxDate = Date.Today
        End Sub
    End Class
End Namespace