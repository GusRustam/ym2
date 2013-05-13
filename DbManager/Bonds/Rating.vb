Namespace Bonds
    Public Class RatingSource
        Public Shared SnP As New RatingSource("Standard and Poor's", "S&P", {"S&P", "SPI"})
        Public Shared Moodys As New RatingSource("Moody's Investor Services", "Moody's", {"MDL", "MIS", "MDY"})
        Public Shared Fitch As New RatingSource("Fitch Rating Agency", "Fitch", {"FTC", "FDL", "FSU"})

        Private ReadOnly _name As String
        Private ReadOnly _descr As String
        Private ReadOnly _abbr As String()

        Public Shared ReadOnly RateSources As RatingSource() = {SnP, Moodys, Fitch}

        Private Sub New(ByVal name As String, ByVal descr As String, ByVal abbr As String())
            _name = name
            _descr = descr
            _abbr = abbr
        End Sub

        Public ReadOnly Property Abbr As String()
            Get
                Return _abbr
            End Get
        End Property

        Public ReadOnly Property Name As String
            Get
                Return _name
            End Get
        End Property

        Public ReadOnly Property Descr As String
            Get
                Return _descr
            End Get
        End Property

        Public Shared Function Parse(ByVal rateSrc As String) As RatingSource
            Dim src = (From s In RateSources
                       Where s.Abbr.Contains(rateSrc)
                       Select s).ToList
            If Not src.Any Then Return Nothing
            Return src.First
        End Function

        Public Overrides Function ToString() As String
            Return _descr
        End Function

        Protected Overloads Function Equals(ByVal other As RatingSource) As Boolean
            Return String.Equals(_descr, other._descr)
        End Function

        Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
            If ReferenceEquals(Nothing, obj) Then Return False
            If ReferenceEquals(Me, obj) Then Return True
            If obj.GetType IsNot Me.GetType Then Return False
            Return Equals(DirectCast(obj, RatingSource))
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return _descr.GetHashCode
        End Function

        Public Shared Operator =(ByVal left As RatingSource, ByVal right As RatingSource) As Boolean
            Return Equals(left, right)
        End Operator

        Public Shared Operator <>(ByVal left As RatingSource, ByVal right As RatingSource) As Boolean
            Return Not Equals(left, right)
        End Operator
    End Class

    Public Class Rating
        Public Shared Aaa As New Rating(21, {"AAA"})
        Public Shared Aa1 As New Rating(20, {"AA+", "Aa1"})
        Public Shared Aa2 As New Rating(19, {"AA", "Aa2"})

        Public Shared Aa3 As New Rating(18, {"AA-", "Aa3"})
        Public Shared A1 As New Rating(17, {"A+", "A1"})
        Public Shared A2 As New Rating(16, {"A", "A2"})
        Public Shared A3 As New Rating(15, {"A-", "A3"})
        Public Shared Baa1 As New Rating(14, {"BBB+", "Baa1"})
        Public Shared Baa2 As New Rating(13, {"BBB", "Baa2"})
        Public Shared Baa3 As New Rating(12, {"BBB-", "Baa3"})
        Public Shared Ba1 As New Rating(11, {"BB+", "Ba1"})
        Public Shared Ba2 As New Rating(10, {"BB", "Ba2"})
        Public Shared Ba3 As New Rating(9, {"BB-", "Ba3"})
        Public Shared B1 As New Rating(8, {"B+", "B1"})
        Public Shared B2 As New Rating(7, {"B", "B2"})
        Public Shared B3 As New Rating(6, {"B-", "B3"})
        Public Shared Caa1 As New Rating(5, {"CCC+", "Caa1"})
        Public Shared Caa2 As New Rating(4, {"CCC", "Caa2"})
        Public Shared Caa3 As New Rating(3, {"CCC-", "Caa3"})
        Public Shared Ca As New Rating(2, {"CC", "Ca"})
        Public Shared C As New Rating(1, {"C"})
        Public Shared Other As New Rating(0, {""})

        Public Shared ReadOnly Property AllRatings(ByVal mode As RatingSource) As List(Of RatingListItem)
            Get
                Dim all = {Aaa, Aa1, Aa2, A1, A2, A3, Baa1, Baa2, Baa3, Ba1, Ba2, Ba3, B1, B2, B3, Caa1, Caa2, Caa3, Ca, C, Other}
                Return all.Select(Function(rt) New RatingListItem(rt, mode)).ToList()
            End Get
        End Property

        Public Shared Ratings As Rating() = {Aaa, Aa1, Aa2, A1, A2, A3,
                                 Baa1, Baa2, Baa3, Ba1, Ba2, Ba3, B1, B2, B3,
                                 Caa1, Caa2, Caa3, Ca, C}

        Public Shared Top As Rating() = {Aaa, Aa1, Aa2, A1, A2, A3}
        Public Shared Middle As Rating() = {Baa1, Baa2, Baa3, Ba1, Ba2, Ba3, B1, B2, B3}
        Public Shared Junk As Rating() = {Caa1, Caa2, Caa3, Ca, C}

        Private ReadOnly _names As String()
        Private ReadOnly _level As Integer

        Public Class RatingListItem
            Private ReadOnly _rating As Rating
            Private ReadOnly _src As RatingSource

            Public Overrides Function ToString() As String
                If _rating <> Other Then
                    Return _rating.GetName(_src)
                Else
                    Return ""
                End If
            End Function

            Sub New(ByVal rating As Rating, ByVal src As RatingSource)
                _rating = rating
                _src = src
            End Sub

            Public ReadOnly Property Rating As Rating
                Get
                    Return _rating
                End Get
            End Property
        End Class

        Public ReadOnly Property Level() As Integer
            Get
                Return _level
            End Get
        End Property

        Public Function GetName(ByVal src As RatingSource) As String
            Return If(src IsNot Nothing AndAlso src = RatingSource.Moodys AndAlso _names.Length > 1, _names(1), _names(0))
        End Function

        Public ReadOnly Property Names() As String()
            Get
                Return _names
            End Get
        End Property

        Public Shared Function Parse(ByVal name As String) As Rating
            Dim res = (From rating In Ratings Where rating.Names.Contains(name)).ToList()
            Return If(res.Any, res.First(), Other)
        End Function

        Private Sub New(ByVal level As Integer, ByVal names As String())
            _level = level
            _names = names
        End Sub

        Protected Overloads Function Equals(ByVal oth As Rating) As Boolean
            Return _level = oth._level
        End Function

        Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
            If ReferenceEquals(Nothing, obj) Then Return False
            If ReferenceEquals(Me, obj) Then Return True
            If obj.GetType IsNot Me.GetType Then Return False
            Return Equals(DirectCast(obj, Rating))
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return _level
        End Function

        Public Shared Operator =(ByVal left As Rating, ByVal right As Rating) As Boolean
            Return Equals(left, right)
        End Operator

        Public Shared Operator <>(ByVal left As Rating, ByVal right As Rating) As Boolean
            Return Not Equals(left, right)
        End Operator

        Public Shared Operator >(ByVal left As Rating, ByVal right As Rating) As Boolean
            Return left.CompareTo(right) > 0
        End Operator

        Public Shared Operator <(ByVal left As Rating, ByVal right As Rating) As Boolean
            Return left.CompareTo(right) < 0
        End Operator

        Public Shared Operator >=(ByVal left As Rating, ByVal right As Rating) As Boolean
            Return left.CompareTo(right) >= 0
        End Operator

        Public Shared Operator <=(ByVal left As Rating, ByVal right As Rating) As Boolean
            Return left.CompareTo(right) <= 0
        End Operator

        Private Function CompareTo(ByVal obj As Rating) As Integer
            If ReferenceEquals(Nothing, obj) Then Return 1
            If ReferenceEquals(Me, obj) Then Return 0
            If _level < obj.Level Then Return -1
            If _level > obj.Level Then Return 1
            Return 0
        End Function

    End Class

    '
    ' VERY INTERESTING THING: RATINGS ARE COMPARED BY VALUE
    ' WHILST RATING_DESCRS ARE COMPARED BY DATE. THIS BEHAVIOR 
    ' WOULDN'T WORK IN CASE OF WHAT? CURRENTLY THAT LOOKS OK
    '
    '
    Public Class RatingDescr
        Implements IFilterable
        Implements IComparable
        Private ReadOnly _rating As Rating
        Private ReadOnly _ratingDate As Date?
        Private ReadOnly _ratingSource As RatingSource

        Public Sub New(ByVal rating As Rating, ByVal ratingDate As Date?, ByVal ratingSource As RatingSource)
            _rating = rating
            _ratingDate = ratingDate
            _ratingSource = ratingSource
        End Sub

        <Filterable()>
        Public ReadOnly Property Rating() As Rating
            Get
                Return _rating
            End Get
        End Property

        <Filterable()>
        Public ReadOnly Property RatingDate() As Date?
            Get
                Return _ratingDate
            End Get
        End Property

        <Filterable()>
        Public ReadOnly Property RatingSource() As RatingSource
            Get
                Return _ratingSource
            End Get
        End Property

        Public Shared Operator >(ByVal r1 As RatingDescr, ByVal r2 As RatingDescr) As Boolean
            If r1 Is Nothing And r2 Is Nothing Then Return True
            If r1 Is Nothing Then Return True
            If r2 Is Nothing Then Return False

            If Not r1.RatingDate.HasValue And Not r2.RatingDate.HasValue Then Return True
            If r1.RatingDate.HasValue And Not r2.RatingDate.HasValue Then Return True
            If r2.RatingDate.HasValue And Not r1.RatingDate.HasValue Then Return False
            Return r1.RatingDate.Value > r2.RatingDate.Value
        End Operator

        Public Shared Operator <(ByVal r1 As RatingDescr, ByVal r2 As RatingDescr) As Boolean
            Return r2 > r1
        End Operator

        Public Function CompareTo(ByVal obj As Object) As Integer Implements IComparable.CompareTo
            If Not TypeOf obj Is RatingDescr Then Return 1
            Return CompareToo(CType(obj, RatingDescr))
        End Function

        Private Function CompareToo(ByVal other As RatingDescr) As Integer
            If Me = other Then Return 0
            If Me > other Then Return 1
            Return -1
        End Function

        Protected Overloads Function Equals(ByVal other As RatingDescr) As Boolean
            Return _ratingDate.Equals(other._ratingDate) AndAlso Equals(_rating, other._rating) AndAlso Equals(_ratingSource, other._ratingSource)
        End Function

        Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
            If ReferenceEquals(Nothing, obj) Then Return False
            If ReferenceEquals(Me, obj) Then Return True
            If obj.GetType IsNot Me.GetType Then Return False
            Return Equals(DirectCast(obj, RatingDescr))
        End Function

        Public Overrides Function GetHashCode() As Integer
            Dim hashCode As Integer = _ratingDate.GetHashCode
            If _rating IsNot Nothing Then
                hashCode = CInt((hashCode * 397L) Mod Integer.MaxValue) Xor _rating.GetHashCode
            End If
            If _ratingSource IsNot Nothing Then
                hashCode = CInt((hashCode * 397L) Mod Integer.MaxValue) Xor _ratingSource.GetHashCode
            End If
            Return hashCode
        End Function


        Public Shared Operator =(ByVal left As RatingDescr, ByVal right As RatingDescr) As Boolean
            Return Equals(left, right)
        End Operator

        Public Shared Operator <>(ByVal left As RatingDescr, ByVal right As RatingDescr) As Boolean
            Return Not Equals(left, right)
        End Operator

        Public Overrides Function ToString() As String
            If _rating <> Rating.Other Then
                Return String.Format("{0} {1:dd/MM/yy} by {2}", _rating.GetName(_ratingSource), _ratingDate, _ratingSource)
            Else
                Return "N/A"
            End If
        End Function
    End Class
End Namespace