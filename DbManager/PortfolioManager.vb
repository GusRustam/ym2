﻿' TODO LIQUIDITY!!!
Imports System.Xml
Imports System.IO
Imports System.ComponentModel
Imports DbManager.Bonds
Imports NLog
Imports System.Collections.ObjectModel
Imports Uitls

Public Class ConfingNameAttribute
    Inherits Attribute
    Private ReadOnly _xmlName As String

    Public Sub New(ByVal xmlName As String)
        _xmlName = xmlName
    End Sub

    Public ReadOnly Property XmlName As String
        Get
            Return _xmlName
        End Get
    End Property
End Class

Public Class FieldSet
    Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(FieldSet))

    <ConfingName("DATE")> Public LastDate As String = ""
    <ConfingName("TIME")> Public Time As String = ""
    <ConfingName("BID")> Public Bid As String = ""
    <ConfingName("ASK")> Public Ask As String = ""
    <ConfingName("LAST")> Public Last As String = ""
    <ConfingName("VWAP")> Public VWAP As String = ""
    <ConfingName("HIST")> Public Hist As String = ""     ' Historical Field. Corresponds to Close
    <ConfingName("HIST_DATE")> Public HistDate As String = ""
    <ConfingName("VOLUME")> Public Volume As String = ""
    <ConfingName("SOURCE")> Public Src As String = ""   ' Contributor

    Public Sub New(ByVal node As XmlNode, ByVal subnode As String)
        Logger.Trace(String.Format("FieldSet({0}, {1})", node.Name, subnode))
        Dim fields = Me.GetType().GetFields()
        For Each info In (From fld In fields
                           Let attx = fld.GetCustomAttributes(GetType(ConfingNameAttribute), False)
                           Where attx.Any
                           Select fld, attx)

            Dim xmlName = CType(info.attx(0), ConfingNameAttribute).XmlName
            Logger.Trace(" ---> found node named {0}", xmlName)
            Dim item = node.SelectSingleNode(String.Format("{0}/field[@type='{1}']", subnode, xmlName))
            If item IsNot Nothing Then
                Logger.Trace(" ---> {0} <- {1}", xmlName, item.InnerText)
                info.fld.SetValue(Me, item.InnerText)
            Else
                Logger.Trace(" ---> no data")
            End If
        Next
    End Sub
End Class

Public Class Fields
    Private ReadOnly _name As String
    Private ReadOnly _history As FieldSet
    Private ReadOnly _realtime As FieldSet

    Public Sub New(ByVal id As String, ByVal doc As XmlDocument)
        Dim node = doc.SelectSingleNode(String.Format("/bonds/field-sets/field-set[@id='{0}']", id))
        If node Is Nothing Then Throw New Exception(String.Format("Failed to find field set with id {0}", id))
        _name = node.Attributes("name").Value
        _realtime = New FieldSet(node, "realtime")
        _history = New FieldSet(node, "historical")
    End Sub

    Public ReadOnly Property Name As String
        Get
            Return _name
        End Get
    End Property

    Public ReadOnly Property History As FieldSet
        Get
            Return _history
        End Get
    End Property

    Public ReadOnly Property Realtime As FieldSet
        Get
            Return _realtime
        End Get
    End Property
End Class

