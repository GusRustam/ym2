Imports System.Data
Imports System.Windows.Forms
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Drawing
Imports AdfinXAnalyticsFunctions
Imports YieldMap.Curves
Imports YieldMap.My.Resources
Imports YieldMap.Commons
Imports YieldMap.Tools
Imports YieldMap.BondsDataSetTableAdapters
Imports NLog
Imports YieldMap.Tools.Estimation

Namespace Forms.ChartForm
    Public Class GraphForm
        Private Shared ReadOnly Logger As Logger = GetLogger(GetType(GraphForm))
        Private WithEvents _tableForm As TableForm.TableForm = New TableForm.TableForm()

        'Private ReadOnly _historicalCurves As New HistoricalCurvesContainer(AddressOf OnHistoricalCurveData, AddressOf OnCurveRemoved)
        Private ReadOnly _moneyMarketCurves As New List(Of SwapCurve)
        Private ReadOnly _bidAskLines As New List(Of Tuple(Of DataPoint, DataPoint))

        Private WithEvents _spreadBenchmarks As New SpreadContainer
        Private WithEvents _ansamble As New VisualizableAnsamble(_spreadBenchmarks)

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
                        _moneyMarketCurves.ForEach(Sub(curve) curve.Cleanup())
                        _moneyMarketCurves.Clear()
                        _bidAskLines.Clear()

                        TheChart.Series.Clear()

                        StatusMessage.Text = "Stopped"
                End Select
            End Set
        End Property
#End Region

#Region "III) General GUI Actions and startup"
#Region "a) Begin and end"
        Private Sub GraphFormLoad(sender As Object, e As EventArgs) Handles MyBase.Load
            Logger.Trace("GraphFormLoad")
            ThisFormStatus = FormDataStatus.Loading

            ZoomCustomButton.CheckOnClick = True
            InitChart()

            ThisFormDataSource = -1
        End Sub

        Private Sub InitChart(Optional ByVal chartEmpty As Boolean = True)
            Dim axisFont = New Font(FontFamily.GenericSansSerif, 11)
            TheChart.AntiAliasing = AntiAliasingStyles.All
            TheChart.TextAntiAliasingQuality = TextAntiAliasingQuality.High

            With TheChart.ChartAreas(0)
                .AxisX.Title = "Duration, years"
                .AxisX.TitleFont = axisFont
                .AxisX.LabelStyle.Format = "F2"
                .AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash
                .AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Dot


                .AxisX.MajorGrid.Interval = 1.0
                .AxisX.MajorGrid.Enabled = True
                .AxisX.MinorGrid.Interval = 0.25
                .AxisX.MinorGrid.Enabled = True

                .AxisX.ScaleView.Zoomable = True
                .AxisX.ScaleView.SmallScrollMinSize = 1.0 / 12.0

                .AxisX.ScrollBar.Enabled = True
                .AxisX.ScrollBar.Size = 14
                .AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All
                .AxisX.ScrollBar.IsPositionedInside = True

                .AxisY.Title = "Yield, %"
                .AxisY.TitleFont = axisFont
                .AxisY.LabelStyle.Format = "P2"
                .AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash
                .AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dot
                .AxisY.MajorGrid.Interval = 0.01
                .AxisY.MajorGrid.Enabled = True
                .AxisY.MinorGrid.Interval = 0.005
                .AxisY.MinorGrid.Enabled = True

                .AxisY.ScaleView.Zoomable = True
                .AxisY.ScaleView.SmallScrollMinSize = 0.01 / 10

                .AxisY.ScrollBar.Enabled = True
                .AxisY.ScrollBar.Size = 14
                .AxisY.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All
                .AxisY.ScrollBar.IsPositionedInside = True
            End With

            If chartEmpty Then
                Dim series As Series = New Series("start") With {.ChartType = SeriesChartType.Point}
                series.Points.Add(New DataPoint(1, 0.1) With {.Color = Color.Black, .MarkerStyle = MarkerStyle.None, .MarkerSize = 1})
                TheChart.Series.Add(series)

                Dim headingFont = New Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold)
                TheChart.Titles.Add(New Title("Please select a portfolio to show", Docking.Top, headingFont, Color.Gray))
            End If
        End Sub

        Private Sub GraphFormFormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
            Logger.Trace("GraphForm_FormClosing")
            ThisFormStatus = FormDataStatus.Stopped
        End Sub
#End Region

