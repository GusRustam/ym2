﻿Imports System.Windows.Forms.DataVisualization.Charting
Imports DbManager
Imports DbManager.Bonds
Imports YieldMap.Tools.Elements
Imports ReutersData
Imports Uitls
Imports YieldMap.Tools.Estimation
Imports YieldMap.Tools

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

            'Dim fitModes = curve.GetFitModes()
            'If fitModes.Count() <= 1 Then
            '    FitTSMI.Enabled = False
            'Else
            '    FitTSMI.Enabled = True
            '    Dim currFit = curve.GetFitMode()

            '    CheckFit(EstimationModel.Lin, fitModes, currFit, LinearRegressionTSMI)
            '    CheckFit(EstimationModel.Log, fitModes, currFit, LogarithmicRegressionTSMI)
            '    CheckFit(EstimationModel.Inv, fitModes, currFit, InverseRegressionTSMI)
            '    CheckFit(EstimationModel.Pow, fitModes, currFit, PowerRegressionTSMI)
            '    CheckFit(EstimationModel.Poly6, fitModes, currFit, Poly6RegressionTSMI)
            '    CheckFit(EstimationModel.NSS, fitModes, currFit, NelsonSiegelSvenssonTSMI)
            '    CheckFit(EstimationModel.LinInterp, fitModes, currFit, LinearInterpolationTSMI)
            '    CheckFit(EstimationModel.CubicSpline, fitModes, currFit, CubicSplineTSMI)
            '    CheckFit(EstimationModel.Vasicek, fitModes, currFit, VasicekCurveTSMI)
            '    CheckFit(EstimationModel.CoxIngersollRoss, fitModes, currFit, CIRCurveTSMI)
            'End If

            If curve.CanBootstrap Then
                BootstrapTSMI.Visible = True
                BootstrapTSMI.Enabled = True
                BootstrapTSMI.Checked = curve.Bootstrapped
            Else
                BootstrapTSMI.Enabled = False
                BootstrapTSMI.Visible = False
            End If
        End Sub

        'Private Shared Sub CheckFit(ByVal fit As EstimationModel, ByVal modes As EstimationModel(), ByVal curModel As EstimationModel, ByVal item As ToolStripMenuItem)
        '    If modes.Contains(fit) Then
        '        item.Visible = True
        '        item.Checked = (curModel = fit)
        '        item.Tag = fit
        '    End If
        'End Sub

        Private Shared Sub AddItem(ByVal name As String, ByVal checked As Boolean, ByVal toolStripMenuItem As ToolStripMenuItem, ByVal eventHandler As EventHandler, Optional ByVal tag As Object = Nothing)
            Dim num = toolStripMenuItem.DropDownItems.Add(New ToolStripMenuItem(name, Nothing, eventHandler) With {.Checked = checked})
            Dim item = toolStripMenuItem.DropDownItems(num)
            If tag IsNot Nothing Then item.Tag = tag
        End Sub

        Private Sub ShowYAxisCMS(ByVal loc As Point)
            If Not _spreadBenchmarks.Benchmarks.Any Then Return
            YAxisCMS.Items.Clear()
            YAxisCMS.Items.Add(YSource.Yield.ToString(), Nothing, AddressOf OnYAxisSelected)
            _spreadBenchmarks.Benchmarks.Keys.ToList.ForEach(Sub(key) YAxisCMS.Items.Add(key.Name, Nothing, AddressOf OnYAxisSelected))
            YAxisCMS.Show(TheChart, loc)
        End Sub

        Private Sub HideBidAsk()
            If Not _theSettings.ShowBidAsk Then Return
            Dim bidAskSeries = TheChart.Series.FindByName("BidAskSeries")
            If bidAskSeries IsNot Nothing Then TheChart.Series.Remove(bidAskSeries)
        End Sub

        Private Sub PlotBidAsk(ByVal bond As Bond)
            If Not _theSettings.ShowBidAsk Then Return
            If Not (bond.QuotesAndYields.Has(bond.Fields.Bid) Or bond.QuotesAndYields.Has(bond.Fields.Ask)) Then Return
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
                            .color = Color.FromName(bond.Parent.Color),
                            .markerSize = 4,
                            .markerStyle = MarkerStyle.Circle
                        }
                TheChart.Series.Add(bidAskSeries)
            End If
            bidAskSeries.Points.Clear()
            If bond.QuotesAndYields.Has(bond.Fields.Bid) Then
                Dim calc = bond.QuotesAndYields(bond.Fields.Bid)
                Dim yValue = _spreadBenchmarks.GetActualQuote(calc)
                bidAskSeries.Points.Add(New DataPoint(calc.Duration, yValue.Value))
            End If
            If bond.QuotesAndYields.Has(bond.Fields.Ask) Then
                Dim calc = bond.QuotesAndYields(bond.Fields.Ask)
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
                    Case YSource.Yield.ToString() : MakeAxisY("Yield, %", "P2")
                    Case YSource.ASWSpread.ToString() : MakeAxisY("ASW Spread, b.p.", "N0")
                    Case YSource.PointSpread.ToString() : MakeAxisY("Spread, b.p.", "N0")
                    Case YSource.ZSpread.ToString() : MakeAxisY("Z-Spread, b.p.", "N0")
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
            GuiAsync(AddressOf DoSetMinMax)
        End Sub

        Private Sub DoSetMinMax()
            TheChart.Invalidate()
            With TheChart.ChartAreas(0).AxisY
                Try
                    Dim lstMax = From srs In TheChart.Series
                                 Where srs.Enabled And srs.Points.Any
                                 Select (
                                     From pnt In srs.Points
                                     Where pnt.YValues.Any
                                     Select pnt.YValues.First).Max

                    If Not lstMax.Any Then
                        Logger.Warn("Nothing to use as maximum")
                        Return
                    End If

                    Dim newMax = lstMax.Max

                    Dim lstMin = From srs In TheChart.Series
                                 Where srs.Enabled And srs.Points.Any
                                 Select (
                                     From pnt In srs.Points
                                     Where pnt.YValues.Any And (_spreadBenchmarks.CurrentType <> YSource.Yield OrElse pnt.YValues.First > 0)
                                     Select pnt.YValues.First).Min


                    If Not lstMin.Any Then
                        Logger.Warn("Nothing to use as minimum")
                        Return
                    End If

                    Dim newMin = lstMin.Min

                    Dim minMin As Double?, maxMax As Double?
                    If _spreadBenchmarks.CurrentType = YSource.Yield Then
                        minMin = _theSettings.MinYield / 100
                        maxMax = _theSettings.MaxYield / 100
                    Else
                        minMin = _theSettings.MinSpread
                        maxMax = _theSettings.MaxSpread
                    End If

                    If newMax > newMin Then
                        Dim theMin As Double, theMax As Double
                        theMin = If(minMin.HasValue, Math.Max(minMin.Value, newMin), newMin)
                        theMax = If(maxMax.HasValue, Math.Min(maxMax.Value, newMax), newMax)

                        Dim pow = If(_spreadBenchmarks.CurrentType = YSource.Yield, 3, 0)
                        theMax = Math.Ceiling(theMax * (10 ^ pow)) / (10 ^ pow)
                        theMin = Math.Floor(theMin * (10 ^ pow)) / (10 ^ pow)
                        If theMax > theMin Then
                            .Maximum = theMax
                            .Minimum = theMin
                        Else
                            .Maximum = newMin
                            .Minimum = newMax
                            StatusMessage.Text = "Ignoring default chart range settings"
                            Logger.Warn("Min > Max")
                        End If
                        .MinorGrid.Interval = (.Maximum - .Minimum) / 20
                        .MajorGrid.Interval = (.Maximum - .Minimum) / 10
                    Else
                        Logger.Warn("Min > Max 2")
                    End If
                Catch ex As Exception
                    Logger.WarnException("Failed to set minmax", ex)
                    Logger.Warn("Exception = {0}", ex.ToString())
                End Try
            End With
        End Sub

        Private Sub MakeAxisY(ByVal title As String, ByVal format As String)
            With TheChart.ChartAreas(0).AxisY
                .Title = title
                .LabelStyle.Format = format
            End With
        End Sub

        Private Sub ShowCurveCMS(ByVal nm As String, ByVal refCurve As SwapCurve)
            SpreadCMS.Items.Clear()
            SpreadCMS.Tag = nm
            'If Not _moneyMarketCurves.Any() Then Return
            _moneyMarketCurves.ForEach(
                Sub(item)
                    Dim elem = CType(SpreadCMS.Items.Add(item.GetFullName(), Nothing, AddressOf OnSwapCurveSelected), ToolStripMenuItem)
                    elem.CheckOnClick = True
                    elem.Checked = refCurve IsNot Nothing AndAlso item.GetFullName() = refCurve.GetFullName()
                    elem.Tag = item
                End Sub)

            Dim items = (From elem In _ansamble.Items Where TypeOf elem.Value Is BondCurve Select elem.Value).ToList()
            If items.Any Then SpreadCMS.Items.Add(New ToolStripSeparator)
            For Each item In items
                Dim elem = CType(SpreadCMS.Items.Add(item.SeriesName, Nothing, AddressOf OnBondCurveSelected), ToolStripMenuItem)
                elem.CheckOnClick = True
                elem.Tag = item
            Next item

            ' todo add item to add a curve and use it as a benchmark
            If SpreadCMS.Items.Count > 0 Then SpreadCMS.Items.Add(New ToolStripSeparator)
            SpreadCMS.Items.Add(New ToolStripSeparator)
            SpreadCMS.Items.Add("New curve...", Nothing, AddressOf OnNewCurvePressed)
            SpreadCMS.Show(MousePosition)
        End Sub

        Private Sub OnNewCurvePressed(ByVal sender As Object, ByVal e As EventArgs)
            ' todo
        End Sub

        Private Sub OnSwapCurveSelected(ByVal sender As Object, ByVal eventArgs As EventArgs)
            Logger.Info("OnYieldCurveSelected()")
            Dim senderTSMI = TryCast(sender, ToolStripMenuItem)
            If senderTSMI Is Nothing Then Return

            If senderTSMI.Checked Then
                _spreadBenchmarks.AddType(YSource.FromString(SpreadCMS.Tag), senderTSMI.Tag)
            Else
                _spreadBenchmarks.RemoveType(YSource.FromString(SpreadCMS.Tag))
            End If

            UpdateAxisYTitle(False)

            ASWLabel.Text = If(_spreadBenchmarks.Benchmarks.ContainsKey(YSource.ASWSpread), " -> " + _spreadBenchmarks.Benchmarks(YSource.ASWSpread).GetFullName(), "")
            SpreadLabel.Text = If(_spreadBenchmarks.Benchmarks.ContainsKey(YSource.PointSpread), " -> " + _spreadBenchmarks.Benchmarks(YSource.PointSpread).GetFullName(), "")
            ZSpreadLabel.Text = If(_spreadBenchmarks.Benchmarks.ContainsKey(YSource.ZSpread), " -> " + _spreadBenchmarks.Benchmarks(YSource.ZSpread).GetFullName(), "")
        End Sub

        Private Sub OnBondCurveSelected(ByVal sender As Object, ByVal e As EventArgs)
            ' todo 
        End Sub

        Private Sub ResetPointSelection()
            TheChart.Series.ToList.ForEach(
                Sub(srs)
                    Dim seriesDescr = TryCast(srs.Tag, BondSetSeries)
                    If seriesDescr Is Nothing Then Return

                    seriesDescr.ResetSelection()
                    srs.Points.ToList.ForEach(Sub(point)
                                                  Dim tg = CType(point.Tag, Bond)
                                                  point.Color = Color.FromName(tg.QuotesAndYields.Main.BackColor)
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

        Private Sub DoAddNew(ByVal curves As IEnumerable(Of Source))
            For Each curve In curves
                Dim item = BondCurvesNewTSMI.DropDownItems.Add(curve.Name, Nothing, AddressOf AddBondCurveNewTSMIClick)
                item.Tag = curve
            Next
        End Sub

        'Private Sub PaintSwapCurve(ByVal curve As SwapCurve, ByVal raw As Boolean)
        '    Logger.Debug("PaintSwapCurve({0}, {1})", curve.GetName(), raw)
        '    Dim points As List(Of SwapPointDescription)
        '    points = curve.GetCurveData(raw)
        '    If points Is Nothing Then Return

        '    Dim estimator = New Estimator(curve.GetFitMode())
        '    Dim xyPoints = estimator.Approximate(XY.ConvertToXY(points, _spreadBenchmarks.CurrentType))

        '    If xyPoints Is Nothing Then Return
        '    Logger.Trace("Got points to plot")

        '    GuiAsync(
        '        Sub()
        '            Dim theSeries = TheChart.Series.FindByName(curve.GetName())
        '            If theSeries Is Nothing Then
        '                theSeries = New Series() With {
        '                    .Name = curve.GetName(),
        '                    .legendText = curve.GetFullName(),
        '                    .ChartType = SeriesChartType.Line,
        '                    .color = curve.GetOuterColor(),
        '                    .markerColor = curve.GetInnerColor(),
        '                    .markerBorderColor = curve.GetOuterColor(),
        '                    .borderWidth = 2,
        '                    .Tag = curve,
        '                    .markerStyle = MarkerStyle.None,
        '                    .markerSize = 0
        '                }
        '                TheChart.Series.Add(theSeries)
        '            Else
        '                theSeries.Points.Clear()
        '                theSeries.LegendText = curve.GetFullName()
        '            End If

        '            If points Is Nothing OrElse points.Count < 2 Then
        '                theSeries.Enabled = False
        '                Logger.Info("Too little points to plot")
        '                Return
        '            End If
        '            theSeries.Enabled = True

        '            xyPoints.ForEach(
        '                Sub(item)
        '                    theSeries.Points.Add(New DataPoint With {
        '                        .Name = String.Format("{0}Y", item.X),
        '                        .XValue = item.X,
        '                        .YValues = {item.Y},
        '                        .Tag = curve
        '                    })
        '                End Sub)
        '        End Sub)
        'End Sub

        Public Sub OnHistoricalData(ByVal ric As String, ByVal data As Dictionary(Of Date, HistoricalItem), ByVal rawData As Dictionary(Of DateTime, RawHistoricalItem))
            Logger.Trace("OnHistoricalQuotes({0})", ric)
            If _ansamble.YSource <> YSource.Yield Then Return

            If data Is Nothing OrElse data.Count <= 0 Then
                Logger.Info("No data on {0} arrived", ric)
                Return
            End If

            Dim elem = _ansamble.Items.Bonds(Function(m) m.RIC = ric).First()
            Dim bondDataPoint = elem.MetaData ' todo that's awful
            Dim points As New List(Of Tuple(Of BondPointDescription, BondDescription))
            For Each dt In data.Keys
                Try
                    Dim calc As New BondPointDescription
                    If data(dt).Close > 0 Then
                        calc.Price = data(dt).Close

                        CalculateYields(dt, bondDataPoint, calc)
                        points.Add(Tuple.Create(calc, bondDataPoint))
                    End If
                Catch ex As Exception
                    Logger.ErrorException("Failed to calculate yield at historical date", ex)
                    Logger.Error("Exception = {0}", ex.ToString())
                End Try
            Next
            GuiAsync(
                Sub()
                    If points.Any() Then
                        Dim id = Guid.NewGuid()
                        Dim theSeries = New Series() With {
                            .Name = bondDataPoint.Label,
                            .legendText = bondDataPoint.Label & " history",
                            .ChartType = SeriesChartType.Line,
                            .color = Color.FromName(elem.Parent.Color),
                            .markerColor = Color.Wheat,
                            .markerBorderColor = Color.FromName(elem.Parent.Color),
                            .borderWidth = 1,
                            .markerStyle = MarkerStyle.Circle,
                            .markerSize = 4,
                            .Tag = id
                        }
                        TheChart.Series.Add(theSeries)
                        points.ForEach(
                            Sub(tpl)
                                Dim point = New DataPoint(tpl.Item1.Duration, tpl.Item1.GetYield.Value) With {
                                                .Tag = New HistoryPointTag With {
                                                    .Ric = bondDataPoint.RIC,
                                                    .Descr = tpl.Item1,
                                                    .Meta = tpl.Item2,
                                                    .SeriesId = id
                                                }
                                    }
                                theSeries.Points.Add(point)
                            End Sub)
                    End If

                End Sub)
        End Sub

        Private Sub OnGroupUpdated(ByVal data As List(Of BondCurve.CurveItem))
            Logger.Trace("OnGroupUpdated()")
            GuiAsync(
                Sub()
                    If Not data.Any Then Return
                    If Not TypeOf data.First Is BondCurve.BondCurveItem Then Return
                    Dim dt = data.Cast(Of BondCurve.BondCurveItem).ToList()
                    Dim group = dt.First.Bond.Parent
                    Dim series As Series = TheChart.Series.FindByName(group.Identity)
                    Dim clr = Color.FromName(group.Color)

                    If series IsNot Nothing Then TheChart.Series.Remove(series)
                    Dim seriesDescr = New BondSetSeries With {.Name = group.SeriesName, .Color = clr}
                    AddHandler seriesDescr.SelectedPointChanged, AddressOf OnSelectedPointChanged
                    series = New Series(group.Identity) With {
                        .YValuesPerPoint = 1,
                        .ChartType = SeriesChartType.Point,
                        .IsVisibleInLegend = False,
                        .color = clr,
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

                    Dim legendItems = TheChart.Legends(0).CustomItems
                    While legendItems.Any(Function(item) item.Tag = group.Identity)
                        legendItems.Remove(legendItems.First(Function(item) item.Tag = group.Identity))
                    End While
                    Dim legendItem = New LegendItem(group.SeriesName, clr, "") With {.Tag = group.Identity}
                    legendItems.Add(legendItem)

                    For Each pnt In dt
                        Dim point = New DataPoint(pnt.TheX, pnt.TheY) With {
                                .Name = pnt.Bond.MetaData.RIC,
                                .Tag = pnt.Bond,
                                .Color = Color.FromName(pnt.BackColor),
                                .Label = pnt.Label
                            }
                        Dim style As MarkerStyle
                        If MarkerStyle.TryParse(pnt.MarkerStyle, style) Then
                            point.MarkerStyle = style
                        Else
                            point.MarkerStyle = IIf(pnt.ToWhat.Equals(YieldToWhat.Maturity), MarkerStyle.Circle, MarkerStyle.Triangle)
                        End If
                        series.Points.Add(point)
                    Next

                    SetChartMinMax()
                End Sub)
        End Sub

        Private Sub OnNewCurvePaint(ByVal data As List(Of BondCurve.CurveItem))
            GuiAsync(
                Sub()
                    If Not data.Any Then Return
                    Dim crv As BondCurve
                    Dim itsBond As Boolean
                    If TypeOf data.First Is BondCurve.BondCurveItem Then
                        crv = CType(CType(data.First, BondCurve.BondCurveItem).Bond.Parent, BondCurve)
                        itsBond = True
                    ElseIf TypeOf data.First Is BondCurve.PointCurveItem Then
                        crv = CType(data.First, BondCurve.PointCurveItem).Curve
                        itsBond = False
                    Else
                        Logger.Warn("Unexpected items type for a bond-based curve")
                        Return
                    End If
                    Dim srs = TheChart.Series.FindByName(crv.Identity)
                    If srs IsNot Nothing Then TheChart.Series.Remove(srs)
                    Dim clr = Color.FromName(crv.Color)
                    srs = New Series() With {
                        .Name = crv.Identity,
                        .ChartType = SeriesChartType.Line,
                        .IsVisibleInLegend = False,
                        .borderWidth = 2,
                        .color = clr,
                        .Tag = crv.Identity
                    }
                    TheChart.Series.Add(srs)
                    Dim legendItems = TheChart.Legends(0).CustomItems
                    While legendItems.Any(Function(item) item.Tag = crv.Identity)
                        legendItems.Remove(legendItems.First(Function(item) item.Tag = crv.Identity))
                    End While
                    Dim legendItem = New LegendItem(crv.SeriesName, clr, "") With {.Tag = crv.Identity}
                    legendItems.Add(legendItem)


                    If itsBond Then
                        For Each point In data.Cast(Of BondCurve.BondCurveItem)()
                            Dim pnt = New DataPoint(point.TheX, point.TheY) With {
                                .Tag = point.Bond,
                                .Label = point.Label
                            }
                            srs.Points.Add(pnt)
                        Next
                    Else
                        For Each point In data.Cast(Of BondCurve.PointCurveItem)()
                            Dim pnt = New DataPoint(point.TheX, point.TheY) With {.Tag = point.Curve}
                            srs.Points.Add(pnt)
                        Next
                    End If
                    SetChartMinMax()
                End Sub)
        End Sub

    End Class
End Namespace