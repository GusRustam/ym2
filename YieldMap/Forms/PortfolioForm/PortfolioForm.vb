Imports DbManager
Imports DbManager.Bonds
Imports System.Runtime.InteropServices
Imports NLog

Namespace Forms.PortfolioForm

    Public Class PortfolioForm
        Private Shared ReadOnly PortfolioManager As IPortfolioManager = DbManager.PortfolioManager.GetInstance()
        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(PortfolioForm))
        Private Shared ReadOnly BondsLoader As IBondsLoader = Bonds.BondsLoader.Instance

        Private Sub PortfolioForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            BondsTableView.DataSource = BondsLoader.GetBondsTable()
            If PortfolioTree.ImageList Is Nothing Then
                PortfolioTree.ImageList = New ImageList
                PortfolioTree.ImageList.Images.Add("folder", My.Resources.folder)
                PortfolioTree.ImageList.Images.Add("portfolio", My.Resources.briefcase)
            End If
            RefreshPortfolioTree()
            RefreshChainsLists()
        End Sub

        Private Shared Sub ColorCellFormatting(ByVal sender As Object, ByVal e As DataGridViewCellFormattingEventArgs) Handles PortfolioChainsListsGrid.CellFormatting, PortfolioItemsGrid.CellFormatting, ChainsListsGrid.CellFormatting
            Dim dgv = TryCast(sender, DataGridView)
            If dgv Is Nothing Then Return
            If dgv.Columns(e.ColumnIndex).DataPropertyName = "Color" Then
                Dim theColor As KnownColor
                If TypeOf e.Value Is String AndAlso KnownColor.TryParse(e.Value, theColor) Then
                    e.CellStyle.BackColor = Color.FromKnownColor(theColor)
                    e.CellStyle.ForeColor = Color.FromKnownColor(theColor)
                End If
            End If
        End Sub

