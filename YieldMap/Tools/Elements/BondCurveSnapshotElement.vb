Imports System.ComponentModel

Namespace Tools.Elements
    ''' <summary>
    ''' Technical class to represent bond curve structure
    ''' </summary>
    ''' <remarks></remarks>
    Public Class BondCurveSnapshotElement
        Inherits CurveSnapshotElement
        Private ReadOnly _descr As String
        Private ReadOnly _price As Double
        Private ReadOnly _quote As String
        Private ReadOnly _yieldDate As Date

        Public Sub New(ByVal ric As String, ByVal descr As String, ByVal [yield] As Double, ByVal duration As Double, ByVal price As Double, ByVal quote As String, ByVal yieldDate As Date)
            MyBase.New(ric, duration, [yield])
            _descr = descr
            _price = price
            _quote = quote
            _yieldDate = yieldDate
        End Sub

        Public ReadOnly Property RIC() As String
            Get
                Return TheRIC
            End Get
        End Property

        Public ReadOnly Property Duration() As String
            Get
                Return String.Format("{0:F2}", Dur)
            End Get
        End Property

        Public ReadOnly Property Descr() As String
            Get
                Return _descr
            End Get
        End Property

        Public ReadOnly Property Yield() As String
            Get
                Return String.Format("{0:P2}", Yld)
            End Get
        End Property

        Public ReadOnly Property Price() As String
            Get
                Return String.Format("{0:F4}", _price)
            End Get
        End Property

        Public ReadOnly Property Quote() As String
            Get
                Return _quote
            End Get
        End Property

        <DisplayName("Yield date")>
        Public ReadOnly Property YieldDate() As String
            Get
                Return String.Format("{0:dd/MM/yyyy}", _yieldDate)
            End Get
        End Property

    End Class
End Namespace