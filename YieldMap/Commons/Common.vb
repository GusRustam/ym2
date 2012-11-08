Imports System.Data
Imports System.Reflection
Imports System.IO
Imports System.Runtime.InteropServices
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

        <DllImport("PLVbaApis.dll")>
        Function CreateReutersObject(ByVal progId As String) As <MarshalAs(UnmanagedType.IUnknown)> Object
        End Function

        <DllImport("user32.dll")>
        Public Function SetForegroundWindow(ByVal hWnd As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        Public Function GetWin1251String(ByVal str As String) As String 'todo no effect
            Dim win1251 = Encoding.GetEncoding(1251)
            Dim utf8Bytes = Encoding.UTF8.GetBytes(str)
            Dim win1251Bytes = Encoding.Convert(Encoding.UTF8, win1251, utf8Bytes)
            Return New String(win1251.GetChars(win1251Bytes))
        End Function

        Public Function GetEnvironment() As String
            Dim res As String = ""
            res += String.Format("CommandLine = {0}", Environment.CommandLine) + Environment.NewLine
            res += String.Format("CurrentDirectory = {0}", Environment.CurrentDirectory) + Environment.NewLine
            res += String.Format("Is64BitOperatingSystem = {0}", Environment.Is64BitOperatingSystem) + Environment.NewLine
            res += String.Format("Is64BitProcess = {0}", Environment.Is64BitProcess) + Environment.NewLine
            res += String.Format("MachineName = {0}", Environment.MachineName) + Environment.NewLine
            res += String.Format("OSVersion = {0}", Environment.OSVersion) + Environment.NewLine
            res += String.Format("SystemDirectory = {0}", Environment.SystemDirectory) + Environment.NewLine
            res += String.Format("UserDomainName = {0}", Environment.UserDomainName) + Environment.NewLine
            res += String.Format("UserName = {0}", Environment.UserName) + Environment.NewLine
            res += String.Format("Version = {0}", Environment.Version) + Environment.NewLine
            Return res
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

        Public Function GetMyPath() As String
            Dim installPath = Path.GetDirectoryName(Assembly.GetAssembly(GetType(Common)).CodeBase)
            Return installPath.Substring(6)
        End Function

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