Imports System.ComponentModel
Imports DbManager.Bonds
Imports Uitls
Imports System.Xml

Public MustInherit Class Source
    Private ReadOnly _id As String
    Private _color As String
    Private _name As String
    Private _enabled As Boolean
    Private _curve As Boolean
    Private _fieldSetId As String
    Private _fields As FieldSet

    Protected Friend ReadOnly PortMan As PortfolioManager = PortfolioManager.ClassInstance

    Protected Overloads Function Equals(ByVal other As Source) As Boolean
        Return String.Equals(_id, other._id)
    End Function

    Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
        If ReferenceEquals(Nothing, obj) Then Return False
        If ReferenceEquals(Me, obj) Then Return True
        Dim other As Source = TryCast(obj, Source)
        Return other IsNot Nothing AndAlso Equals(other)
    End Function

    Public Overrides Function GetHashCode() As Integer
        If _id Is Nothing Then Return 0
        Return _id.GetHashCode
    End Function

    Public Shared Operator =(ByVal left As Source, ByVal right As Source) As Boolean
        Return Equals(left, right)
    End Operator

    Public Shared Operator <>(ByVal left As Source, ByVal right As Source) As Boolean
        Return Not Equals(left, right)
    End Operator

    Friend Sub New(ByVal id As String, ByVal color As String, ByVal fields As FieldSet, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal name As String)
        _id = id
        _color = color
        _enabled = enabled
        _curve = curve
        _name = name
        FieldSetId = fields.ID
    End Sub

    Public Sub New(ByVal color As String, ByVal fldSetId As String, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal name As String)
        _id = GenerateId()
        _color = color
        _enabled = enabled
        _curve = curve
        _name = name
        FieldSetId = fldSetId
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

    Public Function GetDefaultRicsView() As List(Of BondDescription)
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

    Public Shared Operator =(ByVal left As Chain, ByVal right As Chain) As Boolean
        Return Equals(left, right)
    End Operator

    Public Shared Operator <>(ByVal left As Chain, ByVal right As Chain) As Boolean
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
    Inherits Source
    ' todo equality members
    Public Sub New(ByVal id As String, ByVal color As String, ByVal fields As FieldSet, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal name As String)
        MyBase.New(id, color, fields, enabled, curve, name)
    End Sub

    Public Sub New(ByVal color As String, ByVal fldSetId As String, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal name As String)
        MyBase.New(color, fldSetId, enabled, curve, name)
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

Public Class CustomBond
    Inherits Source
    ' todo equality members

    Public Sub New(ByVal id As String, ByVal color As String, ByVal fields As FieldSet, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal name As String)
        MyBase.New(id, color, fields, enabled, curve, name)
    End Sub

    Public Sub New(ByVal color As String, ByVal fldSetId As String, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal name As String)
        MyBase.New(color, fldSetId, enabled, curve, name)
    End Sub

    Public Overrides Function GetXmlTypeName() As String
        Return "custom-bond"
    End Function

    Protected Overrides Function GenerateId() As String
        Return PortMan.GenerateNewCustomBondId()
    End Function

    Public Overrides Function GetDefaultRics() As List(Of String)
        Return {Name}.ToList()
    End Function

    <DisplayName("Is curve")>
    Public Overrides Property Curve() As Boolean
        Get
            Return False
        End Get
        Set(ByVal value As Boolean)
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return "Custom Bond"
    End Function
End Class