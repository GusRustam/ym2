Namespace Forms.PortfolioForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class AddPortfolioSource
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
            Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
            Me.MainTabControl = New System.Windows.Forms.TabControl()
            Me.ChainOrListTP = New System.Windows.Forms.TabPage()
            Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
            Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel()
            Me.ShowChainsRB = New System.Windows.Forms.RadioButton()
            Me.ShowListsRB = New System.Windows.Forms.RadioButton()
            Me.ChainsListsLB = New System.Windows.Forms.ListBox()
            Me.IndividualAndCustomBondsTP = New System.Windows.Forms.TabPage()
            Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
            Me.FlowLayoutPanel4 = New System.Windows.Forms.FlowLayoutPanel()
            Me.IndBondsRB = New System.Windows.Forms.RadioButton()
            Me.CustomBondsRB = New System.Windows.Forms.RadioButton()
            Me.BondsDGV = New System.Windows.Forms.DataGridView()
            Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
            Me.OkButton = New System.Windows.Forms.Button()
            Me.CancelButton = New System.Windows.Forms.Button()
            Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
            Me.ConditionTB = New System.Windows.Forms.TextBox()
            Me.Label5 = New System.Windows.Forms.Label()
            Me.Label3 = New System.Windows.Forms.Label()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.CustomNameTB = New System.Windows.Forms.TextBox()
            Me.FlowLayoutPanel3 = New System.Windows.Forms.FlowLayoutPanel()
            Me.CustomColorCB = New System.Windows.Forms.ComboBox()
            Me.SampleColorPB = New System.Windows.Forms.PictureBox()
            Me.RandomColorB = New System.Windows.Forms.Button()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.IncludeCB = New System.Windows.Forms.CheckBox()
            Me.TableLayoutPanel1.SuspendLayout()
            Me.MainTabControl.SuspendLayout()
            Me.ChainOrListTP.SuspendLayout()
            Me.TableLayoutPanel2.SuspendLayout()
            Me.FlowLayoutPanel2.SuspendLayout()
            Me.IndividualAndCustomBondsTP.SuspendLayout()
            Me.TableLayoutPanel4.SuspendLayout()
            Me.FlowLayoutPanel4.SuspendLayout()
            CType(Me.BondsDGV, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.FlowLayoutPanel1.SuspendLayout()
            Me.TableLayoutPanel3.SuspendLayout()
            Me.FlowLayoutPanel3.SuspendLayout()
            CType(Me.SampleColorPB, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'TableLayoutPanel1
            '
            Me.TableLayoutPanel1.ColumnCount = 1
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel1.Controls.Add(Me.MainTabControl, 0, 0)
            Me.TableLayoutPanel1.Controls.Add(Me.FlowLayoutPanel1, 0, 2)
            Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel3, 0, 1)
            Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
            Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
            Me.TableLayoutPanel1.RowCount = 3
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            Me.TableLayoutPanel1.Size = New System.Drawing.Size(422, 451)
            Me.TableLayoutPanel1.TabIndex = 1
            '
            'MainTabControl
            '
            Me.MainTabControl.Controls.Add(Me.ChainOrListTP)
            Me.MainTabControl.Controls.Add(Me.IndividualAndCustomBondsTP)
            Me.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill
            Me.MainTabControl.Location = New System.Drawing.Point(3, 3)
            Me.MainTabControl.Name = "MainTabControl"
            Me.MainTabControl.SelectedIndex = 0
            Me.MainTabControl.Size = New System.Drawing.Size(416, 295)
            Me.MainTabControl.TabIndex = 1
            '
            'ChainOrListTP
            '
            Me.ChainOrListTP.Controls.Add(Me.TableLayoutPanel2)
            Me.ChainOrListTP.Location = New System.Drawing.Point(4, 22)
            Me.ChainOrListTP.Name = "ChainOrListTP"
            Me.ChainOrListTP.Padding = New System.Windows.Forms.Padding(3)
            Me.ChainOrListTP.Size = New System.Drawing.Size(408, 269)
            Me.ChainOrListTP.TabIndex = 0
            Me.ChainOrListTP.Text = "Chain or List"
            Me.ChainOrListTP.UseVisualStyleBackColor = True
            '
            'TableLayoutPanel2
            '
            Me.TableLayoutPanel2.ColumnCount = 1
            Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel2.Controls.Add(Me.FlowLayoutPanel2, 0, 0)
            Me.TableLayoutPanel2.Controls.Add(Me.ChainsListsLB, 0, 1)
            Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 3)
            Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
            Me.TableLayoutPanel2.RowCount = 2
            Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel2.Size = New System.Drawing.Size(402, 263)
            Me.TableLayoutPanel2.TabIndex = 0
            '
            'FlowLayoutPanel2
            '
            Me.FlowLayoutPanel2.Controls.Add(Me.ShowChainsRB)
            Me.FlowLayoutPanel2.Controls.Add(Me.ShowListsRB)
            Me.FlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FlowLayoutPanel2.Location = New System.Drawing.Point(3, 3)
            Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
            Me.FlowLayoutPanel2.Size = New System.Drawing.Size(396, 24)
            Me.FlowLayoutPanel2.TabIndex = 0
            '
            'ShowChainsRB
            '
            Me.ShowChainsRB.AutoSize = True
            Me.ShowChainsRB.Checked = True
            Me.ShowChainsRB.Location = New System.Drawing.Point(3, 3)
            Me.ShowChainsRB.Name = "ShowChainsRB"
            Me.ShowChainsRB.Size = New System.Drawing.Size(57, 17)
            Me.ShowChainsRB.TabIndex = 0
            Me.ShowChainsRB.TabStop = True
            Me.ShowChainsRB.Text = "Chains"
            Me.ShowChainsRB.UseVisualStyleBackColor = True
            '
            'ShowListsRB
            '
            Me.ShowListsRB.AutoSize = True
            Me.ShowListsRB.Location = New System.Drawing.Point(66, 3)
            Me.ShowListsRB.Name = "ShowListsRB"
            Me.ShowListsRB.Size = New System.Drawing.Size(46, 17)
            Me.ShowListsRB.TabIndex = 1
            Me.ShowListsRB.Text = "Lists"
            Me.ShowListsRB.UseVisualStyleBackColor = True
            '
            'ChainsListsLB
            '
            Me.ChainsListsLB.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ChainsListsLB.FormattingEnabled = True
            Me.ChainsListsLB.Location = New System.Drawing.Point(3, 33)
            Me.ChainsListsLB.Name = "ChainsListsLB"
            Me.ChainsListsLB.Size = New System.Drawing.Size(396, 227)
            Me.ChainsListsLB.TabIndex = 1
            '
            'IndividualAndCustomBondsTP
            '
            Me.IndividualAndCustomBondsTP.Controls.Add(Me.TableLayoutPanel4)
            Me.IndividualAndCustomBondsTP.Location = New System.Drawing.Point(4, 22)
            Me.IndividualAndCustomBondsTP.Name = "IndividualAndCustomBondsTP"
            Me.IndividualAndCustomBondsTP.Padding = New System.Windows.Forms.Padding(3)
            Me.IndividualAndCustomBondsTP.Size = New System.Drawing.Size(408, 313)
            Me.IndividualAndCustomBondsTP.TabIndex = 1
            Me.IndividualAndCustomBondsTP.Text = "Bonds"
            Me.IndividualAndCustomBondsTP.UseVisualStyleBackColor = True
            '
            'TableLayoutPanel4
            '
            Me.TableLayoutPanel4.ColumnCount = 1
            Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel4.Controls.Add(Me.FlowLayoutPanel4, 0, 0)
            Me.TableLayoutPanel4.Controls.Add(Me.BondsDGV, 0, 1)
            Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel4.Location = New System.Drawing.Point(3, 3)
            Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
            Me.TableLayoutPanel4.RowCount = 2
            Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel4.Size = New System.Drawing.Size(402, 307)
            Me.TableLayoutPanel4.TabIndex = 1
            '
            'FlowLayoutPanel4
            '
            Me.FlowLayoutPanel4.Controls.Add(Me.IndBondsRB)
            Me.FlowLayoutPanel4.Controls.Add(Me.CustomBondsRB)
            Me.FlowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FlowLayoutPanel4.Location = New System.Drawing.Point(3, 3)
            Me.FlowLayoutPanel4.Name = "FlowLayoutPanel4"
            Me.FlowLayoutPanel4.Size = New System.Drawing.Size(396, 24)
            Me.FlowLayoutPanel4.TabIndex = 0
            '
            'IndBondsRB
            '
            Me.IndBondsRB.AutoSize = True
            Me.IndBondsRB.Checked = True
            Me.IndBondsRB.Location = New System.Drawing.Point(3, 3)
            Me.IndBondsRB.Name = "IndBondsRB"
            Me.IndBondsRB.Size = New System.Drawing.Size(62, 17)
            Me.IndBondsRB.TabIndex = 0
            Me.IndBondsRB.TabStop = True
            Me.IndBondsRB.Text = "Regular"
            Me.IndBondsRB.UseVisualStyleBackColor = True
            '
            'CustomBondsRB
            '
            Me.CustomBondsRB.AutoSize = True
            Me.CustomBondsRB.Location = New System.Drawing.Point(71, 3)
            Me.CustomBondsRB.Name = "CustomBondsRB"
            Me.CustomBondsRB.Size = New System.Drawing.Size(60, 17)
            Me.CustomBondsRB.TabIndex = 1
            Me.CustomBondsRB.Text = "Custom"
            Me.CustomBondsRB.UseVisualStyleBackColor = True
            '
            'BondsDGV
            '
            Me.BondsDGV.AllowUserToAddRows = False
            Me.BondsDGV.AllowUserToDeleteRows = False
            Me.BondsDGV.AllowUserToResizeRows = False
            Me.BondsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.BondsDGV.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BondsDGV.Location = New System.Drawing.Point(3, 33)
            Me.BondsDGV.Name = "BondsDGV"
            Me.BondsDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.BondsDGV.Size = New System.Drawing.Size(396, 271)
            Me.BondsDGV.TabIndex = 1
            '
            'FlowLayoutPanel1
            '
            Me.FlowLayoutPanel1.Controls.Add(Me.OkButton)
            Me.FlowLayoutPanel1.Controls.Add(Me.CancelButton)
            Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FlowLayoutPanel1.Location = New System.Drawing.Point(3, 424)
            Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
            Me.FlowLayoutPanel1.Size = New System.Drawing.Size(416, 24)
            Me.FlowLayoutPanel1.TabIndex = 2
            '
            'OkButton
            '
            Me.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.OkButton.Location = New System.Drawing.Point(3, 3)
            Me.OkButton.Name = "OkButton"
            Me.OkButton.Size = New System.Drawing.Size(75, 21)
            Me.OkButton.TabIndex = 0
            Me.OkButton.Text = "Ok"
            Me.OkButton.UseVisualStyleBackColor = True
            '
            'CancelButton
            '
            Me.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.CancelButton.Location = New System.Drawing.Point(84, 3)
            Me.CancelButton.Name = "CancelButton"
            Me.CancelButton.Size = New System.Drawing.Size(75, 21)
            Me.CancelButton.TabIndex = 1
            Me.CancelButton.Text = "Cancel"
            Me.CancelButton.UseVisualStyleBackColor = True
            '
            'TableLayoutPanel3
            '
            Me.TableLayoutPanel3.ColumnCount = 2
            Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.57143!))
            Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 71.42857!))
            Me.TableLayoutPanel3.Controls.Add(Me.Label2, 0, 3)
            Me.TableLayoutPanel3.Controls.Add(Me.ConditionTB, 1, 2)
            Me.TableLayoutPanel3.Controls.Add(Me.Label5, 0, 2)
            Me.TableLayoutPanel3.Controls.Add(Me.Label3, 0, 1)
            Me.TableLayoutPanel3.Controls.Add(Me.Label1, 0, 0)
            Me.TableLayoutPanel3.Controls.Add(Me.CustomNameTB, 1, 0)
            Me.TableLayoutPanel3.Controls.Add(Me.FlowLayoutPanel3, 1, 1)
            Me.TableLayoutPanel3.Controls.Add(Me.IncludeCB, 1, 3)
            Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel3.Location = New System.Drawing.Point(3, 304)
            Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
            Me.TableLayoutPanel3.RowCount = 4
            Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062!))
            Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062!))
            Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062!))
            Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.99813!))
            Me.TableLayoutPanel3.Size = New System.Drawing.Size(416, 114)
            Me.TableLayoutPanel3.TabIndex = 3
            '
            'ConditionTB
            '
            Me.ConditionTB.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ConditionTB.Enabled = False
            Me.ConditionTB.Location = New System.Drawing.Point(121, 59)
            Me.ConditionTB.Name = "ConditionTB"
            Me.ConditionTB.Size = New System.Drawing.Size(292, 20)
            Me.ConditionTB.TabIndex = 7
            '
            'Label5
            '
            Me.Label5.AutoSize = True
            Me.Label5.Enabled = False
            Me.Label5.Location = New System.Drawing.Point(3, 59)
            Me.Label5.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
            Me.Label5.Name = "Label5"
            Me.Label5.Size = New System.Drawing.Size(51, 13)
            Me.Label5.TabIndex = 4
            Me.Label5.Text = "Condition"
            '
            'Label3
            '
            Me.Label3.AutoSize = True
            Me.Label3.Location = New System.Drawing.Point(3, 31)
            Me.Label3.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
            Me.Label3.Name = "Label3"
            Me.Label3.Size = New System.Drawing.Size(68, 13)
            Me.Label3.TabIndex = 2
            Me.Label3.Text = "Custom color"
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(3, 3)
            Me.Label1.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(71, 13)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "Custom name"
            '
            'CustomNameTB
            '
            Me.CustomNameTB.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CustomNameTB.Location = New System.Drawing.Point(121, 3)
            Me.CustomNameTB.Name = "CustomNameTB"
            Me.CustomNameTB.Size = New System.Drawing.Size(292, 20)
            Me.CustomNameTB.TabIndex = 5
            '
            'FlowLayoutPanel3
            '
            Me.FlowLayoutPanel3.Controls.Add(Me.CustomColorCB)
            Me.FlowLayoutPanel3.Controls.Add(Me.SampleColorPB)
            Me.FlowLayoutPanel3.Controls.Add(Me.RandomColorB)
            Me.FlowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FlowLayoutPanel3.Location = New System.Drawing.Point(118, 28)
            Me.FlowLayoutPanel3.Margin = New System.Windows.Forms.Padding(0)
            Me.FlowLayoutPanel3.Name = "FlowLayoutPanel3"
            Me.FlowLayoutPanel3.Size = New System.Drawing.Size(298, 28)
            Me.FlowLayoutPanel3.TabIndex = 6
            '
            'CustomColorCB
            '
            Me.CustomColorCB.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable
            Me.CustomColorCB.FormattingEnabled = True
            Me.CustomColorCB.Location = New System.Drawing.Point(3, 3)
            Me.CustomColorCB.Name = "CustomColorCB"
            Me.CustomColorCB.Size = New System.Drawing.Size(191, 21)
            Me.CustomColorCB.TabIndex = 0
            '
            'SampleColorPB
            '
            Me.SampleColorPB.Location = New System.Drawing.Point(200, 3)
            Me.SampleColorPB.Name = "SampleColorPB"
            Me.SampleColorPB.Size = New System.Drawing.Size(28, 21)
            Me.SampleColorPB.TabIndex = 1
            Me.SampleColorPB.TabStop = False
            '
            'RandomColorB
            '
            Me.RandomColorB.Location = New System.Drawing.Point(234, 3)
            Me.RandomColorB.Name = "RandomColorB"
            Me.RandomColorB.Size = New System.Drawing.Size(55, 21)
            Me.RandomColorB.TabIndex = 2
            Me.RandomColorB.Text = "Random"
            Me.RandomColorB.UseVisualStyleBackColor = True
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Enabled = False
            Me.Label2.Location = New System.Drawing.Point(3, 87)
            Me.Label2.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(42, 13)
            Me.Label2.TabIndex = 8
            Me.Label2.Text = "Include"
            '
            'IncludeCB
            '
            Me.IncludeCB.AutoSize = True
            Me.IncludeCB.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.IncludeCB.Checked = True
            Me.IncludeCB.CheckState = System.Windows.Forms.CheckState.Checked
            Me.IncludeCB.Location = New System.Drawing.Point(121, 87)
            Me.IncludeCB.Name = "IncludeCB"
            Me.IncludeCB.Size = New System.Drawing.Size(44, 17)
            Me.IncludeCB.TabIndex = 9
            Me.IncludeCB.Text = "Yes"
            Me.IncludeCB.UseVisualStyleBackColor = True
            '
            'AddPortfolioSource
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(422, 451)
            Me.Controls.Add(Me.TableLayoutPanel1)
            Me.MinimumSize = New System.Drawing.Size(400, 300)
            Me.Name = "AddPortfolioSource"
            Me.Text = "AddPortfolioSource"
            Me.TableLayoutPanel1.ResumeLayout(False)
            Me.MainTabControl.ResumeLayout(False)
            Me.ChainOrListTP.ResumeLayout(False)
            Me.TableLayoutPanel2.ResumeLayout(False)
            Me.FlowLayoutPanel2.ResumeLayout(False)
            Me.FlowLayoutPanel2.PerformLayout()
            Me.IndividualAndCustomBondsTP.ResumeLayout(False)
            Me.TableLayoutPanel4.ResumeLayout(False)
            Me.FlowLayoutPanel4.ResumeLayout(False)
            Me.FlowLayoutPanel4.PerformLayout()
            CType(Me.BondsDGV, System.ComponentModel.ISupportInitialize).EndInit()
            Me.FlowLayoutPanel1.ResumeLayout(False)
            Me.TableLayoutPanel3.ResumeLayout(False)
            Me.TableLayoutPanel3.PerformLayout()
            Me.FlowLayoutPanel3.ResumeLayout(False)
            CType(Me.SampleColorPB, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents MainTabControl As System.Windows.Forms.TabControl
        Friend WithEvents ChainOrListTP As System.Windows.Forms.TabPage
        Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents FlowLayoutPanel2 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents ShowChainsRB As System.Windows.Forms.RadioButton
        Friend WithEvents ShowListsRB As System.Windows.Forms.RadioButton
        Friend WithEvents ChainsListsLB As System.Windows.Forms.ListBox
        Friend WithEvents IndividualAndCustomBondsTP As System.Windows.Forms.TabPage
        Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents OkButton As System.Windows.Forms.Button
        Friend WithEvents CancelButton As System.Windows.Forms.Button
        Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents ConditionTB As System.Windows.Forms.TextBox
        Friend WithEvents Label5 As System.Windows.Forms.Label
        Friend WithEvents Label3 As System.Windows.Forms.Label
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents CustomNameTB As System.Windows.Forms.TextBox
        Friend WithEvents FlowLayoutPanel3 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents CustomColorCB As System.Windows.Forms.ComboBox
        Friend WithEvents SampleColorPB As System.Windows.Forms.PictureBox
        Friend WithEvents RandomColorB As System.Windows.Forms.Button
        Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents FlowLayoutPanel4 As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents IndBondsRB As System.Windows.Forms.RadioButton
        Friend WithEvents CustomBondsRB As System.Windows.Forms.RadioButton
        Friend WithEvents BondsDGV As System.Windows.Forms.DataGridView
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents IncludeCB As System.Windows.Forms.CheckBox
    End Class
End Namespace