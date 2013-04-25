Imports EikonDesktopDataAPILib

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
    Private WithEvents _sdk As EikonDesktopDataAPI
    Private Shared _instance As EikonConnector

    Public Event Connected As Action
    Public Event Disconnected As Action
    Public Event LocalMode As Action
    Public Event Offline As Action

    Private Sub New(ByVal sdk As EikonDesktopDataAPI)
        _sdk = sdk
    End Sub

    Public Sub ConnectToEikon()
        Dim lResult = _sdk.Initialize()
        If lResult <> EEikonDataAPIInitializeResult.Succeed Then
            RaiseEvent Disconnected()
        End If
        ' todo run waiter thread ?
    End Sub

    Public Sub OnStatusChanged(ByVal eStatus As EEikonStatus) Handles _sdk.OnStatusChanged
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