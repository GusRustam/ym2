Imports System.ComponentModel
Imports DbManager.Bonds
Imports Logging
Imports NLog
Imports Uitls
Imports System.Xml

Public MustInherit Class SourceBase
    Protected Shared ReadOnly Logger As Logger = GetLogger(GetType(SourceBase))
    Private ReadOnly _id As String
    Private _color As String
    Private _name As String
    Protected Friend ReadOnly PortMan As PortfolioManager = PortfolioManager.ClassInstance

    Public Overridable Sub Update(ByVal src As SourceBase)
        _color = src.Color
        _name = src.Name
    End Sub

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
    Public MustOverride Function GetXmlPath() As String
    Protected MustOverride Function GenerateId() As String

    ''' <summary>
    ''' Tries to delete current source
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Kill() 'As List(Of String)
        Dim xml = PortfolioManager.ClassInstance.GetConfigDocument()
        Dim node = xml.SelectSingleNode(String.Format("/bonds/{0}[@id='{1}']", GetXmlPath(), ID))
        If node IsNot Nothing Then
            Try
                node.ParentNode.RemoveChild(node)
            Catch ex As Exception
                Logger.Warn("Failed to delete source {0} id {1}", GetXmlTypeName(), ID)
                Logger.Warn("Exception = {0}", ex.ToString())
            End Try
            PortfolioManager.ClassInstance.SaveBonds()
        End If
        'Dim res As New List(Of String)
        'For Each port In PortfolioManager.ClassInstance.GetPortfoliosBySource(Me)
        '    Try
        '        Dim prt = New PortfolioWrapper(port.Id)
        '        If prt.TryDeleteSource(Me) Then res.Add(port.Id)
        '    Catch ex As Exception
        '        Logger.Warn("Failed to delete source from portfolio with id {0}", port.Id)
        '        Logger.Warn("Exception = {0}", ex.ToString())
        '    End Try
        'Next

        'If res.Any Then PortfolioManager.ClassInstance.SaveBonds()
        'Return res
    End Sub
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
            Let descr = bondsData.GetBondInfo(ric)
            Where descr IsNot Nothing
            Select descr).ToList()
    End Function

    Public Overrides Sub Update(ByVal src As SourceBase)
        MyBase.Update(src)
        Dim srcc = CType(src, Source)
        _curve = srcc.Curve
        _enabled = srcc.Enabled
        _fieldSetId = srcc.FieldSetId
        _fields = srcc.Fields
    End Sub
End Class

Public Class ChainSrc
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

    Public Sub New(ByVal color As String, ByVal fieldSetId As String, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal name As String, ByVal chainRic As String, Optional ByVal addSrc As Boolean = True)
        MyBase.New(color, fieldSetId, enabled, curve, name)
        _chainRic = chainRic
        If addSrc Then PortMan.AddSource(Me)
    End Sub

    <DisplayName("Chain RIC")>
    Public Property ChainRic As String
        Get
            Return _chainRic
        End Get
        Private Set(ByVal value As String)
            _chainRic = value
        End Set
    End Property

    Public Overrides Function GetDefaultRics() As List(Of String)
        Return _bondsManager.GetChainRics(_chainRic)
    End Function

    Public Overrides Function GetXmlTypeName() As String
        Return "chain"
    End Function

    Public Overrides Function GetXmlPath() As String
        Return "chains/chain"
    End Function

    Protected Overrides Function GenerateId() As String
        Return PortMan.GenerateNewChainId()
    End Function

    Public Shared Function Load(ByVal id As String) As ChainSrc
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
            Return New ChainSrc(id, color, fields, enabled, curve, chainRic, name)
        Catch ex As Exception
            Throw New NoSourceException(String.Format("Failed to find chain with id {0}", id), ex)
        End Try
    End Function

    Protected Overloads Function Equals(ByVal other As ChainSrc) As Boolean
        Return MyBase.Equals(other) AndAlso String.Equals(_chainRic, other._chainRic)
    End Function

    Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
        If ReferenceEquals(Nothing, obj) Then Return False
        If ReferenceEquals(Me, obj) Then Return True
        If obj.GetType IsNot Me.GetType Then Return False
        Return Equals(DirectCast(obj, ChainSrc))
    End Function

    Public Overrides Function GetHashCode() As Integer
        Dim hashCode As Integer = MyBase.GetHashCode
        If _chainRic IsNot Nothing Then
            hashCode = CInt((hashCode * 397L) Mod Integer.MaxValue) Xor _chainRic.GetHashCode
        End If
        Return hashCode
    End Function

    Public Overrides Sub Update(ByVal src As SourceBase)
        MyBase.Update(src)
        If Not TypeOf src Is ChainSrc Then Throw New PortfolioException("Invalid source type")
        Dim ch = CType(src, ChainSrc)
        _chainRic = ch.ChainRic
    End Sub

    Public Shared Shadows Operator =(ByVal left As ChainSrc, ByVal right As ChainSrc) As Boolean
        Return Equals(left, right)
    End Operator

    Public Shared Shadows Operator <>(ByVal left As ChainSrc, ByVal right As ChainSrc) As Boolean
        Return Not Equals(left, right)
    End Operator
