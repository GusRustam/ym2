Imports System.Data
Imports System.Windows.Forms
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Drawing
Imports AdfinXAnalyticsFunctions
Imports System.ComponentModel
Imports YieldMap.Curves
Imports YieldMap.My.Resources
Imports YieldMap.Commons
Imports YieldMap.Tools
Imports YieldMap.BondsDataSetTableAdapters
Imports NLog

Namespace Forms.ChartForm
    Public Class GraphForm
        Private Shared ReadOnly Logger As Logger = GetLogger(GetType(GraphForm))
        Private WithEvents _tableForm As TableForm.TableForm = New TableForm.TableForm()

        'Private ReadOnly _historicalCurves As New HistoricalCurvesContainer(AddressOf OnHistoricalCurveData, AddressOf OnCurveRemoved)
        Private ReadOnly _moneyMarketCurves As New List(Of SwapCurve)

        Private WithEvents _spreadBenchmarks As New SpreadContainer
        Private WithEvents _ansamble As New Ansamble(_spreadBenchmarks)

        Public Delegate Sub PointUpdateDelegate(ByVal ric As String, ByVal yield As Double, ByVal duration As Double, ByVal lastPrice As Double)
        Public Event PointUpdated As PointUpdateDelegate

#Region "I) Dependent forms"
        Private Sub TableFormShown(sender As Object, e As EventArgs) Handles _tableForm.Shown
            AddHandler PointUpdated, AddressOf _tableForm.OnPointUpdated
        End Sub

        Private Sub TableFormFormClosing(sender As Object, e As FormClosingEventArgs) Handles _tableForm.FormClosing
            RemoveHandler PointUpdated, AddressOf _tableForm.OnPointUpdated
        End Sub
#End Region

#Region "II) Form state manipulation"
        Private Enum FormDataStatus
            Stopped
            Loading
            Running
        End Enum

        Private _thisFormStatus As FormDataStatus = FormDataStatus.Stopped
        Private Property ThisFormStatus As FormDataStatus
            Get
                Return _thisFormStatus
            End Get
            Set(ByVal value As FormDataStatus)
                Logger.Debug("ThisFormStatus.Set({0})", value)
                _thisFormStatus = value
                Select Case value
                    Case FormDataStatus.Loading
                        StatusMessage.Text = "Loading"
                    Case FormDataStatus.Running
                        StatusMessage.Text = ""
                    Case FormDataStatus.Stopped
                        _ansamble.Cleanup()

                        '_historicalCurves.Clear()

                        StatusMessage.Text = "Stopped"
                End Select
            End Set
        End Property

        Private WriteOnly Property ThisFormDataSource As Integer
            Set(ByVal value As Integer)
                Logger.Trace("ThisFormDataSource to id {0}", value)

                Dim currentPortID As Long
                If value < 0 Then
                    ThisFormStatus = FormDataStatus.Running
                    Return
                ElseIf value = 0 Then
                    currentPortID = (New portfolioTableAdapter).GetDefaultPortfolios.First.id
                Else
                    currentPortID = value
                End If

                Dim portfolioSources = (New PortfolioSourcesTableAdapter).GetData()
                Dim portfolioUnitedDataTable = (New PortfolioUnitedTableAdapter).GetData()
                For Each port In (From p In portfolioSources Where p.portfolioID = currentPortID Select p)
                    Dim groupType As GroupType
                    Dim group As Group
                    If groupType.TryParse(port.whereFrom, groupType) Then
                        Dim fieldSetId = port.id_field_set
                        Dim fields = (New field_layoutTableAdapter).GetData().Where(Function(row) row.field_set_id = fieldSetId)
                        Dim realTime = fields.First(Function(row) row.is_realtime = 1)
                        Dim history = fields.First(Function(row) row.is_realtime = 0)
                        group = New Group(_ansamble) With {
                            .Group = groupType,
                            .SeriesName = port.whereFromDescr,
                            .PortfolioID = port.fromID,
                            .BidField = realTime.bid_field,
                            .AskField = realTime.ask_field,
                            .LastField = realTime.last_field,
                            .HistField = history.last_field,
                            .Brokers = {"MM", ""}.ToList(),
                            .Currency = "",
                            .Color = port.color
                        }

                        Dim pD = From elem In portfolioUnitedDataTable Where elem.pid = currentPortID And elem.fromID = group.PortfolioID
                        Dim ars = (From row In pD Where row.include Select row.ric).ToList
                        Dim rrs = (From row In pD Where Not row.include Select row.ric).ToList
                        ars.RemoveAll(Function(ric) rrs.Contains(ric))
                        ars.ForEach(
                            Sub(ric)
                                Dim descr = DbInitializer.GetBondInfo(ric)
                                If descr IsNot Nothing Then
                                    group.AddElement(ric, descr)
                                Else
                                    Logger.Error("No description for bond {0} found", ric)
                                End If
                            End Sub)
                        _ansamble.AddGroup(group)
                    Else
                        Logger.Error("Failed to parse {0}", port.whereFrom)
                    End If
                Next
                _ansamble.StartLoadingLiveData()
            End Set
        End Property

#End Region

#Region "III) Event handling"
#Region "a) Form events"
        Private Sub GraphFormLoad(sender As Object, e As EventArgs) Handles MyBase.Load
            Logger.Trace("GraphFormLoad")
            ThisFormStatus = FormDataStatus.Loading

            ItemDescriptionPanel.Visible = ShowChartToolBar
            If ItemDescriptionPanel.Visible Then
                PinUnpinTSB.Image = Pin
                PinUnpinTSB.ToolTipText = ShowDescriptionPane
            Else
                PinUnpinTSB.Image = UnPin
                PinUnpinTSB.ToolTipText = HideDescriptionPane
            End If

            ZoomCustomButton.CheckOnClick = True
            InitChart()

            ThisFormDataSource = -1
        End Sub

        Private Sub GraphFormFormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
            Logger.Trace("GraphForm_FormClosing")
            _ansamble.Cleanup()
            _moneyMarketCurves.ForEach(Sub(curve) curve.Cleanup())
            _moneyMarketCurves.Clear()
            ThisFormStatus = FormDataStatus.Stopped
        End Sub

        Private Sub GraphFormSizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
            InfoLabel.Left = (MainPanel.ClientSize.Width - InfoLabel.Width) / 2
            InfoLabel.Top = (MainPanel.ClientSize.Height - InfoLabel.Height) / 2
        End Sub

        Private Sub GraphFormClick(sender As Object, e As EventArgs) Handles MainPanel.Click
            Dim mouseEvent As MouseEventArgs = e
            If mouseEvent.X >= TheChart.Left And mouseEvent.X <= TheChart.Right And
                mouseEvent.Y >= TheChart.Top And mouseEvent.X <= TheChart.Bottom And
                mouseEvent.Button = MouseButtons.Right Then

                ChartCMS.Show(MainPanel, mouseEvent.Location)
            End If
        End Sub

#End Region

