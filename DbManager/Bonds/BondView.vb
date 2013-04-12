Namespace Bonds
    Public Class BondView
        Private ReadOnly _items As New List(Of BondDescription)
        Private ReadOnly _interpreter As New FilterInterpreter(Of BondDescription)
        Private ReadOnly _sorter As New Sorter(Of BondDescription)

        Public Sub SetFilter(ByVal flt As String)
            ' todo exception messages and exception handling
            Dim parser As New FilterParser
            _interpreter.SetGrammar(parser.SetFilter(flt))
        End Sub

        Public Sub SetSort(ByVal fieldName As String, ByVal direction As SortDirection)
            ' todo exception messages and exception handling
            _sorter.SetSort(fieldName, direction)
        End Sub

        Public Sub New()
            Dim rics = (From row In BondsLoader.Instance.GetBondsTable() Select row.ric).ToList()
            _items.AddRange(BondsData.Instance.GetBondInfo(rics))
        End Sub

        Public ReadOnly Property Items() As List(Of BondDescription)
            Get
                Dim list = New List(Of BondDescription)(From elem In _items Where _interpreter.Allows(elem))
                list.Sort(_sorter)
                Return list
            End Get
        End Property
    End Class
End Namespace