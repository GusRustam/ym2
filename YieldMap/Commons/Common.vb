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
    End Module
End Namespace