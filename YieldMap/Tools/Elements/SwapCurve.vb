Imports System.Drawing

Namespace Tools.Elements
    Public Interface INamed
        ReadOnly Property Name As String
    End Interface

    Public Interface IChangeable
        Inherits INamed
        Event Cleared As Action
        Sub Cleanup()
        Sub Recalculate()
        Sub Recalculate(ByVal ord As IOrdinate)
        Sub Subscribe()
        Sub FreezeEvents()
        Sub UnfreezeEvents()
        Sub UnfreezeEventsQuiet()
        Sub RecalculateTotal()
    End Interface

    Public Interface IAswBenchmark
        Function CanBeBenchmark() As Boolean

        ReadOnly Property FloatLegStructure() As String
        ReadOnly Property FloatingPointValue() As Double
    End Interface

    Public Interface ICurve
        Inherits INamed
        ReadOnly Property CanBootstrap() As Boolean
        Property Bootstrapped() As Boolean
        Property CurveDate() As Date
        Sub Bootstrap()

        Sub ClearSpread(ByVal ySource As OrdinateBase)
        Sub SetSpread(ByVal ySource As OrdinateBase)
        Function RateArray() As Array
    End Interface

    Public MustInherit Class SwapCurve
        Inherits Identifyable
        Implements ICurve, IChangeable

        Public Event Cleared As Action Implements IChangeable.Cleared
        Public Event Updated As Action(Of List(Of CurveItem))
        Public Event UpdatedSpread As Action(Of List(Of CurveItem), IOrdinate)

        Private ReadOnly _ansamble As Ansamble

        Public ReadOnly Property Ansamble() As Ansamble
            Get
                Return _ansamble
            End Get
        End Property

        '' ============ ICHANGEABLE INTERFACE ============
        Public MustOverride Sub Recalculate() Implements IChangeable.Recalculate
        Public MustOverride Sub RecalculateTotal() Implements IChangeable.RecalculateTotal
        Public MustOverride Sub Recalculate(ByVal ord As IOrdinate) Implements IChangeable.Recalculate
        Public MustOverride ReadOnly Property Name() As String Implements INamed.Name
        Public MustOverride Sub Cleanup() Implements IChangeable.Cleanup
        Public MustOverride Sub Subscribe() Implements IChangeable.Subscribe

        Private _eventsFrozen As Boolean

        Sub New(ByVal ansamble As Ansamble)
            _ansamble = ansamble
        End Sub

        Public Sub FreezeEvents() Implements IChangeable.FreezeEvents
            _eventsFrozen = True
        End Sub

        Public Sub UnfreezeEvents() Implements IChangeable.UnfreezeEvents
            _eventsFrozen = False
            Recalculate()
        End Sub

        Public Sub UnfreezeEventsQuiet() Implements IChangeable.UnfreezeEventsQuiet
            _eventsFrozen = False
            If Ansamble.YSource <> Yield Then Recalculate(_ansamble.YSource)
        End Sub

        '' ============ ICURVE INTERFACE ============
        Public MustOverride Property CurveDate() As Date Implements ICurve.CurveDate
        Public MustOverride ReadOnly Property CanBootstrap() As Boolean Implements ICurve.CanBootstrap
        Public MustOverride Property Bootstrapped() As Boolean Implements ICurve.Bootstrapped
        Public MustOverride Sub Bootstrap() Implements ICurve.Bootstrap
        Public MustOverride Sub ClearSpread(ByVal ySource As OrdinateBase) Implements ICurve.ClearSpread
        Public MustOverride Sub SetSpread(ByVal ySource As OrdinateBase) Implements ICurve.SetSpread
        Public MustOverride Function RateArray() As Array Implements ICurve.RateArray

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
            If Not _eventsFrozen Then RaiseEvent Updated(data)
        End Sub

        Protected Sub NotifyUpdatedSpread(ByVal data As List(Of CurveItem), ByVal ord As IOrdinate)
            If Not _eventsFrozen Then RaiseEvent UpdatedSpread(data, ord)
        End Sub

    End Class
End Namespace