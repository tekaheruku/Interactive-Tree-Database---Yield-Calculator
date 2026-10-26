Imports TreeYield.Core.Models

Namespace Services

    ''' <summary>
    ''' The calculation engine. Every method is a pure function (no database, no UI),
    ''' so the same inputs always give the same outputs and the logic is unit-tested.
    ''' All money and quantities use <see cref="Decimal"/> to avoid floating-point
    ''' rounding errors (for example 0.1 + 0.2 &lt;&gt; 0.3 with Double).
    ''' </summary>
    Public NotInheritable Class YieldCalculator

        ''' <summary>Smallest tree count accepted.</summary>
        Public Const MinTreeCount As Integer = 1

        ''' <summary>Largest tree count accepted.</summary>
        Public Const MaxTreeCount As Integer = 100000

        ''' <summary>Shortest projection horizon in years.</summary>
        Public Const MinProjectionYears As Integer = 1

        ''' <summary>Longest projection horizon in years.</summary>
        Public Const MaxProjectionYears As Integer = 30

        ''' <summary>Horizon used when the user has not chosen one.</summary>
        Public Const DefaultProjectionYears As Integer = 10

        ''' <summary>Shown beside every result, in the UI, the CSV file and the printed report.</summary>
        Public Const EstimateDisclaimer As String =
            "Figures are planning estimates based on sample yield and farm-gate price data. " &
            "Actual results vary with variety, site, farm management, weather and market prices."

        Private Sub New()
            ' Only Shared members: this class is never instantiated.
        End Sub

        ''' <summary>
        ''' Calculates the yearly yield and gross sales at full production.
        ''' <para>Annual yield = tree count x yield per tree per year.</para>
        ''' <para>Gross sales = annual yield x price per unit.</para>
        ''' Example: 25 Carabao Mango trees x 300 kg = 7,500 kg; 7,500 kg x PHP 80 = PHP 600,000.
        ''' </summary>
        ''' <param name="species">The species to calculate. Must not be Nothing.</param>
        ''' <param name="treeCount">Number of trees, from <see cref="MinTreeCount"/> to <see cref="MaxTreeCount"/>.</param>
        ''' <exception cref="ArgumentNullException">species is Nothing.</exception>
        ''' <exception cref="ArgumentOutOfRangeException">treeCount is outside the allowed range.</exception>
        Public Shared Function Calculate(species As TreeSpecies, treeCount As Integer) As YieldResult
            EnsureSpecies(species)
            EnsureTreeCount(treeCount)

            Dim annualYield As Decimal = treeCount * species.YieldPerTreePerYear
            Dim result As New YieldResult With {
                .Species = species,
                .TreeCount = treeCount,
                .AnnualYield = annualYield,
                .GrossSales = annualYield * species.PricePerUnit
            }

            If species.HarvestType = HarvestType.Rotation Then
                ' Timber is sold only once, at rotation age, so the "annual" numbers above
                ' are averages (mean annual increment). The real cash event is the harvest:
                ' everything the trees grew during the whole rotation is cut at once.
                Dim rotationAge As Integer = GetRotationAge(species)
                result.IsAnnualized = True
                result.HarvestYear = rotationAge
                result.HarvestYield = annualYield * rotationAge
                result.HarvestSales = result.HarvestYield * species.PricePerUnit
            End If

            Return result
        End Function

        ''' <summary>
        ''' Returns how much of full production an annual species reaches in a given year
        ''' after planting, as a fraction from 0 to 1.
        ''' <list type="bullet">
        ''' <item>Before the first harvest year the trees are still growing: 0.</item>
        ''' <item>From the first harvest to full production the yield rises in equal steps (linear ramp).</item>
        ''' <item>From the full-production year onward: 1 (100%).</item>
        ''' </list>
        ''' Example: first harvest in year 5, full production in year 8 gives
        ''' 25%, 50%, 75% and 100% for years 5, 6, 7 and 8.
        ''' </summary>
        ''' <param name="species">The species. Must not be Nothing.</param>
        ''' <param name="year">Year after planting, starting at 1.</param>
        Public Shared Function GetProductionFactor(species As TreeSpecies, year As Integer) As Decimal
            EnsureSpecies(species)
            If year < 1 Then
                Throw New ArgumentOutOfRangeException(NameOf(year), "The year must be 1 or later.")
            End If

            ' A value of 0 years means "already bearing", which is the same as year 1.
            Dim firstYear As Integer = Math.Max(1, species.YearsToFirstHarvest)
            Dim fullYear As Integer = Math.Max(firstYear, species.YearsToFullProduction)

            If year < firstYear Then
                Return 0D
            End If

            If year >= fullYear Then
                Return 1D
            End If

            ' Number of bearing years so far divided by the length of the ramp-up period.
            ' CDec makes this a Decimal division (the / operator on Integers gives a Double).
            Return CDec(year - firstYear + 1) / (fullYear - firstYear + 1)
        End Function

        ''' <summary>
        ''' Builds a year-by-year projection of yield and gross sales.
        ''' Annual species follow <see cref="GetProductionFactor"/>. Rotation species
        ''' earn nothing until rotation age and then the whole stand is cut; the projection
        ''' assumes the land is replanted at once, so harvests repeat every rotation.
        ''' </summary>
        ''' <param name="species">The species. Must not be Nothing.</param>
        ''' <param name="treeCount">Number of trees.</param>
        ''' <param name="years">Projection horizon, from <see cref="MinProjectionYears"/> to <see cref="MaxProjectionYears"/>.</param>
        Public Shared Function Project(species As TreeSpecies, treeCount As Integer, years As Integer) As YieldProjection
            EnsureSpecies(species)
            EnsureTreeCount(treeCount)
            If years < MinProjectionYears OrElse years > MaxProjectionYears Then
                Throw New ArgumentOutOfRangeException(NameOf(years),
                    String.Format("The projection must cover {0} to {1} years.", MinProjectionYears, MaxProjectionYears))
            End If

            Dim fullAnnualYield As Decimal = treeCount * species.YieldPerTreePerYear
            Dim rotationAge As Integer = GetRotationAge(species)
            Dim isRotation As Boolean = species.HarvestType = HarvestType.Rotation

            Dim rows As New List(Of ProjectionYear)(years)
            Dim cumulativeYield As Decimal = 0D
            Dim cumulativeSales As Decimal = 0D

            For year As Integer = 1 To years
                Dim factor As Decimal
                Dim yieldThisYear As Decimal

                If isRotation Then
                    ' Harvest happens in years that are multiples of the rotation age.
                    Dim harvestYear As Boolean = (year Mod rotationAge = 0)
                    factor = If(harvestYear, 1D, 0D)
                    yieldThisYear = If(harvestYear, fullAnnualYield * rotationAge, 0D)
                Else
                    factor = GetProductionFactor(species, year)
                    yieldThisYear = fullAnnualYield * factor
                End If

                Dim sales As Decimal = yieldThisYear * species.PricePerUnit
                cumulativeYield += yieldThisYear
                cumulativeSales += sales

                rows.Add(New ProjectionYear With {
                    .Year = year,
                    .ProductionFactor = factor,
                    .IsHarvestYear = factor > 0D,
                    .Yield = yieldThisYear,
                    .GrossSales = sales,
                    .CumulativeYield = cumulativeYield,
                    .CumulativeSales = cumulativeSales
                })
            Next

            Dim firstHarvestYear As Integer = If(isRotation, rotationAge, Math.Max(1, species.YearsToFirstHarvest))

            Return New YieldProjection With {
                .Species = species,
                .TreeCount = treeCount,
                .Years = years,
                .Rows = rows,
                .TotalYield = cumulativeYield,
                .TotalSales = cumulativeSales,
                .FirstHarvestYear = firstHarvestYear,
                .FirstHarvestBeyondHorizon = firstHarvestYear > years
            }
        End Function

        ''' <summary>Rotation age in years; never less than 1 so the Mod above is safe.</summary>
        Private Shared Function GetRotationAge(species As TreeSpecies) As Integer
            Return Math.Max(1, species.YearsToFirstHarvest)
        End Function

        Private Shared Sub EnsureSpecies(species As TreeSpecies)
            If species Is Nothing Then
                Throw New ArgumentNullException(NameOf(species))
            End If

            ' The database CHECK constraints already block these values; this guard
            ' protects the calculator if it is ever called with unsaved data.
            If species.YieldPerTreePerYear < 0D OrElse species.PricePerUnit < 0D Then
                Throw New ArgumentException("Yield and price cannot be negative.", NameOf(species))
            End If
        End Sub

        Private Shared Sub EnsureTreeCount(treeCount As Integer)
            If treeCount < MinTreeCount OrElse treeCount > MaxTreeCount Then
                Throw New ArgumentOutOfRangeException(NameOf(treeCount),
                    String.Format("Tree count must be from {0:N0} to {1:N0}.", MinTreeCount, MaxTreeCount))
            End If
        End Sub

    End Class

End Namespace
