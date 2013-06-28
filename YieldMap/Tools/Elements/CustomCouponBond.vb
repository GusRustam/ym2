Imports DbManager

Namespace Tools.Elements
    Public Class CustomCouponBond
        Inherits Bond

        Private ReadOnly _cstmBond As CustomBondSrc

        Public Sub New(ByVal parent As Group, ByVal cstmBond As CustomBondSrc)
            MyBase.New(parent, cstmBond.GetDescription())
            _cstmBond = cstmBond
        End Sub

        Public Overrides ReadOnly Property Coupon(ByVal dt As Date) As Double
            Get
                Return _cstmBond.CurrentCouponRate
            End Get
        End Property
    End Class
End Namespace