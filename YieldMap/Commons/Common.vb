Imports System.Data
Imports NLog
Imports System.Text

Namespace Commons
    Public Class IdName
        Public Property Id As Integer
        Public Property Name As String
        Public Shared Widening Operator CType(value As IdName) As Int32
            Return value.Id
        End Operator
    End Class

    Module Common
        Private ReadOnly Logger As Logger = GetLogger(GetType(Common))
        Public Function GetWin1251String(ByVal str As String) As String 'todo no effect
            Dim win1251 = Encoding.GetEncoding(1251)
            Dim utf8Bytes = Encoding.UTF8.GetBytes(str)
            Dim win1251Bytes = Encoding.Convert(Encoding.UTF8, win1251, utf8Bytes)
            Return New String(win1251.GetChars(win1251Bytes))
        End Function

        Public Function FromExcelSerialDate(ByVal serialDate As Integer) As DateTime
            If serialDate > 59 Then serialDate -= 1
            Return New DateTime(1899, 12, 31).AddDays(serialDate)
        End Function

        Public Sub SkipInvalidRows(sender As Object, args As FillErrorEventArgs)
            Logger.Info("Invalid row in table {0}", args.DataTable.TableName)
            If TypeOf args.Errors Is ArgumentException Then
                args.Continue = True
            End If
        End Sub

        Public Function GetRange(ByVal min As Double, ByVal max As Double, ByVal numsteps As Integer) As List(Of Double)
            Debug.Assert(numsteps > 1)
            Dim currX = min
            Dim stepX = (max - min) / (numsteps - 1)
            Dim res As New List(Of Double)
            For i = 0 To numsteps - 1
                res.Add(currX)
                currX += stepX
            Next
            Return res
        End Function
    End Module
End Namespace