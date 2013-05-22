Imports System.Drawing
Imports AdfinXRtLib
Imports YieldMap.Tools.Elements
Imports ReutersData
Imports YieldMap.Tools
Imports YieldMap.Tools.Estimation
Imports NLog

Namespace Curves
    Public MustInherit Class SwapCurve
        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(SwapCurve))

#Region "Interface"
        '' ============ BOOTSTRAPPING ============
        Public MustOverride Function Bootstrap(ByVal data As List(Of SwapPointDescription)) As List(Of SwapPointDescription)
        Public MustOverride Function BootstrappingEnabled() As Boolean
        Public MustOverride Function IsBootstrapped() As Boolean
        Public MustOverride Sub SetBootstrapped(ByVal flag As Boolean)

        '' ============ FITTING ============
        Public MustOverride Function GetFitModes() As EstimationModel()
        Public MustOverride Sub SetFitMode(ByVal mode As EstimationModel)
        Public MustOverride Function GetFitMode() As EstimationModel

        '' ============ BROKERS ============
        Public MustOverride Function GetBrokers() As String()
        Public MustOverride Sub SetBroker(ByVal b As String)
        Public MustOverride Function GetBroker() As String

        '' ============ QUOTE SOURCES ============
        Public MustOverride Function GetQuotes() As String()
        Public MustOverride Sub SetQuote(ByVal b As String)
        Public MustOverride Function GetQuote() As String

        '' ============ TODAY AND HISTORICAL DATA ============
        Public MustOverride Sub SetDate(ByVal theDate As Date)
        Public MustOverride Function GetDate() As Date

        '' ============ ELEMENTS DESCRIPTIONS ============
        Public MustOverride Function GetDuration(ByVal ric As String) As Double

        '' ============ COLORS ============
        Public MustOverride Function GetOuterColor() As Color
        Public MustOverride Function GetInnerColor() As Color

        '' ============ NAMES ============
        Public MustOverride Function GetName() As String
        Public MustOverride Function GetFullName() As String

        '' ============ DESCRIPTIONS ============
        Public MustOverride Function GetSnapshot() As List(Of Tuple(Of String, String, Double?, Double))

        '' ============ DATA LOADING ============
        Protected MustOverride Sub StartRealTime()
        Protected MustOverride Sub LoadHistory()
        Protected MustOverride Sub OnHistoricalData(ByVal ric As String, ByVal status As LoaderStatus, ByVal hstatus As HistoryStatus, ByVal data As Dictionary(Of Date, HistoricalItem))

        '' ============ ITEMS ============
        Public MustOverride Function GetOriginalRICs() As List(Of String)
        Public MustOverride Function GetCurrentRICs() As List(Of String)
        Public MustOverride Function RemoveItem(ByVal ric As String) As Boolean
        Public MustOverride Function RemoveItems(ByVal ric As List(Of String)) As Boolean
        Public MustOverride Sub AddItems(ByVal rics As List(Of String))

        Protected MustOverride Function GetRICs(ByVal broker As String) As List(Of String)

        Public MustOverride Sub RecalculateByType(ByVal type As YSource)
        Protected MustOverride Sub Recalculate(ByRef list As List(Of SwapPointDescription))

        Public Sub CleanupByType(ByVal type As YSource)
            Dim rics = Descrs.Keys.ToList()
            rics.ForEach(Sub(ric) SpreadBmk.CleanupSpread(Descrs(ric), type))
        End Sub

