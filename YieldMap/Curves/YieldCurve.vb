Imports System
Imports System.Drawing
Imports AdfinXRtLib
Imports YieldMap.Tools
Imports YieldMap.Forms.ChartForm
Imports YieldMap.Tools.History
Imports YieldMap.Tools.Lists
Imports YieldMap.Tools.Estimation
Imports NLog
Imports AdfinXAnalyticsFunctions

Namespace Curves
    Interface IFittable
        Function GetFitModes() As EstimationModel()
        Sub SetFitMode(ByVal mode As String)
        Function GetFitMode() As EstimationModel

        Function Estimate(points As List(Of YieldDuration)) As List(Of YieldDuration)
    End Interface

    Interface IBootstrappable
        Function Bootstrap(ByVal data As List(Of YieldDuration)) As List(Of YieldDuration)
        Function IsBootstrapped() As Boolean
        Function BootstrappingEnabled() As Boolean
        Sub SetBootstrapped(ByVal flag As Boolean)
    End Interface

    Class YieldCurve
        Inherits SwapCurve
        Implements IFittable, IBootstrappable

        Private _estimator As New Estimator

        Private Shared ReadOnly Logger As Logger = Commons.GetLogger(GetType(YieldCurve))

        Private WithEvents _quoteLoader As New ListLoadManager

        Private ReadOnly _name As String
        Private ReadOnly _fullname As String
        Private ReadOnly _color As Color
        Private ReadOnly _descrs As New Dictionary(Of String, BondPointDescr)
        Private _date As Date
        Private _quote As String
        Private _bootstrapped As Boolean

        Private ReadOnly _fieldNames As Dictionary(Of QuoteSource, String)
        Private _estimationModel As EstimationModel = EstimationModel.DefaultModel

        Public Sub New(ByVal name As String, ByVal fullname As String, ByVal rics As List(Of String), ByVal clr As String, fieldNames As Dictionary(Of QuoteSource, String))
            _name = name
            _fullname = fullname
            _color = Color.FromName(clr)
            rics.ForEach(Sub(ric) _descrs.Add(ric, GetBondDescr(ric)))
            _fieldNames = fieldNames
            _date = Date.Today
            _quote = fieldNames(QuoteSource.Last)
        End Sub


        Protected Overrides Sub StartRealTime()
            If Not _quoteLoader.StartNewTask(New ListTaskDescr() With {
                                                .Name = _name,
                                                .Items = _descrs.Keys.ToList(),
                                                .Fields = {_quote}.ToList()
                                            }) Then
                Logger.Error("Failed to start loading bonds data")
                Throw New InvalidOperationException("Failed to start loading bondss data")
            End If
        End Sub

        Protected Overrides Sub LoadHistory()
            Logger.Debug("LoadHistory")
            _descrs.Keys.ToList().ForEach(Sub(ric) DoLoadRIC(ric, {_fieldNames(QuoteSource.Hist)}.ToList, _date))
        End Sub

        Public Overrides Function GetBrokers() As String()
            Return New String() {}
        End Function

        Public Overrides Sub SetBroker(ByVal b As String)
        End Sub

        Public Overrides Function GetBroker() As String
            Return Nothing
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

        Public Overrides Sub Cleanup()
            _quoteLoader.DiscardTask(_name)
            MyBase.Cleanup()
        End Sub

        Public Overrides Function GetDuration(ByVal ric As String) As Double
            If Not CurveData.Any(Function(elem) elem.RIC = ric) Then
                Return _descrs(ric).Duration
            Else
                Return CurveData.First(Function(elem) elem.RIC = ric).Duration
            End If
        End Function

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
                            Dim yieldDur = CalcYield(price, maxdate, _descrs(ric))

                            Dim duration = yieldDur.Duration
                            Dim bestYield = yieldDur.Yld

                            Dim yieldDuration = New YieldDuration() With {
                                .Yield = bestYield.Yield,
                                .Duration = duration,
                                .RIC = ric,
                                .CalcPrice = price
                            }
                            AddCurveItem(yieldDuration)
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

            RemoveHandler hst.NewData, AddressOf OnHistoricalData
            StopLoading(ric)
        End Sub

        Public Overrides Function GetName() As String
            Return _name
        End Function

        Public Overrides Function GetFullName() As String
            Dim dt = GetDate()
            Dim dateStr = IIf(dt <> DateTime.Today, String.Format("{0:dd/MM/yy}", dt), "Today")

            Return String.Format("{0} ({1}, {2})", _fullname, _quote, dateStr)
        End Function

        Private Sub QuoteLoaderOnNewData(data As Dictionary(Of String, Dictionary(Of String, Dictionary(Of String, Double?)))) Handles _quoteLoader.OnNewData
            Logger.Debug("OnRealTimeData")
            For Each listAndRFV As KeyValuePair(Of String, Dictionary(Of String, Dictionary(Of String, Double?))) In data
                Dim list = listAndRFV.Key
                Dim rfv = listAndRFV.Value

                If list = _name Then
                    Logger.Info(_name)
                    For Each ricAndFieldValue As KeyValuePair(Of String, Dictionary(Of String, Double?)) In rfv
                        Dim ric = ricAndFieldValue.Key
                        Logger.Trace("Got RIC {0}", ric)

                        ' define yield curve elem
                        Dim yieldDuration = New YieldDuration() With {
                            .Duration = GetDuration(ric),
                            .RIC = ric
                        }

                        If ricAndFieldValue.Value.Keys.Contains(_quote) AndAlso CDbl(ricAndFieldValue.Value(_quote)) > 0 Then
                            Try
                                Dim price = CDbl(ricAndFieldValue.Value(_quote))
                                ' calculating new yield / duration

                                Dim yieldDur = CalcYield(price, _date, _descrs(ric))

                                Dim duration = yieldDur.Duration
                                Dim bestYield = yieldDur.Yld

                                With yieldDuration
                                    .Yield = bestYield.Yield
                                    .Duration = duration
                                    .CalcPrice = price
                                End With

                                AddCurveItem(yieldDuration)
                                NotifyUpdated(Me)
                            Catch ex As Exception
                                Logger.WarnException("Failed to parse realtime data", ex)
                                Logger.Warn("Exception = {0}", ex.ToString())
                            End Try
                        Else
                            Logger.Warn("Empty data for ric {0};  will try to load history", ric)
                            DoLoadRIC(ric, {_fieldNames(QuoteSource.Hist)}.ToList, _date.AddDays(-1))
                        End If

