Imports System.Windows.Forms
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Drawing
Imports AdfinXAnalyticsFunctions
Imports System.ComponentModel
Imports DbManager.Bonds
Imports YieldMap.Forms.PortfolioForm
Imports Settings
Imports Uitls
Imports YieldMap.Forms.TableForm
Imports YieldMap.Tools.History
Imports YieldMap.Curves
Imports YieldMap.My.Resources
Imports YieldMap.Commons
Imports YieldMap.Tools
Imports ReutersData
Imports NLog
Imports DbManager

Namespace Forms.ChartForm
    Public Class GraphForm
        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(GraphForm))

        Private WithEvents _theSettings As SettingsManager = SettingsManager.Instance
        Private WithEvents _tableForm As TableForm.TableForm = New TableForm.TableForm()
        Private WithEvents _bondsLoader As IBondsLoader = BondsLoader.Instance()
        Private WithEvents _connector As EikonConnector = EikonConnector.Instance()

        Private ReadOnly _moneyMarketCurves As New List(Of SwapCurve)
        Private WithEvents _spreadBenchmarks As New SpreadContainer
        'Private WithEvents _ansamble As New Ansamble(_spreadBenchmarks)
        Private WithEvents _ansamble As New Ansamble2

        Private Sub Loader_Progress(ByVal obj As ProgressEvent) Handles _bondsLoader.Progress
            ' todo what to do?
        End Sub

        Private Sub TheSettings_DurRangeChanged(ByVal min As Double?, ByVal max As Double?) Handles _theSettings.DurRangeChanged
            SetChartMinMax()
        End Sub

        Private Sub TheSettings_ShowPointSizeChanged(ByVal show As Boolean) Handles _theSettings.ShowPointSizeChanged
            ' todo тут надо просто все перерисовать
        End Sub

        Private Sub TheSettings_SpreadRangeChanged(ByVal min As Double?, ByVal max As Double?) Handles _theSettings.SpreadRangeChanged
            SetChartMinMax()
        End Sub

        Private Sub _theSettings_YieldCalcModeChanged(ByVal obj As String) Handles _theSettings.YieldCalcModeChanged
            ' todo (а что тут туду? просто все пересчитать. хотя это не так просто - ибо у меня пересчет не упорядочен)
            ' todo сначала надо все заморозить. потом пересчитать доходности. потом спреды. хотя.... хотя... а спредам не до лампочки ли?
        End Sub

        Private Sub TheSettings_YieldRangeChanged(ByVal min As Double?, ByVal max As Double?) Handles _theSettings.YieldRangeChanged
            SetChartMinMax()
        End Sub

        Private Sub Connector_Connected() Handles _connector.Connected
            ' todo connected means that we were disconnected. I might have to resume some activities
        End Sub

        Private Sub Connector_Disconnected() Handles _connector.Disconnected
            Close()
        End Sub

        Private Sub Connector_LocalMode() Handles _connector.LocalMode
            ' todo what to do?
        End Sub

        Private Sub Connector_Offline() Handles _connector.Offline
            ' todo what to do?
        End Sub

#Region "I) Dependent forms"
        'Private Sub TableFormShown(sender As Object, e As EventArgs) Handles _tableForm.Shown
        '    AddHandler PointUpdated, AddressOf _tableForm.OnPointUpdated
        'End Sub

        'Private Sub TableFormFormClosing(sender As Object, e As FormClosingEventArgs) Handles _tableForm.FormClosing
        '    RemoveHandler PointUpdated, AddressOf _tableForm.OnPointUpdated
        'End Sub
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

                        '_historicalCurves.Clear()  ' todo where R these curves?

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
                Else
                    currentPortID = value
                End If

                Dim portfolioStructure = PortfolioManager.Instance().GetPortfolioStructure(currentPortID)

                For Each group As Group2 In From port In portfolioStructure.Sources
                                           Where TypeOf port.Source Is DbManager.Chain Or TypeOf port.Source Is UserList
                                           Select Group2.Create(_ansamble, port, portfolioStructure)
                    _ansamble.Items.Add(group)
                Next
                _ansamble.Start()
            End Set
        End Property

        

#End Region

