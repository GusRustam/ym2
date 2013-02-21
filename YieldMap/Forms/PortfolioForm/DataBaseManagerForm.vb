Imports System.Data
Imports System.Windows.Forms
Imports System.Drawing
Imports DbManager
Imports YieldMap.Commons
Imports NLog

Namespace Forms.PortfolioForm
    Public Class DataBaseManagerForm
        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(DataBaseManagerForm))

#Region "Form-wide events"
        Private Sub UdlPageResize(sender As Object, e As EventArgs) Handles UDL_Page.Resize
            With MainTabControl
                .Top = 0
                .Left = 0
                .Width = ClientSize.Width
                .Height = ClientSize.Height - 50
            End With
        End Sub

        Private Sub DataBaseManagerFormLoad(sender As Object, e As EventArgs) Handles MyBase.Load
            FillChains()
            ChainsDGV.Columns(0).Visible = False

            RefreshPortfolioList()
            RefreshPortfolioGrids(-1)
            ConstituentsDGW.Columns(0).Visible = False

            RefreshList()
            RefreshGrid(-1)

            With MainTabControl
                .Top = 0
                .Left = 0
                .Width = ClientSize.Width
                .Height = ClientSize.Height - 50
            End With

            MessageListBox.Items.Clear()
            ReloadBondsButton.Enabled = AppMainForm.Initialized
            If Not AppMainForm.Initialized Then MessageListBox.Items.Add("You need to connect to Eikon in order to update database")
            DbUpdatedLabel.Text = String.Format("{0:dd MMMM yyyy}", LastDbUpdate)
        End Sub
#End Region

