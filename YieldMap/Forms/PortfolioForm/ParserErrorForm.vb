Imports DbManager.Bonds

Namespace Forms.PortfolioForm
    Public Class ParserErrorForm

        Private Sub ParserErrorForm_Load(ByVal sender As System.Object, ByVal e As EventArgs) Handles MyBase.Load
            Dim fields As List(Of String) = FilterHelper.GetFilterableFields(Of BondMetadata)()
            VariablesLB.DataSource = fields
            ConditionTB.Select()

            RatingsLB.DataSource = Rating.AllRatings(If(SpFitchRB.Checked, RatingSource.SnP, RatingSource.Moodys))
            RatingSrcLB.DataSource = RatingSource.RateSources
        End Sub

        Private Sub AddVar(ByVal sender As Object, ByVal e As EventArgs) Handles VariablesLB.DoubleClick, AddVarButton.Click
            If VariablesLB.SelectedIndex < 0 Then Return
            InsertText(VariablesLB.SelectedItem.ToString(), "${0} ")

            ' todo support empty rating []
        End Sub

        Private Sub AddRate(ByVal sender As Object, ByVal e As EventArgs) Handles RatingsLB.DoubleClick, AddRatingButton.Click
            If RatingsLB.SelectedIndex < 0 Then Return
            InsertText(RatingsLB.SelectedItem.ToString(), "[{0}] ")
        End Sub

        Private Sub AddRateSrc(ByVal sender As Object, ByVal e As EventArgs) Handles RatingSrcLB.DoubleClick, AddSrcButton.Click
            If RatingSrcLB.SelectedIndex < 0 Then Return
            InsertText(RatingSrcLB.SelectedItem.ToString(), """{0}"" ")
        End Sub

        Private Sub InsertText(ByVal var As String, ByVal fmt As String)
            Dim value = String.Format(fmt, var)
            Dim newPosition = ConditionTB.SelectionStart + value.Length
            ConditionTB.Text = ConditionTB.Text.Insert(ConditionTB.SelectionStart, value)
            ConditionTB.Select()
            ConditionTB.SelectionStart = newPosition
            ConditionTB.SelectionLength = 0
        End Sub

        Private Sub SpFitchRB_CheckedChanged(ByVal sender As System.Object, ByVal e As EventArgs) Handles SpFitchRB.CheckedChanged
            RatingsLB.DataSource = Rating.AllRatings(If(SpFitchRB.Checked, RatingSource.SnP, RatingSource.Moodys))
        End Sub

        Private Sub TryButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TryButton.Click
            Dim fp As New FilterParser
            Try
                Dim grammar = fp.SetFilter(ConditionTB.Text)
                MessagesTB.ForeColor = Color.DarkGreen
                MessagesTB.Text = "Parsed successfully"
                Dim x As New FilterInterpreter(Of BondMetadata)
                x.SetGrammar(grammar)
                Try
                    ' todo some mock bond class
                    x.Allows(New BondMetadata("X", "X", "X", Date.Today, 10, "X", "X", Date.Today, "X", "X", "X", "X", "X",
                             "X", "X", True, True, True, New RatingDescr(Rating.Other, Nothing, Nothing),
                             New RatingDescr(Rating.Other, Nothing, Nothing),
                             New RatingDescr(Rating.Other, Nothing, Nothing), "x", "x", "x"))
                Catch ex As InterpreterException
                    MessagesTB.ForeColor = Color.DarkRed
                    MessagesTB.Text = "Failed to interpret " + Environment.NewLine + ex.Message
                End Try
            Catch ex As ParserException
                MessagesTB.ForeColor = Color.DarkRed
                MessagesTB.Text = ex.ToString()
                ConditionTB.Select(ex.ErrorPos, 1)
            Catch ex As Exception
                MessagesTB.ForeColor = Color.DarkRed
                MessagesTB.Text = String.Format("Some unexpected problem occured, message is {0}", ex.Message)
            Finally
                MainTC.SelectedTab = MessagesTP
            End Try
        End Sub

        Private Sub AddDateButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AddDateButton.Click
            InsertText(DateTimePckr.Value.Date.ToString("#{0:dd/MM/yyyy}#").Replace(".", "\"), "{0} ")
        End Sub
    End Class
End Namespace