Imports AdfinXAnalyticsFunctions
Imports ReutersData

Namespace Tools.Elements
    Public Interface IOrdinate
        Sub SetValue(ByVal bpd As BasePointDescription, ByVal curve As ICurve, Optional ByVal val As Double? = Nothing)
        Sub ClearValue(ByVal bpd As BasePointDescription)
        ReadOnly Property NameProperty() As String
        ReadOnly Property DescrProperty() As String
    End Interface

    Public MustInherit Class OrdinateBase
        Implements IOrdinate
        Implements IEquatable(Of OrdinateBase)
        Protected ReadOnly BondModule As AdxBondModule = Eikon.Sdk.CreateAdxBondModule()
        Public MustOverride Sub SetValue(ByVal bpd As BasePointDescription, ByVal curve As ICurve, Optional ByVal val As Double? = Nothing) Implements IOrdinate.SetValue
        Public MustOverride Sub ClearValue(ByVal bpd As BasePointDescription) Implements IOrdinate.ClearValue
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

    End Class

    Public Class OrdinateYield
        Inherits OrdinateBase

        Private Shared ReadOnly Inst As New OrdinateYield("Yield", "Yield")

        Public Sub New(ByVal name As String, ByVal descr As String)
            MyBase.New(name, descr)
        End Sub

        ' тогда вопрос, что такое val. Давайте щетать, что для облигации это иё цена, ась?
        Public Overrides Sub SetValue(ByVal bpd As BasePointDescription, ByVal curve As ICurve, Optional ByVal val As Double? = Nothing)
            bpd.CalculateYield(val)
        End Sub

        Public Overrides Sub ClearValue(ByVal bpd As BasePointDescription)
            bpd.ClearYield()
        End Sub

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

        Public Overrides Sub SetValue(ByVal bpd As BasePointDescription, ByVal curve As ICurve, Optional ByVal val As Double? = Nothing)
            If val IsNot Nothing Then
                bpd.PointSpread = val
            Else
                ' todo calculating point spread
            End If
        End Sub

        Public Overrides Sub ClearValue(ByVal bpd As BasePointDescription)
            bpd.PointSpread = Nothing
        End Sub

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

        Public Overrides Sub SetValue(ByVal bpd As BasePointDescription, ByVal curve As ICurve, Optional ByVal val As Double? = Nothing)
            If val IsNot Nothing Then
                bpd.ASWSpread = val
            Else
                ' todo
            End If
        End Sub

        Public Overrides Sub ClearValue(ByVal bpd As BasePointDescription)
            bpd.ASWSpread = Nothing
        End Sub

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

        Public Overrides Sub SetValue(ByVal bpd As BasePointDescription, ByVal curve As ICurve, Optional ByVal val As Double? = Nothing)
            If val IsNot Nothing Then
                bpd.OASpread = val
            Else
                ' todo
            End If
        End Sub

        Public Overrides Sub ClearValue(ByVal bpd As BasePointDescription)
            bpd.OASpread = Nothing
        End Sub

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

        Public Overrides Sub SetValue(ByVal bpd As BasePointDescription, ByVal curve As ICurve, Optional ByVal val As Double? = Nothing)
            If val IsNot Nothing Then
                bpd.ZSpread = val
            Else
                ' todo
            End If
        End Sub

        Public Overrides Sub ClearValue(ByVal bpd As BasePointDescription)
            bpd.ZSpread = Nothing
        End Sub

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