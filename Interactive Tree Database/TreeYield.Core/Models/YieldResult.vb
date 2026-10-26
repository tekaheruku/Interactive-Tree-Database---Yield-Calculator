Namespace Models

    ''' <summary>
    ''' The numbers shown on the three stat cards: the yearly yield and gross
    ''' sales for a number of trees of one species at full production.
    ''' </summary>
    Public Class YieldResult

        ''' <summary>The species that was calculated.</summary>
        Public Property Species As TreeSpecies

        ''' <summary>How many trees were entered.</summary>
        Public Property TreeCount As Integer

        ''' <summary>Trees x yield per tree per year, in the species' yield unit.</summary>
        Public Property AnnualYield As Decimal

        ''' <summary>Annual yield x price per unit, in Philippine pesos.</summary>
        Public Property GrossSales As Decimal

        ''' <summary>
        ''' True for rotation (timber) species: the annual figures are averages,
        ''' because the money is earned only once, at harvest.
        ''' </summary>
        Public Property IsAnnualized As Boolean

        ''' <summary>Rotation species only: the year of the one-time harvest. 0 otherwise.</summary>
        Public Property HarvestYear As Integer

        ''' <summary>Rotation species only: total volume cut at harvest (annual yield x rotation age).</summary>
        Public Property HarvestYield As Decimal

        ''' <summary>Rotation species only: gross sales from the one-time harvest.</summary>
        Public Property HarvestSales As Decimal

    End Class

End Namespace
