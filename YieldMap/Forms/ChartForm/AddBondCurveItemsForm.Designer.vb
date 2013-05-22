Namespace Forms.ChartForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class AddBondCurveItemsForm
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
            Dim TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
            Me.OkButton = New System.Windows.Forms.Button()
            Me.CancelButton = New System.Windows.Forms.Button()
            Me.ItemsDGV = New System.Windows.Forms.DataGridView()
            TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
            TableLayoutPanel1.SuspendLayout()
            CType(Me.ItemsDGV, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'TableLayoutPanel1
            '
            TableLayoutPanel1.ColumnCount = 2
            TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TableLayoutPanel1.Controls.Add(Me.OkButton, 0, 1)
            TableLayoutPanel1.Controls.Add(Me.CancelButton, 1, 1)
            TableLayoutPanel1.Controls.Add(Me.ItemsDGV, 0, 0)
            TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
            TableLayoutPanel1.Name = "TableLayoutPanel1"
            TableLayoutPanel1.RowCount = 2
            TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TableLayoutPanel1.Size = New System.Drawing.Size(539, 273)
            TableLayoutPanel1.TabIndex = 0
            '
            'OkButton
            '
            Me.OkButton.Location = New System.Drawing.Point(0, 248)
            Me.OkButton.Margin = New System.Windows.Forms.Padding(0)
            Me.OkButton.Name = "OkButton"
            Me.OkButton.Size = New System.Drawing.Size(121, 23)
            Me.OkButton.TabIndex = 0
            Me.OkButton.Text = "Add selected"
            Me.OkButton.UseVisualStyleBackColor = True
            '
            'CancelButton
            '
            Me.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.CancelButton.Dock = System.Windows.Forms.DockStyle.Right
            Me.CancelButton.Location = New System.Drawing.Point(417, 248)
            Me.CancelButton.Margin = New System.Windows.Forms.Padding(0)
            Me.CancelButton.Name = "CancelButton"
            Me.CancelButton.Size = New System.Drawing.Size(122, 25)
            Me.CancelButton.TabIndex = 1
            Me.CancelButton.Text = "Cancel"
            Me.CancelButton.UseVisualStyleBackColor = True
            '
            'ItemsDGV
            '
            Me.ItemsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            TableLayoutPanel1.SetColumnSpan(Me.ItemsDGV, 2)
            Me.ItemsDGV.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ItemsDGV.Location = New System.Drawing.Point(3, 3)
            Me.ItemsDGV.Name = "ItemsDGV"
            Me.ItemsDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.ItemsDGV.Size = New System.Drawing.Size(533, 242)
            Me.ItemsDGV.TabIndex = 2
            '
            'AddBondCurveItemsForm
            '
            Me.AcceptButton = Me.OkButton
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(539, 273)
            Me.Controls.Add(TableLayoutPanel1)
            Me.Name = "AddBondCurveItemsForm"
            Me.Text = "Add bond curve items"
            TableLayoutPanel1.ResumeLayout(False)
            CType(Me.ItemsDGV, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents OkButton As System.Windows.Forms.Button
        Friend WithEvents CancelButton As System.Windows.Forms.Button
        Friend WithEvents ItemsDGV As System.Windows.Forms.DataGridView
    End Class
End Namespace