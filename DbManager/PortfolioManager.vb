Imports System.Xml
Imports System.IO
Imports Uitls

Public Interface IPortfolioManager
    '' Returns flat structure of portfolios: only id-name pairs, disregarding any folder structure
    Function GetPortfoliosFlat() As List(Of Tuple(Of Integer, String))

    '' Returns RICs of all chains given in XML
    Function GetChainRics() As List(Of String)

    '' Returns portfolio as a set of sources, each having different settings
    Function GetPortfolioStructure(ByVal currentPortID As Long) As List(Of PortfolioSource)
End Interface

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

Public Class PortfolioSource
    Public Id As Integer
    Public Name As Integer
    Public Fields As Fields
    Public Color As String
    Public Rics As List(Of String)
End Class

Public Class PortfolioManager
    Implements IPortfolioManager

    Private ReadOnly _bonds As New XmlDocument
    Private Shared _instance As PortfolioManager

    Private Sub New()
        _bonds.Load(Path.Combine(Utils.GetMyPath(), "bonds.xml"))
    End Sub

    Public Function GetChainRics() As List(Of String) Implements IPortfolioManager.GetChainRics
        Dim res As New List(Of String)
        Dim iter = _bonds.SelectNodes("/bonds/chains/chain/@ric")
        For i = 0 To iter.Count - 1
            res.Add(iter(i).Value)
        Next
        Return res
    End Function

    Public Function GetPortfolioStructure(ByVal currentPortID As Long) As List(Of PortfolioSource) Implements IPortfolioManager.GetPortfolioStructure
        Throw New NotImplementedException()
    End Function

    Public Function Portfolios() As List(Of Tuple(Of Integer, String)) Implements IPortfolioManager.GetPortfoliosFlat
        Dim res As New List(Of Tuple(Of Integer, String))
        Dim iter = _bonds.SelectNodes("/portfolios//portfolio")
        For i = 0 To iter.Count
            res.Add(Tuple.Create(CInt(iter(i).SelectSingleNode("@id").Value), iter(i).SelectSingleNode("@name").Value))
        Next
        Return res
    End Function

    Public Shared Function GetInstance() As IPortfolioManager
        If _instance Is Nothing Then _instance = New PortfolioManager()
        Return _instance
    End Function
End Class
