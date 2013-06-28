Imports System.Windows.Forms.DataVisualization.Charting
Imports DbManager
Imports DbManager.Bonds
Imports YieldMap.Tools.Elements
Imports ReutersData
Imports Uitls
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

            If curve.CanBootstrap Then
                BootstrapTSMI.Visible = True
                BootstrapTSMI.Enabled = True
                BootstrapTSMI.Checked = curve.Bootstrapped
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
            If Not _ansamble.Benchmarks.Any Then Return
            YAxisCMS.Items.Clear()
            YAxisCMS.Items.Add(Yield.DescrProperty, Nothing, Sub() _ansamble.YSource = Yield)
            For Each key As IOrdinate In _ansamble.Benchmarks.Keys
                Dim tmp = key
                YAxisCMS.Items.Add(key.DescrProperty, Nothing, Sub() _ansamble.YSource = tmp)
            Next
            YAxisCMS.Show(TheChart, loc)
        End Sub

        Private Sub HideBidAsk()
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
                Dim yValue = _ansamble.YSource.GetValue(calc)
                bidAskSeries.Points.Add(New DataPoint(calc.Duration, yValue.Value))
            End If
            If bond.QuotesAndYields.Has(bond.Fields.Ask) Then
                Dim calc = bond.QuotesAndYields(bond.Fields.Ask)
                Dim yValue = _ansamble.YSource.GetValue(calc)
                bidAskSeries.Points.Add(New DataPoint(calc.Duration, yValue.Value))
            End If

            TheChart.ChartAreas(0).AxisX.Minimum = minX
            TheChart.ChartAreas(0).AxisX.Maximum = maxX
            TheChart.ChartAreas(0).AxisY.Minimum = minY
            TheChart.ChartAreas(0).AxisY.Minimum = maxY
        End Sub

        Private Sub UpdateAxisYTitle(ByVal mouseOver As Boolean)
            Dim axisFont As Font, clr As Color

            If _ansamble.Benchmarks.Any() Then
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

        Private Sub SetYAxisMode(ByVal ord As IOrdinate)
            Try
                Select Case ord
                    Case Yield : MakeAxisY("Yield, %", "P2")
                    Case AswSpread : MakeAxisY("ASW Spread, b.p.", "N0")
                    Case PointSpread : MakeAxisY("Spread, b.p.", "N0")
                    Case ZSpread : MakeAxisY("Z-Spread, b.p.", "N0")
                End Select
                TheChart.ChartAreas(0).AxisX.ScaleView.ZoomReset()
                TheChart.ChartAreas(0).AxisY.ScaleView.ZoomReset()
            Catch ex As Exception
                Logger.WarnException(String.Format("Failed to select y-axis variable {0}", ord), ex)
                Logger.Warn("Exception = {0}", ex.ToString())
            End Try
        End Sub

        ' this method simply updates the chart by selecting appropriate y-coordinates for points
        ' and wut 'bout x?
        Private Sub SetChartMinMax()
            Logger.Debug("SetChartMinMax()")
            GuiAsync(AddressOf DoSetMinMax1)
        End Sub

        Private ReadOnly _screenLock As New Object

        Private Sub DoSetMinMax1()
            SyncLock _screenLock
                Dim minMinYStrict As Boolean = _theSettings.MinYStrict
                Dim maxMaxYStrict As Boolean = _theSettings.MaxYStrict
                Dim minMinXStrict As Boolean = _theSettings.MinXStrict
                Dim maxMaxXStrict As Boolean = _theSettings.MaxXStrict

                Dim minMinY As Double?, maxMaxY As Double?
                Dim minMinX As Double?, maxMaxX As Double?
                If _ansamble.YSource = Yield Then
                    minMinY = _theSettings.MinYield / 100
                    maxMaxY = _theSettings.MaxYield / 100
                Else
                    minMinY = _theSettings.MinSpread
                    maxMaxY = _theSettings.MaxSpread
                End If
                minMinX = _theSettings.MinDur
                maxMaxX = _theSettings.MaxDur


                If minMinYStrict And maxMaxYStrict And minMinY.HasValue And maxMaxY.HasValue Then
                    TheChart.ChartAreas(0).AxisY.Minimum = minMinY.Value
                    TheChart.ChartAreas(0).AxisY.Maximum = maxMaxY.Value
                Else
                    Dim minMaxY = CalcMinMax(Function(pnt) If(pnt.YValues.Any, pnt.YValues.First, Nothing))
                    If minMaxY IsNot Nothing Then
                        Dim pow = If(_ansamble.YSource = Yield, 3, 0)
                        Dim theMinY As Double
                        If minMinYStrict Then
                            theMinY = minMinY
                        Else
                            theMinY = If(minMinY.HasValue, Math.Max(minMinY.Value, minMaxY.Item1), minMaxY.Item1)
                        End If
                        theMinY = Math.Floor(theMinY * (10 ^ pow)) / (10 ^ pow)

                        Dim theMaxY As Double
                        If maxMaxYStrict Then
                            theMaxY = maxMaxY
                        Else
                            theMaxY = If(maxMaxY.HasValue, Math.Min(maxMaxY.Value, minMaxY.Item2), minMaxY.Item2)
                        End If
                        theMaxY = Math.Ceiling(theMaxY * (10 ^ pow)) / (10 ^ pow)

                        If theMaxY > theMinY Then
                            Logger.Trace(String.Format("Y: {0:N2} -> {1:N2}", theMinY, theMaxY))
                            TheChart.ChartAreas(0).AxisY.Minimum = theMinY
                            TheChart.ChartAreas(0).AxisY.Maximum = theMaxY
                            TheChart.ChartAreas(0).AxisY.MajorGrid.Interval = (theMaxY - theMinY) / 5
                            TheChart.ChartAreas(0).AxisY.MinorGrid.Interval = (theMaxY - theMinY) / 10
                        Else
                            Logger.Warn("Min Y > Max Y")
                        End If
                    Else
                        Logger.Warn("Failed to determine min/max Y")
                    End If
                End If

                If minMinXStrict And maxMaxXStrict And minMinX.HasValue And maxMaxX.HasValue Then
                    TheChart.ChartAreas(0).AxisY.Minimum = minMinX.Value
                    TheChart.ChartAreas(0).AxisY.Maximum = maxMaxX.Value
                Else
                    Dim minmaxX = CalcMinMax(Function(pnt) pnt.XValue)
                    If minmaxX IsNot Nothing Then
                        Dim theMinX As Double
                        If minMinXStrict Then
                            theMinX = minMinX
                        Else
                            theMinX = If(minMinX.HasValue, Math.Max(minMinX.Value, minmaxX.Item1), minmaxX.Item1)
                        End If
                        theMinX = Math.Floor(theMinX)

                        Dim theMaxX As Double
                        If maxMaxXStrict Then
                            theMaxX = maxMaxX
                        Else
                            theMaxX = If(maxMaxX.HasValue AndAlso maxMaxX.Value > 0, Math.Min(maxMaxX.Value, minmaxX.Item2), minmaxX.Item2)
                        End If
                        theMaxX = Math.Ceiling(theMaxX)

                        If theMaxX > theMinX Then
                            Logger.Trace(String.Format("Y: {0:N2} -> {1:N2}", theMinX, theMaxX))
                            TheChart.ChartAreas(0).AxisX.Minimum = theMinX
                            TheChart.ChartAreas(0).AxisX.Maximum = theMaxX
                            TheChart.ChartAreas(0).AxisX.MajorGrid.Interval = (theMaxX - theMinX) / 5
                            TheChart.ChartAreas(0).AxisX.MinorGrid.Interval = (theMaxX - theMinX) / 10
                        Else
                            Logger.Warn("Min X > Max X")
                        End If
                    Else
                        Logger.Warn("Failed to determine min/max X")
                    End If
                End If
            End SyncLock
        End Sub

        Private Function CalcMinMax(getter As Func(Of DataPoint, Double?)) As Tuple(Of Double, Double)
            Try
                Dim lstMax = From srs In TheChart.Series
                                Where srs.Enabled And srs.Points.Any
                                Select (From pnt In srs.Points
                                        Let vl = getter(pnt)
                                        Where vl.HasValue
                                        Select vl.Value).Max

                If Not lstMax.Any Then
                    Logger.Warn("Nothing to use as maximum")
                    Return Nothing
                End If

                Dim lstMin = From srs In TheChart.Series
                                Where srs.Enabled And srs.Points.Any
                                Select (From pnt In srs.Points
                                        Where pnt.YValues.Any And (_ansamble.YSource <> Yield OrElse pnt.YValues.First > 0)
                                        Let vl = getter(pnt)
                                        Where vl.HasValue
                                        Select vl.Value).Min


                If Not lstMin.Any Then
                    Logger.Warn("Nothing to use as minimum")
                    Return Nothing
                End If

                Return tuple.Create(lstMin.Min, lstMax.Max)
            Catch ex As Exception
                Logger.WarnException("Failed to set minmax", ex)
                Logger.Warn("Exception = {0}", ex.ToString())
            End Try
            Return Nothing
        End Function

        Private Sub MakeAxisY(ByVal title As String, ByVal format As String)
            With TheChart.ChartAreas(0).AxisY
                .Title = title
                .LabelStyle.Format = format
            End With
        End Sub

        Private Sub ShowCurveCMS(ByVal nm As String, ByVal refCurve As ICurve)
            SpreadCMS.Items.Clear()
            SpreadCMS.Tag = nm
            For Each item As SwapCurve In _ansamble.SwapCurves
                Dim elem = CType(SpreadCMS.Items.Add(item.Name, Nothing, AddressOf OnBenchmarkSelected), ToolStripMenuItem)
                elem.CheckOnClick = True
                elem.Checked = refCurve IsNot Nothing AndAlso item.Identity = CType(refCurve, Identifyable).Identity
                elem.Tag = item
            Next

            Dim items = (From elem In _ansamble.Items Where TypeOf elem.Value Is BondCurve Select elem.Value).ToList()
            If SpreadCMS.Items.Count > 0 And items.Any Then SpreadCMS.Items.Add(New ToolStripSeparator)
            For Each item In items
                Dim elem = CType(SpreadCMS.Items.Add(item.Name, Nothing, AddressOf OnBenchmarkSelected), ToolStripMenuItem)
                elem.CheckOnClick = True
                elem.Checked = refCurve IsNot Nothing AndAlso item.Identity = CType(refCurve, Identifyable).Identity
                elem.Tag = item
            Next item
            SpreadCMS.Show(MousePosition)
        End Sub

        Private Sub OnBenchmarkSelected(ByVal sender As Object, ByVal eventArgs As EventArgs)
            Logger.Info("OnBenchmarkSelected()")
            Dim senderTSMI = TryCast(sender, ToolStripMenuItem)
            If senderTSMI Is Nothing Then Return

            If senderTSMI.Checked Then
                _ansamble.Benchmarks.Put(FromString(SpreadCMS.Tag), senderTSMI.Tag)
            Else
                _ansamble.Benchmarks.Clear(FromString(SpreadCMS.Tag))
            End If

            UpdateAxisYTitle(False)

            ASWLabel.Text = If(_ansamble.Benchmarks.HasOrd(AswSpread), " -> " + _ansamble.Benchmarks(AswSpread).Name, "")
            SpreadLabel.Text = If(_ansamble.Benchmarks.HasOrd(PointSpread), " -> " + _ansamble.Benchmarks(PointSpread).Name, "")
            ZSpreadLabel.Text = If(_ansamble.Benchmarks.HasOrd(ZSpread), " -> " + _ansamble.Benchmarks(ZSpread).Name, "")
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

        ' todo that's awful
        Public Sub OnHistoricalData(ByVal ric As String, ByVal data As Dictionary(Of Date, HistoricalItem), ByVal rawData As Dictionary(Of DateTime, RawHistoricalItem))
            Logger.Trace("OnHistoricalQuotes({0})", ric)
            If _ansamble.YSource <> Yield Then Return

            If data Is Nothing OrElse data.Count <= 0 Then
                Logger.Info("No data on {0} arrived", ric)
                Return
            End If

            Dim elem = _ansamble.Items.Bonds(Function(m) m.RIC = ric).First()
            Dim bondDataPoint = elem.MetaData
            Dim points As New List(Of Tuple(Of BondPointDescription, BondMetadata))
            For Each dt In data.Keys
                Try
                    Dim calc As New BondPointDescription("HST")
                    calc.ParentBond = elem
                    If data(dt).Close > 0 Then
                        calc.Yield(dt) = data(dt).Close
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
                        For Each tpl In points
                            Dim point = New DataPoint(tpl.Item1.Duration, tpl.Item1.Yield.Value) With {
                                            .Tag = New HistoryPointTag With {
                                                .Ric = bondDataPoint.RIC,
                                                .Descr = tpl.Item1,
                                                .Meta = tpl.Item2,
                                                .SeriesId = id
                                            }
                                }
                            theSeries.Points.Add(point)
                        Next
                    End If

                End Sub)
        End Sub

        Private Sub OnGroupUpdated(ByVal group As Group, ByVal data As List(Of PointOfCurve))
            Logger.Trace("OnGroupUpdated()")
            GuiAsync(
                Sub()
                    If Not data.Any Then
                        ClearSeries(group.Identity)
                        Return
                    End If
                    If Not TypeOf data.First Is PointOfBondCurve Then Return
                    Dim dt = data.Cast(Of PointOfBondCurve).ToList()
                    Dim series As Series = TheChart.Series.FindByName(group.Identity)
                    Dim clr = Color.FromName(group.Color)

                    If series IsNot Nothing Then TheChart.Series.Remove(series)
                    Dim seriesDescr = New BondSetSeries With {.Name = group.Name, .Color = clr}
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

                    ClearLegendItems(group.Identity)
                    TheChart.Legends(0).CustomItems.Add(New LegendItem(group.Name, clr, "") With {.Tag = group.Identity})

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

        Private Sub OnSwapCurvePaint(ByVal data As List(Of PointOfCurve), ByVal crv As SwapCurve)
            GuiAsync(
                Sub()
                    Dim srs = TheChart.Series.FindByName(crv.Identity)
                    If srs IsNot Nothing Then
                        srs.Points.Clear()
                        TheChart.Series.Remove(srs)
                    End If
                    srs = New Series() With {
                        .Name = crv.Identity,
                        .ChartType = SeriesChartType.Line,
                        .IsVisibleInLegend = False,
                        .borderWidth = 2,
                        .color = crv.OuterColor,
                        .Tag = crv.Identity,
                        .markerColor = crv.InnerColor,
                        .markerStyle = MarkerStyle.Circle,
                        .markerSize = 4
                    }
                    TheChart.Series.Add(srs)
                    ClearLegendItems(crv.Identity)
                    TheChart.Legends(0).CustomItems.Add(New LegendItem(crv.Name, crv.OuterColor, "") With {.Tag = crv.Identity})

                    For Each pnt In From point In data Select New DataPoint(point.TheX, point.TheY) With {.Tag = crv}
                        srs.Points.Add(pnt)
                    Next
                    SetChartMinMax()
                End Sub)
        End Sub

        Private Sub OnNewCurvePaint(ByVal data As List(Of PointOfCurve))
            GuiAsync(
                Sub()
                    If Not data.Any Then Return
                    Dim crv As BondCurve
                    Dim itsBond As Boolean
                    If TypeOf data.First Is PointOfBondCurve Then
                        crv = CType(CType(data.First, PointOfBondCurve).Bond.Parent, BondCurve)
                        itsBond = True
                    ElseIf TypeOf data.First Is JustPoint Then
                        crv = CType(data.First, JustPoint).Curve
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
                    ClearLegendItems(crv.Identity)
                    TheChart.Legends(0).CustomItems.Add(New LegendItem(crv.Name, clr, "") With {.Tag = crv.Identity})

                    If itsBond Then
                        For Each point In data.Cast(Of PointOfBondCurve)()
                            Dim pnt = New DataPoint(point.TheX, point.TheY) With {
                                .Tag = point.Bond,
                                .Label = point.Label
                            }
                            srs.Points.Add(pnt)
                        Next
                    Else
                        For Each point In data.Cast(Of JustPoint)()
                            Dim pnt = New DataPoint(point.TheX, point.TheY) With {.Tag = point.Curve}
                            srs.Points.Add(pnt)
                        Next
                    End If
                    SetChartMinMax()
                End Sub)
        End Sub

        Private Sub OnChainCurvePaint(ByVal data As List(Of PointOfCurve))
            GuiAsync(
                Sub()
                    If Not data.Any Then Return
                    Dim crv As ChainCurve
                    Dim itsBond As Boolean
                    If TypeOf data.First Is PointOfBondCurve Then
                        crv = CType(CType(data.First, PointOfBondCurve).Bond.Parent, ChainCurve)
                        itsBond = True
                    ElseIf TypeOf data.First Is JustPoint Then
                        crv = CType(data.First, JustPoint).Curve
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
                    ClearLegendItems(crv.Identity)
                    TheChart.Legends(0).CustomItems.Add(New LegendItem(crv.Name, clr, "") With {.Tag = crv.Identity})

                    If itsBond Then
                        For Each point In data.Cast(Of PointOfBondCurve)()
                            Dim pnt = New DataPoint(point.TheX, point.TheY) With {
                                .Tag = point.Bond,
                                .Label = point.Label
                            }
                            srs.Points.Add(pnt)
                        Next
                    Else
                        For Each point In data.Cast(Of JustPoint)()
                            Dim pnt = New DataPoint(point.TheX, point.TheY) With {.Tag = point.Curve}
                            srs.Points.Add(pnt)
                        Next
                    End If
                    SetChartMinMax()
                End Sub)
        End Sub

        Private Sub ClearLegendItems(ByVal id As Long)
            Dim legendItems = TheChart.Legends(0).CustomItems
            While legendItems.Any(Function(item) item.Tag = id)
                legendItems.Remove(legendItems.First(Function(item) item.Tag = id))
            End While
        End Sub
    End Class
End Namespace