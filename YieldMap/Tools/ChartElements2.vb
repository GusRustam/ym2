Imports DbManager.Bonds
Imports DbManager
Imports NLog
Imports Settings

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
        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(Group2))
        Private _ansamble As Ansamble2
        Private _seriesName As String
        Private _portfolioID As String
        Private _fields As FieldSet
        Private _color As String

        Public Shared Function Create(ByVal ans As Ansamble2, ByVal port As PortfolioSource, ByVal portfolioStructure As PortfolioStructure) As Group2
            Dim group As Group2

            Dim source = TryCast(port.Source, Source)
            If source Is Nothing Then
                Logger.Warn("Unsupported source {0}", source)
                Return Nothing
            End If
            group = New Group2(ans) With {
                .SeriesName = If(port.Name <> "", port.Name, source.Name),
                .PortfolioID = source.ID,
                .Fields = source.Fields,
                .Color = If(port.Color <> "", port.Color, source.Color)
            }

            Dim fieldPriority = SettingsManager.Instance.FieldsPriority.Split(",")
            Dim yieldMode = SettingsManager.Instance.YieldCalcMode
            Dim selectedField = FindAppropriateField(fieldPriority, group.Fields.Realtime)

            For Each ric In portfolioStructure.Rics(port)
                Dim descr = BondsData.Instance.GetBondInfo(ric)
                If descr IsNot Nothing Then
                    group.AddRic(ric, descr, selectedField, yieldMode)
                Else
                    Logger.Error("No description for bond {0} found", ric)
                End If
            Next
            Return group
        End Function

        Private Sub AddRic(ByVal ric As String, ByVal bondDescription As BondDescription, ByVal selectedField As String, ByVal yieldMode As String)
            Throw New NotImplementedException()
        End Sub

        Private Shared Function FindAppropriateField(ByVal fieldPriority As String(), ByVal realtime As Fields) As String
            Dim usefulFields = From fld In fieldPriority
                    Let val = realtime.GetField(fld)
                    Where val <> ""
                    Select fld
            If Not usefulFields.Any Then Throw New BadBondException()
            Return usefulFields.First
        End Function

        Protected Property Color() As String
            Get
                Return _color
            End Get
            Set(ByVal value As String)
                _color = value
            End Set
        End Property

        Protected Property Fields() As FieldSet
            Get
                Return _fields
            End Get
            Set(ByVal value As FieldSet)
                _fields = value
            End Set
        End Property

        Protected Property PortfolioID() As String
            Get
                Return _portfolioID
            End Get
            Set(ByVal value As String)
                _portfolioID = value
            End Set
        End Property

        Protected Property SeriesName() As String
            Get
                Return _seriesName
            End Get
            Set(ByVal value As String)
                _seriesName = value
            End Set
        End Property

        Private Sub New(ByVal ansamble As Ansamble2)
            _ansamble = ansamble
        End Sub

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

    Friend Class BadBondException
        Inherits Exception
    End Class

    Public Class Bond2
        Inherits Entity2
        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(Bond2))
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
        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(Curve2))
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

    Public Class Ansamble2
        Private Shared ReadOnly Identities As New HashSet(Of Long)

        Public ReadOnly Property Items() As HashSet(Of IGroup2)
            Get
                Return _items
            End Get
        End Property

        Private ReadOnly _items As New HashSet(Of IGroup2)

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
    End Class
End Namespace