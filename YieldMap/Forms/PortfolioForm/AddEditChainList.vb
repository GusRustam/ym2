Imports DbManager
Imports Uitls

Namespace Forms.PortfolioForm
    Public Class AddEditChainList
        Private _src As Source

        Public Property Src() As Source
            Get
                If Not DoValidate() Then Return Nothing

                Dim color = ColorComboBox.Items(ColorComboBox.SelectedIndex)
                Dim fieldSetId = CType(FieldLayoutComboBox.SelectedItem, IdName(Of String)).Id
                Dim curve = CurveCheckBox.Checked
                Dim enbld = EnabledCheckBox.Checked
                Dim nme = NameTextBox.Text
                Dim chainRic = ChainRicTextBox.Text
                Dim cond = ConditionTextBox.Text

                If ListRadioButton.Checked Then
                    Dim userListSrc = New UserListSrc(color, fieldSetId, enbld, curve, nme, _src Is Nothing)
                    If _src IsNot Nothing Then _src.Update(userListSrc)
                ElseIf ChainRadioButton.Checked Then
                    Dim chainSrc = New ChainSrc(color, fieldSetId, enbld, curve, nme, chainRic, _src Is Nothing)
                    If _src IsNot Nothing Then _src.Update(chainSrc)
                ElseIf QueryRadioButton.Checked Then
                    Dim selectedItem As ChainSrc = CType(ChainRicTextBox.SelectedItem, ChainSrc)
                    Dim userQuerySrc = New UserQuerySrc(color, fieldSetId, enbld, curve, nme, cond, selectedItem, _src Is Nothing)
                    If _src IsNot Nothing Then _src.Update(userQuerySrc)
                End If
                Return _src
            End Get
            Set(ByVal value As Source)
                _src = value

                NameTextBox.Text = _src.Name

                SelectFieldLayout(_src.Fields.ID)
                ColorComboBox.SelectedItem = _src.Color
                ColorBox.BackColor = Color.FromName(_src.Color)

                EnabledCheckBox.Checked = _src.Enabled
                CurveCheckBox.Checked = _src.Curve

                If TypeOf _src Is ChainSrc Then
                    Dim source = TryCast(_src, ChainSrc)
                    Text = "Edit chain"
                    ChainRadioButton.Checked = True

                    ChainRicTextBox.DropDownStyle = ComboBoxStyle.Simple
                    ChainRicTextBox.DataSource = Nothing
                    ChainRicTextBox.Text = source.ChainRic

                    EditConditionButton.Enabled = False
                    RemoveHandler ChainRicTextBox.SelectedValueChanged, Sub() AnotherFieldLayout(ChainRicTextBox.SelectedValue)
                    FieldLayoutComboBox.Enabled = True

                ElseIf TypeOf _src Is UserQuerySrc Then
                    Dim source = TryCast(_src, UserQuerySrc)
                    Text = "Edit query"
                    QueryRadioButton.Checked = True
                    ConditionTextBox.Text = source.Condition
                    EditConditionButton.Enabled = True

                    ChainRicTextBox.Enabled = True
                    ChainRicTextBox.DropDownStyle = ComboBoxStyle.DropDown
                    ChainRicTextBox.DataSource = PortfolioManager.Instance.ChainsView

                    For Each item In (From elem In ChainRicTextBox.Items
                                      Let x = TryCast(elem, ChainSrc)
                                      Where x IsNot Nothing AndAlso x.ID = source.MySource.ID
                                      Select x)
                        ChainRicTextBox.SelectedItem = item
                    Next
                    'ChainRicTextBox.ValueMember = "ID"
                    'ChainRicTextBox.DisplayMember = "ChainRic"
                    AddHandler ChainRicTextBox.SelectedValueChanged, Sub() AnotherFieldLayout(ChainRicTextBox.SelectedValue)
                    FieldLayoutComboBox.Enabled = False

                Else
                    Text = "Edit list"
                    ListRadioButton.Checked = True
                    ChainRicTextBox.Enabled = False
                    EditConditionButton.Enabled = False
                    ChainRicTextBox.DropDownStyle = ComboBoxStyle.Simple
                    ChainRicTextBox.DataSource = Nothing
                    RemoveHandler ChainRicTextBox.SelectedValueChanged, Sub() AnotherFieldLayout(ChainRicTextBox.SelectedValue)
                    FieldLayoutComboBox.Enabled = True

                End If

                ListRadioButton.Enabled = False
                ChainRadioButton.Enabled = False
                QueryRadioButton.Enabled = False

            End Set
        End Property

        Private Function DoValidate() As Boolean
            If ColorComboBox.SelectedIndex < 0 Then
                MessageBox.Show("Please select color", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
            If FieldLayoutComboBox.SelectedIndex < 0 Then
                MessageBox.Show("Please field layout", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
            If NameTextBox.Text = "" Then
                MessageBox.Show("Please enter name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
            If ChainRadioButton.Checked AndAlso ChainRicTextBox.Text = "" Then
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

        Private Sub ChainRadioButton_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ChainRadioButton.Click, ListRadioButton.Click, QueryRadioButton.Click
            If ChainRadioButton.Checked Then
                ChainRicTextBox.Enabled = True
                ChainRicTextBox.DropDownStyle = ComboBoxStyle.Simple
                ChainRicTextBox.DataSource = Nothing
                EditConditionButton.Enabled = False

                RemoveHandler ChainRicTextBox.SelectedValueChanged, Sub() AnotherFieldLayout(ChainRicTextBox.SelectedValue)
                FieldLayoutComboBox.Enabled = True

            ElseIf ListRadioButton.Checked Then
                ChainRicTextBox.Enabled = False
                ChainRicTextBox.DropDownStyle = ComboBoxStyle.Simple
                ChainRicTextBox.DataSource = Nothing
                EditConditionButton.Enabled = False

                RemoveHandler ChainRicTextBox.SelectedValueChanged, Sub() AnotherFieldLayout(ChainRicTextBox.SelectedValue)
                FieldLayoutComboBox.Enabled = True

            ElseIf QueryRadioButton.Checked Then
                ChainRicTextBox.Enabled = True
                ChainRicTextBox.DataSource = Nothing
                ChainRicTextBox.DropDownStyle = ComboBoxStyle.DropDownList
                ChainRicTextBox.DataSource = PortfolioManager.Instance.ChainsView
                EditConditionButton.Enabled = True

                AddHandler ChainRicTextBox.SelectedValueChanged, Sub() AnotherFieldLayout(ChainRicTextBox.SelectedValue)
                FieldLayoutComboBox.Enabled = False

            End If
        End Sub

        Private Sub AnotherFieldLayout(ByVal selectedValue As Object)
            If selectedValue Is Nothing Then Return
            Dim x As ChainSrc = TryCast(selectedValue, ChainSrc)
            If x Is Nothing Then Return
            For Each item In FieldLayoutComboBox.Items
                If CType(item, IdName(Of String)).Id = x.FieldSetId Then
                    FieldLayoutComboBox.SelectedItem = item
                    Exit For
                End If
            Next
        End Sub

        Public Function SaveSource() As Source
            Return Src
        End Function

        Private Sub RandomColorButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles RandomColorButton.Click
            ColorComboBox.SelectedIndex = New Random().NextDouble() * (ColorComboBox.Items.Count - 1)
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

        Private Sub EditConditionButton_Click(sender As Object, e As EventArgs) Handles EditConditionButton.Click
            Dim frm As New ParserErrorForm
            frm.ConditionTB.Text = ConditionTextBox.Text
            If frm.ShowDialog() = DialogResult.OK Then ConditionTextBox.Text = frm.ConditionTB.Text
        End Sub
    End Class
End Namespace