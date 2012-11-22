Imports YieldMap.Commons

Namespace Forms.PortfolioForm
    Class ProgressForm
        Inherits Form

        Private ReadOnly _count As Integer
        Private _progress As Integer
        Private ReadOnly _msg As New Label
        Private ReadOnly _adding As Boolean

        Public Sub New(count As Integer, Optional adding As Boolean = True)
            _count = count
            _progress = 0
            _adding = adding

            Height = 100
            Width = 300

            FormBorderStyle = FormBorderStyle.FixedDialog
            StartPosition = FormStartPosition.CenterScreen
            Text = String.Format("{0} bonds {1} user-defined list", If(_adding, "Adding", "Removing"), If(_adding, "into", "from"))

            Controls.Add(_msg)
            _msg.Location = New Point(20, 20)
            _msg.AutoSize = True

            AddHandler FormClosing, Sub(sndr As Object, args As FormClosingEventArgs)
                                        If _progress < _count - 1 Then args.Cancel = True
                                    End Sub
        End Sub

        Public Sub OnItem(ByVal ric As String, ByVal stp As Integer)
            _progress = stp
            GuiAsync(Sub()
                         _msg.Text = String.Format("Progress {0:P2}, {1} {2}", (stp / _count), If(_adding, "adding", "removing"), ric)
                         Refresh()
                     End Sub)
        End Sub
    End Class
End Namespace