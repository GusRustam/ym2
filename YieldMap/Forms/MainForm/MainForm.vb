Imports System.IO
Imports DbManager.Bonds
Imports ReutersData
Imports Settings
Imports YieldMap.Commons
Imports YieldMap.My.Resources
Imports YieldMap.Forms.ChartForm
Imports NLog
Imports Logging

Namespace Forms.MainForm
    Public Class MainForm
        Private Shared ReadOnly Logger As Logger = GetLogger(GetType(MainForm))
        Private WithEvents _theSettings As SettingsManager = SettingsManager.Instance

        Private _initialized As Boolean = False
        Private ReadOnly _graphs As New List(Of GraphForm)

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
            PortfolioForm.PortfolioForm.Show()
        End Sub

        Private Sub YieldMapButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles YieldMapButton.Click
            StartNewChart()
        End Sub

        Private Sub StartNewChart()
            Logger.Info("GraphButtonClick()")
            Dim graphForm = New GraphForm
            graphForm.MdiParent = Me
            graphForm.Show()
            AddHandler graphForm.Closed, AddressOf GraphFormRemoved
            _graphs.Add(graphForm)
            If _graphs.Count = 1 Then LayoutMdi(MdiLayout.TileHorizontal)
        End Sub

        Private Sub GraphFormRemoved(ByVal sender As Object, ByVal e As EventArgs)
            Dim gf As GraphForm = TryCast(sender, GraphForm)
            If gf IsNot Nothing Then
                AddHandler gf.Closed, AddressOf GraphFormRemoved
                _graphs.Remove(gf)
            End If
        End Sub

        Private Shared Sub SettingsButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles SettingsButton.Click
            Dim sf = New SettingsForm
            sf.ShowDialog()
        End Sub

        Private Sub MainFormFormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
            CloseAllGraphForms()
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
        Private WithEvents _connector As New EikonConnector(Eikon.Sdk)

        Private Sub ConnectToEikon()
            _connector.ConnectToEikon()
        End Sub

        Private Sub CloseAllGraphForms()
            While _graphs.Any
                _graphs.First.Close()
            End While
        End Sub

        Private Sub ConnectorConnected() Handles _connector.Connected
            ConnectTSMI.Enabled = False
            ConnectButton.Enabled = False
            StatusPicture.Image = Green
            StatusLabel.Text = Status_Connected

            Dim loader As IBondsLoader = New BondsLoader '.Instance()
            AddHandler loader.Progress, Sub(message As ProgressEvent)
                                            InitEventLabel.Text = message.Msg
                                            If message.Log.Success() Then
                                                Initialized = True
                                                InitEventLabel.Text = DatabaseUpdatedSuccessfully
                                                _theSettings.LastDbUpdate = Today
                                            ElseIf message.Log.Failed() Then
                                                InitEventLabel.Text = FailedToUpdateDatabase
                                            End If
                                        End Sub
            If Not _theSettings.LastDbUpdate.HasValue OrElse _theSettings.LastDbUpdate < Today Then
                loader.Initialize()
            End If
        End Sub

        Private Sub ConnectorDisconnected() Handles _connector.Disconnected
            ConnectTSMI.Enabled = True
            ConnectButton.Enabled = True
            YieldMapButton.Enabled = False
            StatusPicture.Image = Red
            StatusLabel.Text = Status_Disconnected
            CloseAllGraphForms()
        End Sub

        Private Sub ConnectorLocalMode() Handles _connector.LocalMode
            ConnectTSMI.Enabled = True
            ConnectButton.Enabled = True
            YieldMapButton.Enabled = False
            StatusPicture.Image = Orange
            StatusLabel.Text = Status_Local
        End Sub

        Private Sub ConnectorOffline() Handles _connector.Offline
            ConnectTSMI.Enabled = True
            ConnectButton.Enabled = True
            YieldMapButton.Enabled = False
            StatusPicture.Image = Red
            StatusLabel.Text = Status_Offline
        End Sub
#End Region

        Private Sub ToolbarTSMIClick(sender As Object, e As EventArgs) Handles ToolbarTSMI.Click
            MainToolStrip.Visible = ToolbarTSMI.Checked
        End Sub

        Private Shared Sub AboutMenuTSMIClick(sender As Object, e As EventArgs) Handles AboutMenuTSMI.Click
            Dim af As New AboutForm
            af.ShowDialog()
        End Sub

        Private Sub ExitTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles ExitTSMI.Click
            Close()
        End Sub

        Private Sub ConnectTSMIClick(sender As Object, e As EventArgs) Handles ConnectTSMI.Click
            StatusLabel.Text = Connecting_to_Eikon
            ConnectTSMI.Enabled = False
            ConnectButton.Enabled = False
            ConnectToEikon()
        End Sub

        Private Sub NewChartTSMIClick(sender As Object, e As EventArgs) Handles NewChartTSMI.Click
            StartNewChart()
        End Sub

        Private Sub TileWindowsHorizontallyTSMIClick(sender As Object, e As EventArgs) Handles TileWindowsHorizontallyTSMI.Click
            LayoutMdi(MdiLayout.TileHorizontal)
        End Sub

        Private Sub TileVerticallyTSMIClick(sender As Object, e As EventArgs) Handles TileVerticallyTSMI.Click
            LayoutMdi(MdiLayout.TileVertical)
        End Sub

        Private Sub CascadeTSMIClick(sender As Object, e As EventArgs) Handles CascadeTSMI.Click
            LayoutMdi(MdiLayout.Cascade)
        End Sub

        Private Shared Sub SettingsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SettingsToolStripMenuItem.Click
            Dim sf As New SettingsForm
            sf.ShowDialog()
        End Sub
    End Class
End Namespace