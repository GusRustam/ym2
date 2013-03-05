Imports DbManager
Imports DbManager.Bonds

Namespace Forms.PortfolioForm
    Public Class PortfolioForm
#Region "Portfolio TAB"

        Private Sub RefreshPortfolioTree()
            Dim selectedNodeId As String
            If PortfolioTree.SelectedNode IsNot Nothing Then
                Dim descr = CType(PortfolioTree.SelectedNode.Tag, PortfolioItemDescription)
                selectedNodeId = descr.Id
            End If

            PortfolioTree.BeginUpdate()
            PortfolioTree.Nodes.Clear()
            Dim main = PortfolioManager.GetInstance().GetFolderDescr("0")
            Dim mainNode = PortfolioTree.Nodes.Add(main.NodeId, main.Name, "folder", "folder")
            mainNode.Tag = main
            Dim newSelNode = AddPortfoliosByFolder("0", selectedNodeId, PortfolioTree.TopNode)
            If newSelNode IsNot Nothing Then
                newSelNode.EnsureVisible()
                PortfolioTree.SelectedNode = newSelNode
            End If
            PortfolioTree.EndUpdate()
        End Sub

        Private Shared Function AddPortfoliosByFolder(ByVal id As String, ByVal selId As String, ByRef whereTo As TreeNode) As TreeNode
            Dim portMan As PortfolioManager = PortfolioManager.GetInstance()
            Dim res As TreeNode = If(id = selId, whereTo, Nothing)
            Dim descrs = portMan.GetPortfoliosByFolder(id)
            For Each descr In descrs
                Dim img = If(descr.IsFolder, "folder", "portfolio")
                Dim newNode = whereTo.Nodes.Add(descr.Id, descr.Name, img, img)
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
            Dim str = InputBox("Enter new name", "Rename")
            If str <> "" Then
                Dim portDescr As PortfolioItemDescription
                portDescr = CType(node.Tag, PortfolioItemDescription)
                If portDescr Is Nothing Then
                    MessageBox.Show("Failed to update name", "Name edit", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    If portDescr.IsFolder Then
                        PortfolioManager.GetInstance().SetFolderName(portDescr.Id, str)
                    Else
                        PortfolioManager.GetInstance().SetPortfolioName(portDescr.Id, str)
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

        Private Sub AddPortfolioToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddPortfolioToolStripMenuItem.Click

        End Sub
    End Class
End Namespace