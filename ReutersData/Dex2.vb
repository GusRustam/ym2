Imports System.Diagnostics.Contracts
Imports Dex2Lib
Imports NLog

Public Structure Dex2Field
    Public Field As String
    Public FieldNum As Integer
    Public ColumnName As String
    Public ItsDate As Boolean
    Public ItsBool As Boolean
    Public ItsNum As Boolean

    Public Sub New(ByVal field As String, ByVal columnName As String, Optional ByVal itsDate As Boolean = False, Optional ByVal itsBool As Boolean = False, Optional ByVal itsNum As Boolean = False)
        Me.Field = field
        Me.ColumnName = columnName
        Me.ItsDate = itsDate
        Me.ItsBool = itsBool
        Me.ItsNum = itsNum
    End Sub

    Public Sub New(ByVal fieldNum As Integer, ByVal columnName As String, Optional ByVal itsDate As Boolean = False, Optional ByVal itsBool As Boolean = False, Optional ByVal itsNum As Boolean = False)
        Me.FieldNum = fieldNum
        Me.ColumnName = columnName
        Me.ItsDate = itsDate
        Me.ItsBool = itsBool
        Me.ItsNum = itsNum
    End Sub
End Structure

Public Class Dex2Query
    Private ReadOnly _fields As List(Of Dex2Field)
    Private ReadOnly _dispMode As String
    Private ReadOnly _reqMode As String

    Sub New(ByVal fields As List(Of Dex2Field), Optional ByVal dispMode As String = "", Optional ByVal reqMode As String = "")
        Contract.Requires(fields IsNot Nothing AndAlso fields.Any())
        _fields = fields
        _dispMode = dispMode
        _reqMode = reqMode
    End Sub

    Public ReadOnly Property FieldList As String
        Get
            Return (From field In _fields Where field.Field <> "" Select field.Field).Aggregate(Function(str, field) str + ", " + field)
        End Get
    End Property

    Public ReadOnly Property ColumnList As List(Of String)
        Get
            Return _fields.Select(Function(field) field.ColumnName).ToList()
        End Get
    End Property

    Public ReadOnly Property DispMode As String
        Get
            Return _dispMode
        End Get
    End Property

    Public ReadOnly Property ReqMode As String
        Get
            Return _reqMode
        End Get
    End Property

    Public Function IsBool(ByVal colName As String) As Boolean
        Contract.Requires(_fields.Any(Function(field) field.ColumnName = colName))
        Return _fields.First(Function(field) field.ColumnName = colName).ItsBool
    End Function

    Public Function IsNum(ByVal colName As String) As Boolean
        Contract.Requires(_fields.Any(Function(field) field.ColumnName = colName))
        Return _fields.First(Function(field) field.ColumnName = colName).ItsNum
    End Function

    Public Function IsDate(ByVal colName As String) As Boolean
        Contract.Requires(_fields.Any(Function(field) field.ColumnName = colName))
        Return _fields.First(Function(field) field.ColumnName = colName).ItsDate
    End Function
End Class

Public Class Dex2
    Private _rData As RData
    Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(Dex2))
    Private _columns As List(Of String)

    Public Event Metadata As Action(Of LinkedList(Of Dictionary(Of String, Object)))
    Public Event Failure As Action(Of Exception)

    Public Sub Load(ByVal rics As List(Of String), ByVal what As Dex2Query)
        Logger.Trace("Load({0})", rics.Count())
        Contract.Requires(rics.Count > 0 And what IsNot Nothing)
        _columns = what.ColumnList
        Try
            Dim dex2 As Dex2Mgr = Eikon.Sdk.CreateDex2Mgr()
            Dim cookie = dex2.Initialize()
            _rData = dex2.CreateRData(cookie)
            _rData.InstrumentIDList = rics.Aggregate(Function(str, item) str + ", " + item)
        Catch ex As Exception
            Logger.ErrorException("Failed to init Dex2 ", ex)
            Logger.Error("Exception = {0}", ex)
            RaiseEvent Failure(ex)
            Exit Sub
        End Try

        _rData.FieldList = what.FieldList
        _rData.DisplayParam = what.DispMode
        _rData.RequestParam = what.ReqMode
        _rData.Subscribe(False)

        AddHandler _rData.OnUpdate, AddressOf ImportData
    End Sub

    Private Sub ImportData(ByVal datastatus As DEX2_DataStatus, ByVal [error] As Object)
        Logger.Trace("ImportData({0})", datastatus)
        Try
            If datastatus = DEX2_DataStatus.DE_DS_FULL Then
                Dim res As New LinkedList(Of Dictionary(Of String, Object))
                Dim data = _rData.Data
                For i = data.GetLowerBound(0) To data.GetUpperBound(0)
                    res.AddLast(New Dictionary(Of String, Object)())

                    For j = 0 To _columns.Count - 1
                        Dim field = _columns(j)
                        Dim elem = data.GetValue(i, j)
                        res.Last.Value.Add(field, elem)
                    Next
                Next
                RemoveHandler _rData.OnUpdate, AddressOf ImportData
                RaiseEvent Metadata(res)
            ElseIf datastatus <> DEX2_DataStatus.DE_DS_PARTIAL Then
                Logger.Error("Error is {0}", [error].ToString())
                RaiseEvent Failure(New Exception([error].ToString()))
            End If
        Catch ex As Exception
            Logger.ErrorException("Failed to import data", ex)
            Logger.Error("Error is {0} and exception = {0}", [error].ToString(), ex.ToString())
            RaiseEvent Failure(ex)
        End Try

    End Sub
End Class