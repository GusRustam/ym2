Imports System.IO
Imports YieldMap.Commons
Imports YieldMap.My.Resources
Imports YieldMap.Forms.ChartForm
Imports YieldMap.Forms.PortfolioForm
Imports EikonDesktopSDKLib
Imports NLog

Namespace Forms
    Public Class MainForm
        Private Shared ReadOnly Logger As Logger = GetLogger(GetType(MainForm))

        Private WithEvents _myEikonDesktopSdk As EikonDesktopSDK = Eikon.SDK


        Private _initialized As Boolean = False
        Private ReadOnly _graphs As New List(Of GraphForm)

        Public Property Initialized As Boolean
            Get
                Return _initialized
            End Get
            Set(value As Boolean)
                Logger.Warn("Initialized <- {0}", value)
                If value Then GuiAsync(Sub() StatusLabel.Text = Initialized_successfully)
                _initialized = value
                YieldMapButton.Enabled = value
            End Set
        End Property

#Region "I. GUI Events"
        Private Sub TileHorTSBClick(sender As Object, e As EventArgs) Handles TileHorTSB.Click
            LayoutMdi(MdiLayout.TileVertical)
        End Sub

        Private Sub TileVerTSBClick(sender As Object, e As EventArgs) Handles TileVerTSB.Click
            LayoutMdi(MdiLayout.TileHorizontal)
        End Sub

        Private Shared Sub LogSettingsTSMIClick(sender As Object, e As EventArgs) Handles LogSettingsTSMI.Click
            Dim sf = New SettingsForm
            sf.ShowDialog()
        End Sub

        Private Sub MainToolStripMouseDoubleClick(sender As Object, e As MouseEventArgs) Handles MainToolStrip.MouseDoubleClick
            If e.Button = MouseButtons.Right Then
                CMS.Show(Me, e.Location)
            End If
        End Sub

        Private Sub ConnectButtonClick(sender As Object, e As EventArgs) Handles ConnectButton.Click
            StatusLabel.Text = Connecting_to_Eikon
            ConnectButton.Enabled = False
            ConnectToEikon()
        End Sub

        Private Shared Sub MainFormLoad(sender As Object, e As EventArgs) Handles MyBase.Load
            Logger.Info("MainFormLoad")
        End Sub


        Private Shared Sub DatabaseButtonClick(sender As Object, e As EventArgs) Handles DatabaseButton.Click
            Logger.Info("DatabaseButtonClick()")
            Dim managerForm = New DataBaseManagerForm
            managerForm.ShowDialog()
        End Sub

        Private Sub YieldMapButtonClick(sender As Object, e As EventArgs) Handles YieldMapButton.Click
            Logger.Info("GraphButtonClick()")
            Dim graphForm = New GraphForm
            graphForm.MdiParent = Me
            graphForm.Show()
            AddHandler graphForm.Closed, AddressOf GraphFormRemoved
            _graphs.Add(graphForm)
        End Sub

        Private Sub GraphFormRemoved(ByVal sender As Object, ByVal e As EventArgs)
            Dim gf As GraphForm = TryCast(sender, GraphForm)
            If gf IsNot Nothing Then
                AddHandler gf.Closed, AddressOf GraphFormRemoved
                _graphs.Remove(gf)
            End If
        End Sub

        Private Shared Sub SettingsButtonClick(sender As Object, e As EventArgs) Handles SettingsButton.Click
            Dim sf = New SettingsForm
            sf.ShowDialog()
        End Sub

        Private Shared Sub MainFormFormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
            Eikon.Instance.Clear()
        End Sub

        Private Shared Sub RaiseExcTSMIClick(sender As Object, e As EventArgs) Handles RaiseExcTSMI.Click
            SendErrorReport("Yield Map Info", GetEnvironment())
        End Sub

        Private Shared Sub ShowLogTSMIClick(sender As Object, e As EventArgs) Handles ShowLogTSMI.Click
            RunCommand(Path.Combine(LogFilePath, LogFileName))
        End Sub

        Private Shared Sub AboutTSMIClick(sender As Object, e As EventArgs) Handles AboutTSMI.Click
            Dim af As New AboutForm
            af.ShowDialog()
        End Sub
#End Region

#Region "II. Connecting to Eikon"
        Private Sub ConnectToEikon()
            Dim lResult = _myEikonDesktopSdk.Initialize()
            If lResult <> EEikonSDKInitializeResult.Succeed Then
                Select Case lResult
                    Case EEikonSDKInitializeResult.Error_Reinitialize
                        StatusLabel.Text = Reinit_Eikon_forbidden

                    Case EEikonSDKInitializeResult.Error_InitializeFail
                        StatusLabel.Text = Init_Eikon_Fail
                End Select
                UpdateUserFormAccordingToConnectionStatus(EEikonStatus.Disconnected)
            End If
        End Sub

        Private Sub UpdateUserFormAccordingToConnectionStatus(ByVal eEikonStatus As EEikonStatus)
            Select Case eEikonStatus
                Case eEikonStatus.Connected
                    ConnectButton.Enabled = False
                    StatusPicture.Image = Green
                    StatusLabel.Text = Status_Connected

                Case eEikonStatus.Disconnected
                    ConnectButton.Enabled = True
                    YieldMapButton.Enabled = False
                    StatusPicture.Image = Red
                    StatusLabel.Text = Status_Disconnected
                    CloseAllGraphForms()

                Case eEikonStatus.LocalMode
                    ConnectButton.Enabled = True
                    ConnectButton.Enabled = False
                    StatusPicture.Image = Orange
                    StatusLabel.Text = Status_Local

                Case eEikonStatus.Offline
                    ConnectButton.Enabled = True
                    ConnectButton.Enabled = False
                    StatusPicture.Image = Red
                    StatusLabel.Text = Status_Offline
            End Select
        End Sub

        Private Sub CloseAllGraphForms()
            While _graphs.Any
                _graphs.First.Close()
            End While
        End Sub

        Public Sub OnStatusChanged(ByVal eStatus As EEikonStatus) Handles _myEikonDesktopSdk.OnStatusChanged
            UpdateUserFormAccordingToConnectionStatus(eStatus)
            If eStatus = EEikonStatus.Connected Then
                Dim initR = New DbInitializer
                AddHandler initR.Success, Sub()
                                              Initialized = True
                                              InitEventLabel.Text = "Database updated successfully"
                                          End Sub
                AddHandler initR.Failure, Sub(ex As Exception)
                                              Initialized = False
                                              InitEventLabel.Text = "Failed to update database"
                                              If MsgBox("Failed to initialize database. Would you like to report an error to the developer?", vbYesNo, "Database error") = vbYes Then
                                                  SendErrorReport("Yield Map Database Error", "Exception: " + ex.ToString() + Environment.NewLine + Environment.NewLine + GetEnvironment())
                                              End If
                                          End Sub
                AddHandler initR.Progress, Sub(message) GuiAsync(Sub() InitEventLabel.Text = message)
                initR.UpdateDatabase()
            End If
        End Sub
#End Region

    End Class
End Namespace