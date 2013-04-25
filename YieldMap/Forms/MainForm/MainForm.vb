Imports System.IO
Imports DbManager.Bonds
Imports ReutersData
Imports Settings
Imports Uitls
Imports YieldMap.My.Resources
Imports NLog
Imports Logging
Imports YieldMap.Commons


Namespace Forms.MainForm
    Public Class MainForm
        Private Shared ReadOnly Logger As Logger = GetLogger(GetType(MainForm))
        Private WithEvents _theSettings As SettingsManager = SettingsManager.Instance
        Private WithEvents _connector As EikonConnector = EikonConnector.Instance(Eikon.Sdk)
        Private WithEvents _ldr As IBondsLoader = BondsLoader.Instance()
        Private Shared _connected As Boolean
        Private _initialized As Boolean = False

        Public Property Initialized As Boolean
            Get
                Return _initialized
            End Get
            Set(ByVal value As Boolean)
                Logger.Warn("Initialized <- {0}", value)
                If value Then GuiAsync(Sub() StatusLabel.Text = Initialized_successfully)
                _initialized = value
                YieldMapButton.Enabled = value
                NewChartTSMI.Enabled = value
            End Set
        End Property

#Region "0. Settings and Loader Events"
        Sub ProgressHandler(ByVal message As ProgressEvent) Handles _ldr.Progress
            InitEventLabel.Text = message.Msg
            If message.Log.Success() Then
                Initialized = True
                InitEventLabel.Text = DatabaseUpdatedSuccessfully
                _theSettings.LastDbUpdate = Today
                RemoveHandler _ldr.Progress, AddressOf ProgressHandler
            ElseIf message.Log.Failed() Then
                InitEventLabel.Text = FailedToUpdateDatabase
                RemoveHandler _ldr.Progress, AddressOf ProgressHandler
            End If
        End Sub
#End Region

#Region "I. GUI Events"
        Private Sub TileHorTSBClick(ByVal sender As Object, ByVal e As EventArgs) Handles TileHorTSB.Click
            LayoutMdi(MdiLayout.TileVertical)
        End Sub

        Private Sub TileVerTSBClick(ByVal sender As Object, ByVal e As EventArgs) Handles TileVerTSB.Click
            LayoutMdi(MdiLayout.TileHorizontal)
        End Sub

        Private Shared Sub LogSettingsTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles LogSettingsTSMI.Click
            Dim sf = New SettingsForm
            sf.ShowDialog()
        End Sub

        Private Sub MainToolStripMouseDoubleClick(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MainToolStrip.MouseDoubleClick
            If e.Button = MouseButtons.Right Then
                CMS.Show(Me, e.Location)
            End If
        End Sub

        Private Sub ConnectButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles ConnectButton.Click
            StatusLabel.Text = Connecting_to_Eikon
            ConnectButton.Enabled = False
            ConnectTSMI.Enabled = False
            ConnectToEikon()
        End Sub

        Private Sub MainFormLoad(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            Logger.Info("MainFormLoad")
            MainToolStrip.Visible = _theSettings.ShowMainToolBar
            ToolbarTSMI.Checked = _theSettings.ShowChartToolBar
        End Sub

        Private Shared Sub DatabaseButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles DatabaseButton.Click, DatabaseManagerTSMI.Click
            Controller.PortfolioManager()
        End Sub

        Private Sub YieldMapButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles YieldMapButton.Click
            StartNewChart()
        End Sub

        Private Sub StartNewChart()
            Logger.Info("GraphButtonClick()")
            If Controller.NewChart(Me) = 1 Then LayoutMdi(MdiLayout.TileHorizontal)
        End Sub

        Private Shared Sub SettingsButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles SettingsButton.Click
            Dim sf = New SettingsForm
            sf.ShowDialog()
        End Sub

        Private Sub MainFormFormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
            Controller.CloseAllCharts()
        End Sub

        Private Shared Sub RaiseExcTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles RaiseExcTSMI.Click
            SendErrorReport("Yield Map Info", GetEnvironment())
        End Sub

        Private Shared Sub ShowLogTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles ShowLogTSMI.Click
            RunCommand(Path.Combine(LogFilePath, LogFileName))
        End Sub

        Private Shared Sub AboutTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles AboutTSMI.Click
            Dim af As New AboutForm
            af.ShowDialog()
        End Sub
#End Region

#Region "II. Connecting to Eikon"
        Private Sub ConnectToEikon()
            _connector.ConnectToEikon()
        End Sub

        Private Sub ConnectorConnected() Handles _connector.Connected
            ConnectTSMI.Enabled = False
            ConnectButton.Enabled = False
            StatusPicture.Image = Green
            StatusLabel.Text = Status_Connected
            _connected = True

            BondsData.Instance.Refresh()

            If Not _theSettings.LastDbUpdate.HasValue OrElse _theSettings.LastDbUpdate < Today Then
                _ldr.Initialize()
            End If
        End Sub

        Private Sub ConnectorDisconnected() Handles _connector.Disconnected
            _connected = False
            ConnectTSMI.Enabled = True
            ConnectButton.Enabled = True
            YieldMapButton.Enabled = False
            StatusPicture.Image = Red
            StatusLabel.Text = Status_Disconnected
            'Controller.CloseAllCharts()
        End Sub

        Public Shared ReadOnly Property Connected() As Boolean
            Get
                Return _connected
            End Get
        End Property

        Private Sub ConnectorLocalMode() Handles _connector.LocalMode
            _connected = True
            ConnectTSMI.Enabled = True
            ConnectButton.Enabled = True
            YieldMapButton.Enabled = False
            StatusPicture.Image = Orange
            StatusLabel.Text = Status_Local
        End Sub

        Private Sub ConnectorOffline() Handles _connector.Offline
            _connected = False
            ConnectTSMI.Enabled = True
            ConnectButton.Enabled = True
            YieldMapButton.Enabled = False
            StatusPicture.Image = Red
            StatusLabel.Text = Status_Offline
        End Sub
#End Region

        Private Sub ToolbarTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles ToolbarTSMI.Click
            MainToolStrip.Visible = ToolbarTSMI.Checked
        End Sub

        Private Shared Sub AboutMenuTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles AboutMenuTSMI.Click
            Dim af As New AboutForm
            af.ShowDialog()
        End Sub

        Private Sub ExitTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles ExitTSMI.Click
            Close()
        End Sub

        Private Sub ConnectTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles ConnectTSMI.Click
            StatusLabel.Text = Connecting_to_Eikon
            ConnectTSMI.Enabled = False
            ConnectButton.Enabled = False
            ConnectToEikon()
        End Sub

        Private Sub NewChartTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles NewChartTSMI.Click
            StartNewChart()
        End Sub

        Private Sub TileWindowsHorizontallyTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles TileWindowsHorizontallyTSMI.Click
            LayoutMdi(MdiLayout.TileHorizontal)
        End Sub

        Private Sub TileVerticallyTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles TileVerticallyTSMI.Click
            LayoutMdi(MdiLayout.TileVertical)
        End Sub

        Private Sub CascadeTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles CascadeTSMI.Click
            LayoutMdi(MdiLayout.Cascade)
        End Sub

        Private Shared Sub SettingsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SettingsToolStripMenuItem.Click
            Dim sf As New SettingsForm
            sf.ShowDialog()
        End Sub
    End Class
End Namespace