#Region "User-Defined Lists methods"
        Private ReadOnly _hawserTA As New hawserTableAdapter

        Private Sub RefreshList()
            HawserTableAdapter.Fill(BondsDataSet.hawser)
            If ListOfList.Rows.Count = 1 Then ListOfList.Rows(0).Selected = True
            ConstituentsDGW.Columns(0).Visible = False
            ListOfList.Columns(0).Visible = False
        End Sub

        Private Sub RefreshGrid(filterId As Integer)
            HawserDataTableAdapter.Fill(BondsDataSet.HawserData)
            HawserDataBindingSource.Filter = String.Format("hawser_id = {0:D}", filterId)
            ConstituentsDGW.Columns(0).Visible = False
        End Sub

        Private Sub AddListButtonClick(sender As Object, e As EventArgs) Handles AddListButton.Click
            Dim f = New NewChainForm
            f.TheLayout.RowStyles(1).Height = 0
            f.Text = "New user-defined list"
            f.RICLabel.Text = "List name"
            f.CurveCheckBox.Checked = False

            If f.ShowDialog() = DialogResult.OK Then
                Dim newName = f.RICTextBox.Text
                Dim newColor = f.ColorsComboBox.Text
                Dim crv = f.CurveCheckBox.Checked
                Dim fieldSetId = f.FieldLayoutComboBox.SelectedValue
                If newName <> "" Then
                    Try
                        _hawserTA.InsertNew(newName, newColor, crv, fieldSetId)
                        RefreshList()
                        ListOfList.Rows(0).Selected = True
                    Catch ex As Exception
                        MsgBox("Failed to add new list!" + Environment.NewLine + "Message is: " + ex.Message, vbOKOnly, "Create new list")
                        Logger.DebugException("Failure", ex)
                        Logger.Debug("Exception = {0}", ex.ToString())
                    End Try
                End If
            End If
        End Sub

        Private Sub RenameButtonClick(sender As Object, e As EventArgs) Handles RenameButton.Click
            If ListOfList.SelectedRows.Count > 0 Then
                Dim selectedRow = ListOfList.SelectedRows(0)
                StartListEdit(selectedRow)
            Else
                MsgBox("Please select user-defined list to rename")
            End If
        End Sub

        Private Sub StartListEdit(ByVal selectedRow As DataGridViewRow)
            Dim f = New NewChainForm
            f.TheLayout.RowStyles(1).Height = 0
            f.Text = "Edit user-defined list"
            f.RICLabel.Text = "List name"
            f.RICTextBox.Text = selectedRow.Cells(1).Value.ToString()
            f.ColorsComboBox.Text = selectedRow.Cells(2).Value.ToString()
            f.CurveCheckBox.Checked = CBool(selectedRow.Cells(3).Value)
            f.fieldSetId = CLng(selectedRow.Cells(4).Value)
            If f.ShowDialog() = DialogResult.OK Then
                Dim newName = f.RICTextBox.Text
                Dim newColor = f.ColorsComboBox.Text
                Dim crv = f.CurveCheckBox.Checked
                Dim layoutId = f.FieldLayoutComboBox.SelectedValue
                If newName <> "" Then
                    _hawserTA.RenameById(newName, newColor, crv, layoutId, CInt(selectedRow.Cells(0).Value))
                    RefreshList()
                    RefreshGrid(-1)
                End If
            End If
        End Sub

        Private Sub DeleteListButtonClick(sender As Object, e As EventArgs) Handles DeleteListButton.Click
            If ListOfList.SelectedRows.Count > 0 Then

                Dim selectedRow = ListOfList.SelectedRows(0)
                If MsgBox("Do you wish to delete list [" + selectedRow.Cells(1).Value.ToString() + "]?", vbYesNo, "Remove a list") = vbYes Then
                    Try
                        _hawserTA.DeleteById(selectedRow.Cells(0).Value)
                        Call (New hawser_to_bondTableAdapter).DeleteLinkByHawser(selectedRow.Cells(0).Value)
                    Catch ex As Exception
                        MsgBox("Failed to remove list." + Environment.NewLine + "Message is: " + ex.Message, vbOKOnly, "Remove a list")
                        Logger.ErrorException("Failure", ex)
                    End Try
                End If
            Else
                MsgBox("Please select list to remove")
            End If
            RefreshList()
        End Sub

        Private Sub ListOfListSelectionChanged(sender As Object, e As EventArgs) Handles ListOfList.SelectionChanged
            If ListOfList.SelectedRows.Count > 0 Then
                Dim selectedRow = ListOfList.SelectedRows(0)
                RefreshGrid(selectedRow.Cells(0).Value)
            Else
                RefreshGrid(-1)
            End If
        End Sub

        Private Sub AddItemButtonClick(sender As Object, e As EventArgs) Handles AddItemButton.Click
            If ListOfList.Rows.Count > 0 Then
                Dim bondSelector As New BondSelectorForm
                If bondSelector.ShowDialog() = DialogResult.OK Then
                    Dim selectedRow As DataGridViewRow
                    If ListOfList.SelectedRows.Count > 0 Then
                        selectedRow = ListOfList.SelectedRows(0)
                    Else
                        selectedRow = ListOfList.Rows(0)
                    End If
                    Dim selectedListID = selectedRow.Cells(0).Value
                    Dim htbTA As New hawser_to_bondTableAdapter

                    If bondSelector.SelectedRICs.Count > 10 Then
                        Dim ind As Integer = 0
                        Dim progress As New ProgressForm(bondSelector.SelectedRICs.Count)
                        progress.Show()
                        bondSelector.SelectedRICs.ForEach(
                            Sub(aRic)
                                progress.OnItem(aRic, ind)
                                htbTA.InsertLink(selectedListID, aRic)
                                ind = ind + 1
                            End Sub)
                        progress.Close()
                    Else
                        bondSelector.SelectedRICs.ForEach(Sub(aRic) htbTA.InsertLink(selectedListID, aRic))
                    End If
                    RefreshGrid(selectedListID)
                End If
            Else
                MsgBox("Please first select a list for items to add")
            End If
        End Sub

        Private Sub RemoveItemButtonClick(sender As Object, e As EventArgs) Handles RemoveItemButton.Click
            If ConstituentsDGW.SelectedRows.Count > 0 Then
                Dim selectedRICs = (From aRow As DataGridViewRow In ConstituentsDGW.SelectedRows Select CStr(aRow.Cells(1).Value)).ToList
                Dim selectedRow = ListOfList.SelectedRows(0)
                Dim selectedListID = selectedRow.Cells(0).Value
                Dim htbTA As New hawser_to_bondTableAdapter
                If selectedRICs.Count > 10 Then
                    Dim ind As Integer = 0
                    Dim progress As New ProgressForm(selectedRICs.Count, False)
                    progress.Show()
                    selectedRICs.ForEach(
                        Sub(aRic)
                            progress.OnItem(aRic, ind)
                            htbTA.RemoveLink(aRic, selectedListID)
                            ind = ind + 1
                        End Sub)
                    progress.Close()
                Else
                    selectedRICs.ForEach(Sub(aRic) htbTA.RemoveLink(aRic, selectedListID))
                End If
                RefreshGrid(selectedListID)
            Else
                MsgBox("Please select items to remove in the grid")
            End If
        End Sub

        Private Sub ListOfListCellFormatting(sender As System.Object, e As DataGridViewCellFormattingEventArgs) Handles ListOfList.CellFormatting
            If ListOfList.Columns(e.ColumnIndex).HeaderText.ToLower() = "color" Then
                Dim theColor As KnownColor
                If TypeOf e.Value Is String AndAlso KnownColor.TryParse(e.Value, theColor) Then
                    e.CellStyle.BackColor = Drawing.Color.FromKnownColor(theColor)
                    e.CellStyle.ForeColor = Drawing.Color.FromKnownColor(theColor)
                End If
            End If
        End Sub

        Private Sub ListOfListCellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles ListOfList.CellDoubleClick
            Dim selectedRow = ListOfList.Rows(e.RowIndex)
            StartListEdit(selectedRow)
        End Sub

        Private Sub ConstituentsDgwCellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles ConstituentsDGW.CellDoubleClick
            Dim selectedRIC = ConstituentsDGW.Rows(e.RowIndex).Cells(1).Value
            Dim selectedRow = ListOfList.SelectedRows(0)
            Dim selectedListID = selectedRow.Cells(0).Value
            Dim htbTA As New hawser_to_bondTableAdapter
            htbTA.RemoveLink(selectedRIC, selectedListID)
            RefreshGrid(selectedListID)
        End Sub

        Private Sub ConstituentsDgwSelectionChanged(sender As Object, e As EventArgs) Handles ConstituentsDGW.SelectionChanged
            RemoveItemButton.Enabled = ChainsDGV.SelectedRows.Count > 0
        End Sub
