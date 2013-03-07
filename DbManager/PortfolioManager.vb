Imports System.Xml
Imports System.IO
Imports NLog
Imports Uitls

Public Class PortfolioSource
    'todo ??? Wuts vet?
    Public Id As Integer
    Public Name As Integer
    Public Fields As Fields
    Public Color As String
    Public Rics As List(Of String)
End Class

Public Interface IPortfolioManager
    '' Returns flat structure of portfolios: only id-name pairs, disregarding any folder structure
    Function GetPortfoliosFlat() As List(Of Tuple(Of Integer, String))

    Function GetPortfoliosByFolder(ByVal id As String) As List(Of PortfolioItemDescription)

    '' Returns RICs of all chains given in XML
    Function GetChainRics() As List(Of String)

    '' Returns portfolio as a set of sources, each having different settings
    Function GetPortfolioStructure(ByVal currentPortID As Long) As List(Of PortfolioSource)
    Function GetFolderDescr(ByVal id As String) As PortfolioItemDescription

    Sub SetFolderName(ByVal id As String, ByVal name As String)
    Sub SetPortfolioName(ByVal id As String, ByVal name As String)

    Sub DeleteFolder(ByVal id As String)
    Sub DeletePortfolio(ByVal id As String)

    Function AddFolder(ByVal text As String, Optional ByVal id As String = "") As Long
    Function AddPortfolio(ByVal text As String, Optional ByVal id As String = "") As Long
    Function MoveItemToFolder(ByVal whoId As String, ByVal whereId As String) As String
    Function MoveItemToTop(ByVal id As String) As String
    Function CopyItemToTop(ByVal id As String) As String
    Function CopyItemToFolder(ByVal whoId As String, ByVal whereId As String) As String
    Function PortfoliosValid() As Boolean
End Interface

Public Class PortfolioItemDescription
    Public IsFolder As Boolean
    Public Name As String
    Public Id As String
    Public ReadOnly Property NodeId() As String
        Get
            Return If(IsFolder, "Folder", "Node") + Id
        End Get
    End Property

    Public Sub New(ByVal isFolder As Boolean, ByVal name As String, ByVal id As String)
        Me.IsFolder = isFolder
        Me.Name = name
        Me.Id = id
    End Sub
End Class

Public Class FieldSet
    Public Bid As String = Nothing
    Public Ask As String = Nothing
    Public Last As String = Nothing
    Public VWAP As String = Nothing
    Public Hist As String = Nothing     ' Historical Field. Corresponds to Close
    Public HistDate As String = Nothing
    Public Volume As String = Nothing
End Class

Public Class Fields
    Public History As New FieldSet
    Public Realtime As New FieldSet
End Class


Public Class PortfolioManager
    ' TODO multiple files
    ' TOOO events OnLoadConfig, OnConfigFileUpdated
    Implements IPortfolioManager

    Private Const ConfigFile As String = "bonds.xml"
    Private ReadOnly _configXml As String = Path.Combine(Utils.GetMyPath(), ConfigFile)
    Private ReadOnly _bonds As New XmlDocument

    Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(PortfolioManager))

    Private Shared _instance As PortfolioManager

    Private Sub New()
        _bonds.Load(_configXML)
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

    Public Function GetPortfolioStructure(ByVal currentPortID As Long) As List(Of PortfolioSource) Implements IPortfolioManager.GetPortfolioStructure
        Throw New NotImplementedException()
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

    Private ReadOnly _ids As New HashSet(Of Long)
    Private ReadOnly _tmpIds As New HashSet(Of Long)

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
