Namespace Forms.PortfolioForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ParserErrorForm
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
            Dim Label3 As System.Windows.Forms.Label
            Dim Label4 As System.Windows.Forms.Label
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ParserErrorForm))
            Me.ConditionTB = New System.Windows.Forms.TextBox()
            Me.VariablesLB = New System.Windows.Forms.ListBox()
            Me.AddVarButton = New System.Windows.Forms.Button()
            Me.MessagesTB = New System.Windows.Forms.TextBox()
            Me.TryButton = New System.Windows.Forms.Button()
            Me.GrammarTB = New System.Windows.Forms.TextBox()
            Me.OkButton = New System.Windows.Forms.Button()
            Me.CancelButton = New System.Windows.Forms.Button()
            Label1 = New System.Windows.Forms.Label()
            Label2 = New System.Windows.Forms.Label()
            Label3 = New System.Windows.Forms.Label()
            Label4 = New System.Windows.Forms.Label()
            Me.SuspendLayout()
            '
            'Label1
            '
            Label1.AutoSize = True
            Label1.Location = New System.Drawing.Point(12, 9)
            Label1.Name = "Label1"
            Label1.Size = New System.Drawing.Size(51, 13)
            Label1.TabIndex = 0
            Label1.Text = "Condition"
            '
            'ConditionTB
            '
            Me.ConditionTB.Location = New System.Drawing.Point(69, 6)
            Me.ConditionTB.Name = "ConditionTB"
            Me.ConditionTB.Size = New System.Drawing.Size(488, 20)
            Me.ConditionTB.TabIndex = 1
            '
            'Label2
            '
            Label2.AutoSize = True
            Label2.Location = New System.Drawing.Point(12, 43)
            Label2.Name = "Label2"
            Label2.Size = New System.Drawing.Size(95, 13)
            Label2.TabIndex = 2
            Label2.Text = "Available variables"
            '
            'VariablesLB
            '
            Me.VariablesLB.FormattingEnabled = True
            Me.VariablesLB.Location = New System.Drawing.Point(15, 59)
            Me.VariablesLB.Name = "VariablesLB"
            Me.VariablesLB.Size = New System.Drawing.Size(209, 186)
            Me.VariablesLB.TabIndex = 3
            '
            'AddVarButton
            '
            Me.AddVarButton.Location = New System.Drawing.Point(15, 251)
            Me.AddVarButton.Name = "AddVarButton"
            Me.AddVarButton.Size = New System.Drawing.Size(209, 23)
            Me.AddVarButton.TabIndex = 4
            Me.AddVarButton.Text = "Add variable"
            Me.AddVarButton.UseVisualStyleBackColor = True
            '
            'Label3
            '
            Label3.AutoSize = True
            Label3.Location = New System.Drawing.Point(12, 289)
            Label3.Name = "Label3"
            Label3.Size = New System.Drawing.Size(55, 13)
            Label3.TabIndex = 5
            Label3.Text = "Messages"
            '
            'MessagesTB
            '
            Me.MessagesTB.Enabled = False
            Me.MessagesTB.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.MessagesTB.Location = New System.Drawing.Point(15, 305)
            Me.MessagesTB.Multiline = True
            Me.MessagesTB.Name = "MessagesTB"
            Me.MessagesTB.Size = New System.Drawing.Size(633, 136)
            Me.MessagesTB.TabIndex = 6
            '
            'TryButton
            '
            Me.TryButton.Location = New System.Drawing.Point(563, 4)
            Me.TryButton.Name = "TryButton"
            Me.TryButton.Size = New System.Drawing.Size(85, 23)
            Me.TryButton.TabIndex = 7
            Me.TryButton.Text = "Try"
            Me.TryButton.UseVisualStyleBackColor = True
            '
            'Label4
            '
            Label4.AutoSize = True
            Label4.Location = New System.Drawing.Point(227, 43)
            Label4.Name = "Label4"
            Label4.Size = New System.Drawing.Size(49, 13)
            Label4.TabIndex = 5
            Label4.Text = "Grammar"
            '
            'GrammarTB
            '
            Me.GrammarTB.Enabled = False
            Me.GrammarTB.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.GrammarTB.Location = New System.Drawing.Point(230, 59)
            Me.GrammarTB.Multiline = True
            Me.GrammarTB.Name = "GrammarTB"
            Me.GrammarTB.Size = New System.Drawing.Size(418, 215)
            Me.GrammarTB.TabIndex = 6
            Me.GrammarTB.Text = resources.GetString("GrammarTB.Text")
            '
            'OkButton
            '
            Me.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.OkButton.Location = New System.Drawing.Point(15, 447)
            Me.OkButton.Name = "OkButton"
            Me.OkButton.Size = New System.Drawing.Size(75, 23)
            Me.OkButton.TabIndex = 8
            Me.OkButton.Text = "Ok"
            Me.OkButton.UseVisualStyleBackColor = True
            '
            'CancelButton
            '
            Me.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.CancelButton.Location = New System.Drawing.Point(573, 447)
            Me.CancelButton.Name = "CancelButton"
            Me.CancelButton.Size = New System.Drawing.Size(75, 23)
            Me.CancelButton.TabIndex = 8
            Me.CancelButton.Text = "Cancel"
            Me.CancelButton.UseVisualStyleBackColor = True
            '
            'ParserErrorForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.CancelButton
            Me.ClientSize = New System.Drawing.Size(666, 477)
            Me.Controls.Add(Me.CancelButton)
            Me.Controls.Add(Me.OkButton)
            Me.Controls.Add(Me.TryButton)
            Me.Controls.Add(Me.GrammarTB)
            Me.Controls.Add(Label4)
            Me.Controls.Add(Me.MessagesTB)
            Me.Controls.Add(Label3)
            Me.Controls.Add(Me.AddVarButton)
            Me.Controls.Add(Me.VariablesLB)
            Me.Controls.Add(Label2)
            Me.Controls.Add(Me.ConditionTB)
            Me.Controls.Add(Label1)
            Me.Name = "ParserErrorForm"
            Me.Text = "Condition editor"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents ConditionTB As System.Windows.Forms.TextBox
        Friend WithEvents VariablesLB As System.Windows.Forms.ListBox
        Friend WithEvents AddVarButton As System.Windows.Forms.Button
        Friend WithEvents MessagesTB As System.Windows.Forms.TextBox
        Friend WithEvents TryButton As System.Windows.Forms.Button
        Friend WithEvents GrammarTB As System.Windows.Forms.TextBox
        Friend WithEvents OkButton As System.Windows.Forms.Button
        Friend WithEvents CancelButton As System.Windows.Forms.Button
    End Class
End Namespace