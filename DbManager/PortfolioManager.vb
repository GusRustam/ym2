Imports System.Xml
Imports System.IO
Imports DbManager.Bonds
Imports System.Globalization
Imports NLog
Imports Uitls

Public Interface IPortfolioManager
    '' Returns flat structure of portfolios: only id-name pairs, disregarding any folder structure
    Function GetAllPortfolios() As List(Of IdName(Of String))

    Function GetPortfoliosByFolder(ByVal id As String) As List(Of Portfolio)

    '' Returns RICs of all chains given in XML
    Function GetChainRics() As List(Of String)

    '' Returns portfolio as a set of sources, each having different settings
    Function GetPortfolioStructure(ByVal currentPortID As Long) As PortfolioStructure
    Function GetFolderDescr(ByVal id As String) As Portfolio

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
    ReadOnly Property ChainsView() As List(Of ChainSrc)
    ReadOnly Property UserListsView() As List(Of UserListSrc)
    ReadOnly Property UserQueriesView() As List(Of UserQuerySrc)
    ReadOnly Property CustomBondsView() As List(Of CustomBondSrc)
    ReadOnly Property CurveChainsView() As List(Of ChainCurveSrc)
    Function GetFieldLayouts() As List(Of IdName(Of String))
    Sub UpdateFieldSet(ByVal id As String, ByVal type As String, ByVal fields As Dictionary(Of String, String))

    Sub AddSource(ByVal src As SourceBase)
    Sub UpdateSource(ByVal source As SourceBase)
    Function GetPortfoliosBySource(ByVal selectedItem As SourceBase) As List(Of IdName(Of String))
    Sub DeleteSource(ByVal source As SourceBase)

    Sub SelectConfigFile(ByVal fileName As String)
    Function ConfigFile() As String
    Sub SelectDefaultConfigFile()
End Interface

Friend Interface IPortfolioManagerLocal
    Function GenerateNewListId() As String
    Function GenerateNewChainId() As String
    Function GetConfigDocument() As XmlDocument
End Interface

