Imports System.Drawing
Imports AdfinXRtLib
Imports YieldMap.Tools.Estimation
Imports YieldMap.Forms.ChartForm
Imports YieldMap.Tools.History
Imports NLog

Namespace Curves
    Public Interface ICurve
        Function GetName() As String
        Function GetFullName() As String
        Function GetSnapshot() As List(Of Tuple(Of String, Double, Double))
        Function ToArray() As Array

        Event Cleared As Action(Of ICurve)
        Event Updated As Action(Of ICurve, List(Of XY))
        Event Recalculated As Action(Of ICurve, List(Of XY))
    End Interface

    Public MustInherit Class SwapCurve
        Implements ICurve

        Private ReadOnly _clearedHandler As Action(Of ICurve)
        Private ReadOnly _updatedHandler As Action(Of ICurve, List(Of XY))
        Private ReadOnly _recalculatedHandler As Action(Of ICurve, List(Of XY))

        Private Shared ReadOnly Logger As Logger = Commons.GetLogger(GetType(SwapCurve))

#Region "Interface"
        Public MustOverride Function GetBrokers() As String()
        Public MustOverride Sub SetBroker(ByVal b As String)
        Public MustOverride Function GetBroker() As String

        Public MustOverride Function GetQuotes() As String()
        Public MustOverride Sub SetQuote(ByVal b As String)
        Public MustOverride Function GetQuote() As String

        Public MustOverride Sub SetDate(ByVal theDate As Date)
        Public MustOverride Function GetDate() As Date

        Public MustOverride Function GetDuration(ric As String) As Double

        Public MustOverride Function GetOuterColor() As Color
        Public MustOverride Function GetInnerColor() As Color

        Public MustOverride Function CalculateSpread(ByVal data As List(Of YieldDuration)) As List(Of YieldDuration)

        Public MustOverride Function GetName() As String Implements ICurve.GetName
        Public MustOverride Function GetFullName() As String Implements ICurve.GetFullName

        Protected MustOverride Sub StartRealTime()
        Protected MustOverride Sub LoadHistory()
        Protected MustOverride Sub OnHistoricalData(ByVal hst As HistoryLoadManager, ByVal ric As String, ByVal datastatus As RT_DataStatus, ByVal data As Dictionary(Of Date, HistoricalItem))
#End Region

#Region "Non-overridables (internal use only)"
        '' BENCHMARKING
        Private _benchmark As SwapCurve
        Protected ReadOnly Property Benchmark As SwapCurve
            Get
                Return _benchmark
            End Get
        End Property

        Private _mode As SpreadMode = SpreadMode.Yield

        Public ReadOnly Property BmkSpreadMode As SpreadMode
            Get
                Return _mode
            End Get
        End Property

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If obj Is Nothing Then Return False
            If Not TypeOf obj Is SwapCurve Then Return False
            Dim crv = CType(obj, SwapCurve)
            Return GetName() = crv.GetName()
        End Function

        Public Sub SetModeAndBenchmark(ByVal newMode As SpreadMode, ByVal curve As SwapCurve)
            _mode = newMode
            _benchmark = curve

            NotifyRecalculated(Me)
        End Sub


        '' CURVE DESCRIPTION
        Protected ReadOnly CurveData As New List(Of YieldDuration) ' RIC -> Yield / Duration
        Protected Overridable Property BaseInstrumentPrice As Double

#End Region

