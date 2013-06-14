Namespace Tools.Elements
    Public Interface IAswBenchmark
        Function CanBeBenchmark() As Boolean

        ReadOnly Property FloatLegStructure() As String
        ReadOnly Property FloatingPointValue() As Double
    End Interface
End NameSpace