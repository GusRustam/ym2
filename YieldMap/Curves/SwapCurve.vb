Imports System.Drawing
Imports AdfinXRtLib
Imports YieldMap.Tools.Estimation
Imports YieldMap.Forms.ChartForm
Imports YieldMap.Tools.History
Imports NLog

Namespace Curves
    Public Interface ICurve
        'Function PointSpread(ByVal yld As Double, ByVal duration As Double) As Double?
        Function GetName() As String
        Function GetFullName() As String
        Function ToArray() As Array

        Event Updated As Action(Of ICurve, List(Of XY), Boolean)
    End Interface

    Public MustInherit Class SwapCurve
        Implements ICurve
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

            NotifyUpdated(Me, Equals(_benchmark))
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
                .InterestingFields = {"BID", "ASK", "CLOSE"}.ToList()
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
        Public Event Updated As Action(Of ICurve, List(Of XY), Boolean) Implements ICurve.Updated
        Protected Sub NotifyUpdated(theCurve As ICurve, Optional first As Boolean = False)
            RaiseEvent Updated(theCurve, GetCurveData(), first)
        End Sub
#End Region

#Region "Public overridable tools"
        Private ReadOnly _hstLoaders As New Dictionary(Of String, HistoryLoadManager)
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

        '' LOADING DATA
        Public Overridable Sub Subscribe()
            Logger.Debug("Subscirbe({0})", GetName())
            Cleanup()
            If GetDate() = Date.Today Then
                StartRealTime()
            Else
                LoadHistory()
            End If
        End Sub

        '' CLEANUP
        Public Overridable Sub Cleanup()
            _hstLoaders.Keys.ToList().ForEach(Sub(key) _hstLoaders(key).StopTask())
            _hstLoaders.Clear()
            CurveData.Clear()
        End Sub
#End Region
    End Class
End Namespace