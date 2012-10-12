Imports System.Data.SQLite
Imports System.IO
Imports System.Reflection
Imports YieldMap.My.Resources
Imports YieldMap.Forms.ChartForm
Imports YieldMap.Forms.PortfolioForm
Imports YieldMap.Tools.RDataTool
Imports YieldMap.Tools.Chains
Imports YieldMap.BondsDataSetTableAdapters
Imports EikonDesktopSDKLib
Imports NLog

Public Class MainForm
    Private Shared ReadOnly Logger As Logger = Commons.GetLogger(GetType(MainForm))
    Private Shared _initialized As Boolean = False
    Private Shared _tmpBondDataTable As DataTable
    Private ReadOnly _chainsToLoad As New List(Of String)
    Private Shared ReadOnly ListLoader As New ChainLoadManager

    Private WithEvents _myEikonDesktopSdk As EikonDesktopSDK

    Private Event AllChainsLoaded As Action

#Region "I. GUI Events"
    Private Sub UpdateControlsStatus()
        YieldMapButton.Enabled = _initialized
    End Sub

    Private Sub ConnectButtonClick(sender As System.Object, e As EventArgs) Handles ConnectButton.Click
        StatusLabel.Text = Connecting_to_Eikon
        ConnectButton.Enabled = False
        ConnectToEikon()
    End Sub

    Private Sub MainFormLoad(sender As Object, e As EventArgs) Handles MyBase.Load
        Logger.Info("MainForm_Load")

        Dim installPath = Path.GetDirectoryName(Assembly.GetAssembly(GetType(MainForm)).CodeBase)
        installPath = installPath.Substring(6)
        AppDomain.CurrentDomain.SetData("DataDirectory", installPath)
        My.Settings.Default("bondsConnectionString") = String.Format("data source=""{0}\bonds.sqlite""", installPath)

        UpdateControlsStatus()
    End Sub


    Private Sub DatabaseButton_Click(sender As Object, e As EventArgs) Handles DatabaseButton.Click
        Logger.Info("DatabaseButtonClick()")
        Dim managerForm = New DataBaseManagerForm
        managerForm.ShowDialog()

    End Sub

    Private Sub YieldMapButton_Click(sender As Object, e As EventArgs) Handles YieldMapButton.Click
        Logger.Info("GraphButtonClick()")
        Dim graphForm = New GraphForm
        graphForm.Show()
    End Sub
#End Region

#Region "II. Connecting to Eikon"
    Private Sub ConnectToEikon()
        'Create the Eikon Desktop object
        _myEikonDesktopSdk = New EikonDesktopSDK

        ' Initialize the Eikon Desktop object to connect to Eikon
        If _myEikonDesktopSdk IsNot Nothing Then
            Dim lResult = _myEikonDesktopSdk.Initialize()
            If lResult <> EEikonSDKInitializeResult.Succeed Then
                DisconnectFromEikon()

                Select Case lResult
                    Case EEikonSDKInitializeResult.Error_Reinitialize
                        StatusLabel.Text = Reinit_Eikon_forbidden

                    Case EEikonSDKInitializeResult.Error_InitializeFail
                        StatusLabel.Text = Init_Eikon_Fail
                End Select
                UpdateUserFormAccordingToConnectionStatus(EEikonStatus.Disconnected)
            End If
        End If
    End Sub

    Private Sub UpdateUserFormAccordingToConnectionStatus(ByVal eEikonStatus As EEikonStatus)
        Select Case eEikonStatus
            Case eEikonStatus.Connected
                ConnectButton.Enabled = False
                StatusPicture.Image = Green
                StatusLabel.Text = Status_Connected

            Case eEikonStatus.Disconnected
                DisconnectFromEikon()

                ConnectButton.Enabled = True
                StatusPicture.Image = Red
                StatusLabel.Text = Status_Disconnected

            Case eEikonStatus.LocalMode
                ConnectButton.Enabled = True
                StatusPicture.Image = Orange
                StatusLabel.Text = Status_Local

            Case eEikonStatus.Offline
                DisconnectFromEikon()
                ConnectButton.Enabled = True
                StatusPicture.Image = Red
                StatusLabel.Text = Status_Offline
        End Select
    End Sub

    Public Sub OnStatusChanged(ByVal eStatus As EEikonStatus) Handles _myEikonDesktopSdk.OnStatusChanged
        UpdateUserFormAccordingToConnectionStatus(eStatus)
        If eStatus = EEikonStatus.Connected Then
            UpdateDatabase()
        End If
    End Sub

    Private Sub DisconnectFromEikon()
        _myEikonDesktopSdk = Nothing
    End Sub
