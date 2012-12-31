Imports System.IO
Imports Microsoft.Win32
Imports NLog

Namespace Commons
    Module SettingsManager
        Private ReadOnly Branch As RegistryKey = Registry.CurrentUser.CreateSubKey("ThomsonReuters\YieldMapAddIn")

        Private _minYield As Double? = Nothing
        Public Property MinYield() As Double?
            Set(ByVal value As Double?)
                If value.HasValue Then
                    Branch.SetValue("minYield", value.Value.ToString("F2"))
                Else
                    Branch.SetValue("minYield", "")
                End If
                _minYield = value
            End Set
            Get
                Return _minYield
            End Get
        End Property

        Private _maxYield As Double? = Nothing
        Public Property MaxYield() As Double?
            Set(ByVal value As Double?)
                If value.HasValue Then
                    Branch.SetValue("maxYield", value.Value.ToString("F2"))
                Else
                    Branch.SetValue("maxYield", "")
                End If
                _maxYield = value
            End Set
            Get
                Return _maxYield
            End Get
        End Property

        Private _minDur As Double? = Nothing
        Public Property MinDur() As Double?
            Set(ByVal value As Double?)
                If value.HasValue Then
                    Branch.SetValue("minDur", value.Value.ToString("F2"))
                Else
                    Branch.SetValue("minDur", "")
                End If
                _minDur = value
            End Set
            Get
                Return _minDur
            End Get
        End Property

        Private _maxDur As Double? = Nothing
        Public Property MaxDur() As Double?
            Set(ByVal value As Double?)
                If value.HasValue Then
                    Branch.SetValue("maxDur", value.Value.ToString("F2"))
                Else
                    Branch.SetValue("maxDur", "")
                End If
                _maxDur = value
            End Set
            Get
                Return _maxDur
            End Get
        End Property

        Private _minSpread As Double? = Nothing
        Public Property MinSpread() As Double?
            Set(ByVal value As Double?)
                If value.HasValue Then
                    Branch.SetValue("minSpread", value.Value.ToString("F2"))
                Else
                    Branch.SetValue("minSpread", "")
                End If
                _minSpread = value
            End Set
            Get
                Return _minSpread
            End Get
        End Property

        Private _maxSpread As Double? = Nothing
        Public Property MaxSpread() As Double?
            Set(ByVal value As Double?)
                If value.HasValue Then
                    Branch.SetValue("maxSpread", value.Value.ToString("F2"))
                Else
                    Branch.SetValue("maxSpread", "")
                End If
                _maxSpread = value
            End Set
            Get
                Return _maxSpread
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

        Private _defaultField As String = "LAST"
        Public Property DefaultField As String
            Get
                Return _defaultField
            End Get
            Set(value As String)
                Branch.SetValue("defaultField", value.ToUpper())
                _defaultField = value
            End Set
        End Property

        Private _showBidAsk As Boolean = True
        Public Property ShowBidAsk() As Boolean
            Get
                Return _showBidAsk
            End Get
            Set(value As Boolean)
                Branch.SetValue("showBidAsk", value.ToString)
                _showBidAsk = value
            End Set
        End Property

        Private _showPointSize As Boolean = False
        Public Property ShowPointSize() As Boolean
            Get
                Return _showPointSize
            End Get
            Set(value As Boolean)
                Branch.SetValue("showPointSize", value.ToString)
                _showPointSize = value
            End Set
        End Property

        Private _showMainToolBar As Boolean = True
        Public Property ShowMainToolBar() As Boolean
            Get
                Return _showMainToolBar
            End Get
            Set(value As Boolean)
                Branch.SetValue("showMainToolBar", value.ToString)
                _showMainToolBar = value
            End Set
        End Property

        Private _showChartToolBar As Boolean = True
        Public Property ShowChartToolBar() As Boolean
            Get
                Return _showChartToolBar
            End Get
            Set(value As Boolean)
                Branch.SetValue("showChartToolBar", value.ToString)
                _showChartToolBar = value
            End Set
        End Property

        Private _logFileName As String = String.Format("YieldMap_{0:ddMMyyyy}.log", Date.Today)
        Public Property LogFileName() As String
            Get
                Return _logFileName
            End Get
            Set(ByVal value As String)
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

        Public Property LogFilePath As String = Path.GetTempPath()
        Public DbFileName As String = "bonds.sqlite"
        Public ZipFileName As String = String.Format("attachments_{0:ddMMyyyy}.zip", Date.Today)

        Private _bondSelectorVisibleColumns As String = "ALL"
        Public Property BondSelectorVisibleColumns As String
            Get
                Return _bondSelectorVisibleColumns
            End Get
            Set(ByVal value As String)
                Branch.SetValue("bondSelectorVisibleColumns", value.ToString())
                _bondSelectorVisibleColumns = value
            End Set
        End Property

        Sub New()
            Dim val As Object

            val = Branch.GetValue("maxYield") : If val IsNot Nothing AndAlso val.ToString <> "" Then _maxYield = Double.Parse(val)
            val = Branch.GetValue("minYield") : If val IsNot Nothing AndAlso val.ToString <> "" Then _minYield = Double.Parse(val)
            val = Branch.GetValue("maxSpread") : If val IsNot Nothing AndAlso val.ToString <> "" Then _maxSpread = Double.Parse(val)
            val = Branch.GetValue("minSpread") : If val IsNot Nothing AndAlso val.ToString <> "" Then _minSpread = Double.Parse(val)
            val = Branch.GetValue("maxDur") : If val IsNot Nothing AndAlso val.ToString <> "" Then _maxDur = Double.Parse(val)
            val = Branch.GetValue("minDur") : If val IsNot Nothing AndAlso val.ToString <> "" Then _minDur = Double.Parse(val)

            val = Branch.GetValue("showBidAsk") : If val IsNot Nothing Then _showBidAsk = Boolean.Parse(val)
            val = Branch.GetValue("showPointSize") : If val IsNot Nothing Then _showPointSize = Boolean.Parse(val)
            val = Branch.GetValue("showMainToolBar") : If val IsNot Nothing Then _showMainToolBar = Boolean.Parse(val)
            val = Branch.GetValue("showChartToolBar") : If val IsNot Nothing Then _showChartToolBar = Boolean.Parse(val)

            val = Branch.GetValue("bondSelectorVisibleColumns") : If val IsNot Nothing Then _bondSelectorVisibleColumns = val.ToString()

            val = Branch.GetValue("logLevel"): If val IsNot Nothing Then _logLevel = LogLevel.FromString(val)
            val = Branch.GetValue("refreshInterval") : If val IsNot Nothing Then _refreshInterval = CInt(val)
        End Sub
    End Module
End Namespace