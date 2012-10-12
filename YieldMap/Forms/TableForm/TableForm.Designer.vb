Namespace Forms.TableForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class TableForm
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
            Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
            Me.TheTS = New System.Windows.Forms.ToolStrip()
            Me.CopyTSB = New System.Windows.Forms.ToolStripButton()
            Me.TheGrid = New System.Windows.Forms.DataGridView()
            Me.ColumnRIC = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnName = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnMaturity = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnCoupon = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnYield = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnDuration = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnConvexity = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnYieldType = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnPrice = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnQuote = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnState = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnLiveHist = New System.Windows.Forms.DataGridViewCheckBoxColumn()
            Me.ColumnDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.ColumnCalcMode = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.TableLayoutPanel1.SuspendLayout()
            Me.TheTS.SuspendLayout()
            CType(Me.TheGrid, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'TableLayoutPanel1
            '
            Me.TableLayoutPanel1.ColumnCount = 1
            Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel1.Controls.Add(Me.TheGrid, 0, 1)
            Me.TableLayoutPanel1.Controls.Add(Me.TheTS, 0, 0)
            Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
            Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
            Me.TableLayoutPanel1.RowCount = 2
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TableLayoutPanel1.Size = New System.Drawing.Size(915, 490)
            Me.TableLayoutPanel1.TabIndex = 3
            '
            'TheTS
            '
            Me.TheTS.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyTSB})
            Me.TheTS.Location = New System.Drawing.Point(0, 0)
            Me.TheTS.Name = "TheTS"
            Me.TheTS.Size = New System.Drawing.Size(915, 25)
            Me.TheTS.TabIndex = 3
            Me.TheTS.Text = "ToolStrip1"
            '
            'CopyTSB
            '
            Me.CopyTSB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.CopyTSB.Image = Global.YieldMap.My.Resources.Resources.clipboard
            Me.CopyTSB.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.CopyTSB.Name = "CopyTSB"
            Me.CopyTSB.Size = New System.Drawing.Size(23, 22)
            Me.CopyTSB.Text = "Copy to clipboard"
            '
            'TheGrid
            '
            Me.TheGrid.AllowUserToAddRows = False
            Me.TheGrid.AllowUserToDeleteRows = False
            Me.TheGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
            Me.TheGrid.CausesValidation = False
            Me.TheGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.TheGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColumnRIC, Me.ColumnName, Me.ColumnMaturity, Me.ColumnCoupon, Me.ColumnYield, Me.ColumnDuration, Me.ColumnConvexity, Me.ColumnYieldType, Me.ColumnPrice, Me.ColumnQuote, Me.ColumnState, Me.ColumnLiveHist, Me.ColumnDate, Me.ColumnCalcMode})
            Me.TheGrid.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TheGrid.Location = New System.Drawing.Point(3, 28)
            Me.TheGrid.Name = "TheGrid"
            Me.TheGrid.ReadOnly = True
            Me.TheGrid.RowHeadersVisible = False
            Me.TheGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.TheGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.TheGrid.Size = New System.Drawing.Size(909, 459)
            Me.TheGrid.TabIndex = 4
            '
            'ColumnRIC
            '
            Me.ColumnRIC.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            DataGridViewCellStyle1.Format = "N2"
            Me.ColumnRIC.DefaultCellStyle = DataGridViewCellStyle1
            Me.ColumnRIC.HeaderText = "RIC"
            Me.ColumnRIC.Name = "ColumnRIC"
            Me.ColumnRIC.ReadOnly = True
            Me.ColumnRIC.Width = 50
            '
            'ColumnName
            '
            Me.ColumnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.ColumnName.HeaderText = "Name"
            Me.ColumnName.Name = "ColumnName"
            Me.ColumnName.ReadOnly = True
            '
            'ColumnMaturity
            '
            Me.ColumnMaturity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            DataGridViewCellStyle2.Format = "d"
            Me.ColumnMaturity.DefaultCellStyle = DataGridViewCellStyle2
            Me.ColumnMaturity.HeaderText = "Maturity"
            Me.ColumnMaturity.Name = "ColumnMaturity"
            Me.ColumnMaturity.ReadOnly = True
            Me.ColumnMaturity.Width = 69
            '
            'ColumnCoupon
            '
            DataGridViewCellStyle3.Format = "N2"
            Me.ColumnCoupon.DefaultCellStyle = DataGridViewCellStyle3
            Me.ColumnCoupon.HeaderText = "Coupon"
            Me.ColumnCoupon.Name = "ColumnCoupon"
            Me.ColumnCoupon.ReadOnly = True
            Me.ColumnCoupon.Width = 69
            '
            'ColumnYield
            '
            Me.ColumnYield.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            DataGridViewCellStyle4.Format = "P2"
            Me.ColumnYield.DefaultCellStyle = DataGridViewCellStyle4
            Me.ColumnYield.HeaderText = "Yield"
            Me.ColumnYield.Name = "ColumnYield"
            Me.ColumnYield.ReadOnly = True
            Me.ColumnYield.Width = 55
            '
            'ColumnDuration
            '
            Me.ColumnDuration.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            DataGridViewCellStyle5.Format = "N2"
            Me.ColumnDuration.DefaultCellStyle = DataGridViewCellStyle5
            Me.ColumnDuration.HeaderText = "Duration"
            Me.ColumnDuration.Name = "ColumnDuration"
            Me.ColumnDuration.ReadOnly = True
            Me.ColumnDuration.Width = 72
            '
            'ColumnConvexity
            '
            Me.ColumnConvexity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            DataGridViewCellStyle6.Format = "N2"
            Me.ColumnConvexity.DefaultCellStyle = DataGridViewCellStyle6
            Me.ColumnConvexity.HeaderText = "Convexity"
            Me.ColumnConvexity.Name = "ColumnConvexity"
            Me.ColumnConvexity.ReadOnly = True
            Me.ColumnConvexity.Width = 78
            '
            'ColumnYieldType
            '
            Me.ColumnYieldType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.ColumnYieldType.HeaderText = "Yield type"
            Me.ColumnYieldType.Name = "ColumnYieldType"
            Me.ColumnYieldType.ReadOnly = True
            Me.ColumnYieldType.Width = 78
            '
            'ColumnPrice
            '
            Me.ColumnPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            DataGridViewCellStyle7.Format = "N2"
            Me.ColumnPrice.DefaultCellStyle = DataGridViewCellStyle7
            Me.ColumnPrice.HeaderText = "Price"
            Me.ColumnPrice.Name = "ColumnPrice"
            Me.ColumnPrice.ReadOnly = True
            Me.ColumnPrice.Width = 56
            '
            'ColumnQuote
            '
            Me.ColumnQuote.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.ColumnQuote.HeaderText = "Quote"
            Me.ColumnQuote.Name = "ColumnQuote"
            Me.ColumnQuote.ReadOnly = True
            Me.ColumnQuote.Width = 61
            '
            'ColumnState
            '
            Me.ColumnState.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.ColumnState.HeaderText = "State"
            Me.ColumnState.Name = "ColumnState"
            Me.ColumnState.ReadOnly = True
            Me.ColumnState.Width = 57
            '
            'ColumnLiveHist
            '
            Me.ColumnLiveHist.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.ColumnLiveHist.HeaderText = "Live / history"
            Me.ColumnLiveHist.Name = "ColumnLiveHist"
            Me.ColumnLiveHist.ReadOnly = True
            Me.ColumnLiveHist.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
            Me.ColumnLiveHist.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
            Me.ColumnLiveHist.Visible = False
            Me.ColumnLiveHist.Width = 93
            '
            'ColumnDate
            '
            Me.ColumnDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            DataGridViewCellStyle8.Format = "d"
            Me.ColumnDate.DefaultCellStyle = DataGridViewCellStyle8
            Me.ColumnDate.HeaderText = "Date"
            Me.ColumnDate.Name = "ColumnDate"
            Me.ColumnDate.ReadOnly = True
            Me.ColumnDate.Width = 55
            '
            'ColumnCalcMode
            '
            Me.ColumnCalcMode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
            Me.ColumnCalcMode.HeaderText = "Calc mode"
            Me.ColumnCalcMode.Name = "ColumnCalcMode"
            Me.ColumnCalcMode.ReadOnly = True
            Me.ColumnCalcMode.Width = 82
            '
            'TableForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(915, 490)
            Me.Controls.Add(Me.TableLayoutPanel1)
            Me.Name = "TableForm"
            Me.Text = "Table view"
            Me.TableLayoutPanel1.ResumeLayout(False)
            Me.TableLayoutPanel1.PerformLayout()
            Me.TheTS.ResumeLayout(False)
            Me.TheTS.PerformLayout()
            CType(Me.TheGrid, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents TheGrid As System.Windows.Forms.DataGridView
        Friend WithEvents TheTS As System.Windows.Forms.ToolStrip
        Friend WithEvents CopyTSB As System.Windows.Forms.ToolStripButton
        Friend WithEvents ColumnRIC As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnName As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnMaturity As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnCoupon As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnYield As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnDuration As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnConvexity As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnYieldType As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnPrice As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnQuote As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnState As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnLiveHist As System.Windows.Forms.DataGridViewCheckBoxColumn
        Friend WithEvents ColumnDate As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents ColumnCalcMode As System.Windows.Forms.DataGridViewTextBoxColumn
    End Class
End Namespace