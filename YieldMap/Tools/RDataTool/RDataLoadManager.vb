﻿Imports Dex2Lib
Imports EikonDesktopSDKLib
Imports NLog
Imports YieldMap.Commons

Namespace Tools.RDataTool
    Public MustInherit Class RQuery
        Private Shared ReadOnly Logger As Logger = GetLogger(GetType(RQuery))

        Private _items As String
        Private _fields As String
        Private _params As String

        Public Delegate Sub DataEventFineHanler(data As EventArgs, err As String)
        Public Delegate Sub DataEventRawHanler(data As Array, err As String)
        Public Event ParsedData As DataEventFineHanler

        Private _dataEventHanler As DataEventRawHanler

        Private _myDex2Mgr As Dex2Mgr
        Private _cookie As Integer
        Private WithEvents _myRData As RData

        Private Sub Load(ByVal sdk As EikonDesktopSDK, data As RQuery, ByVal dataEventHanler As DataEventRawHanler)
            _dataEventHanler = dataEventHanler
            _myDex2Mgr = sdk.CreateDex2Mgr()
            If _myDex2Mgr IsNot Nothing Then
                _cookie = _myDex2Mgr.Initialize()
                _myRData = _myDex2Mgr.CreateRData(_cookie)

                _myRData.InstrumentIDList = data.Items
                _myRData.FieldList = data.Fields
                _myRData.RequestParam = data.Params
                _myRData.DisplayParam = "RH:In CH:Fd"
                _myRData.Subscribe(False)
            End If
        End Sub

        Private Sub OnUpdateHandler(ByVal datastatus As DEX2_DataStatus, ByVal [error] As Object) Handles _myRData.OnUpdate
            Logger.Debug("OnUpdateHandler()")
            If datastatus = DEX2_DataStatus.DE_DS_FULL Then
                _dataEventHanler(_myRData.Data, Nothing)
                _myRData.CancelRequest()
            ElseIf datastatus = DEX2_DataStatus.DE_DS_PARTIAL Then
                _dataEventHanler(_myRData.Data, Nothing)
            ElseIf datastatus = DEX2_DataStatus.DE_DS_NULL_ERROR Then
                Logger.Error("Failed to load data from Dex2: {0}", Err.ToString)
                _dataEventHanler(Nothing, Err.ToString)
                _myRData.CancelRequest()
            End If
        End Sub

        Sub AddTask(ByVal theItems As String, ByVal theFields As String, ByVal theParams As String, ByVal handler As DataEventRawHanler, ByVal sdk As EikonDesktopSDK)
            Logger.Trace("AddTask({0}, {1}, {2})", theItems, theFields, theParams)
            _items = theItems
            _fields = theFields
            _params = theParams
            Load(sdk, Me, handler)
        End Sub

        Sub NotifyDataParsed(data As EventArgs, err As String)
            If data IsNot Nothing Then
                Logger.Debug("NotifyDataArrived({0})", data.ToString())
            Else
                Logger.Warn("NotifyDataArrived({0})", err)
            End If
            RaiseEvent ParsedData(data, err)
        End Sub

#Region "Simple properties"
        Public ReadOnly Property Params() As String
            Get
                Return _params
            End Get
        End Property

        Public ReadOnly Property Fields() As String
            Get
                Return _fields
            End Get
        End Property

        Public ReadOnly Property Items() As String
            Get
                Return _items
            End Get
        End Property
#End Region
    End Class
End Namespace