Imports AdfinXRtLib
Imports System.Threading.Tasks
Imports System.Collections.Concurrent
Imports NLog
Imports Settings

Public Class Chain
    Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(Chain))
    Private ReadOnly _result As New ConcurrentDictionary(Of String, List(Of String))
    Private ReadOnly _rics As New ConcurrentBag(Of String)
    Private _mode As String

    Private ReadOnly _failedHandlers As New List(Of Action(Of String, Exception))
    Public Custom Event Failed As Action(Of String, Exception)
        AddHandler(ByVal value As Action(Of String, Exception))
            _failedHandlers.Add(value)
        End AddHandler
        RemoveHandler(ByVal value As Action(Of String, Exception))
            _failedHandlers.Remove(value)
        End RemoveHandler
        RaiseEvent(ByVal arg1 As String, ByVal arg2 As Exception)
            _failedHandlers.ForEach(Sub(handler) handler(arg1, arg2))
        End RaiseEvent
    End Event

    Private ReadOnly _chainHandlers As New List(Of Action(Of String, Dictionary(Of String, List(Of String))))
    Public Custom Event Chain As Action(Of String, Dictionary(Of String, List(Of String)))
        AddHandler(ByVal value As Action(Of String, Dictionary(Of String, List(Of String))))
            _chainHandlers.Add(value)
        End AddHandler
        RemoveHandler(ByVal value As Action(Of String, Dictionary(Of String, List(Of String))))
            _chainHandlers.Remove(value)
        End RemoveHandler
        RaiseEvent(ByVal arg1 As String, ByVal arg2 As Dictionary(Of String, List(Of String)))
            _chainHandlers.ForEach(Sub(handler) handler(arg1, arg2))
        End RaiseEvent
    End Event

    Public Sub StartChains(ByVal chains As List(Of String), ByVal mode As String)
        Logger.Info("StartChains()")
        _mode = mode
        For Each ric In chains
            _rics.Add(ric)
            _result.TryAdd(ric, New List(Of String)())
        Next
        Parallel.ForEach(chains, AddressOf LoadRics)
    End Sub

    Sub LoadRics(ByVal ricName As String)
        Logger.Info("LoadRics({0})", ricName)

        Dim chainMan As AdxRtChain
        Try
            chainMan = Eikon.Sdk.CreateAdxRtChain()
            chainMan.Source = SettingsManager.Instance.ReutersDataSource
            chainMan.ItemName = ricName
            chainMan.Mode = _mode
        Catch ex As Exception
            Logger.ErrorException("Failed to init chain " + ricName + " ", ex)
            Logger.Error("Exception = {0}", ex.ToString())

            RaiseEvent Failed(ricName, ex)
            Exit Sub
        End Try

        AddHandler chainMan.OnUpdate,
            Sub(datastatus As RT_DataStatus)
                Logger.Trace("Got data on chain {0}, status is {1}", ricName, datastatus)
                Try
                    If datastatus = RT_DataStatus.RT_DS_FULL Then
                        For i = chainMan.Data.GetLowerBound(0) To chainMan.Data.GetUpperBound(0)
                            _result(ricName).Add(chainMan.Data.GetValue(i).ToString())
                        Next
                        _rics.TryTake(ricName)
                    ElseIf datastatus <> RT_DataStatus.RT_DS_PARTIAL Then
                        _result(ricName) = Nothing
                        _rics.TryTake(ricName)
                    End If
                    If Not _rics.Any Then
                        RaiseEvent Chain(ricName, New Dictionary(Of String, List(Of String))(_result))
                    End If
                Catch ex As Exception
                    Logger.ErrorException("Failed to parse chain [" + ricName + "] data", ex)
                    Logger.Error("Exception = {0}", ex.ToString())
                    RaiseEvent Failed(ricName, ex)
                End Try
            End Sub
        chainMan.RequestChain()
    End Sub
End Class