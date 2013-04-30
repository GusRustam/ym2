Imports System.ComponentModel
Imports DbManager.Bonds
Imports NLog
Imports System.Collections.ObjectModel
Imports System.Xml
Imports Uitls

Public Class RicDescription
    Private ReadOnly _ric As String
    Private ReadOnly _descr As String
    Private ReadOnly _srcType As String
    Private ReadOnly _srcName As String
    Private ReadOnly _color As String
    Private ReadOnly _included As Boolean

    Public Sub New(ByVal ric As String, ByVal descr As String, ByVal srcType As String, ByVal srcName As String, ByVal color As String, ByVal included As Boolean)
        _ric = ric
        _srcType = srcType
        _srcName = srcName
        _color = color
        _included = included
        _descr = descr
    End Sub

    <DisplayName("Description")>
    Public ReadOnly Property Descr() As String
        Get
            Return _descr
        End Get
    End Property

    Public ReadOnly Property RIC() As String
        Get
            Return _ric
        End Get
    End Property

    Public ReadOnly Property From() As String
        Get
            Return _srcType
        End Get
    End Property

    <DisplayName("Name of source")>
    Public ReadOnly Property FromName() As String
        Get
            Return _srcName
        End Get
    End Property

    Public ReadOnly Property Color() As String
        Get
            Return _color
        End Get
    End Property

    Public ReadOnly Property Included() As Boolean
        Get
            Return _included
        End Get
    End Property
End Class

Public Class PortfolioSource
    Private ReadOnly _id As String
    Private ReadOnly _portfolio As Portfolio
    Private ReadOnly _source As SourceBase
    Private ReadOnly _condition As String
    Private ReadOnly _customName As String
    Private ReadOnly _customColor As String
    Private ReadOnly _included As Boolean

    Friend Sub New(ByVal id As String, ByVal source As SourceBase, ByVal condition As String, ByVal customName As String, ByVal customColor As String, ByVal included As Boolean, ByVal portfolio As Portfolio)
        _id = id
        _source = source
        _condition = condition
        _customName = customName
        _customColor = customColor
        _included = included
        _portfolio = portfolio
    End Sub

    <Browsable(False)>
    Public ReadOnly Property Id As String
        Get
            Return _id
        End Get
    End Property

    Public ReadOnly Property Source As SourceBase
        Get
            Return _source
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property Portfolio() As Portfolio
        Get
            Return _portfolio
        End Get
    End Property

    Public ReadOnly Property Condition As String
        Get
            Return _condition
        End Get
    End Property

    Public ReadOnly Property Name As String
        Get
            Return _customName
        End Get
    End Property

    Public ReadOnly Property Included As Boolean
        Get
            Return _included
        End Get
    End Property

    Public ReadOnly Property Color As String
        Get
            Return _customColor
        End Get
    End Property

    Protected Overloads Function Equals(ByVal other As PortfolioSource) As Boolean
        Return String.Equals(_id, other._id) AndAlso Equals(_source, other._source)
    End Function

    Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
        If ReferenceEquals(Nothing, obj) Then Return False
        If ReferenceEquals(Me, obj) Then Return True
        If obj.GetType IsNot Me.GetType Then Return False
        Return Equals(DirectCast(obj, PortfolioSource))
    End Function

    Public Overrides Function GetHashCode() As Integer
        Dim hashCode As Integer = 0
        If _id IsNot Nothing Then
            hashCode = CInt((hashCode * 397L) Mod Integer.MaxValue) Xor _id.GetHashCode
        End If
        If _source IsNot Nothing Then
            hashCode = CInt((hashCode * 397L) Mod Integer.MaxValue) Xor _source.GetHashCode
        End If
        Return hashCode
    End Function

    Public Shared Operator =(ByVal left As PortfolioSource, ByVal right As PortfolioSource) As Boolean
        Return Equals(left, right)
    End Operator

    Public Shared Operator <>(ByVal left As PortfolioSource, ByVal right As PortfolioSource) As Boolean
        Return Not Equals(left, right)
    End Operator
End Class

''' <summary>
''' This class has two representations:
''' 1) Sources property - low level representation
''' 2) Rics property - returns already recalculated data
''' </summary>
''' <remarks></remarks>

