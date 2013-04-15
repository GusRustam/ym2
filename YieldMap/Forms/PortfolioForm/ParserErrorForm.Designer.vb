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
            Dim Label3 As System.Windows.Forms.Label
            Dim Label4 As System.Windows.Forms.Label
            Dim Label6 As System.Windows.Forms.Label
            Dim Label5 As System.Windows.Forms.Label
            Dim Label2 As System.Windows.Forms.Label
            Dim Label7 As System.Windows.Forms.Label
            Dim Label8 As System.Windows.Forms.Label
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ParserErrorForm))
            Me.ConditionTB = New System.Windows.Forms.TextBox()
            Me.TryButton = New System.Windows.Forms.Button()
            Me.OkButton = New System.Windows.Forms.Button()
            Me.CancelButton = New System.Windows.Forms.Button()
            Me.RulesTP = New System.Windows.Forms.TabPage()
            Me.GrammarTB = New System.Windows.Forms.TextBox()
            Me.VariablesTP = New System.Windows.Forms.TabPage()
            Me.DateTimePckr = New System.Windows.Forms.DateTimePicker()
            Me.MoodysRB = New System.Windows.Forms.RadioButton()
            Me.SpFitchRB = New System.Windows.Forms.RadioButton()
            Me.AddDateButton = New System.Windows.Forms.Button()
            Me.AddSrcButton = New System.Windows.Forms.Button()
            Me.AddRatingButton = New System.Windows.Forms.Button()
            Me.RatingSrcLB = New System.Windows.Forms.ListBox()
            Me.RatingsLB = New System.Windows.Forms.ListBox()
            Me.AddVarButton = New System.Windows.Forms.Button()
            Me.VariablesLB = New System.Windows.Forms.ListBox()
            Me.MainTC = New System.Windows.Forms.TabControl()
            Me.MessagesTP = New System.Windows.Forms.TabPage()
            Me.MessagesTB = New System.Windows.Forms.TextBox()
            Label1 = New System.Windows.Forms.Label()
            Label3 = New System.Windows.Forms.Label()
            Label4 = New System.Windows.Forms.Label()
            Label6 = New System.Windows.Forms.Label()
            Label5 = New System.Windows.Forms.Label()
            Label2 = New System.Windows.Forms.Label()
            Label7 = New System.Windows.Forms.Label()
            Label8 = New System.Windows.Forms.Label()
            Me.RulesTP.SuspendLayout()
            Me.VariablesTP.SuspendLayout()
            Me.MainTC.SuspendLayout()
            Me.MessagesTP.SuspendLayout()
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
            'Label3
            '
            Label3.AutoSize = True
            Label3.Location = New System.Drawing.Point(12, 289)
            Label3.Name = "Label3"
            Label3.Size = New System.Drawing.Size(55, 13)
            Label3.TabIndex = 5
            Label3.Text = "Messages"
            '
            'Label4
            '
            Label4.AutoSize = True
            Label4.Location = New System.Drawing.Point(395, 43)
            Label4.Name = "Label4"
            Label4.Size = New System.Drawing.Size(49, 13)
            Label4.TabIndex = 5
            Label4.Text = "Grammar"
            '
            'Label6
            '
            Label6.AutoSize = True
            Label6.Location = New System.Drawing.Point(415, 5)
            Label6.Name = "Label6"
            Label6.Size = New System.Drawing.Size(78, 13)
            Label6.TabIndex = 16
            Label6.Text = "Rating sources"
            '
            'Label5
            '
            Label5.AutoSize = True
            Label5.Location = New System.Drawing.Point(229, 5)
            Label5.Name = "Label5"
            Label5.Size = New System.Drawing.Size(43, 13)
            Label5.TabIndex = 15
            Label5.Text = "Ratings"
            '
            'Label2
            '
            Label2.AutoSize = True
            Label2.Location = New System.Drawing.Point(3, 5)
            Label2.Name = "Label2"
            Label2.Size = New System.Drawing.Size(95, 13)
            Label2.TabIndex = 12
            Label2.Text = "Available variables"
            '
            'Label7
            '
            Label7.AutoSize = True
            Label7.Location = New System.Drawing.Point(229, 329)
            Label7.Name = "Label7"
            Label7.Size = New System.Drawing.Size(26, 13)
            Label7.TabIndex = 15
            Label7.Text = "Use"
            '
            'Label8
            '
            Label8.AutoSize = True
            Label8.Location = New System.Drawing.Point(415, 201)
            Label8.Name = "Label8"
            Label8.Size = New System.Drawing.Size(35, 13)
            Label8.TabIndex = 16
            Label8.Text = "Dates"
            '
            'ConditionTB
            '
            Me.ConditionTB.Location = New System.Drawing.Point(69, 6)
            Me.ConditionTB.Name = "ConditionTB"
            Me.ConditionTB.Size = New System.Drawing.Size(572, 20)
            Me.ConditionTB.TabIndex = 1
            '
            'TryButton
            '
            Me.TryButton.Location = New System.Drawing.Point(647, 3)
            Me.TryButton.Name = "TryButton"
            Me.TryButton.Size = New System.Drawing.Size(85, 23)
            Me.TryButton.TabIndex = 7
            Me.TryButton.Text = "Try"
            Me.TryButton.UseVisualStyleBackColor = True
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
            Me.CancelButton.Location = New System.Drawing.Point(657, 447)
            Me.CancelButton.Name = "CancelButton"
            Me.CancelButton.Size = New System.Drawing.Size(75, 23)
            Me.CancelButton.TabIndex = 8
            Me.CancelButton.Text = "Cancel"
            Me.CancelButton.UseVisualStyleBackColor = True
            '
            'RulesTP
            '
            Me.RulesTP.Controls.Add(Me.GrammarTB)
            Me.RulesTP.Location = New System.Drawing.Point(4, 22)
            Me.RulesTP.Name = "RulesTP"
            Me.RulesTP.Padding = New System.Windows.Forms.Padding(3)
            Me.RulesTP.Size = New System.Drawing.Size(709, 383)
            Me.RulesTP.TabIndex = 1
            Me.RulesTP.Text = "Grammar rules"
            Me.RulesTP.UseVisualStyleBackColor = True
            '
            'GrammarTB
            '
            Me.GrammarTB.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.GrammarTB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
            Me.GrammarTB.Location = New System.Drawing.Point(6, 6)
            Me.GrammarTB.Multiline = True
            Me.GrammarTB.Name = "GrammarTB"
            Me.GrammarTB.ReadOnly = True
            Me.GrammarTB.Size = New System.Drawing.Size(697, 371)
            Me.GrammarTB.TabIndex = 7
            Me.GrammarTB.Text = resources.GetString("GrammarTB.Text")
            '
            'VariablesTP
            '
            Me.VariablesTP.Controls.Add(Me.DateTimePckr)
            Me.VariablesTP.Controls.Add(Me.MoodysRB)
            Me.VariablesTP.Controls.Add(Me.SpFitchRB)
            Me.VariablesTP.Controls.Add(Me.AddDateButton)
            Me.VariablesTP.Controls.Add(Me.AddSrcButton)
            Me.VariablesTP.Controls.Add(Me.AddRatingButton)
            Me.VariablesTP.Controls.Add(Me.RatingSrcLB)
            Me.VariablesTP.Controls.Add(Label8)
            Me.VariablesTP.Controls.Add(Label6)
            Me.VariablesTP.Controls.Add(Me.RatingsLB)
            Me.VariablesTP.Controls.Add(Label7)
            Me.VariablesTP.Controls.Add(Label5)
            Me.VariablesTP.Controls.Add(Me.AddVarButton)
            Me.VariablesTP.Controls.Add(Me.VariablesLB)
            Me.VariablesTP.Controls.Add(Label2)
            Me.VariablesTP.Location = New System.Drawing.Point(4, 22)
            Me.VariablesTP.Name = "VariablesTP"
            Me.VariablesTP.Padding = New System.Windows.Forms.Padding(3)
            Me.VariablesTP.Size = New System.Drawing.Size(709, 383)
            Me.VariablesTP.TabIndex = 0
            Me.VariablesTP.Text = "Edit"
            Me.VariablesTP.UseVisualStyleBackColor = True
            '
            'DateTimePckr
            '
            Me.DateTimePckr.CustomFormat = "dd/MM/yyyy"
            Me.DateTimePckr.Format = System.Windows.Forms.DateTimePickerFormat.Custom
            Me.DateTimePckr.Location = New System.Drawing.Point(418, 217)
            Me.DateTimePckr.Name = "DateTimePckr"
            Me.DateTimePckr.Size = New System.Drawing.Size(200, 20)
            Me.DateTimePckr.TabIndex = 23
            '
            'MoodysRB
            '
            Me.MoodysRB.AutoSize = True
            Me.MoodysRB.Location = New System.Drawing.Point(337, 327)
            Me.MoodysRB.Name = "MoodysRB"
            Me.MoodysRB.Size = New System.Drawing.Size(64, 17)
            Me.MoodysRB.TabIndex = 22
            Me.MoodysRB.Text = "Moody's"
            Me.MoodysRB.UseVisualStyleBackColor = True
            '
            'SpFitchRB
            '
            Me.SpFitchRB.AutoSize = True
            Me.SpFitchRB.Checked = True
            Me.SpFitchRB.Location = New System.Drawing.Point(261, 327)
            Me.SpFitchRB.Name = "SpFitchRB"
            Me.SpFitchRB.Size = New System.Drawing.Size(74, 17)
            Me.SpFitchRB.TabIndex = 22
            Me.SpFitchRB.TabStop = True
            Me.SpFitchRB.Text = "S&&P, Fitch"
            Me.SpFitchRB.UseVisualStyleBackColor = True
            '
            'AddDateButton
            '
            Me.AddDateButton.Location = New System.Drawing.Point(418, 243)
            Me.AddDateButton.Name = "AddDateButton"
            Me.AddDateButton.Size = New System.Drawing.Size(200, 23)
            Me.AddDateButton.TabIndex = 19
            Me.AddDateButton.Text = "Add date"
            Me.AddDateButton.UseVisualStyleBackColor = True
            '
            'AddSrcButton
            '
            Me.AddSrcButton.Location = New System.Drawing.Point(418, 159)
            Me.AddSrcButton.Name = "AddSrcButton"
            Me.AddSrcButton.Size = New System.Drawing.Size(200, 23)
            Me.AddSrcButton.TabIndex = 19
            Me.AddSrcButton.Text = "Add source"
            Me.AddSrcButton.UseVisualStyleBackColor = True
            '
            'AddRatingButton
            '
            Me.AddRatingButton.Location = New System.Drawing.Point(232, 354)
            Me.AddRatingButton.Name = "AddRatingButton"
            Me.AddRatingButton.Size = New System.Drawing.Size(169, 23)
            Me.AddRatingButton.TabIndex = 20
            Me.AddRatingButton.Text = "Add rating"
            Me.AddRatingButton.UseVisualStyleBackColor = True
            '
            'RatingSrcLB
            '
            Me.RatingSrcLB.FormattingEnabled = True
            Me.RatingSrcLB.Location = New System.Drawing.Point(418, 19)
            Me.RatingSrcLB.Name = "RatingSrcLB"
            Me.RatingSrcLB.Size = New System.Drawing.Size(200, 134)
            Me.RatingSrcLB.TabIndex = 17
            '
            'RatingsLB
            '
            Me.RatingsLB.FormattingEnabled = True
            Me.RatingsLB.Location = New System.Drawing.Point(232, 19)
            Me.RatingsLB.Name = "RatingsLB"
            Me.RatingsLB.Size = New System.Drawing.Size(169, 303)
            Me.RatingsLB.TabIndex = 18
            '
            'AddVarButton
            '
            Me.AddVarButton.Location = New System.Drawing.Point(6, 354)
            Me.AddVarButton.Name = "AddVarButton"
            Me.AddVarButton.Size = New System.Drawing.Size(209, 23)
            Me.AddVarButton.TabIndex = 14
            Me.AddVarButton.Text = "Add variable"
            Me.AddVarButton.UseVisualStyleBackColor = True
            '
            'VariablesLB
            '
            Me.VariablesLB.FormattingEnabled = True
            Me.VariablesLB.Location = New System.Drawing.Point(6, 21)
            Me.VariablesLB.Name = "VariablesLB"
            Me.VariablesLB.Size = New System.Drawing.Size(209, 329)
            Me.VariablesLB.TabIndex = 13
            '
            'MainTC
            '
            Me.MainTC.Controls.Add(Me.VariablesTP)
            Me.MainTC.Controls.Add(Me.RulesTP)
            Me.MainTC.Controls.Add(Me.MessagesTP)
            Me.MainTC.Location = New System.Drawing.Point(15, 32)
            Me.MainTC.Name = "MainTC"
            Me.MainTC.SelectedIndex = 0
            Me.MainTC.Size = New System.Drawing.Size(717, 409)
            Me.MainTC.TabIndex = 12
            '
            'MessagesTP
            '
            Me.MessagesTP.Controls.Add(Me.MessagesTB)
            Me.MessagesTP.Location = New System.Drawing.Point(4, 22)
            Me.MessagesTP.Name = "MessagesTP"
            Me.MessagesTP.Padding = New System.Windows.Forms.Padding(3)
            Me.MessagesTP.Size = New System.Drawing.Size(709, 383)
            Me.MessagesTP.TabIndex = 2
            Me.MessagesTP.Text = "Messages"
            Me.MessagesTP.UseVisualStyleBackColor = True
            '
            'MessagesTB
            '
            Me.MessagesTB.BorderStyle = System.Windows.Forms.BorderStyle.None
            Me.MessagesTB.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.MessagesTB.Location = New System.Drawing.Point(6, 6)
            Me.MessagesTB.Multiline = True
            Me.MessagesTB.Name = "MessagesTB"
            Me.MessagesTB.ReadOnly = True
            Me.MessagesTB.Size = New System.Drawing.Size(697, 374)
            Me.MessagesTB.TabIndex = 7
            '
            'ParserErrorForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(742, 477)
            Me.Controls.Add(Me.MainTC)
            Me.Controls.Add(Me.CancelButton)
            Me.Controls.Add(Me.OkButton)
            Me.Controls.Add(Me.TryButton)
            Me.Controls.Add(Label4)
            Me.Controls.Add(Label3)
            Me.Controls.Add(Me.ConditionTB)
            Me.Controls.Add(Label1)
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(750, 504)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(750, 504)
            Me.Name = "ParserErrorForm"
            Me.Text = "Condition editor"
            Me.RulesTP.ResumeLayout(False)
            Me.RulesTP.PerformLayout()
            Me.VariablesTP.ResumeLayout(False)
            Me.VariablesTP.PerformLayout()
            Me.MainTC.ResumeLayout(False)
            Me.MessagesTP.ResumeLayout(False)
            Me.MessagesTP.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents ConditionTB As System.Windows.Forms.TextBox
        Friend WithEvents TryButton As System.Windows.Forms.Button
        Friend WithEvents OkButton As System.Windows.Forms.Button
        Friend WithEvents CancelButton As System.Windows.Forms.Button
        Friend WithEvents RulesTP As System.Windows.Forms.TabPage
        Friend WithEvents GrammarTB As System.Windows.Forms.TextBox
        Friend WithEvents VariablesTP As System.Windows.Forms.TabPage
        Friend WithEvents AddSrcButton As System.Windows.Forms.Button
        Friend WithEvents AddRatingButton As System.Windows.Forms.Button
        Friend WithEvents RatingSrcLB As System.Windows.Forms.ListBox
        Friend WithEvents RatingsLB As System.Windows.Forms.ListBox
        Friend WithEvents AddVarButton As System.Windows.Forms.Button
        Friend WithEvents VariablesLB As System.Windows.Forms.ListBox
        Friend WithEvents MainTC As System.Windows.Forms.TabControl
        Friend WithEvents MessagesTP As System.Windows.Forms.TabPage
        Friend WithEvents MessagesTB As System.Windows.Forms.TextBox
        Friend WithEvents MoodysRB As System.Windows.Forms.RadioButton
        Friend WithEvents SpFitchRB As System.Windows.Forms.RadioButton
        Friend WithEvents DateTimePckr As System.Windows.Forms.DateTimePicker
        Friend WithEvents AddDateButton As System.Windows.Forms.Button
    End Class
End Namespace