#End Region

#Region "III. Loading chains"
    Private Sub UpdateDatabase()
        _initialized = DataAlreadyLoaded() OrElse LoadChains()
    End Sub

    Private Shared Function DataAlreadyLoaded() As Boolean
        Logger.Info("DataAlreadyLoaded")
        Dim settingsData = (New settingsTableAdapter).GetData
        If settingsData.Count > 0 Then
            Dim updateDateStr = settingsData.First.lastupdatedate
            Dim updateDate As DateTime
            If DateTime.TryParse(updateDateStr, updateDate) Then Return updateDate.Date = DateTime.Today.Date
        End If
        Logger.Trace("DataAlreadyLoaded = FALSE")
        Return False
    End Function

    Private Function LoadChains() As Boolean
        Logger.Info("LoadChains")

        'todo start some waiter thread
        'todo and start chain loading in another interruptible thread. For example, use backgroundworker
        'todo or maybe backgroundworker already has support for timeout

        Try
            Dim stChainItemsAdapter As New chain_itemsTableAdapter
            stChainItemsAdapter.ClearAllData()

            Dim stChainsAdapter As New chainTableAdapter
            Dim chainTable = stChainsAdapter.GetData()

            For Each chainRow As BondsDataSet.chainRow In chainTable
                _chainsToLoad.Add(chainRow.chain_name)
                Logger.Debug("row: {0}", chainRow.chain_name)
                Dim loadHandler = ListLoader.AddListToLoad(chainRow.chain_name, "UWC:YES LAY:VER", 30)
                If loadHandler IsNot Nothing Then
                    loadHandler.DoLoadList()
                    AddHandler loadHandler.OnData, AddressOf OnChain
                End If
            Next

            Return True
        Catch ex As Exception
            Logger.ErrorException("Failed to do anything", ex)
            Return False
        End Try
    End Function

    Private Sub OnChain(ByRef sender As Object, ByVal e As ChainItemsData)
        Logger.Info("OnChain(failed = {0})", e.Failed)
        If Not e.Handled Then
            e.Handled = True

            Dim stChainAdapter As New chainTableAdapter
            Dim stChainItemsAdapter As New chain_itemsTableAdapter

            Try
                Dim currentChainId As Integer = stChainAdapter.GetIdsByName(e.ChainName).First().id
                Logger.Debug("filling chain {0}; total items is {1}", e.ChainName, e.ListItems.Count - 1)

                Dim insertCommand As String

                Dim length = e.ListItems.Count, iteration As Integer = 0
                If length > 0 Then
                    Using oleDbConnection = stChainItemsAdapter.Connection
                        oleDbConnection.Open()
                        Dim finished As Boolean
                        Do
                            Logger.Trace("{0} =====---------------=========> iteration {1}", e.ChainName, iteration)
                            Dim minRange = iteration * 500
                            Dim maxRange = Math.Min(minRange + 500, length) - 1
                            finished = minRange + 500 >= length

                            Logger.Trace("saving items {0} to {1}, total {2}, finished: {3}", minRange, maxRange, (maxRange - minRange + 1), finished.ToString())
                            Dim subList = e.ListItems.GetRange(minRange, (maxRange - minRange + 1))

                            insertCommand = subList.Aggregate("INSERT INTO chain_items (item_ric, id_chain) VALUES",
                                                                   Function(current, item) current + String.Format("('{0}', {1}), ", item, currentChainId))
                            insertCommand = insertCommand.Substring(0, insertCommand.Length - 2)
                            Logger.Trace("insertion command is {0}", insertCommand)

                            Try
                                Dim cmd As New SQLiteCommand(insertCommand, oleDbConnection)
                                cmd.ExecuteNonQuery()
                            Catch ex As Exception
                                Logger.WarnException("Failed to insert", ex)
                            End Try

                            iteration = iteration + 1
                        Loop While Not finished
                        oleDbConnection.Close()
                    End Using
                End If
            Catch ex As Exception
                Logger.WarnException("Failed with chain " + e.ChainName, ex)
            End Try
        Else
            Logger.Trace("Ho-ho, {0} is already handled", e.ChainName)
        End If

        RemoveHandler CType(sender, ChainHandler).OnData, AddressOf OnChain
        _chainsToLoad.Remove(e.ChainName)
        If _chainsToLoad.Count = 0 Then RaiseEvent AllChainsLoaded()
    End Sub

    Public Sub OnAllChainsLoaded() Handles Me.AllChainsLoaded
        LoadBonds()
    End Sub
