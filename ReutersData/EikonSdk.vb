Imports EikonDesktopDataAPILib
Imports NLog
Imports CommonController
Imports System.Runtime.InteropServices

Public Class Eikon
    Private Shared _myEikonDesktopSdk As New EikonDesktopDataAPI

    Private Sub New()
        ' ReSharper disable UnusedVariable
        Dim eikonStatus = _myEikonDesktopSdk.Status
        ' ReSharper restore UnusedVariable
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

    'Private ReadOnly _lock As New Object
    'Private _timeOutState As InterlockedState = InterlockedState.BrandNew

    Public Event Connected As Action
    Public Event Disconnected As Action
    Public Event LocalMode As Action
    Public Event Offline As Action
    Public Event Timeout As Action

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

    'Private Enum InterlockedState
    '    BrandNew
    '    Waiter
    '    WaiterFinished
    'End Enum

    Public Sub ConnectToEikon()
        Logger.Trace("ConnectToEikon()")
        'SyncLock _lock
        '    If _timeOutState = InterlockedState.Waiter Then
        '        Logger.Info("Unable to reconnect while waiter is alive")
        '        Return
        '    Else
        '        _timeOutState = InterlockedState.BrandNew
        '    End If
        'End SyncLock
        Dim lResult = _sdk.Initialize()
        If lResult = EEikonDataAPIInitializeResult.Error_InitializeFail Then
            Logger.Warn("Failed to connect, result is {0}", lResult)
            RaiseEvent Disconnected()
            Return
        End If

        'ThreadPool.QueueUserWorkItem(
        '    Sub()
        '        SyncLock _lock
        '            _timeOutState = InterlockedState.Waiter
        '        End SyncLock
        '        Logger.Info("Connection waiter started")
        '        Thread.Sleep(TimeSpan.FromSeconds(20))
        '        SyncLock _lock
        '            Logger.Info("Connection wait finished")
        '            If _timeOutState = InterlockedState.Waiter Then
        '                Logger.Warn("Connection was unsuccessful")
        '                _timeOutState = InterlockedState.WaiterFinished
        '                RaiseEvent Timeout()
        '            Else
        '                Logger.Info("Connection was successful, status is {0}", _timeOutState)
        '            End If
        '        End SyncLock

        '    End Sub)
    End Sub

    Public Sub OnStatusChanged(ByVal eStatus As EEikonStatus) Handles _sdk.OnStatusChanged
        Logger.Trace("OnStatusChanged({0})", eStatus.ToString())
        'SyncLock _lock
        '    If _timeOutState = InterlockedState.WaiterFinished Then
        '        Logger.Warn("Connection message arrived after timeout, status = {0}", eStatus)
        '    Else
        '        _timeOutState = InterlockedState.WaiterFinished
        '        Logger.Info("Connection message arrived before timeout")
        '    End If
        'End SyncLock

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