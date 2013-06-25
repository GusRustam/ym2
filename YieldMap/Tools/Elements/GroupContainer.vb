Imports DbManager.Bonds

Namespace Tools.Elements
    Public Class GroupContainer
        Implements IEnumerable(Of KeyValuePair(Of Long, Group))
        Private ReadOnly _items As New Dictionary(Of Long, Group)

        Public Event Volume As Action(Of Bond)
        Public Event Cleared As Action(Of Group)
        Public Event Recalculated As Action(Of Group)

        Default Public ReadOnly Property Data(ByVal id As Long) As Group
            Get
                Return _items(id)
            End Get
        End Property

        ' todo completely another thing needed. Something that catches all events and allows for dynamic representation
        Public ReadOnly Property AsTable() As List(Of BondDescr)
            Get
                Dim result As New List(Of BondDescr)
                For Each grp In From kvp In _items Select kvp.Value
                    grp.Elements.ForEach(
                        Sub(elem)
                            Dim res As New BondDescr
                            res.RIC = elem.MetaData.RIC
                            res.Name = elem.MetaData.ShortName
                            res.Maturity = elem.MetaData.Maturity
                            res.Coupon = elem.MetaData.Coupon
                            Dim fieldName = elem.QuotesAndYields.MaxPriorityField
                            Dim quote = elem.QuotesAndYields(fieldName)
                            If quote IsNot Nothing Then
                                res.Price = quote.Price
                                res.Quote = fieldName
                                res.QuoteDate = quote.YieldAtDate
                                res.ToWhat = quote.Yld.ToWhat
                                res.BondYield = quote.Yld.Yield
                                res.Convexity = quote.Convexity
                                res.Duration = quote.Duration
                                res.Live = quote.YieldAtDate = Date.Today
                            End If
                            result.Add(res)
                        End Sub)
                Next
                Return result
            End Get
        End Property

        Public Function Exists(ByVal id As Long) As Boolean
            Return _items.Keys.Contains(id)
        End Function

        Public Sub Cleanup()
            While _items.Any
                _items.First.Value.Cleanup()
            End While
        End Sub

        Public Sub Start()
            For Each kvp In _items
                kvp.Value.Subscribe()
            Next
        End Sub

        Public Sub Add(ByVal group As Group)
            _items.Add(group.Identity, group)
            AddHandler group.Updated, Sub() RaiseEvent Recalculated(group)
            AddHandler group.Cleared, Sub()
                                          _items.Remove(group.Identity)
                                          RaiseEvent Cleared(group)
                                      End Sub
            AddHandler group.Volume, Sub(bond As Bond) RaiseEvent Volume(bond)
        End Sub

        Public Sub Remove(ByVal id As Long)
            _items(id).Cleanup()
        End Sub

        Public Function IEnumerable_GetEnumerator() As IEnumerator(Of KeyValuePair(Of Long, Group)) Implements IEnumerable(Of KeyValuePair(Of Long, Group)).GetEnumerator
            Return _items.GetEnumerator()
        End Function

        Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return _items.GetEnumerator()
        End Function

        Public Function Bonds(ByVal clause As Func(Of Bond, Boolean)) As List(Of Bond)
            Dim res As New List(Of Bond)
            For Each item In _items
                res.AddRange(item.Value.Bonds(clause))
            Next
            Return res
        End Function

        Public Function Bonds(ByVal clause As Func(Of BondMetadata, Boolean)) As List(Of Bond)
            Dim res As New List(Of Bond)
            For Each item In _items
                res.AddRange(item.Value.Bonds(clause))
            Next
            Return res
        End Function

        Public Sub CleanupOnlyBonds()
            While _items.Any(Function(item) TypeOf item.Value Is BondGroup)
                _items.First(Function(item) TypeOf item.Value Is BondGroup).Value.Cleanup()
            End While
            While _items.Any(Function(item) TypeOf item.Value Is CustomBondGroup)
                _items.First(Function(item) TypeOf item.Value Is CustomBondGroup).Value.Cleanup()
            End While
        End Sub

        Public Sub CleanupOnlyCurves()
            While _items.Any(Function(item) TypeOf item.Value Is BondCurve)
                _items.First(Function(item) TypeOf item.Value Is BondCurve).Value.Cleanup()
            End While
        End Sub
    End Class
End Namespace