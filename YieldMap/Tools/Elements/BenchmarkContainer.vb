Namespace Tools.Elements
    Public Class BenchmarkContainer
        Implements IEnumerable(Of ICurve)
        Public Event NewBmk As Action(Of IOrdinate)
        Public Event ClearBmk As Action(Of IOrdinate)

        Private ReadOnly _items As New Dictionary(Of IOrdinate, ICurve)
        Private ReadOnly _ansamble As Ansamble

        Default Public ReadOnly Property Items(ByVal wut As IOrdinate) As ICurve
            Get
                Return _items(wut)
            End Get
        End Property

        Public ReadOnly Property Keys() As IEnumerable(Of IOrdinate)
            Get
                Return _items.Keys
            End Get
        End Property

        Public Sub Put(ByVal src As IOrdinate, ByVal crv As ICurve)
            ' there can't be benchmark for Yield
            If src = Yield Then Throw New InvalidOperationException()

            ' saving current Y Source
            Dim _tempYSrc = _ansamble.YSource

            ' removing old benchmark (this will clear all calculated spreads)
            If _items.ContainsKey(src) Then Clear(src)

            ' saving new benchmark
            _items(src) = crv

            ' recalculating spreds
            RaiseEvent NewBmk(src)

            ' restoring Y Source mode
            If _tempYSrc <> _ansamble.YSource Then _ansamble.YSource = _tempYSrc
        End Sub

        Public Function HasOrd(ByVal src As IOrdinate) As Boolean
            Return _items.ContainsKey(src)
        End Function

        Public Function HasCurve(ByVal crv As ICurve) As Boolean
            Return _items.ContainsValue(crv)
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

        Public Function GetOrdinate(ByVal crv As ICurve) As IOrdinate
            For Each kvp In From pair In _items Where pair.Value.Equals(crv)
                Return kvp.Key
            Next
            Return Nothing
        End Function

        Public Sub New(ByVal ansamble As Ansamble)
            _ansamble = ansamble
        End Sub
    End Class
End NameSpace