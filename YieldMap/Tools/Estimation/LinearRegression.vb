Imports YieldMap.Forms.ChartForm
Imports DotNumerics.Optimization
Imports MathNet.Numerics.Interpolation.Algorithms
Imports MathNet.Numerics.LinearAlgebra.Double
Imports System.Reflection
Imports YieldMap.Curves

Namespace Tools.Estimation
    Public Enum EstimationType
        Interpolation
        Estimation
    End Enum

    Public Class EstimationModel
        Implements IComparable(Of EstimationModel)
        Public Shared ReadOnly Lin As New EstimationModel(EstimationType.Estimation, "Lin", "Linear regression", True)
        Public Shared ReadOnly Log As New EstimationModel(EstimationType.Estimation, "Log", "Logarithmic regression", True)
        Public Shared ReadOnly Inv As New EstimationModel(EstimationType.Estimation, "Inv", "Inverse regression", True)
        Public Shared ReadOnly Pow As New EstimationModel(EstimationType.Estimation, "Pow", "Power regression", True)
        Public Shared ReadOnly Poly6 As New EstimationModel(EstimationType.Estimation, "Poly6", "Polynimial regression of power 6 ", True)
        Public Shared ReadOnly Best As New EstimationModel(EstimationType.Estimation, "Best", "Best fit  regression", False)
        Public Shared ReadOnly NSS As New EstimationModel(EstimationType.Estimation, "NSS", "Nelson-Siegel-Svensson", True)
        Public Shared ReadOnly LinInterp As New EstimationModel(EstimationType.Interpolation, "LinInterp", "Linear interpolation", True)
        Public Shared ReadOnly CubicSpline As New EstimationModel(EstimationType.Interpolation, "CubicSpline", "Cubic spline", True)

        Public Shared ReadOnly DefaultModel As EstimationModel = LinInterp

        Private ReadOnly _estimationType As EstimationType
        Private ReadOnly _itemName As String
        Private ReadOnly _fullName As String
        Private ReadOnly _enabled As Boolean

        Private Sub New(ByVal estimationType As EstimationType, ByVal itemName As String, ByVal fullName As String, ByVal enabled As Boolean)
            _estimationType = estimationType
            _itemName = itemName
            _fullName = fullName
            _enabled = enabled
        End Sub

        Public ReadOnly Property EstimationType As EstimationType
            Get
                Return _estimationType
            End Get
        End Property

        Public ReadOnly Property ItemName As String
            Get
                Return _itemName
            End Get
        End Property

        Public ReadOnly Property FullName As String
            Get
                Return _fullName
            End Get
        End Property

        Public ReadOnly Property Enabled As Boolean
            Get
                Return _enabled
            End Get
        End Property

        Public Shared Operator =(ByVal model1 As EstimationModel, ByVal model2 As EstimationModel) As Boolean
            Return model1.ItemName = model2.ItemName
        End Operator

        Public Overrides Function ToString() As String
            Return _itemName
        End Function

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If Not TypeOf obj Is EstimationModel Then Return False
            Dim m = CType(obj, EstimationModel)
            Return m.ItemName.Equals(_itemName)
        End Function

        Public Shared Operator <>(ByVal model1 As EstimationModel, ByVal model2 As EstimationModel) As Boolean
            Return model1.ItemName <> model2.ItemName
        End Operator

        Public Shared Function GetEnabledModels() As EstimationModel()
            Dim res As New List(Of EstimationModel)
            Dim fields = GetType(EstimationModel).GetFields()
            fields.ToList.ForEach(
                Sub(field As FieldInfo)
                    Dim x As EstimationModel
                    x = CType(field.GetValue(Nothing), EstimationModel)
                    If x.Enabled Then res.Add(x)
                End Sub)
            res.Sort()
            Return res.Distinct().ToArray()
        End Function

        Public Function CompareTo(ByVal other As EstimationModel) As Integer Implements IComparable(Of EstimationModel).CompareTo
            If other.EstimationType.Equals(EstimationType) Then
                Return String.Compare(ItemName, other.ItemName, StringComparison.Ordinal)
            Else
                Return other.EstimationType.CompareTo(EstimationType)
            End If
        End Function

        Public Shared Function FromName(ByVal mode As String) As EstimationModel
            Dim fields = GetType(EstimationModel).GetFields()
            Dim res = From fld In fields Let elem = CType(fld.GetValue(Nothing), EstimationModel) Where elem.ItemName = mode Select elem
            Return If(res.Any, res.First, Nothing)
        End Function
    End Class