#Region "c) Show legend, labels, tooltip and other info"
#Region "c.1) Click"
        Private Sub TheChartClick(sender As Object, e As EventArgs) Handles TheChart.Click
            Logger.Trace("TheChartClick")
            Dim mouseEvent As MouseEventArgs = e
            Dim htr As HitTestResult = TheChart.HitTest(mouseEvent.X, mouseEvent.Y)
            If mouseEvent.Button = MouseButtons.Right Then
                Try
                    If htr.ChartElementType = ChartElementType.DataPoint Then
                        Dim point As DataPoint = CType(htr.Object, DataPoint)
                        'If TypeOf point.Tag Is BondPointDescr Then
                        '    Dim bondDataPoint = CType(point.Tag, BondPointDescr)
                        '    With BondCMS
                        '        .Tag = bondDataPoint.RIC
                        '        .Show(TheChart, mouseEvent.Location)
                        '    End With
                        '    MainInfoLine1TSMI.Text = point.ToolTip
                        '    If bondDataPoint.YieldSource = YieldSource.Realtime Then
                        '        MainInfoLine2TSMI.Text =
                        '            String.Format("LAST: P [{0:F4}], Y [{1:P2}] {2}, D [{3:F2}]",
                        '                          bondDataPoint.CalcPrice, bondDataPoint.Yld.Yield, bondDataPoint.Yld.ToWhat.ToString(), bondDataPoint.Duration)
                        '        'StartBidAsk(bondDataPoint)
                        '        ExtInfoTSMI.Visible = True
                        '    Else
                        '        MainInfoLine2TSMI.Text =
                        '            String.Format("LAST: P [{0:F4}], Y [{1:P2}] {2}, D [{3:F2}] @ {4:dd/MM/yy}",
                        '                          bondDataPoint.CalcPrice, bondDataPoint.Yld.Yield, bondDataPoint.Yld.ToWhat.ToString(), bondDataPoint.Duration, bondDataPoint.YieldAtDate)

                        '        ExtInfoTSMI.Visible = False
                        '    End If

                        'Else
                        If TypeOf point.Tag Is MoneyMarketPointDescr Then
                            Dim curveDataPoint = CType(point.Tag, MoneyMarketPointDescr)
                            MMNameTSMI.Text = curveDataPoint.SwpCurve.GetFullName()
                            MoneyCurveCMS.Show(TheChart, mouseEvent.Location)
                            MoneyCurveCMS.Tag = curveDataPoint.YieldCurveName
                            ShowCurveParameters(curveDataPoint)

                            'ElseIf TypeOf point.Tag Is HistCurvePointDescr Then
                            '    Dim histDataPoint = CType(point.Tag, HistCurvePointDescr)
                            '    HistoryCMS.Tag = histDataPoint.RIC
                            '    HistoryCMS.Show(TheChart, mouseEvent.Location)

                            'ElseIf TypeOf point.Tag Is BidAskPointDescr Then
                            '    Dim bidAskDataPoint = CType(point.Tag, BidAskPointDescr)
                            '    BidAskCMS.Tag = bidAskDataPoint.BondTag.RIC
                            '    BidAskCMS.Show(TheChart, mouseEvent.Location)
                        End If
                    ElseIf htr.ChartElementType = ChartElementType.PlottingArea Or htr.ChartElementType = ChartElementType.Gridlines Then
                        ChartCMS.Show(TheChart, mouseEvent.Location)
                    End If
                Catch ex As Exception
                    Logger.WarnException("Something went wrong", ex)
                    Logger.Warn("Exception = {0}", ex.ToString())
                End Try
            ElseIf mouseEvent.Button = MouseButtons.Left AndAlso htr.ChartElementType = ChartElementType.AxisTitle AndAlso htr.Axis.Equals(TheChart.ChartAreas(0).AxisY) Then
                ShowYAxisCMS(mouseEvent.Location)
            End If
        End Sub

        Private Sub ShowCurveParameters(ByVal descr As MoneyMarketPointDescr)
            Dim theCurve = _moneyMarketCurves.First(Function(item) item.GetName() = descr.YieldCurveName)

            BrokerTSMI.DropDownItems.Clear()
            Dim brokers = theCurve.GetBrokers()
            If brokers.Count = 0 Then
                BrokerTSMI.Enabled = False
            Else
                BrokerTSMI.Enabled = True
                Dim currBroker = theCurve.GetBroker()
                brokers.ToList.ForEach(Sub(broker) AddItem(broker, (broker = currBroker), BrokerTSMI, AddressOf OnBrokerSelected))
            End If

            QuoteTSMI.DropDownItems.Clear()
            Dim quotes = theCurve.GetQuotes()
            If quotes.Count = 0 Then
                QuoteTSMI.Enabled = False
            Else
                QuoteTSMI.Enabled = True
                Dim currQuote = theCurve.GetQuote()
                quotes.ToList.ForEach(Sub(quote) AddItem(quote, (quote = currQuote), QuoteTSMI, AddressOf OnQuoteSelected))
            End If

            Dim fitting As IFittable = TryCast(theCurve, IFittable)
            If fitting Is Nothing Then
                FitTSMI.Visible = False
            Else
                FitTSMI.Visible = True

                FitTSMI.DropDownItems.Clear()
                Dim fits = fitting.GetFitModes()
                If fits.Count = 0 Then
                    FitTSMI.Enabled = False
                Else
                    FitTSMI.Enabled = True
                    Dim currFit = fitting.GetFitMode()
                    fits.ToList.ForEach(Sub(fit) AddItem(fit.FullName, (fit = currFit), FitTSMI, AddressOf OnFitSelected, fit.ItemName))
                End If
            End If

            Dim bootstr As IBootstrappable = TryCast(theCurve, IBootstrappable)
            If bootstr IsNot Nothing AndAlso bootstr.BootstrappingEnabled Then
                BootstrapTSMI.Visible = True
                BootstrapTSMI.Enabled = True
                BootstrapTSMI.Checked = bootstr.IsBootstrapped
            Else
                BootstrapTSMI.Enabled = False
                BootstrapTSMI.Visible = False
            End If
        End Sub

        Private Shared Sub AddItem(ByVal name As String, ByVal checked As Boolean, ByVal toolStripMenuItem As ToolStripMenuItem, ByVal eventHandler As EventHandler, Optional ByVal tag As Object = Nothing)
            Dim num = toolStripMenuItem.DropDownItems.Add(New ToolStripMenuItem(name, Nothing, eventHandler) With {.Checked = checked})
            Dim item = toolStripMenuItem.DropDownItems(num)
            If tag IsNot Nothing Then item.Tag = tag
        End Sub

        Private Sub BondContextMenuStripClosing(sender As Object, e As ToolStripDropDownClosingEventArgs) Handles BondCMS.Closing
            Logger.Trace("BondContextMenuStripClosing")
            ExtInfoTSMI.Enabled = False
            ExtInfoTSMI.Visible = True
            ExtInfoTSMI.Text = "Loading info"
            ExtInfoTSMI.DropDownItems.Clear()
        End Sub
