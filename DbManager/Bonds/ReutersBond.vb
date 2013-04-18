Imports System.Text.RegularExpressions
Imports System.Reflection
Imports NLog

Namespace Bonds
    Public Class ReutersDate
        Private ReadOnly _rDate As String
        Private ReadOnly _date As Date

        Private Shared ReadOnly Months As String() = {"JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"}
        Private Shared ReadOnly Pattern As Regex = New Regex("(?<day>\d{2})(?<month>JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)(?<year>\d{2,4})")

        Public Sub New(ByVal dateStr As String)
            _date = ReutersToDate(dateStr)
            _rDate = dateStr
        End Sub

        Public Sub New(ByVal dt As Date)
            _date = dt
            _rDate = ReutersToDate(dt)
        End Sub

        Public ReadOnly Property AsReuters As String
            Get
                Return _rDate
            End Get
        End Property

        Public ReadOnly Property AsDate As Date
            Get
                Return _date
            End Get
        End Property

        Private Shared Function FindMonth(ByVal name As String) As Integer
            Dim i As Integer
            Dim nm = name.ToUpper()
            For i = 0 To UBound(Months)
                If Months(i) = nm Then Return i + 1
            Next
            Return -1
        End Function

        Public Shared Function ReutersToDate(ByVal dt As String) As Date
            Dim match = Pattern.Match(dt)
            If Not match.Success Then Throw New InvalidExpressionException(String.Format("Invalid date {0}", dt))
            Dim day = CInt(match.Groups("day").Value)
            Dim month = FindMonth(match.Groups("month").Value)
            Dim year = CInt(match.Groups("year").Value)
            If year < 99 Then year = year + 2000
            Return New Date(year, month, day)
        End Function

        Public Shared Function DateToReuters(ByVal dt As Date) As String
            Return String.Format("{0:00}{1}{2:0000}", dt.Day, Months(dt.Month - 1), dt.Year)
        End Function
    End Class

    Public MustInherit Class ReutersValueAttribute
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

    Public NotInheritable Class ReutersEmptyAttribute
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

    Public NotInheritable Class ReutersStringAttribute
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

    Public NotInheritable Class ReutersDateAttribute
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

    Public NotInheritable Class ReutersOptionAttribute
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

        Public Property RateProperty() As String
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
            Return GetType(ReutersBondStructure).GetField(Fields(key), BindingFlags.Instance Or BindingFlags.NonPublic).GetValue(Me) IsNot Nothing
        End Function
    End Class
End Namespace