Public MustInherit Class Source
    Private ReadOnly _id As String
    Private ReadOnly _color As String
    Private ReadOnly _name As String
    Private ReadOnly _fields As Fields
    Private ReadOnly _enabled As Boolean
    Private ReadOnly _curve As Boolean

    Protected Overloads Function Equals(ByVal other As Source) As Boolean
        Return String.Equals(_id, other._id)
    End Function

    Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
        If ReferenceEquals(Nothing, obj) Then Return False
        If ReferenceEquals(Me, obj) Then Return True
        Dim other As Source = TryCast(obj, Source)
        Return other IsNot Nothing AndAlso Equals(other)
    End Function

    Public Overrides Function GetHashCode() As Integer
        If _id Is Nothing Then Return 0
        Return _id.GetHashCode
    End Function

    Public Shared Operator =(ByVal left As Source, ByVal right As Source) As Boolean
        Return Equals(left, right)
    End Operator

    Public Shared Operator <>(ByVal left As Source, ByVal right As Source) As Boolean
        Return Not Equals(left, right)
    End Operator

    Public Sub New(ByVal id As String, ByVal color As String, ByVal fields As Fields, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal name As String)
        _id = id
        _color = color
        _fields = fields
        _enabled = enabled
        _curve = curve
        _name = name
    End Sub

    <Browsable(False)>
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


    <Browsable(False)>
    Public ReadOnly Property Fields As Fields
        Get
            Return _fields
        End Get
    End Property

    <DisplayName("Fields layout")>
    Public ReadOnly Property FieldsName As String
        Get
            Return _fields.Name
        End Get
    End Property

    Public ReadOnly Property Enabled As Boolean
        Get
            Return _enabled
        End Get
    End Property

    <DisplayName("Is curve")>
    Public ReadOnly Property Curve As Boolean
        Get
            Return _curve
        End Get
    End Property

    Public ReadOnly Property Color As String
        Get
            Return _color
        End Get
    End Property

    ''' <summary>
    ''' Get list of all Source rics "by default"
    ''' This means that in a portfolio rics might differ because of filters and excluded items
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetDefaultRics() As List(Of String)

    Public Function GetDefaultRicsView() As List(Of BondDescription)
        Return (From ric In GetDefaultRics() Select BondsData.Instance.GetBondInfo(ric)).ToList()
    End Function
End Class

Public Class Chain
    Inherits Source
    Private ReadOnly _chainRic As String
    Private ReadOnly _bondsManager As IBondsLoader = BondsLoader.Instance

    Public Overrides Function ToString() As String
        Return "Chain"
    End Function

    Public Sub New(ByVal id As String, ByVal color As String, ByVal fields As Fields, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal chainRic As String, ByVal name As String)
        MyBase.New(id, color, fields, enabled, curve, name)
        _chainRic = chainRic
    End Sub

    <DisplayName("Chain RIC")>
    Public ReadOnly Property ChainRic As String
        Get
            Return _chainRic
        End Get
    End Property

    Public Overrides Function GetDefaultRics() As List(Of String)
        Return _bondsManager.GetChainRics(_chainRic)
    End Function
End Class

Public Class UserList
    Inherits Source
    Private ReadOnly _bondRics As List(Of String)

    Public Overrides Function ToString() As String
        Return "List"
    End Function

    Public Sub New(ByVal id As String, ByVal color As String, ByVal fields As Fields, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal bondRics As List(Of String), ByVal name As String)
        MyBase.New(id, color, fields, enabled, curve, name)
        _bondRics = bondRics
    End Sub

    Shared Function ExtractRics(ByVal node As XmlNode) As List(Of String)
        Return (From ric As XmlNode In node.SelectNodes("ric") Select ric.InnerText).ToList()
    End Function

    Public Overrides Function GetDefaultRics() As List(Of String)
        Return _bondRics
    End Function
End Class

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
    Private ReadOnly _order As Integer         ' Order of given source in given portfolio
    Private ReadOnly _source As Source
    Private ReadOnly _condition As String
    Private ReadOnly _customName As String
    Private ReadOnly _customColor As String
    Private ReadOnly _included As Boolean

    Protected Overloads Function Equals(ByVal other As PortfolioSource) As Boolean
        Return Equals(_source, other._source)
    End Function

    Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
        If ReferenceEquals(Nothing, obj) Then Return False
        If ReferenceEquals(Me, obj) Then Return True
        If obj.GetType IsNot Me.GetType Then Return False
        Return Equals(DirectCast(obj, PortfolioSource))
    End Function

    Public Overrides Function GetHashCode() As Integer
        If _source Is Nothing Then Return 0
        Return _source.GetHashCode
    End Function

    Public Shared Operator =(ByVal left As PortfolioSource, ByVal right As PortfolioSource) As Boolean
        Return Equals(left, right)
    End Operator

    Public Shared Operator <>(ByVal left As PortfolioSource, ByVal right As PortfolioSource) As Boolean
        Return Not Equals(left, right)
    End Operator

    Public Sub New(ByVal order As Integer, ByVal source As Source, ByVal condition As String, ByVal customName As String, ByVal customColor As String, ByVal included As Boolean)
        _order = order
        _source = source
        _condition = condition
        _customName = customName
        _customColor = customColor
        _included = included
    End Sub

    Public ReadOnly Property Order As Integer
        Get
            Return _order
        End Get
    End Property

    Public ReadOnly Property Source As Source
        Get
            Return _source
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
    Public Const All As Byte = List Or Chain

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
            data.Sort(Function(item1, item2) item1.Order.CompareTo(item1.Order))
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

    Public ReadOnly Property Rics(ByVal source As PortfolioSource) As ReadOnlyCollection(Of String)
        Get
            Return New ReadOnlyCollection(Of String)(_rics(source))
        End Get
    End Property

    Public ReadOnly Property Rics(Optional ByVal netted As Boolean = False) As ReadOnlyCollection(Of RicDescription)
        Get
            Dim res As New List(Of RicDescription)
            For Each src In _sources
                Dim description As RicDescription
                For Each ric In src.Source.GetDefaultRics()
                    If netted AndAlso _excludes.Contains(ric) Then Continue For
                    Try
                        Dim descr = BondsData.Instance.GetBondInfo(ric)
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

        For Each src In (From srcs In _sources Where srcs.Included)
            Dim list = New List(Of String)(src.Source.GetDefaultRics())
            list.RemoveAll(Function(item) _excludes.Contains(item))
            _rics.Add(src, list)
        Next
        ' todo now apply static filtering
        ' TODO make a difference between dynamic as static filtering!!!
    End Sub
End Class

Public Interface IPortfolioManager
    '' Returns flat structure of portfolios: only id-name pairs, disregarding any folder structure
    Function GetPortfoliosFlat() As List(Of Tuple(Of Integer, String))

    Function GetPortfoliosByFolder(ByVal id As String) As List(Of PortfolioItemDescription)

    '' Returns RICs of all chains given in XML
    Function GetChainRics() As List(Of String)

    '' Returns portfolio as a set of sources, each having different settings
    Function GetPortfolioStructure(ByVal currentPortID As Long) As PortfolioStructure
    Function GetFolderDescr(ByVal id As String) As PortfolioItemDescription

    Sub SetFolderName(ByVal id As String, ByVal name As String)
    Sub SetPortfolioName(ByVal id As String, ByVal name As String)

    Sub DeleteFolder(ByVal id As String)
    Sub DeletePortfolio(ByVal id As String)

    Function AddFolder(ByVal text As String, Optional ByVal id As String = "") As Long
    Function AddPortfolio(ByVal text As String, Optional ByVal id As String = "") As Long
    Function MoveItemToFolder(ByVal whoId As String, ByVal whereId As String) As String
    Function MoveItemToTop(ByVal id As String) As String
    Function CopyItemToFolder(ByVal whoId As String, ByVal whereId As String) As String
    Function CopyItemToTop(ByVal id As String) As String

    Function PortfoliosValid() As Boolean ' todo make a function BranchValid so that to be able to find a good branch and show it
    ReadOnly Property ChainsView() As List(Of Chain)
    ReadOnly Property UserListsView() As List(Of UserList)
End Interface

Public Class PortfolioItemDescription
    Public IsFolder As Boolean
    Public Name As String
    Public Id As String

    Public Sub New(ByVal isFolder As Boolean, ByVal name As String, ByVal id As String)
        Me.IsFolder = isFolder
        Me.Name = name
        Me.Id = id
    End Sub
End Class

Public Class PortfolioManager
    ' TODO multiple config files
    ' TOdO events OnLoadConfig, OnConfigFileUpdated
    Implements IPortfolioManager

    Private Const ConfigFile As String = "bonds.xml"
    Private ReadOnly _configXml As String = Path.Combine(Utils.GetMyPath(), ConfigFile)
    Private ReadOnly _bonds As New XmlDocument

    Private ReadOnly _ids As New HashSet(Of Long)
    Private ReadOnly _tmpIds As New HashSet(Of Long)

    Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(PortfolioManager))

    Private Shared _instance As PortfolioManager

    Private Sub New()
        _bonds.Load(_configXml)
    End Sub

    Public Function GetPortfoliosByFolder(ByVal id As String) As List(Of PortfolioItemDescription) Implements IPortfolioManager.GetPortfoliosByFolder
        Dim res As New List(Of PortfolioItemDescription)
        Dim xPathFolder As String
        Dim xPathPort As String

        If id <> "" Then
            xPathFolder = String.Format("/bonds/portfolios//folder[@id='{0}']/folder", id)
            xPathPort = String.Format("/bonds/portfolios//folder[@id='{0}']/portfolio", id)
        Else
            xPathFolder = "/bonds/portfolios/folder"
            xPathPort = "/bonds/portfolios/portfolio"
        End If

        Dim iter As XmlNodeList

        iter = _bonds.SelectNodes(xPathFolder)
        For i = 0 To iter.Count - 1
            Dim attributes = iter(i).Attributes
            If attributes.ItemOf("id") IsNot Nothing AndAlso attributes.ItemOf("name") IsNot Nothing Then
                res.Add(New PortfolioItemDescription(True, attributes("name").Value, attributes("id").Value))
            End If
        Next
        iter = _bonds.SelectNodes(xPathPort)
        For i = 0 To iter.Count - 1
            Dim attributes = iter(i).Attributes
            If attributes.ItemOf("id") IsNot Nothing AndAlso attributes.ItemOf("name") IsNot Nothing Then
                res.Add(New PortfolioItemDescription(False, attributes("name").Value, attributes("id").Value))
            End If
        Next
        Return res
    End Function

    Public Function GetChainRics() As List(Of String) Implements IPortfolioManager.GetChainRics
        Dim res As New List(Of String)
        Dim iter = _bonds.SelectNodes("/bonds/chains/chain[not(@enabled='False')]/@ric")
        For i = 0 To iter.Count - 1
            res.Add(iter(i).Value)
        Next
        Return res
    End Function

    Public Function GetPortfolioStructure(ByVal id As Long) As PortfolioStructure Implements IPortfolioManager.GetPortfolioStructure
        Logger.Info("GetPortfolioStructure({0})", id)
        Dim node = _bonds.SelectSingleNode(String.Format("/bonds/portfolios//portfolio[@id='{0}']", id))
        If node Is Nothing Then Return Nothing
        Try
            Dim res As New PortfolioStructure(id, node.Attributes("name").Value)

            Dim nodes = node.SelectNodes("include | exclude")
            'Dim order As Integer
            Dim source As Source
            For Each item As XmlNode In nodes
                Try
                    source = Nothing

                    Dim isIncluded = (item.Name = "include")
                    Dim what = item.Attributes("what").Value
                    Dim condition = GetAttr(item, "condition")
                    Dim customName = GetAttr(item, "name")
                    Dim customColor = GetAttr(item, "color")
                    Dim order = GetAttr(item, "order", "1") ' todo order field. Requires special treatment on saving

                    Select Case what
                        Case "chain"
                            Dim chainId = item.Attributes("id").Value
                            source = GetChainDescr(chainId)
                        Case "list"
                            Dim listId = item.Attributes("id").Value
                            source = GetListDescr(listId)
                        Case Else
                            Logger.Warn("Unsupported item {0}", what)
                    End Select

                    If source IsNot Nothing Then
                        If customColor = "" Then customColor = source.Color
                        If customName = "" Then customName = source.Name
                        Dim portSource As New PortfolioSource(order, source, condition, customName, customColor, isIncluded)
                        res.AddSource(portSource)
                    Else
                        Logger.Info("Failed to read description for item [{0}]", what)
                    End If
                Catch ex As Exception
                    Logger.WarnException("Failed to parse ", ex)
                    Logger.Warn("Exception = {0}", ex)
                End Try
            Next

            Return res
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Shared Function GetAttr(ByVal node As XmlNode, ByVal name As String, Optional ByVal defaultValue As String = "") As String
        Dim attribute As XmlAttribute = node.Attributes(name)
        If attribute IsNot Nothing Then
            Return attribute.Value
        Else
            Return defaultValue
        End If
    End Function

    Private Shared Function GetAttrStrict(ByVal node As XmlNode, ByVal name As String) As String
        Dim attribute As XmlAttribute = node.Attributes(name)
        If attribute IsNot Nothing Then
            Return attribute.Value
        Else
            Throw New Exception(String.Format("Failed to find attribute {0} in node {1}", name, node.Name))
        End If
    End Function

    Private Function GetChainDescr(ByVal chainId As String) As Chain
        Dim node = _bonds.SelectSingleNode(String.Format("/bonds/chains/chain[@id='{0}']", chainId))
        If node Is Nothing Then Return Nothing
        Try
            Dim color = GetAttrStrict(node, "color")
            Dim name = GetAttrStrict(node, "name")
            Dim enabled = GetAttr(node, "enabled", "True")
            Dim curve = GetAttr(node, "curve", "False")
            Dim chainRic = GetAttrStrict(node, "ric")
            Dim fields = New Fields(GetAttrStrict(node, "field-set-id"), _bonds)
            Return New Chain(chainId, color, fields, enabled, curve, chainRic, name)
        Catch ex As Exception
            Logger.WarnException("Failed to get chain description", ex)
            Logger.Warn("Exception = {0}", ex.ToString())
            Return Nothing
        End Try
    End Function

    Private Function GetListDescr(ByVal listId As String) As UserList
        Dim node = _bonds.SelectSingleNode(String.Format("/bonds/lists/list[@id='{0}']", listId))
        If node Is Nothing Then Return Nothing
        Try
            Dim color = GetAttrStrict(node, "color")
            Dim name = GetAttrStrict(node, "name")
            Dim enabled = GetAttr(node, "enabled", "True")
            Dim curve = GetAttr(node, "curve", "False")
            Dim rics = UserList.ExtractRics(node)
            Dim fields = New Fields(GetAttrStrict(node, "field-set-id"), _bonds)
            Return New UserList(listId, color, fields, enabled, curve, rics, name)
        Catch ex As Exception
            Logger.WarnException("Failed to get list description", ex)
            Logger.Warn("Exception = ", ex.ToString())
            Return Nothing
        End Try
    End Function

    Public Sub SetFolderName(ByVal id As String, ByVal name As String) Implements IPortfolioManager.SetFolderName
        Dim node = _bonds.SelectSingleNode(String.Format("/bonds/portfolios//folder[@id='{0}']/@name", id))
        node.Value = name
        SaveBonds()
    End Sub

    Public Sub SetPortfolioName(ByVal id As String, ByVal name As String) Implements IPortfolioManager.SetPortfolioName
        Dim node = _bonds.SelectSingleNode(String.Format("/bonds/portfolios//portfolio[@id='{0}']/@name", id))
        node.Value = name
        SaveBonds()
    End Sub

    Private Function Add(ByVal text As String, ByVal type As String, Optional ByVal id As String = "") As Long
        Dim parent As XmlNode
        If id <> "" Then
            parent = _bonds.SelectSingleNode(String.Format("/bonds/portfolios//folder[@id='{0}']", id))
        Else
            parent = _bonds.SelectSingleNode("/bonds/portfolios")
        End If
        Dim folder As XmlNode = _bonds.CreateNode(XmlNodeType.Element, type, "")

        Dim idAttr As XmlAttribute = _bonds.CreateAttribute("id")
        Dim newId = GenerateNewId()
        idAttr.Value = newId

        Dim nameAttr As XmlAttribute = _bonds.CreateAttribute("name")
        nameAttr.Value = text

        folder.Attributes.Append(idAttr)
        folder.Attributes.Append(nameAttr)
        parent.AppendChild(folder)

        SaveBonds()

        Return newId
    End Function

    Private Function GenerateNewId() As Long
        Dim rnd As New Random(DateTime.Now.Millisecond)
        Dim elem As Long
        Do
            elem = CLng(rnd.NextDouble() * 100000)
        Loop While _ids.Contains(elem) Or _tmpIds.Contains(elem)
        _tmpIds.Add(elem)
        Return elem
    End Function

    Public Sub DeleteFolder(ByVal id As String) Implements IPortfolioManager.DeleteFolder
        Dim node = _bonds.SelectSingleNode(String.Format("/bonds/portfolios//folder[@id='{0}']", id))
        Dim parent = node.ParentNode

        parent.RemoveChild(node)
        SaveBonds()
    End Sub

    Public Sub DeletePortfolio(ByVal id As String) Implements IPortfolioManager.DeletePortfolio
        Dim node = _bonds.SelectSingleNode(String.Format("/bonds/portfolios//portfolio[@id='{0}']", id))
        Dim parent = node.ParentNode

        parent.RemoveChild(node)
        SaveBonds()
    End Sub

    Public Function AddFolder(ByVal text As String, Optional ByVal id As String = "") As Long Implements IPortfolioManager.AddFolder
        Return Add(text, "folder", id)
    End Function

    Public Function AddPortfolio(ByVal text As String, Optional ByVal id As String = "") As Long Implements IPortfolioManager.AddPortfolio
        Return Add(text, "portfolio", id)
    End Function

    Private Function DoCopyMove(ByVal move As Boolean, ByVal whoId As String, Optional ByVal whereId As String = "") As String
        Dim whoNode = _bonds.SelectSingleNode(String.Format("/bonds/portfolios//folder[@id='{0}'] | /bonds/portfolios//portfolio[@id='{0}']", whoId))
        If whoNode Is Nothing Then Return whoId

        Dim whereNode As XmlNode
        If whereId <> "" Then
            whereNode = _bonds.SelectSingleNode(String.Format("/bonds/portfolios//folder[@id='{0}'] | /bonds/portfolios//portfolio[@id='{0}']", whereId))
        Else
            whereNode = _bonds.SelectSingleNode("/bonds/portfolios")
        End If
        If whereNode Is Nothing Then Return whoId

        Dim res As String
        If move AndAlso whoNode.ParentNode IsNot Nothing Then
            whoNode.ParentNode.RemoveChild(whoNode)
            whereNode.AppendChild(whoNode)
            res = whoId
        ElseIf Not move Then
            Dim clone As XmlNode = DeepClone(whoNode)
            whereNode.AppendChild(clone)
            res = clone.Attributes("id").Value
        Else
            res = ""
        End If

        If res <> "" Then SaveBonds()
        Return res
    End Function

    Private Function DeepClone(ByVal who As XmlNode) As XmlNode
        Dim res As XmlNode = who.CloneNode(False)
        If res.Name = "folder" Or res.Name = "portfolio" Then
            res.Attributes("id").Value = GenerateNewId()
        End If
        For Each node As XmlNode In who.ChildNodes
            res.AppendChild(DeepClone(node))
        Next
        Return res
    End Function

    Public Function MoveItemToFolder(ByVal whoId As String, ByVal whereId As String) As String Implements IPortfolioManager.MoveItemToFolder
        Return DoCopyMove(True, whoId, whereId)
    End Function

    Public Function MoveItemToTop(ByVal id As String) As String Implements IPortfolioManager.MoveItemToTop
        Return DoCopyMove(True, id)
    End Function

    Public Function CopyItemToTop(ByVal id As String) As String Implements IPortfolioManager.CopyItemToTop
        Return DoCopyMove(False, id)
    End Function

    Public Function CopyItemToFolder(ByVal whoId As String, ByVal whereId As String) As String Implements IPortfolioManager.CopyItemToFolder
        Return DoCopyMove(False, whoId, whereId)
    End Function

    Public Function PortfoliosValid() As Boolean Implements IPortfolioManager.PortfoliosValid
        Dim allIds = _bonds.SelectNodes("/bonds/portfolios//folder/@id | /bonds/portfolios//portfolio/@id")
        Dim maxCounts = (
            From v As XmlAttribute In allIds
            Let val = v.Value
            Group val By val Into cnt = Count(val)
            Select cnt).ToList()
        Return Not maxCounts.Any OrElse maxCounts.Max = 1
    End Function

    Public ReadOnly Property ChainsView() As List(Of Chain) Implements IPortfolioManager.ChainsView
        Get
            Dim chainIds = _bonds.SelectNodes("/bonds/chains/chain/@id")
            Return (From id As XmlNode In chainIds Select GetChainDescr(id.Value)).ToList()
        End Get
    End Property

    Public ReadOnly Property UserListsView() As List(Of UserList) Implements IPortfolioManager.UserListsView
        Get
            Dim chainIds = _bonds.SelectNodes("/bonds/lists/list/@id")
            Return (From id As XmlNode In chainIds Select GetListDescr(id.Value)).ToList()
        End Get
    End Property

    Private Sub SaveBonds()
        _ids.Clear()
        _tmpIds.Clear()

        _bonds.Save(_configXml)

        Dim idNodes = _bonds.SelectNodes("/bonds/portfolios//portfolio/@id | /bonds/portfolios//folder/@id")
        For Each node As XmlNode In idNodes
            _ids.Add(CLng(node.Value))
        Next
    End Sub

    Public Function GetFolderDescr(ByVal id As String) As PortfolioItemDescription Implements IPortfolioManager.GetFolderDescr
        Dim node = _bonds.SelectSingleNode(String.Format("/bonds/portfolios//folder[@id='{0}']", id))
        If node Is Nothing Then Return Nothing
        Dim attributes = node.Attributes
        Return New PortfolioItemDescription(True, attributes("name").Value, attributes("id").Value)
    End Function

    Public Function Portfolios() As List(Of Tuple(Of Integer, String)) Implements IPortfolioManager.GetPortfoliosFlat
        Dim res As New List(Of Tuple(Of Integer, String))
        Dim iter = _bonds.SelectNodes("/bonds/portfolios//portfolio")
        For i = 0 To iter.Count - 1
            res.Add(Tuple.Create(CInt(iter(i).SelectSingleNode("@id").Value), iter(i).SelectSingleNode("@name").Value))
        Next
        Return res
    End Function

    Public Shared Function GetInstance() As IPortfolioManager
        If _instance Is Nothing Then _instance = New PortfolioManager()
        Return _instance
    End Function
End Class