Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Globalization
Imports System.ComponentModel

Namespace Tools
    Public Class YieldToWhatConverter
        Inherits TypeConverter
        Public Overrides Function CanConvertFrom(ByVal context As ITypeDescriptorContext, ByVal sourceType As Type) As Boolean
            Return sourceType = GetType(String)
        End Function

        Public Overrides Function ConvertFrom(ByVal context As ITypeDescriptorContext, ByVal culture As CultureInfo, ByVal value As Object) As Object
            If TypeOf value Is String Then
                Return CType(value.ToString(), YieldToWhat)
            End If
            Return Nothing
        End Function
    End Class

    ''' <summary>
    ''' Basic support for different kinds of yield. Java enums would work <b>much</b> better
    ''' </summary>
    ''' <remarks></remarks>
    <TypeConverter(GetType(YieldToWhatConverter))>
    Public Class YieldToWhat
        Implements IComparable(Of YieldToWhat)
        Public Shared Put = New YieldToWhat("Put", "YTP", MarkerStyle.Triangle)
        Public Shared [Call] = New YieldToWhat("Call", "YTC", MarkerStyle.Star5)
        Public Shared Maturity = New YieldToWhat("Maturity", "YTM", MarkerStyle.Circle)

        Private ReadOnly _name As String
        Private ReadOnly _abbr As String
        Private ReadOnly _markerStyle As MarkerStyle

        Private Sub New(ByVal name As String, ByVal abbr As String, ByVal markerStyle As MarkerStyle)
            _name = name
            _abbr = abbr
            _markerStyle = markerStyle
        End Sub

        Public ReadOnly Property Abbr As String
            Get
                Return _abbr
            End Get
        End Property

        Public ReadOnly Property Name As String
            Get
                Return _name
            End Get
        End Property

        Public ReadOnly Property MarkerStyle As MarkerStyle
            Get
                Return _markerStyle
            End Get
        End Property

        Public Function CompareTo(ByVal other As YieldToWhat) As Integer Implements IComparable(Of YieldToWhat).CompareTo
            Return String.Compare(_name, other.Name, StringComparison.Ordinal)
        End Function

        Public Overrides Function ToString() As String
            Return _name
        End Function

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If TypeOf obj Is YieldToWhat Then
                Dim yk = CType(obj, YieldToWhat)
                Return yk.Name = Name
            Else
                Return False
            End If
        End Function

        Public Shared Function TryParse(Of TEnum)(ByVal name As String, ByRef toWhat As TEnum) As Boolean
            If name = Put.Name Then
                toWhat = Put
                Return True
            End If
            If name = [Call].Name Then
                toWhat = [Call]
                Return True
            End If
            If name = Maturity.Name Then
                toWhat = Maturity
                Return True
            End If
            Return False
        End Function

        Public Shared Narrowing Operator CType(x As String) As YieldToWhat
            Dim res As YieldToWhat = YieldToWhat.Call
            If TryParse(x, res) Then
                Return res
            Else
                Return Nothing
            End If
        End Operator

        Public Shared Function GetValues() As Array
            Return {Put, [Call], Maturity}
        End Function

        Public Shared Function Parse(ByVal toWhatStr As String) As YieldToWhat
            If toWhatStr = Put.Name Then Return Put
            If toWhatStr = [Call].Name Then Return [Call]
            If toWhatStr = Maturity.Name Then Return Maturity
            Return Nothing
        End Function
    End Class

    ''' <summary>
    ''' Contains information on one yield: it's value, it's kind and date to which it was calculated
    ''' </summary>
    ''' <remarks></remarks>
    Public Class YieldStructure
        Implements IComparable(Of YieldStructure)
        Public ToWhat As YieldToWhat
        Public Yield As Double
        Public YieldToDate As Date

        Public Function CompareTo(ByVal other As YieldStructure) As Integer Implements IComparable(Of YieldStructure).CompareTo
            Return IIf(Yield < other.Yield, -1, 1)
        End Function

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If TypeOf obj Is YieldStructure Then
                Dim ys = CType(obj, YieldStructure)
                Return ys.ToWhat.Equals(ToWhat) And ys.YieldToDate = YieldToDate
            Else
                Return False
            End If
        End Function

        Public Overrides Function ToString() As String
            If ToWhat IsNot Nothing Then
                Return String.Format("[{0:P2} @ {1}]", Yield, ToWhat.ToString())
            Else
                Return String.Format("{0:P2}", Yield)
            End If
        End Function
    End Class
End Namespace