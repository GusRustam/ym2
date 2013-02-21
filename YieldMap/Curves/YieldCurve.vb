Imports System
Imports System.Drawing
Imports AdfinXRtLib
Imports DbManager.Bonds
Imports DbManager
Imports Uitls
Imports YieldMap.Tools
Imports YieldMap.Tools.History
Imports YieldMap.Tools.Lists
Imports YieldMap.Tools.Estimation
Imports NLog
Imports AdfinXAnalyticsFunctions

Namespace Curves
    Class YieldCurve
        Inherits SwapCurve

        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(YieldCurve))

        Private WithEvents _quoteLoader As New ListLoadManager

        Private ReadOnly _name As String
        Private ReadOnly _fullname As String
        Private ReadOnly _color As Color
        Private ReadOnly _meta As New Dictionary(Of String, BondDescription)

        Private _date As Date
        Private _quote As String
        Private _bootstrapped As Boolean

        Private ReadOnly _fieldNames As Dictionary(Of QuoteSource, String)
        Private _estimationModel As EstimationModel = EstimationModel.DefaultModel
        Private ReadOnly _originalRics As List(Of String)

        Public Sub New(ByVal fullname As String, ByVal rics As List(Of String), ByVal clr As String, ByVal fieldNames As Dictionary(Of QuoteSource, String), ByVal bmk As SpreadContainer)
            MyBase.New(bmk)
            _name = Guid.NewGuid.ToString()
            _fullname = fullname
            _color = Color.FromName(clr)
            _originalRics = New List(Of String)
            For Each ric In rics
                _originalRics.Add(ric)
            Next

            rics.ForEach(
                Sub(ric)
                    Dim meta = BondsData.Instance.GetBondInfo(ric)
                    If meta IsNot Nothing Then
                        _meta.Add(ric, meta)
                    Else
                        Logger.Error("No description for ric {0} found", ric)
                    End If
                End Sub)

            _fieldNames = fieldNames
            _date = Date.Today
            ' TODO: REASONABLE QUOTE SELECTION!!!! EACH ENTITY MUST HAVE EXPLICITLY SET DEFAULT FIELD!!!!!!!
            If fieldNames(QuoteSource.Last).Trim() <> "" Then
                _quote = fieldNames(QuoteSource.Last)
            ElseIf fieldNames(QuoteSource.Bid).Trim() <> "" Then
                _quote = fieldNames(QuoteSource.Bid)
            Else
                _quote = fieldNames(QuoteSource.Ask)
            End If
        End Sub

        Protected Overrides Sub StartRealTime()
            If Not _quoteLoader.StartNewTask(New ListTaskDescr() With {
                                                .Name = _name,
                                                .Items = Descrs.Keys.ToList(),
                                                .Fields = {_quote}.ToList()
                                            }) Then
                Logger.Error("Failed to start loading bonds data")
                Throw New Exception("Failed to start loading realtime bonds data")
            End If
        End Sub

        Protected Overrides Sub LoadHistory()
            Logger.Debug("LoadHistory")
            Descrs.Keys.ToList().ForEach(
                Sub(ric)
                    Try
                        DoLoadRIC(ric, {"DATE", _fieldNames(QuoteSource.Hist)}.ToList, _date)
                    Catch ex As Exception
                        Logger.ErrorException("Failed to start loading bonds data", ex)
                        Logger.Error("Exception = {0}", ex.ToString())
                        Throw New Exception("Failed to start loading historical bonds data")
                    End Try
                End Sub)
        End Sub

        Public Overrides Function GetBrokers() As String()
            Return New String() {}
        End Function

        Public Overrides Sub SetBroker(ByVal b As String)
        End Sub

        Public Overrides Function GetBroker() As String
            Return ""
        End Function

        Public Overrides Function GetQuotes() As String()
            Return {_fieldNames(QuoteSource.Bid), _fieldNames(QuoteSource.Ask), _fieldNames(QuoteSource.Last)}
        End Function

        Public Overrides Sub SetQuote(ByVal b As String)
            _quote = b
            Subscribe()
        End Sub

        Public Overrides Function GetQuote() As String
            Return _quote
        End Function

        Public Overrides Function GetOuterColor() As Color
            Return _color
        End Function

        Public Overrides Function GetInnerColor() As Color
            Return Color.White
        End Function

        Public Overrides Sub SetDate(ByVal theDate As Date)
            Dim tmp = _date
            _date = theDate
            If tmp <> _date Then Subscribe()
        End Sub

        Public Overrides Function GetDate() As Date
            Return _date
        End Function

        Public Overrides Function GetDuration(ByVal ric As String) As Double
            Return Descrs(ric).Duration
        End Function

        Public Overrides Sub RecalculateByType(ByVal type As SpreadType)
            Logger.Trace("RecalculateByType({0})", type)
            Dim rics = Descrs.Keys.ToList()
            If SpreadBmk.Benchmarks.ContainsKey(type) AndAlso SpreadBmk.Benchmarks(type).GetName() = GetName() Then
                rics.ForEach(Sub(ric) SpreadBmk.CleanupSpread(Descrs(ric), type))
            Else
                rics.ForEach(Sub(ric) SpreadBmk.CalcAllSpreads(Descrs(ric), _meta(ric), type))
            End If

            If SpreadBmk.CurrentType = type Then NotifyRecalculated(Me)
        End Sub

        Protected Overrides Sub Recalculate(ByRef list As List(Of SwapPointDescription))
            Logger.Trace("Recalculate()")
            list.ForEach(Sub(elem) SpreadBmk.CalcAllSpreads(elem, _meta(elem.RIC)))
        End Sub

        Protected Overrides Sub OnHistoricalData(ByVal hst As HistoryLoadManager, ByVal ric As String, ByVal datastatus As RT_DataStatus, ByVal data As Dictionary(Of Date, HistoricalItem))
            Logger.Debug("OnHistoricalData({0})", ric)
            Try
                If data IsNot Nothing Then
                    Dim maxdate = data.Where(Function(kvp) kvp.Value.SomePrice()).Select(Function(kvp) kvp.Key).Max
                    Dim elem = data(maxdate)
                    Dim price As Double
                    Try
                        price = elem.GetPropByName(_fieldNames(QuoteSource.Hist))
                    Catch ex As Exception
                        price = 0
                    End Try
                    If price > 0 Then
                        Try
                            Descrs(ric).Price = price
                            CalculateYields(maxdate, _meta(ric), Descrs(ric))
                            NotifyUpdated(Me)
                        Catch ex As Exception
                            Logger.WarnException("Failed to parse instrument " + ric, ex)
                            Logger.Warn("Exception = {0}", ex.ToString())
                        End Try
                    Else
                        Logger.Warn("No data!")
                    End If
                Else
                    Logger.Warn("No data!")
                End If
            Catch ex As Exception
                Logger.InfoException("", ex)
                Logger.Info("Exception = {0}", ex.ToString())
            End Try
        End Sub

        Public Overrides Function GetOriginalRICs() As List(Of String)
            Dim res As New List(Of String)
            For Each ric In _originalRics
                res.Add(ric)
            Next
            Return res
        End Function

        Public Overrides Function GetCurrentRICs() As List(Of String)
            Return _meta.Keys.ToList()
        End Function

        Public Overrides Function RemoveItem(ByVal ric As String) As Boolean
            _meta.Remove(ric)
            Descrs.Remove(ric)
            NotifyUpdated(Me)
            Return True
        End Function

        Public Overrides Function RemoveItems(ByVal rics As List(Of String)) As Boolean
            For Each ric In rics
                _meta.Remove(ric)
                Descrs.Remove(ric)
            Next
            NotifyUpdated(Me)
            Return True
        End Function

        Public Overrides Sub AddItems(ByVal rics As List(Of String))
            rics.ForEach(
                Sub(ric)
                    Dim meta = BondsData.Instance.GetBondInfo(ric)
                    If meta IsNot Nothing Then
                        _meta.Add(ric, meta)
                    Else
                        Logger.Error("No description for ric {0} found", ric)
                    End If
                End Sub)
            Cleanup()
            NotifyUpdated(Me)
            Subscribe()
        End Sub

        Public Overrides Sub Cleanup()
            _quoteLoader.DiscardTask(_name)
            MyBase.Cleanup()
        End Sub


        Public Overrides Function GetName() As String
            Return _name
        End Function

        Public Overrides Function GetFullName() As String
            Dim dt = GetDate()
            Dim dateStr = IIf(dt <> DateTime.Today, String.Format("{0:dd/MM/yy}", dt), "Today")

            Return String.Format("{0} ({1}, {2})", _fullname, _quote, dateStr)
        End Function

        Public Overrides Function GetSnapshot() As List(Of Tuple(Of String, String, Double?, Double))
            Return Descrs.Values.Select(Function(elem) New Tuple(Of String, String, Double?, Double)(elem.RIC, _meta(elem.RIC).ShortName, elem.Yield, elem.Duration)).ToList()
        End Function

        Private Sub OnRealTimeData(ByVal data As Dictionary(Of String, Dictionary(Of String, Dictionary(Of String, Double)))) Handles _quoteLoader.OnNewData
            Logger.Debug("OnRealTimeData")
            Dim errorList As New List(Of Exception)
            For Each listAndRFV As KeyValuePair(Of String, Dictionary(Of String, Dictionary(Of String, Double))) In data
                Try
                    Dim list = listAndRFV.Key
                    Dim rfv = listAndRFV.Value

                    If list = _name Then
                        Logger.Info(_name)
                        For Each ricAndFieldValue As KeyValuePair(Of String, Dictionary(Of String, Double)) In rfv
                            Dim ric = ricAndFieldValue.Key
                            Logger.Trace("Got RIC {0}", ric)

                            If ricAndFieldValue.Value.Keys.Contains(_quote) AndAlso CDbl(ricAndFieldValue.Value(_quote)) > 0 Then
                                Try
                                    Dim price = CDbl(ricAndFieldValue.Value(_quote))
                                    Descrs(ric).Price = price
                                    CalculateYields(_date, _meta(ric), Descrs(ric))
                                    NotifyUpdated(Me)
                                Catch ex As Exception
                                    Logger.WarnException("Failed to parse realtime data", ex)
                                    Logger.Warn("Exception = {0}", ex.ToString())
                                End Try
                            Else
                                Logger.Warn("Empty data for ric {0};  will try to load history", ric)
                                DoLoadRIC(ric, {"DATE", _fieldNames(QuoteSource.Hist)}.ToList, _date.AddDays(-1))
                            End If

