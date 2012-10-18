Imports System.Data
Imports System.Windows.Forms
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Drawing
Imports AdfinXAnalyticsFunctions
Imports AdfinXRtLib
Imports System.Text.RegularExpressions
Imports System.Drawing.Drawing2D
Imports YieldMap.Forms.TableForm
Imports YieldMap.Curves
Imports YieldMap.My.Resources
Imports YieldMap.Tools.Lists
Imports YieldMap.Commons
Imports YieldMap.Tools
Imports YieldMap.Tools.History
Imports YieldMap.BondsDataSetTableAdapters
Imports NLog
Imports YieldMap.Tools.Estimation

Namespace Forms.ChartForm
    Public Class GraphForm
        Private Shared ReadOnly Logger As Logger = GetLogger(GetType(GraphForm))
        Private WithEvents _tableForm As TableForm.TableForm = New TableForm.TableForm()

        Private WithEvents _quoteLoader As ListLoadManager
        Private ReadOnly _historyLoaders As New Dictionary(Of String, HistoryLoadManager)

        Private ReadOnly _historicalCurves As New HistoricalCurvesContainer(AddressOf OnHistoricalCurveData, AddressOf OnCurveRemoved)
        Private ReadOnly _moneyMarketCurves As New List(Of SwapCurve)
        Private ReadOnly _bidAskLines As New List(Of Tuple(Of DataPoint, DataPoint))

        Private ReadOnly _ansamble As New VisualizableAnsamble
        Private WithEvents _spreadBenchmarks As New SpreadContainer
        Private _currentTasks As List(Of TaskDescription)

        Public Delegate Sub PointUpdateDelegate(ByVal ric As String, ByVal yield As Double, ByVal duration As Double, ByVal lastPrice As Double)
        Public Event PointUpdated As PointUpdateDelegate

        Private Sub GuiAsync(ByVal action As Action)
            If action IsNot Nothing Then
                If InvokeRequired Then
                    Invoke(action)
                Else
                    action()
                End If
            End If
        End Sub

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
                        _currentTasks.ForEach(Sub(task) _quoteLoader.DiscardTask(task.Name))
                        _currentTasks.Clear()
                        _ansamble.Cleanup()

                        _historicalCurves.Clear()
                        _historyLoaders.Keys.ToList.ForEach(
                            Sub(key)
                                Logger.Info("Was still waiting for history on {0}", key)
                                _historyLoaders(key).StopTask()
                                RemoveHandler _historyLoaders(key).NewData, AddressOf OnHistoricalQuotes
                            End Sub)
                        _historyLoaders.Clear()
                        _moneyMarketCurves.ForEach(Sub(curve) curve.Cleanup())
                        _moneyMarketCurves.Clear()
                        _bidAskLines.Clear()

                        TheChart.Series.Clear()

                        StatusMessage.Text = "Stopped"
                End Select
            End Set
        End Property

        Private Sub OnSpreadModeSelected(ByVal newMode As SpreadMode, ByVal oldMode As SpreadMode) Handles _spreadBenchmarks.ModeSelected
            Logger.Trace("OnSpreadModeSelected({0}, {1})", newMode, oldMode)
            ReplotSpread(newMode, oldMode)
        End Sub

        Private Sub OnSpreadModeUpdated(ByVal mode As SpreadMode, ByVal currentMode As SpreadMode) Handles _spreadBenchmarks.SpreadUpdated
            Logger.Trace("OnSpreadModeUpdated({0})", mode)
            If mode.Equals(currentMode) Then
                ReplotSpread(mode, currentMode)
            Else
                RecalcSpread(mode)
            End If
        End Sub

        Private Sub RecalcSpread(ByVal mode As SpreadMode)
            Logger.Trace("RecalcSpread({0})", mode)
            TheChart.Series.ToList.ForEach(
                Sub(srs)
                    If TypeOf srs.Tag Is BondPointsSeries Then 'todo history / bidask
                        srs.Points.ToList.ForEach(Sub(pnt) RecalcPoint(pnt, mode))
                        'ElseIf TypeOf srs.Tag Is SwapCurveSeries Then
                        '    Dim crvSrs = CType(srs.Tag, SwapCurveSeries)
                        '    Dim crv = crvSrs.SwpCurve
                        '    crv.SetModeAndBenchmark(mode, If(mode <> SpreadMode.Yield, _spreadBenchmarks.Benchmarks(mode), Nothing))
                    End If
                End Sub
            )
        End Sub

        Private Sub ReplotSpread(ByVal newMode As SpreadMode, ByVal oldMode As SpreadMode)
            Logger.Trace("ReplotSpread({0}, {1})", newMode, oldMode)
            Dim storeOld = oldMode.Equals(SpreadMode.Yield) OrElse _spreadBenchmarks.Benchmarks.ContainsKey(oldMode)
            TheChart.Series.ToList.ForEach(
                Sub(srs)
                    If TypeOf srs.Tag Is BondPointsSeries Then 'todo history / bidask
                        srs.Points.ToList.ForEach(Sub(pnt) ReplotPoint(pnt, newMode, oldMode, storeOld))
                    ElseIf TypeOf srs.Tag Is SwapCurveSeries Then
                        Dim crvSrs = CType(srs.Tag, SwapCurveSeries)
                        Dim crv = crvSrs.SwpCurve
                        'crv.SetBenchmark()
                        crv.SetModeAndBenchmark(newMode, If(newMode <> SpreadMode.Yield, _spreadBenchmarks.Benchmarks(newMode), Nothing))
                    End If
                End Sub
            )
            SetYAxisMode(newMode.ToString())
        End Sub

        Private Sub RecalcPoint(ByVal pnt As DataPoint, ByVal newMode As SpreadMode)
            Dim descr As DataPointDescr = TryCast(pnt.Tag, DataPointDescr)
            If descr IsNot Nothing Then _spreadBenchmarks.CalculateSpreads(descr, newMode)
        End Sub

        Private Sub ReplotPoint(ByVal pnt As DataPoint, newMode As SpreadMode, oldMode As SpreadMode, storeOld As Boolean)
            Dim descr As DataPointDescr = TryCast(pnt.Tag, DataPointDescr)
            If descr IsNot Nothing Then
                Dim spread = _spreadBenchmarks.CalculateSpreads(descr, newMode)
                If spread.HasValue Then
                    Logger.Trace("{0} -> {1}: {2}", pnt.YValues.First, spread, pnt.YValues.First - spread)
                    pnt.YValues = {spread}
                    pnt.IsEmpty = False
                Else
                    pnt.YValues = {0}
                    pnt.IsEmpty = True
                End If
            Else
                pnt.YValues = {0}
                pnt.IsEmpty = True
            End If
            If Not storeOld Then _spreadBenchmarks.SetQuote(descr, Nothing, oldMode)
        End Sub