#End Region

        Protected Overridable Property BaseInstrumentPrice As Double

        Public ReadOnly Property SpreadBmk As SpreadContainer
            Get
                Return _spreadBmk
            End Get
        End Property

        Protected ReadOnly Descrs As New Dictionary(Of String, SwapPointDescription)
        Private ReadOnly _spreadBmk As SpreadContainer

        Public Sub New(ByVal bmk As SpreadContainer)
            _spreadBmk = bmk
        End Sub

        Public Function GetCurveData(Optional ByVal raw As Boolean = False) As List(Of SwapPointDescription)
            Logger.Trace("GetCurveData({0}, raw = {1})", GetFullName(), raw)
            Try
                Dim list = Descrs.Values.Where(Function(elem) elem.Yield.HasValue).ToList()
                list.Sort()
                If IsBootstrapped() Then list = Bootstrap(list)
                If Not raw Then Recalculate(list)
                Return list
            Catch ex As Exception
                Logger.WarnException("Failed to obtain data", ex)
                Logger.Warn("Exception = {0}", ex.ToString())
                NotifyFaulted(Me, ex)
                Return Nothing
            End Try
        End Function

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If obj Is Nothing Then Return False
            If Not TypeOf obj Is SwapCurve Then Return False
            Return GetName() = CType(obj, SwapCurve).GetName()
        End Function

        Public Function ToArray() As Array
            Dim list = Descrs.Values.Where(Function(elem) elem.Yield.HasValue).ToList() ' todo BOOTSTRAPPED DATA IF BOOTSTRAPPED
            list.Sort()
            Dim len = list.Count - 1
            Dim res(0 To len, 1) As Object
            For i = 0 To len
                res(i, 0) = DateTime.Today.AddDays(TimeSpan.FromDays(list(i).Duration * 365).TotalDays)
                res(i, 1) = list(i).Yield
            Next
            Return res
        End Function

        Public Function ToFittedArray() As Array
            Dim data As Array = ToArray()
            Dim mode = GetFitMode()
            If mode = EstimationModel.LinInterp Then
                Return data
            Else
                Dim points = GetCurveData(True)
                Dim estimator = New Estimator(mode)

                Dim xyPoints = estimator.Approximate(XY.ConvertToXY(points, YSource.Yield))
                Dim len = xyPoints.Count() - 1
                Dim res(0 To len, 1) As Object
                For i = 0 To xyPoints.Count() - 1
                    res(i, 0) = DateTime.Today.AddDays(TimeSpan.FromDays(xyPoints(i).X * 365).Days)
                    res(i, 1) = xyPoints(i).Y
                Next
                Return res
            End If
        End Function

#Region "Loading data"
        '' LOAD SEPARATE RIC (HISTORICAL)
        Protected Sub DoLoadRIC(ByVal ric As String, ByVal fields As String, ByVal aDate As Date)
            Logger.Debug("DoLoadRIC({0})", ric)

            Dim hst As History = New History()
            AddHandler hst.HistoricalData, AddressOf OnHistoricalData
            hst.StartTask(ric, fields, aDate.AddDays(-3), aDate)
            If hst.Finished Then Return
        End Sub

        '' LOADING DATA
        Public Overridable Sub Subscribe()
            Logger.Debug("Subscirbe({0})", GetName())

            Cleanup()
            GetRICs(GetBroker()).ForEach(Sub(ric As String) Descrs.Add(ric, New SwapPointDescription(ric)))

            If GetDate() = Date.Today Then
                StartRealTime()
            Else
                LoadHistory()
            End If
        End Sub

        '' CLEANUP
        Public Overridable Sub Cleanup()
            RaiseEvent Cleared(Me)
            Descrs.Clear()
        End Sub
#End Region

#Region "Events"
        Public Event Cleared As Action(Of SwapCurve)
        Public Event Updated As Action(Of SwapCurve)
        Public Event Recalculated As Action(Of SwapCurve)
        Public Event Faulted As Action(Of SwapCurve, Exception)

        Protected Sub NotifyUpdated(ByVal theCurve As SwapCurve)
            RaiseEvent Updated(theCurve)
        End Sub

        Protected Sub NotifyFaulted(ByVal theCurve As SwapCurve, ByVal theEx As Exception)
            RaiseEvent Faulted(theCurve, theEx)
        End Sub

        Protected Sub NotifyRecalculated(ByVal curve As SwapCurve)
            RaiseEvent Recalculated(curve)
        End Sub
#End Region

    End Class
End Namespace