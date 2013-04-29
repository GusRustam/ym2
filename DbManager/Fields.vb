﻿Imports System.ComponentModel
Imports NLog
Imports System.Reflection
Imports System.Xml

Public Class ConfingNameAttribute
    Inherits Attribute
    Private ReadOnly _xmlName As String

    Public Sub New(ByVal xmlName As String)
        _xmlName = xmlName
    End Sub

    Public ReadOnly Property XmlName As String
        Get
            Return _xmlName
        End Get
    End Property
End Class

Public Class FieldException
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

Public Class FieldNotFoundException
    Inherits FieldException

    Public Sub New()
    End Sub

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub
End Class

Public Class Fields
    Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(Fields))
    Private ReadOnly _id As String
    Private ReadOnly _name As String

    ' ReSharper disable ConvertToConstant.Local
    ' ReSharper disable FieldCanBeMadeReadOnly.Local
    <ConfingName("DATE")> Private _lastDate As String = ""
    <ConfingName("TIME")> Private _time As String = ""
    <ConfingName("BID")> Private _bid As String = ""
    <ConfingName("ASK")> Private _ask As String = ""
    <ConfingName("LAST")> Private _last As String = ""
    <ConfingName("VWAP")> Private _vwap As String = ""
    <ConfingName("HIST")> Private _hist As String = ""     ' Historical Field. Corresponds to Close
    <ConfingName("HIST_DATE")> Private _histDate As String = ""
    <ConfingName("VOLUME")> Private _volume As String = ""
    <ConfingName("SOURCE")> Private _src As String = ""
    ' ReSharper restore ConvertToConstant.Local
    ' ReSharper restore FieldCanBeMadeReadOnly.Local

    Public Function GetField(ByVal name As String)
        Dim res = (From fld In GetType(Fields).GetFields(BindingFlags.NonPublic Or BindingFlags.Instance)
                           Let attx = fld.GetCustomAttributes(GetType(ConfingNameAttribute), False)
                           Where attx.Any
                           Let q = CType(attx.First, ConfingNameAttribute)
                           Where q.XmlName = name
                           Select fld.GetValue(Me))

        Return res.First
    End Function

    Public ReadOnly Property ID() As String
        Get
            Return _id
        End Get
    End Property

    Private Sub Update()
        Dim fieldInfos = Me.GetType().GetFields(BindingFlags.NonPublic Or BindingFlags.Instance)
        Dim list = (From fld In fieldInfos
                    Let attx = fld.GetCustomAttributes(GetType(ConfingNameAttribute), False)
                    Where attx.Any
                    Let val1 = fld.GetValue(Me)
                    Where val1 IsNot Nothing
                    Let confName = CType(attx(0), ConfingNameAttribute).XmlName, value = val1.ToString().Trim()
                    Where value <> ""
                    Select confName = confName, value = value).ToDictionary(Function(item) item.confName, Function(item) item.value)
        PortfolioManager.Instance().UpdateFieldSet(_id, _name, list)
    End Sub

    <DisplayName("Trade date")>
    Public Property LastDate() As String
        Get
            Return _lastDate
        End Get
        Set(ByVal value As String)
            _lastDate = value
            Update()
        End Set
    End Property

    <DisplayName("Last trade time")>
    Public Property Time() As String
        Get
            Return _time
        End Get
        Set(ByVal value As String)
            _time = value
            Update()
        End Set
    End Property

    Public Property Bid() As String
        Get
            Return _bid
        End Get
        Set(ByVal value As String)
            _bid = value
            Update()
        End Set
    End Property

    Public Property Ask() As String
        Get
            Return _ask
        End Get
        Set(ByVal value As String)
            _ask = value
            Update()
        End Set
    End Property

    Public Property Last() As String
        Get
            Return _last
        End Get
        Set(ByVal value As String)
            _last = value
            Update()
        End Set
    End Property

    Public Property VWAP() As String
        Get
            Return _vwap
        End Get
        Set(ByVal value As String)
            _vwap = value
            Update()
        End Set
    End Property

    <DisplayName("Historical price")>
    Public Property Hist() As String
        Get
            Return _hist
        End Get
        Set(ByVal value As String)
            _hist = value
            Update()
        End Set
    End Property

    <DisplayName("Historical price date")>
    Public Property HistDate() As String
        Get
            Return _histDate
        End Get
        Set(ByVal value As String)
            _histDate = value
            Update()
        End Set
    End Property

    <DisplayName("Trade volume")>
    Public Property Volume() As String
        Get
            Return _volume
        End Get
        Set(ByVal value As String)
            _volume = value
            Update()
        End Set
    End Property

    <DisplayName("Last quote source")>
    Public Property Src() As String
        Get
            Return _src
        End Get
        Set(ByVal value As String)
            _src = value
            Update()
        End Set
    End Property

    Public Sub New(ByVal id As String, ByVal node As XmlNode, ByVal subnode As String)
        _name = subnode
        _id = id
        'Logger.Trace(String.Format("FieldSet({0}, {1})", node.Name, subnode))
        For Each info In (From fld In Me.GetType().GetFields(BindingFlags.NonPublic Or BindingFlags.Instance)
                           Let attx = fld.GetCustomAttributes(GetType(ConfingNameAttribute), False)
                           Where attx.Any
                           Select fld, attx)

            Dim xmlName = CType(info.attx(0), ConfingNameAttribute).XmlName
            'Logger.Trace(" ---> found node named {0}", xmlName)
            Dim item = node.SelectSingleNode(String.Format("{0}/field[@type='{1}']", subnode, xmlName))
            If item IsNot Nothing Then
                'Logger.Trace(" ---> {0} <- {1}", xmlName, item.InnerText)
                info.fld.SetValue(Me, item.InnerText)
                'Else
                '    Logger.Trace(" ---> no data")
            End If
        Next
    End Sub

    Public Overrides Function ToString() As String
        Return _name
    End Function

    Public Function AsDataSource() As List(Of FieldDescription)
        Return (From fld In Me.GetType().GetFields(BindingFlags.NonPublic Or BindingFlags.Instance)
            Let attx = fld.GetCustomAttributes(GetType(ConfingNameAttribute), False)
            Where attx.Any
            Let xmlName = CType(attx(0), ConfingNameAttribute).XmlName, value = fld.GetValue(Me).ToString()
            Select New FieldDescription(xmlName, value, Me)).ToList()
    End Function

    Public Sub UpdateField(ByVal configName As String, ByVal value As String)
        Logger.Trace("UpdateField({0},{1})", configName, value)
        Dim fields = (From fld In Me.GetType().GetFields(BindingFlags.NonPublic Or BindingFlags.Instance)
            Let attx = fld.GetCustomAttributes(GetType(ConfingNameAttribute), False)
            Where attx.Any AndAlso CType(attx(0), ConfingNameAttribute).XmlName = configName
            Select fld).ToList()
        If Not fields.Any Then Throw New FieldNotFoundException(String.Format("Field with name {0} not found", configName))
        If fields.Count > 1 Then Throw New FieldException(String.Format("There's more than one field with name {0}", configName))
        Dim field = fields.First()
        field.SetValue(Me, value)
        Update()
    End Sub
