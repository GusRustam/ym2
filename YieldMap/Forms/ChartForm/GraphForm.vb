Imports System.Windows.Forms
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Drawing
Imports System.ComponentModel
Imports System.IO
Imports System.Threading
Imports ReutersData
Imports YieldMap.Tools.Elements
Imports YieldMap.Forms.PortfolioForm
Imports Settings
Imports Uitls
Imports YieldMap.My.Resources
Imports NLog
Imports DbManager

Namespace Forms.ChartForm
    Public Class GraphForm
        Implements IEquatable(Of GraphForm)
        Private ReadOnly _id As Guid = Guid.NewGuid()

        Public Overloads Function Equals(ByVal other As GraphForm) As Boolean Implements IEquatable(Of GraphForm).Equals
            If ReferenceEquals(Nothing, other) Then Return False
            If ReferenceEquals(Me, other) Then Return True
            Return _id.Equals(other._id)
        End Function

        Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
            If ReferenceEquals(Nothing, obj) Then Return False
            If ReferenceEquals(Me, obj) Then Return True
            If obj.GetType IsNot Me.GetType Then Return False
            Return Equals(DirectCast(obj, GraphForm))
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return _id.GetHashCode
        End Function

        Public Shared Operator =(ByVal left As GraphForm, ByVal right As GraphForm) As Boolean
            Return Equals(left, right)
        End Operator

        Public Shared Operator <>(ByVal left As GraphForm, ByVal right As GraphForm) As Boolean
            Return Not Equals(left, right)
        End Operator

        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(GraphForm))

        Private WithEvents _theSettings As SettingsManager = SettingsManager.Instance
        Private WithEvents _tableForm As TableForm.TableForm = New TableForm.TableForm()
        Private WithEvents _ansamble As New Ansamble

        Private Sub ViewPortChanged() Handles _theSettings.ViewPortChanged
            SetChartMinMax()
        End Sub

        Private Sub YieldCalcModeChanged() Handles _theSettings.YieldCalcModeChanged
            _ansamble.Replot()
        End Sub

        Private Sub ShowPointSizeChanged(ByVal show As Boolean) Handles _theSettings.ShowPointSizeChanged
            _ansamble.Replot()
        End Sub

        Private Sub ShowBidAskChanged(ByVal show As Boolean) Handles _theSettings.ShowBidAskChanged
            If Not show Then HideBidAsk()
        End Sub

        Private Sub FieldsPriorityChanged(ByVal list As String) Handles _theSettings.FieldsPriorityChanged
            _ansamble.Replot()
        End Sub
        Protected ReadOnly DateModule = Eikon.Sdk.CreateAdxDateModule()

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
                        If _theSettings.ClearPoints Then _ansamble.Items.CleanupOnlyBonds()
                        If _theSettings.ClearBondCurves Then _ansamble.Items.CleanupOnlyCurves()
                        If _theSettings.ClearOtherCurves Then _ansamble.SwapCurves.Cleanup()

                        StatusMessage.Text = "Stopped"
                End Select
            End Set
        End Property

        Private WriteOnly Property ThisFormDataSource As Integer
            Set(ByVal value As Integer)
                Logger.Trace("ThisFormDataSource to id {0}", value)

                Dim currentPortId As Long
                If value < 0 Then
                    ThisFormStatus = FormDataStatus.Running
                    Return
                Else
                    currentPortId = value
                End If

                Dim portfolioStructure = PortfolioManager.Instance.GetPortfolioStructure(currentPortId)
                For Each grp As BondGroup In From port In portfolioStructure.Sources
                                           Where TypeOf port.Source Is DbManager.Chain Or TypeOf port.Source Is UserList
                                           Select New BondGroup(_ansamble, port, portfolioStructure)
                    Dim tmp = grp
                    AddHandler tmp.Updated, Sub(items) OnGroupUpdated(tmp, items)
                    AddHandler tmp.Cleared, Sub() ClearSeries(tmp.Identity)
                    AddHandler tmp.UpdatedSpread, Sub(data As List(Of PointOfCurve), ord As IOrdinate) If _ansamble.YSource = ord Then OnGroupUpdated(tmp, data)
                    _ansamble.Items.Add(tmp)
                Next
                For Each grp As CustomBondGroup In From port In portfolioStructure.Sources
                                           Where TypeOf port.Source Is CustomBond
                                           Select New CustomBondGroup(_ansamble, port, portfolioStructure)
                    Dim tmp = grp
                    AddHandler tmp.Updated, Sub(items) OnGroupUpdated(tmp, items)
                    AddHandler tmp.UpdatedSpread, Sub(data As List(Of PointOfCurve), ord As IOrdinate) If _ansamble.YSource = ord Then OnGroupUpdated(tmp, data)
                    _ansamble.Items.Add(tmp)
                Next
                _ansamble.Items.Start()
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
        Private Sub BondCMS_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles BondCMS.Opening
            If BondCMS.Tag Is Nothing Then
                e.Cancel = True
                Return
            End If

            Dim bondDataPoint = CType(BondCMS.Tag, Bond)
            If TypeOf bondDataPoint.Parent Is BondCurve Then
                BondCurveTSMI.Visible = True
            Else
                BondCurveTSMI.Visible = False
            End If
            MainInfoLine1TSMI.Text = bondDataPoint.MetaData.ShortName

            ExtInfoTSMI.DropDownItems.Clear()
            For Each key In bondDataPoint.QuotesAndYields
                Dim currentKey = key
                Dim x = bondDataPoint.QuotesAndYields(key)
                Dim newItem = ExtInfoTSMI.DropDownItems.Add(String.Format("{0}: {1:F4}, {2:P2} {3}, {4:F2}", key, x.Price, x.Yld.Yield, x.Yld.ToWhat.Abbr, x.Duration))
                If bondDataPoint.QuotesAndYields.MaxPriorityField = key Then CType(newItem, ToolStripMenuItem).Checked = True
                AddHandler newItem.Click,
                    Sub(sender1 As Object, e1 As EventArgs)
                        If currentKey <> bondDataPoint.UserSelectedQuote Then
                            bondDataPoint.UserSelectedQuote = currentKey
                        Else
                            bondDataPoint.UserSelectedQuote = ""
                        End If
                    End Sub
            Next

            ExtInfoTSMI.DropDownItems.Add("-")
            ExtInfoTSMI.DropDownItems.Add(String.Format("Today volume: {0:N0}", bondDataPoint.TodayVolume))
            ExtInfoTSMI.DropDownItems.Add("-")
            Dim newItem1 As ToolStripMenuItem = ExtInfoTSMI.DropDownItems.Add("Set custom price...")
            AddHandler newItem1.Click,
                       Sub(sender1 As Object, e1 As EventArgs)
                           Dim res = InputBox("Enter price", "Custom bond price")
                           If IsNumeric(res) Then
                               bondDataPoint.SetCustomPrice(CDbl(res))
                           ElseIf res <> "" Then
                               MessageBox.Show("Invalid number")
                           End If

                       End Sub

            Dim newItem2 As ToolStripMenuItem = ExtInfoTSMI.DropDownItems.Add("Set spread...")
            AddHandler newItem2.Click,
                       Sub(sender1 As Object, e1 As EventArgs)
                           Dim res = InputBox("Enter spread in b.p.", "Custom bond spread", If(bondDataPoint.UserDefinedSpread > 0, bondDataPoint.UserDefinedSpread, ""))
                           If IsNumeric(res) Then
                               bondDataPoint.UserDefinedSpread = res
                           ElseIf res <> "" Then
                               MessageBox.Show("Invalid number")
                           Else
                               bondDataPoint.UserDefinedSpread = 0
                           End If

                       End Sub

            IssuerNameSeriesTSMI.Checked = (bondDataPoint.LabelMode = LabelMode.IssuerAndSeries)
            ShortNameTSMI.Checked = (bondDataPoint.LabelMode = LabelMode.IssuerCpnMat)
            DescriptionTSMI.Checked = (bondDataPoint.LabelMode = LabelMode.Description)
            SeriesOnlyTSMI.Checked = (bondDataPoint.LabelMode = LabelMode.SeriesOnly)

            Dim found As Boolean = False
            For Each item In YieldCalculationModeToolStripMenuItem.DropDownItems
                Dim elem = CType(item, ToolStripMenuItem)
                If elem IsNot Nothing Then
                    If elem.Text = bondDataPoint.YieldMode Then
                        item.checked = True
                        found = True
                    Else
                        item.checked = False
                    End If
                End If
            Next
            If Not found Then DefaultToolStripMenuItem.Checked = True
        End Sub

        ' The chart
        Private Sub TheChartClick(ByVal sender As Object, ByVal e As EventArgs) Handles TheChart.Click
            Logger.Trace("TheChartClick")
            Dim mouseEvent As MouseEventArgs = e
            Dim htr As HitTestResult = TheChart.HitTest(mouseEvent.X, mouseEvent.Y)
            If mouseEvent.Button = MouseButtons.Right Then
                Try
                    If htr.ChartElementType = ChartElementType.DataPoint Then
                        Dim point As DataPoint = CType(htr.Object, DataPoint)
                        If TypeOf point.Tag Is Bond Then
                            BondCMS.Tag = point.Tag

                            Dim isNotSynt As Boolean = Not TypeOf point.Tag Is CustomCouponBond

                            YieldCalcModeSep.Visible = isNotSynt
                            YieldCalculationModeToolStripMenuItem.Visible = isNotSynt
                            DescriptionSep.Visible = isNotSynt
                            BondDescriptionTSMI.Visible = isNotSynt
                            RelatedQuoteTSMI.Visible = isNotSynt
                            RelatedChartTSMI.Visible = isNotSynt
                            HistorySep.Visible = isNotSynt
                            ShowHistoryTSMI.Visible = isNotSynt
                            BondLabelsTSMI.Visible = isNotSynt

                            BondCMS.Show(TheChart, mouseEvent.Location)

                        ElseIf TypeOf point.Tag Is SwapCurve Then
                            Dim curve = CType(point.Tag, SwapCurve)
                            MMNameTSMI.Text = curve.Name
                            MoneyCurveCMS.Show(TheChart, mouseEvent.Location)
                            MoneyCurveCMS.Tag = curve.Identity
                            ShowCurveParameters(curve)

                        ElseIf TypeOf point.Tag Is HistoryPointTag Then
                            Dim histDataPoint = CType(point.Tag, HistoryPointTag)
                            HistoryCMS.Tag = histDataPoint
                            HistoryCMS.Show(TheChart, mouseEvent.Location)

                        ElseIf TypeOf point.Tag Is BondCurve Then
                            BondCurveCMS.Tag = CType(point.Tag, BondCurve).Identity
                            BondCurveCMS.Show(TheChart, mouseEvent.Location)

                        End If
                    ElseIf htr.ChartElementType = ChartElementType.PlottingArea Or htr.ChartElementType = ChartElementType.Gridlines Then
                        ChartCMS.Show(TheChart, mouseEvent.Location)

                    ElseIf htr.ChartElementType = ChartElementType.LegendItem Then
                        Dim item = CType(htr.Object, LegendItem)
                        If _ansamble(item.Tag) IsNot Nothing Then
                            If TypeOf _ansamble(item.Tag) Is BondGroup Then
                                BondSetCMS.Tag = item.Tag
                                BondSetCMS.Show(TheChart, mouseEvent.Location)
                            ElseIf TypeOf _ansamble(item.Tag) Is BondCurve Then
                                BondCurveCMS.Tag = item.Tag
                                BondCurveCMS.Show(TheChart, mouseEvent.Location)
                            ElseIf TypeOf _ansamble(item.Tag) Is SwapCurve Then
                                MoneyCurveCMS.Tag = item.Tag
                                MoneyCurveCMS.Show(TheChart, mouseEvent.Location)
                            End If
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

                    If TypeOf point.Tag Is Bond And Not point.IsEmpty Then
                        Dim bondData = CType(point.Tag, Bond)
                        DscrLabel.Text = bondData.MetaData.ShortName
                        Dim mainYield = bondData.QuotesAndYields.Main

                        SpreadLabel.Text = If(mainYield.PointSpread IsNot Nothing, String.Format("{0:F0} b.p.", mainYield.PointSpread), N_A)
                        ZSpreadLabel.Text = If(mainYield.ZSpread IsNot Nothing, String.Format("{0:F0} b.p.", mainYield.ZSpread), N_A)
                        ASWLabel.Text = If(mainYield.ASWSpread IsNot Nothing, String.Format("{0:F0} b.p.", mainYield.ASWSpread), N_A)
                        YldLabel.Text = String.Format("{0:P2}", mainYield.Yld)
                        DurLabel.Text = String.Format("{0:F2}", point.XValue)

                        ConvLabel.Text = String.Format("{0:F2}", mainYield.Convexity)
                        YldLabel.Text = String.Format("{0:P2} {1}", mainYield.Yld.Yield, mainYield.Yld.ToWhat.Abbr)
                        DatLabel.Text = String.Format("{0:dd/MM/yyyy}", mainYield.YieldAtDate)
                        MatLabel.Text = String.Format("{0:dd/MM/yyyy}", bondData.MetaData.Maturity)
                        CpnLabel.Text = String.Format("{0:F2}%", bondData.MetaData.Coupon)
                        PVBPLabel.Text = String.Format("{0:F4}", mainYield.PVBP)
                        CType(htr.Series.Tag, BondSetSeries).SelectedPointIndex = htr.PointIndex

                        PlotBidAsk(bondData)

                    ElseIf TypeOf point.Tag Is SwapCurve Then
                        Dim curve = CType(point.Tag, SwapCurve)

                        DscrLabel.Text = curve.Name
                        DatLabel.Text = String.Format("{0:dd/MM/yyyy}", curve.GroupDate)
                        Dim period = String.Format("{0:F0}D", 365 * point.XValue)
                        Dim aDate = DateModule.DfAddPeriod("RUS", Date.Today, period, "")
                        MatLabel.Text = String.Format("{0:dd/MM/yyyy}", Utils.FromExcelSerialDate(aDate.GetValue(1, 1)))

                        Select Case _ansamble.YSource
                            Case Yield : YldLabel.Text = String.Format("{0:P2}", point.YValues(0))
                            Case ZSpread : ZSpreadLabel.Text = String.Format("{0:F0} b.p.", point.YValues(0))
                            Case PointSpread : SpreadLabel.Text = String.Format("{0:F0} b.p.", point.YValues(0))
                            Case AswSpread : ASWLabel.Text = String.Format("{0:F0} b.p.", point.YValues(0))
                        End Select
                        DurLabel.Text = String.Format("{0:F2}", point.XValue)

                        DatLabel.Text = ""

                    ElseIf TypeOf point.Tag Is HistoryPointTag Then
                        DurLabel.Text = String.Format("{0:F2}", point.XValue)
                        YldLabel.Text = String.Format("{0:P2}", point.YValues(0))

                        Dim historyDataPoint = CType(point.Tag, HistoryPointTag)
                        DscrLabel.Text = historyDataPoint.Meta.ShortName
                        DatLabel.Text = String.Format("{0:dd/MM/yyyy}", historyDataPoint.Descr.YieldAtDate)
                        ConvLabel.Text = String.Format("{0:F2}", historyDataPoint.Descr.Convexity)
                        MatLabel.Text = String.Format("{0:dd/MM/yyyy}", historyDataPoint.Meta.Maturity)
                        CpnLabel.Text = String.Format("{0:F2}%", historyDataPoint.Meta.Coupon)
                        PVBPLabel.Text = String.Format("{0:F4}", historyDataPoint.Descr.Pvbp)
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
                    ASWLabel.Text = If(_ansamble.Benchmarks.HasOrd(AswSpread), " -> " + _ansamble.Benchmarks(AswSpread).Name, "")
                    SpreadLabel.Text = If(_ansamble.Benchmarks.HasOrd(PointSpread), " -> " + _ansamble.Benchmarks(PointSpread).Name, "")
                    ZSpreadLabel.Text = If(_ansamble.Benchmarks.HasOrd(ZSpread), " -> " + _ansamble.Benchmarks(ZSpread).Name, "")
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
                                                  point.Color = Color.FromName(tg.QuotesAndYields.Main.BackColor)
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
            TheChart.ChartAreas(0).AxisX.ScaleView.ZoomReset()
            TheChart.ChartAreas(0).AxisY.ScaleView.ZoomReset()
            SetChartMinMax()
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
            Dim rubCCS = New RubCCS(_ansamble)

            AddHandler rubCCS.Cleared, Sub() ClearSeries(rubCCS.Identity)
            AddHandler rubCCS.Updated, Sub(data As List(Of PointOfCurve)) OnSwapCurvePaint(data, rubCCS)
            AddHandler rubCCS.UpdatedSpread, Sub(data As List(Of PointOfCurve), ord As IOrdinate) If _ansamble.YSource = ord Then OnSwapCurvePaint(data, rubCCS)

            rubCCS.Subscribe()
            _ansamble.SwapCurves.Add(rubCCS)
        End Sub

        Private Sub RubIRS_TSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles RubIRSTSMI.Click
            Logger.Debug("RubIRSTSMIClick()")
            Dim rubIRS = New RubIRS(_ansamble)
            AddHandler rubIRS.Cleared, Sub() ClearSeries(rubIRS.Identity)
            AddHandler rubIRS.Updated, Sub(data As List(Of PointOfCurve)) OnSwapCurvePaint(data, rubIRS)
            AddHandler rubIRS.UpdatedSpread, Sub(data As List(Of PointOfCurve), ord As IOrdinate) If _ansamble.YSource = ord Then OnSwapCurvePaint(data, rubIRS)

            rubIRS.Subscribe()
            _ansamble.SwapCurves.Add(rubIRS)
        End Sub

        Private Sub UsdIRS_TSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles UsdIRSTSMI.Click
            Logger.Debug("UsdIRS_TSMIClick()")
            Dim usdIRS = New UsdIRS(_ansamble)
            AddHandler usdIRS.Cleared, Sub() ClearSeries(usdIRS.Identity)
            AddHandler usdIRS.Updated, Sub(data As List(Of PointOfCurve)) OnSwapCurvePaint(data, usdIRS)
            AddHandler usdIRS.UpdatedSpread, Sub(data As List(Of PointOfCurve), ord As IOrdinate) If _ansamble.YSource = ord Then OnSwapCurvePaint(data, usdIRS)

            usdIRS.Subscribe()
            _ansamble.SwapCurves.Add(usdIRS)
        End Sub

        Private Sub NDFTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles NDFTSMI.Click
            Logger.Debug("NDFTSMI_Click()")
            Dim rubNDF = New RubNDF(_ansamble)
            AddHandler rubNDF.Cleared, Sub() ClearSeries(rubNDF.Identity)
            AddHandler rubNDF.Updated, Sub(data As List(Of PointOfCurve)) OnSwapCurvePaint(data, rubNDF)
            AddHandler rubNDF.UpdatedSpread, Sub(data As List(Of PointOfCurve), ord As IOrdinate) If _ansamble.YSource = ord Then OnSwapCurvePaint(data, rubNDF)

            rubNDF.Subscribe()
            _ansamble.SwapCurves.Add(rubNDF)
        End Sub

        Private Sub ShowLegendTSBClicked(ByVal sender As Object, ByVal e As EventArgs) Handles ShowLegendTSB.Click
            TheChart.Legends(0).Enabled = ShowLegendTSB.Checked
        End Sub

        Private Sub ShowLabelsTSBClick(ByVal sender As Object, ByVal e As EventArgs) Handles ShowLabelsTSB.Click
            Logger.Trace("ShowLabelsTSBClick")
            For Each grp As KeyValuePair(Of Long, Group) In _ansamble.Items
                grp.Value.LabelsOn = ShowLabelsTSB.Checked
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
        Private Sub CopyToClipboardTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles CopyToClipboardTSMI.Click
            Dim bmp As New Bitmap(TheChart.Width, TheChart.Height)
            TheChart.DrawToBitmap(bmp, TheChart.ClientRectangle)
            Clipboard.SetImage(bmp)
        End Sub

        Private Sub RelatedQuoteTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles RelatedQuoteTSMI.Click
            If BondCMS.Tag Is Nothing Then Return
            Dim ric = CType(BondCMS.Tag, Bond).MetaData.RIC
            Dim cmd As String, args As String
            Try
                cmd = String.Format("""{0}""", Path.Combine(Utils.GetMyPath(), "Runner.exe"))
                args = String.Format("reuters://REALTIME/verb=FullQuote/ric={0}", ric)
                Utils.RunCommand(cmd, args)
            Catch ex As Exception
                MessageBox.Show("No permission to run external applications", "Cannot perform operation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        End Sub

        Private Sub BondDescriptionTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles BondDescriptionTSMI.Click
            If BondCMS.Tag Is Nothing Then Return
            Dim ric = CType(BondCMS.Tag, Bond).MetaData.RIC
            Dim cmd As String, args As String
            Try
                cmd = String.Format("""{0}""", Path.Combine(Utils.GetMyPath(), "Runner.exe"))
                args = String.Format("reuters://REALTIME/verb=BondData/ric={0}", ric)
                Utils.RunCommand(cmd, args)
            Catch ex As Exception
                Logger.WarnException(String.Format("Failed to run command {0} with args {1}", cmd, args), ex)
                Logger.Warn("Exception = {0}", ex.ToString())
                MessageBox.Show("No permission to run external applications", "Cannot perform operation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        End Sub

        Private Sub RelatedChartTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles RelatedChartTSMI.Click
            If BondCMS.Tag Is Nothing Then Return
            Dim ric = CType(BondCMS.Tag, Bond).MetaData.RIC
            Dim cmd As String, args As String
            Try
                cmd = String.Format("""{0}""", Path.Combine(Utils.GetMyPath(), "Runner.exe"))
                args = String.Format("reuters://REALTIME/verb=RelatedGraph/ric={0}", ric)
                Utils.RunCommand(cmd, args)
            Catch ex As Exception
                MessageBox.Show("No permission to run external applications", "Cannot perform operation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        End Sub

        Private Sub ShowCurveItemsTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles ShowCurveItemsTSMI.Click
            If MoneyCurveCMS.Tag Is Nothing Then Return
            Dim frm As New BondCurveItemsForm

            frm.Curve = _ansamble.SwapCurves(MoneyCurveCMS.Tag)
            frm.BondsTP.Visible = False
            frm.Text = "Swap curve items"
            frm.Show()
        End Sub

        Private Sub AsTableTSBClick(ByVal sender As Object, ByVal e As EventArgs) Handles AsTableTSB.Click
            _tableForm.Bonds = _ansamble.Items.AsTable
            _tableForm.ShowDialog()
        End Sub

        Private Sub LinkSpreadLabelLinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles SpreadLinkLabel.LinkClicked
            ShowCurveCMS("PointSpread",
                If(_ansamble.Benchmarks.HasOrd(PointSpread), _ansamble.Benchmarks(PointSpread), Nothing))
        End Sub

        Private Sub ZSpreadLinkLabelLinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles ZSpreadLinkLabel.LinkClicked
            ShowCurveCMS("ZSpread",
                If(_ansamble.Benchmarks.HasOrd(ZSpread), _ansamble.Benchmarks(ZSpread), Nothing))
        End Sub

        Private Sub ASWLinkLabelLinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles ASWLinkLabel.LinkClicked
            Dim refCurve = If(_ansamble.Benchmarks.HasOrd(AswSpread), _ansamble.Benchmarks(AswSpread), Nothing)
            SpreadCMS.Items.Clear()
            SpreadCMS.Tag = "ASWSpread"
            If Not _ansamble.SwapCurves.Any() Then Return
            For Each item In (From crv In _ansamble.SwapCurves
                              Where TypeOf crv Is IAswBenchmark AndAlso CType(crv, IAswBenchmark).CanBeBenchmark())
                Dim elem = CType(SpreadCMS.Items.Add(item.Name, Nothing, AddressOf OnBenchmarkSelected), ToolStripMenuItem)
                elem.CheckOnClick = True
                elem.Checked = refCurve IsNot Nothing AndAlso item.Name = refCurve.Name
                elem.Tag = item
            Next

            SpreadCMS.Show(MousePosition)
        End Sub

        Private Sub CurvesTSMIDropDownOpening(ByVal sender As Object, ByVal e As EventArgs) Handles CurvesTSMI.DropDownOpening
            Dim portfolioManager = DbManager.PortfolioManager.Instance

            BondCurvesNewTSMI.DropDownItems.Clear()
            DoAddNew(From chain In portfolioManager.ChainsView Where chain.Curve)
            DoAddNew(From list In portfolioManager.UserListsView Where list.Curve)
        End Sub

        Private Sub AddBondCurveNewTSMIClick(ByVal sender As Object, ByVal e As EventArgs)
            Logger.Info("AddBondCurve-New-TSMIClick")
            Dim src = CType(CType(sender, ToolStripMenuItem).Tag, Source)

            Dim curve = New BondCurve(_ansamble, src)
            AddHandler curve.UpdatedSpread, Sub(data As List(Of PointOfCurve), ord As IOrdinate) If _ansamble.YSource = ord Then OnNewCurvePaint(data)
            AddHandler curve.Updated, AddressOf OnNewCurvePaint
            AddHandler curve.Cleared, Sub() ClearSeries(curve.Identity)
            _ansamble.Items.Add(curve)
            curve.Subscribe()
        End Sub

        Private Sub SelDateTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles SelDateTSMI.Click
            Logger.Debug("SelDateTSMI_Click()")
            Dim datePicker = New DatePickerForm
            If datePicker.ShowDialog() = DialogResult.OK Then

                Dim curve = _ansamble.SwapCurves(MoneyCurveCMS.Tag)
                If curve Is Nothing Then
                    Logger.Warn("No such curve {0}", MoneyCurveCMS.Tag.ToString())
                Else
                    curve.GroupDate = datePicker.TheCalendar.SelectionEnd
                End If
            End If
        End Sub

        Private Sub DeleteMmCurveTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles DeleteMMCurveTSMI.Click
            If MoneyCurveCMS.Tag Is Nothing Then Return
            Dim crv = CType(_ansamble(MoneyCurveCMS.Tag), SwapCurve)
            _ansamble(crv.Identity).Cleanup()
        End Sub

        Private Sub BootstrapTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles BootstrapTSMI.Click
            Logger.Debug("BootstrapTSMIClick()")
            Dim snd = CType(sender, ToolStripMenuItem)
            Dim curve = _ansamble.SwapCurves(MoneyCurveCMS.Tag)
            If curve Is Nothing Then Return
            If curve.CanBootstrap() Then curve.Bootstrapped = snd.Checked
        End Sub

        Private Sub OnBrokerSelected(ByVal sender As Object, ByVal e As EventArgs)
            Logger.Debug("OnBrokerSelected()")
            Dim snd = CType(sender, ToolStripMenuItem)
            Dim curve = _ansamble.SwapCurves(MoneyCurveCMS.Tag)
            If curve IsNot Nothing Then
                curve.SetBroker(snd.Text)
            End If
        End Sub

        Private Sub OnQuoteSelected(ByVal sender As Object, ByVal e As EventArgs)
            Logger.Debug("OnQuoteSelected()")
            Dim snd = CType(sender, ToolStripMenuItem)
            Dim curve = _ansamble.SwapCurves(MoneyCurveCMS.Tag)
            If curve IsNot Nothing Then
                curve.SetQuote(snd.Text)
            End If
        End Sub

        Private Sub RemoveFromChartTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles RemoveFromChartTSMI.Click
            If BondSetCMS.Tag IsNot Nothing AndAlso IsNumeric(BondSetCMS.Tag) Then _ansamble.Items.Remove(BondSetCMS.Tag)
        End Sub

        Private Sub RemovePointTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles RemovePointTSMI.Click
            If BondCMS.Tag Is Nothing Then Return
            CType(BondCMS.Tag, Bond).Annihilate()
        End Sub

        Private Sub ShowHistoryTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles ShowHistoryTSMI.Click
            Logger.Trace("ShowHistQuotesTSMIClick")
            Try
                Dim historyLoader As New History
                AddHandler historyLoader.HistoricalData, AddressOf OnHistoricalData
                Dim item = CType(BondCMS.Tag, Bond)
                historyLoader.StartTask(item.MetaData.RIC, "DATE, CLOSE", DateTime.Today.AddDays(-90), DateTime.Today, timeOut:=15)
            Catch ex As Exception
                Logger.ErrorException("Got exception", ex)
                Logger.Error("Exception = {0}", ex.ToString())
            End Try
        End Sub

        Private Sub RemoveHistoryTSMIClick(ByVal sender As Object, ByVal e As EventArgs) Handles RemoveHistoryTSMI.Click
            Try
                Dim tg = CType(HistoryCMS.Tag, HistoryPointTag)
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
                Dim groupSelect As New GroupSelectForm
                groupSelect.InitGroupList((From q In _ansamble.Items Where TypeOf q.Value Is BondGroup Select New IdValue(Of Long, String)(q.Value.Identity, q.Value.Name)).ToDictionary(Of Long, String)(Function(x) x.Id, Function(x) x.Value))
                If groupSelect.ShowDialog() = DialogResult.OK Then
                    Dim grp As BondGroup
                    If groupSelect.UseNew Then
                        grp = New BondGroup(_ansamble, groupSelect.NewName, bondSelector.SelectedRICs, groupSelect.NewColor, New FieldSet(groupSelect.LayoutId))
                        AddHandler grp.Updated, Sub(items) OnGroupUpdated(grp, items)
                        AddHandler grp.Cleared, Sub() ClearSeries(grp.Identity)
                        AddHandler grp.UpdatedSpread, Sub(data As List(Of PointOfCurve), ord As IOrdinate) If _ansamble.YSource = ord Then OnGroupUpdated(grp, data)
                        _ansamble.Items.Add(grp)
                    Else
                        grp = _ansamble.Items(groupSelect.ExistingGroupId)
                        grp.AddRics(bondSelector.SelectedRICs)
                    End If
                    grp.Subscribe()
                End If
            End If
        End Sub
#End Region

#Region "e) Assembly and curves events"
        Private Sub ClearSeries(ByVal id As Long)
            Logger.Trace("ClearSeries({0})", id)
            GuiAsync(
                Sub()
                    Dim series As Series = TheChart.Series.FindByName(id)

                    If series IsNot Nothing Then
                        series.Points.Clear()
                        TheChart.Series.Remove(series)
                    End If

                    With TheChart.Legends(0).CustomItems
                        While .Any(Function(elem) elem.Tag = id)
                            .Remove(.First(Function(elem) elem.Tag = id))
                        End While
                    End With

                    SetChartMinMax()
                End Sub)
        End Sub

        Private Sub OnOrdinate(obj As IOrdinate) Handles _ansamble.Ordinate
            SetYAxisMode(obj)
        End Sub
#End Region
#End Region

        Private Sub IssuerNameSeriesTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles IssuerNameSeriesTSMI.Click
            If BondCMS.Tag Is Nothing OrElse Not TypeOf BondCMS.Tag Is Bond Then Return
            BondCMS.Tag.LabelMode = LabelMode.IssuerAndSeries
        End Sub

        Private Sub ShortNameTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ShortNameTSMI.Click
            If BondCMS.Tag Is Nothing OrElse Not TypeOf BondCMS.Tag Is Bond Then Return
            BondCMS.Tag.LabelMode = LabelMode.IssuerCpnMat
        End Sub

        Private Sub DescriptionTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DescriptionTSMI.Click
            If BondCMS.Tag Is Nothing OrElse Not TypeOf BondCMS.Tag Is Bond Then Return
            BondCMS.Tag.LabelMode = LabelMode.Description
        End Sub

        Private Sub SeriesOnlyTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SeriesOnlyTSMI.Click
            If BondCMS.Tag Is Nothing OrElse Not TypeOf BondCMS.Tag Is Bond Then Return
            BondCMS.Tag.LabelMode = LabelMode.SeriesOnly
        End Sub

        Private Sub SeriesIssuerNameAndSeriesTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SeriesIssuerNameAndSeriesTSMI.Click
            _ansamble.Items(BondSetCMS.Tag).SetLabelMode(LabelMode.IssuerAndSeries)
        End Sub

        Private Sub SeriesSeriesOnlyTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SeriesSeriesOnlyTSMI.Click
            _ansamble.Items(BondSetCMS.Tag).SetLabelMode(LabelMode.SeriesOnly)
        End Sub

        Private Sub SeriesDescriptionTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SeriesDescriptionTSMI.Click
            _ansamble.Items(BondSetCMS.Tag).SetLabelMode(LabelMode.Description)
        End Sub

        Private Sub IssuerCouponMaturityTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles IssuerCouponMaturityTSMI.Click
            _ansamble.Items(BondSetCMS.Tag).SetLabelMode(LabelMode.IssuerCpnMat)
        End Sub

        Private Sub BondCurveTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BondCurveTSMI.Click
            If BondCMS.Tag Is Nothing Then Return
            BondCurveCMS.Tag = CType(BondCMS.Tag, Bond).Parent.Identity
            BondCurveCMS.Show(MousePosition)
        End Sub

        Private Sub DeleteBondCurveTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DeleteBondCurveTSMI.Click
            If BondCurveCMS.Tag Is Nothing Then Return
            Dim crv = CType(_ansamble(BondCurveCMS.Tag), BondCurve)
            _ansamble(crv.Identity).Cleanup()
        End Sub

        Private Sub BootstrappingToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BootstrappingToolStripMenuItem.Click
            If BondCurveCMS.Tag Is Nothing Then Return
            Dim crv = CType(_ansamble(BondCurveCMS.Tag), BondCurve)
            crv.Bootstrap()
        End Sub

        Private Sub ShowBondCurveItemsTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ShowBondCurveItemsTSMI.Click
            If BondCurveCMS.Tag Is Nothing Then Return
            Dim frm As New BondCurveItemsForm
            frm.Curve = CType(_ansamble(BondCurveCMS.Tag), BondCurve)
            frm.Show()
        End Sub

        Private Sub LinRegTSMI_Click(ByVal sender As Object, ByVal e As EventArgs) Handles _
            LinRegTSMI.Click, LogRegTSMI.Click, InvRegTSMI.Click, PowRegTSMI.Click, _
            PolyRegTSMI.Click, NSSRegTSMI.Click, CubSplineTSMI.Click, _
            VasicekTSMI.Click, CIRRTSMI.Click

            If BondCurveCMS.Tag Is Nothing Then Return
            Dim curve = CType(_ansamble(BondCurveCMS.Tag), BondCurve)
            Dim snd = CType(sender, ToolStripMenuItem)
            curve.SetFitMode(snd.Tag.ToString())
        End Sub

        Private Sub BondCurveCMS_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles BondCurveCMS.Opening
            If BondCurveCMS.Tag Is Nothing Then Return
            Dim curve = CType(_ansamble(BondCurveCMS.Tag), BondCurve)
            BootstrappingToolStripMenuItem.Checked = curve.Bootstrapped
            For Each item As ToolStripMenuItem In (From elem In InterpolationTSMI.DropDownItems Where TypeOf elem Is ToolStripMenuItem)
                item.Checked = curve.EstModel IsNot Nothing AndAlso item.Tag = curve.EstModel.ItemName
            Next
        End Sub

        Private Sub SelectDateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As EventArgs) Handles SelectDateToolStripMenuItem.Click
            If BondCurveCMS.Tag Is Nothing Then Return
            Dim curve = CType(_ansamble(BondCurveCMS.Tag), BondCurve)
            Dim datePicker = New DatePickerForm
            If datePicker.ShowDialog() = DialogResult.OK Then curve.GroupDate = datePicker.TheCalendar.SelectionEnd
        End Sub

        Private Sub IssuerSeriesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles IssuerSeriesToolStripMenuItem.Click
            For Each bnd In GetBonds()
                bnd.LabelMode = LabelMode.IssuerAndSeries
            Next
        End Sub

        Private Sub IssuerCouponMaturityToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles IssuerCouponMaturityToolStripMenuItem.Click
            For Each bnd In GetBonds()
                bnd.LabelMode = LabelMode.IssuerCpnMat
            Next
        End Sub

        Private Function GetBonds() As IEnumerable(Of Bond)
            If BondCurveCMS.Tag Is Nothing OrElse Not IsNumeric(BondCurveCMS.Tag) Then Return Nothing
            Return _ansamble.Items(BondCurveCMS.Tag).Bonds()
        End Function

        Private Sub DescriptionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DescriptionToolStripMenuItem.Click
            For Each bnd In GetBonds()
                bnd.LabelMode = LabelMode.Description
            Next
        End Sub

        Private Sub SeriesOnlyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SeriesOnlyToolStripMenuItem.Click
            For Each bnd In GetBonds()
                bnd.LabelMode = LabelMode.SeriesOnly
            Next
        End Sub

        Private Sub BondCurveTSMI_DropDownOpening(sender As Object, e As EventArgs) Handles BondCurveTSMI.DropDownOpened
            If BondCMS.Tag Is Nothing Then Return

            Dim a = New Thread(New ThreadStart(Sub() GuiAsync(Sub()
                                                                  XxxToolStripMenuItem.PerformClick()
                                                                  BondCurveCMS.Tag = CType(BondCMS.Tag, Bond).Parent.Identity
                                                                  BondCurveCMS.Show(MousePosition)
                                                              End Sub)))
            a.Start()
        End Sub

        Private Sub DefaultToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DefaultToolStripMenuItem.Click, _
            YTMToolStripMenuItem.Click, YTWToolStripMenuItem.Click, YTAToolStripMenuItem.Click, YTBToolStripMenuItem.Click, _
            YTPToolStripMenuItem.Click, YTCToolStripMenuItem.Click

            Dim nm = CType(sender, ToolStripMenuItem).Text
            If nm = "Default" Then nm = ""

            Dim bond = CType(BondCMS.Tag, Bond)
            bond.YieldMode = nm
        End Sub

        Private Sub DefaultTSMI_Click(sender As Object, e As EventArgs) Handles DefaultTSMI.Click, _
            YtmTSMI.Click, YtwTSMI.Click, YtaTSMI.Click, YtbTSMI.Click, YtpTSMI.Click, YtcTSMI.Click
            Dim nm = CType(sender, ToolStripMenuItem).Text
            If nm = "Default" Then nm = ""

            Dim group = _ansamble.Items(BondSetCMS.Tag)
            group.SetYieldMode(nm)
        End Sub

        Private Sub SelectDateTSMI_Click(sender As Object, e As EventArgs) Handles SelectDateTSMI.Click
            If BondSetCMS.Tag Is Nothing Then Return
            Dim curve = CType(_ansamble(BondSetCMS.Tag), BondGroup)
            If curve Is Nothing Then Return
            Dim datePicker = New DatePickerForm
            If datePicker.ShowDialog() = DialogResult.OK Then curve.GroupDate = datePicker.TheCalendar.SelectionEnd
        End Sub

        Private Sub SelectChartDateTSMI_Click(sender As Object, e As EventArgs) Handles SelectChartDateTSMI.Click
            Dim datePicker = New DatePickerForm
            If datePicker.ShowDialog() = DialogResult.OK Then _ansamble.GroupDate = datePicker.TheCalendar.SelectionEnd

        End Sub
    End Class
End Namespace