#Region "Regression models"
    Public Class LinearRegression
        Protected A As Double
        Protected B As Double

        Public Overridable Function Fit(x As List(Of Double), y As List(Of Double)) As Double
            Dim len As Integer = x.Count

            Dim ss As Double = len
            Dim sx As Double = 0
            Dim sy As Double = 0
            Dim st2 As Double = 0
            Dim t As Double
            Dim sxoss As Double

            For i As Integer = 0 To len - 1
                sx += x(i)
                sy += y(i)
            Next
            sxoss = sx / ss

            A = 0 : B = 0
            For i As Integer = 0 To len - 1
                t = x(i) - sxoss
                st2 += t * t
                B += t * y(i)
            Next
            B /= st2
            A = (sy - sx * B) / ss

            Dim res As Double = 0
            For i As Integer = 0 To len - 1
                Dim d As Double = (y(i) - A - B * x(i))
                res += d * d
            Next

            Return Math.Sqrt(res)
        End Function

        Public Overridable Function Estimate(x As Double) As Double
            Return A + B * x
        End Function

        Overridable Function GetFormula() As String
            Return String.Format("Y = {0:F4} + {1:F4}*X ", A, B)
        End Function
    End Class

    Public NotInheritable Class LogLinearRegression
        Inherits LinearRegression

        Public Overrides Function Fit(x As List(Of Double), y As List(Of Double)) As Double
            Dim logX As List(Of Double) = (From val In x Select Math.Log(1 + val)).ToList()
            MyBase.Fit(logX, y)

            Dim res As Double = 0
            For i = 0 To x.Count() - 1
                Dim d As Double = y(i) - Estimate(x(i))
                res += d * d
            Next
            Return Math.Sqrt(res)
        End Function

        Public Overrides Function Estimate(x As Double) As Double
            Return A + B * Math.Log(1 + x)
        End Function

        Overrides Function GetFormula() As String
            Return String.Format("Y = {0:F4} + {1:F4}*Log(1 + X) ", A, B)
        End Function
    End Class

    Public NotInheritable Class PowerRegression
        Inherits LinearRegression

        Public Overrides Function Fit(x As List(Of Double), y As List(Of Double)) As Double
            Dim logY As List(Of Double) = (From val In y Select Math.Log(val)).ToList()
            MyBase.Fit(x, logY)

            Dim res As Double = 0
            For i = 0 To x.Count() - 1
                Dim d As Double = y(i) - Estimate(x(i))
                res += d * d
            Next
            Return Math.Sqrt(res)
        End Function

        Public Overrides Function Estimate(x As Double) As Double
            Return Math.Exp(A + B * x)
            ' y = a * b^x => ln(y) = ln(a) + ln(b)x = a' + b'x => y = exp(a'+b'x)
        End Function

        Overrides Function GetFormula() As String
            Return String.Format("Y = {0:F4}*{1:F4}^X", Math.Exp(A), Math.Exp(B))
        End Function
    End Class

    Public NotInheritable Class InverseRegression
        Inherits LinearRegression

        Public Overrides Function Fit(x As List(Of Double), y As List(Of Double)) As Double
            Dim invY As List(Of Double) = (From val In y Select 1 / val).ToList()
            MyBase.Fit(x, invY)

            Dim res As Double = 0
            For i = 0 To x.Count() - 1
                Dim d As Double = y(i) - Estimate(x(i))
                res += d * d
            Next
            Return Math.Sqrt(res)
        End Function

        Public Overrides Function Estimate(x As Double) As Double
            Return 1 / (A + B * x)
        End Function

        Overrides Function GetFormula() As String
            Return String.Format("Y = 1/({0:F4} + {1:F4}*X)", A, B)
        End Function
    End Class

    Public NotInheritable Class Poly6Regression
        Inherits LinearRegression

        Private Const P As Integer = 6          ' power
        Private _n As Integer                    ' number of features

        Private _theta(P) As Double

        Public Overrides Function Estimate(ByVal x As Double) As Double
            Dim res As Double = _theta(0)
            For i = 1 To P
                res = res + _theta(i) * Math.Pow(x, i)
            Next
            Return res
        End Function

        Public Overrides Function Fit(ByVal x As List(Of Double), ByVal y As List(Of Double)) As Double
            _n = x.Count()
            Dim anY = New DenseVector(y.ToArray())

            Dim anT = New DenseMatrix(P + 1, _n)
            For j = 0 To _n - 1
                anT(0, j) = 1
            Next

            For i = 1 To P
                For j = 0 To _n - 1
                    anT(i, j) = Math.Pow(x(j), i)
                Next
            Next

            _theta = anT.Multiply(anT.Transpose()).Inverse().Multiply(anT).Multiply(anY).ToArray() ' inv(t't)*t*y'

            Dim res As Double
            For i = 0 To _n - 1
                res += Math.Pow(y(i) - Estimate(x(i)), 2)
            Next
            Return Math.Sqrt(res)
        End Function

        Public Overrides Function GetFormula() As String
            Dim res = String.Format("Y = {0:P2} + {1:P2}*x", _theta(0), _theta(1))
            For i = 2 To P
                If Math.Abs(_theta(i)) > 0.000001 Then
                    res += String.Format(" + {0:P2}*x^{1}", _theta(i), i)
                End If
            Next
            Return res
        End Function
    End Class
