Imports EikonDesktopSDKLib
Imports NLog
Imports System.Reflection

Namespace Tools.RDataTool
    Public Class BondsDataQuery
        Inherits RQuery

        Private Shared ReadOnly Logger As Logger = Commons.GetLogger(GetType(BondsDataQuery))

        Public Sub New(ByVal newBondRICs As List(Of String))
            Logger.Trace("New()")
            Dim theFields As String = BondStructure.MetaData.GetFieldList
            AddTask(String.Join(";", newBondRICs), theFields, "", AddressOf OnDataArrived)
        End Sub

        Private Sub OnDataArrived(data As Array, err As String)
            If data IsNot Nothing Then
                Logger.Debug("OnDataArrived({0})", data.ToString())
                Dim res As New Dictionary(Of String, BondStructure)()
                For i As Integer = data.GetLowerBound(0) + 1 To data.GetUpperBound(0)
                    Dim ric As String = data.GetValue(i, 0)
                    Dim numFields As Integer = BondStructure.MetaData.GetNumFields
                    Dim dataArr(numFields) As String
                    For j = 0 To numFields
                        dataArr(j) = data.GetValue(i, j + 1)
                    Next

                    Dim struct As BondStructure = BondStructure.MetaData.ParseDataRow(dataArr)
                    res.Add(ric, struct)
                    Logger.Trace("got bond {0}", struct.ToString())
                Next
                NotifyDataParsed(New BondEventArgs(res), Nothing)
            Else
                Logger.Warn("OnDataArrived({0})", err)
                NotifyDataParsed(Nothing, err)
            End If
        End Sub
    End Class

    Public Class BondEventArgs
        Inherits EventArgs

        Private ReadOnly _data As Dictionary(Of String, BondStructure)

        Sub New(ByVal data As Dictionary(Of String, BondStructure))
            _data = data
        End Sub

        Public ReadOnly Property Data() As Dictionary(Of String, BondStructure)
            Get
                Return _data
            End Get
        End Property
    End Class

    Public Class BondFieldAttribute
        Inherits Attribute

        Public Show As Boolean
        Public Dex2Name As String
        Public ShowInDescription As Boolean

        Public Sub New(dex2Name As String, Optional show As Boolean = False, Optional showInDescription As Boolean = False)
            Me.Show = show
            Me.Dex2Name = dex2Name
            Me.ShowInDescription = showInDescription
        End Sub
    End Class

    Public Class BondStructure
#Region "Major Bond Fields"
        <BondField("EJV.X.ADF_BondStructure")> Public Payments As String
        <BondField("EJV.X.ADF_RateStructure")> Public Rates As String
        <BondField("EJV.X.ADF_Coupon", True)> Public Coupon As String
        <BondField("EJV.C.Description", True, True)> Public Description As String
        <BondField("EJV.C.IssuerName")> Public IssuerName As String
        <BondField("EJV.C.ShortName")> Public ShortName As String
        <BondField("EJV.C.Ticker")> Public Ticker As String
        <BondField("EJV.C.IndexRIC")> Public IndexRIC As String
        <BondField("EJV.C.IssueDate")> Public IssueDate As String
        <BondField("EJV.C.MaturityDate")> Public MaturityDate As String
        <BondField("EJV.C.NextPutDate")> Public NextPutDate As String
        <BondField("EJV.C.IssuerCountry")> Public Country As String
        <BondField("EJV.C.Currency")> Public Currency As String
        <BondField("EJV.C.IsStraight")> Public IsStraight As String
        <BondField("EJV.C.IsPutable")> Public IsPutable As String
        <BondField("EJV.C.IsFloater")> Public IsFloater As String
        <BondField("EJV.C.IsConvertible")> Public IsConvertible As String
        <BondField("EJV.C.OriginalAmountIssued")> Public OriginalAmountIssued As String
        <BondField("EJV.C.Series", True)> Public Series As String
#End Region

        Public Overrides Function ToString() As String
            Dim res = (
                From fieldInfo In Me.GetType().GetFields()
                Let attrs = FieldInfo.GetCustomAttributes(GetType(BondFieldAttribute), False)
                Where attrs.Any()
                Let ba = attrs.First()
                Where CType(ba, BondFieldAttribute).ShowInDescription
                Select FieldInfo.GetValue(Me)
            ).Cast(Of String).ToList()

            Return If(res.any(), String.Join("; ", res.ToArray()), Description)
        End Function

        Public Function GetColumnNameAndItsValue() As IEnumerable(Of KeyValuePair(Of String, String))
            Dim res As New List(Of KeyValuePair(Of String, String))

            For Each info As FieldInfo In Me.GetType().GetFields()
                Dim attrs = info.GetCustomAttributes(GetType(BondFieldAttribute), False)
                If attrs.Any() Then
                    If info.GetValue(Me) IsNot Nothing Then
                        res.Add(New KeyValuePair(Of String, String)(info.Name, info.GetValue(Me).ToString))
                    Else
                        res.Add(New KeyValuePair(Of String, String)(info.Name, ""))
                    End If
                End If
            Next

            Return res
        End Function

        ''' <summary>
        ''' Class dedicated to handle metadata from its parent, namely BondStructure.
        ''' This class contains number of useful tools to leverage profit from using BondField Attribute
        ''' </summary>
        ''' <remarks></remarks>
        Public Class MetaData
            Private Shared ReadOnly Dex2Names() As String = New String() {}
            Private Shared ReadOnly VarNames() As String = New String() {}

            Shared Sub New()
                Dim n As Integer = 0
                For Each info As FieldInfo In GetType(BondStructure).GetFields()
                    Dim attrs = info.GetCustomAttributes(GetType(BondFieldAttribute), False)
                    If attrs.Any() Then
                        ReDim Preserve Dex2Names(0 To n)
                        ReDim Preserve VarNames(0 To n)
                        Dex2Names(n) = DirectCast(attrs.First(), BondFieldAttribute).Dex2Name
                        VarNames(n) = info.Name
                        n = n + 1
                    End If
                Next
            End Sub

            Public Shared Function GetFieldList() As String
                Return String.Join(";", Dex2Names)
            End Function

            Public Shared Function GetNumFields() As Integer
                Return Dex2Names.Count() - 1
            End Function

            Public Shared Function ParseDataRow(ByVal dataArr As String()) As BondStructure
                Dim res As New BondStructure
                For i = 0 To GetNumFields()
                    If dataArr(i) IsNot Nothing Then
                        Call GetType(BondStructure).GetField(VarNames(i)).SetValue(res, dataArr(i))
                    Else
                        Call GetType(BondStructure).GetField(VarNames(i)).SetValue(res, "")
                    End If
                Next
                Return res
            End Function

            ''' <summary>
            ''' Describes datagrid to be shown to user
            ''' </summary>
            ''' <returns> 
            '''  Set of pairs Key => Value where
            '''     Key is name of column in data grid
            '''     Value is boolean, describing if this column to be shown to user 
            ''' </returns>
            ''' <remarks></remarks>
            Public Shared Function GetBondFieldDescr() As IEnumerable(Of KeyValuePair(Of String, Boolean))
                Return (From fieldInfo In GetType(BondStructure).GetFields()
                    Let attrs = fieldInfo.GetCustomAttributes(GetType(BondFieldAttribute), False)
                    Where attrs.Any()
                    Select New KeyValuePair(Of String, Boolean)(fieldInfo.Name, DirectCast(attrs.First(), BondFieldAttribute).Show)).ToList()
            End Function
        End Class
    End Class
End Namespace