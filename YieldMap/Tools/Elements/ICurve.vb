Namespace Tools.Elements
    Public Interface ICurve
        Inherits INamed

        ReadOnly Property Snapshot() As ISnapshot
        ReadOnly Property Formula() As String
        ReadOnly Property CanBootstrap() As Boolean
        Property Bootstrapped() As Boolean
        Property CurveDate() As Date
        Sub Bootstrap()
        ReadOnly Property IsSynthetic() As Boolean

        Sub ClearSpread(ByVal ySource As OrdinateBase)
        Sub SetSpread(ByVal ySource As OrdinateBase)
        Function RateArray() As Array
    End Interface
End Namespace