#End Region

#Region "c.2) Y-Axis: yield / spread / z-spread"
        Private Sub UpdateAxisYTitle(ByVal mouseOver As Boolean)
            Dim axisFont As Font, clr As Color

            If _spreadBenchmarks.Benchmarks.Any() Then
                clr = Color.DarkBlue
                Dim fs As FontStyle = IIf(Not mouseOver, FontStyle.Bold, FontStyle.Bold Or FontStyle.Underline)
                axisFont = New Font(FontFamily.GenericSansSerif, 11, fs)
            Else
                clr = Color.Black
                axisFont = New Font(FontFamily.GenericSansSerif, 11)
            End If

            With TheChart.ChartAreas(0).AxisY
                .TitleFont = axisFont
                .TitleForeColor = clr
            End With
        End Sub

        Private Sub ShowYAxisCMS(ByVal loc As Point)
            If Not _spreadBenchmarks.Benchmarks.Any Then Return
            YAxisCMS.Items.Clear()
            YAxisCMS.Items.Add(SpreadMode.Yield.ToString(), Nothing, AddressOf OnYAxisSelected)
            _spreadBenchmarks.Benchmarks.Keys.ToList.ForEach(Sub(key) YAxisCMS.Items.Add(key.Name, Nothing, AddressOf OnYAxisSelected))
            YAxisCMS.Show(TheChart, loc)
        End Sub

        Private Sub OnYAxisSelected(ByVal sender As Object, ByVal e As EventArgs)
            Logger.Info("OnYAxisSelected()")
            Dim item As ToolStripMenuItem = TryCast(sender, ToolStripMenuItem)
            If item IsNot Nothing Then
                _spreadBenchmarks.CurrentMode = SpreadMode.FromString(item.Text)
                SetYAxisMode(item.Text)
            End If
        End Sub

        Private Sub SetYAxisMode(ByVal str As String)
            Try
                Select Case str
                    Case SpreadMode.Yield.ToString() : MakeAxisY("Yield, %", "P2")
                    Case SpreadMode.ASWSpread.ToString() : MakeAxisY("ASW Spread, b.p.", "N0")
                    Case SpreadMode.PointSpread.ToString() : MakeAxisY("Spread, b.p.", "N0")
                    Case SpreadMode.ZSpread.ToString() : MakeAxisY("Z-Spread, b.p.", "N0")
                End Select
                TheChart.ChartAreas(0).AxisX.ScaleView.ZoomReset()
                TheChart.ChartAreas(0).AxisY.ScaleView.ZoomReset()
            Catch ex As Exception
                Logger.WarnException("Failed to select y-axis variable " + str, ex)
                Logger.Warn("Exception = {0}", ex.ToString())
            End Try
        End Sub

        ' this method simply updates the chart by selecting appropriate y-coordinates for points
        Private Sub SetChartMinMax()
            Logger.Debug("SetChartMinMax()")
            GuiAsync(
                Sub()
                    With TheChart.ChartAreas(0).AxisY
                        Try
                            Dim newMax = (From srs In TheChart.Series Where srs.Enabled And srs.Points.Any
                                Select (From pnt In srs.Points Select pnt.YValues.First).Max).Max 'Where Not pnt.IsEmpty

                            Dim newMin As Double
                            If _spreadBenchmarks.CurrentMode.Equals(SpreadMode.Yield) Then
                                newMin = (From srs In TheChart.Series Where srs.Enabled And srs.Points.Any
                                        Select (From pnt In srs.Points Where pnt.YValues.First > 0 Select pnt.YValues.First).Min).Min
                            Else
                                newMin = (From srs In TheChart.Series Where srs.Enabled And srs.Points.Any
                                          Select (From pnt In srs.Points Select pnt.YValues.First).Min).Min
                            End If

                            If newMax > newMin Then
                                .Maximum = newMax
                                .Minimum = newMin
                                .MinorGrid.Interval = (.Maximum - .Minimum) / 20
                                .MajorGrid.Interval = (.Maximum - .Minimum) / 10
                            End If

                            Dim x As Series = TheChart.Series.FindByName("start")
                            If x IsNot Nothing Then
                                TheChart.Series.Remove(x)
                            End If

                        Catch ex As Exception
                            Logger.InfoException("Failed to set minmax", ex)
                            Logger.Info("Exception = {0}", ex.ToString())
                        End Try
                    End With
                End Sub)
        End Sub

        Private Sub MakeAxisY(ByVal title As String, ByVal format As String)
            With TheChart.ChartAreas(0).AxisY
                .Title = title
                .LabelStyle.Format = format
            End With
        End Sub
