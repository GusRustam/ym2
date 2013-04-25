Imports System.Threading
Imports YieldMap.Forms.PortfolioForm
Imports YieldMap.Forms.ChartForm
Imports NLog
Imports Settings
Imports YieldMap.Commons
Imports YieldMap.Forms.MainForm
Imports YieldMap.My.Resources
Imports YieldMap.Forms

Module MainModule
    Private _mainForm As MainForm
    Private ReadOnly Logger As Logger = Logging.GetLogger(GetType(MainModule))

    Public Class MainController
        Private ReadOnly _charts As New List(Of GraphForm)
        Private _portManForm As PortfolioForm
        Private _settingsForm As SettingsForm
        Private Shared _instance As MainController

        Public Shared ReadOnly Property Instance() As MainController
            Get
                If _instance Is Nothing Then _instance = New MainController()
                Return _instance
            End Get
        End Property

        Public Sub CloseAllCharts()
            While _charts.Any
                _charts.First.Close()
            End While
        End Sub

        Public Function NewChart(ByVal parent As Form) As Integer
            Dim frm As New GraphForm
            If parent IsNot Nothing Then frm.MdiParent = parent
            frm.Show()
            AddHandler frm.Closed, Sub() _charts.Remove(frm) ' todo equality members for forms
            _charts.Add(frm)
            Return _charts.Count
        End Function

        Public Sub PortfolioManager()
            If _portManForm IsNot Nothing Then _portManForm.Activate()
            _portManForm = New PortfolioForm()
            _portManForm.Show()
            AddHandler _portManForm.Closed, Sub() _portManForm = Nothing
        End Sub

        Public Sub SettingsManager()
            If _settingsForm IsNot Nothing Then _settingsForm.Activate()
            _settingsForm = New SettingsForm()
            _settingsForm.Show()
            AddHandler _settingsForm.Closed, Sub() _settingsForm = Nothing
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public Controller As MainController = MainController.Instance

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