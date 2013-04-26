Imports DbManager

Namespace Tools
    Public MustInherit Class Entity2
        Implements IEquatable(Of Entity2)

        Private ReadOnly _id As Long = Ansamble2.GenerateID()

        Public ReadOnly Property ID() As Long
            Get
                Return _id
            End Get
        End Property

        Public Overloads Function Equals(ByVal other As Entity2) As Boolean Implements IEquatable(Of Entity2).Equals
            If ReferenceEquals(Nothing, other) Then Return False
            If ReferenceEquals(Me, other) Then Return True
            Return _id = other._id
        End Function

        Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
            If ReferenceEquals(Nothing, obj) Then Return False
            If ReferenceEquals(Me, obj) Then Return True
            If obj.GetType IsNot Me.GetType Then Return False
            Return Equals(DirectCast(obj, Entity2))
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return _id.GetHashCode
        End Function

        Protected Overrides Sub Finalize()
            Ansamble2.ReleaseID(_id)
        End Sub

        Public Shared Operator =(ByVal left As Entity2, ByVal right As Entity2) As Boolean
            Return Equals(left, right)
        End Operator

        Public Shared Operator <>(ByVal left As Entity2, ByVal right As Entity2) As Boolean
            Return Not Equals(left, right)
        End Operator
    End Class

    Public Interface IGroup2
        Sub Start()
        Sub Pause()
        Sub Cleanup()
    End Interface

    Public Class Group2
        Inherits Entity2
        Implements IGroup2
        Private _ansamble As Ansamble

        Public Sub Start() Implements IGroup2.Start
            Throw New NotImplementedException()
        End Sub

        Public Sub Pause() Implements IGroup2.Pause
            Throw New NotImplementedException()
        End Sub

        Public Sub Cleanup() Implements IGroup2.Cleanup
            Throw New NotImplementedException()
        End Sub
    End Class

    Public Class Bond2
        Inherits Entity2
        Private _group As Group2
    End Class

    Public Enum SpreadType2
        PointSpread
        ZSpread
        OASpread
        ASWSpread
    End Enum

    Public Class Curve2
        Inherits Entity2
        Implements IGroup2
        Private _ansamble As Ansamble
        Private _benchmarkType As SpreadType2?

        Public Sub Start() Implements IGroup2.Start
            Throw New NotImplementedException()
        End Sub

        Public Sub Pause() Implements IGroup2.Pause
            Throw New NotImplementedException()
        End Sub

        Public Sub Cleanup() Implements IGroup2.Cleanup
            Throw New NotImplementedException()
        End Sub
    End Class

    Class Ansamble2
        Private Shared ReadOnly Identities As New HashSet(Of Long)

        Private _items As New HashSet(Of Entity2)

        Public Shared Sub ReleaseID(ByVal id As Long)
            Identities.Remove(id)
        End Sub

        Public Shared Function GenerateID() As Long
            Dim rnd = New Random()
            Dim num As Integer
            Do
                num = CLng(Math.Round((89.9999 * rnd.NextDouble() + 10) * 10000))
            Loop While Identities.Contains(num)
            Identities.Add(num)
            Return num
        End Function

        Public Sub Start()
            Throw New NotImplementedException()
        End Sub

        Public Sub Pause()
            Throw New NotImplementedException()
        End Sub

        Public Sub Cleanup()
            Throw New NotImplementedException()
        End Sub

        Public Function CreateGroup(ByVal descr As PortfolioSource) As Group2
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace