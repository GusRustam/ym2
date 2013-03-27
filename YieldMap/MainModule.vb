Imports System.Threading
Imports NLog
Imports Settings
Imports YieldMap.Commons
Imports YieldMap.Forms.MainForm
Imports YieldMap.My.Resources
Imports YieldMap.Forms

Module MainModule
    Private _mainForm As MainForm
    Private ReadOnly Logger As Logger = Logging.GetLogger(GetType(MainModule))

    Public ReadOnly Property AppMainForm() As MainForm
        Get
            Return _mainForm
        End Get
    End Property

    Public Sub Main()
        Logger.Trace("Main method started")
        Logging.LoggingLevel = SettingsManager.Instance.LogLevel

        ' Uninstall:  http://www.codeproject.com/Articles/11377/Add-an-uninstall-start-menu-item-to-your-NET-deplo
        Dim arguments As String() = Environment.GetCommandLineArgs()
        For Each guid As String In From argument In arguments
                                   Where argument.Split("=")(0).ToLower = "/u" Select argument.Split("=")(1)
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.System) & "\msiexec.exe", "/x " & guid)
            Application.Exit()
            End
        Next

        ' Error handling ' todo shitty handling ya'know
        AddHandler Application.ThreadException, New ThreadExceptionEventHandler(AddressOf ThreadEventHandler)
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException)

        AddHandler AppDomain.CurrentDomain.UnhandledException, New UnhandledExceptionEventHandler(AddressOf DomainEventHandler)

        '' Setting up db path
        'Dim installPath = Utils.GetMyPath()
        'AppDomain.CurrentDomain.SetData("DataDirectory", installPath)
        'My.Settings.Default("bondsConnectionString") = String.Format("data source=""{0}\bonds.sqlite""", installPath)

        _mainForm = New MainForm
        Application.Run(_mainForm)
    End Sub

    Private Sub DomainEventHandler(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)
        Dim frm As New UnhandledExcForm
        frm.Text = Unhandled_domain_exception
        frm.ErrorTextBox.Text = GetEnvironment() + Environment.NewLine + Environment.NewLine + e.ExceptionObject.ToString()
        frm.ShowDialog()
    End Sub

    Private Sub ThreadEventHandler(ByVal sender As Object, ByVal e As ThreadExceptionEventArgs)
        Dim frm As New UnhandledExcForm
        frm.Text = Unhandled_thread_exception
        frm.ErrorTextBox.Text = GetEnvironment() + Environment.NewLine + Environment.NewLine + e.Exception.ToString()
        frm.ShowDialog()
    End Sub
End Module