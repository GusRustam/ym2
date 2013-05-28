Namespace Tools.Elements
    Public Class SwapCurveContainer
        Implements IEnumerable(Of SwapCurve)

        Public Event Cleared As Action(Of SwapCurve)

        Private ReadOnly _items As New Dictionary(Of Long, SwapCurve)
        Default Public ReadOnly Property Data(ByVal id As Long) As SwapCurve
            Get
                Return _items(id)
            End Get
        End Property

        Public Function IEnumerable_GetEnumerator() As IEnumerator(Of SwapCurve) Implements IEnumerable(Of SwapCurve).GetEnumerator
            Return _items.Values.GetEnumerator()
        End Function

        Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return _items.Values.GetEnumerator()
        End Function

        Public Function Exists(ByVal id As Long) As Boolean
            Return _items.Keys.Contains(id)
        End Function

        Public Sub Cleanup()
            While _items.Any
                _items.First.Value.Cleanup()
            End While
        End Sub

        Public Sub Start()
            For Each kvp In _items
                kvp.Value.Subscribe()
            Next
        End Sub

        Public Sub Add(ByVal group As SwapCurve)
            _items.Add(group.Identity, group)
            AddHandler group.Cleared, Sub()
                                          _items.Remove(group.Identity)
                                          RaiseEvent Cleared(group)
                                      End Sub
        End Sub

        Public Sub Remove(ByVal id As Long)
            _items.Remove(id)
        End Sub

    End Class
End Namespace