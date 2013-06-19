Imports DbManager.Bonds
Imports DbManager

Namespace Tools.Elements
    Public Class CustomCouponBond
        Inherits Bond

        Private ReadOnly _cstmBond As CustomBond

        Public Sub New(ByVal parent As Group, ByVal cstmBond As CustomBond)
            MyBase.New(parent, cstmBond.GetDescription())
            _cstmBond = cstmBond
        End Sub

        Public Overrides ReadOnly Property Coupon(ByVal dt As Date) As Double
            Get
                Dim descriptions = _cstmBond.Struct.GetCouponsList()
                Dim greatest = (From item1 In descriptions Where item1.Dt > dt)
                Return greatest.First.Rate
            End Get
        End Property
    End Class
End Namespace