Imports System.Diagnostics.Contracts
Imports Logging
Imports NLog
Imports ReutersData

Namespace Bonds
    Public Interface IBondsData
        '' Returns BondDescription object which contains data on selected bond
        Function GetBondInfo(ByVal ric As String) As BondDescription
        Function GetBondPayments(ByVal ric As String) As BondPayments

        Function IsStraight(ByVal ric As String) As Boolean
        Function IsConvertible(ByVal ric As String) As Boolean
        Function IsFrn(ByVal ric As String) As Boolean
        Function IsPutable(ByVal ric As String) As Boolean
        Function IsCallable(ByVal ric As String) As Boolean
    End Interface

    Public Class BondsData
        Implements IBondsData

        Private Shared ReadOnly [Me] As New BondsData

        Private Sub New()
        End Sub

        Public Shared ReadOnly Property Instance As IBondsData
            Get
                Return [Me]
            End Get
        End Property

        Public Function GetBondInfo(ByVal ric As String) As BondDescription Implements IBondsData.GetBondInfo
            Throw New NotImplementedException()
        End Function

        Public Function GetBondPayments(ByVal ric As String) As BondPayments Implements IBondsData.GetBondPayments
            Throw New NotImplementedException()
        End Function

        Public Function IsStraight(ByVal ric As String) As Boolean Implements IBondsData.IsStraight
            Throw New NotImplementedException()
        End Function

        Public Function IsConvertible(ByVal ric As String) As Boolean Implements IBondsData.IsConvertible
            Throw New NotImplementedException()
        End Function

        Public Function IsFrn(ByVal ric As String) As Boolean Implements IBondsData.IsFrn
            Throw New NotImplementedException()
        End Function

        Public Function IsPutable(ByVal ric As String) As Boolean Implements IBondsData.IsPutable
            Throw New NotImplementedException()
        End Function

        Public Function IsCallable(ByVal ric As String) As Boolean Implements IBondsData.IsCallable
            Throw New NotImplementedException()
        End Function
    End Class

    Public Interface IBondsLoader
        Event Progress As Action(Of String)
        Event Success As Action
        Event NoBonds As action
        Event Failure As Action(Of Exception)

        ''' Loads all data from configuration file and stores them into IMDB
        Sub Initialize()

        ''' Loads all data from given chain and stores them into IMDB
        Sub LoadChain(ByVal chainRic As String)

        ''' Loads all data from given chains and stores them into IMDB
        Sub LoadChains(ByVal chainRics As List(Of String))

        ''' Loads all data for given RIC and stores them into IMDB with given pseudo chain
        Sub LoadRic(ByVal ric As String, ByVal pseudoChain As String)

        Function GetBondsTable() As BondsDataSet.BondDataTable
        Function GetCouponsTable() As BondsDataSet.CouponDataTable
        Function GetFRNsTable() As BondsDataSet.FrnDataTable
        Function GetIssueRatingsTable() As BondsDataSet.IssueRatingDataTable
        Function GetIssuerRatingsTable() As BondsDataSet.IssuerRatingDataTable
        Function GetAllRicsTable() As BondsDataSet.RicsDataTable
    End Interface

    Public Class BondsLoader
        Implements IBondsLoader
        Private WithEvents _chainLoader As New Chain

        Private Shared ReadOnly Logger As Logger = GetLogger(GetType(BondsLoader))

        Private Shared ReadOnly RicChain As New BondsDataSet.RicChainDataTable
        Private Shared ReadOnly CouponTable As New BondsDataSet.CouponDataTable
        Private Shared ReadOnly FrnTable As New BondsDataSet.FrnDataTable
        Private Shared ReadOnly BondsTable As New BondsDataSet.BondDataTable
        Private Shared ReadOnly IssueRatingsTable As New BondsDataSet.IssueRatingDataTable
        Private Shared ReadOnly IssuerRatingsTable As New BondsDataSet.IssuerRatingDataTable
        Private Shared ReadOnly RicsTable As New BondsDataSet.RicsDataTable

        Private Shared _instance As BondsLoader

        Public Event Progress As Action(Of String) Implements IBondsLoader.Progress
        Public Event Success As Action Implements IBondsLoader.Success
        Public Event NoBonds As action Implements IBondsLoader.NoBonds
        Public Event Failure As Action(Of Exception) Implements IBondsLoader.Failure

        Private Shared ReadOnly QueryBondDescr As New Dex2Query({
            New Dex2Field(0, BondsTable.ricColumn.ColumnName),
            New Dex2Field("EJV.X.ADF_BondStructure", BondsTable.bondStructureColumn.ColumnName),
            New Dex2Field("EJV.X.ADF_RateStructure", BondsTable.rateStructureColumn.ColumnName, ItsNum:=False),
            New Dex2Field("EJV.C.Description", BondsTable.descriptionColumn.ColumnName),
            New Dex2Field("EJV.C.OriginalAmountIssued", BondsTable.issueSizeColumn.ColumnName, ItsNum:=True),
            New Dex2Field("EJV.C.IssuerName", BondsTable.issuerNameColumn.ColumnName),
            New Dex2Field("EJV.C.BorrowerName", BondsTable.borrowerNameColumn.ColumnName),
            New Dex2Field("EJV.X.ADF_Coupon", BondsTable.currentCouponColumn.ColumnName, ItsNum:=True),
            New Dex2Field("EJV.C.IssueDate", BondsTable.issueDateColumn.ColumnName, ItsDate:=True),
            New Dex2Field("EJV.C.MaturityDate", BondsTable.maturityDateColumn.ColumnName, ItsDate:=True),
            New Dex2Field("EJV.C.Currency", BondsTable.currencyColumn.ColumnName),
            New Dex2Field("EJV.C.ShortName", BondsTable.shortNameColumn.ColumnName),
            New Dex2Field("EJV.C.IsCallable", BondsTable.isCallableColumn.ColumnName, ItsBool:=True),
            New Dex2Field("EJV.C.IsPutable", BondsTable.isPutableColumn.ColumnName, ItsBool:=True),
            New Dex2Field("EJV.C.IsFloater", BondsTable.isFloaterColumn.ColumnName, ItsBool:=True),
            New Dex2Field("EJV.C.IsConvertible", BondsTable.isConvertibleColumn.ColumnName, ItsBool:=True),
            New Dex2Field("EJV.C.IsStraight", BondsTable.isStraightColumn.ColumnName, ItsBool:=True),
            New Dex2Field("EJV.C.Ticker", BondsTable.tickerColumn.ColumnName),
            New Dex2Field("EJV.C.Series", BondsTable.seriesColumn.ColumnName),
            New Dex2Field("EJV.C.BorrowerCntyCode", BondsTable.borrowerCountryColumn.ColumnName),
            New Dex2Field("EJV.C.IssuerCountry", BondsTable.issuerCountryColumn.ColumnName),
            New Dex2Field("RI.ID.ISIN", BondsTable.isinColumn.ColumnName),
            New Dex2Field("EJV.C.ParentTicker", BondsTable.parentTickerColumn.ColumnName)
        }.ToList(), "RH:In")

        Private Shared ReadOnly QueryIssueRating As New Dex2Query({
            New Dex2Field(0, IssueRatingsTable.ricColumn.ColumnName),
            New Dex2Field("EJV.GR.Rating", IssueRatingsTable.ratingColumn.ColumnName),
            New Dex2Field("EJV.GR.RatingDate", IssueRatingsTable.dateColumn.ColumnName),
            New Dex2Field("EJV.GR.RatingSourceCode", IssueRatingsTable.ratingSrcColumn.ColumnName)
        }.ToList(), "RH:In", "RTSRC:MDY;S&P;FTC")

        Private Shared ReadOnly QueryIssuerRating As New Dex2Query({
            New Dex2Field(0, IssuerRatingsTable.ricColumn.ColumnName),
            New Dex2Field("EJV.IR.Rating", IssuerRatingsTable.ratingColumn.ColumnName),
            New Dex2Field("EJV.IR.RatingDate", IssuerRatingsTable.dateColumn.ColumnName),
            New Dex2Field("EJV.IR.RatingSourceCode", IssuerRatingsTable.ratingSrcColumn.ColumnName)
        }.ToList(), "RH:In", "RTS:FDL;SPI;MDL RTSC:FRN")

        Private Shared ReadOnly QueryCoupon As New Dex2Query({
            New Dex2Field(0, CouponTable.ricColumn.ColumnName),
            New Dex2Field(1, CouponTable.dateColumn.ColumnName, itsDate:=True),
            New Dex2Field("EJV.C.CouponRate", CouponTable.rateColumn.ColumnName, itsNum:=True)
        }.ToList(), "RH:In,D", "D:1984;2013")

        Private Shared ReadOnly QueryFrn As New Dex2Query({
            New Dex2Field(0, FrnTable.ricColumn.ColumnName),
            New Dex2Field("EJV.X.FRNFLOOR", FrnTable.floorColumn.ColumnName),
            New Dex2Field("EJV.X.FRNCAP", FrnTable.capColumn.ColumnName),
            New Dex2Field("PAY_FREQ", FrnTable.frequencyColumn.ColumnName),
            New Dex2Field("EJV.C.IndexRic", FrnTable.indexRicColumn.ColumnName),
            New Dex2Field("EJV.X.ADF_MARGIN", FrnTable.marginColumn.ColumnName, itsNum:=True)
        }.ToList(), "RH:In")

        Private Shared ReadOnly QueryRics As New Dex2Query({
            New Dex2Field(0, RicsTable.ricColumn.ColumnName),
            New Dex2Field(1, RicsTable.contributorColumn.ColumnName),
            New Dex2Field("EJV.C.RICS", RicsTable.subRicColumn.ColumnName)
        }.ToList(), "RH:In;Con")
        ''' <summary>
        ''' Entry point. Loads all data from configuration file and stores them into IMDB
        ''' </summary>
        Public Sub UpdateAllChains() Implements IBondsLoader.Initialize
            Dim chainRics = PortfolioManager.GetInstance().GetChainRics()
            If chainRics.Count = 0 Then
                RaiseEvent NoBonds()
                Exit Sub
            End If
            _chainLoader.StartChains(chainRics, "UWC:YES LAY:VER")
        End Sub

        ''' <summary>
        ''' Entry point. Loads all data from given chain and stores them into IMDB
        ''' </summary>
        Public Sub LoadChain(ByVal chainRic As String) Implements IBondsLoader.LoadChain
            Contract.Requires(chainRic <> "")
            LoadChains({chainRic}.ToList())
        End Sub

        ''' <summary>
        ''' Entry point. Loads all data from given chains and stores them into IMDB
        ''' </summary>
        Public Sub LoadChains(ByVal chainRics As List(Of String)) Implements IBondsLoader.LoadChains
            Contract.Requires(chainRics IsNot Nothing AndAlso chainRics.Count > 0)
            _chainLoader.StartChains(chainRics, "UWC:YES LAY:VER")
        End Sub

        ''' <summary>
        ''' Entry point. Loads all data for given RIC and stores them into IMDB
        ''' </summary>
        Public Sub LoadRic(ByVal ric As String, ByVal pseudoChain As String) Implements IBondsLoader.LoadRic
            Contract.Requires(ric <> "" And pseudoChain <> "")
            Dim rics = {ric}.ToList()
            StoreRicsAndChains(pseudoChain, rics)
            StartLoad(rics)
        End Sub

        Public Function GetBondsTable() As BondsDataSet.BondDataTable Implements IBondsLoader.GetBondsTable
            Return BondsTable
        End Function

        Public Function GetCouponsTable() As BondsDataSet.CouponDataTable Implements IBondsLoader.GetCouponsTable
            Return CouponTable
        End Function

        Public Function GetFRNsTable() As BondsDataSet.FrnDataTable Implements IBondsLoader.GetFRNsTable
            Return FrnTable
        End Function

        Public Function GetIssueRatingsTable() As BondsDataSet.IssueRatingDataTable Implements IBondsLoader.GetIssueRatingsTable
            Return IssueRatingsTable
        End Function

        Public Function GetAllRicsTable() As BondsDataSet.RicsDataTable Implements IBondsLoader.GetAllRicsTable
            Return RicsTable
        End Function

        Public Function GetIsuuerRatingsTable() As BondsDataSet.IssuerRatingDataTable Implements IBondsLoader.GetIssuerRatingsTable
            Return IssuerRatingsTable
        End Function

        Private Sub OnChainArrived(ByVal ric As String) Handles _chainLoader.Arrived
            RaiseEvent Progress(ric)
        End Sub

        Private Sub OnChainData(ByVal chainsAndRics As Dictionary(Of String, List(Of String))) Handles _chainLoader.Chain
            If chainsAndRics.Count = 0 Then
                RaiseEvent NoBonds()
                Exit Sub
            End If

            Dim chainRics = chainsAndRics.Keys.ToList()
            chainRics.ForEach(Sub(chainRic) StoreRicsAndChains(chainRic, chainsAndRics(chainRic)))

            Dim rics = GetRics(chainRics)
            If rics.Count = 0 Then
                RaiseEvent NoBonds()
                Exit Sub
            End If

            StartLoad(rics)
        End Sub

        Private Shared Function GetRics(ByVal chain As String) As List(Of String)
            Return (From row As BondsDataSet.RicChainRow In RicChain.Rows
                Where row.chain = chain
                Select row.ric Distinct).ToList()
        End Function

        Private Shared Function GetRics(ByVal chains As List(Of String)) As List(Of String)
            Dim res As New List(Of String)
            chains.ForEach(Sub(chain) res.AddRange(GetRics(chain)))
            Return res.Distinct().ToList()
        End Function

        Private Shared Sub StoreRicsAndChains(ByVal chain As String, ByRef rics As List(Of String))
            Try
                rics.ForEach(
                    Sub(ric)
                        If Not RicChain.Any(Function(row As BondsDataSet.RicChainRow) row.ric = ric And row.chain = chain) Then
                            RicChain.AddRicChainRow(ric, chain)
                        End If
                    End Sub)
            Catch ex As Exception
                Logger.ErrorException("Failed to store chain " & chain, ex)
                Logger.Error("Exception = {0}", ex)
            End Try
        End Sub

        Private Shared Sub ImportData(ByVal data As LinkedList(Of Dictionary(Of String, Object)), ByVal table As DataTable, ByVal query As Dex2Query)
            If data Is Nothing Then
                Logger.Info("Nothing to importing  into table {0}", table.TableName)
            Else
                Logger.Info("Importing {0} data rows into table {1}", data.Count, table.TableName)
                Try
                    For Each slot In data
                        Dim rw = table.NewRow()
                        For Each colName In slot.Keys
                            Dim elem = slot(colName)
                            If query.IsBool(colName) Then
                                rw(colName) = (elem = "Y")
                            ElseIf query.IsDate(colName) Then
                                If IsDate(elem) Then rw(colName) = Date.Parse(elem)
                            ElseIf query.IsNum(colName) Then
                                If IsNumeric(elem) Then rw(colName) = Double.Parse(elem)
                            Else
                                rw(colName) = elem
                            End If
                        Next
                        table.Rows.Add(rw)
                    Next
                Catch ex As Exception
                    Logger.ErrorException("Failed to import data to table" + table.TableName, ex)
                    Logger.Error("Exception = {0}", ex.ToString())
                End Try
            End If

        End Sub

        Private Sub LoadGeneral(ByVal table As DataTable,
                               ByVal query As Dex2Query,
                               ByRef msg As String,
                               ByVal handler As Action(Of LinkedList(Of Dictionary(Of String, Object))),
                               Optional ByVal allowedRics As HashSet(Of String) = Nothing)

            Contract.Requires(table IsNot Nothing And query IsNot Nothing)

            Logger.Info("Starting loading table {0}", table)
            If allowedRics Is Nothing Then
                ' если ничего не передали, то берем все строки
                allowedRics = New HashSet(Of String)((From row In BondsTable Select row.ric))
            End If

            If allowedRics.Count > 0 Then
                RaiseEvent Progress(msg)
                Dim rowsToDelete = From row In table
                                   Where allowedRics.Contains(row("ric").ToString())
                                   Select row

                For Each row In rowsToDelete
                    table.Rows.Remove(row)
                Next

                Dim dex2 As New Dex2
                AddHandler dex2.Failure, Sub() RaiseEvent Failure(New Exception("Dex2 error"))
                AddHandler dex2.Metadata, handler
                dex2.Load(allowedRics.ToList(), query)
            Else
                handler(Nothing)
            End If
        End Sub

        Private Sub StartLoad(ByVal requiredRics As List(Of String))
            Logger.Info("LoadMetadata")
            LoadGeneral(BondsTable, QueryBondDescr, "Loading bonds descriptions",
                         Sub(data As LinkedList(Of Dictionary(Of String, Object)))
                             If data IsNot Nothing Then
                                 ImportData(data, BondsTable, QueryBondDescr)
                                 LoadStep1(New HashSet(Of String)(requiredRics))
                             Else
                                 RaiseEvent NoBonds()
                             End If
                         End Sub,
                         New HashSet(Of String)(requiredRics))
        End Sub

        Private Sub LoadStep1(ByVal rics As HashSet(Of String))
            Logger.Info("LoadIssuerRatings")
            LoadGeneral(IssuerRatingsTable, QueryIssuerRating, "Loading bonds issuers ratings",
                         Sub(data As LinkedList(Of Dictionary(Of String, Object)))
                             ImportData(data, IssuerRatingsTable, QueryIssuerRating)
                             LoadStep2(rics)
                         End Sub)
        End Sub

        Private Sub LoadStep2(ByVal rics As HashSet(Of String))
            Logger.Info("LoadIssueRatings")
            LoadGeneral(IssueRatingsTable, QueryIssueRating, "Loading bonds ratings",
                         Sub(data As LinkedList(Of Dictionary(Of String, Object)))
                             ImportData(data, IssueRatingsTable, QueryIssueRating)
                             LoadStep3(rics)
                         End Sub)
        End Sub

        Private Sub LoadStep3(ByVal rics As HashSet(Of String))
            Logger.Info("LoadCoupons")
            LoadGeneral(CouponTable, QueryCoupon, "Loading bonds coupons",
                        Sub(data As LinkedList(Of Dictionary(Of String, Object)))
                            ImportData(data, CouponTable, QueryCoupon)
                            LoadStep4(rics)
                        End Sub)
        End Sub

        Private Sub LoadStep4(ByVal rics As HashSet(Of String))
            Logger.Info("LoadFrns")
            Dim floaterRics = New HashSet(Of String)(From row In BondsTable Where rics.Contains(row.ric) And row.isFloater Select row.ric)
            LoadGeneral(FrnTable, QueryFrn, "Loading FRN structures",
                         Sub(data As LinkedList(Of Dictionary(Of String, Object)))
                             ImportData(data, FrnTable, QueryFrn)
                             LoadStep5(rics)
                         End Sub,
                         floaterRics)
        End Sub

        Private Sub LoadStep5(ByVal rics As HashSet(Of String))
            Logger.Info("LoadFrns")
            LoadGeneral(RicsTable, QueryRics, "Loading all rics",
                         Sub(data As LinkedList(Of Dictionary(Of String, Object)))
                             ImportData(data, RicsTable, QueryRics)
                             RaiseEvent Success()
                         End Sub)
            'Logger.Info("LoadCalls")
            'Dim rs = New HashSet(Of String)(From row In BondsTable Where rics.Contains(row.ric) And row.isFloater Select row.ric)
            'LoadGeneral(FrnTable, QueryFrn, "Loading FRN structures",
            '             Sub(data As LinkedList(Of Dictionary(Of String, Object)))
            '                 ImportData(data, FrnTable, QueryFrn)
            '                 RaiseEvent Success()
            '             End Sub,
            '             rs)
        End Sub

        Private Sub New()
        End Sub

        Public Shared ReadOnly Property Instance As IBondsLoader
            Get
                If _instance Is Nothing Then _instance = New BondsLoader
                Return _instance
            End Get
        End Property
    End Class
End Namespace