Public Class PortfolioStructure
    Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(PortfolioStructure))

    Public Const List As Byte = 1
    Public Const Chain As Byte = 2
    Public Const CustomBond As Byte = 4
    Public Const All As Byte = List Or Chain Or CustomBond

    Private ReadOnly _id As String
    Private ReadOnly _name As String
    Private ReadOnly _sources As New List(Of PortfolioSource)

    Private ReadOnly _excludes As New HashSet(Of String)
    Private ReadOnly _rics As New Dictionary(Of PortfolioSource, List(Of String))

    Public Sub New(ByVal id As String, ByVal name As String, ByVal sources As List(Of PortfolioSource))
        _id = id
        _name = name
        _sources = sources
    End Sub

    Public Sub New(ByVal id As String, ByVal name As String)
        _id = id
        _name = name
    End Sub

    Public ReadOnly Property Sources(Optional ByVal what As Byte = All) As ReadOnlyCollection(Of PortfolioSource)
        Get
            Dim data As New List(Of PortfolioSource)
            If what And List Then
                data.AddRange(From src In _sources Where TypeOf src.Source Is UserList)
            End If
            If what And Chain Then
                data.AddRange(From src In _sources Where TypeOf src.Source Is Chain)
            End If
            If what And CustomBond Then
                data.AddRange(From src In _sources Where TypeOf src.Source Is CustomBond)
            End If
            Return New ReadOnlyCollection(Of PortfolioSource)(data)
        End Get
    End Property

    Public ReadOnly Property ID As String
        Get
            Return _id
        End Get
    End Property

    Public ReadOnly Property Name As String
        Get
            Return _name
        End Get
    End Property

    Public ReadOnly Property Rics(ByVal src As PortfolioSource) As ReadOnlyCollection(Of String)
        Get
            Dim res As New List(Of String)
            Dim interpreter = New FilterInterpreter(Of BondDescription)
            Dim filter As Boolean = False
            Dim cond = src.Condition
            If cond <> "" Then
                Dim parser As New FilterParser
                Try
                    Dim grammar As LinkedList(Of FilterParser.IGrammarElement)
                    grammar = parser.SetFilter(cond)
                    interpreter.SetGrammar(grammar)
                    filter = True
                Catch ex As Exception
                    Logger.ErrorException(String.Format("Failed to parse condition {0}", cond), ex)
                    Logger.Error("Exception = {0}", ex.ToString())
                End Try

            End If
            For Each ric In From item In src.Source.GetDefaultRics() Where Not _excludes.Contains(item)
                Try
                    Dim descr = BondsData.Instance.GetBondInfo(ric)
                    If filter Then
                        Try
                            If Not interpreter.Allows(descr) Then Continue For
                        Catch ex As Exception
                            Logger.ErrorException(String.Format("Failed to apply filter to bond {0}", ric), ex)
                            Logger.Error("Exception = {0}", ex.ToString())
                        End Try
                    End If
                    res.Add(ric)
                Catch ex As NoBondException
                    Logger.Warn("No bond {0}", ric)
                End Try
            Next
            Return New ReadOnlyCollection(Of String)(res)
        End Get
    End Property

    Public ReadOnly Property Rics(Optional ByVal netted As Boolean = False) As ReadOnlyCollection(Of RicDescription)
        Get
            Dim res As New List(Of RicDescription)
            Dim interpreter = New FilterInterpreter(Of BondDescription)
            Dim filter As Boolean = False
            For Each src In _sources
                Dim description As RicDescription
                Dim cond = src.Condition
                If cond <> "" Then
                    Dim parser As New FilterParser
                    Try
                        Dim grammar As LinkedList(Of FilterParser.IGrammarElement)
                        grammar = parser.SetFilter(cond)
                        interpreter.SetGrammar(grammar)
                        filter = True
                    Catch ex As Exception
                        Logger.ErrorException(String.Format("Failed to parse condition {0}", cond), ex)
                        Logger.Error("Exception = {0}", ex.ToString())
                    End Try

                End If
                For Each ric In src.Source.GetDefaultRics()
                    If netted AndAlso _excludes.Contains(ric) Then Continue For
                    Try
                        Dim descr = BondsData.Instance.GetBondInfo(ric)
                        If filter Then
                            Try
                                If Not interpreter.Allows(descr) Then Continue For
                            Catch ex As Exception
                                Logger.ErrorException(String.Format("Failed to apply filter to bond {0}", ric), ex)
                                Logger.Error("Exception = {0}", ex.ToString())
                            End Try
                        End If
                        Dim type = src.Source.GetType().Name
                        description = New RicDescription(ric, descr.Label1, type, src.Name, src.Color, src.Included)
                        res.Add(description)
                    Catch ex As NoBondException
                        Logger.Warn("No bond {0}", ric)
                    End Try
                Next
            Next
            Return New ReadOnlyCollection(Of RicDescription)(res)
        End Get
    End Property

    Friend Sub AddSource(ByVal src As PortfolioSource)
        _sources.Add(src)
        RecalculateSources()
    End Sub

    Private Sub RecalculateSources()
        _excludes.Clear()
        _rics.Clear()

        For Each ric In From defRics In (
                            From src In _sources
                            Where Not src.Included
                            Select src.Source.GetDefaultRics())
                        From defRic In defRics
                        Select defRic
            _excludes.Add(ric)
        Next

        For Each src In (From srcs In _sources Where srcs.Included) ' Order By srcs
            Dim lst = New List(Of String)(src.Source.GetDefaultRics())
            lst.RemoveAll(Function(item) _excludes.Contains(item))
            _rics.Add(src, lst)
        Next
        ' todo now apply static filtering
        ' TODO make a difference between dynamic as static filtering!!!
    End Sub
