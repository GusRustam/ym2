Namespace Tools.Elements
    Public Interface IOrdinate
        Sub SetValue(ByVal bpd As BasePointDescription, ByVal val As Double?)
        Function CalculateSpread(ByVal item As BasePointDescription, ByVal curve As ICurve) As Double?
        ReadOnly Property NameProperty() As String
        ReadOnly Property DescrProperty() As String
    End Interface

    Public MustInherit Class OrdinateBase
        Implements IOrdinate
        Implements IEquatable(Of OrdinateBase)
        Public MustOverride Sub SetValue(ByVal bpd As BasePointDescription, ByVal val As Double?) Implements IOrdinate.SetValue
        Protected ReadOnly Name As String
        Protected ReadOnly Descr As String

        Public Overloads Function Equals(ByVal other As OrdinateBase) As Boolean Implements IEquatable(Of OrdinateBase).Equals
            If ReferenceEquals(Nothing, other) Then Return False
            If ReferenceEquals(Me, other) Then Return True
            Return String.Equals(Name, other.Name)
        End Function

        Public Overrides Function ToString() As String
            Return Descr
        End Function

        Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
            If ReferenceEquals(Nothing, obj) Then Return False
            If ReferenceEquals(Me, obj) Then Return True
            If obj.GetType IsNot Me.GetType Then Return False
            Return Equals(DirectCast(obj, OrdinateBase))
        End Function

        Public Overrides Function GetHashCode() As Integer
            If Name Is Nothing Then Return 0
            Return Name.GetHashCode
        End Function

        Public Shared Operator =(ByVal left As OrdinateBase, ByVal right As OrdinateBase) As Boolean
            Return Equals(left, right)
        End Operator

        Public Shared Operator <>(ByVal left As OrdinateBase, ByVal right As OrdinateBase) As Boolean
            Return Not Equals(left, right)
        End Operator

        Public ReadOnly Property NameProperty() As String Implements IOrdinate.NameProperty
            Get
                Return Name
            End Get
        End Property

        Public ReadOnly Property DescrProperty() As String Implements IOrdinate.DescrProperty
            Get
                Return Descr
            End Get
        End Property

        Protected Sub New(ByVal name As String, ByVal descr As String)
            Me.Name = name
            Me.Descr = descr
        End Sub

        Public MustOverride Function CalculateSpread(ByVal item As BasePointDescription, ByVal curve As ICurve) As Double? Implements IOrdinate.CalculateSpread
    End Class

    Public Class OrdinateYield
        Inherits OrdinateBase

        Private Shared ReadOnly Inst As New OrdinateYield("Yield", "Yield")

        Public Sub New(ByVal name As String, ByVal descr As String)
            MyBase.New(name, descr)
        End Sub

        Public Overrides Sub SetValue(ByVal bpd As BasePointDescription, ByVal val As Double?)
            ' todo OR NOT?????? Maybe these classes should do ze calculation of ze spreads?
            Throw New InvalidOperationException()
        End Sub

        Public Overrides Function CalculateSpread(ByVal item As BasePointDescription, ByVal curve As ICurve) As Double?
            Throw New InvalidOperationException()
        End Function

        Public Shared ReadOnly Property Instance() As OrdinateYield
            Get
                Return Inst
            End Get
        End Property
    End Class

    Public Class OrdinatePointSpread
        Inherits OrdinateBase

        Private Shared ReadOnly Inst As New OrdinatePointSpread("PointSpread", "Point spread")

        Public Sub New(ByVal name As String, ByVal descr As String)
            MyBase.New(name, descr)
        End Sub

        Public Overrides Sub SetValue(ByVal bpd As BasePointDescription, ByVal val As Double?)
            Throw New InvalidOperationException()
        End Sub

        Public Overrides Function CalculateSpread(ByVal item As BasePointDescription, ByVal curve As ICurve) As Double?
            Throw New NotImplementedException()
        End Function

        Public Shared ReadOnly Property Instance() As OrdinateBase
            Get
                Return Inst
            End Get
        End Property
    End Class

    Public Class OrdinateAswSpread
        Inherits OrdinateBase

        Private Shared ReadOnly Inst As New OrdinateAswSpread("ASWSpread", "Asset swap spread")

        Public Sub New(ByVal name As String, ByVal descr As String)
            MyBase.New(name, descr)
        End Sub

        Public Overrides Sub SetValue(ByVal bpd As BasePointDescription, ByVal val As Double?)
            Throw New InvalidOperationException()
        End Sub

        Public Overrides Function CalculateSpread(ByVal item As BasePointDescription, ByVal curve As ICurve) As Double?
            Throw New NotImplementedException()
        End Function

        Public Shared ReadOnly Property Instance() As OrdinateAswSpread
            Get
                Return Inst
            End Get
        End Property
    End Class

    Public Class OrdinateOaSpread
        Inherits OrdinateBase

        Private Shared ReadOnly Inst As New OrdinateOaSpread("OASpread", "Option-adjusted spread")

        Public Sub New(ByVal name As String, ByVal descr As String)
            MyBase.New(name, descr)
        End Sub

        Public Overrides Sub SetValue(ByVal bpd As BasePointDescription, ByVal val As Double?)
            bpd.OASpread = val
        End Sub

        Public Overrides Function CalculateSpread(ByVal item As BasePointDescription, ByVal curve As ICurve) As Double?
            Throw New NotImplementedException()
        End Function

        Public Shared ReadOnly Property Instance() As OrdinateOaSpread
            Get
                Return Inst
            End Get
        End Property
    End Class

    Public Class OrdinateZSpread
        Inherits OrdinateBase

        Private Shared ReadOnly Inst As New OrdinateZSpread("ZSpread", "Z Spread")

        Public Sub New(ByVal name As String, ByVal descr As String)
            MyBase.New(name, descr)
        End Sub

        Public Overrides Sub SetValue(ByVal bpd As BasePointDescription, ByVal val As Double?)
            bpd.ZSpread = val
        End Sub

        Public Overrides Function CalculateSpread(ByVal item As BasePointDescription, ByVal curve As ICurve) As Double?
            Throw New NotImplementedException()
        End Function

        Public Shared ReadOnly Property Instance() As OrdinateZSpread
            Get
                Return Inst
            End Get
        End Property
    End Class

    Public Module Ordinate
        Public Yield As OrdinateBase = OrdinateYield.Instance
        Public PointSpread As OrdinateBase = OrdinatePointSpread.Instance
        Public AswSpread As OrdinateBase = OrdinateAswSpread.Instance
        Public ZSpread As OrdinateBase = OrdinateZSpread.Instance
        Public OaSpread As OrdinateBase = OrdinateOaSpread.Instance

        Private ReadOnly Items As List(Of OrdinateBase) = {Yield, PointSpread, AswSpread, ZSpread, OaSpread}.ToList()

        Public Function FromString(ByVal name As String) As OrdinateBase
            Dim res = (From item In Items Where item.NameProperty = name).ToList
            Return If(res.Any, res.First, Nothing)
        End Function
    End Module
End Namespace