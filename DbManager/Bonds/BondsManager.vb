Imports Dex2Lib
Imports Logging
Imports NLog
Imports ReutersData

Namespace Bonds

    Public Interface IBondsData
        '' Returns BondDescription object which contains data on selected bond
        Function GetBondInfo(ByVal ric As String) As BondDescription
        Function GetBondPayments(ByVal ric As String) As BondPayments

    End Interface

    Public Class BondsData
        Implements IBondsData

        Private Shared ReadOnly AnInstance As New BondsData

        Private Sub New()
        End Sub

        Public Shared ReadOnly Property Instance As IBondsData
            Get
                Return anInstance
            End Get
        End Property

        Public Function GetBondInfo(ByVal ric As String) As BondDescription Implements IBondsData.GetBondInfo
            Return Nothing ' todo
        End Function

        Public Function GetBondPayments(ByVal ric As String) As BondPayments Implements IBondsData.GetBondPayments
            Throw New NotImplementedException()
        End Function
    End Class

    Public Interface IBondsLoader
        Event Progress As Action(Of String)
        Event Success As Action
        Event NoBonds As action
        Event Failure As Action(Of Exception)
        Event Initialzed As action

        Function IsInitialized() As Boolean

        ''' Loads all data from configuration file and stores them into IMDB
        Sub UpdateAllChains()

        ''' Loads all data from given chain and stores them into IMDB
        Sub LoadChain(ByVal chainRic As String)

        ''' Loads all data from given chains and stores them into IMDB
        Sub LoadChains(ByVal chainRics As List(Of String))

        ''' Loads all data for given RIC and stores them into IMDB
        Sub LoadRic(ByVal ric As String, ByVal pseudoChain As String)
    End Interface

    Public Class BondsLoader
        Implements IBondsLoader
        Private WithEvents _chainLoader As New Chain

        Private Shared ReadOnly Logger As Logger = GetLogger(GetType(BondsLoader))
        Private Shared ReadOnly RicChain As New BondsDataSet.RicChainDataTable
        Private Shared ReadOnly BondsTable As New BondsDataSet.BondDataTable
        Private Shared ReadOnly IssueRatingsTable As New BondsDataSet.IssueRatingDataTable
        Private Shared ReadOnly IssuerRatingsTable As New BondsDataSet.IssuerRatingDataTable

        Private _rData As RData
        Private Shared _instance As BondsLoader

        Public Event Progress As Action(Of String) Implements IBondsLoader.Progress
        Public Event Success As Action Implements IBondsLoader.Success
        Public Event NoBonds As action Implements IBondsLoader.NoBonds
        Public Event Failure As Action(Of Exception) Implements IBondsLoader.Failure
        Public Event Initialzed As Action Implements IBondsLoader.Initialzed

        Private Structure FieldDescription
            Public Field As String
            Public ColumnName As String
            Public ItsDate As Boolean
            Public ItsBool As Boolean
            Public ItsNum As Boolean
        End Structure

        Private Shared ReadOnly BondDescrFields As List(Of FieldDescription) =
                                    {
                                        New FieldDescription With {.Field = "EJV.X.ADF_BondStructure", .ColumnName = BondsTable.bondStructureColumn.ColumnName, .ItsDate = False, .ItsBool = False, .ItsNum = False},
                                        New FieldDescription With {.Field = "EJV.X.ADF_RateStructure", .ColumnName = BondsTable.rateStructureColumn.ColumnName, .ItsDate = False, .ItsBool = False, .ItsNum = False},
                                        New FieldDescription With {.Field = "EJV.C.Description", .ColumnName = BondsTable.descriptionColumn.ColumnName, .ItsDate = False, .ItsBool = False, .ItsNum = False},
                                        New FieldDescription With {.Field = "EJV.C.OriginalAmountIssued", .ColumnName = BondsTable.issueSizeColumn.ColumnName, .ItsDate = False, .ItsBool = False, .ItsNum = True},
                                        New FieldDescription With {.Field = "EJV.C.IssuerName", .ColumnName = BondsTable.issuerNameColumn.ColumnName, .ItsDate = False, .ItsBool = False, .ItsNum = False},
                                        New FieldDescription With {.Field = "EJV.C.BorrowerName", .ColumnName = BondsTable.borrowerNameColumn.ColumnName, .ItsDate = False, .ItsBool = False, .ItsNum = False},
                                        New FieldDescription With {.Field = "EJV.X.ADF_Coupon", .ColumnName = BondsTable.currentCouponColumn.ColumnName, .ItsDate = False, .ItsBool = False, .ItsNum = True},
                                        New FieldDescription With {.Field = "EJV.C.IssueDate", .ColumnName = BondsTable.issueDateColumn.ColumnName, .ItsDate = True, .ItsBool = False, .ItsNum = False},
                                        New FieldDescription With {.Field = "EJV.C.MaturityDate", .ColumnName = BondsTable.maturityDateColumn.ColumnName, .ItsDate = True, .ItsBool = False, .ItsNum = False},
                                        New FieldDescription With {.Field = "EJV.C.Currency", .ColumnName = BondsTable.currencyColumn.ColumnName, .ItsDate = False, .ItsBool = False, .ItsNum = False},
                                        New FieldDescription With {.Field = "EJV.C.ShortName", .ColumnName = BondsTable.shortNameColumn.ColumnName, .ItsDate = False, .ItsBool = False, .ItsNum = False},
                                        New FieldDescription With {.Field = "EJV.C.IsCallable", .ColumnName = BondsTable.isCallableColumn.ColumnName, .ItsDate = False, .ItsBool = True, .ItsNum = False},
                                        New FieldDescription With {.Field = "EJV.C.IsPutable", .ColumnName = BondsTable.isPutableColumn.ColumnName, .ItsDate = False, .ItsBool = True, .ItsNum = False},
                                        New FieldDescription With {.Field = "EJV.C.IsFloater", .ColumnName = BondsTable.isFloaterColumn.ColumnName, .ItsDate = False, .ItsBool = True, .ItsNum = False},
                                        New FieldDescription With {.Field = "EJV.C.IsStraight", .ColumnName = BondsTable.isStraightColumn.ColumnName, .ItsDate = False, .ItsBool = True, .ItsNum = False},
                                        New FieldDescription With {.Field = "EJV.C.Ticker", .ColumnName = BondsTable.tickerColumn.ColumnName, .ItsDate = False, .ItsBool = False, .ItsNum = False},
                                        New FieldDescription With {.Field = "EJV.C.Series", .ColumnName = BondsTable.seriesColumn.ColumnName, .ItsDate = False, .ItsBool = False, .ItsNum = False},
                                        New FieldDescription With {.Field = "EJV.C.BorrowerCntyCode", .ColumnName = BondsTable.borrowerCountryColumn.ColumnName, .ItsDate = False, .ItsBool = False, .ItsNum = False},
                                        New FieldDescription With {.Field = "EJV.C.IssuerCountry", .ColumnName = BondsTable.issuerCountryColumn.ColumnName, .ItsDate = False, .ItsBool = False, .ItsNum = False}
                                    }.ToList()

        Private _initialized As Boolean
        Private Const BondsDescrDisplay = "RH:In"
        Private Const BondsDescrRequest = ""

        Private Const BondIssueRatingFields = "EJV.GR.Rating, EJV.GR.RatingDate"
        Private Const BondIssueRatingDisplay = "RH:In"
        Private Const BondIssueRatingRequest = "RTSRC:MDY;S&P RTS:FDL;SPI;MDL RTSC:FRN"

        Private Const BondIssuerRatingFields = "EJV.IR.Rating, EJV.IR.RatingDate"
        Private Const BondIssuerRatingisplay = "RH:In"
        Private Const BondIssuerRatingRequest = "RTSRC:MDY;S&P RTS:FDL;SPI;MDL RTSC:FRN"

        Private Const BondCouponFields = "EJV.C.CouponRate"
        Private Const BondCouponDisplay = "RH:In,D"
        Private Const BondCouponRequest = ""

        Private Const FrnFields = "EJV.X.FRNFLOOR, EJV.X.FRNCAP, PAY_FREQ, EJV.C.IndexRic, EJV.X.ADF_MARGIN"
        Private Const FrnDisplay = "RH:In"
        Private Const FrnRequest = ""

        Public Function IsInitialized() As Boolean Implements IBondsLoader.IsInitialized
            Return _initialized
        End Function

        ''' <summary>
        ''' Entry point. Loads all data from configuration file and stores them into IMDB
        ''' </summary>
        Public Sub UpdateAllChains() Implements IBondsLoader.UpdateAllChains
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
            LoadChains({chainRic}.ToList())
        End Sub

        ''' <summary>
        ''' Entry point. Loads all data from given chains and stores them into IMDB
        ''' </summary>
        Public Sub LoadChains(ByVal chainRics As List(Of String)) Implements IBondsLoader.LoadChains
            _chainLoader.StartChains(chainRics, "UWC:YES LAY:VER")
        End Sub

        Private Sub _chainLoader_Arrived(ByVal ric As String) Handles _chainLoader.Arrived
            RaiseEvent Progress(ric)
        End Sub

        Private Sub OnChain(ByVal chainsAndRics As Dictionary(Of String, List(Of String))) Handles _chainLoader.Chain
            If chainsAndRics.Count = 0 Then
                RaiseEvent NoBonds()
                Exit Sub
            End If
            chainsAndRics.Keys.ToList().ForEach(Sub(chainRic) StoreRicsAndChains(chainRic, chainsAndRics(chainRic)))

            Dim chainRics = PortfolioManager.GetInstance().GetChainRics()
            Dim rics = GetRics(chainRics)
            If rics.Count = 0 Then
                RaiseEvent NoBonds()
                Exit Sub
            End If
            LoadBonds(rics.Aggregate(Function(str, ric) str + "," + ric))
        End Sub

        ''' <summary>
        ''' Entry point. Loads all data for given RIC and stores them into IMDB
        ''' </summary>
        Public Sub LoadRic(ByVal ric As String, ByVal pseudoChain As String) Implements IBondsLoader.LoadRic
            StoreRicsAndChains(ric, {ric}.ToList())
            Dim rics = GetRics(pseudoChain)
            LoadBonds(rics.Aggregate(Function(str, item) str + "," + item))
        End Sub

        Private Shared Function GetRics() As List(Of String)
            Return (From row As BondsDataSet.RicChainRow In RicChain.Rows Select row.ric Distinct).ToList()
        End Function

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

        'Private ReadOnly _chainLoader As New ChainLoadManager

        Private Sub LoadBondsFromChains(ByVal chainRics As List(Of String))
            Logger.Info("LoadBondsFromChain")
            'Dim res As New Dictionary(Of String, List(Of String))
            'chainRics.ForEach(
            '    Sub(item)
            '        Try
            '            Dim handler = _chainLoader.AddListToLoad(item, "UWC:YES LAY:VER")
            '            res.Add(item, New List(Of String)())

            '            AddHandler handler.OnData, AddressOf OnChainItems

            '            adxChain.RequestChain()
            '        Catch xEx As XPathException
            '            Logger.WarnException("Failed to find any chains in DB", xEx)
            '            Logger.Warn("Exception = {0}", xEx)
            '            RaiseEvent Failure(xEx)
            '        Catch ex As Exception
            '            Logger.ErrorException("Failed to load chains", ex)
            '            Logger.Error("Exception = {0}", ex)
            '            RaiseEvent Failure(ex)
            '        End Try
            '    End Sub)
            'RaiseEvent Progress("All chains loaded")
            'Return res
        End Sub

        'Sub OnChainItems(ByRef sender As Object, ByVal e As ChainItemsData)
        '    Dim handler As ChainHandler = sender
        '    Try
        '        RaiseEvent Progress("Loading chain " + e.ChainName)
        '        e.Handled = True
        '        If dataStatus = RT_DataStatus.RT_DS_FULL Then
        '            Dim data As Array = adxChain.Data
        '            Logger.Debug("filling chain {0}; total items is {1}", item, data.GetUpperBound(0) - data.GetLowerBound(0))

        '            For i = data.GetUpperBound(0) To data.GetLowerBound(0)
        '                res(item).Add(data.GetValue(i).ToString())
        '            Next
        '        ElseIf dataStatus = RT_DataStatus.RT_DS_NULL_ERROR Then
        '            Logger.Warn("Chain failure: {0}", item)
        '        End If
        '    Catch ex As Exception
        '        Logger.ErrorException("Failed to parse chain " + item, ex)
        '        Logger.Error("Exception = {0}", ex.ToString())
        '    End Try

        '    RemoveHandler sender.OnData, AddressOf OnChainItems
        'End Sub

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

        Private Sub LoadBonds(ByVal rics As String)
            Logger.Info("LoadBonds")

            Try
                Dim dex2 As Dex2Mgr = EikonSdk.Sdk.CreateDex2Mgr()
                Dim cookie = dex2.Initialize()
                _rData = dex2.CreateRData(cookie)
                _rData.InstrumentIDList = rics
            Catch ex As Exception
                Logger.ErrorException("Failed to init Dex2 ", ex)
                Logger.Error("Exception = {0}", ex)
                RaiseEvent Failure(ex)
                Exit Sub
            End Try

            _rData.FieldList = (From fieldDescr In BondDescrFields Select fieldDescr.Field).Aggregate(Function(str, field) str + "," + field)
            _rData.DisplayParam = BondsDescrDisplay
            _rData.RequestParam = BondsDescrRequest

            AddHandler _rData.OnUpdate, AddressOf ImportBondsData
            _rData.Subscribe(False)
        End Sub

        Private Sub ImportBondsData(ByVal dataStatus As DEX2_DataStatus, ByVal err As Object)
            Dim data = _rData.Data
            For i = data.GetLowerBound(0) + 1 To data.GetUpperBound(0)
                Dim rw = BondsTable.NewRow()
                rw(BondsTable.ricColumn) = data.GetValue(i, 0)
                For j = 0 To BondDescrFields.Count
                    Dim fieldDescr = BondDescrFields(i)
                    Dim elem = data.GetValue(i, j + 1)
                    If fieldDescr.ItsBool Then
                        rw(fieldDescr.ColumnName) = (elem = "Y")
                    ElseIf fieldDescr.ItsDate Then
                        rw(fieldDescr.ColumnName) = String.Format("{0:yyyyMMdd}", elem)
                    ElseIf fieldDescr.ItsNum Then
                        If IsNumeric(elem) Then rw(fieldDescr.ColumnName) = Double.Parse(elem)
                    Else
                        rw(fieldDescr.ColumnName) = elem
                    End If
                Next
                BondsTable.AddBondRow(rw)
            Next
            RemoveHandler _rData.OnUpdate, AddressOf ImportBondsData

            _initialized = True
        End Sub

        Private Sub New()
        End Sub

        Public Shared Function GetInstance() As IBondsLoader
            If _instance Is Nothing Then _instance = New BondsLoader
            Return _instance
        End Function

    End Class
End Namespace