Public Class PortfolioManager
    Implements IPortfolioManager
    Implements IPortfolioManagerLocal

    Private Const DefaultConfigFile As String = "bonds.xml"
    Private _configFile As String = DefaultConfigFile

    Public Property ConfigFile() As String
        Get
            Return _configFile
        End Get
        Set(ByVal value As String)
            _configFile = value
            LoadBonds()
        End Set
    End Property

    Private ReadOnly _configXml As String = Path.Combine(Utils.GetMyPath(), ConfigFile)
    Private ReadOnly _bonds As New XmlDocument

    Private ReadOnly _ids As New HashSet(Of Long)
    Private ReadOnly _tmpIds As New HashSet(Of Long)

    Private ReadOnly _chainIds As New HashSet(Of Long)
    Private ReadOnly _listIds As New HashSet(Of Long)
    Private ReadOnly _customBondsIds As New HashSet(Of Long)

    Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(PortfolioManager))

    Private Shared _instance As PortfolioManager
    Private Shared ReadOnly Rnd As Random = New Random(DateTime.Now.Millisecond)

    Private Sub New()
        LoadBonds()
    End Sub

    Private Sub LoadBonds()
        _bonds.Load(_configXml)
        _ids.Import(LoadIds(_bonds.SelectNodes("/bonds/portfolios//@id")))
        _chainIds.Import(LoadIds(_bonds.SelectNodes("/chains/chain/@id")))
        _listIds.Import(LoadIds(_bonds.SelectNodes("/lists/list/@id")))
        _customBondsIds.Import(LoadIds(_bonds.SelectNodes("/custom-bonds/bond/@id")))
    End Sub

    Public Sub SaveBonds()
        _ids.Clear()
        _tmpIds.Clear()

        _bonds.Save(_configXml)
        LoadBonds()
    End Sub

    Private Function LoadIds(ByVal nodes As XmlNodeList) As HashSet(Of Long)
        Dim res As New HashSet(Of Long)
        For Each node As XmlNode In nodes
            If Not IsNumeric(node.Value) Then
                Logger.Warn("Node {0} id is not numeric: {1}", node.Name, node.Value)
            Else
                Dim id = CLng(node.Value)
                If _ids.Contains(id) Then
                    Logger.Error("Duplicate ids in {0}: {1}", node.Name, id)
                Else
                    res.Add(node.Value)
                End If
            End If
        Next
        Return res
    End Function

    Public Function GetPortfoliosByFolder(ByVal id As String) As List(Of Portfolio) Implements IPortfolioManager.GetPortfoliosByFolder
        Dim res As New List(Of Portfolio)
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
                res.Add(New Portfolio(True, attributes("name").Value, attributes("id").Value))
            End If
        Next
        iter = _bonds.SelectNodes(xPathPort)
        For i = 0 To iter.Count - 1
            Dim attributes = iter(i).Attributes
            If attributes.ItemOf("id") IsNot Nothing AndAlso attributes.ItemOf("name") IsNot Nothing Then
                res.Add(New Portfolio(False, attributes("name").Value, attributes("id").Value))
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
            Dim source As SourceBase
            For Each item As XmlNode In nodes
                Try
                    source = Nothing
                    Dim srcId As String
                    Dim isIncluded = (item.Name = "include")
                    Dim what = item.Attributes("what").Value
                    Dim condition = item.GetAttr("condition")
                    Dim customName = item.GetAttr("name")
                    Dim customColor = item.GetAttr("color")

                    Select Case what
                        Case "chain"
                            srcId = item.Attributes("id").Value
                            source = ChainSrc.Load(srcId)
                        Case "list"
                            srcId = item.Attributes("id").Value
                            source = UserListSrc.Load(srcId)
                        Case "query"
                            srcId = item.Attributes("id").Value
                            source = UserQuerySrc.Load(srcId)
                        Case "custom-bond"
                            srcId = item.Attributes("id").Value
                            source = CustomBondSrc.LoadById(srcId)
                        Case "ric"
                            source = New RegularBondSrc(item.Attributes("id").Value,
                                                        item.Attributes("color").Value,
                                                        item.Attributes("name").Value,
                                                        item.Attributes("field-layout").Value,
                                                        item.Attributes("rics").Value)

                        Case Else
                            srcId = -1
                            Logger.Warn("Unsupported item {0}", what)
                    End Select

                    If source IsNot Nothing Then
                        If customColor = "" Then customColor = source.Color
                        If customName = "" Then customName = source.Name
                        Dim portSource As New PortfolioSource(srcId, source, condition, customName, customColor, isIncluded, GetPortfolio(id))
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

    Private Function GetPortfolio(ByVal id As Long) As Portfolio
        Dim node = _bonds.SelectSingleNode(String.Format("/bonds/portfolios//portfolio[@id='{0}']", id))
        Return New Portfolio(False, node.GetAttrStrict("name"), id)
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
        If parent Is Nothing Then
            parent = _bonds.SelectSingleNode(String.Format("/bonds/portfolios//folder[portfolio[@id='{0}']]", id))
            If parent Is Nothing Then parent = _bonds.SelectSingleNode("/bonds/portfolios")
            If parent Is Nothing Then Throw New NoPortfolioException()
        End If

        Dim folder As XmlNode = _bonds.CreateNode(XmlNodeType.Element, type, "")

        Dim idAttr As XmlAttribute = _bonds.CreateAttribute("id")
        Dim newId = GenerateNewId(New HashSet(Of Long)(_ids.Union(_tmpIds)))
        _tmpIds.Add(newId)

        idAttr.Value = newId

        Dim nameAttr As XmlAttribute = _bonds.CreateAttribute("name")
        nameAttr.Value = text

        folder.Attributes.Append(idAttr)
        folder.Attributes.Append(nameAttr)
        parent.AppendChild(folder)

        SaveBonds()

        Return newId
    End Function

    Private Shared Function GenerateNewId(ByVal keys As HashSet(Of Long)) As Long
        Dim elem As Long
        Do
            elem = CLng(Rnd.NextDouble() * 100000)
        Loop While keys.Contains(elem)
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
            res.Attributes("id").Value = GenerateNewId(New HashSet(Of Long)(_ids.Union(_tmpIds)))
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

    Public ReadOnly Property ChainsView() As List(Of ChainSrc) Implements IPortfolioManager.ChainsView
        Get
            Dim chainIds = _bonds.SelectNodes("/bonds/chains/chain/@id")
            Return (From id As XmlNode In chainIds Select ChainSrc.Load(id.Value)).ToList()
        End Get
    End Property

    Public ReadOnly Property UserListsView() As List(Of UserListSrc) Implements IPortfolioManager.UserListsView
        Get
            Dim chainIds = _bonds.SelectNodes("/bonds/lists/list/@id")
            Dim tmp = chainIds
            Dim res = (From id As XmlNode In tmp Select UserListSrc.Load(id.Value)).ToList()
            chainIds = _bonds.SelectNodes("/bonds/queries/query/@id")
            res.AddRange((From id As XmlNode In chainIds Select UserQuerySrc.Load(id.Value)).Cast(Of UserListSrc))
            Return res
        End Get
    End Property

    Public ReadOnly Property UserQueriesView() As List(Of UserQuerySrc) Implements IPortfolioManager.UserQueriesView
        Get
            Dim chainIds = _bonds.SelectNodes("/bonds/queries/query/@id")
            Return (From id As XmlNode In chainIds Select UserQuerySrc.Load(id.Value)).ToList()
        End Get
    End Property


    Public ReadOnly Property CurveChainsView() As List(Of ChainCurveSrc) Implements IPortfolioManager.CurveChainsView
        Get
            Dim nodes = _bonds.SelectNodes("/bonds/chain-curves/curve")
            Return (From nd As XmlNode In nodes Select ChainCurveSrc.LoadById(nd.GetAttrStrict("id"))).ToList
        End Get
    End Property

    Public Function GetFieldLayouts() As List(Of IdName(Of String)) Implements IPortfolioManager.GetFieldLayouts
        Try
            Return (From node As XmlNode In _bonds.SelectNodes("/bonds/field-sets/field-set")
                    Select New IdName(Of String)(node.Attributes("id").Value, node.Attributes("name").Value)).ToList()
        Catch ex As Exception
            Logger.ErrorException("Failed to read list of all nodes", ex)
            Logger.Error("Exception = {0}", ex.ToString())
            Return New List(Of IdName(Of String))()
        End Try
    End Function

    Public Sub UpdateFieldSet(ByVal id As String, ByVal type As String, ByVal fields As Dictionary(Of String, String)) Implements IPortfolioManager.UpdateFieldSet
        Dim parent = _bonds.SelectSingleNode(String.Format("/bonds/field-sets/field-set[@id='{0}']", id))
        If parent Is Nothing Then Throw New FieldException(String.Format("Failed to find field set with id {0} ", id))

        Dim child = parent.SelectSingleNode(type)
        If child IsNot Nothing Then
            parent.RemoveChild(child)
        Else
            Logger.Warn("No field set with id {0} and type {1}", id, type)
        End If

        Dim kid = _bonds.CreateNode(XmlNodeType.Element, type, "")
        For Each nameVal In fields
            Dim fieldNode = _bonds.CreateNode(XmlNodeType.Element, "field", "")
            Dim attr = _bonds.CreateAttribute("type")
            attr.Value = nameVal.Key
            fieldNode.Attributes.Append(attr)
            fieldNode.InnerText = nameVal.Value
            kid.AppendChild(fieldNode)
        Next
        parent.AppendChild(kid)
        SaveBonds()
    End Sub

    Friend Function GetConfigDocument() As XmlDocument Implements IPortfolioManagerLocal.GetConfigDocument
        Return _bonds
    End Function

    Friend Function GenerateNewChainId() As String Implements IPortfolioManagerLocal.GenerateNewChainId
        Return GenerateNewId(_chainIds)
    End Function

    Friend Function GenerateNewListId() As String Implements IPortfolioManagerLocal.GenerateNewListId
        Return GenerateNewId(_listIds)
    End Function

    Public Function GenerateNewCustomBondId() As String
        Return GenerateNewId(_customBondsIds)
    End Function

    Public Sub AddSource(ByVal src As SourceBase) Implements IPortfolioManager.AddSource
        If TypeOf src Is ChainSrc Then
            Dim chain = CType(src, ChainSrc)
            Dim parent = _bonds.SelectSingleNode("/bonds/chains")
            Dim newChainNode = _bonds.CreateNode(XmlNodeType.Element, "chain", "")
            _bonds.AppendAttr(newChainNode, "id", chain.ID)
            _bonds.AppendAttr(newChainNode, "ric", chain.ChainRic)
            _bonds.AppendAttr(newChainNode, "field-set-id", chain.Fields.ID)
            _bonds.AppendAttr(newChainNode, "name", chain.Name)
            _bonds.AppendAttr(newChainNode, "color", chain.Color)
            _bonds.AppendAttr(newChainNode, "curve", chain.Curve)
            _bonds.AppendAttr(newChainNode, "enabled", chain.Enabled)
            parent.AppendChild(newChainNode)
            SaveBonds()
        ElseIf TypeOf src Is UserQuerySrc Then
            Dim query = CType(src, UserQuerySrc)
            Dim parent = _bonds.SelectSingleNode("/bonds/queries")
            Dim newListNode = _bonds.CreateNode(XmlNodeType.Element, "quote", "")
            _bonds.AppendAttr(newListNode, "id", query.ID)
            _bonds.AppendAttr(newListNode, "field-set-id", query.Fields.ID)
            _bonds.AppendAttr(newListNode, "name", query.Name)
            _bonds.AppendAttr(newListNode, "color", query.Color)
            _bonds.AppendAttr(newListNode, "curve", query.Curve)
            _bonds.AppendAttr(newListNode, "enabled", query.Enabled)
            _bonds.AppendAttr(newListNode, "chain-id", query.MySource.ID)
            _bonds.AppendAttr(newListNode, "condition", query.Condition)
            parent.AppendChild(newListNode)
            SaveBonds()
        ElseIf TypeOf src Is UserListSrc Then
            Dim list = CType(src, UserListSrc)
            Dim parent = _bonds.SelectSingleNode("/bonds/lists")
            Dim newListNode = _bonds.CreateNode(XmlNodeType.Element, "list", "")
            _bonds.AppendAttr(newListNode, "id", list.ID)
            _bonds.AppendAttr(newListNode, "field-set-id", list.Fields.ID)
            _bonds.AppendAttr(newListNode, "name", list.Name)
            _bonds.AppendAttr(newListNode, "color", list.Color)
            _bonds.AppendAttr(newListNode, "curve", list.Curve)
            _bonds.AppendAttr(newListNode, "enabled", list.Enabled)
            parent.AppendChild(newListNode)
            SaveBonds()
        ElseIf TypeOf src Is CustomBondSrc Then
            Dim bond = CType(src, CustomBondSrc)
            Dim parent = _bonds.SelectSingleNode("/bonds/custom-bonds")
            Dim newBondNode = _bonds.CreateNode(XmlNodeType.Element, "bond", "")
            _bonds.AppendAttr(newBondNode, "id", bond.ID)
            _bonds.AppendAttr(newBondNode, "name", bond.Name)
            _bonds.AppendAttr(newBondNode, "color", bond.Color)
            _bonds.AppendAttr(newBondNode, "code", bond.Code)
            _bonds.AppendAttr(newBondNode, "maturity", If(bond.Maturity.HasValue, ReutersDate.DateToReuters(bond.Maturity), ""))
            _bonds.AppendAttr(newBondNode, "coupon", bond.CurrentCouponRate)
            _bonds.AppendAttr(newBondNode, "bondStructure", bond.Struct.ToString())
            parent.AppendChild(newBondNode)
            SaveBonds()
        ElseIf TypeOf src Is ChainCurveSrc Then
            Dim bond = CType(src, ChainCurveSrc)
            Dim parent = _bonds.SelectSingleNode("/bonds/chain-curves")
            Dim newBondNode = _bonds.CreateNode(XmlNodeType.Element, "curve", "")
            _bonds.AppendAttr(newBondNode, "id", bond.ID)
            _bonds.AppendAttr(newBondNode, "name", bond.Name)
            _bonds.AppendAttr(newBondNode, "color", bond.Color)
            _bonds.AppendAttr(newBondNode, "ric", bond.Ric)
            _bonds.AppendAttr(newBondNode, "skip", bond.Skip)
            _bonds.AppendAttr(newBondNode, "pattern", bond.Pattern)
            _bonds.AppendAttr(newBondNode, "field-set", bond.Fields.ID)
            parent.AppendChild(newBondNode)
            SaveBonds()
        Else
            Logger.Warn("AddSource(): unsupported source type {0}", src.GetType())
        End If
    End Sub

    Public Sub UpdateSource(ByVal src As SourceBase) Implements IPortfolioManager.UpdateSource
        If TypeOf src Is ChainSrc Then
            Dim chain = CType(src, ChainSrc)
            Dim chainNode = _bonds.SelectSingleNode(String.Format("/bonds/chains/chain[@id='{0}']", src.ID))
            If chainNode Is Nothing Then
                Logger.Error("No chain with id {0} found", src.ID)
                Return
            End If
            _bonds.UpdateAttr(chainNode, "id", chain.ID)
            _bonds.UpdateAttr(chainNode, "ric", chain.ChainRic)
            _bonds.UpdateAttr(chainNode, "field-set-id", chain.Fields.ID)
            _bonds.UpdateAttr(chainNode, "name", chain.Name)
            _bonds.UpdateAttr(chainNode, "color", chain.Color)
            _bonds.UpdateAttr(chainNode, "curve", chain.Curve)
            _bonds.UpdateAttr(chainNode, "enabled", chain.Enabled)
            SaveBonds()
        ElseIf TypeOf src Is UserQuerySrc Then
            Dim query = CType(src, UserQuerySrc)
            Dim queryNode = _bonds.SelectSingleNode(String.Format("/bonds/query/queries[@id='{0}']", src.ID))
            If queryNode Is Nothing Then
                Logger.Error("No list with id {0} found", src.ID)
                Return
            End If
            _bonds.UpdateAttr(queryNode, "id", query.ID)
            _bonds.UpdateAttr(queryNode, "field-set-id", query.Fields.ID)
            _bonds.UpdateAttr(queryNode, "name", query.Name)
            _bonds.UpdateAttr(queryNode, "color", query.Color)
            _bonds.UpdateAttr(queryNode, "curve", query.Curve)
            _bonds.UpdateAttr(queryNode, "enabled", query.Enabled)
            _bonds.UpdateAttr(queryNode, "condition", query.Condition)
            _bonds.UpdateAttr(queryNode, "chain-id", query.MySource.ID)
            SaveBonds()
        ElseIf TypeOf src Is UserListSrc Then
            Dim list = CType(src, UserListSrc)
            Dim listNode = _bonds.SelectSingleNode(String.Format("/bonds/lists/list[@id='{0}']", src.ID))
            If listNode Is Nothing Then
                Logger.Error("No list with id {0} found", src.ID)
                Return
            End If
            _bonds.UpdateAttr(listNode, "id", list.ID)
            _bonds.UpdateAttr(listNode, "field-set-id", list.Fields.ID)
            _bonds.UpdateAttr(listNode, "name", list.Name)
            _bonds.UpdateAttr(listNode, "color", list.Color)
            _bonds.UpdateAttr(listNode, "curve", list.Curve)
            _bonds.UpdateAttr(listNode, "enabled", list.Enabled)
            SaveBonds()
        ElseIf TypeOf src Is CustomBondSrc Then
            Dim bond = CType(src, CustomBondSrc)
            Dim bondNode = _bonds.SelectSingleNode(String.Format("/bonds/custom-bonds/bond[@id='{0}']", src.ID))
            If bondNode Is Nothing Then
                Logger.Error("No custom bond with id {0} found", src.ID)
                Return
            End If
            _bonds.UpdateAttr(bondNode, "id", bond.ID)
            _bonds.UpdateAttr(bondNode, "name", bond.Name)
            _bonds.UpdateAttr(bondNode, "color", bond.Color)
            _bonds.UpdateAttr(bondNode, "code", bond.Code)
            _bonds.UpdateAttr(bondNode, "maturity", If(bond.Maturity.HasValue, ReutersDate.DateToReuters(bond.Maturity), ""))
            _bonds.UpdateAttr(bondNode, "coupon", bond.CurrentCouponRate)
            _bonds.UpdateAttr(bondNode, "bondStructure", bond.Struct.ToString())
            SaveBonds()
        ElseIf TypeOf src Is ChainCurveSrc Then
            Dim bond = CType(src, ChainCurveSrc)
            Dim bondNode = _bonds.SelectSingleNode(String.Format("/bonds/chain-curves/curve[@id='{0}']", src.ID))
            If bondNode Is Nothing Then
                Logger.Error("No chain curve with id {0} found", src.ID)
                Return
            End If
            _bonds.UpdateAttr(bondNode, "id", bond.ID)
            _bonds.UpdateAttr(bondNode, "name", bond.Name)
            _bonds.UpdateAttr(bondNode, "color", bond.Color)
            _bonds.UpdateAttr(bondNode, "ric", bond.Ric)
            _bonds.UpdateAttr(bondNode, "skip", bond.Skip)
            _bonds.UpdateAttr(bondNode, "pattern", bond.Pattern)
            _bonds.UpdateAttr(bondNode, "field-set", bond.Fields.ID)
            SaveBonds()
        Else
            Logger.Warn("UpdateSource(): unsupported source type {0}", src.GetType())
        End If

    End Sub

    Public Function GetPortfoliosBySource(ByVal src As SourceBase) As List(Of IdName(Of String)) Implements IPortfolioManager.GetPortfoliosBySource
        If TypeOf src Is ChainSrc Then
            Dim chain = CType(src, ChainSrc)
            Dim nodes = _bonds.SelectNodes(String.Format("/bonds/portfolios//portfolio[include[@what='chain' and @id='{0}']]", chain.ID))
            Return (From node As XmlNode In nodes
                    Select New IdName(Of String)(node.Attributes("id").Value, node.Attributes("name").Value)).ToList()
        ElseIf TypeOf src Is UserListSrc Then
            Dim list = CType(src, UserListSrc)
            Dim nodes = _bonds.SelectNodes(String.Format("/bonds/portfolios//portfolio[include[@what='list' and @id='{0}']]", list.ID))
            Return (From node As XmlNode In nodes
                    Select New IdName(Of String)(node.Attributes("id").Value, node.Attributes("name").Value)).ToList()
        Else
            Return New List(Of IdName(Of String))
        End If
    End Function

    Public Sub DeleteSource(ByVal src As SourceBase) Implements IPortfolioManager.DeleteSource
        If TypeOf src Is ChainSrc Then
            Dim chain = CType(src, ChainSrc)
            Dim nodes = _bonds.SelectNodes(String.Format("/bonds/portfolios//portfolio[include[@what='chain' and @id='{0}']]", chain.ID))
            For Each node As XmlNode In nodes
                node.ParentNode.RemoveChild(node)
            Next
            Dim elem = _bonds.SelectSingleNode(String.Format("/bonds/chains/chain[@id='{0}']", src.ID))
            elem.ParentNode.RemoveChild(elem)
        ElseIf TypeOf src Is UserQuerySrc Then
            Dim list = CType(src, UserQuerySrc)
            Dim nodes = _bonds.SelectNodes(String.Format("/bonds/portfolios//portfolio[include[@what='query' and @id='{0}']]", list.ID))
            For Each node As XmlNode In nodes
                node.ParentNode.RemoveChild(node)
            Next
            Dim elem = _bonds.SelectSingleNode(String.Format("/bonds/queries/query[@id='{0}']", src.ID))
            elem.ParentNode.RemoveChild(elem)
        ElseIf TypeOf src Is UserListSrc Then
            Dim list = CType(src, UserListSrc)
            Dim nodes = _bonds.SelectNodes(String.Format("/bonds/portfolios//portfolio[include[@what='list' and @id='{0}']]", list.ID))
            For Each node As XmlNode In nodes
                node.ParentNode.RemoveChild(node)
            Next
            Dim elem = _bonds.SelectSingleNode(String.Format("/bonds/lists/list[@id='{0}']", src.ID))
            elem.ParentNode.RemoveChild(elem)
        ElseIf TypeOf src Is CustomBondSrc Then
            Dim list = CType(src, CustomBondSrc)
            Dim nodes = _bonds.SelectNodes(String.Format("/bonds/portfolios//portfolio[include[@what='custom-bond' and @id='{0}']]", list.ID))
            For Each node As XmlNode In nodes
                node.ParentNode.RemoveChild(node)
            Next
            Dim elem = _bonds.SelectSingleNode(String.Format("/bonds/custom-bonds/bond[@id='{0}']", src.ID))
            elem.ParentNode.RemoveChild(elem)
        ElseIf TypeOf src Is ChainCurveSrc Then
            Dim elem = _bonds.SelectSingleNode(String.Format("/bonds/chain-curves/curve[@id='{0}']", src.ID))
            elem.ParentNode.RemoveChild(elem)
        Else
            Logger.Warn("DeleteSource(): unsupported source type {0}", src.GetType())
            Return
        End If
        SaveBonds()
    End Sub

    Public Sub SelectConfigFile(ByVal fileName As String) Implements IPortfolioManager.SelectConfigFile
        ConfigFile = fileName
    End Sub

    Public Function IPortfolioManager_ConfigFile() As String Implements IPortfolioManager.ConfigFile
        Return _configFile
    End Function

    Public Sub SelectDefaultConfigFile() Implements IPortfolioManager.SelectDefaultConfigFile
        ConfigFile = DefaultConfigFile
    End Sub

    Public ReadOnly Property CustomBondsView() As List(Of CustomBondSrc) Implements IPortfolioManager.CustomBondsView
        Get
            Dim nodes = _bonds.SelectNodes("/bonds/custom-bonds/bond")
            Return New List(Of CustomBondSrc)(From node As XmlNode In nodes
                                           Select New CustomBondSrc(node.GetAttrStrict("id"), node.GetAttr("color"),
                                                                 node.GetAttrStrict("name"), node.GetAttr("code"),
                                                                 node.GetAttr("bondStructure"), node.GetAttr("maturity").Trim(),
                                                                 Double.Parse(node.GetAttrStrict("coupon"), CultureInfo.InvariantCulture)))
        End Get
    End Property


    Public Function GetFolderDescr(ByVal id As String) As Portfolio Implements IPortfolioManager.GetFolderDescr
        Dim node = _bonds.SelectSingleNode(String.Format("/bonds/portfolios//folder[@id='{0}']", id))
        If node Is Nothing Then Return Nothing
        Dim attributes = node.Attributes
        Return New Portfolio(True, attributes("name").Value, attributes("id").Value)
    End Function

    Public Function GetAllPortfolios() As List(Of IdName(Of String)) Implements IPortfolioManager.GetAllPortfolios
        Dim res As New List(Of IdName(Of String))
        Dim iter = _bonds.SelectNodes("/bonds/portfolios//portfolio")
        For i = 0 To iter.Count - 1
            res.Add(New IdName(Of String)(iter(i).SelectSingleNode("@id").Value, iter(i).SelectSingleNode("@name").Value))
        Next
        Return res
    End Function

    Public Shared ReadOnly Property Instance() As IPortfolioManager
        Get
            If _instance Is Nothing Then _instance = New PortfolioManager()
            Return _instance
        End Get
    End Property

    Friend Shared ReadOnly Property ClassInstance() As PortfolioManager
        Get
            If _instance Is Nothing Then _instance = New PortfolioManager()
            Return _instance
        End Get
    End Property
End Class