#If DEBUG Then
                        Dim fieldValue = ricAndFieldValue.Value
                        For Each fv As KeyValuePair(Of String, Double?) In fieldValue
                            Logger.Trace("  {0} -> {1}", fv.Key, fv.Value)
                        Next
#End If
                    Next
                Else
                    Logger.Info("Will not handle unknown list {0}", list)
                End If
            Next
        End Sub

        Public Function GetFitModes() As EstimationModel() Implements IFittable.GetFitModes
            Return EstimationModel.GetEnabledModels()
        End Function

        Public Sub SetFitMode(ByVal mode As String) Implements IFittable.SetFitMode
            _estimationModel = EstimationModel.FromName(mode)
            _estimator = New Estimator(_estimationModel)
            NotifyUpdated(Me)
        End Sub

        Public Overrides Function GetCurveData() As List(Of YieldDuration)
            Return _estimator.Approximate(If(_bootstrapped, Bootstrap(CurveData), CurveData))
        End Function

        Public Function GetFitMode() As EstimationModel Implements IFittable.GetFitMode
            Return _estimationModel
        End Function

        Public Function Estimate(points As List(Of YieldDuration)) As List(Of YieldDuration) Implements IFittable.Estimate
            Return _estimator.Approximate(points)
        End Function

        Public Function IsBootstrapped() As Boolean Implements IBootstrappable.IsBootstrapped
            Return _bootstrapped
        End Function

        Public Sub SetBootstrapeed(flag As Boolean) Implements IBootstrappable.SetBootstrapped
            _bootstrapped = flag
            NotifyUpdated(Me)
        End Sub

        Function BootstrappingEnabled() As Boolean Implements IBootstrappable.BootstrappingEnabled
            Return True
        End Function

        Public Function Bootstrap(ByVal data As List(Of YieldDuration)) As List(Of YieldDuration) Implements IBootstrappable.Bootstrap
            Dim params(0 To CurveData.Count() - 1, 5) As Object
            For i = 0 To CurveData.Count - 1
                Dim descr = _descrs(CurveData(i).RIC)
                params(i, 0) = "B"
                params(i, 1) = _date
                params(i, 2) = descr.Maturity
                params(i, 3) = descr.Coupon / 100.0            ' todo coupon @ date
                params(i, 4) = CurveData(i).CalcPrice / 100.0
                params(i, 5) = descr.PaymentStructure
            Next
            Dim curveModule = New AdxYieldCurveModule

            Dim termStructure As Array = curveModule.AdTermStructure(params, "RM:YC ZCTYPE:RATE IM:CUBX ND:DIS", Nothing)
            Dim result As New List(Of YieldDuration)
            For i = termStructure.GetLowerBound(0) To termStructure.GetUpperBound(0)
                Dim dur = (Commons.FromExcelSerialDate(termStructure.GetValue(i, 1)) - _date).TotalDays / 365.0
                Dim yld = termStructure.GetValue(i, 2)
                If dur > 0 And yld > 0 Then result.Add(New YieldDuration With {.Yield = yld, .Duration = dur})
            Next
            Return result
        End Function

    End Class
End Namespace

'todo forecasting and modeling this curve!!!!!! 
' load history and implement HJM !!! 
' or simple 3-factor model or BDT model or HW model