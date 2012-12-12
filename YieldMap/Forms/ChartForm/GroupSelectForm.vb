Imports System.Reflection
Imports YieldMap.BondsDataSetTableAdapters

Namespace Forms.ChartForm
    Public Class GroupSelectForm
        Private Class IdValue(Of T)
            Private ReadOnly _id As T
            Private ReadOnly _name As String

            Public Sub New(ByVal id As T, ByVal name As String)
                _id = id
                _name = name
            End Sub

            Public ReadOnly Property ID As T
                Get
                    Return _id
                End Get
            End Property

            Public Overrides Function ToString() As String
                Return _name
            End Function
        End Class

        Public Sub InitGroupList(ByVal data As Dictionary(Of Guid, String))
            ExistingGroupsListBox.Items.Clear()
            For Each key In data.Keys
                ExistingGroupsListBox.Items.Add(New IdValue(Of Guid)(key, data(key)))
            Next

            If Not data.Any Then SelectNew(True)
        End Sub

        Public ReadOnly Property LayoutId() As Integer
            Get
                Return CType(FieldsLayoutComboBox.Items(FieldsLayoutComboBox.SelectedIndex), IdValue(Of Integer)).ID
            End Get
        End Property

        Public ReadOnly Property UseNew As Boolean
            Get
                Return NewGroupRadioButton.Checked
            End Get
        End Property

        Public ReadOnly Property NewName As String
            Get
                Return NewGroupTextBox.Text
            End Get
        End Property

        Public ReadOnly Property ExistingGroupId As Guid
            Get
                If ExistingGroupsListBox.SelectedIndex >= 0 Then
                    Return CType(ExistingGroupsListBox.Items(ExistingGroupsListBox.SelectedIndex), IdValue(Of Guid)).ID
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public ReadOnly Property NewColor As String
            Get
                If ColorsComboBox.SelectedIndex >= 0 Then
                    Return ColorsComboBox.Text
                Else
                    Return Color.Red.ToString()
                End If
            End Get
        End Property

        Private Sub OkButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OkButton.Click
            Close()
        End Sub

        Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CancelButton.Click
            Close()
        End Sub

        Private Sub SelectNew(ByVal really As Boolean)
            ExistingGroupRadioButton.Checked = Not really
            ExistingGroupsListBox.Enabled = Not really

            NewGroupRadioButton.Checked = really
            NewGroupTextBox.Enabled = really
            FieldsLayoutComboBox.Enabled = really
            ColorsComboBox.Enabled = really
        End Sub

        Private Sub ExistingGroupsListBox_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExistingGroupsListBox.Click
            SelectNew(False)
        End Sub

        Private Sub NewGroupTextBox_Click(ByVal sender As Object, ByVal e As EventArgs) Handles NewGroupTextBox.Click
            SelectNew(True)
        End Sub

        Private Sub ExistingGroupRadioButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ExistingGroupRadioButton.Click
            SelectNew(False)
        End Sub

        Private Sub NewGroupRadioButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles NewGroupRadioButton.Click
            SelectNew(True)
        End Sub

        Private Sub ColorsComboBox_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ColorsComboBox.Click
            SelectNew(True)
        End Sub

        Private Sub GroupSelectForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            FieldsLayoutComboBox.Items.Clear()
            Dim layoutTA As New field_setTableAdapter
            For Each row In layoutTA.GetData()
                FieldsLayoutComboBox.Items.Add(New IdValue(Of Integer)(row.id, row.name))
            Next
            FieldsLayoutComboBox.SelectedIndex = 0

            Dim colorsArr = [Enum].GetValues(GetType(KnownColor))
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