Imports System.Drawing
Imports ReutersData

Namespace Tools.Elements
    Public Interface IChangeable
        'Inherits IIdentifyable
        Event Cleared As Action
        Sub Cleanup()
        Sub Recalculate()
        Sub Subscribe()
        ReadOnly Property Name As String
    End Interface

    Public Interface ICurve
        ReadOnly Property CanBootstrap() As Boolean
        Property Bootstrapped() As Boolean
        Sub Bootstrap()
        Property CurveDate() As Date
    End Interface

    Public MustInherit Class SwapCurve
        Inherits Identifyable
        Implements ICurve
        Implements IChangeable

        Public Event Cleared As Action Implements IChangeable.Cleared
        Public Event Updated As Action(Of List(Of CurveItem))

        Public MustOverride Sub Recalculate() Implements IChangeable.Recalculate

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
        Public MustOverride Property CurveDate() As Date Implements ICurve.CurveDate

        '' ============ ELEMENTS DESCRIPTIONS ============
        Public MustOverride Function GetDuration(ByVal ric As String) As Double

        '' ============ COLORS ============
        Public MustOverride ReadOnly Property OuterColor() As Color
        Public MustOverride ReadOnly Property InnerColor() As Color

        '' ============ NAMES ============
        Public MustOverride ReadOnly Property Name() As String Implements IChangeable.Name

        '' ============ DESCRIPTIONS ============
        Public MustOverride Function GetSnapshot() As List(Of Tuple(Of String, String, Double?, Double))

        '' ============ CLEANUP ============
        Public MustOverride Sub Cleanup() Implements IChangeable.Cleanup

        '' ============ DATA LOADING ============
        Protected MustOverride Sub StartRealTime()
        Protected MustOverride Sub LoadHistory()
        Protected MustOverride Sub OnHistoricalData(ByVal ric As String, ByVal data As Dictionary(Of Date, HistoricalItem), ByVal rawData As Dictionary(Of DateTime, RawHistoricalItem))

        '' LOADING DATA
        Public MustOverride Sub Subscribe() Implements IChangeable.Subscribe

        Protected Sub NotifyCleanup()
            RaiseEvent Cleared()
        End Sub
    End Class
End Namespace