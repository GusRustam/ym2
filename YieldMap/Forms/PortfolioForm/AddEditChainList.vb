Imports DbManager
Imports Uitls

Namespace Forms.PortfolioForm
    Public Class AddEditChainList
        Private _src As Source

        Public Property Src() As Source
            Get
                If Not DoValidate(ChainRadioButton.Checked) Then Return Nothing

                Dim color = ColorComboBox.Items(ColorComboBox.SelectedIndex)
                Dim fieldSetId = CType(FieldLayoutComboBox.SelectedItem, IdName(Of String)).Id
                Dim curve = CurveCheckBox.Checked
                Dim enbld = EnabledCheckBox.Checked
                Dim nme = NameTextBox.Text
                Dim chainRic = ChainRicTextBox.Text

                If _src IsNot Nothing Then
                    With _src
                        .Color = color
                        .FieldSetId = fieldSetId
                        .Curve = curve
                        .Enabled = enbld
                        .Name = nme
                        If ChainRadioButton.Checked Then CType(_src, Chain).ChainRic = chainRic
                    End With
                    Return _src
                Else
                    If ListRadioButton.Checked Then
                        Return New UserList(color, fieldSetId, enbld, curve, nme)
                    Else
                        Return New Chain(color, fieldSetId, enbld, curve, nme, chainRic)
                    End If
                End If
            End Get
            Set(ByVal value As Source)
                _src = value

                NameTextBox.Text = _src.Name

                SelectFieldLayout(_src.Fields.ID)
                ColorComboBox.SelectedItem = _src.Color
                ColorBox.BackColor = Color.FromName(_src.Color)

                EnabledCheckBox.Checked = _src.Enabled
                CurveCheckBox.Checked = _src.Curve
                Dim source = TryCast(_src, Chain)
                If source Is Nothing Then
                    Text = "Edit list"
                    ListRadioButton.Checked = True
                Else
                    Text = "Edit chain"
                    ChainRadioButton.Checked = True
                    ChainRicTextBox.Text = source.ChainRic
                End If

                ListRadioButton.Enabled = False
                ChainRadioButton.Enabled = False
            End Set
        End Property

        Private Function DoValidate(Optional ByVal checkChainRic As Boolean = False) As Boolean
            If ColorComboBox.SelectedIndex < 0 Then
                MessageBox.Show("Please select color", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
            If FieldLayoutComboBox.SelectedIndex < 0 Then
                MessageBox.Show("Please field layout", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
            If Name = "" Then
                MessageBox.Show("Please enter name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
            If checkChainRic AndAlso ChainRicTextBox.Text = "" Then
                MessageBox.Show("Please enter chain ric", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
            Return True
        End Function

        Private Sub SelectFieldLayout(ByVal id As String)
            Dim elems = (From elem In FieldLayoutComboBox.Items
                         Let data = CType(elem, FieldDescription)
                         Where data IsNot Nothing AndAlso data.Parent.ID = id
                         Select elem).ToList()
            If Not elems.Any Then Return
            FieldLayoutComboBox.SelectedItem = elems.First()
        End Sub

        Private Sub AddEditChainList_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            For Each clr In Utils.GetColorList()
                ColorComboBox.Items.Add(clr)
            Next
            If _src IsNot Nothing Then ColorComboBox.SelectedItem = _src.Color
            FieldLayoutComboBox.DataSource = PortfolioManager.Instance().GetFieldLayouts()

            Text = "Create new chain or list"
        End Sub

        Private Sub ColorsComboBoxDrawItem(ByVal sender As Object, ByVal e As DrawItemEventArgs) Handles ColorComboBox.DrawItem
            Dim g As Graphics = e.Graphics
            Dim r As Rectangle = e.Bounds
            If e.Index > 0 Then
                Dim txt As String = ColorComboBox.Items(e.Index)
                g.DrawString(txt, ColorComboBox.Font, Brushes.Black, r.X, r.Top)
                Dim m = g.MeasureString(txt, ColorComboBox.Font)
                Dim c As Color = Color.FromName(txt)
                g.FillRectangle(New SolidBrush(c), r.X + m.Width + 10, r.Y + 2, r.Width - m.Width - 15, r.Height - 6)
                g.DrawRectangle(New Pen(New SolidBrush(Color.Black)), r.X + m.Width + 10, r.Y + 2, r.Width - m.Width - 15, r.Height - 6)
            End If
        End Sub

        Private Sub ChainRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ChainRadioButton.CheckedChanged
            ChainRicTextBox.Enabled = ChainRadioButton.Checked
        End Sub

        Public Function SaveSource() As Source
            Return Src
        End Function

        Private Sub RandomColorButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles RandomColorButton.Click
            ColorComboBox.SelectedIndex = New Random().NextDouble() * ColorComboBox.Items.Count
        End Sub

        Private Sub ColorComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ColorComboBox.SelectedIndexChanged
            If ColorComboBox.SelectedIndex < 0 Then
                ColorBox.BackColor = Color.White
            Else
                ColorBox.BackColor = Color.FromName(ColorComboBox.SelectedItem)
            End If
        End Sub

        Private Sub AddEditChainList_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
            If DialogResult = DialogResult.OK Then e.Cancel = Not DoValidate()
        End Sub
    End Class
End Namespace