Imports EikonDesktopDataAPILib
Imports System.Threading
Imports NLog

Public Class Eikon
    Private Shared _myEikonDesktopSdk As New EikonDesktopDataAPI

    Private Sub New()
    End Sub

    Public Shared ReadOnly Property Sdk As EikonDesktopDataAPI
        Get
            Return _myEikonDesktopSdk
        End Get
    End Property

    Public Sub Clear()
        _myEikonDesktopSdk = Nothing
    End Sub
End Class

Public Class EikonConnector
    Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(EikonConnector))
    Private WithEvents _sdk As EikonDesktopDataAPI
    Private Shared _instance As EikonConnector

    Private _timeOutFlag As Long = 0

    Public Event Connected As Action
    Public Event Disconnected As Action
    Public Event LocalMode As Action
    Public Event Offline As Action
    Public Event Timeout As Action

    Private Sub New(ByVal sdk As EikonDesktopDataAPI)
        _sdk = sdk
    End Sub

    Public Sub ConnectToEikon()
        Dim lResult = _sdk.Initialize()
        If lResult <> EEikonDataAPIInitializeResult.Succeed Then
            Logger.Warn("Failed to connect, result is {0}", lResult)
            RaiseEvent Disconnected()
        End If

        ThreadPool.QueueUserWorkItem(
            Sub()
                Logger.Info("Connection waiter started")
                Thread.Sleep(TimeSpan.FromSeconds(20))
                Logger.Info("Connection wait finished")
                If Interlocked.Read(_timeOutFlag) = 0 Then
                    Logger.Warn("Connection was unsuccessful")
                    Interlocked.Increment(_timeOutFlag)
                    RaiseEvent Timeout()
                Else
                    Logger.Info("Connection was successful")
                End If
            End Sub)
    End Sub

    Public Sub OnStatusChanged(ByVal eStatus As EEikonStatus) Handles _sdk.OnStatusChanged
        If Interlocked.Read(_timeOutFlag) = 0 Then
            Logger.Warn("Connection message arrived after timeout, status = {0}", eStatus)
            Return
        End If
        Interlocked.Increment(_timeOutFlag)
        Select Case eStatus
            Case EEikonStatus.Connected
                RaiseEvent Connected()
            Case EEikonStatus.Disconnected
                RaiseEvent Disconnected()
            Case EEikonStatus.LocalMode
                RaiseEvent LocalMode()
            Case EEikonStatus.Offline
                RaiseEvent Offline()
        End Select
    End Sub


    Public Shared ReadOnly Property Instance(ByVal sdk As EikonDesktopDataAPI) As EikonConnector
        Get
            If _instance Is Nothing Then _instance = New EikonConnector(sdk)
            Return _instance
        End Get
    End Property

    Public Shared ReadOnly Property Instance() As EikonConnector
        Get
            Return _instance
        End Get
    End Property

End Class