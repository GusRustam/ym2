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
    Implements IPortfolioManager
    Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(PortfolioManager))

    Private ReadOnly _bonds As New XmlDocument
    Private Shared _instance As PortfolioManager

    Private Sub New()
        _bonds.Load(Path.Combine(Utils.GetMyPath(), "bonds.xml"))
    End Sub

    Public Function GetPortfoliosByFolder(ByVal id As String) As List(Of PortfolioItemDescription) Implements IPortfolioManager.GetPortfoliosByFolder
        Dim res As New List(Of PortfolioItemDescription)
        Dim iter As XmlNodeList
        iter = _bonds.SelectNodes(String.Format("/bonds/portfolios//folder[@id='{0}']/folder", id))
        For i = 0 To iter.Count - 1
            Dim attributes = iter(i).Attributes
            If attributes.ItemOf("id") IsNot Nothing AndAlso attributes.ItemOf("name") IsNot Nothing Then
                res.Add(New PortfolioItemDescription(True, attributes("name").Value, attributes("id").Value))
            End If
        Next
        iter = _bonds.SelectNodes(String.Format("/bonds/portfolios//folder[@id='{0}']/portfolio", id))
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
        _bonds.Save(Path.Combine(Utils.GetMyPath(), "bonds.xml"))
    End Sub

    Public Sub SetPortfolioName(ByVal id As String, ByVal name As String) Implements IPortfolioManager.SetPortfolioName
        Dim node = _bonds.SelectSingleNode(String.Format("/bonds/portfolios//portfolio[@id='{0}']/@name", id))
        node.Value = name
        _bonds.Save(Path.Combine(Utils.GetMyPath(), "bonds.xml"))
    End Sub

    Public Function GetMainFolderName(ByVal id As String) As PortfolioItemDescription Implements IPortfolioManager.GetFolderDescr
        Dim node = _bonds.SelectSingleNode(String.Format("/bonds/portfolios//folder[@id='{0}']", id))
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