#End Region

#Region "III) General GUI Actions and startup"
#Region "a) Begin and end"
        Private Sub GraphFormLoad(sender As Object, e As EventArgs) Handles MyBase.Load
            Logger.Trace("GraphFormLoad")
            ThisFormStatus = FormDataStatus.Loading


            ZoomCustomButton.CheckOnClick = True


            Dim axisFont = New Font(FontFamily.GenericSansSerif, 11)
            'Dim headingFont = New Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold)

            'TheChart.Titles.Add(New Title("Bond Yield Map", Docking.Top, headingFont, Color.Black))

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

            Dim series As Series = New Series("start") With {.ChartType = SeriesChartType.Point}
            series.Points.AddXY(1, 0.1)
            TheChart.Series.Add(series)

            ThisFormDataSource = -1
            DoSetRunning()
        End Sub

        Private Sub GraphFormFormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
            Logger.Trace("GraphForm_FormClosing")
            ThisFormStatus = FormDataStatus.Stopped
        End Sub
#End Region

#Region "b) Start and stop updates"
        Private Sub DoSetRunning()
            Logger.Trace("DoStart")
            _quoteLoader = New ListLoadManager()
            _currentTasks.ForEach(
                Sub(task)
                    Dim list = New List(Of String)
                    list.Add(task.Field.ToUpper())
                    If (_quoteLoader.StartNewTask(New ListTaskDescr() With {
                            .Name = task.Name,
                            .Items = task.RICs,
                            .Fields = list})) Then
                        ThisFormStatus = FormDataStatus.Running
                    Else
                        ThisFormStatus = FormDataStatus.Stopped
                        StatusMessage.Text = "Failed to load the portfolio"
                    End If
                End Sub)
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
                        If TypeOf point.Tag Is BondPointDescr Then
                            Dim bondDataPoint = CType(point.Tag, BondPointDescr)
                            With BondCMS
                                .Tag = bondDataPoint.RIC
                                .Show(TheChart, mouseEvent.Location)
                            End With
                            MainInfoLine1TSMI.Text = point.ToolTip
                            If bondDataPoint.YieldSource = YieldSource.Realtime Then
                                MainInfoLine2TSMI.Text =
                                    String.Format("LAST: P [{0:F4}], Y [{1:P2}] {2}, D [{3:F2}]",
                                                  bondDataPoint.CalcPrice, bondDataPoint.Yld.Yield, bondDataPoint.Yld.ToWhat.ToString(), bondDataPoint.Duration)
                                StartBidAsk(bondDataPoint)
                                ExtInfoTSMI.Visible = True
                            Else
                                MainInfoLine2TSMI.Text =
                                    String.Format("LAST: P [{0:F4}], Y [{1:P2}] {2}, D [{3:F2}] @ {4:dd/MM/yy}",
                                                  bondDataPoint.CalcPrice, bondDataPoint.Yld.Yield, bondDataPoint.Yld.ToWhat.ToString(), bondDataPoint.Duration, bondDataPoint.YieldAtDate)

                                ExtInfoTSMI.Visible = False
                            End If

                        ElseIf TypeOf point.Tag Is MoneyMarketPointDescr Then
                            Dim curveDataPoint = CType(point.Tag, MoneyMarketPointDescr)
                            MMNameTSMI.Text = curveDataPoint.SwpCurve.GetFullName()
                            MoneyCurveCMS.Show(TheChart, mouseEvent.Location)
                            MoneyCurveCMS.Tag = curveDataPoint.YieldCurveName
                            ShowCurveParameters(curveDataPoint)

                        ElseIf TypeOf point.Tag Is HistCurvePointDescr Then
                            Dim histDataPoint = CType(point.Tag, HistCurvePointDescr)
                            HistoryCMS.Tag = histDataPoint.RIC
                            HistoryCMS.Show(TheChart, mouseEvent.Location)

                        ElseIf TypeOf point.Tag Is BidAskPointDescr Then
                            Dim bidAskDataPoint = CType(point.Tag, BidAskPointDescr)
                            BidAskCMS.Tag = bidAskDataPoint.BondTag.RIC
                            BidAskCMS.Show(TheChart, mouseEvent.Location)
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
            _quoteLoader.DiscardTask(BondCMS.Tag)
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
                    Case SpreadMode.Yield.ToString() : MakeAxisY("Yield, %", "P2", 0.01, 0.005)
                    Case SpreadMode.ASWSpread.ToString() : MakeAxisY("ASW Spread, b.p.", "N0", 10, 5)
                    Case SpreadMode.PointSpread.ToString() : MakeAxisY("Spread, b.p.", "N0", 10, 5)
                    Case SpreadMode.ZSpread.ToString() : MakeAxisY("Z-Spread, b.p.", "N0", 10, 5)
                End Select
                'SetChartMinMax()
            Catch ex As Exception
                Logger.WarnException("Failed to select y-axis variable " + str, ex)
                Logger.Warn("Exception = {0}", ex.ToString())
            End Try
        End Sub

        ' these methods simply update the chart by selecting appropriate y-coordinates for points
        Private Sub SetChartMinMax()
            Logger.Debug("SetChartMinMax()")
            GuiAsync(Sub()
                         With TheChart.ChartAreas(0).AxisY
                             Try
                                 Dim newMax = (
                                     From srs In TheChart.Series Where srs.Points.Any
                                     Select (From pnt In srs.Points Select pnt.YValues.First).Max).Max 'Where Not pnt.IsEmpty

                                 Dim newMin As Double
                                 If _spreadBenchmarks.CurrentMode.Equals(SpreadMode.Yield) Then
                                     newMin = (
                                         From srs In TheChart.Series Where srs.Points.Any
                                             Select (From pnt In srs.Points Where pnt.YValues.First > 0 Select pnt.YValues.First).Min).Min
                                 Else
                                     newMin = (
                                           From srs In TheChart.Series Where srs.Points.Any
                                               Select (From pnt In srs.Points Select pnt.YValues.First).Min).Min
                                 End If

                                 'Where Not pnt.IsEmpty

                                 If newMax > newMin Then
                                     .Maximum = newMax
                                     .Minimum = newMin
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

        Private Sub MakeAxisY(ByVal title As String, ByVal format As String, ByVal stepBig As Double,
                              ByVal stepSmall As Double)
            With TheChart.ChartAreas(0).AxisY
                .Title = title
                .LabelStyle.Format = format
                .MajorGrid.Interval = stepBig
                .MinorGrid.Interval = stepSmall
            End With
        End Sub
#End Region

#Region "c.3) Other actions"
        Private Sub ShowLabelsTSBClick(sender As Object, e As EventArgs) Handles ShowLabelsTSB.Click
            Logger.Trace("ShowLabelsTSBClick")
            For Each series As Series In TheChart.Series
                Logger.Trace(" -> series {0}", series.Name)
                series.SmartLabelStyle.Enabled = True
                series.SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.No

                For Each dataPoint As DataPoint In series.Points
                    If ShowLabelsTSB.Checked AndAlso TypeOf dataPoint.Tag Is BondPointDescr Then
                        Dim data = CType(dataPoint.Tag, BondPointDescr)
                        If data.IsValid And data.IsVisible Then
                            dataPoint.Label = GetWin1251String(data.Label)
                            Logger.Trace(" ---> adding label {0}", dataPoint.Label)
                        Else
                            Logger.Trace(" ---> removing label...")
                            dataPoint.Label = ""
                        End If
                    Else
                        Logger.Trace(" ---> removing label...")
                        dataPoint.Label = ""
                    End If
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
                _spreadBenchmarks.CleanupCurve(CType(senderTSMI.Tag, ICurve).GetName())
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

                    If TypeOf point.Tag Is BondPointDescr And Not point.IsEmpty Then
                        Dim bondData = CType(point.Tag, BondPointDescr)
                        DscrLabel.Text = bondData.Label
                        ConvLabel.Text = String.Format("{0:F2}", bondData.Convexity)
                        YldLabel.Text = String.Format("{0:P2} {1}", bondData.Yld.Yield, bondData.Yld.ToWhat.Abbr)
                        DatLabel.Text = String.Format("{0:dd/MM/yyyy}", bondData.YieldAtDate)
                        MatLabel.Text = String.Format("{0:dd/MM/yyyy}", bondData.Maturity)
                        CpnLabel.Text = String.Format("{0:F2}%", bondData.Coupon)
                        PVBPLabel.Text = String.Format("{0:F4}", bondData.PVBP)
                        CType(htr.Series.Tag, BondPointsSeries).SelectedPointIndex = htr.PointIndex

                    ElseIf TypeOf point.Tag Is MoneyMarketPointDescr Then
                        Dim data = CType(point.Tag, MoneyMarketPointDescr)
                        DscrLabel.Text = data.FullName ' .SwpCurve.GetType.Name
                        DatLabel.Text = String.Format("{0:dd/MM/yyyy}", data.SwpCurve.GetDate())
                        Dim period = String.Format("{0:F0}D", 365 * data.Duration)
                        Dim aDate = (New AdxDateModule).DfAddPeriod("RUS", Date.Today, period, "")
                        MatLabel.Text = String.Format("{0:dd/MM/yyyy}", FromExcelSerialDate(aDate.GetValue(1, 1)))

                    ElseIf TypeOf point.Tag Is HistCurvePointDescr Then
                        Dim historyDataPoint = CType(point.Tag, HistCurvePointDescr)
                        DscrLabel.Text = historyDataPoint.BaseBondName
                        DatLabel.Text = String.Format("{0:dd/MM/yyyy}", historyDataPoint.YieldAtDate)
                        ConvLabel.Text = String.Format("{0:F2}", historyDataPoint.Convexity)

                    ElseIf TypeOf point.Tag Is BidAskPointDescr Then
                        Dim bidAskDP = CType(point.Tag, BidAskPointDescr)
                        ConvLabel.Text = String.Format("{0:F2}", bidAskDP.Convexity)
                        DscrLabel.Text = bidAskDP.BondTag.Label + " " + bidAskDP.BidAsk
                        DatLabel.Text = String.Format("{0:dd/MM/yyyy}", Date.Today)
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
                    If seriesDescr IsNot Nothing Then
                        seriesDescr.ResetSelection()
                        srs.Points.ToList.ForEach(
                            Sub(point)
                                Dim bondDescr As BondPointDescr
                                bondDescr = TryCast(point.Tag, BondPointDescr)
                                If bondDescr IsNot Nothing And Not point.IsEmpty Then
                                    point.Color = IIf(bondDescr.YieldSource = YieldSource.Realtime, seriesDescr.Color, Color.Gray)
                                End If
                            End Sub)
                        If seriesDescr.Name = curveName AndAlso pointIndex IsNot Nothing Then
                            srs.Points(pointIndex).Color = Color.Red
                        End If
                    End If
                End Sub)
        End Sub

        Private Sub ResetPointSelection()
            TheChart.Series.ToList.ForEach(
                Sub(srs)
                    Dim seriesDescr = TryCast(srs.Tag, BondPointsSeries)
                    If seriesDescr IsNot Nothing Then
                        seriesDescr.ResetSelection()
                        srs.Points.ToList.ForEach(
                            Sub(point)
                                Dim bondDescr As BondPointDescr
                                bondDescr = TryCast(point.Tag, BondPointDescr)
                                If bondDescr IsNot Nothing And Not point.IsEmpty Then
                                    point.Color = IIf(bondDescr.YieldSource = YieldSource.Realtime, seriesDescr.Color, Color.Gray)
                                End If
                            End Sub)
                    End If
                End Sub)
        End Sub

        Private Sub RelatedQuoteTSMIClick(sender As Object, e As EventArgs) Handles RelatedQuoteTSMI.Click
            If _ansamble.ContainsRIC(BondCMS.Tag.ToString()) Then Process.Start("reuters://REALTIME/verb=FullQuote/ric=" + BondCMS.Tag.ToString())
        End Sub

        Private Sub BondDescriptionTSMIClick(sender As Object, e As EventArgs) Handles BondDescriptionTSMI.Click
            If _ansamble.ContainsRIC(BondCMS.Tag.ToString()) Then Process.Start("reuters://REALTIME/verb=BondData/ric=" + BondCMS.Tag.ToString())

        End Sub

        Private Sub RelatedChartTSMIClick(sender As Object, e As EventArgs) Handles RelatedChartTSMI.Click
            If _ansamble.ContainsRIC(BondCMS.Tag.ToString()) Then Process.Start("reuters://REALTIME/verb=RelatedGraph/ric=" + BondCMS.Tag.ToString())
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
                StatusMessage.Text = "Finished cancelled"
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
            TheChart.ChartAreas(0).AxisX.ScaleView.ZoomReset()
            TheChart.ChartAreas(0).AxisY.ScaleView.ZoomReset()
        End Sub