#End Region

#Region "c.3) Other actions"
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

        Private Sub ShowLabelsTSBClick(sender As Object, e As EventArgs) Handles ShowLabelsTSB.Click
            Logger.Trace("ShowLabelsTSBClick")
            For Each series As Series In TheChart.Series
                Logger.Trace(" -> series {0}", series.Name)
                series.SmartLabelStyle.Enabled = True
                series.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No

                For Each dataPoint As DataPoint In series.Points
                    'If ShowLabelsTSB.Checked AndAlso TypeOf dataPoint.Tag Is BondPointDescr Then
                    '    Dim data = CType(dataPoint.Tag, BondPointDescr)
                    '    If data.IsValid And data.IsVisible Then
                    '        dataPoint.Label = data.Label
                    '        Logger.Trace(" ---> adding label {0}", dataPoint.Label)
                    '    Else
                    '        Logger.Trace(" ---> removing label...")
                    '        dataPoint.Label = ""
                    '    End If
                    'Else
                    '    Logger.Trace(" ---> removing label...")
                    '    dataPoint.Label = ""
                    'End If
                Next
            Next
        End Sub

        Private Sub ShowLegendTSBClicked(sender As Object, e As EventArgs) Handles ShowLegendTSB.Click
            TheChart.Legends(0).Enabled = ShowLegendTSB.Checked
        End Sub

        Private Sub CopyToClipboardTSMIClick(sender As Object, e As EventArgs) Handles CopyToClipboardTSMI.Click
            Dim bmp As New Bitmap(TheChart.Width, TheChart.Height)
            TheChart.DrawToBitmap(bmp, TheChart.ClientRectangle)
            Clipboard.SetImage(bmp)
        End Sub
#End Region

#Region "c.4) Curves"
        Private Sub ShowCurveCMS(nm As String, ByVal refCurve As ICurve)
            SpreadCMS.Items.Clear()
            SpreadCMS.Tag = nm
            If Not _moneyMarketCurves.Any() Then Return
            _moneyMarketCurves.Cast(Of ICurve).ToList().ForEach(
                Sub(item)
                    Dim elem = CType(SpreadCMS.Items.Add(item.GetFullName(), Nothing, AddressOf OnYieldCurveSelected), ToolStripMenuItem)
                    elem.CheckOnClick = True
                    elem.Checked = refCurve IsNot Nothing AndAlso item.GetFullName() = refCurve.GetFullName()
                    elem.Tag = item
                End Sub)
            SpreadCMS.Show(MousePosition)
        End Sub

        Private Sub OnYieldCurveSelected(ByVal sender As Object, ByVal eventArgs As EventArgs)
            Logger.Info("OnYieldCurveSelected()")
            Dim senderTSMI = TryCast(sender, ToolStripMenuItem)
            If senderTSMI Is Nothing Then Return

            If senderTSMI.Checked Then
                _spreadBenchmarks.SetBenchmark(SpreadMode.FromString(SpreadCMS.Tag), senderTSMI.Tag)
            Else
                _spreadBenchmarks.CleanupCurve(CType(senderTSMI.Tag, ICurve))
            End If
            UpdateAxisYTitle(False)
        End Sub
#End Region

