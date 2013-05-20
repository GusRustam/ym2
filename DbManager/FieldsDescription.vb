Imports System.ComponentModel
Imports NLog
Imports System.Reflection
Imports System.Xml
Imports Settings

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

Public Class FieldsDescription
    Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(FieldsDescription))
    Private ReadOnly _id As String
    Private ReadOnly _name As String

    ' ReSharper disable ConvertToConstant.Local
    ' ReSharper disable FieldCanBeMadeReadOnly.Local
    <MarkerStyle("Square")>
    <Price("White")>
    <ConfingName("CUSTOM")>
    Private _custom As String = "CUSTOM"

    <Price("White")>
    <ConfingName("MID")>
    Private _mid As String = "MID"

    <ConfingName("DATE")>
    Private _lastDate As String = ""

    <ConfingName("TIME")>
    Private _time As String = ""

    <Price("White")>
    <ConfingName("BID")>
    Private _bid As String = ""

    <Price("White")>
    <ConfingName("ASK")>
    Private _ask As String = ""

    <Price("White")>
    <ConfingName("LAST")>
    Private _last As String = ""

    <Price("White")>
    <ConfingName("VWAP")>
    Private _vwap As String = ""

    <Price("LightGray")>
    <ConfingName("HIST")>
    Private _hist As String = ""     ' Historical Field. Corresponds to Close

    <ConfingName("HIST_DATE")>
    Private _histDate As String = ""

    <ConfingName("VOLUME")>
    Private _volume As String = ""

    <ConfingName("SOURCE")>
    Private _src As String = ""
    ' ReSharper restore ConvertToConstant.Local
    ' ReSharper restore FieldCanBeMadeReadOnly.Local

    Public Function AsContainer() As FieldContainer
        Return New FieldContainer(Me,
            From fld In GetType(FieldsDescription).GetFields(BindingFlags.NonPublic Or BindingFlags.Instance)
            Let attx = fld.GetCustomAttributes(GetType(ConfingNameAttribute), False)
            Where attx.Any
            Let q = CType(attx.First, ConfingNameAttribute), val = CStr(fld.GetValue(Me))
            Let isPrice = fld.GetCustomAttributes(GetType(PriceAttribute), False).Any
            Select tuple.Create(val, q.XmlName, isPrice))
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

    <Browsable(False)>
    Public Property [Mid]() As String
        Get
            Return _mid
        End Get
        Set(ByVal value As String)
            _mid = value
        End Set
    End Property

    <Browsable(False)>
    Public Property [Custom]() As String
        Get
            Return _custom
        End Get
        Set(ByVal value As String)
            _custom = value
        End Set
    End Property

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
                           Let attx = fld.GetCustomAttributes(GetType(ConfingNameAttribute), False).Cast(Of ConfingNameAttribute)()
                           Where attx.Any
                           Select fld, attx)

            Dim xmlName = info.attx(0).XmlName
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
            Let attx = fld.GetCustomAttributes(GetType(ConfingNameAttribute), False).Cast(Of ConfingNameAttribute)()
            Where attx.Any
            Let xmlName = attx(0).XmlName, value = fld.GetValue(Me).ToString()
            Select New FieldDescription(xmlName, value, Me)).ToList()
    End Function

    Friend Sub UpdateField(ByVal configName As String, ByVal value As String)
        Logger.Trace("UpdateField({0},{1})", configName, value)
        Dim fields = (From fld In Me.GetType().GetFields(BindingFlags.NonPublic Or BindingFlags.Instance)
            Let attx = fld.GetCustomAttributes(GetType(ConfingNameAttribute), False).Cast(Of ConfingNameAttribute)()
            Where attx.Any AndAlso attx(0).XmlName = configName
            Select fld).ToList()
        If Not fields.Any Then Throw New FieldNotFoundException(String.Format("Field with name {0} not found", configName))
        If fields.Count > 1 Then Throw New FieldException(String.Format("There's more than one field with name {0}", configName))
        Dim field = fields.First()
        field.SetValue(Me, value)
        Update()
    End Sub

    Public Function BackColor(ByVal fieldName As String) As String
        Dim items = (From fld In Me.GetType().GetFields(BindingFlags.NonPublic Or BindingFlags.Instance)
                Let attxU = fld.GetCustomAttributes(GetType(ConfingNameAttribute), False).Cast(Of ConfingNameAttribute)()
                Where attxU.Any AndAlso attxU.First.XmlName = fieldName
                Let attx = fld.GetCustomAttributes(GetType(PriceAttribute), False).Cast(Of PriceAttribute)()
                Where attx.Any
                Select attx.First.Color).ToList()

        Return If(items.Any, items.First, "White")
    End Function

    Public Function MarkerStyle(ByVal fieldName As String) As String
        Dim items = (From fld In Me.GetType().GetFields(BindingFlags.NonPublic Or BindingFlags.Instance)
                Let attxU = fld.GetCustomAttributes(GetType(ConfingNameAttribute), False).Cast(Of ConfingNameAttribute)()
                Where attxU.Any AndAlso attxU.First.XmlName = fieldName
                Let attx = fld.GetCustomAttributes(GetType(MarkerStyleAttribute), False).Cast(Of MarkerStyleAttribute)()
                Where attx.Any
                Select attx.First.Style).ToList()

        Return If(items.Any, items.First, "")
    End Function

    Public Function IsPrice(ByVal configName As String) As Boolean
        Return (From fld In Me.GetType().GetFields(BindingFlags.NonPublic Or BindingFlags.Instance)
            Let attx = fld.GetCustomAttributes(GetType(ConfingNameAttribute), False).Cast(Of ConfingNameAttribute)()
            Where attx.Any AndAlso attx(0).XmlName = configName
            Let attxP = fld.GetCustomAttributes(GetType(PriceAttribute), False)
            Where attxP.Any).Any
    End Function

    Public Function GetPriceFieldNames() As List(Of String)
        Return (From fld In Me.GetType().GetFields(BindingFlags.NonPublic Or BindingFlags.Instance)
            Let attx = fld.GetCustomAttributes(GetType(ConfingNameAttribute), False).Cast(Of ConfingNameAttribute)()
            Where attx.Any
            Let attxP = fld.GetCustomAttributes(GetType(PriceAttribute), False)
            Where attxP.Any
            Select fld.GetValue(Me)).Cast(Of String).ToList()
    End Function
