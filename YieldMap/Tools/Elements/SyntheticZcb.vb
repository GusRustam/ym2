Imports DbManager.Bonds

Namespace Tools.Elements
    Public Class SyntheticZcb
        Inherits Bond
        Public Sub New(ByVal parent As Group, ByVal metaData As BondMetadata)
            MyBase.New(parent, metaData)
        End Sub

        Public Overrides ReadOnly Property Coupon(ByVal dt As Date) As Double
            Get
                Return 0
            End Get
        End Property
    End Class
End NameSpace