#Region "c.5) Mouse move and point selection"
        Private Sub TheChartMouseMove(sender As Object, e As MouseEventArgs) Handles TheChart.MouseMove
            Dim mouseEvent As MouseEventArgs = e
            Dim hasShown = False
            Try
                Dim htr As HitTestResult = TheChart.HitTest(mouseEvent.X, mouseEvent.Y)
                If (htr IsNot Nothing) AndAlso htr.ChartElementType = ChartElementType.DataPoint Then
                    hasShown = True
                    Dim point As DataPoint = CType(htr.Object, DataPoint)

                    Dim dat = CType(point.Tag, DataPointDescr)
                    SpreadLabel.Text = If(dat.PointSpread IsNot Nothing, String.Format("{0:F0} b.p.", dat.PointSpread), N_A)
                    ZSpreadLabel.Text = If(dat.ZSpread IsNot Nothing, String.Format("{0:F0} b.p.", dat.ZSpread), N_A)
                    ASWLabel.Text = If(dat.ASWSpread IsNot Nothing, String.Format("{0:F0} b.p.", dat.ASWSpread), N_A)
                    YldLabel.Text = String.Format("{0:P2}", dat.Yld)
                    DurLabel.Text = String.Format("{0:F2}", point.XValue)

                    'If TypeOf point.Tag Is BondPointDescr And Not point.IsEmpty Then
                    '    Dim bondData = CType(point.Tag, BondPointDescr)
                    '    DscrLabel.Text = bondData.Label
                    '    ConvLabel.Text = String.Format("{0:F2}", bondData.Convexity)
                    '    YldLabel.Text = String.Format("{0:P2} {1}", bondData.Yld.Yield, bondData.Yld.ToWhat.Abbr)
                    '    DatLabel.Text = String.Format("{0:dd/MM/yyyy}", bondData.YieldAtDate)
                    '    MatLabel.Text = String.Format("{0:dd/MM/yyyy}", bondData.Maturity)
                    '    CpnLabel.Text = String.Format("{0:F2}%", bondData.Coupon)
                    '    PVBPLabel.Text = String.Format("{0:F4}", bondData.PVBP)
                    '    CType(htr.Series.Tag, BondPointsSeries).SelectedPointIndex = htr.PointIndex

                    'Else
                    If TypeOf point.Tag Is MoneyMarketPointDescr Then
                        Dim data = CType(point.Tag, MoneyMarketPointDescr)
                        DscrLabel.Text = data.SwpCurve.GetFullName()
                        DatLabel.Text = String.Format("{0:dd/MM/yyyy}", data.SwpCurve.GetDate())
                        Dim period = String.Format("{0:F0}D", 365 * data.Duration)
                        Dim aDate = (New AdxDateModule).DfAddPeriod("RUS", Date.Today, period, "")
                        MatLabel.Text = String.Format("{0:dd/MM/yyyy}", FromExcelSerialDate(aDate.GetValue(1, 1)))

                        'ElseIf TypeOf point.Tag Is HistCurvePointDescr Then
                        '    Dim historyDataPoint = CType(point.Tag, HistCurvePointDescr)
                        '    DscrLabel.Text = historyDataPoint.BondTag.ShortName
                        '    DatLabel.Text = String.Format("{0:dd/MM/yyyy}", historyDataPoint.YieldAtDate)
                        '    ConvLabel.Text = String.Format("{0:F2}", historyDataPoint.Convexity)

                        'ElseIf TypeOf point.Tag Is BidAskPointDescr Then
                        '    Dim bidAskDP = CType(point.Tag, BidAskPointDescr)
                        '    ConvLabel.Text = String.Format("{0:F2}", bidAskDP.Convexity)
                        '    DscrLabel.Text = bidAskDP.BondTag.Label + " " + bidAskDP.BidAsk
                        '    DatLabel.Text = String.Format("{0:dd/MM/yyyy}", Date.Today)
                    Else
                        hasShown = False
                    End If
                ElseIf (htr IsNot Nothing) AndAlso htr.ChartElementType = ChartElementType.AxisTitle Then
                    UpdateAxisYTitle(True)
                Else
                    UpdateAxisYTitle(False)
                    ResetPointSelection()
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
                    ASWLabel.Text = If(_spreadBenchmarks.Benchmarks.ContainsKey(SpreadMode.ASWSpread), " -> " + _spreadBenchmarks.Benchmarks(SpreadMode.ASWSpread).GetFullName(), "")
                    SpreadLabel.Text = If(_spreadBenchmarks.Benchmarks.ContainsKey(SpreadMode.PointSpread), " -> " + _spreadBenchmarks.Benchmarks(SpreadMode.PointSpread).GetFullName(), "")
                    ZSpreadLabel.Text = If(_spreadBenchmarks.Benchmarks.ContainsKey(SpreadMode.ZSpread), " -> " + _spreadBenchmarks.Benchmarks(SpreadMode.ZSpread).GetFullName(), "")
                End If
            End Try
        End Sub

        Private Sub OnSelectedPointChanged(ByVal curveName As String, ByVal pointIndex As Integer?)
            TheChart.Series.ToList.ForEach(
                Sub(srs)
                    Dim seriesDescr = TryCast(srs.Tag, BondPointsSeries)
                    If seriesDescr Is Nothing Then Return

                    seriesDescr.ResetSelection()
                    Dim clr = _ansamble.GetGroup(srs.Name).Color
                    srs.Points.ToList.ForEach(Sub(point) point.BorderColor = Color.FromName(clr))
                    If seriesDescr.Name = curveName AndAlso pointIndex IsNot Nothing Then srs.Points(pointIndex).Color = Color.Red
                End Sub)
        End Sub

        Private Sub ResetPointSelection()
            TheChart.Series.ToList.ForEach(
                Sub(srs)
                    Dim seriesDescr = TryCast(srs.Tag, BondPointsSeries)
                    If seriesDescr Is Nothing Then Return

                    seriesDescr.ResetSelection()
                    Dim clr = _ansamble.GetGroup(srs.Name).Color
                    srs.Points.ToList.ForEach(Sub(point) point.BorderColor = Color.FromName(clr))
                End Sub)
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
#End Region
#End Region

