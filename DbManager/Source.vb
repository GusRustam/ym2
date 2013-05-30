Imports System.ComponentModel
Imports DbManager.Bonds
Imports Uitls
Imports System.Xml

Public MustInherit Class SourceBase
    Private ReadOnly _id As String
    Private _color As String
    Private _name As String
    Protected Friend ReadOnly PortMan As PortfolioManager = PortfolioManager.ClassInstance

    Friend Sub New(ByVal id As String, ByVal color As String, ByVal name As String)
        _id = id
        _color = color
        _name = name
    End Sub

    Friend Sub New(ByVal color As String, ByVal name As String)
        _id = GenerateId()
        _color = color
        _name = name
    End Sub

    <Browsable(False)>
    Public ReadOnly Property ID As String
        Get
            Return _id
        End Get
    End Property

    Public Property Name As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Public Property Color As String
        Get
            Return _color
        End Get
        Set(ByVal value As String)
            _color = value
        End Set
    End Property

    ''' <summary>
    ''' Get list of all Source rics "by default"
    ''' This means that in a portfolio rics might differ because of filters and excluded items
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetDefaultRics() As List(Of String)
    Public MustOverride Function GetXmlTypeName() As String
    Protected MustOverride Function GenerateId() As String
End Class

Public MustInherit Class Source
    Inherits SourceBase
    Private _enabled As Boolean
    Private _curve As Boolean
    Private _fieldSetId As String
    Private _fields As FieldSet

    Protected Overloads Function Equals(ByVal other As Source) As Boolean
        Return String.Equals(ID, other.ID)
    End Function

    Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
        If ReferenceEquals(Nothing, obj) Then Return False
        If ReferenceEquals(Me, obj) Then Return True
        Dim other As Source = TryCast(obj, Source)
        Return other IsNot Nothing AndAlso Equals(other)
    End Function

    Public Overrides Function GetHashCode() As Integer
        If ID Is Nothing Then Return 0
        Return ID.GetHashCode
    End Function

    Public Shared Operator =(ByVal left As Source, ByVal right As Source) As Boolean
        Return Equals(left, right)
    End Operator

    Public Shared Operator <>(ByVal left As Source, ByVal right As Source) As Boolean
        Return Not Equals(left, right)
    End Operator

    Friend Sub New(ByVal id As String, ByVal color As String, ByVal fields As FieldSet, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal name As String)
        MyBase.new(id, color, name)
        _enabled = enabled
        _curve = curve
        FieldSetId = fields.ID
    End Sub

    Public Sub New(ByVal color As String, ByVal fldSetId As String, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal name As String)
        MyBase.new(color, name)
        _enabled = enabled
        _curve = curve
        FieldSetId = fldSetId
    End Sub

    <Browsable(False)>
    Public Property FieldSetId() As String
        Get
            Return _fieldSetId
        End Get
        Set(ByVal value As String)
            _fieldSetId = value
            _fields = New FieldSet(_fieldSetId)
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property Fields As FieldSet
        Get
            Return _fields
        End Get
    End Property

    <DisplayName("Fields layout")>
    Public ReadOnly Property FieldsName As String
        Get
            Return _fields.Name
        End Get
    End Property

    Public Property Enabled As Boolean
        Get
            Return _enabled
        End Get
        Set(ByVal value As Boolean)
            _enabled = value
        End Set
    End Property

    <DisplayName("Is curve")>
    Public Overridable Property Curve As Boolean
        Get
            Return _curve
        End Get
        Set(ByVal value As Boolean)
            _curve = value
        End Set
    End Property


    Public Function GetDefaultRicsView() As List(Of BondMetadata)
        Dim bondsData As IBondsData = Bonds.BondsData.Instance
        Return (From ric In GetDefaultRics()
            Where bondsData.BondExists(ric)
            Select bondsData.GetBondInfo(ric)).ToList()
    End Function
End Class

Public Class Chain
    Inherits Source
    Private _chainRic As String
    Private ReadOnly _bondsManager As IBondsLoader = BondsLoader.Instance

    Public Overrides Function ToString() As String
        Return Name
    End Function

    Friend Sub New(ByVal id As String, ByVal color As String, ByVal fields As FieldSet, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal chainRic As String, ByVal name As String)
        MyBase.New(id, color, fields, enabled, curve, name)
        _chainRic = chainRic
    End Sub

    Public Sub New(ByVal color As String, ByVal fieldSetId As String, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal name As String, ByVal chainRic As String)
        MyBase.New(color, fieldSetId, enabled, curve, name)
        _chainRic = chainRic
        PortMan.AddSource(Me)
    End Sub

    <DisplayName("Chain RIC")>
    Public Property ChainRic As String
        Get
            Return _chainRic
        End Get
        Set(ByVal value As String)
            _chainRic = value
        End Set
    End Property

    Public Overrides Function GetDefaultRics() As List(Of String)
        Return _bondsManager.GetChainRics(_chainRic)
    End Function

    Public Overrides Function GetXmlTypeName() As String
        Return "chain"
    End Function

    Protected Overrides Function GenerateId() As String
        Return PortMan.GenerateNewChainId()
    End Function

    Public Shared Function Load(ByVal id As String) As Chain
        Dim xml = PortfolioManager.ClassInstance.GetConfigDocument()
        Dim node = xml.SelectSingleNode(String.Format("/bonds/chains//chain[@id='{0}']", id))
        If node Is Nothing Then Throw New NoSourceException(String.Format("Failed to find chain with id {0}", id))
        Try
            Dim color = node.GetAttrStrict("color")
            Dim name = node.GetAttrStrict("name")
            Dim enabled = node.GetAttr("enabled", "True")
            Dim curve = node.GetAttr("curve", "False")
            Dim chainRic = node.GetAttrStrict("ric")
            Dim fields = New FieldSet(node.GetAttrStrict("field-set-id"))
            Return New Chain(id, color, fields, enabled, curve, chainRic, name)
        Catch ex As Exception
            Throw New NoSourceException(String.Format("Failed to find chain with id {0}", id), ex)
        End Try
    End Function

    Protected Overloads Function Equals(ByVal other As Chain) As Boolean
        Return MyBase.Equals(other) AndAlso String.Equals(_chainRic, other._chainRic)
    End Function

    Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
        If ReferenceEquals(Nothing, obj) Then Return False
        If ReferenceEquals(Me, obj) Then Return True
        If obj.GetType IsNot Me.GetType Then Return False
        Return Equals(DirectCast(obj, Chain))
    End Function

    Public Overrides Function GetHashCode() As Integer
        Dim hashCode As Integer = MyBase.GetHashCode
        If _chainRic IsNot Nothing Then
            hashCode = CInt((hashCode * 397L) Mod Integer.MaxValue) Xor _chainRic.GetHashCode
        End If
        Return hashCode
    End Function

    Public Shared Shadows Operator =(ByVal left As Chain, ByVal right As Chain) As Boolean
        Return Equals(left, right)
    End Operator

    Public Shared Shadows Operator <>(ByVal left As Chain, ByVal right As Chain) As Boolean
        Return Not Equals(left, right)
    End Operator
End Class

Public Class UserList
    Inherits Source

    Public Sub New(ByVal color As String, ByVal fieldSetId As String, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal name As String)
        MyBase.New(color, fieldSetId, enabled, curve, name)
        PortMan.AddSource(Me)
    End Sub

    Public Overrides Function ToString() As String
        Return Name
    End Function

    Friend Sub New(ByVal id As String, ByVal color As String, ByVal fields As FieldSet, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal name As String)
        MyBase.New(id, color, fields, enabled, curve, name)
    End Sub

    Shared Function ExtractRics(ByVal node As XmlNode) As List(Of String)
        Return (From ric As XmlNode In node.SelectNodes("ric") Select ric.InnerText).ToList()
    End Function

    Public Overrides Function GetDefaultRics() As List(Of String)
        Dim node = PortfolioManager.ClassInstance.GetConfigDocument().SelectSingleNode(String.Format("/bonds/lists/list[@id='{0}']", ID))
        If node Is Nothing Then Throw New NoSourceException(String.Format("Failed to find list with id {0}", ID))
        Return ExtractRics(node)
    End Function

    Public Overrides Function GetXmlTypeName() As String
        Return "list"
    End Function

    Protected Overrides Function GenerateId() As String
        Return PortMan.GenerateNewListId()
    End Function

    Public Shared Function Load(ByVal listId As String) As UserList
        Dim node = PortfolioManager.ClassInstance.GetConfigDocument().SelectSingleNode(String.Format("/bonds/lists/list[@id='{0}']", listId))
        If node Is Nothing Then Throw New NoSourceException(String.Format("Failed to find list with id {0}", listId))
        Try
            Dim color = node.GetAttrStrict("color")
            Dim name = node.GetAttrStrict("name")
            Dim enabled = node.GetAttr("enabled", "True")
            Dim curve = node.GetAttr("curve", "False")
            Dim fields = New FieldSet(node.GetAttrStrict("field-set-id"))
            Return New UserList(listId, color, fields, enabled, curve, name)
        Catch ex As Exception
            Throw New NoSourceException(String.Format("Failed to find list with id {0}", listId), ex)
        End Try
    End Function

    Public Sub AddItems(ByVal selectedRICs As List(Of String))
        Dim xml = PortfolioManager.ClassInstance.GetConfigDocument()
        Dim node = xml.SelectSingleNode(String.Format("/bonds/lists/list[@id='{0}']", ID))
        If node Is Nothing Then Return
        For Each ric In selectedRICs
            If node.SelectSingleNode(String.Format("ric[text() = '{0}']", ric)) Is Nothing Then
                Dim ricNode = xml.CreateNode(XmlNodeType.Element, "ric", "")
                ricNode.InnerText = ric
                node.AppendChild(ricNode)
            End If
        Next
        PortfolioManager.ClassInstance.SaveBonds()
    End Sub

    Public Sub RemoveItems(ByVal rics As List(Of String))
        Dim xml = PortfolioManager.ClassInstance.GetConfigDocument()
        Dim node = xml.SelectSingleNode(String.Format("/bonds/lists/list[@id='{0}']", ID))
        If node Is Nothing Then Return
        For Each kid In From ric In rics
                        Let childNode = node.SelectSingleNode(String.Format("ric[text() = '{0}']", ric))
                        Where childNode IsNot Nothing
                        Select elem = childNode
            node.RemoveChild(kid)
        Next
        PortfolioManager.ClassInstance.SaveBonds()
    End Sub
End Class

Public Class RegularBond
    Inherits SourceBase
    ' todo equality members
    Public Sub New(ByVal id As String, ByVal color As String, ByVal name As String)
        MyBase.New(id, color, name)
    End Sub

    Public Overrides Function GetDefaultRics() As List(Of String)
        Throw New NotImplementedException()
    End Function

    Public Overrides Function GetXmlTypeName() As String
        Throw New NotImplementedException()
    End Function

    Protected Overrides Function GenerateId() As String
        Throw New NotImplementedException()
    End Function
End Class


' todo think of it
' Два принципиально разных подхода к созданию объекта
' 1) виртуализировать существующую xml запись
' 2) создать "висячий" объект, который может быть сохранен, а может и не быть сохранен
Public Class CustomBond
    Inherits SourceBase
    ' todo equality members

    Private ReadOnly _code As String
    Private ReadOnly _struct As ReutersBondStructure
    Private _maturity As Date?
    Private _currentCouponRate As Double

    Public Sub New(ByVal id As String, ByVal color As String, ByVal name As String, ByVal code As String, ByVal struct As String, ByVal maturity As String, ByVal currentCouponRate As Double)
        MyBase.New(id, color, name)
        _code = code
        If maturity <> "" Then _maturity = ReutersDate.ReutersToDate(maturity)
        _currentCouponRate = currentCouponRate
        _struct = ReutersBondStructure.Parse(struct)
    End Sub

    Public Sub New(ByVal color As String, ByVal name As String, ByVal code As String, ByVal struct As String, ByVal maturity As String, ByVal currentCouponRate As Double)
        MyBase.New(color, name)
        _code = code
        If maturity <> "" Then _maturity = ReutersDate.ReutersToDate(maturity)
        _currentCouponRate = currentCouponRate
        _struct = ReutersBondStructure.Parse(struct)
    End Sub

    Public ReadOnly Property Code() As String
        Get
            Return _code
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property Struct As ReutersBondStructure
        Get
            Return _struct
        End Get
    End Property

    Public Property Maturity() As Date?
        Get
            Return _maturity
        End Get
        Set(ByVal value As Date?)
            _maturity = value
        End Set
    End Property

    <DisplayName("Coupon")>
    Public Property CurrentCouponRate() As Double
        Get
            Return _currentCouponRate
        End Get
        Set(ByVal value As Double)
            _currentCouponRate = value
        End Set
    End Property

    Public Overrides Function GetXmlTypeName() As String
        Return "custom-bond"
    End Function

    Protected Overrides Function GenerateId() As String
        Return PortMan.GenerateNewCustomBondId()
    End Function

    Public Overrides Function GetDefaultRics() As List(Of String)
        Return {_code}.ToList()
    End Function


    Public Overrides Function ToString() As String
        Return "Custom Bond"
    End Function

    Public Shared Function Load(ByVal bndId As String) As CustomBond
        Dim node = PortfolioManager.ClassInstance.GetConfigDocument().SelectSingleNode(String.Format("/bonds/custom-bonds/bond[@id='{0}']", bndId))
        If node Is Nothing Then Throw New NoSourceException(String.Format("Failed to find custom bond with id {0}", bndId))
        Try
            Dim color = node.GetAttrStrict("color")
            Dim name = node.GetAttrStrict("name")
            Dim code = node.GetAttrStrict("code")
            Dim struct = node.GetAttrStrict("bondStructure")
            Dim maturity = node.GetAttrStrict("maturity")
            Dim coupon = node.GetAttrStrict("coupon")
            Return New CustomBond(bndId, color, name, code, struct, maturity, coupon)
        Catch ex As Exception
            Throw New NoSourceException(String.Format("Failed to find list with id {0}", bndId), ex)
        End Try
    End Function
End Class