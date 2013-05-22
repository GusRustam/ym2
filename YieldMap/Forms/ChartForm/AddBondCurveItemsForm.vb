Imports YieldMap.Tools.Elements
Imports YieldMap.Tools

Namespace Forms.ChartForm
    Public Class AddBondCurveItemsForm
        Private _curve As BondCurve
        Private _snapshot As BondCurve.BondCurveSnapshot

        Public Property Curve() As BondCurve
            Get
                Return _curve
            End Get
            Set(ByVal value As BondCurve)
                _curve = value
                _snapshot = _curve.GetSnapshot()
            End Set
        End Property

        Private Sub AddBondCurveItemsForm_Shown(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Shown
            If Curve Is Nothing Then Close()
            ItemsDGV.DataSource = _snapshot.DisabledElements
            OkButton.Enabled = ItemsDGV.SelectedRows.Count > 0
        End Sub

        Private Sub OkButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OkButton.Click
            If ItemsDGV.SelectedRows.Count <= 0 Then Return
            Dim elements = (From elem As DataGridViewRow In ItemsDGV.SelectedRows Select CType(elem.DataBoundItem, BondCurve.BondCurveSnapshot.BondCurveElement).RIC).ToList()
            Curve.Enable(elements)
            Close()
        End Sub

        Private Sub ItemsDGV_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ItemsDGV.SelectionChanged
            OkButton.Enabled = ItemsDGV.SelectedRows.Count > 0
        End Sub
    End Class
End Namespace