#Region "b) Chart events"
        ' The chart
        Private Sub TheChartClick(sender As Object, e As EventArgs) Handles TheChart.Click
            Logger.Trace("TheChartClick")
            Dim mouseEvent As MouseEventArgs = e
            Dim htr As HitTestResult = TheChart.HitTest(mouseEvent.X, mouseEvent.Y)
            If mouseEvent.Button = MouseButtons.Right Then
                Try
                    If htr.ChartElementType = ChartElementType.DataPoint Then
                        Dim point As DataPoint = CType(htr.Object, DataPoint)
                        If TypeOf point.Tag Is Bond Then
                            Dim bondDataPoint = CType(point.Tag, Bond)
                            With BondCMS
                                .Tag = bondDataPoint.MetaData.RIC
                                .Show(TheChart, mouseEvent.Location)
                            End With
                            MainInfoLine1TSMI.Text = point.ToolTip

                            ExtInfoTSMI.DropDownItems.Clear()
                            bondDataPoint.QuotesAndYields.Keys.ToList.ForEach(
                                Sub(key)
                                    Dim x = bondDataPoint.QuotesAndYields(key)
                                    ExtInfoTSMI.DropDownItems.Add(String.Format("{0}: {1:F4}, {2:P2} {3}, {4:F2}", key, x.Price, x.Yld.Yield, x.Yld.ToWhat.Abbr, x.Duration))
                                End Sub)
                            ExtInfoTSMI.DropDownItems.Add("-")
                            ExtInfoTSMI.DropDownItems.Add(String.Format("Today volume: {0:N0}", bondDataPoint.TodayVolume))


                        ElseIf TypeOf point.Tag Is SwapCurve Then
                            Dim curve = CType(point.Tag, SwapCurve)
                            MMNameTSMI.Text = curve.GetFullName()
                            MoneyCurveCMS.Show(TheChart, mouseEvent.Location)
                            MoneyCurveCMS.Tag = curve.GetName()
                            ShowCurveParameters(curve)

                            'ElseIf TypeOf point.Tag Is HistCurvePointDescr Then
                            '    Dim histDataPoint = CType(point.Tag, HistCurvePointDescr)
                            '    HistoryCMS.Tag = histDataPoint.RIC
                            '    HistoryCMS.Show(TheChart, mouseEvent.Location)
                        End If
                    ElseIf htr.ChartElementType = ChartElementType.PlottingArea Or htr.ChartElementType = ChartElementType.Gridlines Then
                        ChartCMS.Show(TheChart, mouseEvent.Location)
                    ElseIf htr.ChartElementType = ChartElementType.LegendItem Then
                        Dim item = CType(htr.Object, LegendItem)
                        BondSetCMS.Tag = item.Tag
                        BondSetCMS.Show(TheChart, mouseEvent.Location)
                    End If
                Catch ex As Exception
                    Logger.WarnException("Something went wrong", ex)
                    Logger.Warn("Exception = {0}", ex.ToString())
                End Try
            ElseIf mouseEvent.Button = MouseButtons.Left AndAlso htr.ChartElementType = ChartElementType.AxisTitle AndAlso htr.Axis.Equals(TheChart.ChartAreas(0).AxisY) Then
                ShowYAxisCMS(mouseEvent.Location)
            End If
        End Sub

        Private Sub TheChartInvalidated(sender As Object, e As InvalidateEventArgs) Handles TheChart.Invalidated
            If TheChart.Series IsNot Nothing AndAlso TheChart.Series.Count = 0 AndAlso Not MainTableLayout.Controls.ContainsKey("InfoLabel") Then
                TheChart.Visible = False
                InfoLabel.Visible = True
            Else
                InfoLabel.Visible = False
                TheChart.Visible = True
            End If
        End Sub

        Private Sub TheChartMouseMove(sender As Object, e As MouseEventArgs) Handles TheChart.MouseMove
            Dim mouseEvent As MouseEventArgs = e
            Dim hasShown = False
            Try
                Dim htr As HitTestResult = TheChart.HitTest(mouseEvent.X, mouseEvent.Y)
                If (htr IsNot Nothing) AndAlso htr.ChartElementType = ChartElementType.DataPoint Then
                    hasShown = True
                    Dim point As DataPoint = CType(htr.Object, DataPoint)

                    If TypeOf point.Tag Is Bond And Not point.IsEmpty Then
                        Dim bondData = CType(point.Tag, Bond)
                        DscrLabel.Text = bondData.MetaData.ShortName
                        Dim calculatedYield = bondData.QuotesAndYields(bondData.SelectedQuote)

                        SpreadLabel.Text = If(calculatedYield.PointSpread IsNot Nothing, String.Format("{0:F0} b.p.", calculatedYield.PointSpread), N_A)
                        ZSpreadLabel.Text = If(calculatedYield.ZSpread IsNot Nothing, String.Format("{0:F0} b.p.", calculatedYield.ZSpread), N_A)
                        ASWLabel.Text = If(calculatedYield.ASWSpread IsNot Nothing, String.Format("{0:F0} b.p.", calculatedYield.ASWSpread), N_A)
                        YldLabel.Text = String.Format("{0:P2}", calculatedYield.Yld)
                        DurLabel.Text = String.Format("{0:F2}", point.XValue)

                        ConvLabel.Text = String.Format("{0:F2}", calculatedYield.Convexity)
                        YldLabel.Text = String.Format("{0:P2} {1}", calculatedYield.Yld.Yield, calculatedYield.Yld.ToWhat.Abbr)
                        DatLabel.Text = String.Format("{0:dd/MM/yyyy}", calculatedYield.YieldAtDate)
                        MatLabel.Text = String.Format("{0:dd/MM/yyyy}", bondData.MetaData.Maturity)
                        CpnLabel.Text = String.Format("{0:F2}%", bondData.MetaData.Coupon)
                        PVBPLabel.Text = String.Format("{0:F4}", calculatedYield.PVBP)
                        CType(htr.Series.Tag, BondSetSeries).SelectedPointIndex = htr.PointIndex

                        PlotBidAsk(bondData)

                    ElseIf TypeOf point.Tag Is SwapCurve Then
                        Dim curve = CType(point.Tag, SwapCurve)

                        DscrLabel.Text = curve.GetFullName()
                        DatLabel.Text = String.Format("{0:dd/MM/yyyy}", curve.GetDate())
                        Dim period = String.Format("{0:F0}D", 365 * point.XValue)
                        Dim aDate = (New AdxDateModule).DfAddPeriod("RUS", Date.Today, period, "")
                        MatLabel.Text = String.Format("{0:dd/MM/yyyy}", FromExcelSerialDate(aDate.GetValue(1, 1)))

                        Select Case _spreadBenchmarks.CurrentType
                            Case SpreadType.Yield : YldLabel.Text = String.Format("{0:P2}", point.YValues(0))
                            Case SpreadType.ZSpread : ZSpreadLabel.Text = String.Format("{0:F0} b.p.", point.YValues(0))
                            Case SpreadType.PointSpread : SpreadLabel.Text = String.Format("{0:F0} b.p.", point.YValues(0))
                            Case SpreadType.ASWSpread : ASWLabel.Text = String.Format("{0:F0} b.p.", point.YValues(0))
                        End Select
                        DurLabel.Text = String.Format("{0:F2}", point.XValue)

                        DatLabel.Text = ""

                        'ElseIf TypeOf point.Tag Is HistCurvePointDescr Then
                        '    Dim historyDataPoint = CType(point.Tag, HistCurvePointDescr)
                        '    DscrLabel.Text = historyDataPoint.BondTag.ShortName
                        '    DatLabel.Text = String.Format("{0:dd/MM/yyyy}", historyDataPoint.YieldAtDate)
                        '    ConvLabel.Text = String.Format("{0:F2}", historyDataPoint.Convexity)

                    Else
                        hasShown = False
                    End If
                ElseIf (htr IsNot Nothing) AndAlso htr.ChartElementType = ChartElementType.AxisTitle Then
                    UpdateAxisYTitle(True)
                Else
                    UpdateAxisYTitle(False)
                    ResetPointSelection()
                    HideBidAsk()
                End If
            Catch ex As Exception
                Logger.WarnException("Got exception", ex)
                Logger.Warn("Exception = {0}", ex)
            Finally
                If Not hasShown Then
                    DscrLabel.Text = ""
                    ConvLabel.Text = ""
                    DatLabel.Text = ""
                    CpnLabel.Text = ""
                    PVBPLabel.Text = ""
                    YldLabel.Text = ""
                    DurLabel.Text = ""
                    MatLabel.Text = ""
                    ASWLabel.Text = If(_spreadBenchmarks.Benchmarks.ContainsKey(SpreadType.ASWSpread), " -> " + _spreadBenchmarks.Benchmarks(SpreadType.ASWSpread).GetFullName(), "")
                    SpreadLabel.Text = If(_spreadBenchmarks.Benchmarks.ContainsKey(SpreadType.PointSpread), " -> " + _spreadBenchmarks.Benchmarks(SpreadType.PointSpread).GetFullName(), "")
                    ZSpreadLabel.Text = If(_spreadBenchmarks.Benchmarks.ContainsKey(SpreadType.ZSpread), " -> " + _spreadBenchmarks.Benchmarks(SpreadType.ZSpread).GetFullName(), "")
                End If
            End Try
        End Sub

        Private Sub OnSelectedPointChanged(ByVal curveName As String, ByVal pointIndex As Integer?)
            TheChart.Series.ToList.ForEach(
                Sub(srs)
                    Dim seriesDescr = TryCast(srs.Tag, BondSetSeries)
                    If seriesDescr Is Nothing Then Return

                    seriesDescr.ResetSelection()
                    srs.Points.ToList.ForEach(Sub(point)
                                                  Dim tg = CType(point.Tag, Bond)
                                                  point.Color = If(tg.QuotesAndYields(tg.SelectedQuote).YieldSource = YieldSource.Historical, Color.LightGray, Color.White)
                                              End Sub)
                    If seriesDescr.Name = curveName AndAlso pointIndex IsNot Nothing Then
                        srs.Points(pointIndex).Color = Color.Red
                    End If
                End Sub)
        End Sub

        ' The chart resizing
        Private Sub ResizePictureBoxMouseDown(sender As Object, e As MouseEventArgs) Handles ResizePictureBox.MouseDown
            Logger.Trace("ResizePictureBoxMouseDown")
            If ZoomCustomButton.Checked Then
                _isResizing = True
                _resizeBasePoint = e.Location
                ResizePictureBox.Visible = True
                _resizeRectangle.Location = e.Location
                _resizeRectangle.Size = New Size(0, 0)
                StatusMessage.Text = "Started resizing"
            End If
        End Sub

        Private Sub ResizePictureBoxMouseMove(sender As Object, e As MouseEventArgs) Handles ResizePictureBox.MouseMove
            Logger.Trace("ResizePictureBoxMouseMove")
            If ZoomCustomButton.Checked And _isResizing Then
                Try
                    If e.Y < _resizeBasePoint.Y Then
                        _resizeRectangle.Y = e.Y
                        _resizeRectangle.Height = _resizeBasePoint.Y - e.Y
                    Else
                        _resizeRectangle.Y = _resizeBasePoint.Y
                        _resizeRectangle.Height = e.Y - _resizeBasePoint.Y
                    End If

                    If e.X < _resizeRectangle.Left Then
                        _resizeRectangle.X = e.X
                        _resizeRectangle.Width = _resizeBasePoint.X - e.X
                    Else
                        _resizeRectangle.X = _resizeBasePoint.X
                        _resizeRectangle.Width = e.X - _resizeBasePoint.X
                    End If

                    ResizePictureBox.Invalidate()
                    Try
                        With TheChart.ChartAreas.First
                            Dim xmin = Math.Round(.AxisX.PixelPositionToValue(_resizeRectangle.Left), 2)
                            Dim xmax = Math.Round(.AxisX.PixelPositionToValue(_resizeRectangle.Right), 2)
                            Dim ymin = Math.Round(.AxisY.PixelPositionToValue(_resizeRectangle.Top), 2)
                            Dim ymax = Math.Round(.AxisY.PixelPositionToValue(_resizeRectangle.Bottom), 2)
                            StatusMessage.Text = String.Format("Duration: {0} to {1}, Yield: {2} to {3}", xmin, xmax, ymin, ymax)
                        End With
                    Catch ex As Exception
                        StatusMessage.Text = String.Format("Error for ({0}, {1}) -> ({2},{3})", _resizeRectangle.Left, _resizeRectangle.Top, _resizeRectangle.Right, _resizeRectangle.Bottom)
                    End Try
                Catch ex As Exception
                    Logger.InfoException("Resize error", ex)
                    StopResize()
                    StatusMessage.Text = "Resized cancelled"
                End Try
            End If
        End Sub

        Private Sub ResizePictureBoxMouseLeave(sender As Object, e As EventArgs) Handles ResizePictureBox.MouseLeave
            Logger.Trace("ResizePictureBoxMouseLeave")
            If ZoomCustomButton.Checked And _isResizing Then
                StopResize()
                StatusMessage.Text = "Resize cancelled"
            End If
        End Sub

        Private Sub ResizePictureBoxMouseUp(sender As Object, e As MouseEventArgs) Handles ResizePictureBox.MouseUp
            Logger.Trace("ResizePictureBoxMouseUp")
            If ZoomCustomButton.Checked And _isResizing Then
                StopResize()
                Try
                    With TheChart.ChartAreas.First
                        Dim xAxisMin = .AxisX.PixelPositionToValue(_resizeRectangle.Left)
                        Dim xAxisMax = .AxisX.PixelPositionToValue(_resizeRectangle.Right)
                        Dim yAxisMin = .AxisY.PixelPositionToValue(_resizeRectangle.Bottom)
                        Dim yAxisMax = .AxisY.PixelPositionToValue(_resizeRectangle.Top)
                        Logger.Trace("({0},{1}) -> ({2},{3})", xAxisMin, yAxisMin, xAxisMax, yAxisMax)
                        SetZoomRange(xAxisMin, yAxisMin, xAxisMax, yAxisMax)
                    End With
                Catch ex As Exception
                    StatusMessage.Text = "Resize cancelled"
                    Logger.WarnException("Resize exception", ex)
                End Try
            End If
        End Sub

        Private Sub ResizePictureBoxPaint(sender As Object, e As PaintEventArgs) Handles ResizePictureBox.Paint
            Logger.Trace("ResizePictureBoxMouseUp")
            If ZoomCustomButton.Checked And _isResizing Then
                e.Graphics.DrawRectangle(New Pen(Color.Black), _resizeRectangle)
            End If
        End Sub
