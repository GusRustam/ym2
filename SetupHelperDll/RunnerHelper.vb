Imports System.IO

Public Class RunnerHelper

    Public Sub New()
        MyBase.New()

        'Этот вызов является обязательным для конструктора компонентов.
        InitializeComponent()

        'Добавить код инициализации после вызова InitializeComponent

    End Sub

    Protected Overrides Sub OnAfterInstall(ByVal savedState As IDictionary)
        MyBase.OnAfterInstall(savedState)
        Try
            Process.Start(Path.Combine(Context.Parameters("TargetDir"), Context.Parameters("Run")))
        Catch ex As Exception
            MsgBox(ex.ToString(), vbOKOnly, "Error")
        End Try
    End Sub
End Class