#End Region

#Region "Portfolio editor"
        Private ReadOnly _portfolioTA As New portfolioTableAdapter

        Private Sub PorfolioElementsDgvCellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles PorfolioElementsDGV.CellFormatting
            If PorfolioElementsDGV.Columns(e.ColumnIndex).HeaderText.ToLower() = "color" Then
                Dim theColor As KnownColor
                If TypeOf e.Value Is String AndAlso KnownColor.TryParse(e.Value, theColor) Then
                    e.CellStyle.BackColor = Drawing.Color.FromKnownColor(theColor)
                    e.CellStyle.ForeColor = Drawing.Color.FromKnownColor(theColor)
                End If
            End If
        End Sub

        Private Sub PortUserListDgvCellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles PortUserListDGV.CellFormatting
            If PortUserListDGV.Columns(e.ColumnIndex).HeaderText.ToLower() = "color" Then
                Dim theColor As KnownColor
                If TypeOf e.Value Is String AndAlso KnownColor.TryParse(e.Value, theColor) Then
                    e.CellStyle.BackColor = Drawing.Color.FromKnownColor(theColor)
                    e.CellStyle.ForeColor = Drawing.Color.FromKnownColor(theColor)
                End If
            End If
        End Sub

        Private Sub PortChainsDgvCellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles PortChainsDGV.CellFormatting
            If PortChainsDGV.Columns(e.ColumnIndex).HeaderText.ToLower() = "color" Then
                Dim theColor As KnownColor
                If TypeOf e.Value Is String AndAlso KnownColor.TryParse(e.Value, theColor) Then
                    e.CellStyle.BackColor = Drawing.Color.FromKnownColor(theColor)
                    e.CellStyle.ForeColor = Drawing.Color.FromKnownColor(theColor)
                End If
            End If

        End Sub

        Private Sub RefreshPortfolioList()
            PortfolioTableAdapter.Fill(BondsDataSet.portfolio)
            If PortfoliosListBox.Items.Count = 1 Then PortfoliosListBox.SelectedIndex = 0
            ConstituentsDGW.Columns(0).Visible = False
        End Sub

        Private Sub RefreshPortfolioGrids(filterId As Integer, Optional hName As String = "")
            PortfolioUnitedTableAdapter.Fill(BondsDataSet.PortfolioUnited)
            HawsersInPortfolioTableAdapter.Fill(BondsDataSet.HawsersInPortfolio)
            ChainsInPortfolioTableAdapter.Fill(BondsDataSet.ChainsInPortfolio)

            ConstPortLabel.Text = IIf(filterId > 0, "Constituents for " + hName, "")
            Dim filterStr = String.Format("pid = {0:D}", filterId)

            PortfolioUnitedBindingSource.Filter = filterStr
            ChainsInPortfolioBindingSource.Filter = filterStr
            HawsersInPortfolioBindingSource.Filter = filterStr

            PorfolioElementsDGV.Columns(0).Visible = False
            PortUserListDGV.Columns(0).Visible = False
            PortChainsDGV.Columns(0).Visible = False

            PortUserListDGV.Columns(1).Visible = False
            PortChainsDGV.Columns(1).Visible = False

        End Sub

        Private Sub AddPortfolioButtonClick(sender As Object, e As EventArgs) Handles AddPortfolioButton.Click
            Dim newName = InputBox("New portfolio name", "Create new portfolio", "New portfolio")
            If newName <> "" Then
                Try
                    _portfolioTA.Insert(newName, False)
                    RefreshPortfolioList()
                    PortfoliosListBox.SelectedIndex = 0
                Catch ex As Exception
                    MsgBox("Failed to add new portfolio!" + Environment.NewLine + "Message is: " + ex.Message, vbOKOnly,
                           "Create new list")
                    Logger.DebugException("Failure", ex)
                End Try
            End If
        End Sub

        Private Sub RenamePortfolioButtonClick(sender As Object, e As EventArgs) Handles RenamePortfolioButton.Click
            If PortfoliosListBox.SelectedIndex >= 0 Then
                Dim dataRowView = CType(PortfoliosListBox.SelectedItem, DataRowView)
                Dim newName As String = InputBox("New name", "Rename [" + dataRowView("portfolio_name") + "]",
                                                 "New name")
                If newName <> "" Then
                    _portfolioTA.RenameById(newName, CInt(dataRowView("id")))
                End If
                RefreshPortfolioList()
                RefreshPortfolioGrids(-1)
            Else
                MsgBox("Please select user-defined list to rename")
            End If
        End Sub

        Private Sub PortfoliosListBoxClick(sender As Object, e As EventArgs) Handles PortfoliosListBox.Click
            If PortfoliosListBox.SelectedIndex >= 0 Then
                Dim dataRowView = CType(PortfoliosListBox.SelectedItem, DataRowView)
                RefreshPortfolioGrids(dataRowView("Id"), dataRowView("portfolio_name"))
            Else
                RefreshPortfolioGrids(-1)
            End If
        End Sub

        Private Sub DeletePortfolioButtonClick(sender As Object, e As EventArgs) Handles DeletePortfolioButton.Click
            If PortfoliosListBox.SelectedIndex >= 0 Then
                Dim dataRowView = CType(PortfoliosListBox.SelectedItem, DataRowView)
                If _
                    MsgBox("Do you wish to delete portfolio [" + dataRowView("portfolio_name") + "]?", vbYesNo,
                           "Remove a portfolio") = vbYes Then
                    Try
                        _portfolioTA.RemoveById(dataRowView("id"))
                        Call (New portfolio_to_hawserTableAdapter).DeleteLinkByPortfolio(dataRowView("id"))
                        Call (New portfolio_to_chainTableAdapter).DeleteLinkByPortfolio(dataRowView("id"))
                    Catch ex As Exception
                        MsgBox("Failed to remove portfolio." + Environment.NewLine + "Message is: " + ex.Message,
                               vbOKOnly, "Remove a portfolio")
                        Logger.ErrorException("Failure", ex)
                    End Try
                End If
            Else
                MsgBox("Please select portfolio to remove")
            End If
            RefreshPortfolioList()
        End Sub

        Private Sub AddChainButtonClick(sender As Object, e As EventArgs) Handles AddChainButton.Click
            If PortfoliosListBox.Items.Count > 0 Then
                Dim chainSelector As New ChainSelectorForm
                If chainSelector.ShowDialog() = DialogResult.OK Then
                    Dim dataRowView As DataRowView
                    Dim include = chainSelector.IncludeCB.Checked
                    If PortfoliosListBox.SelectedIndex >= 0 Then
                        dataRowView = CType(PortfoliosListBox.SelectedItem, DataRowView)
                    Else
                        dataRowView = CType(PortfoliosListBox.Items(0), DataRowView)
                    End If
                    Dim selectedListID = dataRowView("id")
                    Dim aNamre = dataRowView("portfolio_name")

                    Dim htbTA As New portfolio_to_chainTableAdapter
                    chainSelector.SelectedChains.ForEach(Sub(id) htbTA.InsertLink(selectedListID, id, include))
                    RefreshPortfolioGrids(selectedListID, aNamre)
                End If
            Else
                MsgBox("Please first select a list for items to add")
            End If
        End Sub

        Private Sub AddHawserButtonClick(sender As Object, e As EventArgs) Handles AddHawserButton.Click
            If PortfoliosListBox.Items.Count > 0 Then
                Dim hawserSelector As New HawserSelectorForm
                If hawserSelector.ShowDialog() = DialogResult.OK Then
                    Dim dataRowView As DataRowView
                    Dim include = hawserSelector.IncludeCB.Checked
                    If PortfoliosListBox.SelectedIndex >= 0 Then
                        dataRowView = CType(PortfoliosListBox.SelectedItem, DataRowView)
                    Else
                        dataRowView = CType(PortfoliosListBox.Items(0), DataRowView)
                    End If
                    Dim selectedListID = dataRowView("id")
                    Dim aName = dataRowView("portfolio_name")

                    Dim htbTA As New portfolio_to_hawserTableAdapter
                    hawserSelector.SelectedHawsers.ForEach(Sub(id) htbTA.InsertLink(selectedListID, id, include))
                    RefreshPortfolioGrids(selectedListID, aName)
                End If
            Else
                MsgBox("Please first select a list for items to add")
            End If
        End Sub

        Private Sub RemoveChainButtonClick(sender As Object, e As EventArgs) Handles RemoveChainButton.Click
            If PortChainsDGV.SelectedRows.Count > 0 Then
                Dim selectedIDs = (From aRow As DataGridViewRow In PortChainsDGV.SelectedRows Select CStr(aRow.Cells(1).Value)).ToList
                Dim dataRowView As DataRowView = CType(PortfoliosListBox.SelectedItem, DataRowView)
                Dim selectedListID = dataRowView("id")
                Dim aName = dataRowView("portfolio_name")
                Dim htbTA As New portfolio_to_chainTableAdapter
                selectedIDs.ForEach(Sub(id) htbTA.RemoveLink(id, selectedListID))
                RefreshPortfolioGrids(selectedListID, aName)
            Else
                MsgBox("Please select items to remove in the grid")
            End If
        End Sub

        Private Sub RemoveHawserButtonClick(sender As Object, e As EventArgs) Handles RemoveHawserButton.Click
            If PortUserListDGV.SelectedRows.Count > 0 Then
                Dim selectedIDs = (From aRow As DataGridViewRow In PortUserListDGV.SelectedRows Select CStr(aRow.Cells(1).Value)).ToList
                Dim dataRowView As DataRowView = CType(PortfoliosListBox.SelectedItem, DataRowView)
                Dim selectedListID = dataRowView("id")
                Dim aName = dataRowView("portfolio_name")
                Dim htbTA As New portfolio_to_hawserTableAdapter
                selectedIDs.ForEach(Sub(id) htbTA.RemoveLink(id, selectedListID))
                RefreshPortfolioGrids(selectedListID, aName)
            Else
                MsgBox("Please select items to remove in the grid")
            End If
        End Sub