#End Region

#Region "c) Toolbar events"
        Private Sub ZoomAllButtonClick(sender As Object, e As EventArgs) Handles ZoomAllButton.Click
            SetChartMinMax()
            TheChart.ChartAreas(0).AxisX.ScaleView.ZoomReset()
            TheChart.ChartAreas(0).AxisY.ScaleView.ZoomReset()
        End Sub

        Private Sub ZoomCustomButtonClick(sender As Object, e As EventArgs) Handles ZoomCustomButton.Click
            Logger.Trace("ZoomCustomButtonClick")
            If Not TheChart.Visible Then
                ZoomCustomButton.Checked = False
                Return
            End If
            If ZoomCustomButton.Checked Then
                ResizePictureBox.Visible = True
                Dim bmp As New Bitmap(TheChart.Width, TheChart.Height)
                TheChart.DrawToBitmap(bmp, TheChart.ClientRectangle)
                With ResizePictureBox
                    .Image = bmp
                    .BringToFront()
                    .Parent = TheChart.Parent
                    .Location = TheChart.Location
                    .Size = TheChart.Size
                End With
                TheChart.Enabled = False
                TheChart.Visible = False
                ResizePictureBox.BringToFront()
            Else
                StopResize()
            End If
        End Sub

        Private Sub PortfolioTssbDropDownOpening(sender As Object, e As EventArgs) Handles PortfolioTSSB.DropDownOpening
            ' list of portfolios to show
            Dim portDescrList As List(Of IdName) =
                (From rw In (New portfolioTableAdapter).GetData() Select New IdName() With {.Id = rw("id"), .Name = rw("portfolio_name")}).ToList()

            PortfolioTSSB.DropDownItems.Clear()

            If portDescrList.Any Then
                portDescrList.ForEach(
                    Sub(idname)
                        Dim item = PortfolioTSSB.DropDownItems.Add(idname.Name, Nothing, AddressOf PortfolioSelectTSCBSelectedIndexChanged)
                        item.Tag = idname.Id
                    End Sub)
            End If
        End Sub

        Private Sub PortfolioSelectTSCBSelectedIndexChanged(sender As Object, e As EventArgs)
            Logger.Trace("PortfolioSelectTSCBSelectedIndexChanged")
            If ThisFormStatus <> FormDataStatus.Loading Then
                ThisFormStatus = FormDataStatus.Stopped
                Try
                    Dim portID = CLng(CType(sender, ToolStripMenuItem).Tag)
                    ThisFormDataSource = portID
                    PortfolioTSSB.HideDropDown()
                Catch ex As Exception
                    Logger.ErrorException("Failed to select a portfolio", ex)
                    Logger.Error("Exception = {0}", ex.ToString())
                End Try
            Else
                Logger.Info("Can not change selected portfolio while form is loading")
            End If
        End Sub

        Private Sub RubCCSTSMIClick(sender As Object, e As EventArgs) Handles RubCCSTSMI.Click
            Logger.Debug("RubCCSTSMIClick()")
            Dim rubCCS = New RubCCS(_spreadBenchmarks)

            AddHandler rubCCS.Cleared, AddressOf _spreadBenchmarks.OnCurveRemoved
            AddHandler rubCCS.Recalculated, AddressOf OnCurveRecalculated
            AddHandler rubCCS.Updated, AddressOf OnCurvePaint

            rubCCS.Subscribe()
            _moneyMarketCurves.Add(rubCCS)
        End Sub

        Private Sub RubIRSTSMIClick(sender As Object, e As EventArgs) Handles RubIRSTSMI.Click
            Logger.Debug("RubIRSTSMIClick()")
            Dim rubIRS = New RubIRS(_spreadBenchmarks)
            AddHandler rubIRS.Cleared, AddressOf _spreadBenchmarks.OnCurveRemoved
            AddHandler rubIRS.Recalculated, AddressOf OnCurveRecalculated
            AddHandler rubIRS.Updated, AddressOf OnCurvePaint

            rubIRS.Subscribe()
            _moneyMarketCurves.Add(rubIRS)
        End Sub

        Private Sub NDFTSMIClick(sender As Object, e As EventArgs) Handles NDFTSMI.Click
            Logger.Debug("NDFTSMI_Click()")
            Dim rubNDF = New RubNDF(_spreadBenchmarks)
            AddHandler rubNDF.Cleared, AddressOf _spreadBenchmarks.OnCurveRemoved
            AddHandler rubNDF.Recalculated, AddressOf OnCurveRecalculated
            AddHandler rubNDF.Updated, AddressOf OnCurvePaint

            rubNDF.Subscribe()
            _moneyMarketCurves.Add(rubNDF)
        End Sub

        Private Sub ShowLegendTSBClicked(sender As Object, e As EventArgs) Handles ShowLegendTSB.Click
            TheChart.Legends(0).Enabled = ShowLegendTSB.Checked
        End Sub

        Private Sub ShowLabelsTSBClick(sender As Object, e As EventArgs) Handles ShowLabelsTSB.Click
            Logger.Trace("ShowLabelsTSBClick")
            For Each series In From srs In TheChart.Series Where TypeOf srs.Tag Is BondSetSeries
                Dim points = series.Points
                For Each dataPoint In From pnt In points Where TypeOf pnt.Tag Is Bond Select {pnt, CType(pnt.Tag, Bond)}
                    dataPoint(0).Label = If(ShowLabelsTSB.Checked, dataPoint(1).Metadata.ShortName, "")
                Next
            Next
        End Sub

        Private Sub PinUnpinTSBClick(sender As Object, e As EventArgs) Handles PinUnpinTSB.Click
            If ItemDescriptionPanel.Visible Then
                ItemDescriptionPanel.Visible = False
                PinUnpinTSB.Image = Pin
                PinUnpinTSB.ToolTipText = ShowDescriptionPane
            Else
                ItemDescriptionPanel.Visible = True
                PinUnpinTSB.Image = UnPin
                PinUnpinTSB.ToolTipText = HideDescriptionPane
            End If
        End Sub

        Private Sub ChartCMSOpening(sender As Object, e As CancelEventArgs) Handles ChartCMS.Opening
            CopyToClipboardTSMI.Visible = TheChart.Visible
            ClipboardSeparator.Visible = TheChart.Visible
        End Sub