End Class


Public Class Portfolio
    Public IsFolder As Boolean
    Public Name As String
    Public Id As String

    Private Shared ReadOnly PortMan As PortfolioManager = PortfolioManager.ClassInstance

    Public Sub New(ByVal isFolder As Boolean, ByVal name As String, ByVal id As String)
        Me.IsFolder = isFolder
        Me.Name = name
        Me.Id = id
    End Sub

    Public Sub DeleteSource(ByVal src As PortfolioSource)
        If IsFolder Then Throw New PortfolioException(String.Format("Item with id {0} is a folder", Id))
        Dim xml = PortMan.GetConfigDocument()
        Dim papa = xml.SelectSingleNode(String.Format("/bonds/portfolios//portfolio[@id='{0}']", Id))
        If papa Is Nothing Then Throw New PortfolioException(String.Format("Can not find portfolio with id {0}", Id))
        Dim node = papa.SelectSingleNode(String.Format("include[@what='{0}' and @id='{1}'] | exclude[@what='{0}' and @id='{1}']", src.Source.GetXmlTypeName(), src.Source.ID))
        If node Is Nothing Then Throw New PortfolioException(String.Format("Can not find source with id {0} and type [{2}] in portfolio with id {1} ", src.Source.ID, Id, src.Source.GetXmlTypeName()))
        papa.RemoveChild(node)
        PortMan.SaveBonds()
    End Sub

    Public Sub AddSource(ByVal source As SourceBase, ByVal customName As String, ByVal customColor As String, ByVal condition As String, ByVal include As Boolean)
        If IsFolder Then Throw New PortfolioException(String.Format("Item with id {0} is a folder", Id))
        Dim xml = PortMan.GetConfigDocument()
        Dim node = xml.SelectSingleNode(String.Format("/bonds/portfolios//portfolio[@id='{0}']", Id))
        If node Is Nothing Then Throw New PortfolioException(String.Format("Can not find portfolio with id {0}", Id))
        Dim newSrc = xml.CreateNode(XmlNodeType.Element, If(include, "include", "exclude"), "")
        xml.AppendAttr(newSrc, "what", source.GetXmlTypeName())
        xml.AppendAttr(newSrc, "id", source.ID)
        If customName <> "" Then xml.AppendAttr(newSrc, "name", customName)
        If customColor <> "" Then xml.AppendAttr(newSrc, "color", customColor)
        If condition <> "" Then xml.AppendAttr(newSrc, "condition", condition)
        node.AppendChild(newSrc)
        PortMan.SaveBonds()
    End Sub

    Public Sub UpdateSource(ByVal who As PortfolioSource, ByVal source As Source, ByVal customName As String, ByVal customColor As String, ByVal condition As String, ByVal include As Boolean)
        DeleteSource(who)
        AddSource(source, customName, customColor, condition, include)
    End Sub
End Class


Public Class NoSourceException
    Inherits PortfolioException

    Public Sub New()
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub
End Class


Public Class PortfolioException
    Inherits Exception

    Public Sub New()
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub
End Class

Public Class NoPortfolioException
    Inherits PortfolioException

    Public Sub New()
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub
End Class
