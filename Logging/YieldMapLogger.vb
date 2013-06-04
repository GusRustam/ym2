Imports System.IO
Imports NLog
Imports NLog.Layouts
Imports NLog.Config
Imports NLog.Targets

Public Module LoggerManager
    Public ReadOnly LogFileName As String = String.Format("YieldMap_{0:ddMMyyyy}.log", Date.Today)
    Public ReadOnly LogFilePath As String = Path.GetTempPath()
    Public ZipFileName As String = String.Format("attachments_{0:ddMMyyyy}.zip", Date.Today)

    ' default settings
    Public Property LoggingLevel As LogLevel
        Get
            return _loggingLevel
        End Get
        Set(ByVal value As LogLevel)
            _loggingLevel = value
            LogManager.GlobalThreshold = LoggingLevel
        End Set
    End Property


    Private ReadOnly Logger As Logger
    Private ReadOnly TxtTarget As FileTarget
    Private ReadOnly UdpTarget As ChainsawTarget
    Private _loggingLevel As LogLevel = LogLevel.Trace

    Sub New()
        ' 1) Creating logger text config
        Const layoutText As String = "${date} " + ControlChars.Tab + " ${level} " + ControlChars.Tab + " ${callsite:includeSourcePath=false} | ${message} | ${exception:format=Type,Message} | ${stacktrace}"
        TxtTarget = New FileTarget() With {
            .FileName = Path.Combine(LogFilePath, LogFileName),
            .DeleteOldFileOnStartup = True,
            .Name = "Main",
            .Layout = Layout.FromString(layoutText)
        }

        ' 2) Creating logger UDP config
        UdpTarget = New ChainsawTarget() With {
            .Address = "udp://127.0.0.1:7071",
            .Name = "Chainsaw",
            .Layout = New Log4JXmlEventLayout
        }

        ' 4) Selecting congiguration and initializing
        Logger = LogManager.GetCurrentClassLogger()
        Dim loggerConfig As LoggingConfiguration = New LoggingConfiguration()
        loggerConfig.LoggingRules.Add(New LoggingRule("*", LoggingLevel, TxtTarget))
        loggerConfig.LoggingRules.Add(New LoggingRule("*", LoggingLevel, UdpTarget))
        LogManager.Configuration = loggerConfig

        Logger.Debug("Logger initialized")
    End Sub

    Public Function GetLogger(ByVal aClass As Type) As Logger
        Return LogManager.GetLogger(aClass.Name)
    End Function
End Module


'Dim ConnectionString = String.Format ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}\{1}", installPath, DbFileName).Replace ("\", "\\")

' 2) Creating Logger Database configuration
'dbTarget = new DatabaseTarget() With {
'        .ConnectionString = ConnectionString,    
'        .CommandText = "INSERT INTO log (origin, severity, message, createdate, exc, stacktrace) VALUES (?, ?, ?, ?, ?, ?)",
'        .DBProvider = "oledb",
'        .KeepConnection = True,
'        .Name = "Main"
'        }
'Dim dbParams As List(Of DatabaseParameterInfo) = New List(Of DatabaseParameterInfo)() From {
'        new DatabaseParameterInfo ("origin", Layout.FromString ("${callsite:includeSourcePath=false}")),
'        new DatabaseParameterInfo ("logLevel", Layout.FromString ("${level}")),
'        new DatabaseParameterInfo ("message", Layout.FromString ("${message}")),
'        new DatabaseParameterInfo ("createdate", Layout.FromString ("${date}")),
'        new DatabaseParameterInfo ("except", Layout.FromString ("${exception:format=Type,Message}")),
'        new DatabaseParameterInfo ("stackTrace", Layout.FromString ("${stacktrace}"))
'        }

'dbParams.ForEach (Sub (p) dbTarget.Parameters.Add (p))