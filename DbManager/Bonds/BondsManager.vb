Imports System.Diagnostics.Contracts
Imports Logging
Imports NLog
Imports ReutersData
Imports Settings
Imports Uitls

Namespace Bonds
    Public Class BondsData
        Implements IBondsData

        Private Shared ReadOnly Logger As Logger = GetLogger(GetType(BondsData))
        Private WithEvents _ldr As IBondsLoader = BondsLoader.Instance
        Private Shared ReadOnly [Me] As New BondsData

        Private Shared _ricToSubRic As Dictionary(Of String, List(Of String))    ' соответствия "верхний рик" -> "рики нижнего уровня" (1 -> N)
        Private Shared _subRicToRic As Dictionary(Of String, String)             ' соответствия "рик нижнего уровня" -> "верхний рик"  (1 -> 1)
        Private Shared _initialized As Boolean

        Private Sub OnBondsLoaded(ByVal evt As ProgressEvent) Handles _ldr.Progress
            If evt.Log.Success() Then
                Refresh()
            Else
                Clear()
            End If
        End Sub

        Public Sub Refresh() Implements IBondsData.Refresh
            Logger.Info("Refresh()")
            Clear()

            ' соответствия сам к себе
            For Each rowRic In From row In _ldr.GetBondsTable() Select row.ric 'SkipSlash(row.ric)
                If _ricToSubRic.ContainsKey(rowRic) Then
                    _ricToSubRic(rowRic).Add(rowRic)
                Else
                    _ricToSubRic.Add(rowRic, New List(Of String)({rowRic}))
                End If
                _subRicToRic.Add(rowRic, rowRic)
            Next

            ' соответствия сам к деткам. Только вот среди деток может быть и он сам.
            For Each row In From rw In _ldr.GetAllRicsTable() Where rw.subRic <> rw.ric
                Dim rowRic = row.ric 'SkipSlash(row.ric)
                Dim subRic = row.subRic 'SkipSlash(row.subRic)
                If _ricToSubRic.ContainsKey(rowRic) Then
                    _ricToSubRic(rowRic).Add(subRic)
                Else
                    _ricToSubRic.Add(rowRic, New List(Of String)({subRic}))
                End If
                _subRicToRic.Add(subRic, rowRic)
            Next
            _initialized = True
        End Sub

        Private Shared Sub Clear()
            _ricToSubRic.Clear()
            _subRicToRic.Clear()
        End Sub

        Public Shared ReadOnly Property Instance As IBondsData
            Get
                Return [Me]
            End Get
        End Property

        Private Sub New()
            _ricToSubRic = New Dictionary(Of String, List(Of String))
            _subRicToRic = New Dictionary(Of String, String)

            If Not _initialized Then
                Refresh()
                _initialized = True
            End If
        End Sub

        Public Function BondExists(ByVal ric As String) As Boolean Implements IBondsData.BondExists
            'ric = SkipSlash(ric)  todo RICSSS
            Return _subRicToRic.ContainsKey(ric)
        End Function

        Public Function GetBondInfo(ByVal aRic As String) As BondMetadata Implements IBondsData.GetBondInfo
            'aRic = SkipSlash(aRic) todo RICSSS
            Dim items = (From row In _ldr.GetBondsTable()
                         Where _subRicToRic.Keys.Contains(aRic) AndAlso row.ric = _subRicToRic(aRic)
                         Select row).ToList()

            If Not items.Any Then Throw New NoBondException(aRic) ' todo will anybody catch it?
            Dim descr As BondsDataSet.BondRow = items.First()

            Dim coupon = If(Not IsDBNull(descr("currentCoupon")) AndAlso IsNumeric(descr.currentCoupon), CDbl(descr.currentCoupon), 0)
            Dim maturityDate As Date = If(Not IsDBNull(descr("maturityDate")) AndAlso IsDate(descr.maturityDate), CDate(descr.maturityDate), Nothing)
            Dim issueDate As Date = If(Not IsDBNull(descr("issueDate")) AndAlso IsDate(descr.issueDate), CDate(descr.issueDate), Date.MinValue)
            Dim series = If(Not IsDBNull(descr("series")), descr.series, "")
            Dim rateStructure = If(Not IsDBNull(descr("rateStructure")), descr.rateStructure, "")
            Dim ric = If(Not IsDBNull(descr("ric")), descr.ric, "")
            Dim paymentStructure = If(Not IsDBNull(descr("bondStructure")), descr.bondStructure, "")
            Dim shortName = If(Not IsDBNull(descr("shortName")), descr.shortName, "")
            Dim description = If(Not IsDBNull(descr("description")), descr.description, "")
            Dim issuerName = If(Not IsDBNull(descr("issuerName")), descr.issuerName, "")
            Dim borrowerName = If(Not IsDBNull(descr("borrowerName")), descr.borrowerName, "")
            Dim currency = If(Not IsDBNull(descr("currency")), descr.currency, "")
            Dim isPutable = Not IsDBNull(descr("isPutable")) AndAlso descr.isPutable
            Dim isCallable = Not IsDBNull(descr("isCallable")) AndAlso descr.isCallable
            Dim isFloater = Not IsDBNull(descr("isFloater")) AndAlso descr.isFloater
            Dim sN = shortName & " " & series
            Dim seniorityType = If(Not IsDBNull(descr("seniorityType")), descr.seniorityType, "")
            Dim industry = If(Not IsDBNull(descr("industry")), descr.industry, "")
            Dim subIndustry = If(Not IsDBNull(descr("subIndustry")), descr.subIndustry, "")

            Dim issueRatings = (From row In _ldr.GetIssueRatingsTable()
                                Where row.ric = _subRicToRic(aRic) And Not IsDBNull(row("date")) AndAlso IsDate(row("date"))
                                Let dt = row._date, rate = row.rating, rateSrc = row.ratingSrc
                                Select dt, rate, rateSrc).ToList
            Dim lastIssueRating As RatingDescr
            If issueRatings.Any Then
                Dim maxDate = (From rw In issueRatings Select rw.dt).ToList().Max()
                Dim rt = issueRatings.First(Function(elem) elem.dt = maxDate)
                lastIssueRating = New RatingDescr(Rating.Parse(rt.rate), rt.dt, RatingSource.Parse(rt.rateSrc))
            Else
                lastIssueRating = New RatingDescr(Rating.Other, Nothing, Nothing)
            End If

            Dim issuerRatings = (From row In _ldr.GetIssuerRatingsTable()
                                Where row.ric = _subRicToRic(aRic) And Not IsDBNull(row("date")) AndAlso IsDate(row("date"))
                                Let dt = row._date, rate = row.rating, rateSrc = row.ratingSrc
                                Select dt, rate, rateSrc).ToList

            Dim lastIssuerRating As RatingDescr
            If issuerRatings.Any Then
                Dim maxDate = (From rw In issuerRatings Select rw.dt).ToList().Max()
                Dim rt = issuerRatings.First(Function(elem) elem.dt = maxDate)
                lastIssuerRating = New RatingDescr(Rating.Parse(rt.rate), rt.dt, RatingSource.Parse(rt.rateSrc))
            Else
                lastIssuerRating = New RatingDescr(Rating.Other, Nothing, Nothing)
            End If

            Dim lastRating = If(lastIssueRating > lastIssuerRating, lastIssueRating, lastIssuerRating)

            Return New BondMetadata(ric, sN, sN, maturityDate, coupon,
                                       paymentStructure, rateStructure, issueDate, sN,
                                       shortName & " " & If(coupon > 0, String.Format("{0}", coupon), "ZC"),
                                       description, series, issuerName, borrowerName, currency,
                                       isPutable, isCallable, isFloater, lastIssueRating, lastIssuerRating, lastRating,
                                       seniorityType, industry, subIndustry)
        End Function

        'Private Shared Function SkipSlash(ByVal aRic As String) As String
        '    If aRic.Length = 0 Then Return aRic
        '    If aRic(0) = "/" Then Return aRic.Substring(1)
        '    Return aRic
        'End Function

        Public Function GetBondPayments(ByVal aRic As String) As BondPayments Implements IBondsData.GetBondPayments
            'aRic = SkipSlash(aRic)
            Dim descr = (From row In _ldr.GetBondsTable() Where row.ric = _subRicToRic(aRic) Select row).First()
            Dim rows = (From row In _ldr.GetCouponsTable()
                        Where row.ric = _subRicToRic(aRic)
                        Let dt = CDate(row._date)
                        Select row.ric, dt, row.rate
                        Order By dt)
            Dim res As New BondPayments(CDate(descr.issueDate), If(IsDate(descr.maturityDate), CDate(descr.maturityDate), Nothing))
            For Each row In rows
                res.AddPayment(row.dt, row.rate)
            Next
            Return res
        End Function

        Public Function GetBondInfo(ByVal rics As List(Of String)) As List(Of BondMetadata) Implements IBondsData.GetBondInfo
            Return (From item In rics Select GetBondInfo(item)).ToList()
        End Function
    End Class

    Public Class ChainProgress
        Implements IProgressObject

        Private ReadOnly _name As String

        Public Sub New(ByVal name As String)
            _name = name
        End Sub

        Public ReadOnly Property Name() As String Implements IProgressObject.Name
            Get
                Return _name
            End Get
        End Property
    End Class

    Public Class TableProgress
        Implements IProgressObject

        Private ReadOnly _name As String

        Public Sub New(ByVal name As String)
            _name = name
        End Sub

        Public ReadOnly Property Name() As String Implements IProgressObject.Name
            Get
                Return _name
            End Get
        End Property
    End Class

    Public Class BondLoaderProgressProcess
        Implements IProgressProcess

        Private WithEvents _chainLoader As New ReutersData.Chain
        Private ReadOnly _progress As New ProgressLog
        Private ReadOnly _handlers As New List(Of Action(Of ProgressEvent))
        Private Shared ReadOnly Logger As Logger = GetLogger(GetType(BondLoaderProgressProcess))

        Private Shared ReadOnly RicChain As BondsDataSet.RicChainDataTable = BondsLoader.Instance.GetRicChainTable()
        Private Shared ReadOnly CouponTable As BondsDataSet.CouponDataTable = BondsLoader.Instance.GetCouponsTable()
        Private Shared ReadOnly FrnTable As BondsDataSet.FrnDataTable = BondsLoader.Instance.GetFrnTable()
        Private Shared ReadOnly BondsTable As BondsDataSet.BondDataTable = BondsLoader.Instance.GetBondsTable()
        Private Shared ReadOnly IssueRatingsTable As BondsDataSet.IssueRatingDataTable = BondsLoader.Instance.GetIssueRatingsTable()
        Private Shared ReadOnly IssuerRatingsTable As BondsDataSet.IssuerRatingDataTable = BondsLoader.Instance.GetIssuerRatingsTable()
        Private Shared ReadOnly RicsTable As BondsDataSet.RicsDataTable = BondsLoader.Instance.GetRicsTable()

        Public Custom Event Progress As Action(Of ProgressEvent) Implements IProgressProcess.Progress
            AddHandler(ByVal value As Action(Of ProgressEvent))
                _handlers.Add(value)
            End AddHandler

            RemoveHandler(ByVal value As Action(Of ProgressEvent))
                _handlers.Remove(value)
            End RemoveHandler

            RaiseEvent(ByVal obj As ProgressEvent)
                _progress.LogEvent(obj)
                obj.Log = _progress
                For Each handler In _handlers
                    handler(obj)
                Next
            End RaiseEvent
        End Event

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
            New Dex2Field("EJV.C.ParentTicker", BondsTable.parentTickerColumn.ColumnName),
            New Dex2Field("EJV.C.SeniorityTypeDescription ", BondsTable.seniorityTypeColumn.ColumnName),
            New Dex2Field("EJV.C.SPIndustryDescription ", BondsTable.industryColumn.ColumnName),
            New Dex2Field("EJV.C.SPIndustrySubDescription ", BondsTable.subIndustryColumn.ColumnName)
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
            New Dex2Field("EJV.X.FREQ", FrnTable.frequencyColumn.ColumnName),
            New Dex2Field("EJV.C.IndexRic", FrnTable.indexRicColumn.ColumnName),
            New Dex2Field("EJV.X.ADF_MARGIN", FrnTable.marginColumn.ColumnName, itsNum:=True)
        }.ToList(), "RH:In")

        Private Shared ReadOnly QueryRics As New Dex2Query({
            New Dex2Field(0, RicsTable.ricColumn.ColumnName),
            New Dex2Field(1, RicsTable.contributorColumn.ColumnName),
            New Dex2Field("EJV.C.RICS", RicsTable.subRicColumn.ColumnName)
        }.ToList(), "RH:In;Con")

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
                RaiseEvent Progress(New ProgressEvent(MessageKind.Positive, msg, New TableProgress(table.TableName)))
                Dim rowsToDelete = (From row In table
                                   Where allowedRics.Contains(row("ric").ToString())
                                   Select row).ToList()

                For Each row In rowsToDelete
                    table.Rows.Remove(row)
                Next

                Dim dex2 As New Dex2
                AddHandler dex2.Failure, Sub(ex As Exception) RaiseEvent Progress(New ProgressEvent(MessageKind.Fail, "Failed to start Dex2", exc:=ex))
                AddHandler dex2.Metadata, handler
                dex2.Load(allowedRics.ToList(), query)
            Else
                handler(Nothing)
            End If
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
                             LoadStep5()
                         End Sub,
                         floaterRics)
        End Sub

        Private Sub LoadStep5()
            If SettingsManager.Instance.LoadRics Then
                Logger.Info("LoadAllRics")
                LoadGeneral(RicsTable, QueryRics, "Loading all rics",
                             Sub(data As LinkedList(Of Dictionary(Of String, Object)))
                                 ImportData(data, RicsTable, QueryRics)
                                 BondsData.Instance.Refresh()
                                 RaiseEvent Progress(New ProgressEvent(MessageKind.Finished, "All data loaded"))
                             End Sub)
            Else
                RaiseEvent Progress(New ProgressEvent(MessageKind.Finished, "All data loaded"))
            End If
        End Sub


        Public Sub Start(ByVal ParamArray params()) Implements IProgressProcess.Start
            Dim chainRics As List(Of String) = params(0)
            Dim dexParams As String = params(1)
            _chainLoader.StartChains(chainRics, dexParams)
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

        Private Sub OnChainData(ByVal ricOfChain As String, ByVal chainsAndRics As Dictionary(Of String, List(Of String)), ByVal finished As Boolean) Handles _chainLoader.Chain
            RaiseEvent Progress(New ProgressEvent(MessageKind.Positive, String.Format("Chain {0} arrived", ricOfChain), New ChainProgress(ricOfChain)))
            If Not finished Then Return

            If chainsAndRics.Count = 0 Then
                Exit Sub
            End If

            Dim chainRics = chainsAndRics.Keys.ToList()
            chainRics.ForEach(Sub(chainRic) StoreRicsAndChains(chainRic, chainsAndRics(chainRic)))

            Dim rics = GetRics(chainRics)
            If rics.Count = 0 Then
                Exit Sub
            End If

            StartLoad(rics)
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
                                rw(colName) = elem '.ToString()
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

        Private Sub StartLoad(ByVal requiredRics As List(Of String))
            Logger.Info("LoadMetadata")
            LoadGeneral(BondsTable, QueryBondDescr, "Loading bonds descriptions",
                         Sub(data As LinkedList(Of Dictionary(Of String, Object)))
                             If data IsNot Nothing Then
                                 ImportData(data, BondsTable, QueryBondDescr)
                                 LoadStep1(New HashSet(Of String)(requiredRics))
                             Else
                                 RaiseEvent Progress(New ProgressEvent(MessageKind.Fail, "No bond descriptions available"))
                             End If
                         End Sub,
                         New HashSet(Of String)(requiredRics))
        End Sub

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

        Private Sub ChainLoaderFailed(ByVal ric As String, ByVal ex As Exception, ByVal final As Boolean) Handles _chainLoader.Failed
            Logger.Trace("Chain failed")
            RaiseEvent Progress(New ProgressEvent(If(final, MessageKind.Fail, MessageKind.Negative), String.Format("Failed to load chain {0}", ric)))
        End Sub
    End Class

    Public NotInheritable Class BondsLoader
        Implements IBondsLoader

        Private Shared ReadOnly RicChain As New BondsDataSet.RicChainDataTable
        Private Shared ReadOnly CouponTable As New BondsDataSet.CouponDataTable
        Private Shared ReadOnly FrnTable As New BondsDataSet.FrnDataTable
        Private Shared ReadOnly BondsTable As New BondsDataSet.BondDataTable
        Private Shared ReadOnly IssueRatingsTable As New BondsDataSet.IssueRatingDataTable
        Private Shared ReadOnly IssuerRatingsTable As New BondsDataSet.IssuerRatingDataTable
        Private Shared ReadOnly RicsTable As New BondsDataSet.RicsDataTable
        Private Shared _me As IBondsLoader

        Public Event Progress As Action(Of ProgressEvent) Implements IBondsLoader.Progress

        Private Sub New()
        End Sub

        Public Shared ReadOnly Property Instance() As IBondsLoader
            Get
                If _me Is Nothing Then _me = New BondsLoader
                Return _me
            End Get
        End Property

        ''' <summary>
        ''' Entry point. Loads all data from configuration file and stores them into IMDB
        ''' </summary>
        Public Sub Initialize() Implements IBondsLoader.Initialize
            Dim ppp As New BondLoaderProgressProcess
            Dim chainRics = PortfolioManager.Instance().GetChainRics()
            If chainRics.Count = 0 Then
                Exit Sub
            End If
            AddHandler ppp.Progress, Sub(evt As ProgressEvent) RaiseEvent Progress(evt)
            ppp.Start(chainRics, "UWC:YES LAY:VER")
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
            Dim ppp As New BondLoaderProgressProcess
            AddHandler ppp.Progress, Sub(evt As ProgressEvent) RaiseEvent Progress(evt)
            ppp.Start(chainRics, "UWC:YES LAY:VER")
        End Sub

        ''' <summary>
        ''' Entry point. Loads all data for given RIC and stores them into IMDB
        ''' </summary>
        Public Sub LoadRic(ByVal ric As String, ByVal pseudoChain As String) Implements IBondsLoader.LoadRic
            Contract.Requires(ric <> "" And pseudoChain <> "")
            Dim ppp As New BondLoaderProgressProcess
            AddHandler ppp.Progress, Sub(evt) RaiseEvent Progress(evt)
            ppp.Start({ric}.ToList(), "UWC:YES LAY:VER")
        End Sub

        Public Function GetRicChainTable() As BondsDataSet.RicChainDataTable Implements IBondsLoader.GetRicChainTable
            Return RicChain
        End Function

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

        Public Function GetChainRics(ByVal chainRic As String) As List(Of String) Implements IBondsLoader.GetChainRics
            Return (From row In RicChain Where row.chain = chainRic Select row.ric).ToList()
        End Function

        Public Function GetFrnTable() As BondsDataSet.FrnDataTable Implements IBondsLoader.GetFrnTable
            Return FrnTable
        End Function

        Public Function GetRicsTable() As BondsDataSet.RicsDataTable Implements IBondsLoader.GetRicsTable
            Return RicsTable
        End Function

        Public Sub ClearTables() Implements IBondsLoader.ClearTables
            RicChain.Clear()
            CouponTable.Clear()
            FrnTable.Clear()
            BondsTable.Clear()
            IssueRatingsTable.Clear()
            IssuerRatingsTable.Clear()
            RicsTable.Clear()
        End Sub

        Public Function GetIsuuerRatingsTable() As BondsDataSet.IssuerRatingDataTable Implements IBondsLoader.GetIssuerRatingsTable
            Return IssuerRatingsTable
        End Function
    End Class
End Namespace