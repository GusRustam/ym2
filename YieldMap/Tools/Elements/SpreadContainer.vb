Imports DbManager.Bonds

Namespace Tools.Elements
    Public Class SpreadContainer
        Public Benchmarks As New Dictionary(Of YSource, SwapCurve)

        Public Event TypeSelected As Action(Of YSource, YSource)
        Public Event BenchmarkRemoved As Action(Of YSource)
        Public Event BenchmarkUpdated As Action(Of YSource)

        Private _currentType As YSource = YSource.Yield
        Public Property CurrentType As YSource
            Get
                Return _currentType
            End Get
            Set(ByVal value As YSource)
                Dim oldType = _currentType
                _currentType = value
                RaiseEvent TypeSelected(_currentType, oldType)
            End Set
        End Property

        'Public Sub CalcAllSpreads(ByRef descr As BasePointDescription, Optional ByVal data As BondDescription = Nothing, Optional ByVal type As YSource = Nothing)
        '    If type IsNot Nothing Then
        '        If data IsNot Nothing Then
        '            If type = YSource.ZSpread AndAlso Benchmarks.ContainsKey(YSource.ZSpread) Then
        '                CalcZSprd(Benchmarks(YSource.ZSpread).ToArray(), descr, data)
        '            End If
        '            If type = YSource.ASWSpread AndAlso Benchmarks.ContainsKey(YSource.ASWSpread) Then
        '                Dim aswSpreadMainCurve = Benchmarks(YSource.ASWSpread)
        '                Dim bmk = CType(aswSpreadMainCurve, IAssetSwapBenchmark)
        '                CalcASWSprd(aswSpreadMainCurve.ToArray(), bmk.FloatLegStructure, bmk.FloatingPointValue, descr, data)
        '            End If
        '        End If
        '        If type = YSource.PointSpread AndAlso Benchmarks.ContainsKey(YSource.PointSpread) Then
        '            CalcPntSprd(Benchmarks(YSource.PointSpread).ToFittedArray(), descr)
        '        End If
        '    Else
        '        If data IsNot Nothing Then
        '            If Benchmarks.ContainsKey(YSource.ZSpread) Then
        '                CalcZSprd(Benchmarks(YSource.ZSpread).ToArray(), descr, data)
        '            End If
        '            If Benchmarks.ContainsKey(YSource.ASWSpread) Then
        '                Dim aswSpreadMainCurve = Benchmarks(YSource.ASWSpread)
        '                Dim bmk = CType(aswSpreadMainCurve, IAssetSwapBenchmark)
        '                CalcASWSprd(aswSpreadMainCurve.ToArray(), bmk.FloatLegStructure, bmk.FloatingPointValue, descr, data)
        '            End If
        '        End If
        '        If Benchmarks.ContainsKey(YSource.PointSpread) Then
        '            CalcPntSprd(Benchmarks(YSource.PointSpread).ToFittedArray(), descr)
        '        End If
        '    End If
        'End Sub

        Public Function GetActualQuote(ByVal calc As BasePointDescription) As Double?
            Dim fieldName = _currentType.ToString()
            If fieldName = "Yield" Then
                Return calc.GetYield()
            Else
                Dim value As Object
                Dim fieldInfo = GetType(BondPointDescription).GetField(fieldName)
                If fieldInfo IsNot Nothing Then
                    value = fieldInfo.GetValue(calc)
                Else
                    Dim propertyInfo = GetType(BasePointDescription).GetProperty(fieldName)
                    value = propertyInfo.GetValue(calc, Nothing)
                End If
                If value IsNot Nothing Then Return CDbl(value)
            End If
            Return Nothing
        End Function


        'Public Sub OnCurveRemoved(ByVal curve As SwapCurve)
        '    Dim types = (From keyValue In Benchmarks Where keyValue.Value.GetName() = curve.GetName() Select keyValue.Key).ToList()
        '    types.ForEach(Sub(type)
        '        Benchmarks.Remove(type)
        '        RaiseEvent BenchmarkRemoved(type)
        '                     End Sub)
        '    If types.Contains(CurrentType) Then CurrentType = YSource.Yield
        'End Sub

        'Public Sub UpdateCurve(ByVal curveName As String)
        '    Dim types = (From keyValue In Benchmarks Where keyValue.Value.Identity = curveName Select keyValue.Key).ToList()
        '    types.ForEach(Sub(type) RaiseEvent BenchmarkUpdated(type))
        'End Sub

        Public Sub AddType(ByVal type As YSource, ByVal curve As SwapCurve)
            If Benchmarks.ContainsKey(type) Then
                Benchmarks.Remove(type)
            End If
            Benchmarks.Add(type, curve)
            RaiseEvent BenchmarkUpdated(type)
        End Sub

        Public Sub RemoveType(ByVal type As YSource)
            If Benchmarks.ContainsKey(type) Then
                RaiseEvent BenchmarkRemoved(type)
                Benchmarks.Remove(type)
            End If
            If CurrentType = type Then CurrentType = YSource.Yield
        End Sub

        Public Sub CleanupSpread(ByRef descr As BasePointDescription, ByVal type As YSource)
            If type = YSource.ZSpread Then
                descr.ZSpread = Nothing
            ElseIf type = YSource.ASWSpread Then
                descr.ASWSpread = Nothing
            ElseIf type = YSource.PointSpread Then
                descr.PointSpread = Nothing
            End If
        End Sub

    End Class
End NameSpace