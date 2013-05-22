Imports DbManager.Bonds

Namespace Tools.Elements
    ''' <summary>
    ''' Source of chart X axis
    ''' </summary>
    Public Enum XSource
        Duration
        Maturity
    End Enum

    ''' <summary>
    ''' Points labeling mode
    ''' </summary>
    ''' <remarks>have to create custom labels</remarks>
    Public Enum LabelMode
        IssuerAndSeries
        IssuerCpnMat
        Description
        SeriesOnly
    End Enum

    ''' <summary>
    ''' This class stores information on which point was last MouseOvered on chart.
    ''' It is used as a tag for series on chart and has an event to notify which point is currently on selection
    ''' </summary>
    ''' <remarks></remarks>
    Friend Class BondSetSeries
        Public Name As String
        Public Event SelectedPointChanged As Action(Of String, Integer?)

        Public Color As Color
        Private _selectedPointIndex As Integer?

        Public Property SelectedPointIndex As Integer?
            Get
                Return _selectedPointIndex
            End Get
            Set(ByVal value As Integer?)
                _selectedPointIndex = value
                RaiseEvent SelectedPointChanged(Name, value)
            End Set
        End Property

        Public Sub ResetSelection()
            _selectedPointIndex = Nothing
        End Sub
    End Class

    ''' <summary>
    ''' Used as a tag for history point. 
    ''' Contains short info on bond for which the history was loaded and id of line on chart
    ''' todo add reference to base bond instead of Ric, Descr and Meta
    ''' todo kill history when bond is unloaded or hidden (this means I have to catch bond events)
    ''' </summary>
    ''' <remarks></remarks>
    Friend Structure HistoryPointTag
        Public Ric As String
        Public Descr As BondPointDescription
        Public Meta As BondDescription
        Public SeriesId As Guid
    End Structure

    ''' <summary>
    ''' Contains calculated point parameters
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class BasePointDescription
        Implements IComparable(Of BasePointDescription)

        Public Duration As Double
        Public Price As Double
        Public YieldAtDate As Date
        Public PointSpread As Double?
        Public ZSpread As Double?
        Public ASWSpread As Double?
        Public OASpread As Double?

        Public MustOverride Function GetYield() As Double?

        Public Function CompareTo(ByVal other As BasePointDescription) As Integer Implements IComparable(Of BasePointDescription).CompareTo
            If other IsNot Nothing Then
                If Duration < other.Duration Then
                    Return -1
                ElseIf Duration > other.Duration Then
                    Return 1
                Else
                    Return 0
                End If
            Else
                Return 0
            End If
        End Function
    End Class

    Public Class SwapPointDescription
        Inherits BasePointDescription

        Public Yield As Double?
        Public Overrides Function GetYield() As Double?
            Return Yield
        End Function

        Private ReadOnly _ric As String
        Public ReadOnly Property RIC As String
            Get
                Return _ric
            End Get
        End Property

        Public Sub New(ByVal ric As String)
            _ric = ric
        End Sub

        Public Overrides Function ToString() As String
            Return String.Format("{0} {1:P2}:{2:F2}", RIC, Yield / 100, Duration)
        End Function
    End Class

    Public Class BondPointDescription
        Inherits BasePointDescription

        Public ParentBond As Bond

        Public Yld As New YieldStructure
        Public Convexity As Double
        Public PVBP As Double

        Public BackColor As String
        Public MarkerStyle As String

        Public Overrides Function GetYield() As Double?
            Return Yld.Yield
        End Function
    End Class
End Namespace