#Region "Protected non-overridable tools"
        '' INSERT NEW DATA INTO CURVE
        Protected Sub AddCurveItem(ByVal yieldDuration As YieldDuration)
            CurveData.RemoveAll(Function(item) item.RIC = yieldDuration.RIC)
            CurveData.Add(yieldDuration)
            CurveData.Sort()
        End Sub

        '' LOAD SEPARATE RIC (HISTORICAL)
        Protected Sub DoLoadRIC(ric As String, fields As List(Of String), aDate As Date)
            Logger.Debug("DoLoadRIC({0})", ric)

            If _hstLoaders.Keys.Contains(ric) Then
                Logger.Warn("RIC {0} history is already loading", ric)
                Return
            End If

            Dim descr = New HistoryTaskDescr() With {
                .Item = ric,
                .StartDate = aDate.AddDays(-3),
                .EndDate = aDate,
                .Fields = fields,
                .Frequency = "D",
                .InterestingFields = fields
            }

            Dim hst As HistoryLoadManager = New HistoryLoadManager(New AdxRtHistory)
            hst.StartTask(descr, AddressOf OnHistoricalData)
            If hst.Finished Then Return
            If hst.Success Then
                Logger.Info("Successfully added task for {0}", descr.Item)
                _hstLoaders.Add(ric, hst)
            End If
        End Sub

        Protected Sub StopLoading(ByVal ric As String)
            _hstLoaders(ric).StopTask()
            _hstLoaders.Remove(ric)
        End Sub

        Protected Sub StopHistory()
            _hstLoaders.Keys.ToList.ForEach(
                Sub(key)
                    Logger.Info("Was still waiting for history on {0}", key)
                    _hstLoaders(key).StopTask()
                    RemoveHandler _hstLoaders(key).NewData, AddressOf OnHistoricalData
                End Sub)
            _hstLoaders.Clear()
        End Sub
#End Region

#Region "Events"
        Public Event Cleared As Action(Of ICurve) Implements ICurve.Cleared
        Public Event Updated As Action(Of ICurve, List(Of XY)) Implements ICurve.Updated
        Protected Sub NotifyUpdated(theCurve As ICurve)
            RaiseEvent Updated(theCurve, GetCurveData())
        End Sub
        Public Event Recalculated As Action(Of ICurve, List(Of XY)) Implements ICurve.Recalculated
        Private Sub NotifyRecalculated(ByVal curve As ICurve)
            RaiseEvent Recalculated(curve, GetCurveData())
        End Sub
#End Region

#Region "Public overridable tools"
        Private ReadOnly _hstLoaders As New Dictionary(Of String, HistoryLoadManager)
        Public Sub New(ByVal clearedHandler As Action(Of ICurve),
                       ByVal updatedHandler As Action(Of ICurve, List(Of XY)),
                       ByVal recalculatedHandler As Action(Of ICurve, List(Of XY)))

            _clearedHandler = clearedHandler
            _updatedHandler = updatedHandler
            _recalculatedHandler = recalculatedHandler

            If _clearedHandler IsNot Nothing Then AddHandler Cleared, _clearedHandler
            If _updatedHandler IsNot Nothing Then AddHandler Updated, _updatedHandler
            If _recalculatedHandler IsNot Nothing Then AddHandler Recalculated, _recalculatedHandler
        End Sub

        Public Overridable Function ToArray() As Array Implements ICurve.ToArray
            Dim len = CurveData.Count - 1
            Dim res(0 To len, 1) As Object
            For i = 0 To len
                res(i, 0) = DateTime.Today.AddDays(TimeSpan.FromDays(CurveData(i).Duration * 365).TotalDays)
                res(i, 1) = CurveData(i).Yield
            Next
            Return res
        End Function

        Public Overridable Function GetCurveData() As List(Of XY)
            Return XY.ConvertToXY(CurveData, BmkSpreadMode)
        End Function

        Public Function GetSnapshot() As List(Of Tuple(Of String, Double, Double)) Implements ICurve.GetSnapshot
            Return CurveData.Select(Function(elem) New Tuple(Of String, Double, Double)(elem.RIC, elem.Yield, elem.Duration)).ToList()
        End Function

        '' LOADING DATA
        Public Overridable Sub Subscribe()
            Logger.Debug("Subscirbe({0})", GetName())
            StopLoaders()
            If GetDate() = Date.Today Then
                StartRealTime()
            Else
                LoadHistory()
            End If
        End Sub

        '' CLEANUP
        Public Overridable Sub Cleanup()
            StopLoaders()

            RaiseEvent Cleared(Me)
            CurveData.Clear()

            If _clearedHandler IsNot Nothing Then RemoveHandler Cleared, _clearedHandler
            If _updatedHandler IsNot Nothing Then RemoveHandler Updated, _updatedHandler
            If _recalculatedHandler IsNot Nothing Then RemoveHandler Recalculated, _recalculatedHandler
        End Sub

        Protected Overridable Sub StopLoaders()
            _hstLoaders.Keys.ToList().ForEach(Sub(key) _hstLoaders(key).StopTask())
            _hstLoaders.Clear()
            CurveData.Clear()
        End Sub
#End Region
    End Class
End Namespace