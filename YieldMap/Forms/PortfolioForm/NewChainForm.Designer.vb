Namespace Forms.PortfolioForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class NewChainForm
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
            Me.TheLayout = New System.Windows.Forms.TableLayoutPanel()
            Me.CloseTextBox = New System.Windows.Forms.TextBox()
            Me.LastTextBox = New System.Windows.Forms.TextBox()
            Me.AskTextBox = New System.Windows.Forms.TextBox()
            Me.BidTextBox = New System.Windows.Forms.TextBox()
            Me.RICLabel = New System.Windows.Forms.Label()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.ColorsComboBox = New System.Windows.Forms.ComboBox()
            Me.RICTextBox = New System.Windows.Forms.TextBox()
            Me.DescrTextBox = New System.Windows.Forms.TextBox()
            Me.OkButton = New System.Windows.Forms.Button()
            Me.CancelButton = New System.Windows.Forms.Button()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.Label4 = New System.Windows.Forms.Label()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.Label6 = New System.Windows.Forms.Label()
            Me.TheColorDialog = New System.Windows.Forms.ColorDialog()
            Me.CurveCheckBox = New System.Windows.Forms.CheckBox()
            Me.TheLayout.SuspendLayout()
            Me.SuspendLayout()
            '
            'TheLayout
            '
            Me.TheLayout.ColumnCount = 2
            Me.TheLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200.0!))
            Me.TheLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TheLayout.Controls.Add(Me.CloseTextBox, 1, 6)
            Me.TheLayout.Controls.Add(Me.LastTextBox, 1, 5)
            Me.TheLayout.Controls.Add(Me.AskTextBox, 1, 4)
            Me.TheLayout.Controls.Add(Me.BidTextBox, 1, 3)
            Me.TheLayout.Controls.Add(Me.RICLabel, 0, 0)
            Me.TheLayout.Controls.Add(Me.Label2, 0, 1)
            Me.TheLayout.Controls.Add(Me.Label3, 0, 2)
            Me.TheLayout.Controls.Add(Me.ColorsComboBox, 1, 2)
            Me.TheLayout.Controls.Add(Me.RICTextBox, 1, 0)
            Me.TheLayout.Controls.Add(Me.DescrTextBox, 1, 1)
            Me.TheLayout.Controls.Add(Me.OkButton, 0, 8)
            Me.TheLayout.Controls.Add(Me.CancelButton, 1, 8)
            Me.TheLayout.Controls.Add(Me.Label1, 0, 3)
            Me.TheLayout.Controls.Add(Me.Label4, 0, 4)
            Me.TheLayout.Controls.Add(Me.Label5, 0, 5)
            Me.TheLayout.Controls.Add(Me.Label6, 0, 6)
            Me.TheLayout.Controls.Add(Me.CurveCheckBox, 1, 7)
            Me.TheLayout.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TheLayout.Location = New System.Drawing.Point(0, 0)
            Me.TheLayout.Name = "TheLayout"
            Me.TheLayout.RowCount = 9
            Me.TheLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TheLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TheLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TheLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TheLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TheLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TheLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TheLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TheLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TheLayout.Size = New System.Drawing.Size(616, 234)
            Me.TheLayout.TabIndex = 0
            '
            'CloseTextBox
            '
            Me.CloseTextBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CloseTextBox.Location = New System.Drawing.Point(203, 153)
            Me.CloseTextBox.Name = "CloseTextBox"
            Me.CloseTextBox.Size = New System.Drawing.Size(410, 20)
            Me.CloseTextBox.TabIndex = 8
            '
            'LastTextBox
            '
            Me.LastTextBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LastTextBox.Location = New System.Drawing.Point(203, 128)
            Me.LastTextBox.Name = "LastTextBox"
            Me.LastTextBox.Size = New System.Drawing.Size(410, 20)
            Me.LastTextBox.TabIndex = 7
            '
            'AskTextBox
            '
            Me.AskTextBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.AskTextBox.Location = New System.Drawing.Point(203, 103)
            Me.AskTextBox.Name = "AskTextBox"
            Me.AskTextBox.Size = New System.Drawing.Size(410, 20)
            Me.AskTextBox.TabIndex = 6
            '
            'BidTextBox
            '
            Me.BidTextBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BidTextBox.Location = New System.Drawing.Point(203, 78)
            Me.BidTextBox.Name = "BidTextBox"
            Me.BidTextBox.Size = New System.Drawing.Size(410, 20)
            Me.BidTextBox.TabIndex = 5
            '
            'RICLabel
            '
            Me.RICLabel.Anchor = System.Windows.Forms.AnchorStyles.Left
            Me.RICLabel.AutoSize = True
            Me.RICLabel.Location = New System.Drawing.Point(3, 6)
            Me.RICLabel.Margin = New System.Windows.Forms.Padding(3)
            Me.RICLabel.Name = "RICLabel"
            Me.RICLabel.Size = New System.Drawing.Size(25, 13)
            Me.RICLabel.TabIndex = 0
            Me.RICLabel.Text = "RIC"
            '
            'Label2
            '
            Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.Left
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(3, 31)
            Me.Label2.Margin = New System.Windows.Forms.Padding(3)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(60, 13)
            Me.Label2.TabIndex = 0
            Me.Label2.Text = "Description"
            '
            'Label3
            '
            Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.Left
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(3, 56)
            Me.Label3.Margin = New System.Windows.Forms.Padding(3)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(31, 13)
            Me.Label3.TabIndex = 0
            Me.Label3.Text = "Color"
            '
            'ColorsComboBox
            '
            Me.ColorsComboBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ColorsComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
            Me.ColorsComboBox.FormattingEnabled = True
            Me.ColorsComboBox.Location = New System.Drawing.Point(203, 53)
            Me.ColorsComboBox.Name = "ColorsComboBox"
            Me.ColorsComboBox.Size = New System.Drawing.Size(410, 21)
            Me.ColorsComboBox.TabIndex = 1
            '
            'RICTextBox
            '
            Me.RICTextBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.RICTextBox.Location = New System.Drawing.Point(203, 3)
            Me.RICTextBox.Name = "RICTextBox"
            Me.RICTextBox.Size = New System.Drawing.Size(410, 20)
            Me.RICTextBox.TabIndex = 2
            '
            'DescrTextBox
            '
            Me.DescrTextBox.Dock = System.Windows.Forms.DockStyle.Fill
            Me.DescrTextBox.Location = New System.Drawing.Point(203, 28)
            Me.DescrTextBox.Name = "DescrTextBox"
            Me.DescrTextBox.Size = New System.Drawing.Size(410, 20)
            Me.DescrTextBox.TabIndex = 3
            '
            'OkButton
            '
            Me.OkButton.Anchor = System.Windows.Forms.AnchorStyles.Left
            Me.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.OkButton.Location = New System.Drawing.Point(3, 205)
            Me.OkButton.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
            Me.OkButton.Name = "OkButton"
            Me.OkButton.Padding = New System.Windows.Forms.Padding(3, 0, 0, 0)
            Me.OkButton.Size = New System.Drawing.Size(75, 23)
            Me.OkButton.TabIndex = 4
            Me.OkButton.Text = "Ok"
            Me.OkButton.UseVisualStyleBackColor = True
            '
            'CancelButton
            '
            Me.CancelButton.Anchor = System.Windows.Forms.AnchorStyles.Right
            Me.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.CancelButton.Location = New System.Drawing.Point(538, 205)
            Me.CancelButton.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
            Me.CancelButton.Name = "CancelButton"
            Me.CancelButton.Size = New System.Drawing.Size(75, 23)
            Me.CancelButton.TabIndex = 4
            Me.CancelButton.Text = "Cancel"
            Me.CancelButton.UseVisualStyleBackColor = True
            '
            'Label1
            '
            Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Left
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(3, 81)
            Me.Label1.Margin = New System.Windows.Forms.Padding(3)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(22, 13)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "Bid"
            '
            'Label4
            '
            Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.Left
            Me.Label4.AutoSize = True
            Me.Label4.Location = New System.Drawing.Point(3, 106)
            Me.Label4.Margin = New System.Windows.Forms.Padding(3)
            Me.Label4.Name = "Label4"
            Me.Label4.Size = New System.Drawing.Size(25, 13)
            Me.Label4.TabIndex = 0
            Me.Label4.Text = "Ask"
            '
            'Label5
            '
            Me.Label5.Anchor = System.Windows.Forms.AnchorStyles.Left
            Me.Label5.AutoSize = True
            Me.Label5.Location = New System.Drawing.Point(3, 131)
            Me.Label5.Margin = New System.Windows.Forms.Padding(3)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(27, 13)
            Me.Label5.TabIndex = 0
            Me.Label5.Text = "Last"
            '
            'Label6
            '
            Me.Label6.Anchor = System.Windows.Forms.AnchorStyles.Left
            Me.Label6.AutoSize = True
            Me.Label6.Location = New System.Drawing.Point(3, 156)
            Me.Label6.Margin = New System.Windows.Forms.Padding(3)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New System.Drawing.Size(33, 13)
            Me.Label6.TabIndex = 0
            Me.Label6.Text = "Close"
            '
            'CurveCheckBox
            '
            Me.CurveCheckBox.AutoSize = True
            Me.CurveCheckBox.Location = New System.Drawing.Point(203, 178)
            Me.CurveCheckBox.Name = "CurveCheckBox"
            Me.CurveCheckBox.Size = New System.Drawing.Size(157, 17)
            Me.CurveCheckBox.TabIndex = 9
            Me.CurveCheckBox.Text = "Can be used to plot a curve"
            Me.CurveCheckBox.UseVisualStyleBackColor = True
            '
            'NewChainForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(616, 234)
            Me.Controls.Add(Me.TheLayout)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.Name = "NewChainForm"
            Me.Text = "Chain"
            Me.TheLayout.ResumeLayout(False)
            Me.TheLayout.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents TheLayout As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents RICLabel As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents TheColorDialog As System.Windows.Forms.ColorDialog
        Friend WithEvents ColorsComboBox As System.Windows.Forms.ComboBox
        Friend WithEvents RICTextBox As System.Windows.Forms.TextBox
        Friend WithEvents DescrTextBox As System.Windows.Forms.TextBox
        Friend WithEvents OkButton As System.Windows.Forms.Button
        Friend WithEvents CancelButton As System.Windows.Forms.Button
        Friend WithEvents CloseTextBox As System.Windows.Forms.TextBox
        Friend WithEvents LastTextBox As System.Windows.Forms.TextBox
        Friend WithEvents AskTextBox As System.Windows.Forms.TextBox
        Friend WithEvents BidTextBox As System.Windows.Forms.TextBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Label4 As System.Windows.Forms.Label
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents CurveCheckBox As System.Windows.Forms.CheckBox
    End Class
End Namespace