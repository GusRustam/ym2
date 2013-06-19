Imports System.Text.RegularExpressions

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

        Public Overrides Function ToString() As String
            Return _rDate
        End Function
    End Class
End NameSpace