Imports EikonDesktopSDKLib

Namespace Commons

    Public Class Eikon
        Private _myEikonDesktopSdk As EikonDesktopSDK
        Private Shared _sdk As Eikon

        Private Sub New()
            _myEikonDesktopSdk = New EikonDesktopSDK
        End Sub

        Public Shared ReadOnly Property Instance As Eikon
            Get
                If _sdk Is Nothing Then _sdk = New Eikon
                Return _sdk
            End Get
        End Property

        Public Shared ReadOnly Property SDK As EikonDesktopSDK
            Get
                Return Instance._myEikonDesktopSdk
            End Get
        End Property

        Public Sub Clear()
            _myEikonDesktopSdk = Nothing
        End Sub
    End Class
End Namespace