Imports System.Xml
Imports System.IO
Imports NLog
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

Public Structure FieldSet
    <ConfingName("TIME")> Public Time As String = ""
    <ConfingName("BID")> Public Bid As String = ""
    <ConfingName("ASK")> Public Ask As String = ""
    <ConfingName("LAST")> Public Last As String = ""
    <ConfingName("VWAP")> Public VWAP As String = ""
    <ConfingName("HIST")> Public Hist As String = ""     ' Historical Field. Corresponds to Close
    <ConfingName("HIST_DATE")> Public HistDate As String = ""
    <ConfingName("VOLUME")> Public Volume As String = ""
    <ConfingName("SOURCE")> Public Src As String = ""
End Structure

Public Structure Fields
    Public History As New FieldSet
    Public Realtime As New FieldSet
End Structure

Public Class Source
    Private ReadOnly _id As String
    Private ReadOnly _color As String
    Private ReadOnly _fields As Fields
    Private ReadOnly _enabled As Boolean
    Private ReadOnly _curve As Boolean

    Public Sub New(ByVal id As String, ByVal color As String, ByVal fields As Fields, ByVal enabled As Boolean, ByVal curve As Boolean)
        _id = id
        _color = color
        _fields = fields
        _enabled = enabled
        _curve = curve
    End Sub

    Public ReadOnly Property ID As String
        Get
            Return _id
        End Get
    End Property

    Public ReadOnly Property Color As String
        Get
            Return _color
        End Get
    End Property

    Public ReadOnly Property Fields As Fields
        Get
            Return _fields
        End Get
    End Property

    Public ReadOnly Property Enabled As Boolean
        Get
            Return _enabled
        End Get
    End Property

    Public ReadOnly Property Curve As Boolean
        Get
            Return _curve
        End Get
    End Property
End Class

Public Class Chain
    Inherits Source
    Private ReadOnly _chainRic As String

    Public Sub New(ByVal id As String, ByVal color As String, ByVal fields As Fields, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal chainRic As String)
        MyBase.New(id, color, fields, enabled, curve)
        _chainRic = chainRic
    End Sub

    Public ReadOnly Property ChainRic As String
        Get
            Return _chainRic
        End Get
    End Property
End Class

Public Class UserList
    Inherits Source
    Private ReadOnly _bondRics As List(Of String)

    Public Sub New(ByVal id As String, ByVal color As String, ByVal fields As Fields, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal bondRics As List(Of String))
        MyBase.New(id, color, fields, enabled, curve)
        _bondRics = bondRics
    End Sub

    Public ReadOnly Property BondRics As List(Of String)
        Get
            Return _bondRics
        End Get
    End Property
End Class

Public Class PortfolioSource
    Private ReadOnly _order As Integer         ' Order of given source in given portfolio
    Private ReadOnly _source As Source
    Private ReadOnly _condition As String
    Private ReadOnly _customName As String
    Private ReadOnly _included As Boolean

    Public Sub New(ByVal order As Integer, ByVal source As Source, ByVal condition As String, ByVal customName As String, ByVal included As Boolean)
        _order = order
        _source = source
        _condition = condition
        _customName = customName
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

    Public ReadOnly Property CustomName As String
        Get
            Return _customName
        End Get
    End Property

    Public ReadOnly Property Included As Boolean
        Get
            Return _included
        End Get
    End Property
End Class

Public Class PortfolioStructure
    Private ReadOnly _id As String
    Private ReadOnly _name As String
    Private _sources As List(Of PortfolioSource)

    Public Sub New(ByVal id As String, ByVal name As String, ByVal sources As List(Of PortfolioSource))
        _id = id
        _name = name
        _sources = sources
    End Sub

    Public Sub New(ByVal id As String, ByVal name As String)
        _id = id
        _name = name
    End Sub

    Public Property Sources As List(Of PortfolioSource)
        Get
            Return _sources
        End Get
        Friend Set(ByVal value As List(Of PortfolioSource))
            _sources = value
        End Set
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
    ' TODO multiple files
    ' TOOO events OnLoadConfig, OnConfigFileUpdated
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

            Dim nodes = node.SelectNodes("/include | /exclude")
            Dim order As Integer
            For Each item As XmlNode In nodes
                Try
                    Dim source As Source

                    Dim isIncluded = (node.Name = "include")
                    Dim what = item.Attributes("what").Value
                    Dim condition = GetAttr(item, "condition")
                    Dim customName = GetAttr(item, "name")

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
                        order = order + 1
                        Dim portSource As New PortfolioSource(order, source, condition, customName, isIncluded)
                        res.Sources.Add(portSource)
                    Else
                        Logger.Info("Failed to read description for item {0}", what)
                    End If
                Catch ex As Exception
                    Logger.Warn("Failed to parse ")
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

    Private Function GetAttrStrict(ByVal node As XmlNode, ByVal name As String) As String
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
            Dim enabled = GetAttr(node, "enabled", "True")
            Dim curve = GetAttr(node, "curve", "False")
            Dim chainRic = GetAttrStrict(node, "ric")
            Dim fields = GetFields(GetAttrStrict(node, "field-set-id"))
            Return New Chain(chainId, color, fields, enabled, curve, chainRic)
        Catch ex As Exception
            Logger.WarnException("Failed to get chain description", ex)
            Logger.Warn("Exception = ", ex.ToString())
            Return Nothing
        End Try
    End Function

    Private Function GetListDescr(ByVal listId As String) As UserList
        Dim node = _bonds.SelectSingleNode(String.Format("/bonds/chains/list[@id='{0}']", listId))
        If node Is Nothing Then Return Nothing
        Try
            Dim color = GetAttrStrict(node, "color")
            Dim enabled = GetAttr(node, "enabled", "True")
            Dim curve = GetAttr(node, "curve", "False")
            Dim rics = GetListRics(listId)
            Dim fields = GetFields(GetAttrStrict(node, "field-set-id"))
            Return New UserList(listId, color, fields, enabled, curve, rics)
        Catch ex As Exception
            Logger.WarnException("Failed to get chain description", ex)
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
