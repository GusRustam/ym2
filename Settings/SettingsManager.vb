Imports System.Globalization
Imports System.IO
Imports NLog
Imports System.Xml
Imports Uitls


Public Module SettingsManager
    Public Event YieldRangeChanged As Action(Of Double?, Double?)
    Public Event DurRangeChanged As Action(Of Double?, Double?)
    Public Event SpreadRangeChanged As Action(Of Double?, Double?)
    Public Event ShowBidAskChanged As Action(Of Boolean)
    Public Event ShowPointSizeChanged As Action(Of Boolean)

    Private ReadOnly Settings As New XmlDocument

    Private _minYield As Double? = Nothing
    Public Property MinYield() As Double?
        Set(ByVal value As Double?)
            SaveValue("/settings/viewport/yield/@min", If(value.HasValue, value.Value.ToString("F2"), ""))
            _minYield = value
            RaiseEvent YieldRangeChanged(_minYield, _maxYield)
        End Set
        Get
            Return _minYield
        End Get
    End Property

    Private _maxYield As Double? = Nothing
    Public Property MaxYield() As Double?
        Set(ByVal value As Double?)
            SaveValue("/settings/viewport/yield/@max", If(value.HasValue, value.Value.ToString("F2"), ""))
            _maxYield = value
            RaiseEvent YieldRangeChanged(_minYield, _maxYield)
        End Set
        Get
            Return _maxYield
        End Get
    End Property

    Private _minDur As Double? = Nothing
    Public Property MinDur() As Double?
        Set(ByVal value As Double?)
            SaveValue("/settings/viewport/duration/@min", If(value.HasValue, value.Value.ToString("F2"), ""))
            _minDur = value
            RaiseEvent DurRangeChanged(_minDur, _maxDur)
        End Set
        Get
            Return _minDur
        End Get
    End Property

    Private _maxDur As Double? = Nothing
    Public Property MaxDur() As Double?
        Set(ByVal value As Double?)
            SaveValue("/settings/viewport/duration/@max", If(value.HasValue, value.Value.ToString("F2"), ""))
            _maxDur = value
            RaiseEvent DurRangeChanged(_minDur, _maxDur)
        End Set
        Get
            Return _maxDur
        End Get
    End Property

    Private _minSpread As Double? = Nothing
    Public Property MinSpread() As Double?
        Set(ByVal value As Double?)
            SaveValue("/settings/viewport/spread/@min", If(value.HasValue, value.Value.ToString("F2"), ""))
            _minSpread = value
            RaiseEvent SpreadRangeChanged(_minSpread, _maxSpread)
        End Set
        Get
            Return _minSpread
        End Get
    End Property

    Private _maxSpread As Double? = Nothing
    Public Property MaxSpread() As Double?
        Set(ByVal value As Double?)
            SaveValue("/settings/viewport/spread/@max", If(value.HasValue, value.Value.ToString("F2"), ""))
            _maxSpread = value
            RaiseEvent SpreadRangeChanged(_minSpread, _maxSpread)
        End Set
        Get
            Return _maxSpread
        End Get
    End Property

    Private _logLevel As LogLevel = LogLevel.Error
    Public Property LogLevel() As LogLevel
        Set(ByVal value As LogLevel)
            SaveValue("/settings/property/[@name=log-level]/@value", value.ToString())
            _logLevel = value
        End Set
        Get
            Return _logLevel
        End Get
    End Property

    Private _showBidAsk As Boolean = True
    Public Property ShowBidAsk() As Boolean
        Get
            Return _showBidAsk
        End Get
        Set(ByVal value As Boolean)
            SaveValue("/settings/property/[@name=show-bid-ask]/@value", value.ToString())
            If value <> _showBidAsk Then RaiseEvent ShowBidAskChanged(value)
            _showBidAsk = value
        End Set
    End Property

    Private _showPointSize As Boolean = False
    Public Property ShowPointSize() As Boolean
        Get
            Return _showPointSize
        End Get
        Set(ByVal value As Boolean)
            SaveValue("/settings/property/[@name=show-point-size]/@value", value.ToString())
            If value <> _showPointSize Then RaiseEvent ShowPointSizeChanged(value)
            _showPointSize = value
        End Set
    End Property

    Private _showMainToolBar As Boolean = True
    Public Property ShowMainToolBar() As Boolean
        Get
            Return _showMainToolBar
        End Get
        Set(ByVal value As Boolean)
            SaveValue("/settings/property/[@name=show-main-toolbar]/@value", value.ToString())
            _showMainToolBar = value
        End Set
    End Property

    Private _showChartToolBar As Boolean = True
    Public Property ShowChartToolBar() As Boolean
        Get
            Return _showChartToolBar
        End Get
        Set(ByVal value As Boolean)
            SaveValue("/settings/property/[@name=show-chart-toolbar]/@value", value.ToString())
            _showChartToolBar = value
        End Set
    End Property

    Private _bondSelectorVisibleColumns As String = "ALL"
    Public Property BondSelectorVisibleColumns As String
        Get
            Return _bondSelectorVisibleColumns
        End Get
        Set(ByVal value As String)
            SaveValue("/settings/property/[@name=visible-columns]/@value", value.ToString())
            _bondSelectorVisibleColumns = value
        End Set
    End Property

    Private _dataSource As String = "IDN"
    Public Property ReutersDataSource As String
        Get
            Return _dataSource
        End Get
        Set(ByVal value As String)
            SaveValue("/settings/property/[@name=data-sourve]/@value", value.ToString())
            _dataSource = value
        End Set
    End Property

    Private _lastDbUpdate As Date? = Nothing
    Public Property LastDbUpdate As Date?
        Get
            Return _lastDbUpdate
        End Get
        Set(ByVal value As Date?)
            SaveValue("/settings/property/[@name=last-db-update]/@value", If(value.HasValue, value.Value.ToString("yyyyMMdd"), ""))
            _lastDbUpdate = Date.Parse(value)
        End Set
    End Property


    Sub New()
        Dim p = Path.Combine(Utils.GetMyPath(), "settings.xml")
        Settings.Load(p)

        GetDoubleValue("/settings/viewport/yield/@max", _maxYield)
        GetDoubleValue("/settings/viewport/yield/@min", _minYield)
        GetDoubleValue("/settings/viewport/spread/@max", _maxSpread)
        GetDoubleValue("/settings/viewport/spread/@min", _minSpread)
        GetDoubleValue("/settings/viewport/duration/@max", _maxDur)
        GetDoubleValue("/settings/viewport/duration/@min", _minDur)

        GetBoolValue("/settings/property[@name='show-bid-ask']/@value", _showBidAsk)
        GetBoolValue("/settings/property[@name='show-point-size']/@value", _showPointSize)
        GetBoolValue("/settings/property[@name='show-main-toolbar']/@value", _showMainToolBar)
        GetBoolValue("/settings/property[@name='show-chart-toolbar']/@value", _showChartToolBar)

        GetStringValue("/settings/property[@name='visible-columns']/@value", _bondSelectorVisibleColumns)
        GetStringValue("/settings/property[@name='data-source']/@value", _dataSource)
        GetDateValue("/settings/property[@name='last-db-update']/@value", _lastDbUpdate)

        Dim tmp As String : GetStringValue("/settings/property[@name='log-level']/@value", tmp)
        _logLevel = If(tmp <> "", LogLevel.FromString(tmp), LogLevel.Error)
        ' todo somebody do set this logging level!
    End Sub

    Private Sub GetDoubleValue(ByVal address As String, ByRef var As Double?)
        Dim val As XmlNode
        val = Settings.SelectSingleNode(address)
        If val IsNot Nothing AndAlso val.Value <> "" Then var = Double.Parse(val.Value)
    End Sub

    Private Sub GetBoolValue(ByVal address As String, ByRef var As Boolean)
        Dim val As XmlNode
        val = Settings.SelectSingleNode(address)
        If val IsNot Nothing AndAlso val.Value <> "" Then var = Boolean.Parse(val.Value)
    End Sub

    Private Sub GetDateValue(ByVal address As String, ByRef var As Date?)
        Dim val As XmlNode
        val = Settings.SelectSingleNode(address)
        If val IsNot Nothing AndAlso val.Value <> "" Then var = Date.ParseExact(var, "yyyyMMdd", CultureInfo.InvariantCulture)
    End Sub

    Private Sub GetStringValue(ByVal address As String, ByRef var As String)
        Dim val As XmlNode
        val = Settings.SelectSingleNode(address)
        var = If(val IsNot Nothing, val.Value, "")
    End Sub

    Private Sub SaveValue(ByVal address As String, ByVal value As String)
        Dim val As XmlNode
        val = Settings.SelectSingleNode(address)
        If val Is Nothing Then
            val.Value = value
            Settings.Save("settings.xml")
        End If
    End Sub
End Module