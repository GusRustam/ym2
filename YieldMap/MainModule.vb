Imports System.Threading
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Reflection
Imports System.ComponentModel
Imports DbManager
Imports Logging
Imports YieldMap.Forms.PortfolioForm
Imports YieldMap.Forms.ChartForm
Imports NLog
Imports Settings
Imports YieldMap.Forms.MainForm
Imports CommonController
Imports Uitls
Imports Ionic.Zip

Module MainModule
    Private _mainForm As MainForm
    Private ReadOnly Logger As Logger = GetLogger(GetType(MainModule))

    Public Enum UserMessageLevel
        Usual
        Warning
        Failure
    End Enum

    Private _userMessageCounter As Long = 0

    Public Class UserMessageItem
        Private ReadOnly _id As Long
        Private ReadOnly _level As UserMessageLevel
        Private ReadOnly _time As DateTime
        Private ReadOnly _source As String
        Private ReadOnly _message As String
        Private ReadOnly _details As String

        Public Sub New(ByVal level As UserMessageLevel, ByVal source As String, ByVal message As String, Optional ByVal details As String = "")
            _id = _userMessageCounter
            _details = details
            _level = level
            _message = message
            _source = source
            _time = DateTime.Now

            _userMessageCounter += 1
        End Sub

        <Browsable(False)>
        Public ReadOnly Property ID() As Long
            Get
                Return _id
            End Get
        End Property

        Public ReadOnly Property Level() As UserMessageLevel
            Get
                Return _level
            End Get
        End Property

        Public ReadOnly Property Time() As Date
            Get
                Return _time
            End Get
        End Property

        Public ReadOnly Property Source() As String
            Get
                Return _source
            End Get
        End Property

        Public ReadOnly Property Message() As String
            Get
                Return _message
            End Get
        End Property

        Public ReadOnly Property Details() As String
            Get
                Return _details
            End Get
        End Property
    End Class

    Public Class MainController
        Private ReadOnly _charts As New List(Of GraphForm)
        Private _portManForm As PortfolioForm
        Private _settingsForm As SettingsForm
        Private Shared _instance As MainController
        Private Shared ReadOnly ShutdownController As ShutdownController = ShutdownController.Instance

        Public Shared Event UserLogUpdated As Action(Of Guid)
        Public Shared ReadOnly GlobalMessage As Guid = Guid.NewGuid()
        Private Shared ReadOnly UserMessageLog As New Dictionary(Of Guid, List(Of UserMessageItem))

        Public Shared Sub LogMessage(ByVal source As Guid, ByVal msg As UserMessageItem)
            If Not UserMessageLog.ContainsKey(source) Then UserMessageLog.Add(source, New List(Of UserMessageItem))
            UserMessageLog(source).Add(msg)
            RaiseEvent UserLogUpdated(source)
        End Sub

        Public Shared Sub ClearUserLog(Optional ByVal source As Guid? = Nothing)
            If Not source.HasValue Then
                UserMessageLog.Clear()
            Else
                If Not UserMessageLog.ContainsKey(source.Value) Then Return
                UserMessageLog.Remove(source.Value)
            End If
        End Sub

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

        Public Sub SettingsManager(Optional ByVal page As String = Nothing)
            If _settingsForm IsNot Nothing Then _settingsForm.Activate()
            _settingsForm = New SettingsForm()
            _settingsForm.Show()
            If page IsNot Nothing Then _settingsForm.MainTabControl.SelectTab(page)
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
        LoggingLevel = SettingsManager.Instance.LogLevel

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

    Public Function GetMyPath() As String
        Dim installPath = Path.GetDirectoryName(Assembly.GetAssembly(GetType(MainModule)).CodeBase)
        Return installPath.Substring(6)
    End Function

    Public Sub ZipAndAttachFiles(ByVal mail As MAPI)
        Using zip As New ZipFile
            Dim logName = Path.Combine(LogFilePath, LogFileName)
            If File.Exists(logName) Then zip.AddFile(logName, "")

            Dim dbName = Path.Combine(GetMyPath(), PortfolioManager.Instance.ConfigFile())
            If File.Exists(dbName) Then zip.AddFile(dbName, "")

            Dim timestampStr = Date.Now.ToString("yyyy-MM-dd hh-mm-ss")
            Dim num As Integer
            Screen.AllScreens.ToList().ForEach(
                  Sub(screen)
                      Dim bmpScreenshot = New Bitmap(screen.Bounds.Width, screen.Bounds.Height, PixelFormat.Format32bppArgb)
                      Dim gfxScreenshot = Graphics.FromImage(bmpScreenshot)
                      gfxScreenshot.CopyFromScreen(screen.Bounds.X, screen.Bounds.Y, 0, 0, screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy)
                      bmpScreenshot.Save(Path.Combine(LogFilePath, String.Format("{0}_{1}.png", timestampStr, num)), ImageFormat.Png)
                      num += 1
                  End Sub)

            For i = 0 To num - 1
                Dim fName = Path.Combine(LogFilePath, String.Format("{0}_{1}.png", timestampStr, i))
                If File.Exists(fName) Then zip.AddFile(fName, "")
            Next

            Dim dbZipName = Path.Combine(LogFilePath, ZipFileName)
            zip.Save(dbZipName)
            If File.Exists(dbZipName) Then mail.AddAttachment(dbZipName)
        End Using
    End Sub

    Public Sub SendErrorReport(ByVal header As String, ByVal message As String)
        Try
            Dim mail As New MAPI
            mail.AddRecipientTo("rustam.guseynov@thomsonreuters.com")
            ZipAndAttachFiles(mail)

            mail.SendMailPopup(header, message)
        Catch ex As Exception
            Clipboard.SetText(message)
            Utils.RunCommand("mailto:rustam.guseynov@thomsonreuters.com?subject=YieldMap%20Error&body=---Paste%20error%20info%20here---", "")
        End Try
    End Sub
End Module