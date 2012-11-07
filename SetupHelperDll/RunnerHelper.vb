Imports System.ComponentModel
Imports System.Configuration.Install
Imports System.Windows.Forms
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
        Dim fileName As String = Context.Parameters("Run")
        Dim targetFolder As String = Context.Parameters("TargetDir")
        MessageBox.Show("fileName = " + fileName)
        MessageBox.Show("targetFolder = " + targetFolder)
        Process.Start(Path.Combine(targetFolder, fileName))
    End Sub
End Class
