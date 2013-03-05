Imports DbManager
Imports DbManager.Bonds

Namespace Forms.PortfolioForm
    Public Class PortfolioForm

#Region "Portfolio TAB"

        Private _flag As Boolean

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

            PortfolioTree.BeginUpdate()
            Dim newSelNode = AddPortfoliosByFolder("", selectedNodeId)
            If newSelNode IsNot Nothing Then
                newSelNode.EnsureVisible()
                PortfolioTree.SelectedNode = newSelNode
            End If
            PortfolioTree.EndUpdate()
        End Sub

        Private Function AddPortfoliosByFolder(ByVal id As String, ByVal selId As String, Optional ByVal whereTo As TreeNode = Nothing) As TreeNode
            Dim portMan As PortfolioManager = PortfolioManager.GetInstance()
            Dim res As TreeNode = If(id = selId, whereTo, Nothing)
            Dim descrs = portMan.GetPortfoliosByFolder(id)
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

        Private Sub PortfolioForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            BondsTableView.DataSource = BondsLoader.Instance.GetBondsTable()
            If PortfolioTree.ImageList Is Nothing Then
                PortfolioTree.ImageList = New ImageList
                PortfolioTree.ImageList.Images.Add("folder", My.Resources.folder)
                PortfolioTree.ImageList.Images.Add("portfolio", My.Resources.briefcase)
            End If
            RefreshPortfolioTree()
        End Sub

        Private Sub RenameToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles RenameToolStripMenuItem.Click, PortfolioTree.DoubleClick
            Dim node = CType(PortTreeCM.Tag, TreeNode)
            If node Is Nothing Then Return
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
                        PortfolioManager.GetInstance().SetFolderName(portDescr.Id, adder.NewName.Text)
                    Else
                        PortfolioManager.GetInstance().SetPortfolioName(portDescr.Id, adder.NewName.Text)
                    End If
                    RefreshPortfolioTree()
                End If
            End If
        End Sub

        Private Sub PortfolioTree_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) Handles PortfolioTree.NodeMouseClick
            If e.Button = MouseButtons.Right Then
                PortTreeCM.Tag = e.Node
                PortTreeCM.Show(PortfolioTree, e.Location)
            End If
            _flag = false
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
                    newId = PortfolioManager.GetInstance().AddFolder(adder.NewName.Text, theId)
                Else
                    newId = PortfolioManager.GetInstance().AddPortfolio(adder.NewName.Text, theId)
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
                    PortfolioManager.GetInstance().DeleteFolder(descr.Id)
                Else
                    PortfolioManager.GetInstance().DeletePortfolio(descr.Id)
                End If
                RefreshPortfolioTree()
            End If
        End Sub

        Private Sub PortfolioTree_DragEnter(ByVal sender As Object, ByVal e As DragEventArgs) Handles PortfolioTree.DragEnter

        End Sub

        Private Sub PortfolioTree_DragOver(ByVal sender As Object, ByVal e As DragEventArgs) Handles PortfolioTree.DragOver

        End Sub

        Private Sub PortfolioTree_DragLeave(ByVal sender As Object, ByVal e As EventArgs) Handles PortfolioTree.DragLeave

        End Sub

        Private Sub PortfolioTree_DragDrop(ByVal sender As Object, ByVal e As DragEventArgs) Handles PortfolioTree.DragDrop

        End Sub
#End Region

#Region "Data TAB"
        Private Sub TableChooserList_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles TableChooserList.SelectedIndexChanged
            If TableChooserList.SelectedIndex >= 0 Then
                Select Case TableChooserList.Items(TableChooserList.SelectedIndex).ToString()
                    Case "Bonds" : BondsTableView.DataSource = BondsLoader.Instance.GetBondsTable()
                    Case "Coupons" : BondsTableView.DataSource = BondsLoader.Instance.GetCouponsTable()
                    Case "FRNs" : BondsTableView.DataSource = BondsLoader.Instance.GetFRNsTable()
                    Case "Issue ratings" : BondsTableView.DataSource = BondsLoader.Instance.GetIssueRatingsTable()
                    Case "Issuer ratings" : BondsTableView.DataSource = BondsLoader.Instance.GetIssuerRatingsTable()
                    Case "Rics" : BondsTableView.DataSource = BondsLoader.Instance.GetAllRicsTable()
                End Select
            End If
        End Sub
#End Region

    End Class
End Namespace