Imports System.Threading
Imports YieldMap.Forms.PortfolioForm
Imports YieldMap.Forms.ChartForm
Imports NLog
Imports Settings
Imports YieldMap.Forms.MainForm
Imports CommonController
Imports Uitls

Module MainModule
    Private _mainForm As MainForm
    Private ReadOnly Logger As Logger = Logging.GetLogger(GetType(MainModule))

    Public Class MainController
        Private ReadOnly _charts As New List(Of GraphForm)
        Private _portManForm As PortfolioForm
        Private _settingsForm As SettingsForm
        Private Shared _instance As MainController
        Private Shared ReadOnly ShutdownController As ShutdownController = ShutdownController.Instance

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
            AddHandler frm.Closed, Sub() _charts.Remove(frm)
            _charts.Add(frm)
            Return _charts.Count
        End Function

        Public Sub PortfolioManager()
            If _portManForm IsNot Nothing Then
                _portManForm.Activate()
                Return
            End If
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

        Public Sub Shutdown()
            CloseAllCharts()
            ShutdownController.Shutdown()
            CoUninitialize()
            Application.Exit()
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

        ' Error handling 
        AddHandler Application.ThreadException, New ThreadExceptionEventHandler(AddressOf ThreadEventHandler)
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException)
        AddHandler AppDomain.CurrentDomain.UnhandledException, New UnhandledExceptionEventHandler(AddressOf DomainEventHandler)

        _mainForm = New MainForm
        Application.Run(_mainForm)
    End Sub

    Private Sub DomainEventHandler(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)
        Logger.Fatal("Domain error {0}", e.ExceptionObject.ToString())
        ' ReSharper disable VBPossibleMistakenCallToGetType.2
        If MessageBox.Show(String.Format("Unhandled exception of type {0} occured.{1}Would you like to close the application?", e.ExceptionObject.GetType(), Environment.NewLine), "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) = MsgBoxResult.Yes Then
            ' ReSharper restore VBPossibleMistakenCallToGetType.2
            Controller.Shutdown()
        End If
    End Sub

    Private Sub ThreadEventHandler(ByVal sender As Object, ByVal e As ThreadExceptionEventArgs)
        Logger.Fatal("Thread error {0}", e.Exception.ToString())
        If MessageBox.Show(String.Format("Unhandled exception {0} occured.{1}Would you like to close the application?", e.Exception.GetType(), Environment.NewLine), "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) = MsgBoxResult.Yes Then
            Controller.Shutdown()
        End If
    End Sub
End Module