Imports YieldMap.Commons
Imports NLog
Imports AdfinXRtLib

Namespace Tools.Chains
    Public Class ChainLoadManager
        Private Shared ReadOnly Logger As Logger = GetLogger(GetType(ChainHandler))
        Private ReadOnly _loaders As New Dictionary(Of String, ChainHandler)

        Public Function AddListToLoad(ByVal chainName As String, ByVal params As String, Optional ByVal timeOut As Integer = 5) As ChainHandler
            Logger.Debug("AddListToLoad({0}, {1}, {2})", chainName, params, timeOut)
            Dim chainMan As AdxRtChain = Eikon.SDK.CreateAdxRtChain()
            ' Вдруг такой уже есть? Дублировать не будем
            If _loaders.ContainsKey(chainName) Then
                Return Nothing
            Else
                ' Создаем обработчик данной цепочки (читатель)
                Dim handler As New ChainHandler(chainMan, chainName, params, timeOut)

                ' И на его событие поступления данных вешаем свой обработчик
                AddHandler handler.OnData,
                    Sub()
                        SyncLock (_loaders)
                            ' Раз данные пришли, убираем обработчик
                            _loaders.Remove(chainName)
                        End SyncLock
                    End Sub

                SyncLock (_loaders)
                    ' А пока что данные еще не пришли - заносим нового читателя в список читателей
                    _loaders.Add(chainName, handler)
                End SyncLock

                Return handler
            End If
        End Function
    End Class
End Namespace