#End Region

#Region "Interpolation models"
    Public Class CubicSpline
        Public Function Interpolate(ByVal xv As List(Of Double), ByVal yv As List(Of Double)) As List(Of XY)
            Dim spline As New CubicSplineInterpolation(xv, yv)
            Return Commons.GetRange(xv.Min, xv.Max, 300).Select(Function(anX) New XY With {.X = anX, .Y = spline.Interpolate(anX)})
        End Function
    End Class

    Public Class NelsonSiegelSvensson
        Private _xv, _yv As List(Of Double)
        Private _n As Double

        Private Shared Function NSS(ByVal t As Double, ByVal p() As Double) As Double
            Dim b1 = p(0), b2 = p(1), b3 = p(2), b4 = p(3), l1 = p(4), l2 = p(5)
            Dim tl1 = t / l1, tl2 = t / l2
            Dim etl1 = Math.Exp(-tl1)
            Dim metl1 = (1 - etl1) / tl1
            Dim etl2 = Math.Exp(-tl2)
            Dim metl2 = (1 - etl2) / tl2
            Dim res = b1 + b2 * metl1 + b3 * (metl1 - etl1) + b4 * (metl2 - etl2)
            Return res
        End Function

        Private Function NSSCost(ByVal p() As Double) As Double
            Dim res As Double
            For i = 0 To _n - 1
                res += (NSS(_xv(i), p) - _yv(i)) ^ 2
            Next
            Return Math.Sqrt(res)
        End Function

        Private Function NSSCg(ByVal p() As Double) As Double()
            Dim res(5) As Double
            For i = 0 To _n - 1
                Dim delta = 2 * (NSS(_xv(i), p) - _yv(i))
                res(0) += delta
                res(1) += delta * NSSCgDb2(_xv(i), p)
                res(2) += delta * NSSCgDb3(_xv(i), p)
                res(3) += delta * NSSCgDb4(_xv(i), p)
                res(4) += delta * NSSCgDl1(_xv(i), p)
                res(5) += delta * NSSCgDl2(_xv(i), p)
            Next
            Return res
        End Function

        Private Shared Function NSSCgDb2(ByVal t As Double, ByVal p() As Double) As Double
            Dim l1 = p(4), tl1 = t / l1
            Dim etl1 = Math.Exp(-tl1)
            Dim metl1 = (1 - etl1) / tl1
            Return metl1
        End Function

        Private Shared Function NSSCgDb3(ByVal t As Double, ByVal p() As Double) As Double
            Dim l1 = p(4), tl1 = t / l1
            Dim etl1 = Math.Exp(-tl1)
            Dim metl1 = (1 - etl1) / tl1
            Return metl1 - etl1
        End Function

        Private Shared Function NSSCgDb4(ByVal t As Double, ByVal p() As Double) As Double
            Dim l2 = p(5), tl2 = t / l2
            Dim etl2 = Math.Exp(-tl2)
            Dim metl2 = (1 - etl2) / tl2
            Return metl2 - etl2
        End Function

        Private Shared Function NSSCgDl1(ByVal t As Double, ByVal p() As Double) As Double
            Dim b2 = p(1), b3 = p(2), l1 = p(4)
            Dim tl1 = t / l1
            Dim etl1 = Math.Exp(-tl1)
            Dim res = -(b2 * etl1 / l1) + (b2 * (1 - etl1) / t) + b3 * (-(etl1 / l1) + ((1 - etl1) / t) - (t * etl1 / (l1 ^ 2)))
            Return res
        End Function

        Private Shared Function NSSCgDl2(ByVal t As Double, ByVal p() As Double) As Double
            Dim b4 = p(3), l2 = p(5)
            Dim tl2 = t / l2
            Dim etl2 = Math.Exp(-tl2)
            Dim res = b4 * (-(etl2 / l2) + ((1 - etl2) / t) - (t * etl2 / (l2 ^ 2)))
            Return res
        End Function

        Public Function Fit(ByVal x As List(Of Double), ByVal y As List(Of Double)) As List(Of XY)
            _xv = x
            Dim avgY = y.Average()
            _yv = y.Select(Function(anY) 10 * anY / avgY).ToList()
            _n = x.Count()

            Dim vars = New OptBoundVariable() {
               New OptBoundVariable() With {.Name = "b1", .LowerBound = 0, .InitialGuess = 1},
               New OptBoundVariable() With {.Name = "b2", .InitialGuess = 1},
               New OptBoundVariable() With {.Name = "b3", .InitialGuess = 1},
               New OptBoundVariable() With {.Name = "b4", .InitialGuess = 1},
               New OptBoundVariable() With {.Name = "l1", .LowerBound = 0.0001, .InitialGuess = 1},
               New OptBoundVariable() With {.Name = "l2", .LowerBound = 0.0001, .InitialGuess = 1}
            }

            Dim lbfgsb As New L_BFGS_B
            Dim minimum = lbfgsb.ComputeMin(AddressOf NSSCost, AddressOf NSSCg, vars)

            Return Commons.GetRange(_xv.Min, _xv.Max, 50).Select(Function(anX) New XY With {.X = anX, .Y = avgY * NSS(anX, minimum) / 10}).ToList()
        End Function
    End Class
