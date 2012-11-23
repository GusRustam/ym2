Imports YieldMap.Tools
Imports YieldMap.BondsDataSetTableAdapters

Namespace Commons
    Module BondDescriber
        Private _bondDescriptions As BondsDataSet.BondDescriptionsDataTable

        Sub InitBondDescriber()
            Dim bh As New BondDescriptionsTableAdapter
            AddHandler bh.Adapter.FillError, AddressOf SkipInvalidRows
            _bondDescriptions = bh.GetData()
            RemoveHandler bh.Adapter.FillError, AddressOf SkipInvalidRows
        End Sub

        Function GetBondInfo(ByVal ric As String) As DataBaseBondDescription
            If _bondDescriptions.Any(Function(row) row.ric = ric) Then
                Dim bondDescr = _bondDescriptions.First(Function(row) row.ric = ric)
                Return New DataBaseBondDescription(ric, bondDescr.bondshortname, bondDescr.bondshortname,
                                                   bondDescr.maturitydate, bondDescr.coupon, bondDescr.payments,
                                                   bondDescr.rates, bondDescr.issuedate)
            Else
                Return Nothing
            End If
        End Function
    End Module
End Namespace