Imports System.Drawing
Imports ReutersData
Imports NLog

Namespace Tools.Elements
    Public Interface IChangeable
        Sub Cleanup()
        Sub Recalculate()
    End Interface

    Public Interface ICurve
        ReadOnly Property CanBootstrap() As Boolean
        Property Bootstrapped() As Boolean
        Sub Bootstrap()
        Property [Date]() As Date
    End Interface

    Public MustInherit Class SwapCurve
        Inherits Identifyable
        Implements ICurve
        Implements IChangeable
        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(SwapCurve))

        Public Event Cleared As Action
        Public Event Updated As Action

        Public Sub Recalculate() Implements IChangeable.Recalculate
            RaiseEvent Updated()
        End Sub

        '' ============ BOOTSTRAPPING ============
        Public MustOverride ReadOnly Property CanBootstrap() As Boolean Implements ICurve.CanBootstrap
        Public MustOverride Property Bootstrapped() As Boolean Implements ICurve.Bootstrapped
        Public MustOverride Sub Bootstrap() Implements ICurve.Bootstrap

        '' ============ BROKERS ============
        Public MustOverride Function GetBrokers() As String()
        Public MustOverride Sub SetBroker(ByVal b As String)
        Public MustOverride Function GetBroker() As String

        '' ============ QUOTE SOURCES ============
        Public MustOverride Function GetQuotes() As String()
        Public MustOverride Sub SetQuote(ByVal b As String)
        Public MustOverride Function GetQuote() As String

        '' ============ TODAY AND HISTORICAL DATA ============
        Public MustOverride Property [Date]() As Date Implements ICurve.Date

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

        '' ============ CLEANUP ============
        Public MustOverride Sub Cleanup() Implements IChangeable.Cleanup

        '' ============ DATA LOADING ============
        Protected MustOverride Sub StartRealTime()
        Protected MustOverride Sub LoadHistory()
        Protected MustOverride Sub OnHistoricalData(ByVal ric As String, ByVal data As Dictionary(Of Date, HistoricalItem), ByVal rawData As Dictionary(Of DateTime, RawHistoricalItem))

        '' ============ ITEMS ============
        Protected MustOverride Function GetRICs(ByVal broker As String) As List(Of String)
        Protected Overridable Property BaseInstrumentPrice As Double
        Protected ReadOnly Descrs As New Dictionary(Of String, SwapPointDescription)

        '' LOAD SEPARATE RIC (HISTORICAL)
        Protected Sub DoLoadRIC(ByVal ric As String, ByVal fields As String, ByVal aDate As Date)
            Logger.Debug("DoLoadRIC({0})", ric)
            If ric = "" Then Return

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

            If [Date]() = Date.Today Then
                StartRealTime()
            Else
                LoadHistory()
            End If
        End Sub

        Protected Sub NotifyCleanup()
            RaiseEvent Cleared()
        End Sub
    End Class
End Namespace