#End Region

#Region "Estimator itself"
    Public NotInheritable Class Estimator
        Private ReadOnly _estimationModel As EstimationModel

        Private _regression As LinearRegression
        Private ReadOnly _regressions() As LinearRegression = {
            New LinearRegression,
            New InverseRegression,
            New PowerRegression,
            New LogLinearRegression,
            New Poly6Regression
        }

        Private Const Epsilon As Double = 0.01

        Public Sub New()
            _estimationModel = EstimationModel.DefaultModel
            Select Case _estimationModel
                Case EstimationModel.Lin : _regression = New LinearRegression()
                Case EstimationModel.Inv : _regression = New InverseRegression()
                Case EstimationModel.Pow : _regression = New PowerRegression
                Case EstimationModel.Log : _regression = New LogLinearRegression()
                Case EstimationModel.Poly6 : _regression = New Poly6Regression()
            End Select
        End Sub

        Public Sub New(ByVal estimationModel As EstimationModel)
            _estimationModel = estimationModel
            Select Case _estimationModel
                Case estimationModel.Lin : _regression = New LinearRegression()
                Case estimationModel.Inv : _regression = New InverseRegression()
                Case estimationModel.Pow : _regression = New PowerRegression
                Case estimationModel.Log : _regression = New LogLinearRegression()
                Case estimationModel.Poly6 : _regression = New Poly6Regression()
            End Select
        End Sub

        Public Function Approximate(ByVal data As List(Of YieldDuration), ByVal what As SpreadMode) As List(Of XY)
            If what Is Nothing Then what = SpreadMode.Yield
            Dim dt = XY.ConvertToXY(data, what)
            Dim x = XY.GetX(dt)
            Dim y = XY.GetY(dt)
            If _estimationModel.EstimationType = EstimationType.Estimation Then
                If dt.Count <= 1 Then Return Nothing
                If _estimationModel = EstimationModel.Best Then
                    Dim fits = _regressions.Select(
                        Function(regr)
                            Try
                                Return New FitElem With {.Regression = regr, .SSE = regr.Fit(x, y)}
                            Catch ex As Exception
                                Return New FitElem With {.Regression = regr, .SSE = Double.MaxValue}
                            End Try
                        End Function).ToList()
                    Dim minSSE = fits.Select(Function(fit) fit.SSE).Min
                    _regression = fits.First(Function(fit) Math.Abs(fit.SSE - minSSE) < Epsilon).Regression
                ElseIf _estimationModel = EstimationModel.NSS Then
                    Return (New NelsonSiegelSvensson).Fit(x, y)
                Else
                    _regression.Fit(x, y)
                End If

                Return Commons.GetRange(x.Min, x.Max, 100).Select(Function(anX) New XY With {.X = anX, .Y = _regression.Estimate(anX)}).ToList()
            Else
                If _estimationModel = EstimationModel.CubicSpline Then
                    Return (New CubicSpline).Interpolate(x, y)
                Else
                    Return XY.ConvertToXY(data, what)
                End If
            End If
        End Function

        Private Class FitElem
            Public Regression As LinearRegression
            Public SSE As Double
        End Class
    End Class

    Public Class XY
        Public X As Double
        Public Y As Double

        Public Shared Function ConvertToXY(ByVal data As List(Of YieldDuration), ByVal mode As SpreadMode) As List(Of XY)
            Dim x As List(Of Double)
            Dim y As List(Of Double)
            Select Case mode
                Case SpreadMode.Yield
                    x = data.Select(Function(elem) elem.Duration).ToList()
                    y = data.Select(Function(elem) elem.Yield).ToList()
                Case SpreadMode.PointSpread
                    x = data.Where(Function(elem) elem.PointSpread.HasValue).Select(Function(elem) elem.Duration).ToList()
                    y = data.Where(Function(elem) elem.PointSpread.HasValue).Select(Function(elem) elem.PointSpread.Value).ToList()
                Case SpreadMode.ZSpread
                    x = data.Where(Function(elem) elem.ZSpread.HasValue).Select(Function(elem) elem.Duration).ToList()
                    y = data.Where(Function(elem) elem.ZSpread.HasValue).Select(Function(elem) elem.ZSpread.Value).ToList()
                Case SpreadMode.ASWSpread
                    x = data.Where(Function(elem) elem.ASWSpread.HasValue).Select(Function(elem) elem.Duration).ToList()
                    y = data.Where(Function(elem) elem.ASWSpread.HasValue).Select(Function(elem) elem.ASWSpread.Value).ToList()
            End Select
            Return PackXY(x, y)
        End Function

        Public Shared Function GetX(ByVal xy As List(Of XY)) As List(Of Double)
            Return xy.Select(Function(elem) elem.X).ToList()
        End Function

        Public Shared Function GetY(ByVal xy As List(Of XY)) As List(Of Double)
            Return xy.Select(Function(elem) elem.Y).ToList()
        End Function

        Public Shared Function PackXY(ByVal x As List(Of Double), ByVal y As List(Of Double)) As List(Of XY)
            Dim res As New List(Of XY)
            For i = 0 To x.Count() - 1
                res.Add(New XY With {.X = x(i), .Y = y(i)})
            Next
            Return res
        End Function
    End Class
#End Region
End Namespace