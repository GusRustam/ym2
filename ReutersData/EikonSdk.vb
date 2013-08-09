Imports EikonDesktopDataAPILib
Imports NLog
Imports CommonController
Imports System.Runtime.InteropServices
Imports System.Threading

Public Class Eikon
    Private Shared _eikon As Eikon
    Private ReadOnly _myEikonDesktopSdk As EikonDesktopDataAPI
    Private WithEvents _shutdownManager As ShutdownController = ShutdownController.Instance

    ReadOnly Property MyEikonDesktopSdk() As EikonDesktopDataAPI
        Get
            Return _myEikonDesktopSdk
        End Get
    End Property


    Private Sub New()
        ' ReSharper disable UnusedVariable
        _myEikonDesktopSdk = New EikonDesktopDataAPI()
        Dim eikonStatus = _myEikonDesktopSdk.Status
        ' ReSharper restore UnusedVariable
    End Sub

    Public Shared ReadOnly Property Sdk As EikonDesktopDataAPI
        Get
            If _eikon Is Nothing Then _eikon = New Eikon
            Return _eikon.MyEikonDesktopSdk
        End Get
    End Property

    Public Sub Clear()
        '_myEikonDesktopSdk = Nothing
    End Sub

    Private Sub ShutdownNow() Handles _shutdownManager.ShutdownNow
        'todo?
    End Sub
End Class

Public Class EikonConnector
    Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(EikonConnector))
    Private WithEvents _sdk As EikonDesktopDataAPI
    Private Shared _instance As EikonConnector

    Public Event Connected As Action
    Public Event Disconnected As Action
    Public Event LocalMode As Action
    Public Event Offline As Action

    Private WithEvents _shutdownManager As ShutdownController = ShutdownController.Instance

    Private Sub ShutdownNow() Handles _shutdownManager.ShutdownNow
        Logger.Warn("EikonConnector.New()")
        If _sdk IsNot Nothing Then
            Marshal.ReleaseComObject(_sdk)
            _sdk = Nothing
        End If
    End Sub

    Private Sub New(ByVal sdk As EikonDesktopDataAPI)
        Logger.Trace("ConnectToEikon()")
        _sdk = sdk
    End Sub

    Public Sub ConnectToEikon()
        Logger.Trace("ConnectToEikon(), current status = {0}", _sdk.Status)
        If _sdk.Status = EEikonStatus.Connected Then
            RaiseEvent Connected()
        Else
            Dim lResult = _sdk.Initialize()
            If lResult = EEikonDataAPIInitializeResult.Error_InitializeFail Then
                Logger.Warn("Failed to connect, result is {0}", lResult)
                RaiseEvent Disconnected()
                Return
            End If
        End If
    End Sub

    Public Sub OnStatusChanged(ByVal eStatus As EEikonStatus) Handles _sdk.OnStatusChanged
        Logger.Trace("OnStatusChanged({0})", eStatus.ToString())
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

    Public Sub Disconnect()
        'todo can I disconnect
    End Sub
End Class