#Region "d) Zoom, in and out"
        Private _resizeBasePoint As Point
        Private _isResizing As Boolean = False
        Private _resizeRectangle As New Rectangle

        Private Sub ZoomCustomButtonClick(sender As Object, e As EventArgs) Handles ZoomCustomButton.Click
            Logger.Trace("ZoomCustomButtonClick")
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

        Private Sub SetZoomRange(ByVal minX As Double, ByVal minY As Double, ByVal maxX As Double, ByVal maxY As Double)
            If minX > maxX Then
                Dim tmp = minX
                minX = maxX
                maxX = tmp
            End If
            TheChart.ChartAreas(0).AxisX.ScaleView.Zoom(minX, maxX)
            TheChart.ChartAreas(0).AxisY.ScaleView.Zoom(minY, maxY)
        End Sub

        Private Sub ResizePictureBoxPaint(sender As Object, e As PaintEventArgs) Handles ResizePictureBox.Paint
            Logger.Trace("ResizePictureBoxMouseUp")
            If ZoomCustomButton.Checked And _isResizing Then
                e.Graphics.DrawRectangle(New Pen(Color.Black), _resizeRectangle)
            End If
        End Sub

        Private Sub StopResize()
            ZoomCustomButton.Checked = False
            _isResizing = False
            ResizePictureBox.Visible = False
            TheChart.Visible = True
            TheChart.Enabled = True
            TheChart.BringToFront()
        End Sub

        Private Sub ZoomAllButtonClick(sender As Object, e As EventArgs) Handles ZoomAllButton.Click
            SetChartMinMax()
            TheChart.ChartAreas(0).AxisX.ScaleView.ZoomReset()
            TheChart.ChartAreas(0).AxisY.ScaleView.ZoomReset()
        End Sub
#End Region

#Region "e) Show as table"
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
            aTable.Columns.Add("Rate", "Rate")
            aTable.Columns.Add("Duration", "Duration")

            aTable.Columns("RIC").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            aTable.Columns("Rate").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            aTable.Columns("Rate").DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "P2"}
            aTable.Columns("Duration").AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            aTable.Columns("Duration").DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "N2"}

            aCurve.ForEach(Sub(item) aTable.Rows.Add(New Object() {item.Item1, item.Item2, item.Item3}))

            aForm.Controls.Add(aTable)
            aTable.Dock = DockStyle.Fill

            aForm.ShowDialog()
        End Sub
#End Region

#Region "e) Benchmark selection"
        Private Sub LinkSpreadLabelLinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkSpreadLabel.LinkClicked, LinkLabel1.LinkClicked
            ShowCurveCMS("PointSpread",
                If(_spreadBenchmarks.Benchmarks.ContainsKey(SpreadMode.PointSpread), _spreadBenchmarks.Benchmarks(SpreadMode.PointSpread), Nothing))
        End Sub

        Private Sub ZSpreadLinkLabelLinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles ZSpreadLinkLabel.LinkClicked
            ShowCurveCMS("ZSpread",
                If(_spreadBenchmarks.Benchmarks.ContainsKey(SpreadMode.ZSpread), _spreadBenchmarks.Benchmarks(SpreadMode.ZSpread), Nothing))
        End Sub

        Private Sub ASWLinkLabelLinkClicked(sender As System.Object, e As LinkLabelLinkClickedEventArgs) Handles ASWLinkLabel.LinkClicked
            Dim refCurve = If(_spreadBenchmarks.Benchmarks.ContainsKey(SpreadMode.ASWSpread), _spreadBenchmarks.Benchmarks(SpreadMode.ASWSpread), Nothing)
            SpreadCMS.Items.Clear()
            SpreadCMS.Tag = "ASWSpread"
            If Not _moneyMarketCurves.Any() Then Return
            _moneyMarketCurves.Where(
                Function(crv)
                    Return TypeOf crv Is IAssetSwapBenchmark AndAlso CType(crv, IAssetSwapBenchmark).CanBeBenchmark()
                End Function
            ).Cast(Of ICurve).ToList().ForEach(
                Sub(item)
                    Dim elem = CType(SpreadCMS.Items.Add(item.GetFullName(), Nothing, AddressOf OnYieldCurveSelected), ToolStripMenuItem)
                    elem.CheckOnClick = True
                    elem.Checked = refCurve IsNot Nothing AndAlso item.GetFullName() = refCurve.GetFullName()
                    elem.Tag = item
                End Sub)
            SpreadCMS.Show(MousePosition)

        End Sub
#End Region
#End Region