#End Region

#Region "IV. Loading bonds"
    Private Sub LoadBonds()
        Logger.Info("LoadBonds")

        Dim bta As New bondTableAdapter
        bta.ClearAllData()

        Dim cta As New currencyTableAdapter
        cta.ClearAllData()

        Dim ita As New issuerTableAdapter
        ita.ClearAllData()

        Dim cota As New countryTableAdapter
        cota.ClearAllData()

        Dim ricStr = (New chain_itemsTableAdapter).GetData().Select(Function(row As BondsDataSet.chain_itemsRow) row.item_ric).Distinct.ToList
        Dim dataLoader As New BondsDataQuery(ricStr, _myEikonDesktopSdk)
        AddHandler dataLoader.ParsedData, AddressOf OnBondData
    End Sub

    Private Sub OnBondData(e As EventArgs, err As String)
        Logger.Info("OnBondData")
        Dim bondStructures As BondEventArgs = e
        If bondStructures IsNot Nothing Then
            FormatBondData(bondStructures)
            ImportAll()
        Else
            Logger.Warn("No data arrived! Message is [{0}]", err)
        End If
    End Sub

    Private Shared Sub FormatBondData(ByRef bondStructures As BondEventArgs)
        Logger.Info("FormatBondData")
        If bondStructures.Data.Count = 0 Then
            Logger.Warn("No data arrived")
            Exit Sub
        End If

        _tmpBondDataTable = New DataTable()
        ' adding corresponding columns
        With _tmpBondDataTable
            For Each field As KeyValuePair(Of String, Boolean) In BondStructure.MetaData.GetBondFieldDescr
                .Columns.Add(field.Key, GetType(String))
            Next
            .Columns.Add("ric", GetType(String))
        End With

        For Each element As KeyValuePair(Of String, BondStructure) In bondStructures.Data
            Dim theRow = _tmpBondDataTable.NewRow()
            theRow("ric") = element.Key
            For Each fieldData As KeyValuePair(Of String, String) In element.Value.GetColumnNameAndItsValue
                theRow(fieldData.Key) = fieldData.Value
            Next
            _tmpBondDataTable.Rows.Add(theRow)
        Next

    End Sub
#End Region

#Region "V. DB methods"
#Region "a) Import runner"
    Private Class IssuerComparer
        Implements IEqualityComparer(Of DataRow)

        Public Overloads Function Equals(ByVal x As DataRow, ByVal y As DataRow) As Boolean Implements IEqualityComparer(Of DataRow).Equals
            Return x("ticker") = y("ticker")
        End Function

        Public Overloads Function GetHashCode(ByVal obj As DataRow) As Integer Implements IEqualityComparer(Of DataRow).GetHashCode
            Return obj("ticker").GetHashCode()
        End Function
    End Class

    Private Sub ImportAll()
        Logger.Info("ImportAll")
        Using conn = New SQLiteConnection(My.Settings.Default("bondsConnectionString").ToString())
            Try
                conn.Open()
                Dim ctAdapter As New currencyTableAdapter, cotAdapter As New countryTableAdapter
                ' 1) Finding and storing unknown currencies and countries
                StoreSimpleTable(conn, "currency", ctAdapter.GetData())
                StoreSimpleTable(conn, "country", cotAdapter.GetData())

                ' 2) Finding and storing unknown issuers
                StoreIssuers(conn)

                ' 3) Storing da bonds ^_^ nya!
                StoreBonds(conn)
                _initialized = True
                UpdateUpdateDate()
            Catch ex As Exception
                Logger.ErrorException("Got exception while connecting to database", ex)
                _initialized = False
            End Try
            UpdateControlsStatus()
        End Using
    End Sub
