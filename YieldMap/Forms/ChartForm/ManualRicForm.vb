Imports YieldMap.BondsDataSetTableAdapters

Namespace Forms.ChartForm
    Public Class ManualRicForm
        Private _selectedRic As String

        Public ReadOnly Property SelectedRic As String
            Get
                Return _selectedRic
            End Get
        End Property

        Public ReadOnly Property LayoutId() As Integer
            Get
                Return CType(LayoutComboBox.Items(LayoutComboBox.SelectedIndex), IdValue).ID
            End Get
        End Property

        Private Class IdValue
            Private ReadOnly _id As Long
            Private ReadOnly _name As String

            Public Sub New(data As BondsDataSet.field_setRow)
                _id = data.id
                _name = data.name
            End Sub

            Public ReadOnly Property ID As Long
                Get
                    Return _id
                End Get
            End Property

            Public Overrides Function ToString() As String
                Return _name
            End Function
        End Class

        Private Sub RicTextBoxKeyUp(sender As Object, e As KeyEventArgs) Handles RicTextBox.KeyUp, LayoutComboBox.KeyUp
            e.Handled = True
            If e.KeyCode = Keys.Escape Then
                _selectedRic = ""
                Close()
            ElseIf e.KeyCode = Keys.Enter Then
                _selectedRic = RicTextBox.Text
                Close()
            Else
                e.Handled = False
            End If

        End Sub

        Private Sub ManualRicFormLeave(sender As Object, e As EventArgs) Handles Me.Leave
            _selectedRic = ""
            Close()
        End Sub

        Private Sub ManualRicFormLoad(sender As Object, e As EventArgs) Handles Me.Load
            Dim layoutTA As New field_setTableAdapter
            For Each row In layoutTA.GetData()
                LayoutComboBox.Items.Add(New IdValue(row))
            Next
            LayoutComboBox.SelectedIndex = 0
        End Sub
    End Class
End Namespace