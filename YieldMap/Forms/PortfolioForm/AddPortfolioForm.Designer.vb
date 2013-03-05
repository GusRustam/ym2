Namespace Forms.PortfolioForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class AddPortfolioForm
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
            Me.Label1 = New System.Windows.Forms.Label()
            Me.Label2 = New System.Windows.Forms.Label()
            Me.Panel1 = New System.Windows.Forms.Panel()
            Me.ItsPortfolio = New System.Windows.Forms.RadioButton()
            Me.ItsFolder = New System.Windows.Forms.RadioButton()
            Me.NewName = New System.Windows.Forms.TextBox()
            Me.OkButton = New System.Windows.Forms.Button()
            Me.CancelButton = New System.Windows.Forms.Button()
            Me.TableLayoutPanel1.SuspendLayout()
            Me.Panel1.SuspendLayout()
            Me.SuspendLayout()
            '
            'TableLayoutPanel1
            '
            Me.TableLayoutPanel1.ColumnCount = 2
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66667!))
            Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
            Me.TableLayoutPanel1.Controls.Add(Me.Label2, 0, 1)
            Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 1, 1)
            Me.TableLayoutPanel1.Controls.Add(Me.NewName, 1, 0)
            Me.TableLayoutPanel1.Controls.Add(Me.OkButton, 0, 2)
            Me.TableLayoutPanel1.Controls.Add(Me.CancelButton, 1, 2)
            Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
            Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
            Me.TableLayoutPanel1.RowCount = 3
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            Me.TableLayoutPanel1.Size = New System.Drawing.Size(380, 95)
            Me.TableLayoutPanel1.TabIndex = 0
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(3, 0)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(35, 13)
            Me.Label1.TabIndex = 0
            Me.Label1.Text = "Name"
            '
            'Label2
            '
            Me.Label2.AutoSize = True
            Me.Label2.Location = New System.Drawing.Point(3, 32)
            Me.Label2.Name = "Label2"
            Me.Label2.Size = New System.Drawing.Size(31, 13)
            Me.Label2.TabIndex = 1
            Me.Label2.Text = "Type"
            '
            'Panel1
            '
            Me.Panel1.Controls.Add(Me.ItsFolder)
            Me.Panel1.Controls.Add(Me.ItsPortfolio)
            Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.Panel1.Location = New System.Drawing.Point(129, 35)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(248, 26)
            Me.Panel1.TabIndex = 2
            '
            'ItsPortfolio
            '
            Me.ItsPortfolio.AutoSize = True
            Me.ItsPortfolio.Checked = True
            Me.ItsPortfolio.Location = New System.Drawing.Point(0, -2)
            Me.ItsPortfolio.Name = "ItsPortfolio"
            Me.ItsPortfolio.Size = New System.Drawing.Size(63, 17)
            Me.ItsPortfolio.TabIndex = 0
            Me.ItsPortfolio.TabStop = True
            Me.ItsPortfolio.Text = "Portfolio"
            Me.ItsPortfolio.UseVisualStyleBackColor = True
            '
            'ItsFolder
            '
            Me.ItsFolder.AutoSize = True
            Me.ItsFolder.Location = New System.Drawing.Point(69, 0)
            Me.ItsFolder.Name = "ItsFolder"
            Me.ItsFolder.Size = New System.Drawing.Size(54, 17)
            Me.ItsFolder.TabIndex = 1
            Me.ItsFolder.Text = "Folder"
            Me.ItsFolder.UseVisualStyleBackColor = True
            '
            'NewName
            '
            Me.NewName.Dock = System.Windows.Forms.DockStyle.Top
            Me.NewName.Location = New System.Drawing.Point(129, 3)
            Me.NewName.Name = "NewName"
            Me.NewName.Size = New System.Drawing.Size(248, 20)
            Me.NewName.TabIndex = 3
            '
            'OkButton
            '
            Me.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.OkButton.Location = New System.Drawing.Point(3, 67)
            Me.OkButton.Name = "OkButton"
            Me.OkButton.Size = New System.Drawing.Size(75, 25)
            Me.OkButton.TabIndex = 4
            Me.OkButton.Text = "Ok"
            Me.OkButton.UseVisualStyleBackColor = True
            '
            'CancelButton
            '
            Me.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.CancelButton.Location = New System.Drawing.Point(129, 67)
            Me.CancelButton.Name = "CancelButton"
            Me.CancelButton.Size = New System.Drawing.Size(75, 25)
            Me.CancelButton.TabIndex = 5
            Me.CancelButton.Text = "Cancel"
            Me.CancelButton.UseVisualStyleBackColor = True
            '
            'AddPortfolioForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(380, 95)
            Me.Controls.Add(Me.TableLayoutPanel1)
            Me.MinimumSize = New System.Drawing.Size(220, 110)
            Me.Name = "AddPortfolioForm"
            Me.Text = "AddPortfolioForm"
            Me.TableLayoutPanel1.ResumeLayout(False)
            Me.TableLayoutPanel1.PerformLayout()
            Me.Panel1.ResumeLayout(False)
            Me.Panel1.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents Label2 As System.Windows.Forms.Label
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents ItsFolder As System.Windows.Forms.RadioButton
        Friend WithEvents ItsPortfolio As System.Windows.Forms.RadioButton
        Friend WithEvents NewName As System.Windows.Forms.TextBox
        Friend WithEvents OkButton As System.Windows.Forms.Button
        Friend WithEvents CancelButton As System.Windows.Forms.Button
    End Class
End Namespace