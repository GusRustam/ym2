Namespace Tools.Elements
    Public Class BenchmarkContainer
        Implements IEnumerable(Of ICurve)
        Public Event NewBmk As Action(Of YSource)
        Public Event ClearBmk As Action(Of YSource)

        Private ReadOnly _items As New Dictionary(Of YSource, ICurve)

        Default Public ReadOnly Property Items(ByVal wut As YSource) As ICurve
            Get
                Return _items(wut)
            End Get
        End Property

        Public Sub Put(ByVal src As YSource, ByVal crv As ICurve)
            _items(src) = crv
            RaiseEvent NewBmk(src)
        End Sub

        Public Function Has(ByVal src As YSource) As Boolean
            Return _items.ContainsKey(src)
        End Function

        Public Sub Clear(ByVal src As YSource)
            _items.Remove(src)
            RaiseEvent ClearBmk(src)
        End Sub

        Public Function Any() As Boolean
            Return _items.Any
        End Function

        Public Function IEnumerable_GetEnumerator() As IEnumerator(Of ICurve) Implements IEnumerable(Of ICurve).GetEnumerator
            Return _items.Values.GetEnumerator()
        End Function

        Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return _items.Values.GetEnumerator()
        End Function
    End Class
End NameSpace