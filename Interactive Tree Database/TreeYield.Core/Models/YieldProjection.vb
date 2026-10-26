Namespace Models

    ''' <summary>The year-by-year projection produced by <see cref="Services.YieldCalculator.Project"/>.</summary>
    Public Class YieldProjection

        ''' <summary>The species that was projected.</summary>
        Public Property Species As TreeSpecies

        ''' <summary>How many trees were entered.</summary>
        Public Property TreeCount As Integer

        ''' <summary>Number of years in the projection (the horizon).</summary>
        Public Property Years As Integer

        ''' <summary>One row per year, from year 1 to <see cref="Years"/>.</summary>
        Public Property Rows As IList(Of ProjectionYear)

        ''' <summary>Total yield over the whole horizon.</summary>
        Public Property TotalYield As Decimal

        ''' <summary>Total gross sales over the whole horizon, in pesos.</summary>
        Public Property TotalSales As Decimal

        ''' <summary>Year of the first harvest (first bearing year, or rotation age).</summary>
        Public Property FirstHarvestYear As Integer

        ''' <summary>True when the first harvest happens after the last projected year.</summary>
        Public Property FirstHarvestBeyondHorizon As Boolean

    End Class

End Namespace