#End Region

#Region "Chain editor"
        Private Sub FillChains()
            ChainTableAdapter.Fill(BondsDataSet.chain)
        End Sub

        Private Sub RemoveChainClick(sender As Object, e As EventArgs) Handles RemoveChain.Click
            If ChainsDGV.SelectedRows.Count > 0 Then
                Dim selectedRow = ChainsDGV.SelectedRows(0)
                Dim id = CInt(selectedRow.Cells.Item(0).Value.ToString())
                Dim theRic = selectedRow.Cells.Item(1).Value.ToString()
                If MsgBox(String.Format("Removing chain [{0}]", theRic), MsgBoxStyle.YesNo, "Please confirm") = MsgBoxResult.Yes Then
                    ChainTableAdapter.RemoveItem(id)
                    FillChains()
                End If
            End If
        End Sub

        Private Sub ChainsDgvCellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles ChainsDGV.CellDoubleClick
            Dim selectedRow = ChainsDGV.Rows(e.RowIndex)
            StartChainEdit(selectedRow)
        End Sub

        Private Sub StartChainEdit(ByVal selectedRow As DataGridViewRow)
            Dim id = CInt(selectedRow.Cells.Item(0).Value.ToString())
            Dim theRic = selectedRow.Cells.Item(1).Value.ToString()
            Dim descr = selectedRow.Cells.Item(2).Value.ToString()
            Dim clr = selectedRow.Cells.Item(3).Value.ToString()
            Dim crv = CBool(selectedRow.Cells.Item(4).Value)


            Dim f = New NewChainForm
            f.RICTextBox.Text = theRic
            f.DescrTextBox.Text = descr
            f.RICTextBox.Text = theRic
            f.ColorsComboBox.Text = clr
            f.CurveCheckBox.Checked = crv
            f.fieldSetId = CLng(selectedRow.Cells.Item(5).Value)

            If f.ShowDialog() = DialogResult.OK Then
                theRic = f.RICTextBox.Text
                descr = f.DescrTextBox.Text
                clr = f.ColorsComboBox.Text
                crv = f.CurveCheckBox.Checked
                Dim layoutId = f.FieldLayoutComboBox.SelectedValue
                ChainTableAdapter.UpdateItem(theRic, descr, clr, crv, layoutId, id)
                FillChains()
            End If
        End Sub

        Private Sub AddChainClick(sender As System.Object, e As EventArgs) Handles AddChain.Click
            Dim f = New NewChainForm
            If f.ShowDialog() = DialogResult.OK Then
                Dim newRic = f.RICTextBox.Text
                Dim descr = f.DescrTextBox.Text
                Dim clr = f.ColorsComboBox.SelectedItem.ToString()
                Dim crv = f.CurveCheckBox.Checked
                Dim layoutId = f.FieldLayoutComboBox.SelectedValue
                ChainTableAdapter.InsertNew(newRic, descr, clr, crv, layoutId)
                FillChains()
            End If
        End Sub

        Private Sub EditChainClick(sender As System.Object, e As EventArgs) Handles EditChain.Click
            If ChainsDGV.SelectedRows.Count > 0 Then
                Dim selectedRow = ChainsDGV.SelectedRows(0)
                StartChainEdit(selectedRow)
            End If

        End Sub

        Private Sub ChainsDgvCellFormatting(sender As System.Object, e As DataGridViewCellFormattingEventArgs) Handles ChainsDGV.CellFormatting
            If ChainsDGV.Columns(e.ColumnIndex).HeaderText.ToLower() = "color" Then
                Dim theColor As KnownColor
                If TypeOf e.Value Is String AndAlso KnownColor.TryParse(e.Value, theColor) Then
                    e.CellStyle.BackColor = Drawing.Color.FromKnownColor(theColor)
                    e.CellStyle.ForeColor = Drawing.Color.FromKnownColor(theColor)

                End If
            End If
        End Sub
