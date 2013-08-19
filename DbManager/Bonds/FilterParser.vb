Imports System.Text.RegularExpressions
Imports System.Reflection
Imports System.Globalization

Namespace Bonds
    Public Class FilterParser
        Private Const LogicalOpPriority = 1
        Private Const BinaryOpPriority = 2
        Private Const BracketsPriority = 10

        Private ReadOnly _opStack As New LinkedList(Of Operation)

        Private Shared ReadOnly VarName1 As Regex = New Regex("^\s*?\$(?<varname>\w+\.\w+|\w+)")
        Private Shared ReadOnly ObjName1 As Regex = New Regex("^\s*?\$(?<objname>\w+)\.(?<fieldname>\w+)")
        Private Shared ReadOnly LogOp As Regex = New Regex("^\s*?(?<lop>AND|OR)")
        Private Shared ReadOnly BinOp As Regex = New Regex("^\s*?(?<bop>\<=|\>=|=|\<\>|\<|\>|like|nlike)")
        Private Shared ReadOnly NumValue As Regex = New Regex("^\s*?(?<num>-?\d+.\d+|-?\d+)")
        Private Shared ReadOnly BoolValue As Regex = New Regex("^\s*?(?<bool>True|False)")
        Private Shared ReadOnly StrValue As Regex = New Regex("^\s*?""(?<str>[^""]*)""")
        Private Shared ReadOnly DatValue As Regex = New Regex("^\s*?#(?<dd>\d{1,2})/(?<mm>\d{1,2})/(?<yy>\d{2}|\d{4})#")
        Private Shared ReadOnly RatingValue As Regex = New Regex("^\s*?\[(?<rating>[^\]]*)\]")

        Private Enum ParserState
            Expr
            Term
            Bop
            Lop
            Name
            Value
        End Enum

        Private _state As ParserState = ParserState.Expr
        Private _filterString As String

        Public Interface IGrammarElement
        End Interface

        Public MustInherit Class Operation
            Implements IGrammarElement

            Private ReadOnly _priority As Integer

            Sub New(ByVal priority As Integer)
                _priority = priority
            End Sub

            Public ReadOnly Property Priority As Integer
                Get
                    Return _priority
                End Get
            End Property
        End Class

        Public Class Lop
            Inherits Operation

            Public Enum LogicalOperation
                OpAnd
                OpOr
            End Enum

            Private ReadOnly _logicalOperation As LogicalOperation

            Public Overrides Function ToString() As String
                Return If(_logicalOperation = LogicalOperation.OpAnd, "And", "Or")
            End Function

            Sub New(ByVal logicalOperation As String, ByVal priority As Integer)
                MyBase.new(priority)
                If logicalOperation.ToUpper() = "AND" Then
                    _logicalOperation = Lop.LogicalOperation.OpAnd
                ElseIf logicalOperation.ToUpper() = "OR" Then
                    _logicalOperation = Lop.LogicalOperation.OpOr
                Else
                    Throw New ConditionLexicalException(String.Format("Invalid logical operation {0}", logicalOperation))
                End If
            End Sub

            Public ReadOnly Property LogOperation As LogicalOperation
                Get
                    Return _logicalOperation
                End Get
            End Property
        End Class

        Public Class Bop
            Inherits Operation

            Public Enum BinaryOperation
                OpEquals
                OpGreater
                OpLess
                OpGreaterOrEquals
                OpLessOrEquals
                OpNotEqual
                OpLike
                OpNLike
            End Enum

            Private ReadOnly _binaryOperation As BinaryOperation

            Public Overrides Function ToString() As String
                Dim res As String = ""
                Select Case _binaryOperation
                    Case BinaryOperation.OpEquals : res = "="
                    Case BinaryOperation.OpGreater : res = ">"
                    Case BinaryOperation.OpLess : res = "<"
                    Case BinaryOperation.OpGreaterOrEquals : res = ">="
                    Case BinaryOperation.OpLessOrEquals : res = "<="
                    Case BinaryOperation.OpNotEqual : res = "<>"
                    Case BinaryOperation.OpLike : res = "like"
                    Case BinaryOperation.OpLike : res = "nlike"
                End Select
                Return res
            End Function

            Sub New(ByVal binaryOperation As String, ByVal priority As Integer)
                MyBase.new(priority)
                Select Case binaryOperation
                    Case "=" : _binaryOperation = Bop.BinaryOperation.OpEquals
                    Case "<>" : _binaryOperation = Bop.BinaryOperation.OpNotEqual
                    Case ">" : _binaryOperation = Bop.BinaryOperation.OpGreater
                    Case "<" : _binaryOperation = Bop.BinaryOperation.OpLess
                    Case ">=" : _binaryOperation = Bop.BinaryOperation.OpGreaterOrEquals
                    Case "<=" : _binaryOperation = Bop.BinaryOperation.OpLessOrEquals
                    Case "like" : _binaryOperation = Bop.BinaryOperation.OpLike
                    Case "nlike" : _binaryOperation = Bop.BinaryOperation.OpNLike
                    Case Else : Throw New ConditionLexicalException(String.Format("Invalid binary operation {0}", binaryOperation))
                End Select
            End Sub

            Public ReadOnly Property BinOperation As BinaryOperation
                Get
                    Return _binaryOperation
                End Get
            End Property
        End Class

        Public Class Var
            Implements IGrammarElement
            Private ReadOnly _name As String

            Public Overrides Function ToString() As String
                Return _name
            End Function

            Sub New(ByVal name As String)
                _name = name
            End Sub

            Public ReadOnly Property Name As String
                Get
                    Return _name
                End Get
            End Property

        End Class

        Public Class ObjVar
            Inherits Var
            Private ReadOnly _fieldname As String

            Public ReadOnly Property FullName() As String
                Get
                    Return String.Format("{0}.{1}", Name, _fieldname)
                End Get
            End Property

            Public Overrides Function ToString() As String
                Return FullName
            End Function

            Sub New(ByVal name As String, ByVal fieldname As String)
                MyBase.new(name)
                _fieldname = fieldname
            End Sub

            Public ReadOnly Property FieldName As String
                Get
                    Return _fieldname
                End Get
            End Property
        End Class

        Public Interface IVal
            Inherits IGrammarElement
        End Interface

        Public Class Val(Of T)
            Implements IVal
            Private ReadOnly _value As T

            Sub New(ByVal value As T)
                _value = value
            End Sub

            Public Overrides Function ToString() As String
                Return _value.ToString()
            End Function

            Public ReadOnly Property Value As T
                Get
                    Return _value
                End Get
            End Property
        End Class

        Public ReadOnly Property FilterString As String
            Get
                Return _filterString
            End Get
        End Property

        Public Function SetFilter(ByVal fltStr As String) As LinkedList(Of IGrammarElement)
            fltStr = fltStr.Trim()
            _filterString = fltStr
            Dim res As LinkedList(Of IGrammarElement)
            Try
                _opStack.Clear()
                _state = ParserState.Expr

                Dim ind As Integer
                res = ParseFilterString(fltStr, ind, 0)
                If ind < fltStr.Length() Then Throw New ParserException("Parsing not finished", ind)
            Catch ex As ParserException
                If ex.FilterStr = "" Then ex.FilterStr = fltStr
                Throw
            End Try
            Return res
        End Function

        Friend Function GetStack(ByVal grammar As LinkedList(Of IGrammarElement)) As List(Of String)
            Dim list = New List(Of String)
            If grammar.Any Then
                Dim lnk As LinkedListNode(Of IGrammarElement) = grammar.First
                Do
                    list.Add(String.Format("[{0}] ", lnk.Value.ToString()))
                    lnk = lnk.Next
                Loop While lnk IsNot Nothing
            End If
            Return list
        End Function

        '=========================================================================================
        '
        '   GRAMMAR LOOKS LIKE THIS
        '   --------------------------------------------------------------------------------------
        '   <EXPR>          ::= <BR_EXPR>|<TERM>|<TERM_CHAIN>
        '   <BR_EXPR>       ::= (<EXPR>)
        '   <LOP>           ::= AND|OR
        '   <TERM_CHAIN>    ::= <TERM> <LOP> <TERM> | <TERM> <LOP> <EXPR>
        '   <TERM>          ::= <NAME> <OP> <VALUE>
        '   <OP>            ::= =|>|<|>=|<=
        '   <NAME>          ::= ${<LETTERS>}
        '   <VALUE>         ::= <STRVAL>|<DATVAL>|<NUMVAL>|<RATEVAL>
        '
        '=========================================================================================
        Private Function ParseFilterString(ByVal fltStr As String, ByRef endIndex As Integer, ByVal bracketsLevel As Integer) As LinkedList(Of IGrammarElement)
            Dim i As Integer = 0
            Dim res As New LinkedList(Of IGrammarElement)

            While i < fltStr.Length
                ' SKIP EMPTY SPACES
                While fltStr(i) = " "
                    i = i + 1
                End While

                Dim match As Match
                Select Case _state
                    Case ParserState.Expr
                        'Console.WriteLine("--> EXPR")
                        If fltStr(i) = "(" Then
                            ' BR_EXPR
                            Dim ind As Integer
                            Dim elems As LinkedList(Of IGrammarElement) = Nothing
                            Try
                                elems = ParseFilterString(fltStr.Substring(i + 1), ind, bracketsLevel + 1)
                            Catch ex As ParserException
                                If bracketsLevel > 0 Then
                                    ex.ErrorPos = ex.ErrorPos + i + 1
                                    Throw
                                End If
                            End Try

                            If elems Is Nothing OrElse Not elems.Any Then
                                Throw New ParserException("Invalid expression in brackets", i)
                            End If

                            ' ReSharper disable VBWarnings::BC42104 ' it's ok, if nothing exception is thrown, see above
                            Dim elem = elems.First
                            ' ReSharper restore VBWarnings::BC42104
                            Do
                                res.AddLast(elem.Value)
                                elem = elem.Next
                            Loop Until elem Is Nothing

                            i = i + ind + 1
                        ElseIf fltStr(i) = ")" Then
                            ' END OF CURRENT BR_EXPR
                            endIndex = i + 1
                            Return res
                        ElseIf fltStr(i) = "$" Then
                            ' NAME
                            _state = ParserState.Term
                        Else
                            Throw New ParserException("Unexpected symbol, brackets or variable name required", i)
                        End If

                    Case ParserState.Term
                        'Console.WriteLine("--> TERM")
                        If fltStr(i) = "$" Then
                            _state = ParserState.Name
                        ElseIf fltStr(i) = ")" Then
                            ' END OF CURRENT BR_EXPR
                            endIndex = i + 1
                            Return res
                        Else
                            _state = ParserState.Lop
                        End If

                    Case ParserState.Name
                        'Console.WriteLine("--> NAME")
                        If fltStr(i) <> "$" Then
                            Throw New ParserException("Unexpected symbol, variable name required", i)
                        End If

                        ' Reading variable name
                        Dim match1 = VarName1.Match(fltStr.Substring(i))
                        Dim match2 = ObjName1.Match(fltStr.Substring(i))
                        If match1.Success Then
                            Dim variableName = match1.Groups("varname").Captures(0).Value
                            Dim node As New Var(variableName.ToUpper())
                            i = i + match1.Length + 1
                            res.AddLast(node)
                        ElseIf match2.Success Then
                            Dim objName = match1.Groups("objname").Captures(0).Value
                            Dim fieldName = match1.Groups("fieldname").Captures(0).Value
                            Dim node As New ObjVar(objName.ToUpper(), fieldName.ToUpper())
                            i = i + match1.Length + 1
                            res.AddLast(node)
                        Else
                            Throw New ParserException("Unexpected sequence, variable name required", i)
                        End If
                        _state = ParserState.Bop

                    Case ParserState.Bop
                        'Console.WriteLine("--> BOP")
                        ' Reading binary operation
                        match = BinOp.Match(fltStr.Substring(i).ToLower())
                        Dim opNode As Bop
                        If match.Success Then
                            Dim opName = match.Groups("bop").Captures(0).Value
                            Try
                                opNode = New Bop(opName, BinaryOpPriority + bracketsLevel * BracketsPriority)
                                i = i + match.Length + 1
                                PushToOpStack(res, opNode)
                            Catch ex As ConditionLexicalException
                                Throw New ParserException("Unexpected error in binary operation", ex, i)
                            End Try

                        Else
                            Throw New ParserException("Unexpected sequence, binary operation (>/</=/<>/>=/<=/like) required", i)
                        End If
                        _state = ParserState.Value

                    Case ParserState.Value
                        'Console.WriteLine("--> VALUE")
                        Dim valNode As IGrammarElement
                        ' Reading value
                        If fltStr(i) = """" Then          ' string value
                            match = StrValue.Match(fltStr.Substring(i))
                            If match.Success Then
                                Dim str = match.Groups("str").Captures(0).Value
                                valNode = New Val(Of String)(str)
                                i = i + match.Length + 1
                            Else
                                Throw New ParserException("Unexpected sequence, string expression required", i)
                            End If
                        ElseIf IsNumeric(fltStr(i)) Then ' number
                            match = NumValue.Match(fltStr.Substring(i))
                            If match.Success Then
                                Dim num = match.Groups("num").Captures(0).Value
                                If Not IsNumeric(num) Then Throw New ParserException("Invalid number", i)
                                valNode = New Val(Of Double)(num)
                                i = i + match.Length + 1
                            Else
                                Throw New ParserException("Unexpected sequence, string expression required", i)
                            End If
                        ElseIf fltStr(i) = "#" Then       ' date
                            match = DatValue.Match(fltStr.Substring(i))
                            If match.Success Then
                                Dim dd = match.Groups("dd").Captures(0).Value
                                Dim mm = match.Groups("mm").Captures(0).Value
                                Dim yy = match.Groups("yy").Captures(0).Value
                                Dim dt As New Date(yy, mm, dd)
                                valNode = New Val(Of Date)(dt)
                                i = i + match.Length + 1
                            Else
                                Throw New ParserException("Unexpected sequence, date expression required", i)
                            End If
                        ElseIf fltStr(i) = "[" Then       ' Rating
                            match = RatingValue.Match(fltStr.Substring(i))
                            If match.Success Then
                                Dim rate = match.Groups("rating").Captures(0).Value
                                Dim rt = Rating.Parse(rate)
                                valNode = New Val(Of Rating)(rt)
                                i = i + match.Length + 1
                            Else
                                Throw New ParserException("Unexpected sequence, rating expression required", i)
                            End If
                        ElseIf {"T", "F"}.Contains(fltStr.ToUpper()(i)) Then
                            match = BoolValue.Match(fltStr.Substring(i))
                            If match.Success Then
                                Dim bool = match.Groups("bool").Captures(0).Value
                                Try
                                    valNode = New Val(Of Boolean)(Boolean.Parse(bool))
                                Catch ex As FormatException
                                    Throw New ParserException("Unexpected sequence, boolean (True/False) expression required", i)
                                End Try
                                i = i + match.Length + 1
                            Else
                                Throw New ParserException("Unexpected sequence, rating expression required", i)
                            End If
                        Else
                            Throw New ParserException("Unexpected symbol, string, date or number required", i)
                        End If
                        res.AddLast(valNode)

                        _state = ParserState.Term

                    Case ParserState.Lop
                        'Console.WriteLine("--> LOP")
                        match = LogOp.Match(fltStr.Substring(i).ToUpper())
                        Dim opNode As Lop
                        If match.Success Then
                            Try
                                Dim num = match.Groups("lop").Captures(0).Value
                                opNode = New Lop(num, LogicalOpPriority + bracketsLevel * BracketsPriority)
                                PushToOpStack(res, opNode)
                                i = i + match.Length + 1
                            Catch ex As ConditionLexicalException
                                Throw New ParserException("Unexpected error in logical operation", ex, i)
                            End Try
                        Else
                            Throw New ParserException("Unexpected sequence, logical expression required", i)
                        End If
                        _state = ParserState.Expr
                End Select
            End While
            FlushOpStack(res, bracketsLevel * BracketsPriority)
            endIndex = i
            Return res
        End Function

        Private Sub FlushOpStack(ByRef res As LinkedList(Of IGrammarElement), ByVal priority As Integer)
            While _opStack.Any() AndAlso _opStack.First().Value.Priority > priority
                res.AddLast(_opStack.First.Value)
                _opStack.RemoveFirst()
            End While
        End Sub

        Private Sub PushToOpStack(ByRef res As LinkedList(Of IGrammarElement), ByVal opNode As Operation)
            If Not TypeOf opNode Is Lop And Not TypeOf opNode Is Bop Then Throw New ConditionLexicalException(String.Format("Invalid operation {0}", opNode))
            FlushOpStack(res, opNode.Priority)
            _opStack.AddFirst(opNode)
        End Sub
    End Class

    Public Class ParserException
        Inherits Exception

        Private _errorPos As Integer
        Private _filterStr As String

        Public Sub New(ByVal message As String, ByVal errorPos As Integer, ByVal filterStr As String)
            MyBase.New(message)
            _errorPos = errorPos
            _filterStr = filterStr
        End Sub

        Public Sub New(ByVal message As String, ByVal innerException As Exception, ByVal errorPos As Integer)
            MyBase.New(message, innerException)
            _errorPos = errorPos
        End Sub

        Public Property FilterStr() As String
            Get
                Return _filterStr
            End Get
            Friend Set(ByVal value As String)
                _filterStr = value
            End Set
        End Property

        Public Sub New(ByVal message As String, ByVal errorPos As Integer)
            MyBase.New(message)
            _errorPos = errorPos
        End Sub

        Public Overrides Function ToString() As String
            If InnerException Is Nothing Then
                If _filterStr = "" Then
                    Return String.Format("At position {0}: {1}", ErrorPos, Message)
                Else
                    Return String.Format("At position {0}: {1} {2}{3} {2}{4," + CStr(ErrorPos) + "} {2}", ErrorPos, Message, Environment.NewLine, _filterStr, "^")
                End If
            Else
                If _filterStr = "" Then
                    Return String.Format("At position {0}: {1}{2}{3}", ErrorPos, Message, Environment.NewLine, InnerException.ToString())
                Else
                    Return String.Format("At position {0}: {1} {2}{3} {2}{4," + CStr(ErrorPos) + "} {2}{3}{5}", ErrorPos, Message, Environment.NewLine, _filterStr, "^", InnerException.ToString())
                End If
            End If
        End Function

        Public Property ErrorPos As Integer
            Get
                Return _errorPos
            End Get
            Friend Set(ByVal value As Integer)
                _errorPos = value
            End Set
        End Property
    End Class

    Friend Class ConditionLexicalException
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

    Public Class FilterableAttribute
        Inherits Attribute
    End Class

    Public Class SortableAttribute
        Inherits Attribute
    End Class

    Public Class InterpreterException
        Inherits Exception

        Public Sub New(ByVal message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(ByVal message As String, ByVal innerException As Exception)
            MyBase.New(message, innerException)
        End Sub
    End Class

    Public Class FilterHelper
        Public Shared Function GetFilterableFields(Of T)() As List(Of String)
            Dim filteredFields As List(Of PropertyInfo) = GetFilteredFields(Of T)()
            Dim fields = (
                    From field In filteredFields
                    Where field.PropertyType.GetInterface(GetType(IFilterable).Name) Is Nothing
                    Let nm = field.Name
                    Select nm).ToList()

            filteredFields.
                Where(Function(field) field.PropertyType.GetInterface(GetType(IFilterable).Name) IsNot Nothing).
                Select(Function(field) New With {.Nam = field.Name, .Typ = field.PropertyType}).ToList().
                ForEach(Sub(element)
                            ' ReSharper disable VBPossibleMistakenCallToGetType.2
                            Dim readableProperties = (
                                    From prop In element.Typ.GetProperties(BindingFlags.Public Or BindingFlags.Instance)
                                    Where prop.GetCustomAttributes(GetType(FilterableAttribute), False).Any
                                    Let nm = String.Format("{0}.{1}", element.Nam, prop.Name)
                                    Select nm
                                    ).ToList()
                            ' ReSharper restore VBPossibleMistakenCallToGetType.2
                            fields.AddRange(readableProperties)
                        End Sub)
            Return fields
        End Function

        Private Shared Function GetFilteredFields(Of T)() As List(Of PropertyInfo)
            Return (From prop In GetType(T).GetProperties(BindingFlags.Public Or BindingFlags.Instance)
                    Where prop.GetCustomAttributes(GetType(FilterableAttribute), False).Any).ToList()
        End Function

        Public Shared Function GetFieldsAndValues(Of T)(ByVal elem As T) As Dictionary(Of String, Object)
            Dim filteredFields = GetFilteredFields(Of T)()
            Dim fieldsAndValues = (From field In filteredFields
                    Where Not TypeOf field.GetValue(elem, Nothing) Is IFilterable
                    Let nm = field.Name.ToUpper(), val = field.GetValue(elem, Nothing)
                    Select nm, val).ToDictionary(Function(item) item.nm, Function(item) item.val)

            filteredFields.
                Where(Function(field) TypeOf field.GetValue(elem, Nothing) Is IFilterable).
                Select(Function(field) New With {.Nam = field.Name, .Val = field.GetValue(elem, Nothing)}).ToList().
                ForEach(Sub(element)
                            ' ReSharper disable VBPossibleMistakenCallToGetType.2
                            Dim readableProperties = (
                                    From prop In element.Val.GetType().GetProperties(BindingFlags.Public Or BindingFlags.Instance)
                                    Where prop.GetCustomAttributes(GetType(FilterableAttribute), False).Any
                                    Let nm = String.Format("{0}.{1}", element.Nam, prop.Name).ToUpper(), val = prop.GetValue(element.Val, Nothing)
                                    Select nm, val
                                    ).ToDictionary(Function(item) item.nm, Function(item) item.val)
                            ' ReSharper restore VBPossibleMistakenCallToGetType.2
                            For Each kvp In readableProperties
                                fieldsAndValues.Add(kvp.Key, kvp.Value)
                            Next
                        End Sub)
            Return fieldsAndValues
        End Function
    End Class

    Public Class FilterInterpreter(Of T)
        Private _grammar As LinkedList(Of FilterParser.IGrammarElement)

        Public Sub New()

        End Sub

        Public Sub SetGrammar(ByVal grammar As LinkedList(Of FilterParser.IGrammarElement))
            _grammar = grammar
        End Sub

        Public Function Allows(ByVal elem As T) As Boolean
            If _grammar Is Nothing OrElse Not _grammar.Any Then Return True
            Dim fieldsAndValues As Dictionary(Of String, Object) = FilterHelper.GetFieldsAndValues(elem)
            ' field.GetType.IsValueType
            Return Allows(fieldsAndValues)
        End Function

        

        Private Function Allows(ByVal fav As Dictionary(Of String, Object)) As Boolean
            ' тут надо считывать и вычислять тройки, складывать их в стек, а в стеке постепенно продолжать вычисления
            Dim resultStack As New Stack(Of Boolean)
            Dim first = True
            Dim pointer = _grammar.First
            Do
                If TypeOf pointer.Value Is FilterParser.Var Then
                    InterpretTriple(resultStack, fav, pointer)
                ElseIf TypeOf pointer.Value Is FilterParser.Lop And Not first Then
                    ApplyBoolean(resultStack, pointer.Value)
                Else
                    Throw New InterpreterException("Invalid filter expression") ' unexpected sequence of elements
                End If
                first = False
                pointer = pointer.Next
            Loop Until pointer Is Nothing
            If resultStack.Count <> 1 Then Throw New InterpreterException("Filter not fully evaluated")
            Return resultStack.Pop()
        End Function

        Private Shared Sub ApplyBoolean(ByRef resultStack As Stack(Of Boolean), ByVal lop As FilterParser.Lop)
            ' pop 2 booleans, apply lop and push back
            Try
                Dim operand1 = resultStack.Pop()
                Dim operand2 = resultStack.Pop()
                Dim res As Boolean
                Select Case lop.LogOperation
                    Case FilterParser.Lop.LogicalOperation.OpAnd
                        res = operand1 And operand2
                    Case FilterParser.Lop.LogicalOperation.OpOr
                        res = operand1 Or operand2
                    Case Else
                        Throw New InterpreterException(String.Format("Invalid operand {0}", lop.LogOperation))
                End Select
                resultStack.Push(res)
            Catch ex As InvalidOperationException
                Throw New InterpreterException("Failed to load operands")
            End Try
        End Sub

        Private Shared Sub InterpretTriple(ByRef resultStack As Stack(Of Boolean), ByVal fav As Dictionary(Of String, Object), ByRef node As LinkedListNode(Of FilterParser.IGrammarElement))
            ' load three items - var, val and boolop, return pointer to last
            ' evaluate var, boolop it to var, push result to resultStack
            Try
                If node Is Nothing Then Throw New InterpreterException("Failed to load operands")
                Dim var = TryCast(node.Value, FilterParser.Var)
                node = node.Next
                If node Is Nothing Then Throw New InterpreterException("Failed to load operands")
                If var Is Nothing Then Throw New InterpreterException(String.Format("Variable name expected instead of {0}", node.Value.ToString()))

                If TypeOf var Is FilterParser.ObjVar Then
                    Dim fullName = CType(var, FilterParser.ObjVar).FullName
                    If Not fav.Keys.Contains(fullName) Then Throw New InterpreterException(String.Format("Object variable {0} not found", fullName))
                ElseIf TypeOf var Is FilterParser.Var Then
                    If Not fav.Keys.Contains(var.Name) Then Throw New InterpreterException(String.Format("Variable {0} not found", var.Name))
                End If

                Dim val = TryCast(node.Value, FilterParser.IVal)
                node = node.Next
                If node Is Nothing Then Throw New InterpreterException("Failed to load operands")
                If val Is Nothing Then Throw New InterpreterException(String.Format("Value expected instead of {0}", node.Value.ToString()))

                Dim boolOp = TryCast(node.Value, FilterParser.Bop)
                If boolOp Is Nothing Then Throw New InterpreterException(String.Format("Invalid filter expression; boolean operation expected instead of {0}", node.Value.ToString()))

                Dim fieldVarValue = fav(var.Name)
                If fieldVarValue Is Nothing Then fieldVarValue = ""

                If TypeOf val Is FilterParser.Val(Of String) Then
                    Dim strObjVal = fieldVarValue.ToString().ToUpper()
                    Dim strValVal = CType(val, FilterParser.Val(Of String)).Value.ToUpper()

                    Select Case boolOp.BinOperation
                        Case FilterParser.Bop.BinaryOperation.OpEquals
                            resultStack.Push(strObjVal = strValVal)
                        Case FilterParser.Bop.BinaryOperation.OpNotEqual
                            resultStack.Push(strObjVal <> strValVal)
                        Case FilterParser.Bop.BinaryOperation.OpLike
                            Try
                                Dim rx = New Regex(strValVal)
                                resultStack.Push(rx.Match(strObjVal).Success)
                            Catch ex As ArgumentException
                                Throw New InterpreterException(String.Format("Invalid regular expression pattern {0}", strValVal)) ' invalid pattern
                            End Try
                        Case FilterParser.Bop.BinaryOperation.OpNLike
                            Try
                                Dim rx = New Regex(strValVal)
                                resultStack.Push(Not rx.Match(strObjVal).Success)
                            Catch ex As ArgumentException
                                Throw New InterpreterException(String.Format("Invalid regular expression pattern {0}", strValVal)) ' invalid pattern
                            End Try
                        Case Else
                            Throw New InterpreterException(String.Format("Operation {0} is not applicable to strings", boolOp.BinOperation)) ' invalid string operation
                    End Select

                ElseIf TypeOf val Is FilterParser.Val(Of Date) Then
                    If IsDate(fieldVarValue) Then
                        Dim datObjVal As Date
                        If Date.TryParse(fieldVarValue, CultureInfo.InvariantCulture, DateTimeStyles.None, datObjVal) Then
                            Dim datValVal = CType(val, FilterParser.Val(Of Date)).Value

                            Select Case boolOp.BinOperation
                                Case FilterParser.Bop.BinaryOperation.OpEquals
                                    resultStack.Push(datObjVal = datValVal)
                                Case FilterParser.Bop.BinaryOperation.OpNotEqual
                                    resultStack.Push(datObjVal <> datValVal)
                                Case FilterParser.Bop.BinaryOperation.OpGreater
                                    resultStack.Push(datObjVal > datValVal)
                                Case FilterParser.Bop.BinaryOperation.OpGreaterOrEquals
                                    resultStack.Push(datObjVal >= datValVal)
                                Case FilterParser.Bop.BinaryOperation.OpLess
                                    resultStack.Push(datObjVal < datValVal)
                                Case FilterParser.Bop.BinaryOperation.OpLessOrEquals
                                    resultStack.Push(datObjVal <= datValVal)
                                Case Else
                                    Throw New InterpreterException(String.Format("Operation {0} is not applicable to dates", boolOp.BinOperation)) ' invalid date operation
                            End Select
                        Else
                            resultStack.Push(True)
                        End If
                    Else
                        resultStack.Push(True)
                    End If
                    ElseIf TypeOf val Is FilterParser.Val(Of Double) Then
                        ' todo in theory I could separate double and integer so that to disallow = and <> for double
                        If Not IsNumeric(fieldVarValue) Then Throw New InterpreterException(String.Format("Value {0} is not in numeric format", fieldVarValue))
                        Dim dblObjVal = CType(fieldVarValue, Double)
                        Dim dblValVal = CType(val, FilterParser.Val(Of Double)).Value
                        Select Case boolOp.BinOperation
                            Case FilterParser.Bop.BinaryOperation.OpEquals
                                ' ReSharper disable CompareOfFloatsByEqualityOperator
                                resultStack.Push(dblObjVal = dblValVal)
                                ' ReSharper restore CompareOfFloatsByEqualityOperator
                            Case FilterParser.Bop.BinaryOperation.OpNotEqual
                                ' ReSharper disable CompareOfFloatsByEqualityOperator
                                resultStack.Push(dblObjVal <> dblValVal)
                                ' ReSharper restore CompareOfFloatsByEqualityOperator
                            Case FilterParser.Bop.BinaryOperation.OpGreater
                                resultStack.Push(dblObjVal > dblValVal)
                            Case FilterParser.Bop.BinaryOperation.OpGreaterOrEquals
                                resultStack.Push(dblObjVal >= dblValVal)
                            Case FilterParser.Bop.BinaryOperation.OpLess
                                resultStack.Push(dblObjVal < dblValVal)
                            Case FilterParser.Bop.BinaryOperation.OpLessOrEquals
                                resultStack.Push(dblObjVal <= dblValVal)
                            Case Else
                                Throw New InterpreterException(String.Format("Operation {0} is not applicable to numbers", boolOp.BinOperation)) ' invalid date operation
                        End Select

                    ElseIf TypeOf val Is FilterParser.Val(Of Boolean) Then
                        ' todo interpretation of booleans
                        Try
                            Dim boolObjVal = CType(fieldVarValue, Boolean)
                            Dim boolValVal = CType(val, FilterParser.Val(Of Boolean)).Value
                            Select Case boolOp.BinOperation
                                Case FilterParser.Bop.BinaryOperation.OpEquals
                                    resultStack.Push(boolObjVal = boolValVal)
                                Case FilterParser.Bop.BinaryOperation.OpNotEqual
                                    resultStack.Push(boolObjVal <> boolValVal)
                                Case Else
                                    Throw New InterpreterException(String.Format("Operation {0} is not applicable to booleans", boolOp.BinOperation)) ' invalid bool operation
                            End Select
                        Catch ex As Exception
                            Throw New InterpreterException(String.Format("Value {0} is not in boolean format", fieldVarValue), ex)
                        End Try

                    ElseIf TypeOf val Is FilterParser.Val(Of Rating) Then
                        Dim rateObjVal = TryCast(fieldVarValue, Rating)
                        If rateObjVal Is Nothing Then Throw New InterpreterException(String.Format("Value {0} is not in rating format", fieldVarValue))
                        Dim rateValVal = CType(val, FilterParser.Val(Of Rating)).Value
                        Select Case boolOp.BinOperation
                            Case FilterParser.Bop.BinaryOperation.OpEquals
                                resultStack.Push(rateObjVal = rateValVal)
                            Case FilterParser.Bop.BinaryOperation.OpNotEqual
                                resultStack.Push(rateObjVal <> rateValVal)
                            Case FilterParser.Bop.BinaryOperation.OpGreater
                                resultStack.Push(rateObjVal > rateValVal)
                            Case FilterParser.Bop.BinaryOperation.OpGreaterOrEquals
                                resultStack.Push(rateObjVal >= rateValVal)
                            Case FilterParser.Bop.BinaryOperation.OpLess
                                resultStack.Push(rateObjVal < rateValVal)
                            Case FilterParser.Bop.BinaryOperation.OpLessOrEquals
                                resultStack.Push(rateObjVal <= rateValVal)
                            Case Else
                                Throw New InterpreterException(String.Format("Operation {0} is not applicable to ratings", boolOp.BinOperation)) ' invalid rating operation
                        End Select
                    Else
                        ' ReSharper disable VBPossibleMistakenCallToGetType.2
                        Throw New InterpreterException(String.Format("Unknown operand type {0} ", node.Value.GetType().ToString())) ' unknown type
                        ' ReSharper restore VBPossibleMistakenCallToGetType.2
                    End If
            Catch ex As NullReferenceException
                Throw New InterpreterException("Failed to interpret, NPE occured", ex) ' unknown type

            Catch ex As InvalidOperationException
                Throw New InterpreterException("Failed to load operands")
            End Try
        End Sub
    End Class

    Public Interface IFilterable
    End Interface

    Public Enum SortDirection
        None
        Asc
        Desc
    End Enum

    Friend Class Sorter(Of T)
        Implements IComparer(Of T)


        Private _field As PropertyInfo
        Private _direction As SortDirection = SortDirection.None

        Public Sub SetSort(ByVal fieldName As String, ByVal sortDirection As SortDirection)
            Dim filteredFields = (From prop In GetType(T).GetProperties(BindingFlags.Public Or BindingFlags.Instance)
                                  Where prop.Name = fieldName).ToList()
            If Not filteredFields.Any Then Throw New SorterException()
            _field = filteredFields.First()
            _direction = sortDirection
        End Sub

        Public Function Compare(ByVal x As T, ByVal y As T) As Integer Implements IComparer(Of T).Compare
            If _direction = SortDirection.None Then Return 0
            If _field Is Nothing Then Return 0
            Dim xVal = _field.GetValue(x, Nothing)
            Dim yVal = _field.GetValue(y, Nothing)
            Return If(_direction = SortDirection.Asc, 1, -1) * Comparer.Default.Compare(xVal, yVal)
        End Function
    End Class

    Friend Class SorterException
        Inherits Exception
    End Class
End Namespace