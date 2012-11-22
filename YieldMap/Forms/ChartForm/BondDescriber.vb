Imports YieldMap.BondsDataSetTableAdapters

Namespace Forms.ChartForm
    Module BondDescriber
        Private _bondDescriptions As BondsDataSet.BondDescriptionsDataTable

        Sub InitBondDescriber()
            Dim bh As New BondDescriptionsTableAdapter
            AddHandler bh.Adapter.FillError, AddressOf Commons.SkipInvalidRows
            _bondDescriptions = bh.GetData()
            RemoveHandler bh.Adapter.FillError, AddressOf Commons.SkipInvalidRows
        End Sub

        Function GetBondDescr(ByVal ric As String) As BondPointDescr
            If _bondDescriptions.Any(Function(row) row.ric = ric) Then
                Dim bondDescr = _bondDescriptions.First(Function(row) row.ric = ric)
                Dim descr = New BondPointDescr() With {
                    .RIC = ric,
                    .Coupon = bondDescr.coupon,
                    .Maturity = bondDescr.maturitydate,
                    .IsVisible = False,
                    .IssuerID = bondDescr.issuer_id,
                    .ShortName = Commons.GetWin1251String(bondDescr.bondshortname),
                    .Label = Commons.GetWin1251String(bondDescr.bondshortname),
                    .SelectedQuote = QuoteSource.Last,
                    .PaymentStructure = bondDescr.payments,
                    .RateStructure = bondDescr.rates,
                    .IssueDate = bondDescr.issuedate
                }
                Return descr
            Else
                Return Nothing
            End If
        End Function
    End Module
End Namespace