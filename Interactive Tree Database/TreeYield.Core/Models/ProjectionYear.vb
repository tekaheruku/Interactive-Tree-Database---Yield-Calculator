Namespace Models

    ''' <summary>One row of the multi-year projection table (one year after planting).</summary>
    Public Class ProjectionYear

        ''' <summary>Year number, starting at 1 for the first year after planting.</summary>
        Public Property Year As Integer

        ''' <summary>
        ''' Share of full production reached in this year, from 0 to 1.
        ''' For rotation species it is 1 in a harvest year and 0 otherwise.
        ''' </summary>
        Public Property ProductionFactor As Decimal

        ''' <summary>True when something is harvested in this year.</summary>
        Public Property IsHarvestYear As Boolean

        ''' <summary>Yield harvested in this year, in the species' yield unit.</summary>
        Public Property Yield As Decimal

        ''' <summary>Gross sales earned in this year, in pesos.</summary>
        Public Property GrossSales As Decimal

        ''' <summary>Running total of <see cref="Yield"/> from year 1 to this year.</summary>
        Public Property CumulativeYield As Decimal

        ''' <summary>Running total of <see cref="GrossSales"/> from year 1 to this year.</summary>
        Public Property CumulativeSales As Decimal

    End Class

End Namespace