End Class

''' <summary>
''' That's a new class intended to fight old and stupid way of how I treat a portfolio
''' It is more tightly linked to XML 
''' The main use case is just call New(id) and have fun
''' The main challenge is that one person can call New(100)
''' And another person can call New(100)
''' And the first does smth and saves it (f.e. kill the portfolio, or deletes some source)
''' And the second doesn't know anything and thus fucks up (i.e. he can behave as though the portfolio still exists)
''' Cures:
'''  1) Event-based Portfolio Manager
'''  2) Careful treatment of all Portfolio operations (any time be ready to find out you are dead =/)
''' </summary>
''' <remarks></remarks>
Public Class PortfolioWrapper
    Private Shared ReadOnly PMan As PortfolioManager = PortfolioManager.ClassInstance
    Private ReadOnly _id As String

    Public Sub New(ByVal id As String, Optional ByVal createIfNotExists As Boolean = False)
        Dim xml = PMan.GetConfigDocument()
        _id = id
        Dim node = xml.SelectSingleNode(String.Format("/bonds/portfolios//portfolio[@id='{0}']", id))
        If node Is Nothing Then
            If createIfNotExists Then
                ' todo create TEMPORARY portfolio
                ' todo introduce concept of TEMPORARY elements which are to be hidden and killed on any load
                ' todo store bonds.xml in zipped format
                ' todo backward compatibility
                Throw New NotImplementedException()
            Else
                Throw New InvalidOperationException(String.Format("Portfolio with ID {0} not found", id))
            End If
        End If
    End Sub

    Public Function TryDeleteSource(ByVal src As SourceBase) As Boolean
        Dim xml = PMan.GetConfigDocument()
        Dim node = xml.SelectSingleNode(String.Format("/bonds/portfolios//portfolio[@id='{0}']/portfolio", _id))
        If node Is Nothing Then Return False
        Dim srcNode = node.SelectSingleNode(String.Format("include[@what='{0}' and @id='{1}'] | exclude[@what='{0}' and @id='{1}']", src.GetXmlTypeName(), src.ID))

        If srcNode IsNot Nothing Then
            node.RemoveChild(srcNode)
            PMan.SaveBonds()
        End If

        Return False
    End Function
End Class

Public Class UserListSrc
    Inherits Source

    Public Sub New(ByVal color As String, ByVal fieldSetId As String, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal name As String, Optional ByVal addSrc As Boolean = True)
        MyBase.New(color, fieldSetId, enabled, curve, name)
        If addSrc Then PortMan.AddSource(Me)
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
        Dim availableRics = New HashSet(Of String)(From row As BondsDataSet.BondRow In BondsLoader.Instance.GetBondsTable().Rows Select row.ric)
        Dim foundRics = ExtractRics(node)
        Dim resultingRics As New List(Of String)
        For Each foundRic In foundRics
            If availableRics.Contains(foundRic) Then
                resultingRics.Add(foundRic)
            Else
                Dim newRic As String
                If foundRic(0) = "/" Then
                    newRic = foundRic.Substring(1)
                Else
                    newRic = "/" + foundRic
                End If
                If availableRics.Contains(newRic) Then
                    resultingRics.Add(newRic)
                Else
                    ' it could well be ok (f.e. in case bond has matured and cease to exist)
                    Logger.Warn("Failed to find both old {0} and updated ric {1}", foundRic, newRic)
                End If
            End If
        Next
        Return resultingRics
    End Function

    Public Overrides Function GetXmlTypeName() As String
        Return "list"
    End Function

    Public Overrides Function GetXmlPath() As String
        Return "lists/list"
    End Function

    Protected Overrides Function GenerateId() As String
        Return PortMan.GenerateNewListId()
    End Function

    Public Shared Function Load(ByVal listId As String) As UserListSrc
        Dim node = PortfolioManager.ClassInstance.GetConfigDocument().SelectSingleNode(String.Format("/bonds/lists/list[@id='{0}']", listId))
        If node Is Nothing Then Throw New NoSourceException(String.Format("Failed to find list with id {0}", listId))
        Try
            Dim color = node.GetAttrStrict("color")
            Dim name = node.GetAttrStrict("name")
            Dim enabled = node.GetAttr("enabled", "True")
            Dim curve = node.GetAttr("curve", "False")
            Dim fields = New FieldSet(node.GetAttrStrict("field-set-id"))
            Return New UserListSrc(listId, color, fields, enabled, curve, name)
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

Public Class UserQuerySrc
    Inherits UserListSrc

    Private _condition As String
    Private _mySource As ChainSrc

    Public Overrides Sub Update(ByVal src As SourceBase)
        MyBase.Update(src)
        If Not TypeOf src Is UserQuerySrc Then Throw New PortfolioException("Invalid source type")
        Dim ch = CType(src, UserQuerySrc)
        _condition = ch.Condition
        _mySource = ch.MySource
    End Sub

    Public Property Condition() As String
        Get
            Return _condition
        End Get
        Friend Set(ByVal value As String)
            _condition = value
        End Set
    End Property

    Public Property MySource() As ChainSrc
        Get
            Return _mySource
        End Get
        Friend Set(ByVal value As ChainSrc)
            _mySource = value
        End Set
    End Property

    Public Sub New(ByVal color As String, ByVal fieldSetId As String, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal name As String, ByVal cond As String, ByVal src As ChainSrc, Optional ByVal addSrc As Boolean = True)
        MyBase.New(color, fieldSetId, enabled, curve, name, False)
        _mySource = src
        _condition = cond
        If addsrc Then PortMan.AddSource(Me)
    End Sub

    Public Sub New(ByVal id As String, ByVal color As String, ByVal fields As FieldSet, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal name As String)
        MyBase.New(id, color, fields, enabled, curve, name)
    End Sub

    Public Sub New(ByVal color As String, ByVal fieldSetId As String, ByVal enabled As Boolean, ByVal curve As Boolean, ByVal name As String)
        MyBase.New(color, fieldSetId, enabled, curve, name)
    End Sub

    Public Overrides Function GetDefaultRics() As List(Of String)
        Dim res As New List(Of String)
        Dim interpreter = New FilterInterpreter(Of BondMetadata)
        Dim filter As Boolean = False

        If _condition = "" Then Return res
        Dim parser As New FilterParser
        Try
            Dim grammar As LinkedList(Of FilterParser.IGrammarElement)
            grammar = parser.SetFilter(_condition)
            interpreter.SetGrammar(grammar)
            filter = True
        Catch ex As Exception
            Logger.ErrorException(String.Format("Failed to parse condition {0}", _condition), ex)
            Logger.Error("Exception = {0}", ex.ToString())
        End Try

        For Each ric In _mySource.GetDefaultRics()
            Dim descr = BondsData.Instance.GetBondInfo(ric)
            If descr IsNot Nothing Then
                If filter Then
                    Try
                        If Not interpreter.Allows(descr) Then Continue For
                    Catch ex As Exception
                        Logger.ErrorException(String.Format("Failed to apply filter to bond {0}", ric), ex)
                        Logger.Error("Exception = {0}", ex.ToString())
                    End Try
                End If
                res.Add(ric)
            Else
                Logger.Warn("No bond {0}", ric)
            End If
        Next
        Return res
    End Function

    Public Overrides Function GetXmlTypeName() As String
        Return "query"
    End Function

    Public Overrides Function GetXmlPath() As String
        Return "queries/query"
    End Function

    Protected Overrides Function GenerateId() As String
        Return MyBase.GenerateId()
    End Function

    Public Shared Function Load(ByVal listId As String) As UserQuerySrc
        Dim node = PortfolioManager.ClassInstance.GetConfigDocument().SelectSingleNode(String.Format("/bonds/queries/query[@id='{0}']", listId))
        If node Is Nothing Then Throw New NoSourceException(String.Format("Failed to find list with id {0}", listId))
        Try
            Dim color = node.GetAttrStrict("color")
            Dim name = node.GetAttrStrict("name")
            Dim enabled = node.GetAttr("enabled", "True")
            Dim cond = node.GetAttr("condition")
            Dim srcId = node.GetAttr("chain-id")
            Dim curve = node.GetAttr("curve", "False")
            Dim fields = New FieldSet(node.GetAttrStrict("field-set-id"))
            Return New UserQuerySrc(listId, color, fields, enabled, curve, name) With {
                .Condition = cond,
                .MySource = ChainSrc.Load(srcId)
            }
        Catch ex As Exception
            Throw New NoSourceException(String.Format("Failed to find list with id {0}", listId), ex)
        End Try
    End Function
End Class

Public Class RegularBondSrc
    Inherits Source

    Private ReadOnly _rics As List(Of String)

    Public ReadOnly Property Rics() As String
        Get
            Return If(_rics Is Nothing, "", String.Join(",", _rics))
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return "Regular bond"
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If obj Is Nothing Then Return False
        If Not TypeOf obj Is RegularBondSrc Then Return False
        Dim hisRics = CType(obj, RegularBondSrc)._rics
        If hisRics.Count <> _rics.Count Then Return False
        Dim i As Long
        For i = 0 To hisRics.Count
            If hisRics(i) <> _rics(i) Then Return False
        Next
        Return True
    End Function

    Public Shared Operator =(ByVal a As RegularBondSrc, ByVal b As RegularBondSrc) As Boolean
        If a Is Nothing Then Return False
        Return a.Equals(b)
    End Operator

    Public Shared Operator <>(ByVal a As RegularBondSrc, ByVal b As RegularBondSrc) As Boolean
        If a Is Nothing Then Return False
        Return a.Equals(b)
    End Operator

    Public Overrides Function GetHashCode() As Integer
        Return String.Join(",", _rics).GetHashCode()
    End Function

    Public Sub New(ByVal id As String, ByVal color As String, ByVal name As String, fsid As String, ByVal rics As String)
        MyBase.New(id, color, New FieldSet(fsid), True, False, name)
        _rics = (From ric In rics.Split(",") Select Trim(ric)).ToList()
    End Sub

    Public Sub New(ByVal color As String, ByVal name As String, ByVal rics As String)
        MyBase.New(Guid.NewGuid().ToString(), color, New FieldSet("MICEX"), True, False, name) ' todo very bad
        _rics = (From ric In rics.Split(",") Select Trim(ric)).ToList()
    End Sub


    Public Overrides Function GetDefaultRics() As List(Of String)
        Dim availableRics = New HashSet(Of String)(From row As BondsDataSet.BondRow In BondsLoader.Instance.GetBondsTable().Rows Select row.ric)
        Dim res As New List(Of String)
        For Each ric In _rics
            If availableRics.Contains(ric) Then
                res.Add(ric)
            Else
                Dim newRic As String
                If _rics(0) = "/" Then
                    newRic = ric.Substring(1)
                Else
                    newRic = "/" + ric
                End If
                If availableRics.Contains(newRic) Then
                    res.Add(newRic)
                Else
                    ' it could well be ok (f.e. in case bond has matured and cease to exist)
                    Logger.Warn("Failed to find both old {0} and updated ric {1}", ric, newRic)
                End If
            End If
        Next
        Return res
    End Function

    Public Overrides Function GetXmlTypeName() As String
        Return "ric"
    End Function

    Public Overrides Function GetXmlPath() As String
        Return ""
    End Function

    Protected Overrides Function GenerateId() As String
        Return ""
    End Function
End Class

Public Class CustomBondSrc
    Inherits SourceBase

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

    Public Overrides Function GetXmlPath() As String
        Return "custom-bonds/bond"
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

    Public Shared Function LoadByCode(ByVal cd As String) As CustomBondSrc
        Dim node = PortfolioManager.ClassInstance.GetConfigDocument().SelectSingleNode(String.Format("/bonds/custom-bonds/bond[@code='{0}']", cd))
        If node Is Nothing Then Throw New NoSourceException(String.Format("Failed to find custom bond with code {0}", cd))
        Try
            Dim color = node.GetAttrStrict("color")
            Dim name = node.GetAttrStrict("name")
            Dim struct = node.GetAttrStrict("bondStructure")
            Dim maturity = node.GetAttrStrict("maturity")
            Dim coupon = node.GetAttrStrict("coupon")
            Dim bndId = node.GetAttrStrict("id")
            Return New CustomBondSrc(bndId, color, name, cd, struct, maturity, coupon)
        Catch ex As Exception
            Throw New NoSourceException(String.Format("Failed to find custom bond with code {0}", cd), ex)
        End Try
    End Function

    Public Shared Function LoadById(ByVal bndId As String) As CustomBondSrc
        Dim node = PortfolioManager.ClassInstance.GetConfigDocument().SelectSingleNode(String.Format("/bonds/custom-bonds/bond[@id='{0}']", bndId))
        If node Is Nothing Then Throw New NoSourceException(String.Format("Failed to find custom bond with id {0}", bndId))
        Try
            Dim color = node.GetAttrStrict("color")
            Dim name = node.GetAttrStrict("name")
            Dim code = node.GetAttrStrict("code")
            Dim struct = node.GetAttrStrict("bondStructure")
            Dim maturity = node.GetAttrStrict("maturity")
            Dim coupon = node.GetAttrStrict("coupon")
            Return New CustomBondSrc(bndId, color, name, code, struct, maturity, coupon)
        Catch ex As Exception
            Throw New NoSourceException(String.Format("Failed to find custom bond with id {0}", bndId), ex)
        End Try
    End Function

    Public Function GetDescription() As BondMetadata
        Dim newDt As Date
        Try
            newDt = ReutersDate.ReutersToDate(_struct.IssueDate)
        Catch ex As Exception
            newDt = Today
        End Try
        Return New BondMetadata(_code, _maturity, _currentCouponRate, _struct.ToString(), "RM:YTM", "Custom", _code, newDt)
    End Function
End Class

Public Class ChainCurveSrc
    Inherits Source

    Private ReadOnly _pattern As String
    Private ReadOnly _ric As String
    Private ReadOnly _skip As String
    Private ReadOnly _type As String
    Private ReadOnly _fields As FieldSet


    Public ReadOnly Property Ric() As String
        Get
            Return _ric
        End Get
    End Property

    Public ReadOnly Property Pattern() As String
        Get
            Return _pattern
        End Get
    End Property

    Public ReadOnly Property Skip() As String
        Get
            Return _skip
        End Get
    End Property

    Public ReadOnly Property Type() As String
        Get
            Return _type
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property FieldsProperty() As FieldSet
        Get
            Return _fields
        End Get
    End Property

    Public Sub New(ByVal id As String, ByVal color As String, ByVal name As String, ByVal pattern As String, ByVal ric As String, ByVal skip As String, ByVal fields As FieldSet, Optional ByVal type As String = "Regular")
        MyBase.New(id, color, fields, True, False, name)
        _pattern = pattern
        _ric = ric
        _skip = skip
        _fields = fields
        _type = type
    End Sub


    Public Sub New(ByVal color As String, ByVal name As String, ByVal pattern As String, ByVal ric As String, ByVal skip As String, ByVal fields As FieldSet, Optional ByVal type As String = "Regular")
        MyBase.New(color, fields.ID, True, False, name)
        _pattern = pattern
        _ric = ric
        _skip = skip
        _fields = fields
        _type = type
    End Sub

    Public Overrides Function GetXmlTypeName() As String
        Return "chain-curves"
    End Function

    Public Overrides Function GetXmlPath() As String
        Return "chain-curves/curve"
    End Function

    Protected Overrides Function GenerateId() As String
        Return PortMan.GenerateNewCustomBondId()
    End Function

    Public Overrides Function GetDefaultRics() As List(Of String)
        Return {_ric}.ToList()
    End Function

    Public Overrides Function ToString() As String
        Return "Chain Curve"
    End Function

    Public Shared Function LoadById(ByVal bndId As String) As ChainCurveSrc
        Dim node = PortfolioManager.ClassInstance.GetConfigDocument().SelectSingleNode(String.Format("/bonds/chain-curves/curve[@id='{0}']", bndId))
        If node Is Nothing Then Throw New NoSourceException(String.Format("Failed to find chain curve with id {0}", bndId))
        Try
            Dim color = node.GetAttrStrict("color")
            Dim name = node.GetAttrStrict("name")
            Dim pattern = node.GetAttrStrict("pattern")
            Dim ric = node.GetAttrStrict("ric")
            Dim skip = node.GetAttr("skip")
            Dim fsId = node.GetAttrStrict("field-set")
            Dim type = node.GetAttr("type", "Regular")
            Return New ChainCurveSrc(bndId, color, name, pattern, ric, skip, New FieldSet(fsId), type)
        Catch ex As Exception
            Throw New NoSourceException(String.Format("Failed to find chain curve with id {0}", bndId), ex)
        End Try
    End Function
End Class