#Region "V) Portfolio selection"
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
                    Dim group As VisualizableGroup
                    If groupType.TryParse(port.whereFrom, groupType) Then
                        group = New VisualizableGroup(_ansamble) With {
                            .Group = groupType,
                            .SeriesName = port.whereFromDescr,
                            .PortfolioID = port.fromID,
                            .BidField = port.bid_field,
                            .AskField = port.ask_field,
                            .LastField = port.last_field,
                            .HistField = port.hist_field,
                            .Brokers = {"MM", ""}.ToList(),
                            .Currency = "",
                            .Color = port.color
                        }

                        Dim pD = From elem In portfolioUnitedDataTable Where elem.pid = currentPortID And elem.fromID = group.PortfolioID
                        Dim ars = (From row In pD Where row.include Select row.ric).ToList
                        Dim rrs = (From row In pD Where Not row.include Select row.ric).ToList
                        ars.RemoveAll(Function(ric) rrs.Contains(ric))
                        InitBondDescriber()
                        ars.ForEach(
                            Sub(ric)
                                Dim descr = GetBondInfo(ric)
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

        Private Sub PortfolioSelectTSCBSelectedIndexChanged(sender As Object, e As EventArgs) ' Handles portfolioSelectTSCB.SelectedIndexChanged
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
#End Region

#Region "VI) Show historical data"
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

        'Private Sub OnCurveRemoved(ByVal theName As String)
        '    Dim item = TheChart.Series.FindByName(theName + "_HIST_CURVE")
        '    If item IsNot Nothing Then
        '        item.Points.Clear()
        '        TheChart.Series.Remove(item)
        '    Else
        '        Logger.Warn("Failed to remove historical series {0}", theName)
        '    End If
        'End Sub

        Private Sub RemoveHistoryTSMIClick(sender As Object, e As EventArgs) Handles RemoveHistoryTSMI.Click
            '_historicalCurves.RemoveCurve(HistoryCMS.Tag)
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
#End Region

#Region "VII) Curves"
#Region "1) Common methods"
        Public Class CurveDescr
            Public Type As String
            Public Name As String
            Public ID As Integer
            Public Color As String
        End Class

        Private Sub CurvesTSMIDropDownOpening(sender As System.Object, e As EventArgs) Handles CurvesTSMI.DropDownOpening
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

        Private Sub DoAdd(ByVal curves As List(Of CurveDescr))
            curves.ForEach(
                Sub(curve)
                    Dim item = BondCurvesTSMI.DropDownItems.Add(curve.Name, Nothing, AddressOf AddBondCurveTSMIClick)
                    item.Tag = curve
                End Sub)
        End Sub

        Private Sub AddBondCurveTSMIClick(sender As Object, e As EventArgs)
            Logger.Info("AddBondCurveTSMIClick")
            Dim selectedItem = CType(CType(sender, ToolStripMenuItem).Tag, CurveDescr)
            Dim ricsInCurve As List(Of String)

            Dim fieldNames As New Dictionary(Of QuoteSource, String)
            If selectedItem.Type = "List" Then
                Dim data As New _ricsInHawserTableAdapter
                Dim theData = data.GetData
                ricsInCurve = theData.Where(Function(row) row.ric IsNot Nothing AndAlso row.id = selectedItem.ID).Select(Function(row) row.ric).ToList()
                Dim hawserData = (New hawserTableAdapter).GetData.First(Function(row) row.id = selectedItem.ID)
                fieldNames.Add(QuoteSource.Bid, hawserData.bid_field)
                fieldNames.Add(QuoteSource.Ask, hawserData.ask_field)
                fieldNames.Add(QuoteSource.Last, hawserData.last_field)
                fieldNames.Add(QuoteSource.Hist, hawserData.hist_field)
            Else
                Dim data As New _ricsInChainTableAdapter
                Dim theData = data.GetData
                ricsInCurve = theData.Where(Function(row) row.ric IsNot Nothing AndAlso row.id = selectedItem.ID).Select(Function(row) row.ric).ToList()
                Dim chainData = (New chainTableAdapter).GetData.First(Function(row) row.id = selectedItem.ID)
                fieldNames.Add(QuoteSource.Bid, chainData.bid_field)
                fieldNames.Add(QuoteSource.Ask, chainData.ask_field)
                fieldNames.Add(QuoteSource.Last, chainData.last_field)
                fieldNames.Add(QuoteSource.Hist, chainData.hist_field)
            End If

            If Not ricsInCurve.Any() Then
                MsgBox("Empty curve selected!")
                Return
            End If

            Dim newCurve = New YieldCurve(
                           Guid.NewGuid.ToString,
                           selectedItem.Name,
                           ricsInCurve,
                           selectedItem.Color,
                           fieldNames,
                           AddressOf _spreadBenchmarks.CleanupCurve,
                           AddressOf OnCurvePaint,
                           AddressOf OnCurveRecalculated)

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
            Dim fitting As IBootstrappable = TryCast(curve, IBootstrappable)
            If fitting IsNot Nothing Then fitting.SetBootstrapped(snd.Checked)
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
            Dim fitting As IFittable = TryCast(curve, IFittable)
            If fitting IsNot Nothing Then fitting.SetFitMode(snd.Tag)
        End Sub

        Private Sub OnCurvePaint(ByVal curve As ICurve, ByVal points As List(Of XY))
            Logger.Debug("OnCurvePaint({0})", curve.GetName())
            PaintSwapCurve(curve, points)
            _spreadBenchmarks.UpdateCurve(curve.GetName())
            SetChartMinMax()
        End Sub

        Private Sub OnCurveRecalculated(ByVal curve As ICurve, ByVal points As List(Of XY))
            Logger.Debug("OnCurveRecalculated({0})", curve.GetName())
            PaintSwapCurve(curve, points)
            SetChartMinMax()
        End Sub

        Private Sub PaintSwapCurve(ByVal curve As SwapCurve, ByVal points As List(Of XY))
            Logger.Debug("PaintSwapCurve({0}, {1} points)", curve.GetName(), If(points IsNot Nothing, points.Count, "No"))

            GuiAsync(
                Sub()
                    Dim theSeries = TheChart.Series.FindByName(curve.GetName())
                    If theSeries Is Nothing Then
                        theSeries = New Series() With {
                            .Name = curve.GetName(),
                            .legendText = curve.GetFullName(),
                            .ChartType = SeriesChartType.Line,
                            .color = curve.GetOuterColor(),
                            .markerColor = curve.GetInnerColor(),
                            .markerBorderColor = curve.GetOuterColor(),
                            .borderWidth = 2,
                            .Tag = New SwapCurveSeries With {.Name = curve.GetName(), .SwpCurve = curve}
                        }
                        TheChart.Series.Add(theSeries)
                    Else
                        theSeries.Points.Clear()
                        theSeries.LegendText = curve.GetFullName()
                    End If

                    If points Is Nothing OrElse points.Count < 2 Then
                        theSeries.Enabled = False
                        Logger.Info("Too little points to plot")
                        Return
                    End If
                    theSeries.Enabled = True

                    With theSeries
                        If points.Count < 50 Then
                            .MarkerStyle = MarkerStyle.Circle
                            .MarkerSize = 5
                        Else
                            .MarkerStyle = MarkerStyle.None
                            .MarkerSize = 0
                        End If
                    End With

                    points.ForEach(
                        Sub(item)
                            Dim theTag = New MoneyMarketPointDescr() With {
                                .Duration = item.X,
                                .IsVisible = True,
                                .YieldCurveName = curve.GetName(),
                                .FullName = curve.GetFullName(),
                                .SwpCurve = curve
                            }

                            Select Case curve.BmkSpreadMode
                                Case SpreadMode.Yield : theTag.Yld = New YieldStructure With {.Yield = item.Y}
                                Case SpreadMode.PointSpread : theTag.PointSpread = item.Y
                                Case SpreadMode.ZSpread : theTag.ZSpread = item.Y
                                Case SpreadMode.ASWSpread : theTag.ASWSpread = item.Y
                            End Select

                            theSeries.Points.Add(New DataPoint With {
                                .Name = String.Format("{0}Y", item.X),
                                .XValue = item.X,
                                .YValues = {item.Y},
                                .Tag = theTag
                            })
                        End Sub)
                End Sub)
        End Sub
#End Region

#Region "2) Specific curves"
        Private Sub RubCCSTSMIClick(sender As Object, e As EventArgs) Handles RubCCSTSMI.Click
            Logger.Debug("RubCCSTSMIClick()")
            Dim rubCCS = New RubCCS(DateTime.Today, Guid.NewGuid.ToString(),
                           AddressOf _spreadBenchmarks.CleanupCurve,
                           AddressOf OnCurvePaint,
                           AddressOf OnCurveRecalculated)
            rubCCS.Subscribe()
            _moneyMarketCurves.Add(rubCCS)
        End Sub

        Private Sub RubIRSTSMIClick(sender As Object, e As EventArgs) Handles RubIRSTSMI.Click
            Logger.Debug("RubIRSTSMIClick()")
            Dim rubIRS = New RubIRS(DateTime.Today, Guid.NewGuid.ToString(),
                           AddressOf _spreadBenchmarks.CleanupCurve,
                           AddressOf OnCurvePaint,
                           AddressOf OnCurveRecalculated)
            rubIRS.Subscribe()
            _moneyMarketCurves.Add(rubIRS)
        End Sub

        Private Sub NDFTSMIClick(sender As Object, e As EventArgs) Handles NDFTSMI.Click
            Logger.Debug("NDFTSMI_Click()")
            Dim rubNDF = New RubNDF(DateTime.Today, Guid.NewGuid.ToString(),
                           AddressOf _spreadBenchmarks.CleanupCurve,
                           AddressOf OnCurvePaint,
                           AddressOf OnCurveRecalculated)
            rubNDF.Subscribe()
            _moneyMarketCurves.Add(rubNDF)
        End Sub
#End Region
#End Region

        Private Sub OnBondQuote(ByVal descr As VisualizableBond, ByVal fieldName As String) Handles _ansamble.Quote
            GuiAsync(Sub()
                         Dim group = descr.ParentGroup
                         Dim calc = descr.QuotesAndYields(fieldName)
                         Dim ric = descr.MetaData.RIC

                         Dim series As Series = TheChart.Series.FindByName(group.SeriesName)
                         Dim clr = Color.FromName(group.Color)
                         If series Is Nothing Then
                             Dim seriesDescr = New BondPointsSeries With {.Name = group.SeriesName, .Color = clr}
                             AddHandler seriesDescr.SelectedPointChanged, AddressOf OnSelectedPointChanged
                             series = New Series(group.SeriesName) With {
                                  .YValuesPerPoint = 1,
                                  .ChartType = SeriesChartType.Point,
                                  .IsVisibleInLegend = True,
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
                             TheChart.Series.Add(series)
                         End If

                         ' creating data point
                         Dim point As DataPoint
                         Dim yValue = _spreadBenchmarks.GetQt(calc)
                         If Not series.Points.Any(Function(pnt) pnt.Name = ric) Then
                             If yValue IsNot Nothing Then
                                 point = New DataPoint(calc.Duration, yValue.Value) With {
                                                     .Name = descr.MetaData.RIC,
                                                     .Tag = descr,
                                                     .ToolTip = descr.MetaData.ShortName,
                                                     .Color = If(calc.YieldSource = YieldSource.Realtime, Color.White, Color.LightGray)
                                 }
                                 series.Points.Add(point)
                             End If
                         Else
                             point = series.Points.First(Function(pnt) pnt.Name = ric)
                             point.XValue = calc.Duration
                             point.YValues = {yValue.Value}
                             point.Color = If(calc.YieldSource = YieldSource.Realtime, Color.White, Color.LightGray)
                         End If
                     End Sub)
        End Sub
    End Class
End Namespace