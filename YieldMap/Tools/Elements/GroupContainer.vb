Namespace Tools.Elements
    Public Class GroupContainer(Of T As BaseGroup)
        Private ReadOnly _groups As New Dictionary(Of Long, T)

        Public Event RemovedItem As Action(Of T, String)
        Public Event Quote As Action(Of Bond)
        Public Event Volume As Action(Of Bond)
        Public Event Cleared As Action(Of T)

        Default Public ReadOnly Property Data(ByVal id As Long) As T
            Get
                Return _groups(id)
            End Get
        End Property

        ' todo completely another thing needed. Something that catches all events and allows for dynamic representation
        Public ReadOnly Property AsTable() As List(Of BondDescr)
            Get
                Dim result As New List(Of BondDescr)
                For Each grp In From kvp In _groups Select kvp.Value
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
                                res.State = BondDescr.StateType.Ok
                                res.ToWhat = quote.Yld.ToWhat
                                res.BondYield = quote.Yld.Yield
                                res.CalcMode = BondDescr.CalculationMode.SystemPrice
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
            Return _groups.Keys.Contains(id)
        End Function

        Public Sub Cleanup()
            For Each kvp In _groups
                kvp.Value.Cleanup()
            Next
            _groups.Clear()
        End Sub

        Public Sub Start()
            For Each kvp In _groups
                kvp.Value.StartAll()
            Next
        End Sub

        Public Sub Add(ByVal group As T)
            _groups.Add(group.Identity, group)
            AddHandler group.Clear, Sub(base As BaseGroup) RaiseEvent Cleared(base)
            AddHandler group.Quote, Sub(bond As Bond) RaiseEvent Quote(bond)
            AddHandler group.RemovedItem, Sub(grp As BaseGroup, ric As String) RaiseEvent RemovedItem(grp, ric)
            AddHandler group.Volume, Sub(bond As Bond) RaiseEvent Volume(bond)
        End Sub

        Public Sub Remove(ByVal id As Long)
            _groups.Remove(id)
        End Sub

        Public Function FindBond(ByVal ric As String) As Bond
            For Each kvp In From elem In _groups Where elem.Value.HasRic(ric)
                Return kvp.Value.GetBond(ric)
            Next
            Return Nothing
        End Function
    End Class
End Namespace