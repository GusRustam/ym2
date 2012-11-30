Imports System.Data.SQLite

Namespace Database
    Public Module Database
        Private ReadOnly Conn As New SQLiteConnection(My.Settings.Default.bondsConnectionString)

        Public Sub Execute(ByVal sql As String)
            Dim cmd As New SQLiteCommand(sql, Conn)
            cmd.ExecuteNonQuery()
        End Sub

        Public Function GetValue(ByVal sql As String) As Object
            Dim cmd As New SQLiteCommand(sql, Conn)
            Return cmd.ExecuteScalar()
        End Function

        Public Function Query(ByVal sql As String) As SQLiteDataReader
            Dim cmd As New SQLiteCommand(sql, Conn)
            Return cmd.ExecuteReader()
        End Function
    End Module
End Namespace