#End Region

#Region "e) Show as table"
        Private Sub AsTableTSBClick(sender As Object, e As EventArgs) Handles AsTableTSB.Click
            Dim bondsToShow = _ansamble.Groups.SelectMany(
                Function(grp)
                    Return grp.Elements.Keys.ToList.Select(
                        Function(key)
                            Dim res As New BondDescr
                            Dim point = grp.Elements(key)
                            If point.Yld.ToWhat Is Nothing Then Return Nothing

                            res.RIC = point.RIC
                            res.Name = point.ShortName
                            res.Price = point.CalcPrice
                            res.Quote = IIf(point.YieldAtDate <> Date.Today, QuoteType.Close, QuoteType.Last)
                            res.QuoteDate = point.YieldAtDate
                            res.State = BondDescr.StateType.Ok
                            res.ToWhat = point.Yld.ToWhat
                            res.BondYield = point.Yld.Yield
                            res.CalcMode = BondDescr.CalculationMode.SystemPrice
                            res.Convexity = point.Convexity
                            res.Duration = point.Duration
                            res.Live = point.YieldAtDate = Date.Today
                            res.Maturity = point.Maturity
                            res.Coupon = point.Coupon
                            Return res
                        End Function)
                End Function).Where(Function(elem) elem IsNot Nothing).ToList()
            _tableForm.Bonds = bondsToShow
            _tableForm.ShowDialog()
        End Sub
