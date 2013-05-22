Imports System.Windows.Forms
Imports YieldMap.Tools.Elements
Imports NLog

Namespace Forms.TableForm
    Public Class TableForm
        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(TableForm))
        Private _bonds As New List(Of BondDescr)

        Public Property Bonds As List(Of BondDescr)
            Get
                Return _bonds
            End Get
            Set(value As List(Of BondDescr))
                _bonds = value

                TheGrid.DataSource = Nothing

                TheGrid.AutoGenerateColumns = False
                TheGrid.AllowUserToAddRows = True
                TheGrid.AllowUserToDeleteRows = True

                TheGrid.DataSource = Bonds
                ColumnRIC.DataPropertyName = "RIC"
                ColumnName.DataPropertyName = "Name"
                ColumnYield.DataPropertyName = "BondYield"
                ColumnMaturity.DataPropertyName = "Maturity"
                ColumnDuration.DataPropertyName = "Duration"
                ColumnConvexity.DataPropertyName = "Convexity"
                ColumnYieldType.DataPropertyName = "ToWhat"
                ColumnPrice.DataPropertyName = "Price"
                ColumnQuote.DataPropertyName = "Quote"
                ColumnState.DataPropertyName = "State"
                ColumnDate.DataPropertyName = "QuoteDate"
                ColumnCalcMode.DataPropertyName = "CalcMode"
                ColumnLiveHist.DataPropertyName = "Live"
                ColumnCoupon.DataPropertyName = "Coupon"

                ColumnState.Visible = False
                ColumnLiveHist.Visible = False
                ColumnCalcMode.Visible = False
            End Set
        End Property

        Private Sub CopyTSBClick(sender As System.Object, e As EventArgs) Handles CopyTSB.Click
            TheGrid.SelectAll()
            Clipboard.SetDataObject(TheGrid.GetClipboardContent())
            TheGrid.ClearSelection()
        End Sub

        Public Sub OnPointUpdated(ByVal ric As String, ByVal [yield] As Double, ByVal duration As Double, lastPrice As Double)
            _bonds.ForEach(
                Sub(bond)
                    If bond.RIC = ric Then
                        bond.BondYield = [yield]
                        bond.Duration = duration
                        bond.Price = lastPrice
                    End If
                End Sub)
        End Sub

        Private Sub TheGridColusmnHeaderMouseClick(sender As System.Object, e As DataGridViewCellMouseEventArgs) Handles TheGrid.ColumnHeaderMouseClick
            Logger.Debug("TheGrid_ColumnHeaderMouseClick()")
            Dim fieldName = TheGrid.Columns(e.ColumnIndex).DataPropertyName
            Dim sortOrder = GetSortOrder(e.ColumnIndex)
            _bonds.Sort(New BondDescrComparer(fieldName, sortOrder))

            Bonds = _bonds
            For Each col As DataGridViewColumn In TheGrid.Columns
                col.HeaderCell.SortGlyphDirection = sortOrder.None
            Next
            TheGrid.Columns(e.ColumnIndex).HeaderCell.SortGlyphDirection = sortOrder
        End Sub

        Private Function GetSortOrder(ByVal columnIndex As Integer) As SortOrder
            Dim dir = TheGrid.Columns(columnIndex).HeaderCell.SortGlyphDirection
            Return IIf(dir = SortOrder.None Or dir = SortOrder.Descending, SortOrder.Ascending, SortOrder.Descending)
        End Function
    End Class
End Namespace