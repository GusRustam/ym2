Imports YieldMap.Tools.Elements

Namespace Forms.ChartForm
    Public Class BondCurveItemsForm
        Private _curve As BondCurve

        Public Property Curve() As BondCurve
            Get
                Return _curve
            End Get
            Set(ByVal value As BondCurve)
                If _curve IsNot Nothing Then
                    RemoveHandler _curve.Cleared, AddressOf OnCurveCleared
                    RemoveHandler _curve.Updated, AddressOf OnCurveUpdated
                End If
                _curve = value
                If _curve IsNot Nothing Then
                    AddHandler _curve.Cleared, AddressOf OnCurveCleared
                    AddHandler _curve.Updated, AddressOf OnCurveUpdated
                    AddHandler _curve.UpdatedSpread, AddressOf OnCurveUpdatedSpread
                End If
            End Set
        End Property

        Private Sub OnCurveUpdatedSpread(ByVal arg1 As List(Of CurveItem), ByVal arg2 As IOrdinate)
            'Dim pg As TabPage = Nothing
            'For Each page In From page1 As Object In MainTC.TabPages Where TypeOf (page1.tag) Is IOrdinate AndAlso CType(page1.tag, IOrdinate).Equals(arg2)
            '    pg = page
            '    Exit For
            'Next
            'If pg Is Nothing Then
            '    CreatePage(arg2, arg1)
            'Else
            '    Dim ctl = pg.Controls(arg2.NameProperty + "_DGV")
            '    If ctl Is Nothing Then Return
            '    Dim dgv = CType(ctl, DataGridView)
            '    dgv.DataSource = arg1
            'End If
            CurveUpdate()
        End Sub

        Private Sub CurveUpdate()
            If Curve IsNot Nothing Then
                Dim curveSnapshot = Curve.GetSnapshot()
                BondsDGV.DataSource = curveSnapshot.EnabledElements
                CurrentDGV.DataSource = curveSnapshot.Current
                FormulaTB.Text = Curve.Formula
                Dim i As Integer = 0
                Do
                    If MainTC.TabPages(i).Tag IsNot Nothing Then
                        MainTC.TabPages.RemoveAt(i)
                    Else
                        i = i + 1
                    End If
                Loop While i < MainTC.TabPages.Count
                For Each key In curveSnapshot.Spreads.Keys
                    CreatePage(key, curveSnapshot.Spreads(key))
                Next
            Else
                BondsDGV.DataSource = Nothing
                CurrentDGV.DataSource = Nothing
                FormulaTB.Text = ""
            End If
            ResetEnabled()
        End Sub

        Private Sub CreatePage(key As IOrdinate, data As List(Of BondSpreadCurveItem))
            Dim pg As New TabPage(key.DescrProperty) With {.Tag = key}
            Dim dgv As New DataGridView With {.Name = key.NameProperty + "_DGV"}
            dgv.DataSource = data
            pg.Controls.Add(dgv)
            dgv.Dock = DockStyle.Fill
            MainTC.TabPages.Add(pg)
            MainTC.SelectedTab = pg
        End Sub

        Private Sub ResetEnabled()
            AddItemsTSB.Enabled = MainTC.SelectedTab.Name = BondsTP.Name AndAlso Curve.DisabledElements.Any
            RemoveItemsTSB.Enabled = MainTC.SelectedTab.Name = BondsTP.Name
        End Sub

        Private Sub OnCurveUpdated(ByVal obj As List(Of CurveItem))
            CurveUpdate()
        End Sub

        Private Sub OnCurveCleared()
            Close()
        End Sub

        Private Sub BondCurveItemsForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
            Curve = Nothing
        End Sub

        Private Sub MainTC_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MainTC.SelectedIndexChanged
            ResetEnabled()
        End Sub

        Private Sub AddItemsTSB_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AddItemsTSB.Click
            Dim frm As New AddBondCurveItemsForm
            frm.Curve = Curve
            frm.ShowDialog()
        End Sub

        Private Sub RemoveItemsTSB_Click(ByVal sender As Object, ByVal e As EventArgs) Handles RemoveItemsTSB.Click
            If BondsDGV.SelectedRows.Count <= 0 Then Return
            If BondsDGV.Rows.Count <= 2 Then
                MessageBox.Show("There must be at least two points in curve", "Cannot remove point", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            Dim elements = (From elem As DataGridViewRow In BondsDGV.SelectedRows Select CType(elem.DataBoundItem, BondCurve.BondCurveSnapshot.BondCurveElement).RIC).ToList()
            Curve.Disable(elements)
        End Sub

        Private Sub BondCurveItemsForm_Shown(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Shown
            CurveUpdate()
        End Sub
    End Class
End Namespace