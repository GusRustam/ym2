Imports System.Reflection

Namespace Bonds
    Public Class BondViewFilterException
        Inherits Exception

        Public Sub New()
        End Sub

        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(ByVal message As String, ByVal innerException As Exception)
            MyBase.New(message, innerException)
        End Sub
    End Class

    Public Class FilterNameAttribute
        Inherits Attribute
        Private ReadOnly _name As String
        Private ReadOnly _pattern As String
        Private ReadOnly _operation As String

        Public Sub New(ByVal name As String, Optional ByVal operation As String = "=", Optional ByVal pattern As String = "^'(?<val>.+?)'$")
            _name = name
            _operation = operation
            _pattern = pattern
        End Sub

        Public ReadOnly Property Name As String
            Get
                Return _name
            End Get
        End Property

        Public ReadOnly Property Pattern As String
            Get
                Return _pattern
            End Get
        End Property
    End Class

    Friend Class Filter
        Private _sortItem As String
        Private _filterString As String
        Private _sortAsc As Boolean
        Private _comparer As IComparer(Of BondDescription)

        Public Sub SetSort(ByVal sortItem As String, ByVal sortAsc As Boolean)
            _sortItem = sortItem
            _sortAsc = _sortAsc
            _comparer = New BondComparer(_sortItem, _sortAsc)
        End Sub

        Public Function Allows(ByVal bondRow As BondDescription) As Boolean
            If _filterString = "" Then Return True
            Return True
        End Function

        Public Function GetComparer() As IComparer(Of BondDescription)
            If _sortItem = "" Then Return Nothing
            Return _comparer
        End Function

        Private Class BondComparer
            Implements IComparer(Of BondDescription)

            Private ReadOnly _field As String
            Private ReadOnly _fieldToCompare As FieldInfo
            Private ReadOnly _asc As Boolean

            Public Sub New(ByVal field As String, ByVal asc As Boolean)
                _field = field
                _asc = asc
                Dim fields = (From fld In GetType(BondDescription).GetFields(BindingFlags.Instance)
                                Let attrs = fld.GetCustomAttributes(GetType(FilterNameAttribute), False)
                                Where attrs.Any(Function(elem) CType(elem, FilterNameAttribute).Name = _field)
                                Select fld).ToList()
                If Not fields.Any() Then Throw New BondViewFilterException(String.Format("Couldn't find fields with name {0} to compare", _field))
                If fields.Count() > 1 Then Throw New BondViewFilterException(String.Format("There's more than one field fields with name {0} to compare", _field))
                _fieldToCompare = fields.First()
            End Sub

            Public Function Compare(ByVal x As BondDescription, ByVal y As BondDescription) As Integer Implements IComparer(Of BondDescription).Compare
                Dim xVal = _fieldToCompare.GetValue(x), yVal = _fieldToCompare.GetValue(y)
                Return If(_asc, 1, -1) * Comparer(Of Object).Default.Compare(xVal, yVal)
            End Function
        End Class
    End Class

    Public Class BondView
        Private ReadOnly _items As New List(Of BondDescription)
        Private ReadOnly _filter As New Filter

        Public Sub SetFilter(ByVal flt As String)
            ' todo _filter.SetFilter(flt)
        End Sub

        Public Sub SetSort(ByVal sortItem As String, ByVal sortAsc As Boolean)
            _filter.SetSort(sortItem, sortAsc)
        End Sub

        Public Sub New()
            _items.AddRange(From row In BondsLoader.Instance.GetBondsTable()
                            Let item = BondsData.Instance.GetBondInfo(row.ric)
                            Where _filter Is Nothing OrElse _filter.Allows(item)
                            Select item)

            _items.Sort(_filter.GetComparer())
        End Sub

        Public ReadOnly Property Items() As List(Of BondDescription)
            Get
                Return New List(Of BondDescription)(_items)
            End Get
        End Property
    End Class
End Namespace