Imports Dex2Lib
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
        Event Initialzed As action

        Property Initialized() As Boolean

        ''' Loads all data from configuration file and stores them into IMDB
        Sub Initialize()

        ''' Loads all data from given chain and stores them into IMDB
        Sub LoadChain(ByVal chainRic As String)

        ''' Loads all data from given chains and stores them into IMDB
        Sub LoadChains(ByVal chainRics As List(Of String))

        ''' Loads all data for given RIC and stores them into IMDB with given pseudo chain
        Sub LoadRic(ByVal ric As String, ByVal pseudoChain As String)
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

        Private _rData As RData
        Private Shared _instance As BondsLoader

        Public Event Progress As Action(Of String) Implements IBondsLoader.Progress
        Public Event Success As Action Implements IBondsLoader.Success
        Public Event NoBonds As action Implements IBondsLoader.NoBonds
        Public Event Failure As Action(Of Exception) Implements IBondsLoader.Failure
        Public Event Initialzed As Action Implements IBondsLoader.Initialzed

        Private Structure FieldDescription
            Public Field As String
            Public FieldNum As Integer
            Public ColumnName As String
            Public ItsDate As Boolean
            Public ItsBool As Boolean
            Public ItsNum As Boolean

            Public Sub New(ByVal field As String, ByVal columnName As String, Optional ByVal itsDate As Boolean = False, Optional ByVal itsBool As Boolean = False, Optional ByVal itsNum As Boolean = False)
                Me.Field = field
                Me.ColumnName = columnName
                Me.ItsDate = itsDate
                Me.ItsBool = itsBool
                Me.ItsNum = itsNum
            End Sub

            Public Sub New(ByVal fieldNum As Integer, ByVal columnName As String, Optional ByVal itsDate As Boolean = False, Optional ByVal itsBool As Boolean = False, Optional ByVal itsNum As Boolean = False)
                Me.FieldNum = fieldNum
                Me.ColumnName = columnName
                Me.ItsDate = itsDate
                Me.ItsBool = itsBool
                Me.ItsNum = itsNum
            End Sub
        End Structure

        Private Shared ReadOnly BondDescrFields As List(Of FieldDescription) = {
            New FieldDescription With {.FieldNum = 0, .ColumnName = BondsTable.ricColumn.ColumnName, .ItsDate = False, .ItsBool = False, .ItsNum = False},
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
            New FieldDescription With {.Field = "EJV.C.IsConvertible", .ColumnName = BondsTable.isConvertibleColumn.ColumnName, .ItsDate = False, .ItsBool = True, .ItsNum = False},
            New FieldDescription With {.Field = "EJV.C.IsStraight", .ColumnName = BondsTable.isStraightColumn.ColumnName, .ItsDate = False, .ItsBool = True, .ItsNum = False},
            New FieldDescription With {.Field = "EJV.C.Ticker", .ColumnName = BondsTable.tickerColumn.ColumnName, .ItsDate = False, .ItsBool = False, .ItsNum = False},
            New FieldDescription With {.Field = "EJV.C.Series", .ColumnName = BondsTable.seriesColumn.ColumnName, .ItsDate = False, .ItsBool = False, .ItsNum = False},
            New FieldDescription With {.Field = "EJV.C.BorrowerCntyCode", .ColumnName = BondsTable.borrowerCountryColumn.ColumnName, .ItsDate = False, .ItsBool = False, .ItsNum = False},
            New FieldDescription With {.Field = "EJV.C.IssuerCountry", .ColumnName = BondsTable.issuerCountryColumn.ColumnName, .ItsDate = False, .ItsBool = False, .ItsNum = False}
        }.ToList()

        Private _initialized As Boolean
        Private Const BondsDescrDisplay = "RH:In"
        Private Const BondsDescrRequest = ""

        Private Shared ReadOnly BondIssueRatingFields As List(Of FieldDescription) = {
            New FieldDescription(0, IssueRatingsTable.ricColumn.ColumnName),
            New FieldDescription("EJV.GR.Rating", IssueRatingsTable.ratingColumn.ColumnName),
            New FieldDescription("EJV.GR.RatingDate", IssueRatingsTable.dateColumn.ColumnName),
            New FieldDescription("EJV.GR.RatingSourceCode", IssueRatingsTable.ratingSrcColumn.ColumnName)
        }.ToList()
        Private Const BondIssueRatingDisplay = "RH:In"
        Private Const BondIssueRatingRequest = "RTSRC:MDY;S&P RTS:FDL;SPI;MDL RTSC:FRN"

        Private Shared ReadOnly BondIssuerRatingFields As List(Of FieldDescription) = {
            New FieldDescription(0, IssuerRatingsTable.ricColumn.ColumnName),
            New FieldDescription("EJV.IR.Rating", IssuerRatingsTable.ratingColumn.ColumnName),
            New FieldDescription("EJV.IR.RatingDate", IssuerRatingsTable.dateColumn.ColumnName),
            New FieldDescription("EJV.IR.RatingSourceCode", IssuerRatingsTable.ratingSrcColumn.ColumnName)
        }.ToList()
        Private Const BondIssuerRatingisplay = "RH:In"
        Private Const BondIssuerRatingRequest = "RTSRC:MDY;S&P RTS:FDL;SPI;MDL RTSC:FRN"

        Private Shared ReadOnly BondCouponFields As List(Of FieldDescription) = {
            New FieldDescription(0, CouponTable.ricColumn.ColumnName),
            New FieldDescription(1, CouponTable.dateColumn.ColumnName, itsDate:=True),
            New FieldDescription("EJV.C.CouponRate", CouponTable.rateColumn.ColumnName, itsNum:=True)
        }.ToList()
        Private Const BondCouponDisplay = "RH:In,D"
        Private Const BondCouponRequest = ""

        Private Shared ReadOnly FrnFields As List(Of FieldDescription) = {
            New FieldDescription(0, FrnTable.ricColumn.ColumnName),
            New FieldDescription("EJV.X.FRNFLOOR", FrnTable.floorColumn.ColumnName),
            New FieldDescription("EJV.X.FRNCAP", FrnTable.floorColumn.ColumnName),
            New FieldDescription("PAY_FREQ", FrnTable.floorColumn.ColumnName),
            New FieldDescription("EJV.C.IndexRic", FrnTable.floorColumn.ColumnName),
            New FieldDescription("EJV.X.ADF_MARGIN", FrnTable.floorColumn.ColumnName, itsNum:=True)
        }.ToList()
        Private Const FrnDisplay = "RH:In"
        Private Const FrnRequest = ""

        Public Property Initialized() As Boolean Implements IBondsLoader.Initialized
            Get
                Return _initialized
            End Get
            Private Set(ByVal value As Boolean)
                _initialized = value
                If _initialized Then RaiseEvent Initialzed()
            End Set
        End Property

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
            LoadChains({chainRic}.ToList())
        End Sub

        ''' <summary>
        ''' Entry point. Loads all data from given chains and stores them into IMDB
        ''' </summary>
        Public Sub LoadChains(ByVal chainRics As List(Of String)) Implements IBondsLoader.LoadChains
            _chainLoader.StartChains(chainRics, "UWC:YES LAY:VER")
        End Sub

        ''' <summary>
        ''' Entry point. Loads all data for given RIC and stores them into IMDB
        ''' </summary>
        Public Sub LoadRic(ByVal ric As String, ByVal pseudoChain As String) Implements IBondsLoader.LoadRic
            StoreRicsAndChains(ric, {ric}.ToList())
            LoadMetadata(ric)
        End Sub

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

            LoadMetadata(rics.Aggregate(Function(str, ric) str + "," + ric))
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

        Private Sub LoadMetadata(ByVal rics As String)
            Logger.Info("LoadMetadata")
            ' todo cleanup before adding! This method might be used not only at the beginning
            RaiseEvent Progress("Loading bonds descriptions")
            Try
                Dim dex2 As Dex2Mgr = Eikon.Sdk.CreateDex2Mgr()
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
            For i = data.GetLowerBound(0) To data.GetUpperBound(0)
                Dim rw = BondsTable.NewRow()
                For j = 0 To BondDescrFields.Count - 1
                    Dim fieldDescr = BondDescrFields(j)
                    Dim elem = data.GetValue(i, j + 1)
                    If fieldDescr.ItsBool Then
                        rw(fieldDescr.ColumnName) = (elem = "Y")
                    ElseIf fieldDescr.ItsDate Then
                        If IsDate(elem) Then rw(fieldDescr.ColumnName) = Date.Parse(elem)
                    ElseIf fieldDescr.ItsNum Then
                        If IsNumeric(elem) Then rw(fieldDescr.ColumnName) = Double.Parse(elem)
                    Else
                        rw(fieldDescr.ColumnName) = elem
                    End If
                Next
                BondsTable.AddBondRow(rw)
            Next
            RemoveHandler _rData.OnUpdate, AddressOf ImportBondsData

            Initialized = True
        End Sub

        Private Sub New()
        End Sub

        Public Shared Function GetInstance() As IBondsLoader
            If _instance Is Nothing Then _instance = New BondsLoader
            Return _instance
        End Function

    End Class
End Namespace