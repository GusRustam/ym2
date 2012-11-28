Imports System.Data
Imports System.Reflection
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Drawing.Imaging
Imports System.Runtime.CompilerServices
Imports YieldMap.BondsDataSetTableAdapters
Imports NLog
Imports System.Text
Imports System.Security.Permissions
Imports Ionic.Zip

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

        Public Sub RunCommand(ByVal command As String)
            Try
                Dim perm As New SecurityPermission(SecurityPermissionFlag.AllFlags)
                perm.Demand()
                Process.Start(command)
            Catch ex As Exception
                Logger.WarnException("Failed to run command [" + command + "]", ex)
                Logger.Warn("Exception = {0}", ex)
            End Try
        End Sub

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

        Public Function GetDbUpdateDate() As Date?
            Logger.Info("GetDbUpdateDate")
            Dim settingsData = (New settingsTableAdapter).GetData
            If settingsData.Count > 0 Then
                Dim updateDateStr = settingsData.First.lastupdatedate
                Dim updateDate As DateTime
                If DateTime.TryParse(updateDateStr, updateDate) Then Return updateDate.Date
            End If
            Return Nothing
        End Function

        Public Sub ZipAndAttachFiles(ByVal mail As MAPI)
            Using zip As New ZipFile
                Dim logName = Path.Combine(LogFilePath, LogFileName)
                If File.Exists(logName) Then zip.AddFile(logName, "")

                Dim dbName = Path.Combine(GetMyPath(), DbFileName)
                If File.Exists(dbName) Then zip.AddFile(dbName, "")

                Dim timestampStr = Date.Now.ToString("yyyy-MM-dd hh-mm-ss")
                Dim num As Integer
                Screen.AllScreens.ToList().ForEach(
                      Sub(screen)
                          Dim bmpScreenshot = New Bitmap(screen.Bounds.Width, screen.Bounds.Height, PixelFormat.Format32bppArgb)
                          Dim gfxScreenshot = Graphics.FromImage(bmpScreenshot)
                          gfxScreenshot.CopyFromScreen(screen.Bounds.X, screen.Bounds.Y, 0, 0, screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy)
                          bmpScreenshot.Save(Path.Combine(LogFilePath, String.Format("{0}_{1}.png", timestampStr, num)), ImageFormat.Png)
                          num += 1
                      End Sub)

                For i = 0 To num - 1
                    Dim fName = Path.Combine(LogFilePath, String.Format("{0}_{1}.png", timestampStr, i))
                    If File.Exists(fName) Then zip.AddFile(fName, "")
                Next

                Dim dbZipName = Path.Combine(LogFilePath, ZipFileName)
                zip.Save(dbZipName)
                If File.Exists(dbZipName) Then mail.AddAttachment(dbZipName)
            End Using
        End Sub

        Public Sub SendErrorReport(ByVal header As String, ByVal message As String)
            Try
                Dim mail As New MAPI
                mail.AddRecipientTo("rustam.guseynov@thomsonreuters.com")
                ZipAndAttachFiles(mail)

                mail.SendMailPopup(header, message)
            Catch ex As Exception
                Clipboard.SetText(message)
                RunCommand("mailto:rustam.guseynov@thomsonreuters.com?subject=YieldMap%20Error&body=---Paste%20error%20info%20here---")
            End Try
        End Sub

        <Extension()>
        Public Sub GuiAsync(frm As Form, ByVal action As Action)
            If action IsNot Nothing Then
                If frm.InvokeRequired Then
                    frm.Invoke(action)
                Else
                    action()
                End If
            End If
        End Sub
    End Module
End Namespace