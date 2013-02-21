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

    Public Event Arrived As Action(Of String)
    Public Event Chain As Action(Of Dictionary(Of String, List(Of String)))

    Public Sub StartChains(ByVal chains As List(Of String), ByVal mode As String)
        Logger.Info("StartChains()")
        _mode = mode
        For Each ric In chains
            _rics.Add(ric)
            _result.TryAdd(ric, New List(Of String)())
        Next
        Parallel.ForEach(chains, AddressOf LoadRics)
    End Sub

    Sub LoadRics(ByVal ricName)
        Logger.Info("LoadRics({0})", ricName)

        Dim chainMan As AdxRtChain = EikonSdk.Sdk.CreateAdxRtChain()
        chainMan.Source = ReutersDataSource
        chainMan.ItemName = ricName
        chainMan.Mode = _mode
        AddHandler chainMan.OnUpdate,
            Sub(datastatus As RT_DataStatus)
                If datastatus = RT_DataStatus.RT_DS_FULL Then
                    RaiseEvent Arrived(ricName)
                    Try
                        For i = chainMan.Data.GetLowerBound(0) To chainMan.Data.GetUpperBound(0)
                            _result(ricName).Add(chainMan.Data.GetValue(i).ToString())
                        Next
                    Catch ex As Exception
                        Dim a As Integer
                        a = 1
                    End Try
                    _rics.TryTake(ricName)
                ElseIf datastatus <> RT_DataStatus.RT_DS_PARTIAL Then
                    _result(ricName) = Nothing
                    _rics.TryTake(ricName)
                End If
                If Not _rics.Any Then
                    RaiseEvent Chain(New Dictionary(Of String, List(Of String))(_result))
                End If
            End Sub
        chainMan.RequestChain()
    End Sub
End Class