#End Region

#Region "d) Context menu events"
        ' Selection of y-axis type (yield / spread)
        Private Sub OnYAxisSelected(ByVal sender As Object, ByVal e As EventArgs)
            Logger.Info("OnYAxisSelected()")
            Dim item As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
            If item IsNot Nothing Then _spreadBenchmarks.CurrentType = SpreadType.FromString(item.Text)
        End Sub

        Private Sub CopyToClipboardTSMIClick(sender As Object, e As EventArgs) Handles CopyToClipboardTSMI.Click
            Dim bmp As New Bitmap(TheChart.Width, TheChart.Height)
            TheChart.DrawToBitmap(bmp, TheChart.ClientRectangle)
            Clipboard.SetImage(bmp)
        End Sub

        Private Sub OnYieldCurveSelected(ByVal sender As Object, ByVal eventArgs As EventArgs)
            Logger.Info("OnYieldCurveSelected()")
            Dim senderTSMI = TryCast(sender, ToolStripMenuItem)
            If senderTSMI Is Nothing Then Return

            If senderTSMI.Checked Then
                _spreadBenchmarks.AddType(SpreadType.FromString(SpreadCMS.Tag), senderTSMI.Tag)
            Else
                _spreadBenchmarks.RemoveType(SpreadType.FromString(SpreadCMS.Tag))
            End If

            UpdateAxisYTitle(False)

            ASWLabel.Text = If(_spreadBenchmarks.Benchmarks.ContainsKey(SpreadType.ASWSpread), " -> " + _spreadBenchmarks.Benchmarks(SpreadType.ASWSpread).GetFullName(), "")
            SpreadLabel.Text = If(_spreadBenchmarks.Benchmarks.ContainsKey(SpreadType.PointSpread), " -> " + _spreadBenchmarks.Benchmarks(SpreadType.PointSpread).GetFullName(), "")
            ZSpreadLabel.Text = If(_spreadBenchmarks.Benchmarks.ContainsKey(SpreadType.ZSpread), " -> " + _spreadBenchmarks.Benchmarks(SpreadType.ZSpread).GetFullName(), "")
        End Sub

        Private Sub RelatedQuoteTSMIClick(sender As Object, e As EventArgs) Handles RelatedQuoteTSMI.Click
            If _ansamble.ContainsRIC(BondCMS.Tag.ToString()) Then RunCommand("reuters://REALTIME/verb=FullQuote/ric=" + BondCMS.Tag.ToString())
        End Sub

        Private Sub BondDescriptionTSMIClick(sender As Object, e As EventArgs) Handles BondDescriptionTSMI.Click
            If _ansamble.ContainsRIC(BondCMS.Tag.ToString()) Then RunCommand("reuters://REALTIME/verb=BondData/ric=" + BondCMS.Tag.ToString())
        End Sub

        Private Sub RelatedChartTSMIClick(sender As Object, e As EventArgs) Handles RelatedChartTSMI.Click
            If _ansamble.ContainsRIC(BondCMS.Tag.ToString()) Then RunCommand("reuters://REALTIME/verb=RelatedGraph/ric=" + BondCMS.Tag.ToString())
        End Sub

        Private Sub ShowCurveItemsTSMIClick(sender As Object, e As EventArgs) Handles ShowCurveItemsTSMI.Click
            Dim aCurve = _moneyMarketCurves.First(Function(curve) curve.GetName() = CStr(MoneyCurveCMS.Tag)).GetSnapshot()

            Dim aForm = New Form With {
                .Text = "Curve items",
                .Width = 400,
                .Height = 400,
                .FormBorderStyle = FormBorderStyle.Sizable
            }

            Dim aTable = New DataGridView
            aTable.AutoGenerateColumns = False
            aTable.Columns.Add("RIC", "RIC")
            aTable.Columns.Add("Descr", "Descr")
            aTable.Columns.Add("Rate", "Rate")
            aTable.Columns.Add("Duration", "Duration")

            aTable.Columns("RIC").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            aTable.Columns("Descr").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            aTable.Columns("Rate").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            aTable.Columns("Rate").DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "P2"}
            aTable.Columns("Duration").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            aTable.Columns("Duration").DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "N2"}

            aCurve.ForEach(Sub(item) aTable.Rows.Add(New Object() {item.Item1, item.Item2, item.Item3, item.Item4}))

            aForm.Controls.Add(aTable)
            aTable.Dock = DockStyle.Fill

            aForm.ShowDialog()
        End Sub

        'Private Sub AsTableTSBClick(sender As Object, e As EventArgs) Handles AsTableTSB.Click
        '    Dim bondsToShow = _ansamble.Groups.SelectMany(
        '        Function(grp)
        '            Return grp.Elements.Keys.ToList.Select(
        '                Function(key)
        '                    Dim res As New BondDescr
        '                    Dim point = grp.Elements(key)
        '                    If point.Yld.ToWhat Is Nothing Then Return Nothing

        '                    res.RIC = point.RIC
        '                    res.Name = point.ShortName
        '                    res.Price = point.CalcPrice
        '                    res.Quote = IIf(point.YieldAtDate <> Date.Today, QuoteType.Close, QuoteType.Last)
        '                    res.QuoteDate = point.YieldAtDate
        '                    res.State = BondDescr.StateType.Ok
        '                    res.ToWhat = point.Yld.ToWhat
        '                    res.BondYield = point.Yld.Yield
        '                    res.CalcMode = BondDescr.CalculationMode.SystemPrice
        '                    res.Convexity = point.Convexity
        '                    res.Duration = point.Duration
        '                    res.Live = point.YieldAtDate = Date.Today
        '                    res.Maturity = point.Maturity
        '                    res.Coupon = point.Coupon
        '                    Return res
        '                End Function)
        '        End Function).Where(Function(elem) elem IsNot Nothing).ToList()
        '    _tableForm.Bonds = bondsToShow
        '    _tableForm.ShowDialog()
        'End Sub

        Private Sub LinkSpreadLabelLinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles SpreadLinkLabel.LinkClicked
            ShowCurveCMS("PointSpread",
                If(_spreadBenchmarks.Benchmarks.ContainsKey(SpreadType.PointSpread), _spreadBenchmarks.Benchmarks(SpreadType.PointSpread), Nothing))
        End Sub

        Private Sub ZSpreadLinkLabelLinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles ZSpreadLinkLabel.LinkClicked
            ShowCurveCMS("ZSpread",
                If(_spreadBenchmarks.Benchmarks.ContainsKey(SpreadType.ZSpread), _spreadBenchmarks.Benchmarks(SpreadType.ZSpread), Nothing))
        End Sub

        Private Sub ASWLinkLabelLinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles ASWLinkLabel.LinkClicked
            Dim refCurve = If(_spreadBenchmarks.Benchmarks.ContainsKey(SpreadType.ASWSpread), _spreadBenchmarks.Benchmarks(SpreadType.ASWSpread), Nothing)
            SpreadCMS.Items.Clear()
            SpreadCMS.Tag = "ASWSpread"
            If Not _moneyMarketCurves.Any() Then Return
            _moneyMarketCurves.Where(
                Function(crv)
                    Return TypeOf crv Is IAssetSwapBenchmark AndAlso CType(crv, IAssetSwapBenchmark).CanBeBenchmark()
                End Function
            ).Cast(Of SwapCurve).ToList().ForEach(
                Sub(item)
                    Dim elem = CType(SpreadCMS.Items.Add(item.GetFullName(), Nothing, AddressOf OnYieldCurveSelected), ToolStripMenuItem)
                    elem.CheckOnClick = True
                    elem.Checked = refCurve IsNot Nothing AndAlso item.GetFullName() = refCurve.GetFullName()
                    elem.Tag = item
                End Sub)
            SpreadCMS.Show(MousePosition)
        End Sub

        Private Sub CurvesTSMIDropDownOpening(sender As Object, e As EventArgs) Handles CurvesTSMI.DropDownOpening
            BondCurvesTSMI.DropDownItems.Clear()
            Dim chainTA As New chainTableAdapter
            Dim chainCurves = chainTA.GetData.Where(Function(row) row.curve).Select(
                Function(row)
                    Return New CurveDescr With {
                    .Type = "Chain",
                    .Name = row.descr,
                    .ID = row.id,
                    .Color = row.color}
                End Function).ToList()
            DoAdd(chainCurves)

            Dim hawserTA As New hawserTableAdapter
            Dim hawserCurves = hawserTA.GetData.Where(Function(row) row.curve).Select(
                Function(row)
                    Return New CurveDescr With {
                    .Type = "List",
                    .Name = row.hawser_name,
                    .ID = row.id,
                    .Color = row.color}
                End Function).ToList()
            DoAdd(hawserCurves)
        End Sub

        Private Sub AddBondCurveTSMIClick(sender As Object, e As EventArgs)
            Logger.Info("AddBondCurveTSMIClick")
            Dim selectedItem = CType(CType(sender, ToolStripMenuItem).Tag, CurveDescr)
            Dim ricsInCurve As List(Of String)

            Dim fieldNames As New Dictionary(Of QuoteSource, String)
            Dim fieldSetId As Integer
            If selectedItem.Type = "List" Then
                Dim data As New _ricsInHawserTableAdapter
                Dim theData = data.GetData
                ricsInCurve = theData.Where(Function(row) row.ric IsNot Nothing AndAlso row.id = selectedItem.ID).Select(Function(row) row.ric).ToList()
                fieldSetId = (New hawserTableAdapter).GetData.First(Function(row) row.id = selectedItem.ID).id_field_set
            Else
                Dim data As New _ricsInChainTableAdapter
                Dim theData = data.GetData
                ricsInCurve = theData.Where(Function(row) row.ric IsNot Nothing AndAlso row.id = selectedItem.ID).Select(Function(row) row.ric).ToList()
                fieldSetId = (New chainTableAdapter).GetData.First(Function(row) row.id = selectedItem.ID).id_field_set
            End If

            Dim layoutData = (New field_layoutTableAdapter).GetData.Where(Function(row) row.field_set_id = fieldSetId)
            Dim realTime = layoutData.First(Function(row) row.is_realtime = 1)
            Dim history = layoutData.First(Function(row) row.is_realtime = 0)

            fieldNames.Add(QuoteSource.Bid, realTime.bid_field)
            fieldNames.Add(QuoteSource.Ask, realTime.ask_field)
            fieldNames.Add(QuoteSource.Last, realTime.last_field)
            fieldNames.Add(QuoteSource.Hist, history.last_field)

            If Not ricsInCurve.Any() Then
                MsgBox("Empty curve selected!")
                Return
            End If

            Dim newCurve = New YieldCurve(selectedItem.Name, ricsInCurve, selectedItem.Color, fieldNames, _spreadBenchmarks)

            AddHandler newCurve.Cleared, AddressOf _spreadBenchmarks.OnCurveRemoved
            AddHandler newCurve.Recalculated, AddressOf OnCurveRecalculated
            AddHandler newCurve.Updated, AddressOf OnCurvePaint
            AddHandler newCurve.Faulted, AddressOf OnCurveFault

            _moneyMarketCurves.Add(newCurve)
            newCurve.Subscribe()

        End Sub

        Private Sub SelDateTSMIClick(sender As Object, e As EventArgs) Handles SelDateTSMI.Click
            Logger.Debug("SelDateTSMI_Click()")
            Dim datePicker = New DatePickerForm
            If datePicker.ShowDialog() = DialogResult.OK Then
                Dim curves = _moneyMarketCurves.Where(Function(item) item.GetName() = MoneyCurveCMS.Tag.ToString())
                If curves.Count = 0 Then
                    Logger.Warn("No such curve {0}", MoneyCurveCMS.Tag.ToString())
                Else
                    Dim curve = curves.First
                    curve.SetDate(datePicker.TheCalendar.SelectionEnd)
                End If
            End If
        End Sub

        Private Sub DeleteMmCurveTSMIClick(sender As Object, e As EventArgs) Handles DeleteMMCurveTSMI.Click
            Logger.Debug("DeleteMmCurveTSMIClick()")
            Dim curves = _moneyMarketCurves.Where(Function(item) item.GetName() = MoneyCurveCMS.Tag.ToString())
            If curves.Count = 0 Then
                Logger.Warn("No such curve {0}", MoneyCurveCMS.Tag.ToString())
            Else
                Dim curve = curves.First
                Dim irsSeries = TheChart.Series.FindByName(curve.GetName())
                If irsSeries IsNot Nothing Then TheChart.Series.Remove(irsSeries)
                _moneyMarketCurves.Remove(curve)

                curve.Cleanup()
                SetChartMinMax()
            End If
        End Sub

        Private Sub BootstrapTSMIClick(sender As Object, e As EventArgs) Handles BootstrapTSMI.Click
            Logger.Debug("BootstrapTSMIClick()")
            Dim snd = CType(sender, ToolStripMenuItem)
            Dim curve = _moneyMarketCurves.First(Function(item) item.GetName() = MoneyCurveCMS.Tag.ToString())
            If curve Is Nothing Then Return
            If curve.BootstrappingEnabled() Then curve.SetBootstrapped(snd.Checked)
        End Sub

        Private Sub OnBrokerSelected(sender As Object, e As EventArgs)
            Logger.Debug("OnBrokerSelected()")
            Dim snd = CType(sender, ToolStripMenuItem)
            Dim curve = _moneyMarketCurves.First(Function(item) item.GetName() = MoneyCurveCMS.Tag.ToString())
            If curve IsNot Nothing Then
                curve.SetBroker(snd.Text)
            End If
        End Sub

        Private Sub OnQuoteSelected(sender As Object, e As EventArgs)
            Logger.Debug("OnQuoteSelected()")
            Dim snd = CType(sender, ToolStripMenuItem)
            Dim curve = _moneyMarketCurves.First(Function(item) item.GetName() = MoneyCurveCMS.Tag.ToString())
            If curve IsNot Nothing Then
                curve.SetQuote(snd.Text)
            End If
        End Sub

        Private Sub OnFitSelected(ByVal sender As Object, ByVal e As EventArgs)
            Logger.Debug("OnFitSelected()")
            Dim snd = CType(sender, ToolStripMenuItem)
            Dim curve = _moneyMarketCurves.First(Function(item) item.GetName() = MoneyCurveCMS.Tag.ToString())
            If curve Is Nothing Then Return
            curve.SetFitMode(snd.Tag)
        End Sub

        Private Shared Sub OnCurveFault(ByVal curve As SwapCurve, ByVal ex As Exception)
            MessageBox.Show(ex.Message, "Curve error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Sub

        Private Sub OnCurvePaint(ByVal curve As SwapCurve)
            Logger.Debug("OnCurvePaint({0})", curve.GetName())
            PaintSwapCurve(curve, False)
            _spreadBenchmarks.UpdateCurve(curve.GetName())
            SetChartMinMax()
        End Sub

        Private Sub OnCurveRecalculated(ByVal curve As SwapCurve)
            Logger.Debug("OnCurveRecalculated({0})", curve.GetName())
            PaintSwapCurve(curve, True)
            SetChartMinMax()
        End Sub

        Private Sub RemoveFromChartTSMIClick(sender As Object, e As EventArgs) Handles RemoveFromChartTSMI.Click
            _ansamble.RemoveGroup(BondSetCMS.Tag)
        End Sub

        Private Sub RemovePointTSMIClick(sender As Object, e As EventArgs) Handles RemovePointTSMI.Click
            _ansamble.RemovePoint(BondCMS.Tag.ToString())
        End Sub

        Private Sub ShowHistoryTSMIClick(sender As Object, e As EventArgs) Handles ShowHistoryTSMI.Click
            Logger.Trace("ShowHistQuotesTSMIClick")
            Try
                '1) Load history for 10 days
                '_historicalCurves.AddCurve(BondCMS.Tag,
                '                           New HistoryTaskDescr() With {
                '                                .Item = BondCMS.Tag,
                '                                .EndDate = DateTime.Today,
                '                                .StartDate = DateTime.Today.AddDays(-90),
                '                                .Fields = {"DATE", "CLOSE"}.ToList(),
                '                                .Frequency = "D",
                '                                .InterestingFields = {"DATE", "CLOSE"}.ToList()
                '                            })
            Catch ex As Exception
                Logger.ErrorException("Got exception", ex)
                Logger.Error("Exception = {0}", ex.ToString())
            End Try
        End Sub

        Private Sub RemoveHistoryTSMIClick(sender As Object, e As EventArgs) Handles RemoveHistoryTSMI.Click
            '_historicalCurves.RemoveCurve(HistoryCMS.Tag)
        End Sub

        Private Sub EnterRICTSMIClick(sender As Object, e As EventArgs) Handles EnterRICTSMI.Click
            Dim askForm As New ManualRicForm()
            AddHandler askForm.Activated,
                Sub(snd As Object, evnt As EventArgs)
                    askForm.Left = Left + (Width - askForm.Width) / 2
                    askForm.Top = Top + (Height - askForm.Height) / 2
                End Sub

            askForm.ShowDialog(Me)
            If askForm.SelectedRic <> "" Then
                Dim group = New Group(_ansamble)
                Dim descr As DataBaseBondDescription

                Dim layout As New field_layoutTableAdapter
                Dim setInfo = layout.GetData().Where(Function(row) row.id = askForm.LayoutId)

                Dim rw = setInfo.First(Function(row) row.is_realtime = 1)
                group.AskField = rw.ask_field
                group.BidField = rw.bid_field
                group.LastField = rw.last_field
                group.VWAPField = rw.vwap_field
                group.VolumeField = rw.volume_field

                group.Fields.FieldName(FieldTime.RealTime, FieldType.Bid) = rw.bid_field
                group.Fields.FieldName(FieldTime.RealTime, FieldType.Ask) = rw.ask_field
                group.Fields.FieldName(FieldTime.RealTime, FieldType.Last) = rw.last_field
                group.Fields.FieldName(FieldTime.RealTime, FieldType.VWAP) = rw.vwap_field
                group.Fields.FieldName(FieldTime.RealTime, FieldType.TimeStamp) = rw.timestamp_field
                group.Fields.FieldName(FieldTime.RealTime, FieldType.Volume) = rw.volume_field

                rw = setInfo.First(Function(row) row.is_realtime = 0)
                group.HistField = rw.last_field

                group.Fields.FieldName(FieldTime.Historical, FieldType.Bid) = rw.bid_field
                group.Fields.FieldName(FieldTime.Historical, FieldType.Ask) = rw.ask_field
                group.Fields.FieldName(FieldTime.Historical, FieldType.Last) = rw.last_field
                group.Fields.FieldName(FieldTime.Historical, FieldType.VWAP) = rw.vwap_field
                group.Fields.FieldName(FieldTime.Historical, FieldType.TimeStamp) = rw.timestamp_field
                group.Fields.FieldName(FieldTime.Historical, FieldType.Volume) = rw.volume_field

                group.Color = "Red"
                descr = DbInitializer.GetBondInfo(askForm.SelectedRic)
                If descr IsNot Nothing Then
                    group.SeriesName = descr.ShortName
                    group.AddElement(askForm.SelectedRic, descr)
                    group.StartLoadingLiveData()
                    _ansamble.AddGroup(group)
                Else
                    Dim handled As Boolean = False
                    Dim selectedRic As String = askForm.SelectedRic
                    DbInitializer.RequestBondInfo(
                        askForm.SelectedRic,
                        Sub(dscr As DataBaseBondDescription)
                            If dscr Is Nothing Then
                                MsgBox("No bond found (RIC " & selectedRic & ")", MsgBoxStyle.Exclamation)
                                _ansamble.RemoveGroup(group.Id)
                            Else
                                If handled Then Return
                                handled = True
                                group.SeriesName = dscr.ShortName
                                group.AddElement(selectedRic, dscr)
                                group.StartLoadingLiveData()
                                _ansamble.AddGroup(group)
                            End If
                        End Sub)
                End If
            End If
        End Sub

        Private Sub SelectFromAListTSMIClick(sender As Object, e As EventArgs) Handles SelectFromAListTSMI.Click

        End Sub
#End Region

#Region "e) Assembly and curves events"
        Private Sub OnGroupClear(ByVal group As Group) Handles _ansamble.Clear
            Logger.Trace("OnGroupClear()")
            GuiAsync(
                Sub()
                    Dim series As Series = TheChart.Series.FindByName(group.SeriesName)

                    If series IsNot Nothing Then
                        series.Points.Clear()
                        TheChart.Series.Remove(series)
                    End If

                    With TheChart.Legends(0).CustomItems
                        While .Any(Function(elem) elem.Name = group.SeriesName)
                            .Remove(.First(Function(elem) elem.Name = group.SeriesName))
                        End While
                    End With
                End Sub)
        End Sub

        Private Sub OnBondAllQuotes(ByVal data As List(Of Bond)) Handles _ansamble.AllQuotes
            Logger.Trace("OnBondAllQuotes()")
            data.Where(Function(elem) elem.QuotesAndYields.ContainsKey(elem.SelectedQuote)).ToList.ForEach(Sub(elem) OnBondQuote(elem, elem.SelectedQuote, True))
            SetChartMinMax()
        End Sub

        Private Sub OnBondQuote(ByVal descr As Bond, ByVal fieldName As String, Optional ByVal raw As Boolean = False) Handles _ansamble.Quote
            Logger.Trace("OnBondQuote({0}, {1})", descr.MetaData.ShortName, descr.ParentGroup.SeriesName)
            GuiAsync(
                Sub()
                    Dim group = descr.ParentGroup
                    Dim calc = descr.QuotesAndYields(fieldName)
                    Dim ric = descr.MetaData.RIC

                    Dim series As Series = TheChart.Series.FindByName(group.SeriesName)
                    Dim clr = Color.FromName(group.Color)
                    If series Is Nothing Then
                        Dim seriesDescr = New BondSetSeries With {.Name = group.SeriesName, .Color = clr}
                        AddHandler seriesDescr.SelectedPointChanged, AddressOf OnSelectedPointChanged
                        series = New Series(group.SeriesName) With {
                            .YValuesPerPoint = 1,
                            .ChartType = SeriesChartType.Point,
                            .IsVisibleInLegend = False,
                            .color = If(calc.YieldSource = YieldSource.Realtime, Color.White, Color.Black),
                            .markerSize = 8,
                            .markerBorderWidth = 2,
                            .markerBorderColor = clr,
                            .markerStyle = MarkerStyle.Circle,
                            .Tag = seriesDescr
                        }
                        With series.EmptyPointStyle
                            .BorderWidth = 0
                            .MarkerSize = 0
                            .MarkerStyle = MarkerStyle.None
                        End With
                        series.SmartLabelStyle.Enabled = True
                        series.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No
                        TheChart.Series.Add(series)
                        Dim legendItem As New LegendItem(group.SeriesName, clr, "") With {
                            .Tag = group.Id
                        }
                        TheChart.Legends(0).CustomItems.Add(legendItem)
                    End If

                    ' creating data point
                    Dim point As DataPoint
                    Dim yValue = _spreadBenchmarks.GetActualQuote(calc)
                    Dim haveSuchPoint = series.Points.Any(Function(pnt) CType(pnt.Tag, Bond).MetaData.RIC = ric)

                    If haveSuchPoint Then
                        point = series.Points.First(Function(pnt) CType(pnt.Tag, Bond).MetaData.RIC = ric)
                        If yValue IsNot Nothing Then
                            point.XValue = calc.Duration
                            If yValue.Value <> point.YValues.First Then
                                Logger.Trace("{0}: delta is {1}", ric, yValue.Value - point.YValues.First)
                            End If
                            point.YValues = {yValue.Value}
                            point.Color = If(calc.YieldSource = YieldSource.Realtime, Color.White, Color.LightGray)
                            If ShowLabelsTSB.Checked Then point.Label = descr.MetaData.ShortName
                        Else
                            series.Points.Remove(point)
                        End If
                    ElseIf yValue IsNot Nothing Then
                        point = New DataPoint(calc.Duration, yValue.Value) With {
                            .Name = descr.MetaData.RIC,
                            .Tag = descr,
                            .ToolTip = descr.MetaData.ShortName,
                            .Color = If(calc.YieldSource = YieldSource.Realtime, Color.White, Color.LightGray)
                        }
                        If ShowLabelsTSB.Checked Then point.Label = descr.MetaData.ShortName
                        series.Points.Add(point)
                    End If
                    If Not raw Then SetChartMinMax()
                End Sub)
        End Sub

        Private Sub OnRemovedItem(group As Group, ric As String) Handles _ansamble.RemovedItem
            Logger.Trace("OnRemovedItem({0})", ric)
            GuiAsync(
                Sub()
                    Dim series As Series = TheChart.Series.FindByName(group.SeriesName)
                    If series IsNot Nothing Then
                        While series.Points.Any(Function(pnt) CType(pnt.Tag, Bond).MetaData.RIC = ric)
                            series.Points.Remove(series.Points.First(Function(pnt) CType(pnt.Tag, Bond).MetaData.RIC = ric))
                        End While
                    End If
                    If series.Points.Count = 0 Then _ansamble.RemoveGroup(group.Id)
                    SetChartMinMax()
                End Sub)
        End Sub

        Private Sub OnBenchmarkRemoved(type As SpreadType) Handles _spreadBenchmarks.BenchmarkRemoved
            Logger.Trace("OnBenchmarkRemoved({0})", type)
            _moneyMarketCurves.ForEach(Sub(curve) curve.CleanupByType(type))
            _ansamble.CleanupByType(type)
        End Sub

        Private Sub OnBenchmarkUpdated(type As SpreadType) Handles _spreadBenchmarks.BenchmarkUpdated
            Logger.Trace("OnBenchmarkUpdated({0})", type)
            _moneyMarketCurves.ForEach(Sub(curve) curve.RecalculateByType(type))
            _ansamble.RecalculateByType(type)
        End Sub

        Private Sub OnTypeSelected(newType As SpreadType, oldType As SpreadType) Handles _spreadBenchmarks.TypeSelected
            Logger.Trace("OnTypeSelected({0}, {1})", newType, oldType)
            If newType <> oldType Then
                SetYAxisMode(newType.ToString())
                _moneyMarketCurves.ForEach(Sub(curve) curve.RecalculateByType(newType))
                _ansamble.RecalculateByType(newType)
            End If
        End Sub

        'Public Sub OnHistoricalCurveData(ByVal hst As HistoryLoadManager, ByVal ric As String, ByVal datastatus As RT_DataStatus,
        '                                 ByVal data As Dictionary(Of Date, HistoricalItem))
        '    Logger.Debug("OnHistoricalCurveData")
        '    If data Is Nothing Then
        '        StatusMessage.Text = String.Format("No historical data on {0} available", _ansamble.GetBondDescription(ric).ShortName)
        '        Return
        '    End If
        '    GuiAsync(
        '        Sub()
        '            Try
        '                Dim series As Series = TheChart.Series.FindByName(ric + "_HIST_CURVE")
        '                If series Is Nothing Then
        '                    series = New Series(ric + "_HIST_CURVE") With {
        '                        .YValuesPerPoint = 1,
        '                        .ChartType = SeriesChartType.Line,
        '                        .borderWidth = 1,
        '                        .borderDashStyle = ChartDashStyle.Dash,
        '                        .borderColor = Color.Green,
        '                        .Tag = New HistCurveSeries() With {.Name = ric},
        '                        .IsVisibleInLegend = False
        '                    }
        '                End If

        '                For Each yieldDur As Tuple(Of Double, DataPointDescr) In _
        '                    From bondHistoryDescr In data
        '                    Where bondHistoryDescr.Value.Close > 0
        '                    Select New Tuple(Of Double, DataPointDescr)(bondHistoryDescr.Value.Close, CalcYield(bondHistoryDescr.Value.Close, bondHistoryDescr.Key, _ansamble.GetBondDescription(ric)))

        '                    Dim point As New DataPoint
        '                    Dim duration = yieldDur.Item2.Duration
        '                    Dim convex = yieldDur.Item2.Convexity
        '                    Dim pvbp = yieldDur.Item2.PVBP
        '                    Dim bestYield = yieldDur.Item2.Yld
        '                    Dim thePrice = yieldDur.Item1

        '                    Dim theTag = New HistCurvePointDescr With {
        '                        .Duration = duration,
        '                        .Convexity = convex,
        '                        .Yld = bestYield,
        '                        .PVBP = pvbp,
        '                        .RIC = ric,
        '                        .HistCurveName = ric + "_HIST_CURVE",
        '                        .Price = thePrice,
        '                        .BondTag = _ansamble.GetBondDescription(ric)
        '                    }

        '                    Dim yValue = _spreadBenchmarks.CalculateSpreads(theTag)
        '                    If yValue IsNot Nothing Then
        '                        With point
        '                            .YValues = {yValue}
        '                            .XValue = duration
        '                            .MarkerStyle = bestYield.ToWhat.MarkerStyle
        '                            .MarkerBorderWidth = 1
        '                            .Tag = theTag
        '                            .IsEmpty = Not theTag.IsValid
        '                        End With
        '                        series.Points.Add(point)
        '                    End If
        '                Next
        '                If series.Points.Count > 0 Then TheChart.Series.Add(series)
        '            Catch ex As Exception
        '                Logger.WarnException("Failed to plot a chart", ex)
        '            End Try
        '        End Sub)
        '    If datastatus <> RT_DataStatus.RT_DS_PARTIAL Then
        '        RemoveHandler hst.NewData, AddressOf OnHistoricalCurveData
        '    End If
        'End Sub

        'Private Sub OnCurveRemoved(ByVal theName As String)
        '    Dim item = TheChart.Series.FindByName(theName + "_HIST_CURVE")
        '    If item IsNot Nothing Then
        '        item.Points.Clear()
        '        TheChart.Series.Remove(item)
        '    Else
        '        Logger.Warn("Failed to remove historical series {0}", theName)
        '    End If
        'End Sub
#End Region
#End Region
    End Class
End Namespace