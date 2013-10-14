Imports DbManager
Imports Uitls

Namespace Forms
    Public Class CustomBondSelectorForm
        Private _selectedRic As String
        Public ReadOnly Property SelectedRic As String
            Get
                Return _selectedRic
            End Get
        End Property

        Private _color As String
        Public ReadOnly Property SelectedColor As String
            Get
                Return _color
            End Get
        End Property

        Private Sub CustomBondSelectorForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            For Each clr In Utils.GetColorList()
                ColorSelectComboBox.Items.Add(clr)
            Next
            CustomBondListBox.DataSource = PortfolioManager.Instance.CustomBondsView
            CustomBondListBox.ValueMember = "Code"
            CustomBondListBox.DisplayMember = "Code"
        End Sub

        Private Sub CustomColorCB_DrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles ColorSelectComboBox.DrawItem
            Dim g As Graphics = e.Graphics
            Dim r As Rectangle = e.Bounds
            If e.Index > 0 Then
                Dim txt As String = ColorSelectComboBox.Items(e.Index)
                g.DrawString(txt, ColorSelectComboBox.Font, Brushes.Black, r.X, r.Top)
                Dim m = g.MeasureString(txt, ColorSelectComboBox.Font)
                Dim c As Color = Color.FromName(txt)
                g.FillRectangle(New SolidBrush(c), r.X + m.Width + 10, r.Y + 2, r.Width - m.Width - 15, r.Height - 6)
                g.DrawRectangle(New Pen(New SolidBrush(Color.Black)), r.X + m.Width + 10, r.Y + 2, r.Width - m.Width - 15, r.Height - 6)
            End If
        End Sub

        Private Sub ColorSelectComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ColorSelectComboBox.SelectedIndexChanged
            If ColorSelectComboBox.SelectedIndex < 0 Then
                ColorSelectComboBox.BackColor = Color.Black
                _color = "Black"
            Else
                SelectedColorPictureBox.BackColor = Color.FromName(ColorSelectComboBox.SelectedItem)
                _color = ColorSelectComboBox.SelectedItem.ToString()
            End If
        End Sub

        Private Sub CustomBondListBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CustomBondListBox.SelectedIndexChanged
            If CustomBondListBox.SelectedIndex < 0 Then
                _selectedRic = ""
            Else
                Dim selectedItem = CustomBondListBox.SelectedItem
                Dim cBond = TryCast(selectedItem, CustomBondSrc)

                _selectedRic = If(cBond Is Nothing, "", cBond.Code)
            End If
        End Sub

        Private Sub RandomButton_Click(sender As System.Object, e As EventArgs) Handles RandomButton.Click
            ColorSelectComboBox.SelectedIndex = New Random().NextDouble() * (ColorSelectComboBox.Items.Count - 1)
        End Sub
    End Class
End Namespace