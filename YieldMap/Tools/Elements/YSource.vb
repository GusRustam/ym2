Imports System.Reflection

Namespace Tools.Elements
    Public Class YSource
        Public Shared Yield As New YSource("Yield", True)
        Public Shared PointSpread As New YSource("PointSpread", True)
        Public Shared ZSpread As New YSource("ZSpread", True)
        Public Shared OASpread As New YSource("OASpread", False)
        Public Shared ASWSpread As New YSource("ASWSpread", True)

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If TypeOf obj Is YSource Then
                Return _name = obj.Name
            Else
                Return False
            End If
        End Function

        Public Overrides Function ToString() As String
            Return _name
        End Function

        Private ReadOnly _name As String
        Private ReadOnly _enabled As Boolean

        Private Sub New(ByVal mode As String, ByVal enabled As Boolean)
            _name = mode
            _enabled = enabled
        End Sub

        Public Shared Operator =(ByVal mode1 As YSource, ByVal mode2 As YSource)
            If mode1 Is Nothing OrElse mode2 Is Nothing Then Return False
            Return mode1.Name = mode2.Name
        End Operator

        Public Shared Operator <>(ByVal mode1 As YSource, ByVal mode2 As YSource)
            If mode1 Is Nothing OrElse mode2 Is Nothing Then Return False
            Return mode1.Name <> mode2.Name
        End Operator

        Public Shared Function IsEnabled(ByVal name As String)
            Dim staticFields = GetType(YSource).GetFields(BindingFlags.Instance Or BindingFlags.Public)
            Return (From info In staticFields Let element = CType(info.GetValue(Nothing), YSource) Select element.Name = name And element.Enabled).FirstOrDefault()
        End Function

        Public ReadOnly Property Enabled As Boolean
            Get
                Return _enabled
            End Get
        End Property

        Public ReadOnly Property Name As String
            Get
                Return _name
            End Get
        End Property

        Public Shared Function FromString(ByVal name As String) As YSource
            Dim staticFields = GetType(YSource).GetFields()
            Return (From info In staticFields Let element = CType(info.GetValue(Nothing), YSource) Where element.Name = name And element.Enabled Select element).FirstOrDefault()
        End Function
    End Class
End NameSpace