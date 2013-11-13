Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class CustomBondSelectorForm
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
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

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim Label1 As System.Windows.Forms.Label
            Dim Label2 As System.Windows.Forms.Label
            Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
            Me.CustomBondListBox = New System.Windows.Forms.ListBox()
            Me.OkButton = New System.Windows.Forms.Button()
            Me.Button1 = New System.Windows.Forms.Button()
            Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
            Me.ColorSelectComboBox = New System.Windows.Forms.ComboBox()
            Me.SelectedColorPictureBox = New System.Windows.Forms.PictureBox()
            Me.RandomButton = New System.Windows.Forms.Button()
            Label1 = New System.Windows.Forms.Label()
            Label2 = New System.Windows.Forms.Label()
            Me.TableLayoutPanel1.SuspendLayout()
            Me.TableLayoutPanel2.SuspendLayout()
            CType(Me.SelectedColorPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'Label1
            '
            Label1.AutoSize = True
            Me.TableLayoutPanel1.SetColumnSpan(Label1, 2)
            Label1.Dock = System.Windows.Forms.DockStyle.Bottom
            Label1.Location = New System.Drawing.Point(28, 17)
            Label1.Name = "Label1"
            Label1.Size = New System.Drawing.Size(438, 13)
            Label1.TabIndex = 0
            Label1.Text = "Select one or more bonds:"
            '
            'Label2
            '
            Label2.AutoSize = True
            Label2.Location = New System.Drawing.Point(3, 3)
            Label2.Margin = New System.Windows.Forms.Padding(3)
            Label2.Name = "Label2"
            Label2.Size = New System.Drawing.Size(63, 13)
            Label2.TabIndex = 7
            Label2.Text = "Select color"
            '
            'TableLayoutPanel1
            '
            Me.TableLayoutPanel1.ColumnCount = 4
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
            Me.TableLayoutPanel1.Controls.Add(Label1, 1, 0)
            Me.TableLayoutPanel1.Controls.Add(Me.CustomBondListBox, 1, 1)
            Me.TableLayoutPanel1.Controls.Add(Me.OkButton, 1, 3)
            Me.TableLayoutPanel1.Controls.Add(Me.Button1, 2, 3)
            Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 1, 2)
            Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
            Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
            Me.TableLayoutPanel1.RowCount = 4
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            Me.TableLayoutPanel1.Size = New System.Drawing.Size(495, 262)
            Me.TableLayoutPanel1.TabIndex = 0
            '
            'CustomBondListBox
            '
            Me.TableLayoutPanel1.SetColumnSpan(Me.CustomBondListBox, 2)
            Me.CustomBondListBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CustomBondListBox.FormattingEnabled = True
            Me.CustomBondListBox.Location = New System.Drawing.Point(28, 33)
            Me.CustomBondListBox.Name = "CustomBondListBox"
            Me.CustomBondListBox.Size = New System.Drawing.Size(438, 171)
            Me.CustomBondListBox.TabIndex = 1
            '
            'OkButton
            '
            Me.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.OkButton.Dock = System.Windows.Forms.DockStyle.Left
            Me.OkButton.Location = New System.Drawing.Point(28, 235)
            Me.OkButton.Name = "OkButton"
            Me.OkButton.Size = New System.Drawing.Size(100, 24)
            Me.OkButton.TabIndex = 2
            Me.OkButton.Text = "Add selected"
            Me.OkButton.UseVisualStyleBackColor = True
            '
            'Button1
            '
            Me.Button1.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.Button1.Dock = System.Windows.Forms.DockStyle.Right
            Me.Button1.Location = New System.Drawing.Point(366, 235)
            Me.Button1.Name = "Button1"
            Me.Button1.Size = New System.Drawing.Size(100, 24)
            Me.Button1.TabIndex = 3
            Me.Button1.Text = "Cancel"
            Me.Button1.UseVisualStyleBackColor = True
            '
            'TableLayoutPanel2
            '
            Me.TableLayoutPanel2.ColumnCount = 4
            Me.TableLayoutPanel1.SetColumnSpan(Me.TableLayoutPanel2, 2)
            Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.0!))
            Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.0!))
            Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.0!))
            Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 61.0!))
            Me.TableLayoutPanel2.Controls.Add(Me.ColorSelectComboBox, 2, 0)
            Me.TableLayoutPanel2.Controls.Add(Label2, 0, 0)
            Me.TableLayoutPanel2.Controls.Add(Me.SelectedColorPictureBox, 1, 0)
            Me.TableLayoutPanel2.Controls.Add(Me.RandomButton, 3, 0)
            Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel2.Location = New System.Drawing.Point(25, 207)
            Me.TableLayoutPanel2.Margin = New System.Windows.Forms.Padding(0)
            Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
            Me.TableLayoutPanel2.RowCount = 1
            Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel2.Size = New System.Drawing.Size(444, 25)
            Me.TableLayoutPanel2.TabIndex = 6
            '
            'ColorSelectComboBox
            '
            Me.ColorSelectComboBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ColorSelectComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
            Me.ColorSelectComboBox.FormattingEnabled = True
            Me.ColorSelectComboBox.Location = New System.Drawing.Point(155, 3)
            Me.ColorSelectComboBox.Name = "ColorSelectComboBox"
            Me.ColorSelectComboBox.Size = New System.Drawing.Size(223, 21)
            Me.ColorSelectComboBox.TabIndex = 6
            '
            'SelectedColorPictureBox
            '
            Me.SelectedColorPictureBox.Dock = System.Windows.Forms.DockStyle.Right
            Me.SelectedColorPictureBox.Location = New System.Drawing.Point(129, 3)
            Me.SelectedColorPictureBox.Name = "SelectedColorPictureBox"
            Me.SelectedColorPictureBox.Size = New System.Drawing.Size(20, 19)
            Me.SelectedColorPictureBox.TabIndex = 8
            Me.SelectedColorPictureBox.TabStop = False
            '
            'RandomButton
            '
            Me.RandomButton.Location = New System.Drawing.Point(381, 0)
            Me.RandomButton.Margin = New System.Windows.Forms.Padding(0)
            Me.RandomButton.Name = "RandomButton"
            Me.RandomButton.Size = New System.Drawing.Size(61, 23)
            Me.RandomButton.TabIndex = 9
            Me.RandomButton.Text = "Random"
            Me.RandomButton.UseVisualStyleBackColor = True
            '
            'CustomBondSelectorForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(495, 262)
            Me.Controls.Add(Me.TableLayoutPanel1)
            Me.Name = "CustomBondSelectorForm"
            Me.Text = "Select bonds"
            Me.TableLayoutPanel1.ResumeLayout(False)
            Me.TableLayoutPanel1.PerformLayout()
            Me.TableLayoutPanel2.ResumeLayout(False)
            Me.TableLayoutPanel2.PerformLayout()
            CType(Me.SelectedColorPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents CustomBondListBox As System.Windows.Forms.ListBox
        Friend WithEvents OkButton As System.Windows.Forms.Button
        Friend WithEvents Button1 As System.Windows.Forms.Button
        Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents ColorSelectComboBox As System.Windows.Forms.ComboBox
        Friend WithEvents SelectedColorPictureBox As System.Windows.Forms.PictureBox
        Friend WithEvents RandomButton As System.Windows.Forms.Button
    End Class
End Namespace