#If DEBUG Then
                            Dim fieldValue = ricAndFieldValue.Value
                            For Each fv As KeyValuePair(Of String, Double) In fieldValue
                                Logger.Trace("  {0} -> {1}", fv.Key, fv.Value)
                            Next
#End If
                        Next
                    Else
                        Logger.Info("Will not handle unknown list {0}", list)
                    End If
                Catch ex As Exception
                    Logger.ErrorException("Failed to load realtime data", ex)
                    Logger.Error("Exception = {0}", ex)
                    errorList.Add(New Exception(String.Format("Failed to load realtime data for {0}", GetFullName())))
                End Try
            Next
            errorList.ForEach(Sub(err) NotifyFaulted(Me, err))
        End Sub

        Public Overrides Function GetFitModes() As EstimationModel()
            Return EstimationModel.GetEnabledModels()
        End Function

        Public Overrides Sub SetFitMode(ByVal fitMode As EstimationModel)
            _estimationModel = fitMode ' EstimationModel.FromName(fitMode)
            NotifyUpdated(Me)
        End Sub

        Protected Overrides Function GetRICs(ByVal broker As String) As List(Of String)
            Return _meta.Keys.ToList()
        End Function

        Public Overrides Function GetFitMode() As EstimationModel
            Return _estimationModel
        End Function

        Public Overrides Function IsBootstrapped() As Boolean
            Return _bootstrapped
        End Function

        Public Overrides Sub SetBootstrapped(ByVal flag As Boolean)
            _bootstrapped = flag
            NotifyUpdated(Me)
        End Sub

        Public Overrides Function BootstrappingEnabled() As Boolean
            Return True
        End Function

        Public Overrides Function Bootstrap(ByVal data As List(Of SwapPointDescription)) As List(Of SwapPointDescription)
            Try
                data = data.Where(Function(elem) _meta(elem.RIC).IssueDate <= _date And _meta(elem.RIC).Maturity > _date).ToList()
                Dim params(0 To data.Count() - 1, 5) As Object
                For i = 0 To data.Count - 1
                    Dim meta = _meta(data(i).RIC)
                    params(i, 0) = "B"
                    params(i, 1) = _date
                    params(i, 2) = meta.Maturity
                    params(i, 3) = meta.GetCouponByDate(_date)
                    params(i, 4) = data(i).Price / 100.0
                    params(i, 5) = meta.PaymentStructure
                Next
                Dim curveModule = New AdxYieldCurveModule

                Dim termStructure As Array = curveModule.AdTermStructure(params, "RM:YC ZCTYPE:RATE IM:CUBX ND:DIS", Nothing)
                Dim result As New List(Of SwapPointDescription)
                For i = termStructure.GetLowerBound(0) To termStructure.GetUpperBound(0)
                    Dim matDate = Utils.FromExcelSerialDate(termStructure.GetValue(i, 1))
                    Dim dur = (matDate - _date).TotalDays / 365.0
                    Dim yld = termStructure.GetValue(i, 2)
                    If dur > 0 And yld > 0 Then
                        Dim ric = _meta.First(Function(elem) elem.Value.Maturity = matDate).Key
                        result.Add(New SwapPointDescription(ric) With {.Yield = yld, .Duration = dur})
                    End If
                Next
                Return result
            Catch ex As Exception
                Throw New Exception("Bootstrapping failed", ex)
            End Try
        End Function
    End Class
End Namespace