#End Region

#Region "f) Show bid and ask"
        Private Sub StartBidAsk(ByVal bondDataPoint As BondPointDescr, Optional toShow As Boolean = False)
            _quoteLoader.StartNewTask(
                New ListTaskDescr() With {
                    .Name = bondDataPoint.RIC + IIf(toShow, "_BidAsk", ""),
                    .Items = {bondDataPoint.RIC}.ToList,
                    .Fields = {"BID", "ASK"}.ToList
                })
        End Sub

        Private Sub ShowBidAsk(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim point = TheChart.Series.FindByName(_ansamble.GetSeriesName(BondCMS.Tag)).Points.First(Function(pnt) CType(pnt.Tag, BondPointDescr).RIC = BondCMS.Tag)
                StartBidAsk(CType(point.Tag, BondPointDescr), True)
            Catch ex As Exception
                Logger.ErrorException("Failed to do bid-ask request", ex)
                Logger.Error("Exception = {0}", ex.ToString())
            End Try
        End Sub

        Private Sub RemoveBidAskTSMIClick(sender As Object, e As EventArgs) Handles RemoveBidAskTSMI.Click
            Try
                Dim theName As String = BidAskCMS.Tag + "_BidAsk"
                Dim theSeries As Series = TheChart.Series.FindByName(theName)
                _quoteLoader.DiscardTask(theName)
                TheChart.Series.Remove(theSeries)
                _bidAskLines.RemoveAll(Function(tup) CType(tup.Item1.Tag, BondPointDescr).RIC = BidAskCMS.Tag)
                TheChart.Invalidate()
            Catch ex As Exception
                Logger.WarnException("Failed to hide bid-ask points", ex)
                Logger.Warn("Exception = {0}", ex.ToString())
            End Try
        End Sub

        Private Sub TheChartPaint(sender As Object, e As PaintEventArgs) Handles TheChart.Paint
            Dim minX As Double, maxX As Double, minY As Double, maxY As Double
            If Not _bidAskLines.Any() Then Return

            With TheChart.ChartAreas(0)
                minX = .AxisX.ValueToPixelPosition(.AxisX.ScaleView.ViewMinimum)
                minY = .AxisY.ValueToPixelPosition(.AxisY.ScaleView.ViewMaximum)
                maxX = .AxisX.ValueToPixelPosition(.AxisX.ScaleView.ViewMaximum)
                maxY = .AxisY.ValueToPixelPosition(.AxisY.ScaleView.ViewMinimum)
            End With

            For Each bidAskLine As Tuple(Of DataPoint, DataPoint) In _bidAskLines
                Dim point1 As New Point, point2 As New Point
                With TheChart.ChartAreas(0)
                    point1.X = .AxisX.ValueToPixelPosition(bidAskLine.Item1.XValue)
                    point1.Y = .AxisY.ValueToPixelPosition(bidAskLine.Item1.YValues.First)
                    point2.X = .AxisX.ValueToPixelPosition(bidAskLine.Item2.XValue)
                    point2.Y = .AxisY.ValueToPixelPosition(bidAskLine.Item2.YValues.First)
                End With

                If point1.X < minX And point2.X < minX Then Continue For
                If point1.X > maxX And point2.X > maxX Then Continue For
                If point1.Y < minY And point2.Y < minY Then Continue For
                If point1.Y > maxY And point2.Y > maxY Then Continue For

                If point1.X < minX Then point1.X = minX
                If point1.X > maxX Then point1.X = maxX
                If point2.X < minX Then point2.X = minX
                If point2.X > maxX Then point2.X = maxX
                If point1.Y < minY Then point1.Y = minY
                If point1.Y > maxY Then point1.Y = maxY
                If point2.Y < minY Then point2.Y = minY
                If point2.Y > maxY Then point2.Y = maxY

                Dim pen As Pen = New Pen(Color.Black, 1) With {
                    .StartCap = LineCap.NoAnchor,
                    .EndCap = LineCap.ArrowAnchor
                }
                e.Graphics.DrawLine(pen, point1, point2)
            Next
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

#Region "IV) Data loading and chart series creation"
        ''' <summary>
        ''' Main method which handles incoming stream of quotes
        ''' </summary>
        ''' <param name="data">Collection of Task -> RIC -> Field -> Value</param>
        ''' <remarks></remarks>
        Private Sub OnLiveQuotes(ByVal data As Dictionary(Of String, Dictionary(Of String, Dictionary(Of String, Double?)))) Handles _quoteLoader.OnNewData
            Logger.Trace("OnIncomingQuotes")
            For Each taskRicAndOther As KeyValuePair(Of String, Dictionary(Of String, Dictionary(Of String, Double?))) In data
                Dim task As String = taskRicAndOther.Key
                Dim ricFieldValue = taskRicAndOther.Value

                If _currentTasks.Any(Function(elem) elem.Name = task) Then
                    PlotLiveQuotes(ricFieldValue, _currentTasks.Where(Function(elem) elem.Name = task).Select(Function(elem) elem.Field).First.ToUpper())
                ElseIf task.EndsWith("BidAsk") Then
                    PlotBidAskQuotes(task, ricFieldValue)
                Else
                    ' currently this might be only context menu data
                    If BondCMS.Visible AndAlso BondCMS.Tag = task Then
                        Try
                            Dim fieldsAndValues = ricFieldValue.Values.First()
                            ExtInfoTSMI.Text = "Bid/ask"
                            ExtInfoTSMI.DropDownItems.Clear()

                            Dim found = False
                            For Each itemsString In
                                    From bidAsk In {"BID", "ASK"}
                                    Where fieldsAndValues.ContainsKey(bidAsk) AndAlso fieldsAndValues(bidAsk) > 0
                                    Let durYield = CalcYield(fieldsAndValues(bidAsk), DateTime.Today, _ansamble.GetBondDescription(task))
                                    Select bidAsk + ": " + QuoteDescription(fieldsAndValues(bidAsk), durYield.Yld.Yield,
                                                                                      durYield.Duration, durYield.Yld.ToWhat)
                                ExtInfoTSMI.DropDownItems.Add(itemsString)
                                found = True
                            Next
                            If ExtInfoTSMI.HasDropDownItems Then
                                ExtInfoTSMI.DropDownItems.Add(New ToolStripSeparator())
                                ExtInfoTSMI.DropDownItems.Add("Show bid/ask", Nothing, AddressOf ShowBidAsk)
                                ExtInfoTSMI.Enabled = found
                            End If
                        Catch ex As Exception
                            Logger.WarnException("Exception while showing bid and ask", ex)
                        End Try
                    End If
                End If
            Next
        End Sub

        Private Sub PlotBidAskQuotes(ByVal task As String, ByVal data As Dictionary(Of String, Dictionary(Of String, Double?)))
            Logger.Debug("PlotBidAskQuotes({0})", task)
            Try
                Dim match = Regex.Match(task, "(?<ric>.+?)_BidAsk")
                Dim ric = match.Groups("ric").Value

                Dim bondPoint = TheChart.Series.FindByName(_ansamble.GetSeriesName(ric)).Points.First(Function(pnt) CType(pnt.Tag, BondPointDescr).RIC = ric)
                Dim bondPointTag = CType(bondPoint.Tag, BondPointDescr)

                Dim series = TheChart.Series.FindByName(task)
                If series Is Nothing Then
                    series = New Series(task) With {
                        .ChartType = SeriesChartType.Point,
                        .IsVisibleInLegend = False,
                        .Tag = New BidAskSeries() With {.Name = task}
                    }
                    series.EmptyPointStyle.BorderWidth = 0
                    series.EmptyPointStyle.MarkerStyle = MarkerStyle.None
                    TheChart.Series.Add(series)
                End If
                Dim fieldsAndValues = data.Values.First()
                For Each bidAsk In {"BID", "ASK"}
                    If fieldsAndValues.ContainsKey(bidAsk) AndAlso fieldsAndValues(bidAsk) > 0 Then
                        Dim durYield = CalcYield(fieldsAndValues(bidAsk), DateTime.Today, _ansamble.GetBondDescription(ric))
                        Dim thePoint As DataPoint
                        Dim descr As BidAskPointDescr
                        Dim ba = bidAsk
                        If series.Points.Any(Function(point) point.Name = ba) Then
                            thePoint = series.Points.First(Function(point) point.Name = ba)
                            descr = CType(thePoint.Tag, BidAskPointDescr)
                        Else
                            descr = New BidAskPointDescr() With {
                                .BondTag = bondPointTag,
                                .BidAsk = bidAsk,
                                .Yld = durYield.Yld,
                                .PVBP = durYield.PVBP,
                                .Duration = durYield.Duration,
                                .Convexity = durYield.Convexity,
                                .Price = fieldsAndValues(bidAsk)
                            }
                            ' plotting
                            thePoint = New DataPoint() With {
                                .MarkerStyle = durYield.Yld.ToWhat.MarkerStyle,
                                .Name = bidAsk,
                                .MarkerColor = Color.DarkKhaki,
                                .MarkerBorderColor = Color.DarkOliveGreen,
                                .MarkerSize = 5,
                                .Tag = descr
                            }
                            series.Points.Add(thePoint)
                        End If
                        thePoint.XValue = durYield.Duration

                        With descr
                            .Duration = durYield.Duration
                            .Yld.Yield = durYield.Yld.Yield
                        End With

                        Dim yValue = _spreadBenchmarks.CalculateSpreads(descr)
                        If yValue IsNot Nothing And descr.IsValid() Then
                            thePoint.IsEmpty = False
                            thePoint.YValues = {yValue.Value}
                            _bidAskLines.Add(New Tuple(Of DataPoint, DataPoint)(bondPoint, thePoint))
                        Else
                            thePoint.IsEmpty = True
                        End If
                    End If

                Next
            Catch ex As Exception
                Logger.WarnException("Exception while showing bid and ask", ex)
                Logger.Warn("Exception = {0}", ex.ToString())
            End Try
        End Sub

        Private Sub PlotLiveQuotes(ByVal data As Dictionary(Of String, Dictionary(Of String, Double?)), ByVal fieldName As String)
            Logger.Trace("PlotLiveQuotes()")
            Dim i As Integer

            Dim updatedBonds As New HashSet(Of String)

            For Each series As Series In TheChart.Series
                For Each dataPoint As DataPoint In series.Points
                    If TypeOf dataPoint.Tag Is BondPointDescr Then
                        dataPoint.MarkerBorderWidth = 0
                    End If
                Next
            Next

            For Each instrAndFields As KeyValuePair(Of String, Dictionary(Of String, Double?)) In data
                Try
                    Dim instrument As String = instrAndFields.Key
                    Dim fieldsAndValues As Dictionary(Of String, Double?) = instrAndFields.Value

                    ' checking if this bond is allowed to show up
                    If Not _ansamble.ContainsRIC(instrument) Then
                        Logger.Warn("Unknown instrument {0}", instrument)
                        Continue For
                    End If

                    Dim seriesName = _ansamble.GetSeriesName(instrument)
                    Dim bondSeries As Series = TheChart.Series.FindByName(seriesName)

                    ' creating series
                    Dim theColor = _ansamble.GetColor(instrument)
                    If bondSeries Is Nothing Then
                        Dim seriesDescr = New BondPointsSeries With {.Name = seriesName, .Color = theColor}
                        AddHandler seriesDescr.SelectedPointChanged, AddressOf OnSelectedPointChanged

                        bondSeries = New Series(seriesName) With {
                            .YValuesPerPoint = 1,
                            .ChartType = SeriesChartType.Point,
                            .IsVisibleInLegend = True,
                            .color = theColor,
                            .markerSize = 10,
                            .markerBorderColor = Color.Black,
                            .markerStyle = MarkerStyle.Circle,
                            .Tag = seriesDescr
                        }
                        With bondSeries.EmptyPointStyle
                            .BorderWidth = 0
                            .MarkerSize = 0
                            .MarkerStyle = MarkerStyle.None
                        End With
                        TheChart.Series.Add(bondSeries)
                    End If

                    ' creating data point
                    Dim point As DataPoint
                    If Not bondSeries.Points.Any(
                        Function(pnt)
                            Dim typed = TryCast(pnt.Tag, BondPointDescr)
                            Return typed IsNot Nothing AndAlso typed.RIC = instrument
                        End Function) _
                        Then
                        'todo this won't work in case one adds bonds "on-the-fly""
                        Dim descr = _ansamble.GetBondDescription(seriesName, instrument)
                        point = New DataPoint(0, 0) With {
                            .IsEmpty = True,
                            .Name = instrument,
                            .Tag = descr,
                            .ToolTip = descr.ShortName,
                            .IsVisibleInLegend = False
                            }
                        bondSeries.Points.Add(point)
                    Else
                        point = bondSeries.Points.First(Function(pnt) CType(pnt.Tag, BondPointDescr).RIC = instrument)
                        point.Color = theColor
                    End If

                    ' now update data point
                    Dim bondDataPoint As BondPointDescr
                    bondDataPoint = CType(point.Tag, BondPointDescr)
                    If fieldsAndValues.ContainsKey(fieldName) AndAlso fieldsAndValues(fieldName) > 0.0001 Then
                        Try
                            bondDataPoint.CalcPrice = fieldsAndValues(fieldName)

                            ' calculating new yield / duration
                            Dim yieldDur As DataPointDescr =
                                    CalcYield(fieldsAndValues(fieldName), DateTime.Today, _ansamble.GetBondDescription(instrument))

                            With bondDataPoint
                                .Yld = yieldDur.Yld
                                .Duration = yieldDur.Duration
                                .Convexity = yieldDur.Convexity
                                .PVBP = yieldDur.PVBP
                                .YieldAtDate = yieldDur.YieldAtDate
                                .YieldSource = YieldSource.Realtime
                            End With

                            Dim yValue = _spreadBenchmarks.CalculateSpreads(bondDataPoint)

                            ' plotting
                            If yValue IsNot Nothing And bondDataPoint.IsValid() Then
                                bondDataPoint.IsVisible = True
                                With point
                                    .YValues = {yValue}
                                    .MarkerStyle = yieldDur.Yld.ToWhat.MarkerStyle
                                    .XValue = yieldDur.Duration
                                    .IsEmpty = False
                                    .MarkerBorderWidth = 1
                                    RaiseEvent PointUpdated(instrument, yieldDur.Yld.Yield, yieldDur.Duration, bondDataPoint.CalcPrice)
                                End With
                                updatedBonds.Add(bondDataPoint.ShortName)
                            End If
                        Catch ex As Exception
                            point.IsEmpty = True
                            Logger.WarnException("Failed to plot the point", ex)
                            Logger.Warn("Exception = {0}", ex.ToString())
                        End Try
                    ElseIf Not bondDataPoint.IsValid Then
                        DoLoadHistory(bondDataPoint)
                    Else
                        Logger.Trace("Empty quote for a valid point {0} arrived, no action required", bondDataPoint.RIC)
                    End If
                Catch ex As Exception
                    Logger.WarnException("Got exception", ex)
                    Logger.Warn("Exception = {0}", ex.ToString())
                End Try
            Next

            Logger.Debug(updatedBonds.Aggregate("Updates collection is ", Function(str, elem) str + "[" + elem + "], "))
            ' notifying user on updated bonds
            Dim count = updatedBonds.Count
            If count > 0 Then
                Dim list = updatedBonds.ToArray
                Dim msg As String = "Updated "
                For i = 0 To Math.Min(count, 3) - 1
                    msg += String.Format("[{0}], ", list(i))
                Next
                msg = msg.Substring(0, msg.Length - 2)
                If count > 3 Then
                    msg += String.Format(" and {0} others", count - 3)
                End If
                StatusMessage.Text = msg
            End If

            SetChartMinMax()
        End Sub

        Private Sub DoLoadHistory(ByVal bondDataPoint As BondPointDescr)
            Dim str As String
            If _historyLoaders.Any() Then
                str = _historyLoaders.Keys.Aggregate(Function(strg, item) strg + item + "; ")
            Else
                str = "[NONE]"
            End If
            Logger.Info("No live quote for {0}, history loading for {1}", bondDataPoint.RIC, str)
            If Not _historyLoaders.Any(Function(elem) elem.Key = bondDataPoint.RIC) Then
                Logger.Debug("Will load {0}", bondDataPoint.RIC)

                Dim historyTaskDescr = New HistoryTaskDescr() With {
                        .Item = bondDataPoint.RIC,
                        .StartDate = DateTime.Today.AddDays(-3),
                        .EndDate = DateTime.Today,
                        .Fields = {"TRDPRC_1.TIMESTAMP", "TRDPRC_1.CLOSE"}.ToList,
                        .Frequency = "D",
                        .InterestingFields = {"CLOSE"}.ToList()
                        }
                ' .SingleValue = True,
                Dim hst = New HistoryLoadManager(New AdxRtHistory)
                hst.StartTask(historyTaskDescr, AddressOf OnHistoricalQuotes)
                If hst.Success Then
                    Logger.Info("Successfully added task for {0}", historyTaskDescr.Item)
                    _historyLoaders.Add(bondDataPoint.RIC, hst)
                End If
            Else
                Logger.Warn("History for {0} is already loading", bondDataPoint.RIC)
            End If
        End Sub

        Private Sub OnHistoricalQuotes(ByVal hst As HistoryLoadManager, ByVal ric As String, ByVal datastatus As RT_DataStatus,
                                       ByVal data As Dictionary(Of Date, HistoricalItem))
            GuiAsync(
                Sub()
                    Logger.Trace("OnHistoricalQuotes({0})", ric)
                    RemoveHandler hst.NewData, AddressOf OnHistoricalQuotes
                    _historyLoaders.Remove(ric)

                    If (data Is Nothing) OrElse data.Count <= 0 Then
                        Logger.Info("No data on {0} arrived", ric)
                        StatusMessage.Text = String.Format("No data on {0} history available", _ansamble.GetBondDescription(ric).ShortName)
                        Return
                    End If

                    Dim maxdate As Date
                    Dim maxElem As HistoricalItem
                    Try
                        maxdate = data.Where(Function(kvp) kvp.Value.SomePrice()).Select(Function(kvp) kvp.Key).Max
                        maxElem = data(maxdate)

                    Catch ex As Exception
                        Return
                    End Try

                    If maxElem.Close <= 0 Then
                        Logger.Warn("Zero close historical price for RIC {0}; Price {1:P2}; Date {2:dd/MM/yy}", ric,
                                    maxElem.Close, maxdate)
                        Return
                    End If

                    Try
                        Dim series = TheChart.Series.FindByName(_ansamble.GetSeriesName(ric))
                        If series Is Nothing Then Return

                        Dim dataPoint = series.Points.First(Function(pnt) CType(pnt.Tag, BondPointDescr).RIC = ric)
                        If Not dataPoint.IsEmpty Then
                            Logger.Warn("Received history after live quote arrived")
                            Return
                        End If

                        Dim pointDescr = CType(dataPoint.Tag, BondPointDescr)
                        With pointDescr
                            .CalcPrice = maxElem.Close
                            .YieldSource = YieldSource.Historical
                            .YieldAtDate = maxdate
                        End With

                        ' calculating new yield / duration
                        Dim yieldDur = CalcYield(maxElem.Close, maxdate, _ansamble.GetBondDescription(ric))

                        Dim duration = yieldDur.Duration
                        Dim convex = yieldDur.Convexity
                        Dim pvbp = yieldDur.PVBP
                        Dim bestYield = yieldDur.Yld

                        With pointDescr
                            .Yld = bestYield
                            .Duration = duration
                            .Convexity = convex
                            .PVBP = pvbp
                        End With

                        If Not pointDescr.IsValid() Then
                            Logger.Warn("Received history on invalid bond: {0}", pointDescr.ToLongString())
                            Return
                        End If

                        Dim yValue = _spreadBenchmarks.CalculateSpreads(pointDescr)

                        If yValue IsNot Nothing Then
                            pointDescr.IsVisible = True
                            With dataPoint
                                .YValues = {yValue}
                                .MarkerStyle = bestYield.ToWhat.MarkerStyle
                                .XValue = duration
                                .IsEmpty = False
                                .MarkerBorderWidth = 1
                                .Color = Color.Gray
                                .BackHatchStyle = ChartHatchStyle.Cross
                                .BackSecondaryColor = Color.Gainsboro
                                .IsVisibleInLegend = False
                            End With
                            RaiseEvent PointUpdated(ric, bestYield.Yield, duration, pointDescr.CalcPrice)
                        End If
                    Catch ex As Exception
                        Logger.WarnException(
                            String.Format("Failed to plot historical data: RIC {0}; Price {1:P2}; Date {2:dd/MM/yy}",
                                          ric, maxElem.Close, maxdate), ex)
                        Logger.Warn("Exception = {0}", ex.ToString())
                    End Try
                    SetChartMinMax()
                End Sub)
        End Sub
#End Region

#Region "V) Portfolio selection"
        Private WriteOnly Property ThisFormDataSource As Integer
            Set(ByVal value As Integer)
                Logger.Trace("ThisFormDataSource to {0} with id {1}", value)

                Dim currentPortID As Long
                If value < 0 Then
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
                        group = New VisualizableGroup() With {
                            .Group = groupType,
                            .Name = port.whereFromDescr,
                            .FromID = port.fromID,
                            .BidField = port.bid_field,
                            .AskField = port.ask_field,
                            .LastField = port.last_field,
                            .HistField = port.hist_field,
                            .RicStructure = port.ric_structure,
                            .Brokers = {"MM", ""}.ToList(),
                            .Currency = "",
                            .Color = port.color
                        }

                        Dim pD = From elem In portfolioUnitedDataTable Where elem.pid = currentPortID And elem.fromID = group.FromID
                        Dim ars = (From row In pD Where row.include Select row.ric).ToList
                        Dim rrs = (From row In pD Where Not row.include Select row.ric).ToList
                        ars.RemoveAll(Function(ric) rrs.Contains(ric))
                        InitBondDescriber()
                        ars.ForEach(
                            Sub(ric)
                                Dim descr = GetBondDescr(ric)
                                descr.SeriesName = group.Name
                                group.Elements.Add(ric, descr)
                            End Sub)
                        _ansamble.Groups.Add(group)
                    Else
                        Logger.Error("Failed to parse {0}", port.whereFrom)
                    End If
                Next
                _currentTasks = _ansamble.PrepareTasks()
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
                'portfolioSelectTSCB.Enabled = True
                'With portfolioSelectTSCB.ComboBox
                '    .BindingContext = BindingContext
                '    .DataSource = portDescrList
                '    .DisplayMember = "Name"
                '    .ValueMember = "Id"
                'End With
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
                    DoSetRunning()
                Catch ex As Exception
                    Logger.ErrorException("Failed to select a portfolio", ex)
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
                _historicalCurves.AddCurve(BondCMS.Tag + "_HIST",
                                           New HistoryTaskDescr() With {
                                                .Item = BondCMS.Tag,
                                                .EndDate = DateTime.Today,
                                                .StartDate = DateTime.Today.AddDays(-90),
                                                .Frequency = "W",
                                                .InterestingFields = {"CLOSE"}.ToList()
                                            })
                '2) Take this bond series and add other points into it. They must be ordered by date (regardless of direction)
            Catch ex As Exception
                Logger.ErrorException("", ex)
            End Try
        End Sub

        Private Sub OnCurveRemoved(ByVal theName As String)
            Dim item = TheChart.Series.FindByName(theName)
            If item IsNot Nothing Then
                item.Points.Clear()
                TheChart.Series.Remove(item)
            Else
                Logger.Warn("Failed to remove historical series {0}", theName)
            End If
        End Sub

        Private Sub RemoveHistoryTSMIClick(sender As Object, e As EventArgs) Handles RemoveHistoryTSMI.Click
            _historicalCurves.RemoveCurve(HistoryCMS.Tag)
        End Sub

        Public Sub OnHistoricalCurveData(ByVal hst As HistoryLoadManager, ByVal ric As String, ByVal datastatus As RT_DataStatus,
                                         ByVal data As Dictionary(Of Date, HistoricalItem))
            Logger.Debug("OnHistoricalCurveData")
            If data Is Nothing Then
                StatusMessage.Text = String.Format("No historical data on {0} available", _ansamble.GetBondDescription(ric).ShortName)
                Return
            End If
            GuiAsync(
                Sub()
                    Try
                        Dim series As Series = TheChart.Series.FindByName(ric + "_HIST_CURVE")
                        If series Is Nothing Then
                            series = New Series(ric + "_HIST_CURVE") With {
                                .YValuesPerPoint = 1,
                                .ChartType = SeriesChartType.Line,
                                .borderWidth = 1,
                                .borderDashStyle = ChartDashStyle.Dash,
                                .borderColor = Color.Green,
                                .Tag = New HistCurveSeries() With {.Name = ric},
                                .IsVisibleInLegend = False
                            }
                        End If

                        Dim baseBondName = _ansamble.GetBondDescription(ric).ShortName
                        For Each yieldDur As DataPointDescr In _
                            From bondHistoryDescr In data
                            Where bondHistoryDescr.Value.Close > 0
                            Select CalcYield(bondHistoryDescr.Value.Close, bondHistoryDescr.Key, _ansamble.GetBondDescription(ric))

                            Dim point As New DataPoint
                            Dim duration = yieldDur.Duration
                            Dim convex = yieldDur.Convexity
                            Dim pvbp = yieldDur.PVBP
                            Dim bestYield = yieldDur.Yld

                            Dim theTag = New HistCurvePointDescr With {
                                .Duration = duration,
                                .Convexity = convex,
                                .Yld = bestYield,
                                .PVBP = pvbp,
                                .RIC = ric,
                                .HistCurveName = ric + "_HIST_CURVE",
                                .BaseBondName = baseBondName
                            }
                            '.YieldToDate = bestYield.YieldAtDate,
                            Dim yValue = _spreadBenchmarks.CalculateSpreads(theTag)
                            If yValue IsNot Nothing Then
                                With point
                                    .YValues = {yValue}
                                    .XValue = duration
                                    .MarkerStyle = bestYield.ToWhat.MarkerStyle
                                    .MarkerBorderWidth = 1
                                    .Tag = theTag
                                    .IsEmpty = Not theTag.IsValid
                                End With
                                series.Points.Add(point)
                            End If
                        Next
                        If series.Points.Count > 0 Then TheChart.Series.Add(series)
                    Catch ex As Exception
                        Logger.WarnException("Failed to plot a chart", ex)
                    End Try
                End Sub)
            If datastatus <> RT_DataStatus.RT_DS_PARTIAL Then
                RemoveHandler hst.NewData, AddressOf OnHistoricalCurveData
            End If
        End Sub
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
            'Dim cForm = New NewCurveForm()
            'cForm.ShowDialog()

            'If cForm.DialogResult = DialogResult.OK Then
            'If cForm.CurveListView.SelectedItems.Count = 0 Then
            '    MsgBox("Nothing selected")
            '    Return
            'End If

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
                MsgBox("No rics found")
                Return
            End If

            Dim newCurve = New YieldCurve(Guid.NewGuid.ToString, selectedItem.Name, ricsInCurve, selectedItem.Color, fieldNames)
            AddHandler newCurve.Updated, AddressOf OnCurvePaint
            _moneyMarketCurves.Add(newCurve)
            newCurve.Subscribe()
            'End If
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

                Dim curveName = curve.GetName()
                _spreadBenchmarks.CleanupCurve(curveName)
                curve.Cleanup()
            End If
        End Sub

        Private Sub BootstrapTSMIClick(sender As Object, e As EventArgs) Handles BootstrapTSMI.Click
            Logger.Debug("BootstrapTSMI_Click()")
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

        Private Sub OnCurvePaint(ByVal curve As ICurve, ByVal points As List(Of XY), ByVal first As Boolean)
            PaintSwapCurve(curve, points)
            If Not first Then _spreadBenchmarks.UpdateCurve(curve.GetName())
            SetChartMinMax()
        End Sub

        Private Sub PaintSwapCurve(ByVal curve As SwapCurve, ByVal points As List(Of XY))
            Logger.Debug("PaintSwapCurve({0})", curve.GetName())
            If points.Count < 2 Then
                Logger.Info("Too little points to plot")
                Return
            End If

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

                    With theSeries
                        If points.Count <= 50 Then
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
                                Case SpreadMode.Yield
                                    theTag.Yld = New YieldStructure With {.Yield = item.Y}
                                Case SpreadMode.PointSpread
                                    theTag.PointSpread = item.Y
                                Case SpreadMode.ZSpread
                                    theTag.ZSpread = item.Y
                                Case SpreadMode.ASWSpread
                                    theTag.ASWSpread = item.Y
                            End Select

                            'Dim yValue = _spreadBenchmarks.CalculateSpreads(theTag)
                            'If yValue IsNot Nothing Then
                            theSeries.Points.Add(
                                New DataPoint With {
                                    .Name = String.Format("{0}Y", item.X),
                                    .XValue = item.X,
                                    .YValues = {item.Y},
                                    .Tag = theTag
                                })
                            'End If
                        End Sub)
                End Sub)
        End Sub
#End Region

#Region "2) Specific curves"
        Private Sub RubCCSTSMIClick(sender As Object, e As EventArgs) Handles RubCCSTSMI.Click
            Logger.Debug("RubCCSTSMIClick()")
            Dim rubCCS = New RubCCS(DateTime.Today, Guid.NewGuid.ToString())
            rubCCS.Subscribe()
            _moneyMarketCurves.Add(rubCCS)
            AddHandler rubCCS.Updated, AddressOf OnCurvePaint
        End Sub

        Private Sub RubIRSTSMIClick(sender As Object, e As EventArgs) Handles RubIRSTSMI.Click
            Logger.Debug("RubIRSTSMIClick()")
            Dim rubIRS = New RubIRS(DateTime.Today, Guid.NewGuid.ToString())
            rubIRS.Subscribe()
            _moneyMarketCurves.Add(rubIRS)
            AddHandler rubIRS.Updated, AddressOf OnCurvePaint
        End Sub

        Private Sub NDFTSMIClick(sender As Object, e As EventArgs) Handles NDFTSMI.Click
            Logger.Debug("NDFTSMI_Click()")
            Dim rubNDF = New RubNDF(DateTime.Today, Guid.NewGuid.ToString())
            rubNDF.Subscribe()
            _moneyMarketCurves.Add(rubNDF)
            AddHandler rubNDF.Updated, AddressOf OnCurvePaint
        End Sub
#End Region
#End Region
    End Class
End Namespace