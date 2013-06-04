Imports AdfinXRtLib
Imports System.Threading.Tasks
Imports System.Collections.Concurrent
Imports System.Runtime.InteropServices
Imports NLog
Imports Settings
Imports System.Threading
Imports CommonController

Public Class Chain
    Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(Chain))
    Private ReadOnly _result As New ConcurrentDictionary(Of String, List(Of String))
    Private ReadOnly _rics As New ConcurrentBag(Of String)
    Private _mode As String
    Private _failedFlag As Integer = 0
    Private Shared ReadOnly _chainManagers As New List(Of AdxRtChain)

    Private WithEvents _shutdownManager As ShutdownController = ShutdownController.Instance

    Private ReadOnly _failedHandlers As New List(Of Action(Of String, Exception, Boolean))
    Public Custom Event Failed As Action(Of String, Exception, Boolean)
        AddHandler(ByVal value As Action(Of String, Exception, Boolean))
            _failedHandlers.Add(value)
        End AddHandler
        RemoveHandler(ByVal value As Action(Of String, Exception, Boolean))
            _failedHandlers.Remove(value)
        End RemoveHandler
        RaiseEvent(ByVal ric As String, ByVal ex As Exception, ByVal final As Boolean)
            If Interlocked.Increment(_failedFlag) <= 1 Then
                _failedHandlers.ForEach(Sub(handler) handler(ric, ex, final))
            Else
                Logger.Warn("Already notified")
            End If
        End RaiseEvent
    End Event

    Delegate Sub ChainHandler(ByVal ric As String, ByVal data As Dictionary(Of String, List(Of String)), ByVal done As Boolean)

    Private ReadOnly _chainHandlers As New List(Of ChainHandler)
    Public Custom Event Chain As ChainHandler
        AddHandler(ByVal value As ChainHandler)
            _chainHandlers.Add(value)
        End AddHandler
        RemoveHandler(ByVal value As ChainHandler)
            _chainHandlers.Remove(value)
        End RemoveHandler
        RaiseEvent(ByVal arg1 As String, ByVal arg2 As Dictionary(Of String, List(Of String)), ByVal arg3 As Boolean)
            _chainHandlers.ForEach(Sub(handler) handler(arg1, arg2, arg3))
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
        If Interlocked.Read(_failedFlag) > 0 Then
            Logger.Warn("Already failed, exiting LoadRics")
            Exit Sub
        End If
        Try
            chainMan = Eikon.Sdk.CreateAdxRtChain()
            _chainManagers.Add(chainMan)
            chainMan.Source = SettingsManager.Instance.ReutersDataSource
            chainMan.ItemName = ricName
            chainMan.Mode = _mode
        Catch ex As COMException
            Logger.ErrorException("COM Exception, cannot init AdFinRT", ex)
            Logger.Error("Exception = {0}", ex.ToString())

            RaiseEvent Failed("", ex, True)
            Exit Sub
        Catch ex As Exception
            Logger.ErrorException("Failed to init chain " + ricName + " ", ex)
            Logger.Error("Exception = {0}", ex.ToString())

            RaiseEvent Failed(ricName, ex, False)
            Exit Sub
        End Try

        AddHandler chainMan.OnUpdate,
            Sub(datastatus As RT_DataStatus)
                Logger.Trace("Got data on chain {0}, status is {1}", ricName, datastatus)
                If Interlocked.Read(_failedFlag) > 0 Then
                    Logger.Warn("Already failed, exiting Parse")
                    Exit Sub
                End If
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
                    RaiseEvent Chain(ricName, New Dictionary(Of String, List(Of String))(_result), Not _rics.Any)
                Catch ex As Exception
                    Logger.ErrorException("Failed to parse chain [" + ricName + "] data", ex)
                    Logger.Error("Exception = {0}", ex.ToString())
                    RaiseEvent Failed(ricName, ex, False) ' todo this means load will stop completely. I could devise event that wouldn't stop the process
                End Try
            End Sub
        chainMan.RequestChain()
    End Sub

    Private Sub ShutdownNow() Handles _shutdownManager.ShutdownNow
        For Each hM In _chainManagers
            Marshal.ReleaseComObject(hM)
            hM = Nothing
        Next
        _chainManagers.Clear()
    End Sub
End Class