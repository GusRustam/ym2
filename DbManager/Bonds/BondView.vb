Imports System.Data.SqlClient
Imports NLog

Namespace Bonds
    Public Class BondView
        Private ReadOnly _items As New List(Of BondMetadata)
        Private ReadOnly _interpreter As New FilterInterpreter(Of BondMetadata)
        Private ReadOnly _sorter As New Sorter(Of BondMetadata)
        Private _fieldName As String
        Private _direction As SortDirection = SortDirection.None
        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(BondView))

        Public Sub SetFilter(ByVal flt As String)
            Try
                Dim parser As New FilterParser
                _interpreter.SetGrammar(parser.SetFilter(flt))
            Catch ex As Exception
                Logger.ErrorException(String.Format("Failed to set filter {0}", flt), ex)
                Logger.Error("Exception = {0}", ex.ToString())
            End Try
        End Sub

        Public Sub SetSort(ByVal fieldName As String, ByVal direction As SortDirection)
            Try
                _fieldName = fieldName
                _direction = direction
                _sorter.SetSort(fieldName, direction)
            Catch ex As Exception
                Logger.ErrorException(String.Format("Failed to set sorting field {0} direction {1}", fieldName, direction), ex)
                Logger.Error("Exception = {0}", ex.ToString())
            End Try
        End Sub

        Public Sub New()
            Dim rics = (From row In BondsLoader.Instance.GetBondsTable() Select row.ric).ToList()
            _items.AddRange(BondsData.Instance.GetBondInfo(rics))
        End Sub

        Public ReadOnly Property Items() As List(Of BondMetadata)
            Get
                Dim list = New List(Of BondMetadata)(From elem In _items Where _interpreter.Allows(elem))
                list.Sort(_sorter)
                Return list
            End Get
        End Property

        Public ReadOnly Property SortFieldName() As String
            Get
                Return _fieldName
            End Get
        End Property

        Public ReadOnly Property SortOrder() As SortOrder
            Get
                If _direction = SortDirection.Asc Then
                    Return SortOrder.Ascending
                ElseIf _direction = SortDirection.Desc Then
                    Return SortOrder.Descending
                Else
                    Return SortOrder.Unspecified
                End If
            End Get
        End Property

        Public Function Sorted() As Boolean
            Return _direction <> SortDirection.None
        End Function
    End Class
End Namespace