End Class

Public Class FieldDescription
    Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(FieldDescription))
    Private ReadOnly _configName As String
    Private _value As String
    Private ReadOnly _parent As Fields

    <Browsable(False)>
    Public ReadOnly Property Parent() As Fields
        Get
            Return _parent
        End Get
    End Property

    Public Sub New(ByVal configName As String, ByVal value As String, ByVal parent As Fields)
        _configName = configName
        _value = value
        _parent = parent
    End Sub

    <DisplayName("Name")>
    Public ReadOnly Property ConfigName() As String
        Get
            Return _configName
        End Get
    End Property

    Public Property Value() As String
        Get
            Return _value
        End Get
        Set(ByVal val As String)
            _value = val
            Try
                _parent.UpdateField(_configName, _value)
            Catch ex As FieldException
                Logger.ErrorException("Failed to save field update", ex)
                Logger.Error("Exception = {0}", ex.ToString())
            End Try
        End Set
    End Property
End Class

Public Class FieldSet
    Private ReadOnly _name As String
    Private ReadOnly _history As Fields
    Private ReadOnly _realtime As Fields
    Private ReadOnly _id As String

    Public Sub New(ByVal id As String, Optional ByVal doc As XmlDocument = Nothing)
        _id = id
        If doc Is Nothing Then doc = PortfolioManager.ClassInstance.GetConfigDocument
        Dim node = doc.SelectSingleNode(String.Format("/bonds/field-sets/field-set[@id='{0}']", id))
        If node Is Nothing Then Throw New Exception(String.Format("Failed to find field set with id {0}", id))
        _name = node.Attributes("name").Value
        _realtime = New Fields(id, node, "realtime")
        _history = New Fields(id, node, "historical")
    End Sub

    Public Overrides Function ToString() As String
        Return Name
    End Function

    Public ReadOnly Property Name As String
        Get
            Return _name
        End Get
    End Property

    Public ReadOnly Property History As Fields
        Get
            Return _history
        End Get
    End Property

    Public ReadOnly Property Realtime As Fields
        Get
            Return _realtime
        End Get
    End Property

    Public ReadOnly Property AsDataSource() As List(Of Fields)
        Get
            Return New List(Of Fields)({_history, _realtime})
        End Get
    End Property

    Public ReadOnly Property ID As String
        Get
            Return _id
        End Get
    End Property
End Class