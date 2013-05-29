Imports System.Reflection

Namespace Tools.Elements
    'Public Class YSource
    '    Public Shared Yield As New YSource("Yield", True)
    '    Public Shared PointSpread As New YSource("PointSpread", True)
    '    Public Shared ZSpread As New YSource("ZSpread", True)
    '    Public Shared OASpread As New YSource("OASpread", False)
    '    Public Shared ASWSpread As New YSource("ASWSpread", True)

    '    Public Overrides Function Equals(ByVal obj As Object) As Boolean
    '        If TypeOf obj Is YSource Then
    '            Return _name = obj.Name
    '        Else
    '            Return False
    '        End If
    '    End Function

    '    Public Overrides Function ToString() As String
    '        Return _name
    '    End Function

    '    Private ReadOnly _name As String
    '    Private ReadOnly _enabled As Boolean

    '    Private Sub New(ByVal mode As String, ByVal enabled As Boolean)
    '        _name = mode
    '        _enabled = enabled
    '    End Sub

    '    Public Shared Operator =(ByVal mode1 As YSource, ByVal mode2 As YSource)
    '        If mode1 Is Nothing OrElse mode2 Is Nothing Then Return False
    '        Return mode1.Name = mode2.Name
    '    End Operator

    '    Public Shared Operator <>(ByVal mode1 As YSource, ByVal mode2 As YSource)
    '        If mode1 Is Nothing OrElse mode2 Is Nothing Then Return False
    '        Return mode1.Name <> mode2.Name
    '    End Operator

    '    Public Shared Function IsEnabled(ByVal name As String)
    '        Dim staticFields = GetType(YSource).GetFields(BindingFlags.Instance Or BindingFlags.Public)
    '        Return (From info In staticFields Let element = CType(info.GetValue(Nothing), YSource) Select element.Name = name And element.Enabled).FirstOrDefault()
    '    End Function

    '    Public ReadOnly Property Enabled As Boolean
    '        Get
    '            Return _enabled
    '        End Get
    '    End Property

    '    Public ReadOnly Property Name As String
    '        Get
    '            Return _name
    '        End Get
    '    End Property

    '    Public Shared Function FromString(ByVal name As String) As YSource
    '        Dim staticFields = GetType(YSource).GetFields()
    '        Return (From info In staticFields Let element = CType(info.GetValue(Nothing), YSource) Where element.Name = name And element.Enabled Select element).FirstOrDefault()
    '    End Function
    'End Class

    Public Interface IOrdinate
        Sub SetValue(ByVal bpd As BasePointDescription, ByVal val As Double)
        ReadOnly Property NameProperty() As String
        ReadOnly Property DescrProperty() As String
    End Interface

    Public MustInherit Class OrdinateBase
        Implements IOrdinate
        Implements IEquatable(Of OrdinateBase)
        Public MustOverride Sub SetValue(ByVal bpd As BasePointDescription, ByVal val As Double) Implements IOrdinate.SetValue
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

        Public Overrides Sub SetValue(ByVal bpd As BasePointDescription, ByVal val As Double)
            ' todo OR NOT?????? Maybe these classes should do ze calculation of ze spreads?
            Throw New InvalidOperationException()
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

        Public Overrides Sub SetValue(ByVal bpd As BasePointDescription, ByVal val As Double)
            Throw New InvalidOperationException()
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

        Public Overrides Sub SetValue(ByVal bpd As BasePointDescription, ByVal val As Double)
            Throw New InvalidOperationException()
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

        Public Overrides Sub SetValue(ByVal bpd As BasePointDescription, ByVal val As Double)
            bpd.OASpread = val
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

        Public Overrides Sub SetValue(ByVal bpd As BasePointDescription, ByVal val As Double)
            bpd.ZSpread = val
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