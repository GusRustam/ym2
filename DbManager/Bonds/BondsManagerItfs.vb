Imports System.ComponentModel
Imports Uitls

Namespace Bonds
    Public Class NoBondException
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

    Public Interface IBondsData
        Sub Refresh()
        Function BondExists(ByVal ric As String) As Boolean
        Function GetBondInfo(ByVal ric As String) As BondMetadata
        Function GetBondPayments(ByVal ric As String) As BondPayments
        Function GetBondInfo(ByVal rics As List(Of String)) As List(Of BondMetadata)
    End Interface

    Public Interface IBondsLoader
        Event Progress As Action(Of ProgressEvent)

        ''' Loads all data from configuration file and stores them into IMDB
        Sub Initialize()

        ''' Loads all data from given chain and stores them into IMDB
        Sub LoadChain(ByVal chainRic As String)

        ''' Loads all data from given chains and stores them into IMDB
        Sub LoadChains(ByVal chainRics As List(Of String))

        ''' Loads all data for given RIC and stores them into IMDB with given pseudo chain
        Sub LoadRic(ByVal ric As String, ByVal pseudoChain As String)

        Function GetRicChainTable() As BondsDataSet.RicChainDataTable
        Function GetBondsTable() As BondsDataSet.BondDataTable
        Function GetCouponsTable() As BondsDataSet.CouponDataTable
        Function GetFRNsTable() As BondsDataSet.FrnDataTable
        Function GetIssueRatingsTable() As BondsDataSet.IssueRatingDataTable
        Function GetIssuerRatingsTable() As BondsDataSet.IssuerRatingDataTable
        Function GetAllRicsTable() As BondsDataSet.RicsDataTable
        Function GetChainRics(ByVal chainRic As String) As List(Of String)
        Function GetFrnTable() As BondsDataSet.FrnDataTable
        Function GetRicsTable() As BondsDataSet.RicsDataTable
        Sub ClearTables()
    End Interface
End Namespace
