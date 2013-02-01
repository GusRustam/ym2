Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms

Namespace Forms.PortfolioForm
    Public Class NewChainForm
        Public fieldSetId As Long
        Private Sub NewChainFormLoad(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            Field_setTableAdapter.Fill(BondsDataSet.field_set)
            Dim colorsArr = [Enum].GetValues(GetType(KnownColor))
            FieldLayoutComboBox.SelectedValue = fieldSetId
            Dim colors = New List(Of String)
            Array.ForEach(Of KnownColor)(colorsArr, Sub(color) colors.Add(color.ToString()))
            Dim props = GetType(SystemColors).GetProperties(BindingFlags.Static Or BindingFlags.Public)
            Array.ForEach(props, Sub(prop) colors.Remove(prop.Name))

            For Each clr In colors
                ColorsComboBox.Items.Add(clr)
            Next
        End Sub

        Private Sub ColorsComboBoxDrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles ColorsComboBox.DrawItem
            Dim g As Graphics = e.Graphics
            Dim r As Rectangle = e.Bounds
            If e.Index > 0 Then
                Dim txt As String = ColorsComboBox.Items(e.Index)
                g.DrawString(txt, ColorsComboBox.Font, Brushes.Black, r.X, r.Top)
                Dim m = g.MeasureString(txt, ColorsComboBox.Font)
                Dim c As Color = Color.FromName(txt)
                g.FillRectangle(New SolidBrush(c), r.X + m.Width + 10, r.Y + 2, r.Width - m.Width - 15, r.Height - 6)
                g.DrawRectangle(New Pen(New SolidBrush(Color.Black)), r.X + m.Width + 10, r.Y + 2, r.Width - m.Width - 15, r.Height - 6)
            End If
        End Sub
    End Class
End Namespace