End Class

Public Class FieldContainer
    Private ReadOnly _fields As FieldsDescription
    Private ReadOnly _nameToXml As New Dictionary(Of String, String)
    Private ReadOnly _nameToPrice As New Dictionary(Of String, Boolean)
    Private ReadOnly _xmlToName As New Dictionary(Of String, String)
    Private ReadOnly _xmlToPrice As New Dictionary(Of String, Boolean)
    Private ReadOnly _xmlNameToPriority As New Dictionary(Of String, Integer)

    ''' <summary>
    ''' Initialized Field Container
    ''' </summary>
    ''' <param name="data">list of tuples of (string, string, bool) where
    ''' item1 is real name
    ''' item2 is xml name
    ''' item3 is price flag
    ''' </param>
    ''' <remarks></remarks>
    Public Sub New(ByVal fields As FieldsDescription, ByVal data As IEnumerable(Of Tuple(Of String, String, Boolean)))
        Dim fieldPriority = SettingsManager.Instance.FieldsPriority.Split(",")
        Dim i As Integer
        For i = 0 To fieldPriority.Count - 1
            _xmlNameToPriority.Add(fieldPriority(i), i)
        Next

        _fields = fields
        For Each elem In From item In data Where item.Item1 <> ""
            _nameToXml.Add(elem.Item1, elem.Item2)
            _xmlToName.Add(elem.Item2, elem.Item1)
            _nameToPrice.Add(elem.Item1, elem.Item3)
            _xmlToPrice.Add(elem.Item2, elem.Item3)
        Next
    End Sub

    Public ReadOnly Property AllNames() As List(Of String)
        Get
            Return _nameToXml.Keys.ToList()
        End Get
    End Property

    Public ReadOnly Property AllXmlNames() As List(Of String)
        Get
            Return _xmlToName.Keys.ToList()
        End Get
    End Property

    Public ReadOnly Property XmlName(ByVal nm As String) As String
        Get
            Return If(_nameToXml.Keys.Contains(nm), _nameToXml(nm), "")
        End Get
    End Property

    Public ReadOnly Property Name(ByVal xName As String) As String
        Get
            Return If(_xmlToName.Keys.Contains(xName), _xmlToName(xName), "")
        End Get
    End Property

    Public ReadOnly Property Fields As FieldsDescription
        Get
            Return _fields
        End Get
    End Property

    Public ReadOnly Property IsPriceByName(ByVal nm As String) As Boolean
        Get
            Return _nameToPrice.Keys.Contains(nm) AndAlso _nameToPrice(nm)
        End Get
    End Property

    Public ReadOnly Property IsPriceByXmlName(ByVal xName As String) As Boolean
        Get
            Return _xmlToPrice.Keys.Contains(xName) AndAlso _xmlToPrice(xName)
        End Get
    End Property

    'Public Function IsMaximumPriority(ByVal whatXmlName As String, ByVal xmlNames As IEnumerable(Of String))
    '    Return _xmlNameToPriority(whatXmlName) >= (From key In xmlNames Select _xmlNameToPriority(key)).Max
    'End Function
End Class

Friend Class PriceAttribute
    Inherits Attribute
    Private ReadOnly _color As String

    Sub New(ByVal color As String)
        _color = color
    End Sub

    Public ReadOnly Property Color As String
        Get
            Return _color
        End Get
    End Property
End Class

Friend Class MarkerStyleAttribute
    Inherits Attribute
    Private ReadOnly _style As String

    Sub New(ByVal style As String)
        _style = style
    End Sub

    Public ReadOnly Property Style As String
        Get
            Return _style
        End Get
    End Property
End Class

Public Class FieldDescription
    Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(FieldDescription))
    Private ReadOnly _configName As String
    Private _value As String
    Private ReadOnly _parent As FieldsDescription

    <Browsable(False)>
    Public ReadOnly Property Parent() As FieldsDescription
        Get
            Return _parent
        End Get
    End Property

    <DisplayName("Is price")>
    Public ReadOnly Property IsPrice() As Boolean
        Get
            Return _parent.IsPrice(_configName)
        End Get
    End Property

    Public Sub New(ByVal configName As String, ByVal value As String, ByVal parent As FieldsDescription)
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
    Private ReadOnly _history As FieldsDescription
    Private ReadOnly _realtime As FieldsDescription
    Private ReadOnly _id As String

    Public Sub New(ByVal id As String, Optional ByVal doc As XmlDocument = Nothing)
        _id = id
        If doc Is Nothing Then doc = PortfolioManager.ClassInstance.GetConfigDocument
        Dim node = doc.SelectSingleNode(String.Format("/bonds/field-sets/field-set[@id='{0}']", id))
        If node Is Nothing Then Throw New Exception(String.Format("Failed to find field set with id {0}", id))
        _name = node.Attributes("name").Value
        _realtime = New FieldsDescription(id, node, "realtime")
        _history = New FieldsDescription(id, node, "historical")
    End Sub

    Public Overrides Function ToString() As String
        Return Name
    End Function

    Public ReadOnly Property Name As String
        Get
            Return _name
        End Get
    End Property

    Public ReadOnly Property History As FieldsDescription
        Get
            Return _history
        End Get
    End Property

    Public ReadOnly Property Realtime As FieldsDescription
        Get
            Return _realtime
        End Get
    End Property

    Public ReadOnly Property AsDataSource() As List(Of FieldsDescription)
        Get
            Return New List(Of FieldsDescription)({_history, _realtime})
        End Get
    End Property

    Public ReadOnly Property ID As String
        Get
            Return _id
        End Get
    End Property
End Class