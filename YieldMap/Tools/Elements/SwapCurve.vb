Imports System.Drawing

Namespace Tools.Elements
    Public Interface IChangeable
        Event Cleared As Action
        ReadOnly Property Name As String
        Sub Cleanup()
        Sub Recalculate()
        Sub Subscribe()
    End Interface

    Public Interface ICurve
        ReadOnly Property CanBootstrap() As Boolean
        Property Bootstrapped() As Boolean
        Property CurveDate() As Date
        Sub Bootstrap()
    End Interface

    Public MustInherit Class SwapCurve
        Inherits Identifyable
        Implements ICurve
        Implements IChangeable

        Public Event Cleared As Action Implements IChangeable.Cleared
        Public Event Updated As Action(Of List(Of CurveItem))

        '' ============ ICHANGEABLE INTERFACE ============
        Public MustOverride Sub Recalculate() Implements IChangeable.Recalculate
        Public MustOverride ReadOnly Property Name() As String Implements IChangeable.Name
        Public MustOverride Sub Cleanup() Implements IChangeable.Cleanup
        Public MustOverride Sub Subscribe() Implements IChangeable.Subscribe

        '' ============ ICURVE INTERFACE ============
        Public MustOverride Property CurveDate() As Date Implements ICurve.CurveDate
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

        '' ============ COLORS ============
        Public MustOverride ReadOnly Property OuterColor() As Color
        Public MustOverride ReadOnly Property InnerColor() As Color

        '' ============ DESCRIPTIONS ============
        Public MustOverride Function GetSnapshot() As List(Of Tuple(Of String, String, Double?, Double))

        Protected Sub NotifyCleanup()
            RaiseEvent Cleared()
        End Sub

        Protected Sub NotifyUpdated(ByVal data As List(Of CurveItem))
            RaiseEvent Updated(data)
        End Sub
    End Class
End Namespace