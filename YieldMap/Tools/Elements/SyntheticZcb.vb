Imports AdfinXAnalyticsFunctions
Imports DbManager.Bonds
Imports ReutersData

Namespace Tools.Elements
    Public Class SyntheticZcb
        Inherits Bond

        Private Shared ReadOnly DateModule As AdxDateModule = Eikon.Sdk.CreateAdxDateModule()
        Private Const Threshold As Double = 0.001

        Private Const ZcbPmtStructureTemplate As String = _
           "ACC:A5 IC:L1 CLDR:RUS_FI SETTLE:0WD CFADJ:NO DMC:FOLLOWING EMC:LASTDAY FRQ:ZERO " &
           "PX:CLEAN REFDATE:MATURITY YM:DISCA5 ISSUE:{0}"

        Public Shared ReadOnly Property ZcbPmtStructure(ByVal dt As Date) As String
            Get
                Return String.Format(ZcbPmtStructureTemplate, ReutersDate.DateToReuters(dt))
            End Get
        End Property

        Public Sub New(ByVal parent As Group, ByVal metaData As BondMetadata)
            MyBase.New(parent, metaData)
        End Sub

        Public Sub New(ByVal parent As Group, ByVal dt As Date, ByVal yield As Double, ByVal dur As Double, ByVal issName As String)
            MyBase.New(parent, New BondMetadata(GetName(dur), FindZcbMaturity(dt, yield, dur), 0, ZcbPmtStructure(dt), "RM:YTM", issName, GetName(dur), dt))
        End Sub

        Private Shared Function FindZcbMaturity(ByVal dt As Date, ByVal yld As Double, ByVal dur As Double) As Date?
            Return dt.AddDays(365 * dur / (1 + yld * dur))
        End Function

        'Private Shared Function GetZcbDuration(ByVal mat As Date, ByVal yld As Double) As Double
        '    Throw New NotImplementedException()
        'End Function


        Public Sub New(ByVal parent As Group, ByVal dt As Date, ByVal yield As Double, ByVal dur As Double, ByVal issName As String, ric As String)
            MyBase.New(parent, New BondMetadata(ric, FindZcbMaturity(dt, yield, dur), 0, ZcbPmtStructure(dt), "RM:YTM", issName, GetName(dur), dt))
        End Sub

        Private Shared Function GetName(ByVal dur As Double) As String
            Return String.Format("ZCB {0:N2}", dur)
        End Function

        Public Overrides ReadOnly Property Coupon(ByVal dt As Date) As Double
            Get
                Return 0
            End Get
        End Property
    End Class
End Namespace