#Region "Portfolio TAB"
        Private _dragNode As TreeNode
        Private _currentItem As PortfolioItemDescription
        Private _flag As Boolean

        Private Property CurrentItem As PortfolioItemDescription
            Get
                Return _currentItem
            End Get
            Set(ByVal value As PortfolioItemDescription)
                _currentItem = value
                RefreshPortfolioData()
            End Set
        End Property

        Private Sub PortSourcesCheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ChainsCB.CheckedChanged, ListsCB.CheckedChanged
            RefreshPortfolioData()
        End Sub

        Private Sub PortItemsCheckChanged(ByVal sender As Object, ByVal e As EventArgs) Handles AllRB.CheckedChanged, SeparateRB.CheckedChanged
            RefreshPortfolioData()
        End Sub

        Private Sub RefreshPortfolioData()
            If CurrentItem Is Nothing Then Return

            If CurrentItem.IsFolder Then
                PortfolioChainsListsGrid.Columns.Clear()
                PortfolioChainsListsGrid.Rows.Clear()
                PortfolioItemsGrid.Columns.Clear()
                PortfolioItemsGrid.Rows.Clear()
            Else
                Dim descr = PortfolioManager.GetPortfolioStructure(CurrentItem.Id)
                PortfolioChainsListsGrid.DataSource = descr.Sources(
                    If(ChainsCB.Checked, PortfolioStructure.Chain, 0) Or
                    If(ListsCB.Checked, PortfolioStructure.List, 0))

                PortfolioItemsGrid.DataSource = descr.Rics(AllRB.Checked)
            End If
        End Sub

        Private Sub RefreshPortfolioTree(Optional ByVal selId As Long = -1)
            Dim selectedNodeId As String
            If selId = -1 Then
                If PortfolioTree.SelectedNode IsNot Nothing Then
                    Dim descr = CType(PortfolioTree.SelectedNode.Tag, PortfolioItemDescription)
                    selectedNodeId = descr.Id
                End If
            Else
                selectedNodeId = selId
            End If

            PortfolioTree.Nodes.Clear()

            If PortfolioManager.PortfoliosValid() Then
                PortfolioTree.BeginUpdate()
                Dim newSelNode = AddPortfoliosByFolder("", selectedNodeId)
                If newSelNode IsNot Nothing Then
                    newSelNode.EnsureVisible()
                    PortfolioTree.SelectedNode = newSelNode
                End If
                PortfolioTree.EndUpdate()
            Else
                MessageBox.Show("Portfolios are corrupted, unable to show", "Portfolios...", MessageBoxButtons.OK, MessageBoxIcon.Hand)
            End If

        End Sub

        Private Function AddPortfoliosByFolder(ByVal theId As String, ByVal selId As String, Optional ByVal whereTo As TreeNode = Nothing) As TreeNode
            Dim portMan As PortfolioManager = PortfolioManager
            Dim res As TreeNode = If(theId = selId, whereTo, Nothing)
            Dim descrs = portMan.GetPortfoliosByFolder(theId)

            For Each descr In descrs
                Dim img = If(descr.IsFolder, "folder", "portfolio")
                Dim newNode As TreeNode
                If whereTo IsNot Nothing Then
                    newNode = whereTo.Nodes.Add(descr.Id, descr.Name, img, img)
                Else
                    newNode = PortfolioTree.Nodes.Add(descr.Id, descr.Name, img, img)
                End If
                newNode.Tag = descr
                If descr.IsFolder Then
                    Dim tmp = AddPortfoliosByFolder(descr.Id, selId, newNode)
                    If res Is Nothing AndAlso tmp IsNot Nothing Then res = tmp
                Else
                    If descr.Id = selId Then res = newNode
                End If
            Next
            Return res
        End Function

        Private Sub PortfolioTree_DblClick(ByVal sender As Object, ByVal e As EventArgs) Handles PortfolioTree.DoubleClick
            Dim mea = TryCast(e, MouseEventArgs)
            Dim node = PortfolioTree.GetNodeAt(mea.Location)
            If node Is Nothing Then Return
            DoRename(node)
        End Sub

        Private Sub RenameToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles RenameToolStripMenuItem.Click
            Dim node = CType(PortTreeCM.Tag, TreeNode)
            If node Is Nothing Then Return
            DoRename(node)
        End Sub

        Private Sub DoRename(ByVal node As TreeNode)
            PortfolioTree.SelectedNode = node

            Dim adder = New AddPortfolioForm
            adder.EditMode = True
            adder.NewName.Text = node.Text
            adder.ItIsPortfolio = Not CType(node.Tag, PortfolioItemDescription).IsFolder

            If adder.ShowDialog() = DialogResult.OK Then
                Dim portDescr As PortfolioItemDescription
                portDescr = CType(node.Tag, PortfolioItemDescription)
                If portDescr Is Nothing Then
                    MessageBox.Show("Failed to update name", "Name edit", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    If portDescr.IsFolder Then
                        PortfolioManager.SetFolderName(portDescr.Id, adder.NewName.Text)
                    Else
                        PortfolioManager.SetPortfolioName(portDescr.Id, adder.NewName.Text)
                    End If
                    RefreshPortfolioTree()
                End If
            End If
        End Sub

        Private Sub PortfolioTree_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles PortfolioTree.NodeMouseClick
            If e.Button = MouseButtons.Right Then
                PortTreeCM.Tag = e.Node
                PortTreeCM.Show(PortfolioTree, e.Location)
            Else
                Dim temp = TryCast(e.Node.Tag, PortfolioItemDescription)
                CurrentItem = If(temp IsNot Nothing AndAlso Not temp.IsFolder, temp, Nothing)
            End If
            _flag = False
        End Sub

        Private Sub PortfolioTree_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles PortfolioTree.MouseUp
            If Not _flag Then
                _flag = True
                Return
            End If
            If e.Button = MouseButtons.Right Then
                PortTreeCM.Tag = Nothing
                PortTreeCM.Show(PortfolioTree, e.Location)
            End If
        End Sub

        Private Sub AddToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AddToolStripMenuItem.Click
            Dim node = TryCast(PortTreeCM.Tag, TreeNode)

            Dim adder As New AddPortfolioForm With {
                .EditMode = False,
                .ItIsPortfolio = True
            }

            Dim theId As String

            If node IsNot Nothing Then
                PortfolioTree.SelectedNode = node
                Dim descr = TryCast(node.Tag, PortfolioItemDescription)
                If descr Is Nothing Then Return
                theId = descr.Id
            End If

            If adder.ShowDialog() = DialogResult.OK Then
                Dim newId As Long
                If adder.ItsFolder.Checked Then
                    newId = PortfolioManager.AddFolder(adder.NewName.Text, theId)
                Else
                    newId = PortfolioManager.AddPortfolio(adder.NewName.Text, theId)
                End If
                RefreshPortfolioTree(newId)
            End If
        End Sub

        Private Sub DeleteToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DeleteToolStripMenuItem.Click
            Dim node = TryCast(PortTreeCM.Tag, TreeNode)
            If node Is Nothing Then Return

            Dim descr = TryCast(node.Tag, PortfolioItemDescription)
            If descr Is Nothing Then Return
            If MessageBox.Show("Are you sure you would like to delete an item permanently?", "Delete...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                If descr.IsFolder Then
                    PortfolioManager.DeleteFolder(descr.Id)
                Else
                    PortfolioManager.DeletePortfolio(descr.Id)
                End If
                RefreshPortfolioTree()
            End If
        End Sub

        Private Sub PortfolioTree_ItemDrag(ByVal sender As Object, ByVal e As ItemDragEventArgs) Handles PortfolioTree.ItemDrag
            _dragNode = e.Item
            DoDragDrop(e.Item, DragDropEffects.Copy Or DragDropEffects.Move)
        End Sub

        <DllImport("user32.dll")>
        Private Shared Function GetKeyState(ByVal key As Keys) As Short
        End Function

        Private Sub PortfolioTree_DragOver(ByVal sender As Object, ByVal e As DragEventArgs) Handles PortfolioTree.DragOver
            Dim pos = PortfolioTree.PointToClient(New Point(e.X, e.Y))
            Dim node = PortfolioTree.GetNodeAt(pos)
            Dim copy = GetKeyState(Keys.ControlKey) < 0
            If node Is Nothing Then
                e.Effect = If(copy, DragDropEffects.Copy, DragDropEffects.Move)
            Else
                node.Expand()
                Dim descr = TryCast(node.Tag, PortfolioItemDescription)
                If IsChildOf(_dragNode, node) OrElse (descr IsNot Nothing AndAlso Not descr.IsFolder) Then
                    e.Effect = DragDropEffects.None
                Else
                    e.Effect = If(copy, DragDropEffects.Copy, DragDropEffects.Move)
                End If
            End If
        End Sub

        Private Shared Function IsChildOf(ByVal ofWhat As TreeNode, ByVal who As TreeNode) As Boolean
            If ofWhat Is Nothing Then Return False
            Do
                If ofWhat.Equals(who) Then Return True
                If ofWhat.Equals(who.Parent) Then Return True
                If who.Parent IsNot Nothing AndAlso TypeOf who.Parent Is TreeNode Then
                    who = who.Parent
                Else
                    Return False
                End If
            Loop
        End Function

        Private Sub PortfolioTree_DragDrop(ByVal sender As Object, ByVal e As DragEventArgs) Handles PortfolioTree.DragDrop
            Dim dragDescr = TryCast(_dragNode.Tag, PortfolioItemDescription)
            If dragDescr Is Nothing Then Return
            Dim copy = GetKeyState(Keys.ControlKey) < 0

            Dim pos = PortfolioTree.PointToClient(New Point(e.X, e.Y))
            Dim node = PortfolioTree.GetNodeAt(pos)
            Dim resId As String
            If node Is Nothing Then
                If copy Then
                    resId = PortfolioManager.CopyItemToTop(dragDescr.Id)
                Else
                    resId = PortfolioManager.MoveItemToTop(dragDescr.Id)
                End If
            Else
                Dim descr = TryCast(node.Tag, PortfolioItemDescription)
                If descr Is Nothing OrElse Not descr.IsFolder Then Return
                If copy Then
                    resId = PortfolioManager.CopyItemToFolder(dragDescr.Id, descr.Id)
                Else
                    resId = PortfolioManager.MoveItemToFolder(dragDescr.Id, descr.Id)
                End If
            End If
            RefreshPortfolioTree(resId)
        End Sub
#End Region

#Region "Data TAB"
        Private Sub TableChooserList_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TableChooserList.SelectedIndexChanged
            If TableChooserList.SelectedIndex >= 0 Then
                Select Case TableChooserList.Items(TableChooserList.SelectedIndex).ToString()
                    Case "Bonds" : BondsTableView.DataSource = BondsLoader.GetBondsTable()
                    Case "Coupons" : BondsTableView.DataSource = BondsLoader.GetCouponsTable()
                    Case "FRNs" : BondsTableView.DataSource = BondsLoader.GetFRNsTable()
                    Case "Issue ratings" : BondsTableView.DataSource = BondsLoader.GetIssueRatingsTable()
                    Case "Issuer ratings" : BondsTableView.DataSource = BondsLoader.GetIssuerRatingsTable()
                    Case "Rics" : BondsTableView.DataSource = BondsLoader.GetAllRicsTable()
                End Select
            End If
        End Sub
#End Region

#Region "Chains and lists TAB"
        Private Sub ChainsListCheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ChainsButton.CheckedChanged
            Logger.Debug("Refresh...")
            RefreshChainsLists()
        End Sub

        Private Sub RefreshChainsLists()
            ChainsListsGrid.DataSource = If(ChainsButton.Checked, PortfolioManager.ChainsView, PortfolioManager.UserListsView)
            AddItemsButton.Enabled = ChainsButton.Checked
            DeleteItemsButton.Enabled = ChainsButton.Checked
            For Each col As DataGridViewColumn In ChainsListsGrid.Columns
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            Next
        End Sub

        Private Sub ChainsListsGrid_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles ChainsListsGrid.CellClick
            Dim elem = ChainsListsGrid.Rows(e.RowIndex).DataBoundItem
            Dim chain As Source = TryCast(elem, Source)
            If chain IsNot Nothing Then ChainListItemsGrid.DataSource = chain.GetDefaultRicsView()
        End Sub
#End Region

    End Class
End Namespace