Imports YieldMap.Tools

Namespace Forms.ChartForm
    Public Class BondCurveItemsForm
        Private _curve As BondCurve

        Public Property Curve() As BondCurve
            Get
                Return _curve
            End Get
            Set(ByVal value As BondCurve)
                If _curve IsNot Nothing Then
                    RemoveHandler _curve.Clear, AddressOf OnCurveCleared
                    RemoveHandler _curve.Updated, AddressOf OnCurveUpdated
                    RemoveHandler _curve.RemovedItem, AddressOf OnCurveRemovedItem
                End If
                _curve = value
                If _curve IsNot Nothing Then
                    AddHandler _curve.Clear, AddressOf OnCurveCleared
                    AddHandler _curve.Updated, AddressOf OnCurveUpdated
                    AddHandler _curve.RemovedItem, AddressOf OnCurveRemovedItem
                End If
            End Set
        End Property

        Private Sub OnCurveRemovedItem(ByVal grp As BaseGroup, ByVal ric As String)
            CurveUpdate()
        End Sub

        Private Sub CurveUpdate()
            If Curve IsNot Nothing Then
                Dim curveSnapshot = Curve.GetSnapshot()
                BondsDGV.DataSource = curveSnapshot.Elements
                CurrentDGV.DataSource = curveSnapshot.Current
                FormulaTB.Text = Curve.Formula
            Else
                BondsDGV.DataSource = Nothing
                CurrentDGV.DataSource = Nothing
                FormulaTB.Text = ""
            End If
            ResetEnabled
        End Sub

        Private Sub ResetEnabled()
            AddItemsTSB.Enabled = MainTC.SelectedTab.Name = BondsTP.Name AndAlso Curve.DisabledElements.Any
            RemoveItemsTSB.Enabled = MainTC.SelectedTab.Name = BondsTP.Name
        End Sub

        Private Sub OnCurveUpdated(ByVal obj As List(Of BondCurve.CurveItem))
            CurveUpdate()
        End Sub

        Private Sub OnCurveCleared(ByVal obj As BaseGroup)
            Close()
        End Sub

        Private Sub BondCurveItemsForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
            Curve = Nothing
        End Sub

        Private Sub MainTC_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MainTC.SelectedIndexChanged
            ResetEnabled()
        End Sub

        Private Sub AddItemsTSB_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AddItemsTSB.Click
            ' todo
            ' todo 1) отдельная опция - показ отдельно точек по облигациям
            ' todo 2) конвертация группы в кривую
            ' todo 2.5) убрать показ отдельных точек в группе - показывать все разом
            ' todo 3) спреды!!!
            ' todo 4) упроситить и редуцировать swap curves
            ' todo 5) сделать контекстное меню группы и бондовой кривой в легенде
        End Sub

        Private Sub RemoveItemsTSB_Click(ByVal sender As Object, ByVal e As EventArgs) Handles RemoveItemsTSB.Click
            If BondsDGV.SelectedRows.Count <= 0 Then Return
            Dim elements = (From elem As DataGridViewRow In BondsDGV.SelectedRows Select CType(elem.DataBoundItem, BondCurve.BondCurveSnapshot.BondCurveElement).RIC).ToList()
            Curve.Disable(elements)
        End Sub

        Private Sub BondCurveItemsForm_Shown(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Shown
            CurveUpdate()
        End Sub
    End Class
End Namespace