#End Region

        Private Sub InformOnProgress(ByVal message As String)
            GuiAsync(Sub()
                         MessageListBox.Items.Add(message)
                         MessageListBox.SelectedIndex = MessageListBox.Items.Count - 1
                     End Sub)
        End Sub

        Private Sub ReloadBondsButtonClick(sender As Object, e As EventArgs) Handles ReloadBondsButton.Click
            MessageListBox.Items.Clear()
            Dim initR = BondsDatabaseManager.GetInstance
            AddHandler initR.Success, Sub()
                                          InformOnProgress("Database initialized successfully")
                                          DbUpdatedLabel.Text = String.Format("{0:dd MMMM yyyy}", LastDbUpdate)
                                      End Sub
            AddHandler initR.Failure,
                Sub(ex As Exception)
                    InformOnProgress("Failed to initialize database")
                    If MsgBox("Failed to initialize database. Would you like to report an error to the developer?", vbYesNo, "Database error") = vbYes Then
                        SendErrorReport("Yield Map Database Error", "Exception: " + ex.ToString() + Environment.NewLine + Environment.NewLine + GetEnvironment())
                    End If
                End Sub
            AddHandler initR.Progress, AddressOf InformOnProgress
            initR.UpdateAllChains()
        End Sub
    End Class
End Namespace