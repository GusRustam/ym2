Imports System.Windows.Forms.DataVisualization.Charting
Imports YieldMap.Tools.Estimation
Imports YieldMap.Curves
Imports YieldMap.Tools
Imports YieldMap.Commons

Namespace Forms.ChartForm
    Partial Class GraphForm
        Private Sub InitChart()
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
        End Sub

        Private Sub ShowCurveParameters(ByVal curve As SwapCurve)
            BrokerTSMI.DropDownItems.Clear()
            Dim brokers = curve.GetBrokers()
            If brokers.Count = 0 Then
                BrokerTSMI.Enabled = False
            Else
                BrokerTSMI.Enabled = True
                Dim currBroker = curve.GetBroker()
                brokers.ToList.ForEach(Sub(broker) AddItem(broker, (broker = currBroker), BrokerTSMI, AddressOf OnBrokerSelected))
            End If

            QuoteTSMI.DropDownItems.Clear()
            Dim quotes = curve.GetQuotes()
            If quotes.Count = 0 Then
                QuoteTSMI.Enabled = False
            Else
                QuoteTSMI.Enabled = True
                Dim currQuote = curve.GetQuote()
                quotes.ToList.ForEach(Sub(quote) AddItem(quote, (quote = currQuote), QuoteTSMI, AddressOf OnQuoteSelected))
            End If

            Dim fitModes = curve.GetFitModes()
            If fitModes.Count() <= 1 Then
                FitTSMI.Visible = False
            Else
                FitTSMI.Visible = True
                FitTSMI.DropDownItems.Clear()
                Dim currFit = curve.GetFitMode()
                fitModes.ToList.ForEach(Sub(fit) AddItem(fit.FullName, (fit = currFit), FitTSMI, AddressOf OnFitSelected, fit.ItemName))
            End If

            If curve.BootstrappingEnabled Then
                BootstrapTSMI.Visible = True
                BootstrapTSMI.Enabled = True
                BootstrapTSMI.Checked = curve.IsBootstrapped
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

        Private Sub ShowYAxisCMS(ByVal loc As Point)
            If Not _spreadBenchmarks.Benchmarks.Any Then Return
            YAxisCMS.Items.Clear()
            YAxisCMS.Items.Add(SpreadType.Yield.ToString(), Nothing, AddressOf OnYAxisSelected)
            _spreadBenchmarks.Benchmarks.Keys.ToList.ForEach(Sub(key) YAxisCMS.Items.Add(key.Name, Nothing, AddressOf OnYAxisSelected))
            YAxisCMS.Show(TheChart, loc)
        End Sub

        Private Sub HideBidAsk()
            Dim bidAskSeries = TheChart.Series.FindByName("BidAskSeries")
            If bidAskSeries IsNot Nothing Then TheChart.Series.Remove(bidAskSeries)
        End Sub

        Private Sub PlotBidAsk(ByVal bond As VisualizableBond)
            If Not (bond.QuotesAndYields.ContainsKey(QuoteSource.Bid.ToString.ToUpper()) Or bond.QuotesAndYields.ContainsKey(QuoteSource.Ask.ToString.ToUpper())) Then Return
            Dim bidAskSeries = TheChart.Series.FindByName("BidAskSeries")
            Dim minX = TheChart.ChartAreas(0).AxisX.Minimum
            Dim maxX = TheChart.ChartAreas(0).AxisX.Maximum
            Dim minY = TheChart.ChartAreas(0).AxisY.Minimum
            Dim maxY = TheChart.ChartAreas(0).AxisY.Minimum
            If bidAskSeries Is Nothing Then
                bidAskSeries = New Series("BidAskSeries") With {
                            .YValuesPerPoint = 1,
                            .ChartType = SeriesChartType.Line,
                            .IsVisibleInLegend = False,
                            .color = Color.FromName(bond.ParentGroup.Color),
                            .markerSize = 4,
                            .markerStyle = MarkerStyle.Circle
                        }
                TheChart.Series.Add(bidAskSeries)
            End If
            bidAskSeries.Points.Clear()
            If bond.QuotesAndYields.ContainsKey(QuoteSource.Bid.ToString.ToUpper()) Then
                Dim calc = bond.QuotesAndYields(QuoteSource.Bid.ToString.ToUpper())
                Dim yValue = _spreadBenchmarks.GetActualQuote(calc)
                bidAskSeries.Points.Add(New DataPoint(calc.Duration, yValue.Value))
            End If
            If bond.QuotesAndYields.ContainsKey(QuoteSource.Ask.ToString.ToUpper()) Then
                Dim calc = bond.QuotesAndYields(QuoteSource.Ask.ToString.ToUpper())
                Dim yValue = _spreadBenchmarks.GetActualQuote(calc)
                bidAskSeries.Points.Add(New DataPoint(calc.Duration, yValue.Value))
            End If
            TheChart.ChartAreas(0).AxisX.Minimum = minX
            TheChart.ChartAreas(0).AxisX.Maximum = maxX
            TheChart.ChartAreas(0).AxisY.Minimum = minY
            TheChart.ChartAreas(0).AxisY.Minimum = maxY
        End Sub

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

        Private Sub SetYAxisMode(ByVal str As String)
            Try
                Select Case str
                    Case SpreadType.Yield.ToString() : MakeAxisY("Yield, %", "P2")
                    Case SpreadType.ASWSpread.ToString() : MakeAxisY("ASW Spread, b.p.", "N0")
                    Case SpreadType.PointSpread.ToString() : MakeAxisY("Spread, b.p.", "N0")
                    Case SpreadType.ZSpread.ToString() : MakeAxisY("Z-Spread, b.p.", "N0")
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
                    TheChart.Invalidate()
                    With TheChart.ChartAreas(0).AxisY
                        Try
                            Dim newMax = (From srs In TheChart.Series Where srs.Enabled And srs.Points.Any
                                Select (From pnt In srs.Points Select pnt.YValues.First).Max).Max 'Where Not pnt.IsEmpty

                            Dim newMin As Double
                            If _spreadBenchmarks.CurrentType.Equals(SpreadType.Yield) Then
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

        Private Sub ShowCurveCMS(nm As String, ByVal refCurve As SwapCurve)
            SpreadCMS.Items.Clear()
            SpreadCMS.Tag = nm
            If Not _moneyMarketCurves.Any() Then Return
            _moneyMarketCurves.Cast(Of SwapCurve).ToList().ForEach(
                Sub(item)
                    Dim elem = CType(SpreadCMS.Items.Add(item.GetFullName(), Nothing, AddressOf OnYieldCurveSelected), ToolStripMenuItem)
                    elem.CheckOnClick = True
                    elem.Checked = refCurve IsNot Nothing AndAlso item.GetFullName() = refCurve.GetFullName()
                    elem.Tag = item
                End Sub)
            SpreadCMS.Show(MousePosition)
        End Sub

        Private Sub ResetPointSelection()
            TheChart.Series.ToList.ForEach(
                Sub(srs)
                    Dim seriesDescr = TryCast(srs.Tag, BondSetSeries)
                    If seriesDescr Is Nothing Then Return

                    seriesDescr.ResetSelection()
                    srs.Points.ToList.ForEach(Sub(point)
                                                  Dim tg = CType(point.Tag, VisualizableBond)
                                                  point.Color = If(tg.QuotesAndYields(tg.SelectedQuote).YieldSource = YieldSource.Historical, Color.LightGray, Color.White)
                                              End Sub)
                End Sub)
        End Sub

        Private _resizeBasePoint As Point
        Private _isResizing As Boolean = False
        Private _resizeRectangle As New Rectangle

        Private Sub SetZoomRange(ByVal minX As Double, ByVal minY As Double, ByVal maxX As Double, ByVal maxY As Double)
            If minX > maxX Then
                Dim tmp = minX
                minX = maxX
                maxX = tmp
            End If
            TheChart.ChartAreas(0).AxisX.ScaleView.Zoom(minX, maxX)
            TheChart.ChartAreas(0).AxisY.ScaleView.Zoom(minY, maxY)
        End Sub

        Private Sub StopResize()
            ZoomCustomButton.Checked = False
            _isResizing = False
            ResizePictureBox.Visible = False
            TheChart.Visible = True
            TheChart.Enabled = True
            TheChart.BringToFront()
        End Sub


        Private Class CurveDescr
            Public Type As String
            Public Name As String
            Public ID As Integer
            Public Color As String
        End Class

        Private Sub DoAdd(ByVal curves As List(Of CurveDescr))
            curves.ForEach(
                Sub(curve)
                    Dim item = BondCurvesTSMI.DropDownItems.Add(curve.Name, Nothing, AddressOf AddBondCurveTSMIClick)
                    item.Tag = curve
                End Sub)
        End Sub

        Private Sub PaintSwapCurve(ByVal curve As SwapCurve, raw As Boolean)
            Logger.Debug("PaintSwapCurve({0}, {1})", curve.GetName(), raw)
            Dim points As List(Of SwapPointDescription)
            points = curve.GetCurveData()
            If points Is Nothing Then Return

            Dim estimator = New Estimator(curve.GetFitMode())
            Dim xyPoints = estimator.Approximate(XY.ConvertToXY(points, _spreadBenchmarks.CurrentType))

            If xyPoints Is Nothing Then Return

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
                            .Tag = curve,
                            .markerStyle = MarkerStyle.None,
                            .markerSize = 0
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

                    xyPoints.ForEach(
                        Sub(item)
                            theSeries.Points.Add(New DataPoint With {
                                .Name = String.Format("{0}Y", item.X),
                                .XValue = item.X,
                                .YValues = {item.Y},
                                .Tag = curve
                            })
                        End Sub)
                End Sub)
        End Sub
    End Class
End Namespace