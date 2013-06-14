Namespace Tools.Elements
    Public Class Ansamble
        Private Shared ReadOnly Identities As New HashSet(Of Long)
        Public Event Ordinate As Action(Of IOrdinate)

        Public Shared Sub ReleaseID(ByVal id As Long)
            Identities.Remove(id)
        End Sub

        Public Shared Function GenerateID() As Long
            Dim rnd = New Random()
            Dim num As Integer
            Do
                num = CLng(Math.Round((89.9999 * rnd.NextDouble() + 10) * 10000))
            Loop While Identities.Contains(num)
            Identities.Add(num)
            Return num
        End Function

        Private _xSource As XSource
        Public Property XSource() As XSource
            Get
                Return _xSource
            End Get
            Set(ByVal value As XSource)
                _xSource = value
                Replot()
            End Set
        End Property

        Private _ySource As OrdinateBase = Yield
        Public Property YSource() As OrdinateBase
            Get
                Return _ySource
            End Get
            Set(ByVal value As OrdinateBase)
                _ySource = value
                RaiseEvent Ordinate(value)
                Replot()
            End Set
        End Property

        Private WithEvents _items As New GroupContainer
        Public ReadOnly Property Items As GroupContainer
            Get
                Return _items
            End Get
        End Property

        Private WithEvents _swapCurves As New SwapCurveContainer
        Public ReadOnly Property SwapCurves() As SwapCurveContainer
            Get
                Return _swapCurves
            End Get
        End Property

        Default Public ReadOnly Property Data(ByVal id As Long) As IChangeable
            Get
                Return If(_items.Exists(id), Items(id), _swapCurves(id))
            End Get
        End Property

        Private WithEvents _benchmarks As New BenchmarkContainer(Me)

        Public ReadOnly Property Benchmarks() As BenchmarkContainer
            Get
                Return _benchmarks
            End Get
        End Property

        Public Sub Replot()
            For Each item In AllItems()
                item.RecalculateTotal()
            Next
        End Sub

        Public Sub Cleanup()
            Items.Cleanup()
            SwapCurves.Cleanup()
        End Sub

        Private Sub Benchmarks_ClearBmk(obj As IOrdinate) Handles _benchmarks.ClearBmk
            FreezeEvents()
            For Each item In AllCurves()
                item.ClearSpread(obj)
            Next
            If YSource = obj Then YSource = Yield
            UnfreezeEvents()
        End Sub

        Private Sub Benchmarks_NewBmk(obj As IOrdinate) Handles _benchmarks.NewBmk
            FreezeEvents()
            For Each item In AllCurves()
                item.SetSpread(obj)
            Next
            UnfreezeEvents()
        End Sub

        Private Sub UnfreezeEvents()
            For Each item In AllItems()
                item.UnfreezeEvents()
            Next
        End Sub

        Private Sub FreezeEvents()
            For Each item In AllItems()
                item.FreezeEvents()
            Next
        End Sub

        Private Sub UnfreezeEventsQuiet()
            For Each item In AllItems()
                item.UnfreezeEventsQuiet()
            Next
        End Sub

        Private Function AllCurves() As List(Of ICurve)
            Dim res = (From item In Items Where TypeOf item.Value Is ICurve Select item.Value).Cast(Of ICurve).ToList()
            res.AddRange(_swapCurves.Cast(Of ICurve))
            Return res
        End Function

        Private Function AllItems() As List(Of IChangeable)
            Dim res = (From item In Items Select item.Value).Cast(Of IChangeable).ToList()
            res.AddRange(_swapCurves.Cast(Of IChangeable))
            Return res
        End Function

        Private Sub GroupCleared(obj As Group) Handles _items.Cleared
            If Not TypeOf obj Is ICurve Then Return
            If _benchmarks.HasCurve(obj) Then _benchmarks.Clear(obj.Identity)
        End Sub

        Private Sub SwapCurveCleared(obj As SwapCurve) Handles _swapCurves.Cleared
            If _benchmarks.HasCurve(obj) Then _benchmarks.Clear(obj.Identity)
        End Sub

        ' сюда можно попасть двумя путями - либо когда пересчитали ВСЕ в ответ на одну из глобальных команд,
        ' или когда пересчитали только одну сущность в ответ на новые данные.
        ' но потом сюда посыплются сообщения о пересчете от тех, кого мы Recalculate в цикле. 
        ' однако, мы заморозили события.
        ' Тем не менее, когда разморозим, опять пойдет волна.
        ' Может быть, тогда нужна неуведомляющаяя разморозка?
        Private Sub SomethingRecalculated(obj As IChangeable) Handles _items.Recalculated, _swapCurves.Recalculated
            If Not TypeOf obj Is ICurve Then Return ' group was recalculated; not interesting
            Dim crv = CType(obj, ICurve)
            If _benchmarks.HasCurve(crv) Then
                Dim ord = _benchmarks.GetOrdinate(crv)
                FreezeEvents()
                For Each item In AllItems()
                    item.Recalculate(ord)
                Next
                UnfreezeEventsQuiet()
            End If
        End Sub

    End Class
End Namespace