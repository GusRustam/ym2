Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Reflection
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Xml
Imports NLog
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions

Public Interface IProgressObject
    ReadOnly Property Name() As String
End Interface

Public Enum MessageKind
    Positive
    Neutral
    Negative
    Fail
    Finished
End Enum

Public Class ProgressEvent
    Private ReadOnly _kind As MessageKind
    Private ReadOnly _msg As String
    Private ReadOnly _item As IProgressObject
    Private ReadOnly _exc As Exception
    Private _log As ProgressLog

    Public Sub New(ByVal kind As MessageKind, ByVal msg As String, Optional ByVal item As IProgressObject = Nothing, Optional ByVal exc As Exception = Nothing)
        _kind = kind
        _msg = msg
        _exc = exc
        _log = Log
        _item = item
    End Sub

    Public ReadOnly Property Item() As IProgressObject
        Get
            Return _item
        End Get
    End Property

    Public ReadOnly Property Kind() As MessageKind
        Get
            Return _kind
        End Get
    End Property

    Public ReadOnly Property Msg() As String
        Get
            Return _msg
        End Get
    End Property

    Public ReadOnly Property Exc() As Exception
        Get
            Return _exc
        End Get
    End Property

    Public Property Log As ProgressLog
        Get
            Return _log
        End Get
        Set(ByVal value As ProgressLog)
            _log = value
        End Set
    End Property
End Class

Public NotInheritable Class ProgressLog
    Private ReadOnly _entries As New List(Of ProgressEvent)

    Public ReadOnly Property Entries() As List(Of ProgressEvent)
        Get
            Return _entries
        End Get
    End Property

    Public Sub LogEvent(ByVal evt As ProgressEvent)
        _entries.Add(evt)
    End Sub

    Public Function Success() As Boolean
        Return Not _entries.Any(Function(item) item.Kind = MessageKind.Fail) AndAlso _entries.Any(Function(item) item.Kind = MessageKind.Finished)
    End Function

    Public Function Failed() As Boolean
        Return _entries.Any(Function(item) item.Kind = MessageKind.Fail)
    End Function
End Class

Public Interface IProgressProcess
    Event Progress As Action(Of ProgressEvent)
    Sub Start(ByVal ParamArray params())
End Interface

Public Class IdValue(Of TIdType, TValueType)
    Public Property Id As TIdType
    Public Property Value As TValueType
    Public Shared Widening Operator CType(ByVal value As IdValue(Of TIdType, TValueType)) As TIdType
        Return value.Id
    End Operator

    Public Sub New()
    End Sub

    Public Sub New(ByVal id As TIdType, ByVal name As TValueType)
        Me.Id = id
        Value = name
    End Sub

    Public Overrides Function ToString() As String
        Return Value.ToString()
    End Function
End Class

Public Class IdName(Of TIdType)
    Inherits IdValue(Of TIdType, String)

    Public Sub New()
    End Sub

    Public Sub New(ByVal id As TIdType, ByVal name As String)
        MyBase.New(id, name)
    End Sub
End Class

Public Class Utils
    Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(Utils))
    Public Shared Sub RunCommand(ByVal command As String, ByVal args As String, Optional ByVal sh As Boolean = False)
        Try
            Dim prc As New ProcessStartInfo(command, args)
            prc.UseShellExecute = sh
            Process.Start(prc)
        Catch ex As Exception
            Logger.WarnException("Failed to run command [" + command + "] with args [" + args + "]", ex)
            Logger.Warn("Exception = {0}", ex)
            Try
                Dim prc As New ProcessStartInfo(command, args)
                prc.UseShellExecute = sh
                prc.Verb = "runas"
                Process.Start(prc)
            Catch ex1 As Exception
                Logger.ErrorException("Failed to run command with admin rights request [" + command + "] with args [" + args + "]", ex)
                Logger.Error("Exception = {0}", ex)
            End Try
        End Try
    End Sub

    Public Shared Function GetColorList(Optional ByVal threshold As Double = 0.7) As List(Of String)
        Dim colorsArr = [Enum].GetValues(GetType(KnownColor))
        Dim colors = New List(Of String)()
        Array.ForEach(Of KnownColor)(colorsArr, Sub(color) colors.Add(color.ToString()))
        Dim props = GetType(SystemColors).GetProperties(BindingFlags.Static Or BindingFlags.Public)
        Array.ForEach(props, Sub(prop) colors.Remove(prop.Name))

        Return (From aColor In colors
            Let c = Color.FromName(aColor),
                cmps = {c.R, c.G, c.B}.ToList(),
                lightness = 0.5 * (CDbl(cmps.Min()) + CDbl(cmps.Max())) / 255
            Where lightness < threshold
            Select aColor).ToList
    End Function

    Public Shared Function GetMyPath() As String
        Dim installPath = Path.GetDirectoryName(Assembly.GetAssembly(GetType(Utils)).CodeBase)
        Return installPath.Substring(6)
    End Function

    Public Shared Function GetEnvironment() As String
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

    Public Shared Function FromExcelSerialDate(ByVal serialDate As Integer) As DateTime
        If serialDate > 59 Then serialDate -= 1
        Return New DateTime(1899, 12, 31).AddDays(serialDate)
    End Function

    Public Shared Function GetRange(ByVal min As Double, ByVal max As Double, ByVal numsteps As Integer) As List(Of Double)
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

    Private Shared Function IsNone(ByVal str As String) As Boolean
        Return str = "" Or str = "Default"
    End Function

    ''' <summary>
    ''' Returns adjusted rate structure
    ''' </summary>
    ''' <param name="main">Rate mode fixed in settings</param>
    ''' <param name="parent">Rate mode fixed in bond</param>
    ''' <param name="original">Rate mode in question</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Priority is as follows:
    ''' 1) First, apply mode in parent (if not Default)
    ''' 2) Then, apply mode in settings (if not Default)
    ''' 3) If failed, return original
    ''' </remarks>
    Public Shared Function GetRateStructure(ByVal main As String, ByVal parent As String, ByVal original As String) As String
        If Not IsNone(parent) Then Return Regex.Replace(original, "YT[A-Z]", parent)
        If Not IsNone(main) Then Return Regex.Replace(original, "YT[A-Z]", main)
        Return original
    End Function