#End Region

#Region "b) Import routines"
    Private Shared Sub StoreIssuers(ByVal conn As SQLiteConnection)
        Logger.Info("StoreIssuers")
        Dim ctAdapter As New countryTableAdapter
        Dim countryTable As DataTable = ctAdapter.GetData()
        Dim itAdapter As New issuerTableAdapter
        Dim issuerTable As DataTable = itAdapter.GetData()

        Dim existingIssuers = issuerTable.AsEnumerable().Select(Function(row) row("Ticker")).ToList
        Dim newIssuers = _tmpBondDataTable.Rows.Cast(Of DataRow).
                Where(Function(row As DataRow) Not existingIssuers.Contains(row("Ticker"))).
                Distinct(New IssuerComparer).ToList

        Dim length = newIssuers.Count, iteration As Integer = 0, finished As Boolean
        If length > 0 Then
            Do
                Dim minRange = iteration * 500
                finished = minRange + 500 > length
                Dim maxRange = If(finished, length, minRange + 500) - 1

                Dim subList = newIssuers.GetRange(minRange, maxRange - minRange + 1)
                Dim insertCommand = subList.Aggregate("INSERT INTO issuer(shortname, issuername, ticker, country_id) VALUES",
                                                        Function(current, item) current + String.Format("('{0}', '{1}', '{2}', {3}), ",
                                                                                                        item("ShortName").Replace("''", """").Replace("'", """"),
                                                                                                        item("IssuerName").Replace("''", """").Replace("'", """"),
                                                                                                        item("Ticker"),
                                                                                                        (From x In countryTable
                                                                                                        Where x("Country") = item("Country")
                                                                                                        Select CInt(x("id"))).First()))
                insertCommand = insertCommand.Substring(0, insertCommand.Length - 2)
                Logger.Trace("INSERT command is {0}", insertCommand)
                Try
                    Dim cmd As New SQLiteCommand(insertCommand, conn)
                    cmd.ExecuteNonQuery()
                Catch ex As Exception
                    Logger.WarnException("Failed to insert", ex)
                End Try

                iteration = iteration + 1
            Loop Until finished
        End If

    End Sub

    Private Shared Sub StoreSimpleTable(ByVal conn As SQLiteConnection, itsName As String, itemTable As DataTable)
        Logger.Info("StoreSimpleTable({0})", itsName)
        Dim addCommand = New SQLiteCommand(String.Format("INSERT INTO [{0}]([{0}]) VALUES(?)", itsName), conn)
        Dim paramName = String.Format("@{0}", itsName)
        addCommand.Parameters.Add(paramName, DbType.AnsiString)

        ' Now we have to select all bonds from the list retrieved and find out if they are
        Dim foundItems = _tmpBondDataTable.Rows.Cast(Of DataRow).Select(Function(r) r(itsName)).Distinct.ToList

        If foundItems IsNot Nothing AndAlso foundItems.Any() Then
            Dim existingItems = itemTable.AsEnumerable().Select(Function(row) row(itsName).ToString).Distinct.ToList
            Dim newItems = (From items In foundItems Where Not existingItems.Contains(items) Select items).ToList

            If newItems IsNot Nothing AndAlso newItems.Any() Then
                For Each newItem As String In newItems
                    Logger.Trace("Storing {0} [{0}]", itsName, newItem)
                    With addCommand
                        .Parameters(paramName).Value = newItem
                        .ExecuteNonQuery()
                    End With
                Next
            End If
        End If
    End Sub

    Private Sub StoreBonds(ByRef conn As SQLiteConnection)
        Logger.Info("StoreBonds")
        Dim btAdapter As New bondTableAdapter
        Dim bondTable As DataTable = btAdapter.GetData()

        Dim ctAdapter As New currencyTableAdapter
        Dim currency As DataTable = ctAdapter.GetData()

        Dim itAdapter As New issuerTableAdapter
        Dim issuers As DataTable = itAdapter.GetData()

        ' Now we have to select all bonds from the list retrieved and find out if they are

        Dim existingBonds = bondTable.AsEnumerable().Select(Function(row) row("Ric").ToString()).ToList
        Dim newBonds = (From issuer In _tmpBondDataTable.Rows.Cast(Of DataRow)() Where Not existingBonds.Contains(issuer("Ric"))).ToList

        Dim insertCommand As String

        Dim length = newBonds.Count, iteration As Integer = 0, finished As Boolean
        If length > 0 Then
            Do
                Dim minRange = iteration * 500
                finished = minRange + 500 > length
                Dim maxRange = If(finished, length, minRange + 500) - 1

                Dim subList = newBonds.GetRange(minRange, maxRange - minRange + 1)

                insertCommand = subList.Aggregate("INSERT INTO Bond(ric, payments, rates, descr, series, issuer_id, " +
                                                                    "currency_id, ir_fixing_ric, issuedate, maturitydate, " +
                                                                    "nextputdate, is_straight, is_putable, is_floater, " +
                                                                    "is_convertible, issue_size, coupon) VALUES",
                    Function(current, item)
                        Dim description = item("Description").Replace("''", """").Replace("'", """")

                        Dim issuerId = GetItemId(issuers, "ticker", item("Ticker"))
                        Dim currencyId = GetItemId(currency, "Currency", item("Currency"))

                        Dim isStraight = IIf(item("IsStraight").ToUpper = "Y", 1, 0)
                        Dim isPutable = IIf(item("IsPutable").ToUpper = "Y", 1, 0)
                        Dim isFloater = IIf(item("IsFloater").ToUpper = "Y", 1, 0)
                        Dim isConvertible = IIf(item("IsConvertible").ToUpper = "Y", 1, 0)

                        Dim issueSize As Long
                        Try
                            issueSize = CLng(item("OriginalAmountIssued"))
                        Catch ex As Exception
                            issueSize = -1
                        End Try

                        Dim theCoupon As Double
                        Try
                            theCoupon = CSng(item("coupon"))
                        Catch ex As Exception
                            theCoupon = 0
                        End Try

                        Return current + String.Format("('{0}', '{1}', '{2}', '{3}', '{4}', {5}, {6}, '{7}', '{8}', '{9}', '{10}', {11}, {12}, {13}, {14}, {15}, '{16}'), ",
                                                        item("Ric"), item("Payments"), item("Rates"), description, item("Series"), issuerId, currencyId, item("IndexRIC"),
                                                        item("IssueDate"), item("MaturityDate"), item("NextPutDate"), isStraight, isPutable, isFloater, isConvertible, issueSize, theCoupon)
                    End Function)


                insertCommand = insertCommand.Substring(0, insertCommand.Length - 2)
                Logger.Trace("INSERT command is {0}", insertCommand)
                Try
                    Dim cmd As New SQLiteCommand(insertCommand, conn)
                    cmd.ExecuteNonQuery()
                Catch ex As Exception
                    Logger.WarnException("Failed to insert", ex)
                End Try

                iteration = iteration + 1
            Loop Until finished
            _initialized = True
            UpdateControlsStatus()
        End If
    End Sub

    Private Shared Function GetItemId(ByVal itemTable As DataTable, itemName As String, ByVal itemValue As String) As Integer
        Dim res = From x In itemTable Where x(itemName) = itemValue Select CInt(x("id"))

        If res Is Nothing OrElse res.Count() = 0 Then
            Return Nothing
        Else
            If res.Count > 1 Then
                Logger.Warn("{0} with {1} in {2} has {3} values: {4}",
                            itemName, itemValue, itemTable.TableName, res.Count,
                            res.ToList.Aggregate("", Function(sum, item) sum + ", " + item))
            End If
            Return CInt(res.First)
        End If
    End Function
#End Region

#Region "b) Store update date"
    Private Shared Sub UpdateUpdateDate()
        Logger.Info("DataAlreadyLoaded")
        Dim settingsTableAdapter = New settingsTableAdapter
        Dim settingsDataTable = settingsTableAdapter.GetData
        If settingsDataTable.Count > 0 Then
            Dim dataRow = settingsDataTable.First()
            dataRow.lastupdatedate = DateTime.Today.ToShortDateString()
            settingsTableAdapter.ClearAllData()
            settingsTableAdapter.Insert(DateTime.Today.ToShortDateString)
        Else
            settingsTableAdapter.Insert(DateTime.Today.ToShortDateString)
        End If
    End Sub
#End Region
#End Region
End Class
