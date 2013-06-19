Imports System.Text.RegularExpressions
Imports System.Reflection
Imports AdfinXAnalyticsFunctions
Imports System.ComponentModel
Imports NLog
Imports Uitls

Namespace Bonds
    Friend MustInherit Class ReutersValueAttribute
        Inherits Attribute
        Private _name As String

        Delegate Sub FieldParser(ByRef rbs As ReutersBondStructure, ByVal keyName As String, ByVal fieldName As String, ByVal whatToParse As String)
        Private ReadOnly _parser As FieldParser

        Public Sub New(ByVal name As String, ByVal parser As FieldParser)
            _name = name
            _parser = parser
        End Sub
        Public Property Name As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public ReadOnly Property Parser As FieldParser
            Get
                Return _parser
            End Get
        End Property

        Public MustOverride Function IntoString(ByVal bond As ReutersBondStructure, ByVal fieldName As String) As String
    End Class

    Friend NotInheritable Class ReutersEmptyAttribute
        Inherits ReutersValueAttribute

        Public Sub New(ByVal name As String)
            MyBase.new(name, AddressOf ParseEmptyVariable)
        End Sub

        Private Shared Sub ParseEmptyVariable(ByRef bond As ReutersBondStructure, ByVal keyName As String, ByVal fieldName As String, ByVal value As String)
            Dim tp = GetType(ReutersBondStructure)
            tp.GetField(fieldName, BindingFlags.NonPublic Or BindingFlags.Instance).SetValue(bond, True)
        End Sub

        Public Overrides Function IntoString(ByVal bond As ReutersBondStructure, ByVal fieldName As String) As String
            Return Name
        End Function
    End Class

    Friend NotInheritable Class ReutersStringAttribute
        Inherits ReutersValueAttribute

        Public Sub New(ByVal name As String)
            MyBase.new(name, AddressOf ParseSimpleVariable)
        End Sub

        Private Shared Sub ParseSimpleVariable(ByRef bond As ReutersBondStructure, ByVal keyName As String, ByVal fieldName As String, ByVal value As String)
            Dim values = value.Split(":")
            If values.Count <> 2 Then
                Throw New InvalidExpressionException(String.Format("Invalid expression for simple variable {0}", value))
            End If
            Dim tp = GetType(ReutersBondStructure)
            Dim fieldInfo = tp.GetField(fieldName, BindingFlags.NonPublic Or BindingFlags.Instance)
            Try
                fieldInfo.SetValue(bond, values(1))
            Catch ex As Exception
                Throw New InvalidExpressionException(String.Format("Failed to save {0} into var {1}", values(1), fieldInfo.Name), ex)
            End Try
        End Sub

        Public Overrides Function IntoString(ByVal bond As ReutersBondStructure, ByVal fieldName As String) As String
            Dim tp = GetType(ReutersBondStructure)
            Dim field = tp.GetField(fieldName, BindingFlags.NonPublic Or BindingFlags.Instance)
            Return Name + ":" + field.GetValue(bond).ToString()
        End Function
    End Class

    Friend NotInheritable Class ReutersDateAttribute
        Inherits ReutersValueAttribute

        Public Sub New(ByVal name As String)
            MyBase.new(name, AddressOf ParseDateVariable)
        End Sub
        Private Shared Sub ParseDateVariable(ByRef bond As ReutersBondStructure, ByVal keyName As String, ByVal fieldName As String, ByVal value As String)
            Dim values = value.Split(":")
            If values.Count <> 3 Then
                Throw New InvalidExpressionException(String.Format("Invalid expression for simple variable {0}", value))
            End If
            Dim dt As Date
            Dim vl As Single
            Try
                dt = CDate(values(1))
                vl = CSng(values(2))
            Catch ex As Exception
                Throw New InvalidExpressionException(String.Format("Invalid format for simple variable {0}", value), ex)
            End Try

            Dim field As FieldInfo
            Dim fieldCurrentValue As List(Of Tuple(Of Date, Single))
            Try
                field = GetType(ReutersBondStructure).GetField(fieldName, BindingFlags.NonPublic Or BindingFlags.Instance)
                fieldCurrentValue = CType(field.GetValue(bond), List(Of Tuple(Of Date, Single)))
            Catch ex As Exception
                Throw New InvalidExpressionException(String.Format("Couldn't find field with name {1} for simple variable {0}", value, fieldName), ex)
            End Try
            fieldCurrentValue.Add(Tuple.Create(dt, vl))
            field.SetValue(bond, fieldCurrentValue)
        End Sub

        Public Overrides Function IntoString(ByVal bond As ReutersBondStructure, ByVal fieldName As String) As String
            Dim tp = GetType(ReutersBondStructure)
            Dim field = tp.GetField(fieldName, BindingFlags.NonPublic Or BindingFlags.Instance)
            Dim val As List(Of Tuple(Of Date, Single)) = CType(field.GetValue(bond), List(Of Tuple(Of Date, Single)))
            Dim res As String = val.Aggregate("",
                Function(current, item) String.Format("{0}{1}:{2}:{3:F2} ", current, Name, ReutersDate.DateToReuters(item.Item1), item.Item2)
            )
            Return res.TrimEnd
        End Function

    End Class

    Friend NotInheritable Class ReutersOptionAttribute
        Inherits ReutersValueAttribute

        Public Sub New(ByVal name As String)
            MyBase.new(name, AddressOf ParseOptionDateArray)
        End Sub

        Private Shared Sub ParseOptionDateArray(ByRef bond As ReutersBondStructure, ByVal keyName As String, ByVal fieldName As String, ByVal value As String)
            Dim values = value.Split(":")
            If values.Count < 3 Then
                Throw New InvalidExpressionException(String.Format("Invalid expression for simple variable {0}", value))
            End If
            Dim dt1 As Date, dt2 As Date
            Dim vl As Single
            Try
                If values.Count = 3 Then
                    dt1 = ReutersDate.ReutersToDate(values(1))
                    dt2 = dt1
                    vl = CSng(values(2))
                Else
                    dt1 = ReutersDate.ReutersToDate(values(1))
                    dt2 = ReutersDate.ReutersToDate(values(2))
                    vl = CSng(values(3))
                End If
            Catch ex As Exception
                Throw New InvalidExpressionException(String.Format("Invalid format for simple variable {0}", value), ex)
            End Try

            Dim field As FieldInfo
            Dim fieldCurrentValue As List(Of Tuple(Of Date, Date, Single))
            Try
                field = GetType(ReutersBondStructure).GetField(fieldName, BindingFlags.NonPublic Or BindingFlags.Instance)
                fieldCurrentValue = CType(field.GetValue(bond), List(Of Tuple(Of Date, Date, Single)))
            Catch ex As Exception
                Throw New InvalidExpressionException(String.Format("Couldn't find field with name {1} for simple variable {0}", value, fieldName), ex)
            End Try
            fieldCurrentValue.Add(Tuple.Create(dt1, dt2, vl))
            field.SetValue(bond, fieldCurrentValue)
        End Sub

        Public Overrides Function IntoString(ByVal bond As ReutersBondStructure, ByVal fieldName As String) As String
            Dim tp = GetType(ReutersBondStructure)
            Dim field = tp.GetField(fieldName, BindingFlags.NonPublic Or BindingFlags.Instance)
            Dim val As List(Of Tuple(Of Date, Date, Single)) = CType(field.GetValue(bond), List(Of Tuple(Of Date, Date, Single)))
            Dim res As String = val.Aggregate("",
                Function(current, item)
                    If item.Item1 = item.Item2 Then
                        Return String.Format("{0}{1}:{2}:{3:F2} ", current, Name, ReutersDate.DateToReuters(item.Item1), item.Item3)
                    Else
                        Return String.Format("{0}{1}:{2}:{3}:{4:F2} ", current, Name, ReutersDate.DateToReuters(item.Item1), ReutersDate.DateToReuters(item.Item2), item.Item3)
                    End If
                End Function
            )
            Return res.TrimEnd
        End Function
    End Class

    Public Class ReutersBondStructure
        Private Shared ReadOnly Attributes As Dictionary(Of String, ReutersValueAttribute)
        Private Shared ReadOnly Fields As Dictionary(Of String, String)
        Private Shared ReadOnly Item As Regex = New Regex("^(?<name>[^:]+)")
        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(ReutersBondStructure))

        <ReutersString("ACC")> Private _accruedInterestCalc As String
        <ReutersString("IC")> Private _irregularFirstCoupon As String
        <ReutersString("CLDR")> Private _calendar As String
        <ReutersString("SETTLE")> Private _settle As String
        <ReutersString("CFADJ")> Private _cashFlowAdj As String
        <ReutersString("DMC")> Private _dateMovingConvention As String
        <ReutersString("EMC")> Private _endMonthConvention As String
        <ReutersString("FRQ")> Private _frequency As String
        <ReutersString("PX")> Private _priceType As String
        <ReutersString("REFDATE")> Private _referenceDate As String
        <ReutersString("YM")> Private _yieldStyleName As String
        <ReutersString("ISSUE")> Private _issueDate As String
        <ReutersString("RATE")> Private _rate As String
        <ReutersDate("AMORT")> Private _amortPattern As New List(Of Tuple(Of Date, Single))
        <ReutersOption("CALL")> Private _callPattern As New List(Of Tuple(Of Date, Date, Single))
        <ReutersOption("PUT")> Private _putPattern As New List(Of Tuple(Of Date, Date, Single))
        <ReutersDate("STEP")> Private _stepCouponPattern As New List(Of Tuple(Of Date, Single))
        <ReutersString("RT")> Private _reimbursementType As String
        Private _bondModule As AdxBondModule

        Private Sub New()
        End Sub

        Shared Sub New()
            Dim items = (From field In GetType(ReutersBondStructure).GetFields(BindingFlags.Instance Or BindingFlags.NonPublic)
                    Let attrs = field.GetCustomAttributes(GetType(ReutersValueAttribute), False)
                    Where attrs.Any()
                    Let attr = CType(attrs(0), ReutersValueAttribute), nm = attr.Name, att = attr, fieldName = field.Name
                    Select New With {.Name = nm, .Attr = att, .FieldName = fieldName}).ToList()
            Attributes = items.ToDictionary(Function(item) item.Name, Function(item) item.Attr)
            Fields = items.ToDictionary(Function(item) item.Name, Function(item) item.FieldName)
        End Sub

        Public Property AccruedInterestCalc() As String
            Get
                Return _accruedInterestCalc
            End Get
            Set(ByVal value As String)
                _accruedInterestCalc = value
            End Set
        End Property

        Public Property IrregularFirstCoupon() As String
            Get
                Return _irregularFirstCoupon
            End Get
            Set(ByVal value As String)
                _irregularFirstCoupon = value
            End Set
        End Property

        Public Property Calendar() As String
            Get
                Return _calendar
            End Get
            Set(ByVal value As String)
                _calendar = value
            End Set
        End Property

        Public Property Settle() As String
            Get
                Return _settle
            End Get
            Set(ByVal value As String)
                _settle = value
            End Set
        End Property

        Public Property CashFlowAdj() As String
            Get
                Return _cashFlowAdj
            End Get
            Set(ByVal value As String)
                _cashFlowAdj = value
            End Set
        End Property

        Public Property DateMovingConvention() As String
            Get
                Return _dateMovingConvention
            End Get
            Set(ByVal value As String)
                _dateMovingConvention = value
            End Set
        End Property

        Public Property EndMonthConvention() As String
            Get
                Return _endMonthConvention
            End Get
            Set(ByVal value As String)
                _endMonthConvention = value
            End Set
        End Property

        Public Property Frequency() As String
            Get
                Return _frequency
            End Get
            Set(ByVal value As String)
                _frequency = value
            End Set
        End Property

        Public Property PriceType() As String
            Get
                Return _priceType
            End Get
            Set(ByVal value As String)
                _priceType = value
            End Set
        End Property

        Public Property ReferenceDate() As String
            Get
                Return _referenceDate
            End Get
            Set(ByVal value As String)
                _referenceDate = value
            End Set
        End Property

        Public Property YieldStyleName() As String
            Get
                Return _yieldStyleName
            End Get
            Set(ByVal value As String)
                _yieldStyleName = value
            End Set
        End Property

        Public Property IssueDate() As String
            Get
                Return _issueDate
            End Get
            Set(ByVal value As String)
                _issueDate = value
            End Set
        End Property

        Public Property Rate() As String
            Get
                Return _rate
            End Get
            Set(ByVal value As String)
                _rate = value
            End Set
        End Property

        Public Property AmortPattern() As List(Of Tuple(Of Date, Single))
            Get
                Return _amortPattern
            End Get
            Set(ByVal value As List(Of Tuple(Of Date, Single)))
                _amortPattern = value
            End Set
        End Property

        Public Property CallPattern() As List(Of Tuple(Of Date, Date, Single))
            Get
                Return _callPattern
            End Get
            Set(ByVal value As List(Of Tuple(Of Date, Date, Single)))
                _callPattern = value
            End Set
        End Property

        Public Property PutPattern() As List(Of Tuple(Of Date, Date, Single))
            Get
                Return _putPattern
            End Get
            Set(ByVal value As List(Of Tuple(Of Date, Date, Single)))
                _putPattern = value
            End Set
        End Property

        Public Property StepCouponPattern() As List(Of Tuple(Of Date, Single))
            Get
                Return _stepCouponPattern
            End Get
            Set(ByVal value As List(Of Tuple(Of Date, Single)))
                _stepCouponPattern = value
            End Set
        End Property

        Public Property ReimbursementType() As String
            Get
                Return _reimbursementType
            End Get
            Set(ByVal value As String)
                _reimbursementType = value
            End Set
        End Property

        Public Sub Load(ByVal str As String)
            Dim items = str.Split(" ")
            ClearArrays()
            For Each elem In items
                Dim element = elem.Trim.ToUpper()
                If element = "" Then Continue For
                Dim match = Item.Match(element)
                If Not match.Success Then
                    Logger.Warn(String.Format("Failed to parse item {0}", element))
                    Continue For
                End If

                Dim name = match.Groups("name").Value
                If name = String.Empty Then
                    Logger.Warn(String.Format("Failed to extract name from {0}", element))
                    Continue For
                ElseIf Not Attributes.Keys.Contains(name) Then
                    Logger.Warn(String.Format("Unexpected token {0}", name))
                    Continue For
                End If

                Dim rva = Attributes(name)
                Dim parser = rva.Parser()
                parser(Me, rva.Name, Fields(name), element)
            Next
        End Sub

        Private Sub ClearArrays()
            _callPattern.Clear()
            _putPattern.Clear()
            _stepCouponPattern.Clear()
            _amortPattern.Clear()
        End Sub

        Public Shared Function Parse(ByVal str As String) As ReutersBondStructure
            Dim res As New ReutersBondStructure
            Dim items = str.Split(" ")
            For Each elem In items
                Dim element = elem.Trim.ToUpper()
                If element = "" Then Continue For
                Dim match = Item.Match(element)
                If Not match.Success Then
                    Logger.Warn(String.Format("Failed to parse item {0}", element))
                    Continue For
                End If

                Dim name = match.Groups("name").Value
                If name = String.Empty Then
                    Logger.Warn(String.Format("Failed to extract name from {0}", element))
                    Continue For
                ElseIf Not Attributes.Keys.Contains(name) Then
                    Logger.Warn(String.Format("Unexpected token {0}", name))
                    Continue For
                End If

                Dim rva = Attributes(name)
                Dim parser = rva.Parser()
                parser(res, rva.Name, Fields(name), element)
            Next
            Return res
        End Function

        Public Overrides Function ToString() As String
            Return Attributes.Keys.Where(AddressOf FieldNotNothing).
                Aggregate("", Function(current, name) current + Attributes(name).IntoString(Me, Fields(name)) + " ")
        End Function

        Private Function FieldNotNothing(ByVal key As String) As Boolean
            Dim field = GetType(ReutersBondStructure).GetField(Fields(key), BindingFlags.Instance Or BindingFlags.NonPublic)
            Dim empty = field.GetCustomAttributes(GetType(ReutersEmptyAttribute), False).Any()
            Dim value = field.GetValue(Me)

            If value IsNot Nothing AndAlso Not empty AndAlso TypeOf value Is String Then
                Return CStr(value) <> ""
            Else
                Return value IsNot Nothing
            End If
        End Function

        Public Function GetCashFlows(ByVal matDate As String, ByVal couponRate As Double) As List(Of CashFlowDescription)
            Dim cashFlows As Array = _bondModule.BdCashflows(Today, matDate, couponRate, ToString(), "IAC RET:A100")
            Dim res As New List(Of CashFlowDescription)
            Dim i As Integer
            For i = cashFlows.GetLowerBound(0) To cashFlows.GetUpperBound(0)
                If cashFlows.GetValue(i, 1) Is Nothing Then Exit For
                Try
                    Dim dt = Utils.FromExcelSerialDate(cashFlows.GetValue(i, 1))
                    Dim cpn = CDbl(cashFlows.GetValue(i, 2))
                    Dim amort = CDbl(cashFlows.GetValue(i, 3))
                    res.Add(New CashFlowDescription(dt, cpn, amort))
                Catch ex As Exception
                    Logger.WarnException(String.Format("Failed to read {0}th row in result", i), ex)
                    Logger.Warn("Exception = {0}", ex.ToString())
                End Try
            Next
            Return res
        End Function

        Public Class CashFlowDescription
            Private ReadOnly _dt As Date
            Private ReadOnly _cpn As Double
            Private ReadOnly _amort As Double

            <DisplayName("Date")>
            Public ReadOnly Property Dt() As String
                Get
                    Return String.Format("{0:dd/MMM/yyyy}", _dt)
                End Get
            End Property

            <DisplayName("Coupon")>
            Public ReadOnly Property Cpn() As String
                Get
                    Return String.Format("{0:F4}", _cpn)
                End Get
            End Property

            <DisplayName("Redemption")>
            Public ReadOnly Property Amort() As String
                Get
                    Return String.Format("{0:F4}", _amort)
                End Get
            End Property

            Public Sub New(ByVal dt As Date, ByVal cpn As Double, ByVal amort As Double)
                _dt = dt
                _cpn = cpn
                _amort = amort
            End Sub
        End Class

        Public Class CouponDescription
            Private ReadOnly _dt As Date
            Private ReadOnly _rate As Single

            Public Sub New(ByVal dt As Date, ByVal rate As Single)
                _dt = dt
                _rate = rate
            End Sub

            <DisplayName("Date")>
            Public ReadOnly Property Dt() As String
                Get
                    Return String.Format("{0:dd/MMM/yyyy}", _dt)
                End Get
            End Property

            <DisplayName("Coupon rate")>
            Public ReadOnly Property Rate() As String
                Get
                    Return String.Format("{0:F4}", _rate)
                End Get
            End Property

            <Browsable(False)>
            Friend ReadOnly Property DtFriend() As Date
                Get
                    Return _dt
                End Get
            End Property

            <Browsable(False)>
            Friend ReadOnly Property RateFriend() As Single
                Get
                    Return _rate
                End Get
            End Property
        End Class

        Public Class AmortizationDescription
            Private ReadOnly _dt As Date
            Private ReadOnly _amount As Single

            Public Sub New(ByVal dt As Date, ByVal amount As Single)
                _dt = dt
                _amount = amount
            End Sub

            <DisplayName("Date")>
            Public ReadOnly Property Dt() As String
                Get
                    Return String.Format("{0:dd/MMM/yyyy}", _dt)
                End Get
            End Property

            Public ReadOnly Property Amount() As String
                Get
                    Return String.Format("{0:F4}", _amount)
                End Get
            End Property

            <Browsable(False)>
            Friend ReadOnly Property DtFriend() As Date
                Get
                    Return _dt
                End Get
            End Property

            <Browsable(False)>
            Friend ReadOnly Property AmountFriend() As Single
                Get
                    Return _amount
                End Get
            End Property
        End Class

        Public Class EmbdeddedOptionDescription
            Private ReadOnly _type As String
            Private ReadOnly _kind As String
            Private ReadOnly _starts As Date
            Private ReadOnly _ends As Date
            Private ReadOnly _price As Single

            Public ReadOnly Property Type() As String
                Get
                    Return _type
                End Get
            End Property

            Public ReadOnly Property Kind() As String
                Get
                    Return _kind
                End Get
            End Property

            Public ReadOnly Property Starts() As String
                Get
                    Return String.Format("{0:dd/MMM/yyyy}", _starts)
                End Get
            End Property

            Public ReadOnly Property Ends() As String
                Get
                    Return String.Format("{0:dd/MMM/yyyy}", _ends)
                End Get
            End Property

            Public ReadOnly Property Price() As String
                Get
                    Return String.Format("{0:F4}", _price)
                End Get
            End Property

            Public Sub New(ByVal elem As Tuple(Of Date, Date, Single), ByVal type As String)
                _type = type
                _kind = If(elem.Item1 = elem.Item2, "European", "American")
                _starts = elem.Item1
                _ends = elem.Item2
                _price = elem.Item3
            End Sub

            Friend ReadOnly Property StartsFriend() As Date
                Get
                    Return _starts
                End Get
            End Property

            Friend ReadOnly Property EndsFriend() As Date
                Get
                    Return _ends
                End Get
            End Property

            Friend ReadOnly Property PriceFriend() As Single
                Get
                    Return _price
                End Get
            End Property

        End Class

        Public Sub SetBondModule(ByVal adxBondModule As AdxBondModule)
            _bondModule = adxBondModule
        End Sub

        Public Function HasSingleFixedRate() As Boolean
            Return (_stepCouponPattern.Count() = 0) Or (_rate <> "" And _stepCouponPattern.Count() <= 1)
        End Function

        Public Function GetFixedRate() As Single
            If _rate <> "" Then Return CSng(_rate)
            If _stepCouponPattern.Any Then Return CSng(_stepCouponPattern(0).Item2)
            Return 0
        End Function

        Public Function GetCouponsList() As List(Of CouponDescription)
            Return (From elem In _stepCouponPattern Select New CouponDescription(elem.Item1, elem.Item2)).ToList()
        End Function

        Public Function HasAmortizationSchedule() As Boolean
            Return _amortPattern.Count > 0
        End Function

        Public Function GetAmortizationSchedule() As List(Of AmortizationDescription)
            Return (From elem In _amortPattern Select New AmortizationDescription(elem.Item1, elem.Item2)).ToList()
        End Function

        Public Function IsPerpetual() As Boolean
            Return _reimbursementType = "P"
        End Function

        Public Function IsAnnuity() As Boolean
            Return _reimbursementType = "C"
        End Function

        Public Function GetEmbeddedOptions() As List(Of EmbdeddedOptionDescription)
            Dim res As New List(Of EmbdeddedOptionDescription)
            res.AddRange(From elem In _callPattern Select New EmbdeddedOptionDescription(elem, "Call"))
            res.AddRange(From elem In _putPattern Select New EmbdeddedOptionDescription(elem, "Put"))
            Return res
        End Function

        Public Sub DeleteAmortizationItem(ByVal ad As AmortizationDescription)
            _amortPattern.Remove(Tuple.Create(ad.DtFriend, ad.AmountFriend))
        End Sub

        Public Sub DeleteCouponItem(ByVal cd As CouponDescription)
            _stepCouponPattern.Remove(Tuple.Create(cd.DtFriend, cd.RateFriend))
        End Sub

        Public Sub DeleteOptionItem(ByVal eod As EmbdeddedOptionDescription)
            If eod.Type = "Call" Then
                _callPattern.Remove(Tuple.Create(eod.StartsFriend, eod.EndsFriend, eod.PriceFriend))
            Else
                _putPattern.Remove(Tuple.Create(eod.StartsFriend, eod.EndsFriend, eod.PriceFriend))
            End If
        End Sub
    End Class
End Namespace