End Class

Public Module DllFunctions
    <DllImport("ole32.dll")>
    Public Sub CoUninitialize()
    End Sub
End Module

Public Module Extensions
    <Extension()>
    Public Function Belongs(Of T)(ByVal x As T, ByVal whereTo As IEnumerable(Of T)) As Boolean
        Return whereTo.Contains(x)
    End Function

    <Extension()>
    Public Function Belongs(Of T)(ByVal x As T, ByVal ParamArray whereTo As T()) As Boolean
        Return whereTo.Cast(Of T).Contains(x)
    End Function

    <Extension()>
    Public Sub Import(Of T)(ByVal this As HashSet(Of T), ByVal what As HashSet(Of T))
        For Each elem In what
            this.Add(elem)
        Next
    End Sub

    <Extension()>
    Public Sub GuiAsync(ByVal frm As Form, ByVal action As Action)
        If action IsNot Nothing Then
            If frm.InvokeRequired Then
                frm.Invoke(action)
            Else
                action()
            End If
        End If
    End Sub

    <Extension()>
    Public Sub InsertForm(ByVal ctl As Control, ByVal frm As Form)
        frm.TopLevel = False
        frm.FormBorderStyle = FormBorderStyle.None
        frm.Dock = DockStyle.Fill
        frm.Visible = True
        ctl.Controls.Add(frm)
    End Sub


    <Extension()>
    Public Function GetAttr(ByVal node As XmlNode, ByVal name As String, Optional ByVal defaultValue As String = "") As String
        Dim attribute As XmlAttribute = node.Attributes(name)
        If attribute IsNot Nothing Then
            Return attribute.Value
        Else
            Return defaultValue
        End If
    End Function

    <Extension()>
    Public Function GetAttrStrict(ByVal node As XmlNode, ByVal name As String) As String
        Dim attribute As XmlAttribute = node.Attributes(name)
        If attribute IsNot Nothing Then
            Return attribute.Value
        Else
            Throw New Exception(String.Format("Failed to find attribute {0} in node {1}", name, node.Name))
        End If
    End Function

    <Extension()>
    Public Sub UpdateAttr(ByVal xml As XmlDocument, ByRef node As XmlNode, ByVal attrName As String, ByVal attrVal As String)
        If node.Attributes(attrName) Is Nothing Then
            xml.AppendAttr(node, attrName, attrVal)
        Else
            node.Attributes(attrName).Value = attrVal
        End If
    End Sub

    <Extension()>
    Public Sub AppendAttr(ByVal xml As XmlDocument, ByRef node As XmlNode, ByVal attrName As String, ByVal attrVal As String)
        If attrVal = "" Then Return
        Dim attr As XmlAttribute
        attr = xml.CreateAttribute(attrName)
        attr.Value = attrVal
        node.Attributes.Append(attr)
    End Sub

    <Extension()>
    Public Sub AddRange(ByVal this As DataPointCollection, ByVal points As IEnumerable(Of DataPoint))
        For Each pnt In points
            this.Add(pnt)
        Next
    End Sub
End Module