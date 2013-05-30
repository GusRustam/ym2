Namespace Tools.Elements
    Public Class BenchmarkContainer
        Implements IEnumerable(Of ICurve)
        Public Event NewBmk As Action(Of IOrdinate)
        Public Event ClearBmk As Action(Of IOrdinate)

        Private ReadOnly _items As New Dictionary(Of IOrdinate, ICurve)

        Default Public ReadOnly Property Items(ByVal wut As IOrdinate) As ICurve
            Get
                Return _items(wut)
            End Get
        End Property

        Public Sub Put(ByVal src As IOrdinate, ByVal crv As ICurve)
            If src = Yield Then Throw New InvalidOperationException()
            ' todo ???????? ?? ??????????? ???????????
            _items(src) = crv
            RaiseEvent NewBmk(src)
        End Sub

        Public Function Has(ByVal src As IOrdinate) As Boolean
            Return _items.ContainsKey(src)
        End Function

        Public Sub Clear(ByVal src As IOrdinate)
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