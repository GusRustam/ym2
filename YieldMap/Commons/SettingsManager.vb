Imports System.IO
Imports Microsoft.Win32
Imports NLog

Namespace Commons
    Module SettingsManager
        Private ReadOnly Branch As RegistryKey = Registry.CurrentUser.CreateSubKey("ThomsonReuters\YieldMapAddIn")

        Private _minYield As Double = 0
        Public Property MinYield() As Double
            Set(ByVal value As Double)
                Branch.SetValue("minYield", value.ToString("F2"))
                _minYield = value
            End Set
            Get
                Return _minYield
            End Get
        End Property

        Private _maxYield As Double = 30
        Public Property MaxYield() As Double
            Set(ByVal value As Double)
                Branch.SetValue("maxYield", value.ToString("F2"))
                _maxYield = value
            End Set
            Get
                Return _maxYield
            End Get
        End Property

        Private _minDur As Double = 0
        Public Property MinDur() As Double
            Set(ByVal value As Double)
                Branch.SetValue("minDur", value.ToString("F2"))
                _minDur = value
            End Set
            Get
                Return _minDur
            End Get
        End Property

        Private _maxDur As Double = 20
        Public Property MaxDur() As Double
            Set(ByVal value As Double)
                Branch.SetValue("maxDur", value.ToString("F2"))
                _maxDur = value
            End Set
            Get
                Return _maxDur
            End Get
        End Property

        Private _logLevel As LogLevel = LogLevel.Trace
        Public Property LogLevel() As LogLevel
            Set(ByVal value As LogLevel)
                Branch.SetValue("logLevel", value.ToString())
                _logLevel = value
            End Set
            Get
                Return _logLevel
            End Get
        End Property

        Public Property LogFilePath As String = Path.GetTempPath()

        Public DbFileName As String = "bonds.sqlite"

        Public ZipFileName As String = String.Format("attachments_{0:ddMMyyyy}.zip", Date.Today)

        Private _logFileName As String = String.Format("YieldMap_{0:ddMMyyyy}.log", Date.Today)
        Public Property LogFileName() As String
            Get
                Return _logFileName
            End Get
            Set(ByVal value As String)
                'Branch.SetValue("logFileName", value.ToString())
                _logFileName = value
            End Set
        End Property

        Private _refreshInterval As Integer = 10
        Public Property RefreshInterval() As Integer
            Get
                Return _refreshInterval
            End Get
            Set(ByVal value As Integer)
                Branch.SetValue("refreshInterval", value.ToString())
                _refreshInterval = value
            End Set
        End Property

        Sub New()
            Dim maxY = Branch.GetValue("maxYield")
            If maxY IsNot Nothing Then _maxYield = Double.Parse(maxY)

            Dim minY = Branch.GetValue("minYield")
            If minY IsNot Nothing Then _minYield = Double.Parse(minY)

            Dim maxD = Branch.GetValue("maxDur")
            If maxD IsNot Nothing Then _maxDur = Double.Parse(maxD)

            Dim minD = Branch.GetValue("minDur")
            If minD IsNot Nothing Then _minDur = Double.Parse(minD)

            Dim lL = Branch.GetValue("logLevel")
            If lL IsNot Nothing Then _logLevel = LogLevel.FromString(lL)

            Dim lFn = Branch.GetValue("logFileName")
            If lFn IsNot Nothing Then _logFileName = lFn

            Dim rInt = Branch.GetValue("refreshInterval")
            If rInt IsNot Nothing Then _refreshInterval = CInt(rInt)
        End Sub
    End Module
End Namespace