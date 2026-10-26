Namespace Models

    ''' <summary>
    ''' How a species produces income. The yield calculator uses a different
    ''' formula for each type (see <see cref="Services.YieldCalculator"/>).
    ''' </summary>
    Public Enum HarvestType

        ''' <summary>
        ''' The tree is harvested every year once it starts bearing
        ''' (fruit, nuts, beans, latex). Yield ramps up until full production.
        ''' </summary>
        Annual = 0

        ''' <summary>
        ''' The tree is cut once at rotation age (timber, pulpwood). The yearly
        ''' figure is the mean annual increment; income happens at harvest.
        ''' </summary>
        Rotation = 1

    End Enum

End Namespace
