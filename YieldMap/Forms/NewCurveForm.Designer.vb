Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class NewCurveForm
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
            Me.CancelButton = New System.Windows.Forms.Button()
            Me.CurveListView = New System.Windows.Forms.ListView()
            Me.OkButton = New System.Windows.Forms.Button()
            Me.TableLayoutPanel1.SuspendLayout()
            Me.SuspendLayout()
            '
            'TableLayoutPanel1
            '
            Me.TableLayoutPanel1.ColumnCount = 3
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel1.Controls.Add(Me.CancelButton, 2, 1)
            Me.TableLayoutPanel1.Controls.Add(Me.CurveListView, 0, 0)
            Me.TableLayoutPanel1.Controls.Add(Me.OkButton, 0, 1)
            Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
            Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
            Me.TableLayoutPanel1.RowCount = 2
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TableLayoutPanel1.Size = New System.Drawing.Size(232, 379)
            Me.TableLayoutPanel1.TabIndex = 1
            '
            'CancelButton
            '
            Me.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.CancelButton.Dock = System.Windows.Forms.DockStyle.Right
            Me.CancelButton.Location = New System.Drawing.Point(154, 357)
            Me.CancelButton.Margin = New System.Windows.Forms.Padding(0, 3, 3, 0)
            Me.CancelButton.Name = "CancelButton"
            Me.CancelButton.Size = New System.Drawing.Size(75, 22)
            Me.CancelButton.TabIndex = 3
            Me.CancelButton.Text = "Cancel"
            Me.CancelButton.UseVisualStyleBackColor = True
            '
            'CurveListView
            '
            Me.TableLayoutPanel1.SetColumnSpan(Me.CurveListView, 3)
            Me.CurveListView.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CurveListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            Me.CurveListView.Location = New System.Drawing.Point(3, 3)
            Me.CurveListView.Name = "CurveListView"
            Me.CurveListView.Size = New System.Drawing.Size(226, 348)
            Me.CurveListView.TabIndex = 0
            Me.CurveListView.TileSize = New System.Drawing.Size(168, 20)
            Me.CurveListView.UseCompatibleStateImageBehavior = False
            Me.CurveListView.View = System.Windows.Forms.View.Tile
            '
            'OkButton
            '
            Me.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.OkButton.Dock = System.Windows.Forms.DockStyle.Left
            Me.OkButton.Location = New System.Drawing.Point(3, 357)
            Me.OkButton.Margin = New System.Windows.Forms.Padding(3, 3, 0, 0)
            Me.OkButton.Name = "OkButton"
            Me.OkButton.Size = New System.Drawing.Size(75, 22)
            Me.OkButton.TabIndex = 1
            Me.OkButton.Text = "Ok"
            Me.OkButton.UseVisualStyleBackColor = True
            '
            'NewCurveForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(232, 379)
            Me.Controls.Add(Me.TableLayoutPanel1)
            Me.Name = "NewCurveForm"
            Me.Text = "Select curve source"
            Me.TableLayoutPanel1.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents CurveListView As System.Windows.Forms.ListView
        Friend WithEvents CancelButton As System.Windows.Forms.Button
        Friend WithEvents OkButton As System.Windows.Forms.Button
    End Class
End Namespace