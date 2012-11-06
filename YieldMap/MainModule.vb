Imports System.Threading
Imports YieldMap.Forms

Module MainModule
    Public Sub Main()
        AddHandler Application.ThreadException, New ThreadExceptionEventHandler(AddressOf ThreadEventHandler)
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException)

        AddHandler AppDomain.CurrentDomain.UnhandledException,
            New UnhandledExceptionEventHandler(AddressOf DomainEventHandler)

        Dim installPath = Commons.GetMyPath()
        AppDomain.CurrentDomain.SetData("DataDirectory", installPath)
        My.Settings.Default("bondsConnectionString") = String.Format("data source=""{0}\bonds.sqlite""", installPath)

        Application.Run(New MainForm)
    End Sub

    Private Sub DomainEventHandler(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)
        Dim frm As New UnhandledExcForm
        frm.Text = "Unhandled domain exception"
        frm.ErrorTextBox.Text = GetEnvironment() + Environment.NewLine + e.ExceptionObject.ToString()
        frm.ShowDialog()
    End Sub

    Private Function GetEnvironment() As String
        Dim res As String = ""
        res += String.Format("CommandLine = {0}", Environment.CommandLine) + Environment.NewLine
        res += String.Format("CurrentDirectory = {0}", Environment.CurrentDirectory) + Environment.NewLine
        res += String.Format("Is64BitOperatingSystem = {0}", Environment.Is64BitOperatingSystem) + Environment.NewLine
        res += String.Format("Is64BitProcess = {0}", Environment.Is64BitProcess) + Environment.NewLine
        res += String.Format("MachineName = {0}", Environment.MachineName) + Environment.NewLine
        res += String.Format("OSVersion = {0}", Environment.OSVersion) + Environment.NewLine
        res += String.Format("SystemDirectory = {0}", Environment.SystemDirectory) + Environment.NewLine
        res += String.Format("UserDomainName = {0}", Environment.UserDomainName) + Environment.NewLine
        res += String.Format("UserName = {0}", Environment.UserName) + Environment.NewLine
        res += String.Format("Version = {0}", Environment.Version) + Environment.NewLine
        Return res
    End Function

    Private Sub ThreadEventHandler(ByVal sender As Object, ByVal e As ThreadExceptionEventArgs)
        Dim frm As New UnhandledExcForm
        frm.Text = "Unhandled thread exception"
        frm.ErrorTextBox.Text = GetEnvironment() + Environment.NewLine + e.Exception.ToString()
        frm.ShowDialog()
    End Sub

End Module
