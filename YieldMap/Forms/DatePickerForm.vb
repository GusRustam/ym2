Namespace Forms
    Public Class DatePickerForm
        Private Sub DatePickerFormLoad(sender As System.Object, e As EventArgs) Handles MyBase.Load
            TheCalendar.MaxDate = Date.Today
            TheCalendar.SetDate(Date.Today)
            TheCalendar.MinDate = Date.Today.AddMonths(-6)
        End Sub
    End Class
End Namespace