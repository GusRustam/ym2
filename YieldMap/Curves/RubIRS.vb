Imports AdfinXAnalyticsFunctions
Imports AdfinXRtLib
Imports System.Text.RegularExpressions
Imports System.Drawing
Imports Uitls
Imports YieldMap.Tools.Estimation
Imports YieldMap.Tools
Imports YieldMap.Tools.History
Imports NLog
Imports YieldMap.Tools.Lists

Namespace Curves
    Public Interface IAssetSwapBenchmark
        Function CanBeBenchmark() As Boolean

        ReadOnly Property FloatLegStructure() As String
        ReadOnly Property FloatingPointValue() As Double
    End Interface

    Public Class RubIRS
        Inherits SwapCurve
        Implements IAssetSwapBenchmark

        Public Sub New(bmk As SpreadContainer)
            MyBase.New(bmk)
        End Sub

        '' LOGGER
        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(RubIRS))

        '' SWAP STRUCTURES
        Private Shared ReadOnly Struct =
            "LBOTH CLDR:RUS ARND:NO CFADJ:YES CRND:NO DMC:MODIFIED EMC:SAMEDAY IC:S1 " +
            "PDELAY:0 REFDATE:MATURITY RP:1 XD:NO LPAID LTYPE:FIXED CCM:A5P FRQ:Y " +
            "LRECEIVED LTYPE:FLOAT CCM:MMAA FRQ:Q"

        Private Shared ReadOnly FloatLeg =
            "CLDR:RUS ARND:NO CCM:MMAA CFADJ:YES CRND:NO DMC:MODIFIED EMC:SAMEDAY IC:S1 " +
            "PDELAY:0 REFDATE:MATURITY RP:1 RT:BULLET XD:NO FRQ:Q "

        Protected Overridable Property InstrumentName As String = "RUBAM3MO"
        Protected Overridable Property BaseInstrument As String = "MOSPRIME3MD="
        Protected Overridable Property AllowedTenors() As String() = {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"}
        Protected Overridable Property Brokers() As String() = {"GFI", "TRDL", "ICAP", ""}

        Private Shared ReadOnly InstrumentType = "S"
        Private Shared ReadOnly PossibleQuotes() As String = {"BID", "ASK", "MID"}

        Private _bootstrapped As Boolean

        '' LOADERS
        Private WithEvents _quoteLoader As New ListLoadManager

        '' DATA LOADING PARAMETERS
        Private _theDate As Date = Date.Today
        Private _broker As String = ""
        Private _quote As String = "MID"
        Private ReadOnly _name As String = Guid.NewGuid().ToString()


        ''' <summary>
        ''' Parsing swap name to retrieve term
        ''' </summary>
        ''' <param name="ric">swap ric</param>
        ''' <returns>numeric value - swap tenor</returns>
        ''' <remarks></remarks>
        Public Overrides Function GetDuration(ByVal ric As String) As Double
            Dim match = Regex.Match(ric, String.Format("{0}(?<year>[0-9]+?)Y.*", InstrumentName))
            Dim capture = match.Groups("year").Value
            Return CInt(capture)
        End Function

        Public Overrides Function RemoveItem(ByVal ric As String) As Boolean
            Return False
        End Function

        Public Overrides Function RemoveItems(ByVal ric As List(Of String)) As Boolean
            Return False
        End Function

        Public Overrides Sub AddItems(ByVal rics As List(Of String))
            Return
        End Sub

        ''' <summary>
        ''' Return full list of rics to be loaded
        ''' </summary>
        ''' <param name="broker">broker name</param>
        ''' <returns>a list of string</returns>
        ''' <remarks></remarks>
        Protected Overrides Function GetRICs(ByVal broker As String) As List(Of String)
            Return AllowedTenors.Select(Function(item) String.Format("{0}{1}Y={2}", InstrumentName, item, broker)).ToList()
        End Function

        Public Overrides Sub RecalculateByType(ByVal type As SpreadType)
            Logger.Trace("RecalculateByType({0})", type)
            Dim rics = Descrs.Keys.ToList()
            If SpreadBmk.Benchmarks.ContainsKey(type) AndAlso SpreadBmk.Benchmarks(type).GetName() = GetName() Then
                rics.ForEach(Sub(ric) SpreadBmk.CleanupSpread(Descrs(ric), type:=type))
            Else
                rics.ForEach(Sub(ric) SpreadBmk.CalcAllSpreads(Descrs(ric), type:=type))
            End If
            If SpreadBmk.CurrentType = type Then NotifyRecalculated(Me)
        End Sub

        '' START LOADING HISTORICAL DATA
        Protected Overrides Sub LoadHistory()
            Logger.Debug("LoadHistory")
            Dim rics = GetRICs(_broker)
            rics.ForEach(Sub(ric) DoLoadRIC(ric, {"DATE", "BID", "ASK"}.ToList, _theDate))
            DoLoadRIC(BaseInstrument, {"DATE", "CLOSE"}.ToList, _theDate)
        End Sub

        '' START LOADING REALTIME DATA
        Protected Overrides Sub StartRealTime()
            If Not _quoteLoader.StartNewTask(New ListTaskDescr() With {
                                                .Name = _name,
                                                .Items = GetRICs(_broker),
                                                .Fields = {"275", "393"}.ToList()
                                            }) Then
                Logger.Error("Failed to start loading {0} data", Me.GetType().Name)
                Throw New InvalidOperationException("Failed to start loading RUBIRS data")
            End If
            If BaseInstrument <> "" Then
                If Not _quoteLoader.StartNewTask(New ListTaskDescr() With {
                                                    .Name = _name + "_BASE",
                                                    .Items = {BaseInstrument}.ToList(),
                                                    .Fields = {"BID", "ASK"}.ToList()
                                                }) Then
                    Logger.Error("Failed to start loading {0}_BASE data", Me.GetType().Name)
                    Throw New InvalidOperationException(String.Format("Failed to start loading {0}_BASE data", Me.GetType().Name))
                End If
            End If
        End Sub

        '' HISTORICAL DATA ARRIVED
        Protected Overrides Sub OnHistoricalData(ByVal hst As HistoryLoadManager, ByVal ric As String, ByVal datastatus As RT_DataStatus, ByVal data As Dictionary(Of Date, HistoricalItem))
            Logger.Debug("OnHistoricalData({0})", ric)
            If data IsNot Nothing Then
                Dim lastDate = data.Keys.Max
                Dim elem = data(lastDate)
                Dim aYield As Double
                Select Case _quote
                    Case "BID" : aYield = elem.Bid
                    Case "ASK" : aYield = elem.Ask
                    Case "MID" : aYield = If(elem.Bid > 0 And elem.Ask > 0, (elem.Bid + elem.Ask) / 2, If(elem.Bid > 0, elem.Bid, elem.Ask))
                End Select
                Try
                    If ric <> BaseInstrument Then
                        With Descrs(ric)
                            .Yield = aYield / 100
                            .Duration = GetDuration(ric)
                            .YieldAtDate = lastDate
                        End With
                    Else
                        BaseInstrumentPrice = elem.Value
                    End If
                    NotifyUpdated(Me)
                Catch ex As Exception
                    Logger.WarnException("Failed to parse instrument " + ric, ex)
                    Logger.Warn("Exception = {0}", ex.ToString())
                End Try
            Else
                Logger.Warn("No data!")
            End If
        End Sub

        Public Overrides Function GetOriginalRICs() As List(Of String)
            Return GetRICs("")
        End Function

        Public Overrides Function GetCurrentRICs() As List(Of String)
            Return GetRICs(GetBroker())
        End Function

        Public Overrides Sub Cleanup()
            _quoteLoader.DiscardTask(_name)
            _quoteLoader.DiscardTask(_name + "_BASE")
            MyBase.Cleanup()
        End Sub

        '' REALTIME DATA ARRIVED
        Private Sub OnRealTimeData(ByVal data As Dictionary(Of String, Dictionary(Of String, Dictionary(Of String, Double)))) Handles _quoteLoader.OnNewData
            Logger.Debug("OnRealTimeData")
            For Each listAndRFV As KeyValuePair(Of String, Dictionary(Of String, Dictionary(Of String, Double))) In data
                Dim list = listAndRFV.Key
                Dim rfv = listAndRFV.Value

                If list = _name Then
                    Logger.Info(Me.GetType().Name)
                    For Each ricAndFieldValue As KeyValuePair(Of String, Dictionary(Of String, Double)) In rfv
                        Dim ric = ricAndFieldValue.Key
                        Logger.Trace("Got RIC {0}", ric)

                        ' define yield curve elem
                        Dim duration = GetDuration(ric)
                        If ricAndFieldValue.Value.Keys.Contains("393") Or ricAndFieldValue.Value.Keys.Contains("275") Then
                            Try
                                Descrs(ric).YieldAtDate = GetDate()
                                If _quote = "BID" Or _quote = "ASK" Then
                                    Dim yld As Double
                                    yld = CDbl(ricAndFieldValue.Value(IIf(_quote = "BID", "393", "275")))
                                    If yld > 0 Then
                                        Descrs(ric).Yield = yld / 100
                                        Descrs(ric).Duration = duration
                                        NotifyUpdated(Me)
                                    End If
                                Else
                                    Dim bidYield = CDbl(ricAndFieldValue.Value("393")) / 100
                                    Dim askYield = CDbl(ricAndFieldValue.Value("275")) / 100
                                    Dim found = True
                                    If bidYield > 0 And askYield > 0 Then
                                        Descrs(ric).Yield = (bidYield + askYield) / 2
                                    ElseIf bidYield > 0 Then
                                        Descrs(ric).Yield = bidYield
                                    ElseIf askYield > 0 Then
                                        Descrs(ric).Yield = askYield
                                    Else
                                        found = False
                                    End If
                                    If found Then
                                        Descrs(ric).Duration = duration
                                        NotifyUpdated(Me)
                                    End If
                                End If
                            Catch ex As Exception
                                Logger.WarnException("Failed to parse realtime data", ex)
                                Logger.Warn("Exception = {0}", ex.ToString())
                            End Try
                        End If

#If DEBUG Then
                        Dim fieldValue = ricAndFieldValue.Value
                        For Each fv As KeyValuePair(Of String, Double) In fieldValue
                            Logger.Trace("  {0} -> {1}", fv.Key, fv.Value)
                        Next
#End If

                    Next
                ElseIf list = _name + "_BASE" Then
                    Logger.Info(Me.GetType().Name + "_BASE")
                    If rfv.Keys.First <> BaseInstrument Then
                        Logger.Warn("No base data in {0}", rfv.ToString())
                    Else
                        For Each ricAndFieldValue As KeyValuePair(Of String, Dictionary(Of String, Double)) In rfv
                            Logger.Trace("Got base instrument {0}", BaseInstrument)
                            If ricAndFieldValue.Value.Keys.Contains("BID") Or ricAndFieldValue.Value.Keys.Contains("ASK") Then
                                Try
                                    Dim found = False
                                    If _quote = "BID" Or _quote = "ASK" Then
                                        Dim yld As Double
                                        yld = CDbl(ricAndFieldValue.Value(_quote))
                                        If yld > 0 Then
                                            BaseInstrumentPrice = yld
                                            found = True
                                        End If
                                    Else
                                        found = True
                                        Dim bidYield = CDbl(ricAndFieldValue.Value("BID"))
                                        Dim askYield = CDbl(ricAndFieldValue.Value("ASK"))
                                        If bidYield > 0 And askYield > 0 Then
                                            BaseInstrumentPrice = (bidYield + askYield) / 2
                                        ElseIf bidYield > 0 Then
                                            BaseInstrumentPrice = bidYield
                                        ElseIf askYield > 0 Then
                                            BaseInstrumentPrice = askYield
                                        Else
                                            found = False
                                        End If
                                    End If
                                    If found Then NotifyUpdated(Me)
                                Catch ex As Exception
                                    Logger.WarnException("Failed to parse realtime base data", ex)
                                    Logger.Warn("Exception = {0}", ex.ToString())
                                End Try
                            End If
#If DEBUG Then
                            Dim fieldValue = ricAndFieldValue.Value
                            For Each fv As KeyValuePair(Of String, Double) In fieldValue
                                Logger.Trace("  {0} -> {1}", fv.Key, fv.Value)
                            Next
#End If
                        Next
                    End If
                Else
                    Logger.Info("Will not handle unknown list {0}", list)
                End If
            Next
        End Sub

        Public Overrides Function IsBootstrapped() As Boolean
            Return _bootstrapped
        End Function

        Public Overrides Sub SetBootstrapped(flag As Boolean)
            _bootstrapped = flag
            NotifyUpdated(Me)
        End Sub

        Public Overrides Function Bootstrap(ByVal data As List(Of SwapPointDescription)) As List(Of SwapPointDescription)
            Dim params(0 To data.Count() - 1, 5) As Object
            For i = 0 To data.Count - 1
                params(i, 0) = InstrumentType
                params(i, 1) = _theDate
                params(i, 2) = _theDate.AddDays(data(i).Duration * 365.0)
                params(i, 3) = BaseInstrumentPrice / 100
                params(i, 4) = data(i).Yield
                params(i, 5) = Struct
            Next
            Dim curveModule = New AdxYieldCurveModule
            Dim termStructure As Array = curveModule.AdTermStructure(params, "RM:YC ZCTYPE:RATE IM:CUBX ND:DIS", Nothing)
            Dim result As New List(Of SwapPointDescription)
            For i = termStructure.GetLowerBound(0) To termStructure.GetUpperBound(0)
                Dim dur = (Utils.FromExcelSerialDate(termStructure.GetValue(i, 1)) - _theDate).TotalDays / 365.0
                Dim yld = termStructure.GetValue(i, 2)
                If dur > 0 And yld > 0 Then
                    Dim ric = Descrs.Where(Function(pair) Math.Abs(pair.Value.Duration - dur) < 0.00001).Select(Function(pair) pair.Value).ToList()
                    If ric.Any() Then result.Add(New SwapPointDescription(ric.First.RIC) With {.Yield = yld, .Duration = dur, .YieldAtDate = ric.First.YieldAtDate})
                End If
            Next
            Return result
        End Function

        Public Overrides Function BootstrappingEnabled() As Boolean
            Return True
        End Function

        Public Overrides Function GetSnapshot() As List(Of Tuple(Of String, String, Double?, Double))
            Return Descrs.Values.Select(Function(elem) New Tuple(Of String, String, Double?, Double)(elem.RIC, String.Format("{0:N}Y", GetDuration(elem.RIC)), elem.Yield, elem.Duration)).ToList()
        End Function

        '' OVERRIDEN METHODS
        Public Overrides Function GetFitModes() As EstimationModel()
            Return {EstimationModel.DefaultModel}
        End Function

        Public Overrides Sub SetFitMode(ByVal mode As EstimationModel)
        End Sub

        Public Overrides Function GetFitMode() As EstimationModel
            Return EstimationModel.DefaultModel
        End Function

        Public Overrides Function GetBrokers() As String()
            Return Brokers
        End Function

        Public Overrides Sub SetBroker(ByVal b As String)
            _broker = b
            Subscribe()
        End Sub

        Public Overrides Function GetBroker() As String
            Return _broker
        End Function

        Public Overrides Function GetQuotes() As String()
            Return PossibleQuotes
        End Function

        Public Overrides Sub SetQuote(ByVal b As String)
            _quote = b
            Subscribe()
        End Sub

        Public Overrides Function GetQuote() As String
            Return _quote
        End Function

        Public Overrides Sub SetDate(ByVal theDate As Date)
            _theDate = theDate
            Subscribe()
        End Sub

        Public Overrides Function GetDate() As Date
            Return _theDate
        End Function

        Protected Overrides Sub Recalculate(ByRef list As List(Of SwapPointDescription))
            Logger.Trace("Recalculate()")
            list.ForEach(Sub(elem) SpreadBmk.CalcAllSpreads(elem))
        End Sub

        Public Overrides Function GetName() As String
            Return _name
        End Function

        Public Overrides Function GetFullName() As String
            Dim dt = GetDate()
            Dim dateStr = IIf(dt <> DateTime.Today, String.Format("{0:dd/MM/yy}", dt), "Today")

            Dim broker = GetBroker()
            If broker.Trim().Length = 0 Then
                Return String.Format("{0} ({1}, {2})", Me.GetType().Name, GetQuote, dateStr)
            Else
                Return String.Format("{0} ({1}, {2} by {3})", Me.GetType().Name, GetQuote, dateStr, broker)
            End If
        End Function

        Public Overrides Function GetOuterColor() As Color
            Return Color.Firebrick
        End Function

        Public Overrides Function GetInnerColor() As Color
            Return Color.NavajoWhite
        End Function

        Public Overridable Function BenchmarkEnabled() As Boolean Implements IAssetSwapBenchmark.CanBeBenchmark
            Return True
        End Function

        Public Overridable ReadOnly Property FloatLegStructure() As String Implements IAssetSwapBenchmark.FloatLegStructure
            Get
                Return FloatLeg
            End Get
        End Property

        Public Overridable ReadOnly Property FloatingPointValue() As Double Implements IAssetSwapBenchmark.FloatingPointValue
            Get
                Return 0
            End Get
        End Property
    End Class

    Public NotInheritable Class RubCCS
        Inherits RubIRS
        Public Sub New(bmk As SpreadContainer)
            MyBase.New(bmk)
        End Sub

        Protected Overrides Property InstrumentName() As String = "RUUSAM3L"
        Protected Overrides Property AllowedTenors() As String() = {"1", "2", "3", "4", "5", "6", "7", "10", "15", "20"}
        Protected Overrides Property BaseInstrument As String = ""

        Protected Overrides Property BaseInstrumentPrice As Double
            Set(ByVal value As Double)
            End Set
            Get
                Return 0
            End Get
        End Property

        Public Overrides Function GetOuterColor() As Color
            Return Color.MidnightBlue
        End Function

        Public Overrides Function GetInnerColor() As Color
            Return Color.LightSteelBlue
        End Function
    End Class

    Public NotInheritable Class RubNDF
        Inherits RubIRS
        Public Sub New(bmk As SpreadContainer)
            MyBase.New(bmk)
        End Sub

        Protected Overrides Property InstrumentName() As String = "RUB"
        Protected Overrides Property AllowedTenors() As String() = {"1W", "2W", "1M", "2M", "3M", "6M", "9M", "1Y", "18M", "2Y", "3Y", "4Y", "5Y"}
        Protected Overrides Property Brokers() As String() = {"GFI", "TRDL", "ICAP", "R", ""}
        Protected Overrides Property BaseInstrument As String = ""

        Public Overrides Function GetOuterColor() As Color
            Return Color.OrangeRed
        End Function

        Public Overrides Function GetInnerColor() As Color
            Return Color.Orange
        End Function

        Public Overrides Function BootstrappingEnabled() As Boolean
            Return False
        End Function

        Public Overrides Function GetDuration(ByVal ric As String) As Double
            Dim match = Regex.Match(ric, String.Format("{0}(?<term>[0-9]+?[DWMY])ID=.*", InstrumentName))
            Dim term = match.Groups("term").Value
            Dim dateModule As New AdxDateModule
            Dim aDate As Array = dateModule.DfAddPeriod("RUS", GetDate(), term, "")
            Return dateModule.DfCountYears(GetDate(), Utils.FromExcelSerialDate(aDate.GetValue(1, 1)), "")
        End Function

        Public Overrides Function BenchmarkEnabled() As Boolean
            Return False
        End Function

        Protected Overrides Function GetRICs(ByVal broker As String) As List(Of String)
            Return AllowedTenors.Select(Function(item) String.Format("{0}{1}ID={2}", InstrumentName, item, broker)).ToList()
        End Function
    End Class
End Namespace