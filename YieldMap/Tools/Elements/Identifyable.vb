Namespace Tools.Elements
    'Public Interface IIdentifyable
    '    ReadOnly Property Identity() As Long
    'End Interface

    Public MustInherit Class Identifyable
        Implements IEquatable(Of Identifyable) ', IIdentifyable
        Private ReadOnly _identity As Long = Ansamble.GenerateID()

        Public ReadOnly Property Identity() As Long 'Implements IIdentifyable.Identity
            Get
                Return _identity
            End Get
        End Property

        Public Overloads Function Equals(ByVal other As Identifyable) As Boolean Implements IEquatable(Of Identifyable).Equals
            If ReferenceEquals(Nothing, other) Then Return False
            If ReferenceEquals(Me, other) Then Return True
            Return _identity = other._identity
        End Function

        Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
            If ReferenceEquals(Nothing, obj) Then Return False
            If ReferenceEquals(Me, obj) Then Return True
            If obj.GetType IsNot Me.GetType Then Return False
            Return Equals(DirectCast(obj, Identifyable))
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return _identity.GetHashCode
        End Function

        Public Shared Operator =(ByVal left As Identifyable, ByVal right As Identifyable) As Boolean
            Return Equals(left, right)
        End Operator

        Public Shared Operator <>(ByVal left As Identifyable, ByVal right As Identifyable) As Boolean
            Return Not Equals(left, right)
        End Operator

        Protected Overrides Sub Finalize()
            Ansamble.ReleaseID(_identity)
        End Sub
    End Class
End Namespace