#Region "III) Event handling"
#Region "a) Form events"
        Private Sub GraphFormLoad(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            Logger.Trace("GraphFormLoad")
            ThisFormStatus = FormDataStatus.Loading

            ItemDescriptionPanel.Visible = _theSettings.ShowChartToolBar
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

        Private Sub GraphFormFormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
            Logger.Trace("GraphForm_FormClosing")
            _ansamble.Cleanup()
            _moneyMarketCurves.ForEach(Sub(curve) curve.Cleanup())
            _moneyMarketCurves.Clear()
            ThisFormStatus = FormDataStatus.Stopped
        End Sub

        Private Sub GraphFormSizeChanged(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.SizeChanged
            InfoLabel.Left = (MainPanel.ClientSize.Width - InfoLabel.Width) / 2
            InfoLabel.Top = (MainPanel.ClientSize.Height - InfoLabel.Height) / 2
        End Sub

        Private Sub GraphFormClick(ByVal sender As Object, ByVal e As EventArgs) Handles MainPanel.Click
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
        Private Sub TheChartClick(ByVal sender As Object, ByVal e As EventArgs) Handles TheChart.Click
            Logger.Trace("TheChartClick")
            Dim mouseEvent As MouseEventArgs = e
            Dim htr As HitTestResult = TheChart.HitTest(mouseEvent.X, mouseEvent.Y)
            If mouseEvent.Button = MouseButtons.Right Then
                Try
                    If htr.ChartElementType = ChartElementType.DataPoint Then
                        Dim point As DataPoint = CType(htr.Object, DataPoint)
                        If TypeOf point.Tag Is Bond2 Then
                            Dim bondDataPoint = CType(point.Tag, Bond2)
                            With BondCMS
                                .Tag = bondDataPoint.MetaData.RIC
                                .Show(TheChart, mouseEvent.Location)
                            End With
                            MainInfoLine1TSMI.Text = point.ToolTip

                            ExtInfoTSMI.DropDownItems.Clear()
                            bondDataPoint.QuotesAndYields.Keys.ToList.ForEach(
                                Sub(key)
                                    Dim x = bondDataPoint.QuotesAndYields(key)
                                    Dim newItem = ExtInfoTSMI.DropDownItems.Add(String.Format("{0}: {1:F4}, {2:P2} {3}, {4:F2}", bondDataPoint.GetFieldByKey(key), x.Price, x.Yld.Yield, x.Yld.ToWhat.Abbr, x.Duration))
                                    If bondDataPoint.SelectedQuote = key Then CType(newItem, ToolStripMenuItem).Checked = True
                                    AddHandler newItem.Click,
                                        Sub(sender1 As Object, e1 As EventArgs)
                                            bondDataPoint.SelectedQuote = key
                                        End Sub
                                End Sub)

                            Dim newItem1 As ToolStripMenuItem

                            ExtInfoTSMI.DropDownItems.Add("-")
                            ExtInfoTSMI.DropDownItems.Add(String.Format("Today volume: {0:N0}", bondDataPoint.TodayVolume))
                            ExtInfoTSMI.DropDownItems.Add("-")
                            newItem1 = ExtInfoTSMI.DropDownItems.Add("Set custom price...")
                            AddHandler newItem1.Click,
                                       Sub(sender1 As Object, e1 As EventArgs)
                                           Dim res = InputBox("Enter price", "Custom bond price")
                                           If IsNumeric(res) Then
                                               bondDataPoint.SetCustomPrice(CDbl(res))
                                               bondDataPoint.SelectedQuote = Group.CustomField
                                           ElseIf res <> "" Then
                                               MessageBox.Show("Invalid number")
                                           End If

                                       End Sub
                            IssuerNameSeriesTSMI.Checked = (bondDataPoint.LabelMode = LabelMode.IssuerAndSeries)
                            ShortNameTSMI.Checked = (bondDataPoint.LabelMode = LabelMode.IssuerCpnMat)
                            DescriptionTSMI.Checked = (bondDataPoint.LabelMode = LabelMode.Description)
                            SeriesOnlyTSMI.Checked = (bondDataPoint.LabelMode = LabelMode.SeriesOnly)
                        ElseIf TypeOf point.Tag Is SwapCurve Then
                            Dim curve = CType(point.Tag, SwapCurve)
                            MMNameTSMI.Text = curve.GetFullName()
                            MoneyCurveCMS.Show(TheChart, mouseEvent.Location)
                            MoneyCurveCMS.Tag = curve.GetName()
                            ShowCurveParameters(curve)

                        ElseIf TypeOf point.Tag Is HistoryPoint Then
                            Dim histDataPoint = CType(point.Tag, HistoryPoint)
                            HistoryCMS.Tag = histDataPoint
                            HistoryCMS.Show(TheChart, mouseEvent.Location)
                        End If
                    ElseIf htr.ChartElementType = ChartElementType.PlottingArea Or htr.ChartElementType = ChartElementType.Gridlines Then
                        ChartCMS.Show(TheChart, mouseEvent.Location)
                    ElseIf htr.ChartElementType = ChartElementType.LegendItem Then
                        Dim item = CType(htr.Object, LegendItem)
                        If _ansamble.HasGroupById(item.Tag) Then
                            BondSetCMS.Tag = item.Tag
                            BondSetCMS.Show(TheChart, mouseEvent.Location)
                        End If
                    End If
                Catch ex As Exception
                    Logger.WarnException("Something went wrong", ex)
                    Logger.Warn("Exception = {0}", ex.ToString())
                End Try
            ElseIf mouseEvent.Button = MouseButtons.Left AndAlso htr.ChartElementType = ChartElementType.AxisTitle AndAlso htr.Axis.Equals(TheChart.ChartAreas(0).AxisY) Then
                ShowYAxisCMS(mouseEvent.Location)
            End If
        End Sub

        Private Sub TheChartInvalidated(ByVal sender As Object, ByVal e As InvalidateEventArgs) Handles TheChart.Invalidated
            If TheChart.Series IsNot Nothing AndAlso TheChart.Series.Count = 0 AndAlso Not MainTableLayout.Controls.ContainsKey("InfoLabel") Then
                TheChart.Visible = False
                InfoLabel.Visible = True
            Else
                InfoLabel.Visible = False
                TheChart.Visible = True
            End If
        End Sub

        Private Sub TheChartMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles TheChart.MouseMove
            Dim mouseEvent As MouseEventArgs = e
            Dim hasShown = False
            Try
                Dim htr As HitTestResult = TheChart.HitTest(mouseEvent.X, mouseEvent.Y)
                If (htr IsNot Nothing) AndAlso htr.ChartElementType = ChartElementType.DataPoint Then
                    hasShown = True
                    Dim point As DataPoint = CType(htr.Object, DataPoint)

                    If TypeOf point.Tag Is Bond2 And Not point.IsEmpty Then
                        Dim bondData = CType(point.Tag, Bond2)
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
                        MatLabel.Text = String.Format("{0:dd/MM/yyyy}", Utils.FromExcelSerialDate(aDate.GetValue(1, 1)))

                        Select Case _spreadBenchmarks.CurrentType
                            Case SpreadType.Yield : YldLabel.Text = String.Format("{0:P2}", point.YValues(0))
                            Case SpreadType.ZSpread : ZSpreadLabel.Text = String.Format("{0:F0} b.p.", point.YValues(0))
                            Case SpreadType.PointSpread : SpreadLabel.Text = String.Format("{0:F0} b.p.", point.YValues(0))
                            Case SpreadType.ASWSpread : ASWLabel.Text = String.Format("{0:F0} b.p.", point.YValues(0))
                        End Select
                        DurLabel.Text = String.Format("{0:F2}", point.XValue)

                        DatLabel.Text = ""

                    ElseIf TypeOf point.Tag Is HistoryPoint Then
                        DurLabel.Text = String.Format("{0:F2}", point.XValue)
                        YldLabel.Text = String.Format("{0:P2}", point.YValues(0))

                        Dim historyDataPoint = CType(point.Tag, HistoryPoint)
                        DscrLabel.Text = historyDataPoint.Meta.ShortName
                        DatLabel.Text = String.Format("{0:dd/MM/yyyy}", historyDataPoint.Descr.YieldAtDate)
                        ConvLabel.Text = String.Format("{0:F2}", historyDataPoint.Descr.Convexity)
                        MatLabel.Text = String.Format("{0:dd/MM/yyyy}", historyDataPoint.Meta.Maturity)
                        CpnLabel.Text = String.Format("{0:F2}%", historyDataPoint.Meta.Coupon)
                        PVBPLabel.Text = String.Format("{0:F4}", historyDataPoint.Descr.PVBP)
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
                                                  Dim tg = CType(point.Tag, Bond2)
                                                  point.Color = If(tg.QuotesAndYields(tg.SelectedQuote).YieldSource = YieldSource.Historical, Color.LightGray, Color.White)
                                              End Sub)
                    If seriesDescr.Name = curveName AndAlso pointIndex IsNot Nothing Then
                        srs.Points(pointIndex).Color = Color.Red
                    End If
                End Sub)
        End Sub

        ' The chart resizing
        Private Sub ResizePictureBoxMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles ResizePictureBox.MouseDown
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

        Private Sub ResizePictureBoxMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles ResizePictureBox.MouseMove
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

        Private Sub ResizePictureBoxMouseLeave(ByVal sender As Object, ByVal e As EventArgs) Handles ResizePictureBox.MouseLeave
            Logger.Trace("ResizePictureBoxMouseLeave")
            If ZoomCustomButton.Checked And _isResizing Then
                StopResize()
                StatusMessage.Text = "Resize cancelled"
            End If
        End Sub

        Private Sub ResizePictureBoxMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles ResizePictureBox.MouseUp
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

        Private Sub ResizePictureBoxPaint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles ResizePictureBox.Paint
            Logger.Trace("ResizePictureBoxMouseUp")
            If ZoomCustomButton.Checked And _isResizing Then
                e.Graphics.DrawRectangle(New Pen(Color.Black), _resizeRectangle)
            End If
        End Sub
