Namespace Forms.ChartForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ManualRicForm
        Inherits System.Windows.Forms.Form

        'Форма переопределяет dispose для очистки списка компонентов.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Является обязательной для конструктора форм Windows Forms
        Private components As System.ComponentModel.IContainer

        'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
        'Для ее изменения используйте конструктор форм Windows Form.  
        'Не изменяйте ее в редакторе исходного кода.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.RicTextBox = New System.Windows.Forms.TextBox()
            Me.LayoutComboBox = New System.Windows.Forms.ComboBox()
            Me.TableLayoutPanel1.SuspendLayout()
            Me.SuspendLayout()
            '
            'TableLayoutPanel1
            '
            Me.TableLayoutPanel1.ColumnCount = 4
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.99999!))
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200.0!))
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.00001!))
            Me.TableLayoutPanel1.Controls.Add(Me.Label1, 1, 1)
            Me.TableLayoutPanel1.Controls.Add(Me.Label2, 1, 3)
            Me.TableLayoutPanel1.Controls.Add(Me.RicTextBox, 2, 1)
            Me.TableLayoutPanel1.Controls.Add(Me.LayoutComboBox, 2, 3)
            Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
            Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
            Me.TableLayoutPanel1.RowCount = 5
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel1.Size = New System.Drawing.Size(400, 200)
            Me.TableLayoutPanel1.TabIndex = 0
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(52, 80)
            Me.Label1.Margin = New System.Windows.Forms.Padding(3)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(25, 13)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "RIC"
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(52, 105)
            Me.Label2.Margin = New System.Windows.Forms.Padding(3)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(60, 13)
            Me.Label2.TabIndex = 0
            Me.Label2.Text = "Field layout"
            '
            'RicTextBox
            '
            Me.RicTextBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.RicTextBox.Location = New System.Drawing.Point(149, 77)
            Me.RicTextBox.Margin = New System.Windows.Forms.Padding(0)
            Me.RicTextBox.Name = "RicTextBox"
            Me.RicTextBox.Size = New System.Drawing.Size(200, 20)
            Me.RicTextBox.TabIndex = 1
            '
            'LayoutComboBox
            '
            Me.LayoutComboBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LayoutComboBox.FormattingEnabled = True
            Me.LayoutComboBox.Location = New System.Drawing.Point(149, 102)
            Me.LayoutComboBox.Margin = New System.Windows.Forms.Padding(0)
            Me.LayoutComboBox.Name = "LayoutComboBox"
            Me.LayoutComboBox.Size = New System.Drawing.Size(200, 21)
            Me.LayoutComboBox.TabIndex = 2
            '
            'ManualRicForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.CornflowerBlue
            Me.ClientSize = New System.Drawing.Size(400, 200)
            Me.Controls.Add(Me.TableLayoutPanel1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.Name = "ManualRicForm"
            Me.Text = "ManualRic"
            Me.TableLayoutPanel1.ResumeLayout(False)
            Me.TableLayoutPanel1.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents RicTextBox As System.Windows.Forms.TextBox
        Friend WithEvents LayoutComboBox As System.Windows.Forms.ComboBox
    End Class
End Namespace