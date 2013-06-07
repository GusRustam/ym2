Imports AdfinXAnalyticsFunctions
Imports NLog
Imports System.Collections.ObjectModel
Imports ReutersData

Namespace Tools.Elements
    Public Interface IOrdinate
        Sub SetValue(ByVal bpd As BasePointDescription, ByVal curve As ICurve, Optional ByVal val As Double? = Nothing)
        Sub ClearValue(ByVal bpd As BasePointDescription)
        ReadOnly Property NameProperty() As String
        ReadOnly Property DescrProperty() As String
        Function GetValue(ByVal bpd As BasePointDescription) As Double?
    End Interface

    Public MustInherit Class OrdinateBase
        Implements IOrdinate
        Implements IEquatable(Of OrdinateBase)
        Protected ReadOnly BondModule As AdxBondModule = Eikon.Sdk.CreateAdxBondModule()
        Public MustOverride Sub SetValue(ByVal bpd As BasePointDescription, ByVal curve As ICurve, Optional ByVal val As Double? = Nothing) Implements IOrdinate.SetValue
        Public MustOverride Function GetValue(ByVal bpd As BasePointDescription) As Double? Implements IOrdinate.GetValue
        Public MustOverride Sub ClearValue(ByVal bpd As BasePointDescription) Implements IOrdinate.ClearValue
        Protected ReadOnly Name As String
        Protected ReadOnly Descr As String

        Public Overloads Function Equals(ByVal other As OrdinateBase) As Boolean Implements IEquatable(Of OrdinateBase).Equals
            If ReferenceEquals(Nothing, other) Then Return False
            If ReferenceEquals(Me, other) Then Return True
            Return String.Equals(Name, other.Name)
        End Function

        Public Overrides Function ToString() As String
            Return Descr
        End Function

        Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
            If ReferenceEquals(Nothing, obj) Then Return False
            If ReferenceEquals(Me, obj) Then Return True
            If obj.GetType IsNot Me.GetType Then Return False
            Return Equals(DirectCast(obj, OrdinateBase))
        End Function

        Public Overrides Function GetHashCode() As Integer
            If Name Is Nothing Then Return 0
            Return Name.GetHashCode
        End Function

        Public Shared Operator =(ByVal left As OrdinateBase, ByVal right As OrdinateBase) As Boolean
            Return Equals(left, right)
        End Operator

        Public Shared Operator <>(ByVal left As OrdinateBase, ByVal right As OrdinateBase) As Boolean
            Return Not Equals(left, right)
        End Operator

        Public ReadOnly Property NameProperty() As String Implements IOrdinate.NameProperty
            Get
                Return Name
            End Get
        End Property

        Public ReadOnly Property DescrProperty() As String Implements IOrdinate.DescrProperty
            Get
                Return Descr
            End Get
        End Property


        Protected Sub New(ByVal name As String, ByVal descr As String)
            Me.Name = name
            Me.Descr = descr
        End Sub

    End Class

    Public Class OrdinateYield
        Inherits OrdinateBase

        Private Shared ReadOnly Inst As New OrdinateYield("Yield", "Yield")

        Public Sub New(ByVal name As String, ByVal descr As String)
            MyBase.New(name, descr)
        End Sub

        ' тогда вопрос, что такое val. Давайте щетать, что для облигации это иё цена, ась?
        Public Overrides Sub SetValue(ByVal bpd As BasePointDescription, ByVal curve As ICurve, Optional ByVal val As Double? = Nothing)
            ' этот метод не должен вызываться ни для бондов, ни для кривой, поскольку они получают свои доходности из загруженных данных
            Throw New InvalidOperationException()
            'bpd.Yield = val
        End Sub

        Public Overrides Sub ClearValue(ByVal bpd As BasePointDescription)
            bpd.ClearYield()
        End Sub

        Public Overrides Function GetValue(ByVal bpd As BasePointDescription) As Double?
            Return bpd.Yield
        End Function

        Public Shared ReadOnly Property Instance() As OrdinateYield
            Get
                Return Inst
            End Get
        End Property
    End Class

    Public Class OrdinatePointSpread
        Inherits OrdinateBase

        Private Shared ReadOnly Inst As New OrdinatePointSpread("PointSpread", "Point spread")

        Public Sub New(ByVal name As String, ByVal descr As String)
            MyBase.New(name, descr)
        End Sub

        Private Shared Function CalcPntSprd(ByVal rateArray As Array, ByVal dscr As BasePointDescription) As Double?
            Dim data As New List(Of XY)
            For i = rateArray.GetLowerBound(0) To rateArray.GetUpperBound(0)
                data.Add(New XY() With {.Y = rateArray.GetValue(i, 1), .X = (CDate(rateArray.GetValue(i, 0)) - dscr.YieldAtDate).Days / 365})
            Next

            Dim yld = dscr.Yield(Nothing)
            Dim duration = dscr.Duration

            If data.Count() >= 2 Then
                Dim minDur = data.Select(Function(theXY) theXY.X).Min
                Dim maxDur = data.Select(Function(theXY) theXY.X).Max
                If duration < minDur Or duration > maxDur Then Return Nothing

                For i = 0 To data.Count() - 2
                    Dim xi = data(i).X
                    Dim xi1 = data(i + 1).X
                    If xi <= duration And xi1 >= duration Then
                        Dim yi = data(i).Y
                        Dim yi1 = data(i + 1).Y
                        Dim a = (xi1 - duration) / (xi1 - xi)
                        Return (yld - (a * yi + (1 - a) * yi1)) * 10000
                    End If
                Next
            End If
            Return Nothing
        End Function

        Public Overrides Sub SetValue(ByVal bpd As BasePointDescription, ByVal curve As ICurve, Optional ByVal val As Double? = Nothing)
            If val IsNot Nothing Then
                bpd.PointSpread = val
            Else
                bpd.PointSpread = CalcPntSprd(curve.RateArray, bpd)
            End If
        End Sub

        Public Overrides Sub ClearValue(ByVal bpd As BasePointDescription)
            bpd.PointSpread = Nothing
        End Sub

        Public Overrides Function GetValue(ByVal bpd As BasePointDescription) As Double?
            Return bpd.PointSpread
        End Function

        Public Shared ReadOnly Property Instance() As OrdinateBase
            Get
                Return Inst
            End Get
        End Property
    End Class

    Public Class OrdinateAswSpread
        Inherits OrdinateBase

        Private Shared ReadOnly Inst As New OrdinateAswSpread("ASWSpread", "Asset swap spread")
        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(OrdinateZSpread))
        Private Shared ReadOnly SwapModule As AdxSwapModule = Eikon.Sdk.CreateAdxSwapModule()

        Public Sub New(ByVal name As String, ByVal descr As String)
            MyBase.New(name, descr)
        End Sub

        Private Function CalcAswSprd(ByVal rateArray As Array, ByVal floatLegStructure As String, ByVal floatingRate As Double, ByRef dscr As BondPointDescription) As Double?
            Dim data = dscr.ParentBond.MetaData
            If dscr.Price > 0 Then
                Try
                    Dim settleDate = BondModule.BdSettle(DateTime.Today, data.PaymentStructure)
                    Dim res As Array = SwapModule.AdAssetSwapBdSpread(settleDate, data.Maturity, rateArray, dscr.Price / 100.0,
                                                                      data.Coupon / 100.0, floatingRate, data.PaymentStructure,
                                                                      floatLegStructure, "ZCTYPE:RATE IM:LIX RM:YC", "")
                    Return res.GetValue(1, 1) + dscr.ParentBond.UserDefinedSpread(AswSpread)
                Catch ex As Exception
                    Logger.ErrorException("Failed to calculate ASW Spread", ex)
                    Logger.Error("Exception = {0}", ex.ToString())
                    Return Nothing
                End Try
            Else
                Return Nothing
            End If
        End Function

        Public Overrides Sub SetValue(ByVal bpd As BasePointDescription, ByVal curve As ICurve, Optional ByVal val As Double? = Nothing)
            If val IsNot Nothing Then
                bpd.ASWSpread = val
            Else
                If TypeOf curve Is IAswBenchmark AndAlso TypeOf bpd Is BondPointDescription Then
                    Dim asw = CType(curve, IAswBenchmark)
                    bpd.ASWSpread = CalcASWSprd(curve.RateArray, asw.FloatLegStructure, asw.FloatingPointValue, bpd)
                Else
                    ClearValue(bpd)
                End If
            End If
        End Sub

        Public Overrides Sub ClearValue(ByVal bpd As BasePointDescription)
            bpd.ASWSpread = Nothing
        End Sub

        Public Overrides Function GetValue(ByVal bpd As BasePointDescription) As Double?
            Return bpd.ASWSpread
        End Function

        Public Shared ReadOnly Property Instance() As OrdinateAswSpread
            Get
                Return Inst
            End Get
        End Property
    End Class

    Public Class OrdinateOaSpread
        Inherits OrdinateBase

        Private Shared ReadOnly Inst As New OrdinateOaSpread("OASpread", "Option-adjusted spread")

        Public Sub New(ByVal name As String, ByVal descr As String)
            MyBase.New(name, descr)
        End Sub

        Public Overrides Sub SetValue(ByVal bpd As BasePointDescription, ByVal curve As ICurve, Optional ByVal val As Double? = Nothing)
            If val IsNot Nothing Then
                bpd.OASpread = val
            Else
                ClearValue(bpd)
            End If
        End Sub

        Public Overrides Sub ClearValue(ByVal bpd As BasePointDescription)
            bpd.OASpread = Nothing
        End Sub

        Public Overrides Function GetValue(ByVal bpd As BasePointDescription) As Double?
            Return bpd.OASpread
        End Function

        Public Shared ReadOnly Property Instance() As OrdinateOaSpread
            Get
                Return Inst
            End Get
        End Property
    End Class

    Public Class OrdinateZSpread
        Inherits OrdinateBase

        Private Shared ReadOnly Logger As Logger = Logging.GetLogger(GetType(OrdinateZSpread))
        Private Shared ReadOnly Inst As New OrdinateZSpread("ZSpread", "Z Spread")

        Public Sub New(ByVal name As String, ByVal descr As String)
            MyBase.New(name, descr)
        End Sub

        Private Function CalcZSprd(ByVal rateArray As Array, ByRef dscr As BondPointDescription) As Double?
            Dim data = dscr.ParentBond.MetaData
            If dscr.Price > 0 Then
                Try
                    Dim settleDate = BondModule.BdSettle(DateTime.Today, data.PaymentStructure)
                    Return BondModule.AdBondSpread(settleDate, rateArray, dscr.Price / 100.0,
                                                   data.Maturity, data.Coupon / 100.0,
                                                   data.PaymentStructure, "ZCTYPE:RATE IM:LIX RM:YC", "", ""
                                                  ) + dscr.ParentBond.UserDefinedSpread(ZSpread)
                Catch ex As Exception
                    Logger.ErrorException("Failed to calculate Z-Spread", ex)
                    Logger.Error("Exception = {0}", ex.ToString())
                    Return Nothing
                End Try
            End If
            Return Nothing
        End Function

        Public Overrides Sub SetValue(ByVal bpd As BasePointDescription, ByVal curve As ICurve, Optional ByVal val As Double? = Nothing)
            If val IsNot Nothing Then
                bpd.ZSpread = val
            Else
                If TypeOf bpd Is BondPointDescription Then
                    bpd.ZSpread = CalcZSprd(curve.RateArray, bpd)
                Else
                    ClearValue(bpd)
                End If
            End If
        End Sub

        Public Overrides Sub ClearValue(ByVal bpd As BasePointDescription)
            bpd.ZSpread = Nothing
        End Sub

        Public Overrides Function GetValue(ByVal bpd As BasePointDescription) As Double?
            Return bpd.ZSpread
        End Function

        Public Shared ReadOnly Property Instance() As OrdinateZSpread
            Get
                Return Inst
            End Get
        End Property
    End Class

    Public Module Ordinate
        Public Yield As OrdinateBase = OrdinateYield.Instance
        Public PointSpread As OrdinateBase = OrdinatePointSpread.Instance
        Public AswSpread As OrdinateBase = OrdinateAswSpread.Instance
        Public ZSpread As OrdinateBase = OrdinateZSpread.Instance
        Public OaSpread As OrdinateBase = OrdinateOaSpread.Instance

        Public ReadOnly Ordinates As ReadOnlyCollection(Of OrdinateBase) = _
            New ReadOnlyCollection(Of OrdinateBase)({Yield, PointSpread, AswSpread, ZSpread, OaSpread}.ToList())
        Public ReadOnly Spreads As ReadOnlyCollection(Of OrdinateBase) = _
            New ReadOnlyCollection(Of OrdinateBase)({PointSpread, AswSpread, ZSpread, OaSpread}.ToList())

        Public Function FromString(ByVal name As String) As OrdinateBase
            Dim res = (From item In Ordinates Where item.NameProperty = name).ToList
            Return If(res.Any, res.First, Nothing)
        End Function
    End Module
End Namespace