#End Region

#Region "c) Toolbar events"
        Private Sub ZoomAllButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles ZoomAllButton.Click
            SetChartMinMax()
            TheChart.ChartAreas(0).AxisX.ScaleView.ZoomReset()
            TheChart.ChartAreas(0).AxisY.ScaleView.ZoomReset()
        End Sub

        Private Sub ZoomCustomButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles ZoomCustomButton.Click
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

        Private Sub PortfolioTssbDropDownOpening(ByVal sender As Object, ByVal e As EventArgs) Handles PortfolioTSSB.DropDownOpening
            Dim portDescrList As List(Of IdName(Of Integer)) =
                (From rw In PortfolioManager.Instance.GetAllPortfolios()
                 Select New IdName(Of Integer)() With {.Id = rw.Id, .Value = rw.Value}).ToList()

            PortfolioTSSB.DropDownItems.Clear()

            If portDescrList.Any Then
                portDescrList.ForEach(
                    Sub(idname)
                        Dim item = PortfolioTSSB.DropDownItems.Add(idname.Value, Nothing, AddressOf PortfolioSelectTSCBSelectedIndexChanged)
                        item.Tag = idname.Id
                    End Sub)
            End If
        End Sub

        Private Sub PortfolioSelectTSCBSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
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

        Private Sub RubCCSTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles RubCCSTSMI.Click
            Logger.Debug("RubCCSTSMIClick()")
            Dim rubCCS = New RubCCS(_spreadBenchmarks)

            AddHandler rubCCS.Cleared, AddressOf _spreadBenchmarks.OnCurveRemoved
            AddHandler rubCCS.Cleared, AddressOf CurveDeleted
            AddHandler rubCCS.Recalculated, AddressOf OnCurveRecalculated
            AddHandler rubCCS.Updated, AddressOf OnCurvePaint

            rubCCS.Subscribe()
            _moneyMarketCurves.Add(rubCCS)
        End Sub

        Private Sub RubIRS_TSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles RubIRSTSMI.Click
            Logger.Debug("RubIRSTSMIClick()")
            Dim rubIRS = New RubIRS(_spreadBenchmarks)
            AddHandler rubIRS.Cleared, AddressOf _spreadBenchmarks.OnCurveRemoved
            AddHandler rubIRS.Cleared, AddressOf CurveDeleted
            AddHandler rubIRS.Recalculated, AddressOf OnCurveRecalculated
            AddHandler rubIRS.Updated, AddressOf OnCurvePaint

            rubIRS.Subscribe()
            _moneyMarketCurves.Add(rubIRS)
        End Sub

        Private Sub NDFTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles NDFTSMI.Click
            Logger.Debug("NDFTSMI_Click()")
            Dim rubNDF = New RubNDF(_spreadBenchmarks)
            AddHandler rubNDF.Cleared, AddressOf _spreadBenchmarks.OnCurveRemoved
            AddHandler rubNDF.Cleared, AddressOf CurveDeleted
            AddHandler rubNDF.Recalculated, AddressOf OnCurveRecalculated
            AddHandler rubNDF.Updated, AddressOf OnCurvePaint

            rubNDF.Subscribe()
            _moneyMarketCurves.Add(rubNDF)
        End Sub

        Private Sub ShowLegendTSBClicked(ByVal sender As Object, ByVal e As EventArgs) Handles ShowLegendTSB.Click
            TheChart.Legends(0).Enabled = ShowLegendTSB.Checked
        End Sub

        Private Sub ShowLabelsTSBClick(ByVal sender As Object, ByVal e As EventArgs) Handles ShowLabelsTSB.Click
            Logger.Trace("ShowLabelsTSBClick")
            For Each series In From srs In TheChart.Series Where TypeOf srs.Tag Is BondSetSeries
                Dim points = series.Points
                For Each dataPoint In From pnt In points Where TypeOf pnt.Tag Is Bond2 Select {pnt, CType(pnt.Tag, Bond2)}
                    Dim lab As String
                    Select Case dataPoint(1).LabelMode
                        Case LabelMode.IssuerAndSeries : lab = dataPoint(1).Metadata.Label1
                        Case LabelMode.IssuerCpnMat : lab = dataPoint(1).Metadata.Label2
                        Case LabelMode.Description : lab = dataPoint(1).Metadata.Label3
                        Case LabelMode.SeriesOnly : lab = dataPoint(1).Metadata.Label4
                    End Select
                    dataPoint(0).Label = If(ShowLabelsTSB.Checked, lab, "")
                Next
            Next
        End Sub

        Private Sub PinUnpinTSBClick(ByVal sender As Object, ByVal e As EventArgs) Handles PinUnpinTSB.Click
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

        Private Sub ChartCMSOpening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ChartCMS.Opening
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

        Private Sub CopyToClipboardTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles CopyToClipboardTSMI.Click
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

        Private Sub RelatedQuoteTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles RelatedQuoteTSMI.Click
            If _ansamble.ContainsRIC(BondCMS.Tag.ToString()) Then RunCommand("reuters://REALTIME/verb=FullQuote/ric=" + BondCMS.Tag.ToString())
        End Sub

        Private Sub BondDescriptionTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles BondDescriptionTSMI.Click
            If _ansamble.ContainsRIC(BondCMS.Tag.ToString()) Then RunCommand("reuters://REALTIME/verb=BondData/ric=" + BondCMS.Tag.ToString())
        End Sub

        Private Sub RelatedChartTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles RelatedChartTSMI.Click
            If _ansamble.ContainsRIC(BondCMS.Tag.ToString()) Then RunCommand("reuters://REALTIME/verb=RelatedGraph/ric=" + BondCMS.Tag.ToString())
        End Sub



        Private Sub ShowCurveItemsTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles ShowCurveItemsTSMI.Click
            Dim aTable As DataGridView
            Dim theCurve As SwapCurve
            Dim aForm As Form

            theCurve = _moneyMarketCurves.First(Function(curve) curve.GetName() = CStr(MoneyCurveCMS.Tag))

            Dim aCurve = theCurve.GetSnapshot()

            aForm = New Form With {
                .Text = "Curve items",
                .Width = 400,
                .Height = 400,
                .FormBorderStyle = FormBorderStyle.Sizable
            }

            Dim tl As New TableLayoutPanel
            tl.RowCount = 2
            tl.RowStyles.Add(New RowStyle(SizeType.Absolute, 30))
            tl.RowStyles.Add(New RowStyle(SizeType.AutoSize))
            tl.Dock = DockStyle.Fill
            aForm.Controls.Add(tl)

            Dim aToolBar = New ToolStrip
            aToolBar.Dock = DockStyle.Fill
            Dim addButton = aToolBar.Items.Add("Add items...")
            addButton.Enabled = TypeOf theCurve Is YieldCurve
            Dim removeButton = aToolBar.Items.Add("Remove selected")
            removeButton.Enabled = TypeOf theCurve Is YieldCurve

            tl.Controls.Add(aToolBar, 0, 0)

            aTable = New DataGridView
            aTable.AutoGenerateColumns = False
            aTable.AllowUserToAddRows = False
            aTable.AllowUserToDeleteRows = False
            aTable.AllowUserToResizeRows = False
            aTable.AllowDrop = False

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
            aTable.Dock = DockStyle.Fill

            tl.Controls.Add(aTable, 0, 1)

            Dim rmHandler = GetRemoveHandler(aForm, aTable, theCurve)
            AddHandler removeButton.Click, rmHandler
            AddHandler addButton.Click, GetAddHandler(theCurve)

            AddHandler aTable.RowHeaderMouseClick,
                Sub(sender1 As Object, args As MouseEventArgs)
                    If Not TypeOf theCurve Is YieldCurve Then Return

                    If args.Button = MouseButtons.Right Then
                        Dim cms As New ContextMenuStrip
                        Dim removeItemTSMI = cms.Items.Add("Remove item from curve")
                        AddHandler removeItemTSMI.Click, rmHandler
                        cms.Show(MousePosition)
                    End If
                End Sub
            aForm.ShowDialog()
        End Sub

        Private Function GetAddHandler(ByVal swapCurve As SwapCurve) As EventHandler
            Return Sub()
                       Dim aForm = New Form With {
                            .Text = "Select one or more items to add",
                            .Width = 400,
                            .Height = 400,
                            .FormBorderStyle = FormBorderStyle.Sizable
                       }

                       Dim tl As New TableLayoutPanel
                       tl.RowCount = 2
                       tl.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
                       tl.RowStyles.Add(New RowStyle(SizeType.Absolute, 30))
                       tl.Dock = DockStyle.Fill
                       tl.ColumnCount = 2
                       tl.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50))
                       tl.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50))
                       aForm.Controls.Add(tl)

                       Dim lst As New ListBox
                       lst.SelectionMode = SelectionMode.MultiExtended

                       Dim allItems = swapCurve.GetOriginalRICs()
                       Dim existingItems = swapCurve.GetCurrentRICs()
                       For Each item In existingItems
                           allItems.Remove(item)
                       Next
                       For Each item In allItems
                           lst.Items.Add(item)
                       Next

                       lst.Dock = DockStyle.Fill
                       tl.Controls.Add(lst, 0, 0)
                       tl.SetColumnSpan(lst, 2)

                       Dim selectedItems As New List(Of String)

                       Dim okButt As New Button
                       okButt.Text = "OK"
                       AddHandler okButt.Click,
                           Sub()
                               If lst.SelectedItems.Count = 0 Then
                                   If MessageBox.Show("No items selected! Would you like to continue selecting items?", "Adding items into curve", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = MsgBoxResult.No Then
                                       aForm.DialogResult = DialogResult.Cancel
                                       aForm.Close()
                                   End If
                               Else
                                   selectedItems.AddRange(lst.SelectedItems.Cast(Of String))
                                   aForm.DialogResult = DialogResult.OK
                                   aForm.Close()
                               End If
                           End Sub
                       tl.Controls.Add(okButt, 0, 1)

                       Dim cncButt As New Button
                       cncButt.Text = "Cancel"
                       cncButt.DialogResult = DialogResult.Cancel
                       cncButt.Dock = DockStyle.Right
                       tl.Controls.Add(cncButt, 1, 1)

                       If aForm.ShowDialog() = DialogResult.OK Then
                           swapCurve.AddItems(selectedItems)
                       End If
                   End Sub
        End Function

        Private Function GetRemoveHandler(ByVal aForm As Form, ByVal aTable As DataGridView, ByVal theCurve As SwapCurve) As EventHandler
            Return Sub(sender As Object, args As EventArgs)
                       Try
                           If aTable.SelectedRows.Count > 0 Then
                               If aTable.SelectedRows.Count >= aTable.Rows.Count - 1 Then
                                   If MessageBox.Show("This will delete the curve. Would you like to continue?", "Remove the curve",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question) = MsgBoxResult.Yes Then
                                       ' todo curve MUST HAVE DELETE METHOD WHICH WILL CLEAN IT UP AND RAISE EVENT TO CLEAR CHART

                                       ' deleting whole curve
                                       Dim irsSeries = TheChart.Series.FindByName(theCurve.GetName())
                                       If irsSeries IsNot Nothing Then TheChart.Series.Remove(irsSeries)
                                       _moneyMarketCurves.Remove(theCurve)

                                       theCurve.Cleanup()
                                       SetChartMinMax()
                                       aForm.Close()
                                   End If
                               Else
                                   ' deleting selected items
                                   Dim toDelete As List(Of String) = (From rw As DataGridViewRow In aTable.SelectedRows Select rw.Cells("RIC").Value).Cast(Of String).ToList()
                                   If Not theCurve.RemoveItems(toDelete) Then
                                       MessageBox.Show("Failed to delete selected items", "Remove items", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                   End If
                                   Dim crv = theCurve.GetSnapshot()
                                   aTable.Rows.Clear()
                                   crv.ForEach(Sub(item) aTable.Rows.Add(New Object() {item.Item1, item.Item2, item.Item3, item.Item4}))
                               End If
                           Else
                               MessageBox.Show("No items selected", "Remove curve items", MessageBoxButtons.OK, MessageBoxIcon.Information)
                           End If
                       Catch ex As Exception
                           Logger.ErrorException("Failed to delete items from curve", ex)
                           Logger.Error("Exception = {0}", ex)
                       End Try
                   End Sub
        End Function

        Private Sub AsTableTSBClick(ByVal sender As Object, ByVal e As EventArgs) Handles AsTableTSB.Click
            Dim bondsToShow As New List(Of BondDescr)
            _ansamble.Groups.ForEach(
                Sub(group)
                    group.Elements.Keys.ToList().ForEach(
                        Sub(elem)
                            Dim res As New BondDescr
                            Dim point = group.Elements(elem)
                            res.RIC = point.MetaData.RIC
                            res.Name = point.MetaData.ShortName
                            res.Maturity = point.MetaData.Maturity
                            res.Coupon = point.MetaData.Coupon
                            If point.QuotesAndYields.ContainsKey(point.SelectedQuote) Then
                                Dim quote = point.QuotesAndYields(point.SelectedQuote)
                                res.Price = quote.Price
                                res.Quote = point.SelectedQuote
                                res.QuoteDate = quote.YieldAtDate
                                res.State = BondDescr.StateType.Ok
                                res.ToWhat = quote.Yld.ToWhat
                                res.BondYield = quote.Yld.Yield
                                res.CalcMode = BondDescr.CalculationMode.SystemPrice
                                res.Convexity = quote.Convexity
                                res.Duration = quote.Duration
                                res.Live = quote.YieldAtDate = Date.Today
                            End If
                            bondsToShow.Add(res)
                        End Sub)
                End Sub)
            _tableForm.Bonds = bondsToShow
            _tableForm.ShowDialog()
        End Sub

        Private Sub LinkSpreadLabelLinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles SpreadLinkLabel.LinkClicked
            ShowCurveCMS("PointSpread",
                If(_spreadBenchmarks.Benchmarks.ContainsKey(SpreadType.PointSpread), _spreadBenchmarks.Benchmarks(SpreadType.PointSpread), Nothing))
        End Sub

        Private Sub ZSpreadLinkLabelLinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles ZSpreadLinkLabel.LinkClicked
            ShowCurveCMS("ZSpread",
                If(_spreadBenchmarks.Benchmarks.ContainsKey(SpreadType.ZSpread), _spreadBenchmarks.Benchmarks(SpreadType.ZSpread), Nothing))
        End Sub

        Private Sub ASWLinkLabelLinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles ASWLinkLabel.LinkClicked
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

        Private Sub CurvesTSMIDropDownOpening(ByVal sender As Object, ByVal e As EventArgs) Handles CurvesTSMI.DropDownOpening
            BondCurvesTSMI.DropDownItems.Clear()
            DoAdd(From chain In PortfolioManager.Instance.ChainsView Where chain.Curve)
            DoAdd(From list In PortfolioManager.Instance.UserListsView Where list.Curve)
        End Sub

        Private Sub AddBondCurveTSMIClick(ByVal sender As Object, ByVal e As EventArgs)
            Logger.Info("AddBondCurveTSMIClick")
            Dim selectedItem = CType(CType(sender, ToolStripMenuItem).Tag, Source)
            Dim fieldNames As New Dictionary(Of QuoteSource, String)
            Dim ricsInCurve = selectedItem.GetDefaultRics()

            If Not ricsInCurve.Any() Then
                MsgBox("Empty curve selected!")
                Return
            End If

            Dim fields = New FieldSet(selectedItem.FieldSetId)

            fieldNames.Add(QuoteSource.Bid, fields.Realtime.Bid)
            fieldNames.Add(QuoteSource.Ask, fields.Realtime.Ask)
            fieldNames.Add(QuoteSource.Last, fields.Realtime.Last)
            fieldNames.Add(QuoteSource.Hist, fields.History.Last)


            Dim newCurve = New YieldCurve(selectedItem.Name, ricsInCurve, selectedItem.Color, fieldNames, _spreadBenchmarks)

            AddHandler newCurve.Cleared, AddressOf _spreadBenchmarks.OnCurveRemoved
            AddHandler newCurve.Cleared, AddressOf CurveDeleted
            AddHandler newCurve.Recalculated, AddressOf OnCurveRecalculated
            AddHandler newCurve.Updated, AddressOf OnCurvePaint
            AddHandler newCurve.Faulted, AddressOf OnCurveFault

            _moneyMarketCurves.Add(newCurve)
            newCurve.Subscribe()
        End Sub

        Private Sub SelDateTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles SelDateTSMI.Click
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

        Private Sub DeleteMmCurveTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles DeleteMMCurveTSMI.Click
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

        Private Sub BootstrapTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles BootstrapTSMI.Click
            Logger.Debug("BootstrapTSMIClick()")
            Dim snd = CType(sender, ToolStripMenuItem)
            Dim curve = _moneyMarketCurves.First(Function(item) item.GetName() = MoneyCurveCMS.Tag.ToString())
            If curve Is Nothing Then Return
            If curve.BootstrappingEnabled() Then curve.SetBootstrapped(snd.Checked)
        End Sub

        Private Sub OnBrokerSelected(ByVal sender As Object, ByVal e As EventArgs)
            Logger.Debug("OnBrokerSelected()")
            Dim snd = CType(sender, ToolStripMenuItem)
            Dim curve = _moneyMarketCurves.First(Function(item) item.GetName() = MoneyCurveCMS.Tag.ToString())
            If curve IsNot Nothing Then
                curve.SetBroker(snd.Text)
            End If
        End Sub

        Private Sub OnQuoteSelected(ByVal sender As Object, ByVal e As EventArgs)
            Logger.Debug("OnQuoteSelected()")
            Dim snd = CType(sender, ToolStripMenuItem)
            Dim curve = _moneyMarketCurves.First(Function(item) item.GetName() = MoneyCurveCMS.Tag.ToString())
            If curve IsNot Nothing Then
                curve.SetQuote(snd.Text)
            End If
        End Sub

        Private Sub OnFitSelected(ByVal sender As Object, ByVal e As EventArgs) Handles LinearRegressionTSMI.Click, LogarithmicRegressionTSMI.Click, InverseRegressionTSMI.Click, PowerRegressionTSMI.Click, Poly6RegressionTSMI.Click, NelsonSiegelSvenssonTSMI.Click, LinearInterpolationTSMI.Click, CubicSplineTSMI.Click, VasicekCurveTSMI.Click, CIRCurveTSMI.Click
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

        Private Sub RemoveFromChartTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles RemoveFromChartTSMI.Click
            If _ansamble.HasGroupById(BondSetCMS.Tag) Then
                _ansamble.RemoveGroup(BondSetCMS.Tag)
            End If
        End Sub

        Private Sub RemovePointTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles RemovePointTSMI.Click
            _ansamble.RemovePoint(BondCMS.Tag.ToString())
        End Sub

        Private Sub ShowHistoryTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles ShowHistoryTSMI.Click
            Logger.Trace("ShowHistQuotesTSMIClick")
            Try
                Dim historyLoader As New HistoryLoadManager_v2
                AddHandler historyLoader.HistoricalData, AddressOf OnHistoricalData
                historyLoader.StartTask(BondCMS.Tag, "DATE, CLOSE", DateTime.Today.AddDays(-90), DateTime.Today, timeOut:=15)
            Catch ex As Exception
                Logger.ErrorException("Got exception", ex)
                Logger.Error("Exception = {0}", ex.ToString())
            End Try
        End Sub

        Private Sub RemoveHistoryTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles RemoveHistoryTSMI.Click
            Try
                Dim tg = CType(HistoryCMS.Tag, HistoryPoint)
                Dim histSeries = TheChart.Series.First(Function(srs) TypeOf srs.Tag Is Guid AndAlso CType(srs.Tag, Guid) = tg.SeriesId)
                TheChart.Series.Remove(histSeries)
            Catch ex As Exception
                Logger.ErrorException("Failed to remove historical series", ex)
                Logger.Error("Exception = {0}", ex.ToString())
            End Try
        End Sub

        Private Sub SelectFromAListTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SelectFromAListTSMI.Click
            Dim bondSelector As New BondSelectorForm
            If bondSelector.ShowDialog() = DialogResult.OK AndAlso bondSelector.SelectedRICs.Any Then
                '    Dim groupSelector As New GroupSelectForm
                '    groupSelector.InitGroupList(_ansamble.GetGroupList())
                '    If groupSelector.ShowDialog() = DialogResult.OK Then
                '        Dim grp As Group
                '        If groupSelector.UseNew Then
                '            grp = New Group(_ansamble)
                '            grp.Color = groupSelector.NewColor.ToString()
                '            grp.SeriesName = groupSelector.NewName

                '            Dim layout As New field_layoutTableAdapter
                '            Dim setInfo = layout.GetData().Where(Function(row) row.field_set_id = groupSelector.LayoutId)
                '            Dim rw = setInfo.First(Function(row) row.is_realtime = 1)

                '            grp.AskField = rw.ask_field
                '            grp.BidField = rw.bid_field
                '            grp.LastField = rw.last_field
                '            grp.VwapField = rw.vwap_field
                '            grp.VolumeField = rw.volume_field

                '            rw = setInfo.First(Function(row) row.is_realtime = 0)
                '            grp.HistField = rw.last_field

                '            _ansamble.AddGroup(grp)
                '        Else
                '            grp = _ansamble.GetGroup(groupSelector.ExistingGroupId)
                '        End If

                '        ' TODO BULLSHIT AGAIN!!!
                '        Dim selectedField As String
                '        If grp.LastField.Trim() <> "" Then
                '            selectedField = grp.LastField
                '        ElseIf grp.VwapField.Trim() <> "" Then
                '            selectedField = grp.VwapField
                '        ElseIf grp.BidField.Trim() <> "" Then
                '            selectedField = grp.BidField
                '        Else
                '            selectedField = grp.AskField
                '        End If

                '        bondSelector.SelectedRICs.ForEach(
                '            Sub(aRic)
                '                Dim descr = DbInitializer.GetBondInfo(aRic)
                '                If descr IsNot Nothing Then
                '                    grp.AddRic(aRic, descr, selectedField)
                '                End If
                '            End Sub)
                '        grp.StartRics(bondSelector.SelectedRICs)
                '    End If

            End If
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

        Private Sub OnBondAllQuotes(ByVal data As List(Of Bond2)) Handles _ansamble.AllQuotes
            Logger.Trace("OnBondAllQuotes()")
            data.Where(Function(elem) elem.QuotesAndYields.ContainsKey(elem.SelectedQuote)).ToList.ForEach(Sub(elem) OnBondQuote(elem, elem.SelectedQuote, True))
            SetChartMinMax()
        End Sub

        Private Sub OnBondQuote(ByVal descr As Bond2, ByVal fieldName As String, Optional ByVal raw As Boolean = False) Handles _ansamble.Quote
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
                    Dim haveSuchPoint = series.Points.Any(Function(pnt) CType(pnt.Tag, Bond2).MetaData.RIC = ric)

                    If haveSuchPoint Then
                        point = series.Points.First(Function(pnt) CType(pnt.Tag, Bond2).MetaData.RIC = ric)
                        If yValue IsNot Nothing Then
                            point.XValue = calc.Duration
                            If Math.Abs(yValue.Value - point.YValues.First) > 0.01 Then
                                Logger.Trace("{0}: delta is {1}", ric, yValue.Value - point.YValues.First)
                            End If
                            point.YValues = {yValue.Value}
                            point.Color = If(calc.YieldSource = YieldSource.Realtime, Color.White, Color.LightGray)
                            point.MarkerStyle = IIf(fieldName <> group.CustomField,
                                               IIf(calc.Yld.ToWhat.Equals(YieldToWhat.Maturity), MarkerStyle.Circle, MarkerStyle.Triangle),
                                               MarkerStyle.Square)
                            If ShowLabelsTSB.Checked Then point.Label = descr.Label

                        Else
                            series.Points.Remove(point)
                        End If
                    ElseIf yValue IsNot Nothing Then
                        point = New DataPoint(calc.Duration, yValue.Value) With {
                            .Name = descr.MetaData.RIC,
                            .Tag = descr,
                            .ToolTip = descr.MetaData.ShortName,
                            .Color = If(calc.YieldSource = YieldSource.Realtime, Color.White, Color.LightGray),
                            .MarkerStyle = IIf(fieldName <> group.CustomField,
                                               IIf(calc.Yld.ToWhat.Equals(YieldToWhat.Maturity), MarkerStyle.Circle, MarkerStyle.Triangle),
                                               MarkerStyle.Square)
                        }
                        If ShowLabelsTSB.Checked Then point.Label = descr.MetaData.ShortName
                        series.Points.Add(point)
                    End If
                    If Not raw Then SetChartMinMax()
                End Sub)
        End Sub

        Private Sub OnRemovedItem(ByVal group As Group, ByVal ric As String) Handles _ansamble.RemovedItem
            Logger.Trace("OnRemovedItem({0})", ric)
            GuiAsync(
                Sub()
                    Dim series As Series = TheChart.Series.FindByName(group.SeriesName)
                    If series IsNot Nothing Then
                        While series.Points.Any(Function(pnt) CType(pnt.Tag, Bond2).MetaData.RIC = ric)
                            series.Points.Remove(series.Points.First(Function(pnt) CType(pnt.Tag, Bond2).MetaData.RIC = ric))
                        End While
                    End If
                    If series.Points.Count = 0 Then _ansamble.RemoveGroup(group.Id)
                    SetChartMinMax()
                End Sub)
        End Sub

        Private Sub OnBenchmarkRemoved(ByVal type As SpreadType) Handles _spreadBenchmarks.BenchmarkRemoved
            Logger.Trace("OnBenchmarkRemoved({0})", type)
            _moneyMarketCurves.ForEach(Sub(curve) curve.CleanupByType(type))
            _ansamble.CleanupByType(type)
        End Sub

        Private Sub OnBenchmarkUpdated(ByVal type As SpreadType) Handles _spreadBenchmarks.BenchmarkUpdated
            Logger.Trace("OnBenchmarkUpdated({0})", type)
            _moneyMarketCurves.ForEach(Sub(curve) curve.RecalculateByType(type))
            _ansamble.RecalculateByType(type)
        End Sub

        Private Sub OnTypeSelected(ByVal newType As SpreadType, ByVal oldType As SpreadType) Handles _spreadBenchmarks.TypeSelected
            Logger.Trace("OnTypeSelected({0}, {1})", newType, oldType)
            If newType <> oldType Then
                SetYAxisMode(newType.ToString())
                _moneyMarketCurves.ForEach(Sub(curve) curve.RecalculateByType(newType))
                _ansamble.RecalculateByType(newType)
            End If
        End Sub
#End Region
#End Region

        Private Sub SetLabel(ByVal ric As String, ByVal mode As LabelMode)
            Try
                Dim group = _ansamble.GetInstrumentGroup(ric)
                Dim bond = group.GetElement(ric)

                bond.LabelMode = mode
                If ShowLabelsTSB.Checked Then
                    TheChart.Series.FindByName(group.SeriesName).Points.First(Function(pnt) CType(pnt.Tag, Bond2).MetaData.RIC = ric).Label = bond.Label
                End If
            Catch ex As Exception
                Logger.WarnException("Failed to set label mode", ex)
                Logger.Warn("Exception = {0}", ex)
            End Try
        End Sub

        Private Sub IssuerNameSeriesTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles IssuerNameSeriesTSMI.Click
            SetLabel(BondCMS.Tag, LabelMode.IssuerAndSeries)
        End Sub

        Private Sub ShortNameTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ShortNameTSMI.Click
            SetLabel(BondCMS.Tag, LabelMode.IssuerCpnMat)
        End Sub

        Private Sub DescriptionTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DescriptionTSMI.Click
            SetLabel(BondCMS.Tag, LabelMode.Description)
        End Sub

        Private Sub SeriesOnlyTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SeriesOnlyTSMI.Click
            SetLabel(BondCMS.Tag, LabelMode.SeriesOnly)
        End Sub

        Private Sub SetSeriesLabel(ByVal id As String, ByVal mode As LabelMode)
            Try
                Dim group = _ansamble.GetGroupById(id)
                group.Elements.Values.ToList.ForEach(Sub(bond) bond.LabelMode = mode)
                If ShowLabelsTSB.Checked Then
                    TheChart.Series.FindByName(group.SeriesName).Points.ToList.ForEach(
                        Sub(pnt)
                            Try
                                Dim bond = CType(pnt.Tag, Bond2)
                                bond.LabelMode = mode
                                pnt.Label = bond.Label
                            Catch ex As Exception
                                Logger.WarnException("Failed to set label mode for point " & pnt.Tag, ex)
                                Logger.Warn("Exception = {0}", ex)
                            End Try
                        End Sub)
                End If
            Catch ex As Exception
            End Try
        End Sub

        Private Sub SeriesIssuerNameAndSeriesTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SeriesIssuerNameAndSeriesTSMI.Click
            SetSeriesLabel(BondSetCMS.Tag, LabelMode.IssuerAndSeries)
        End Sub

        Private Sub SeriesSeriesOnlyTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SeriesSeriesOnlyTSMI.Click
            SetSeriesLabel(BondSetCMS.Tag, LabelMode.SeriesOnly)
        End Sub

        Private Sub SeriesDescriptionTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SeriesDescriptionTSMI.Click
            SetSeriesLabel(BondSetCMS.Tag, LabelMode.Description)
        End Sub

        Private Sub IssuerCouponMaturityTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles IssuerCouponMaturityTSMI.Click
            SetSeriesLabel(BondSetCMS.Tag, LabelMode.IssuerCpnMat)
        End Sub

    End Class
End Namespace