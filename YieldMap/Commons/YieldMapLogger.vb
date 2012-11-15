Imports System.IO
Imports NLog
Imports NLog.Layouts
Imports NLog.Config
Imports NLog.Targets

Namespace Commons
    Public Module YieldMapLogger
        ' default settings
        Public LoggingLevel As LogLevel = LogLevel.Trace
        Private ReadOnly LogFileName As String = SettingsManager.LogFileName

        Private ReadOnly Logger As Logger
        Private ReadOnly TxtTarget As FileTarget
        Private ReadOnly UdpTarget As ChainsawTarget

        Sub New()
            ' 0) Read registry
            LoggingLevel = LogLevel

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
                .Address = "udp://localhost:7071",
                .Name = "Chainsaw",
                .Layout = New Log4JXmlEventLayout
            }

            ' 4) Selecting congiguration and initializing
            SetLoggingLevel()

            Logger = LogManager.GetCurrentClassLogger()
            Logger.Debug("Logger initialized")
        End Sub

        Public Sub SetLoggingLevel()
            Dim loggerConfig As LoggingConfiguration = new LoggingConfiguration()
            
            loggerConfig.AddTarget("Main", TxtTarget)
            loggerConfig.AddTarget("Chainsaw", UdpTarget)
            loggerConfig.LoggingRules.Add(New LoggingRule("*", LoggingLevel, TxtTarget))
            loggerConfig.LoggingRules.Add(New LoggingRule("*", LoggingLevel, UdpTarget))

            LogManager.Configuration = loggerConfig
        End Sub

        Public Function GetLogger(aClass As Type) As Logger
            Return LogManager.GetLogger(aClass.Name)
        End Function
    End Module
End NameSpace



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