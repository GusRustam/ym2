Imports System.Windows.Forms
Imports YieldMap.BondsDataSetTableAdapters

Namespace Forms
    Public Class NewCurveForm
        Public Class CurveDescr
            Public Type As String
            Public Name As String
            Public ID As Integer
            Public Color As String
        End Class

        Private Sub NewCurveFormLoad(sender As Object, e As EventArgs) Handles MyBase.Load
            Dim chainTA As New chainTableAdapter
            Dim chainCurves = chainTA.GetData.Where(Function(row) row.curve).Select(
                Function(row)
                    Return New CurveDescr With {
                    .Type = "Chain",
                    .Name = row.descr,
                    .ID = row.id,
                    .Color = row.color}
                End Function).ToList()
            DoAdd(chainCurves)

            Dim hawserTA As New hawserTableAdapter
            Dim hawserCurves = hawserTA.GetData.Where(Function(row) row.curve).Select(
                Function(row)
                    Return New CurveDescr With {
                    .Type = "List",
                    .Name = row.hawser_name,
                    .ID = row.id,
                    .Color = row.color}
                End Function).ToList()
            DoAdd(hawserCurves)
        End Sub

        Private Sub DoAdd(ByVal curves As List(Of CurveDescr))
            Dim groups = curves.Select(Function(curve) curve.Type).Distinct.ToList()
            groups.ForEach(Sub(group) CurveListView.Groups.Add(group, group))
            curves.ForEach(Sub(curve)
                               Dim item = CurveListView.Items.Add(curve.Name, curve.Name)
                               item.Group = CurveListView.Groups(curve.Type)
                               item.Tag = curve
                           End Sub)
        End Sub

        Private Sub OkButtonClick(sender As Object, e As EventArgs) Handles OkButton.Click
            Close()
        End Sub

        Private Sub CancelButtonClick(sender As Object, e As EventArgs) Handles CancelButton.Click
            Close()
        End Sub
    End Class
End Namespace