using System;
using System.Collections.Generic;
using TreeYield.Core.Models;

namespace TreeYield.Core.Services
{
    /// <summary>
    /// The calculation engine. Every method is a pure function (no database, no UI),
    /// so the same inputs always give the same outputs and the logic is unit-tested.
    /// All money and quantities use <see cref="decimal"/> to avoid floating-point
    /// rounding errors (for example 0.1 + 0.2 != 0.3 with double).
    /// </summary>
    public static class YieldCalculator
    {
        /// <summary>Smallest tree count accepted.</summary>
        public const int MinTreeCount = 1;

        /// <summary>Largest tree count accepted.</summary>
        public const int MaxTreeCount = 100000;

        /// <summary>Shortest projection horizon in years.</summary>
        public const int MinProjectionYears = 1;

        /// <summary>Longest projection horizon in years.</summary>
        public const int MaxProjectionYears = 30;

        /// <summary>Horizon used when the user has not chosen one.</summary>
        public const int DefaultProjectionYears = 10;

        /// <summary>Shown beside every result, in the UI, the CSV file and the printed report.</summary>
        public const string EstimateDisclaimer =
            "Figures are planning estimates based on sample yield and farm-gate price data. " +
            "Actual results vary with variety, site, farm management, weather and market prices.";

        /// <summary>
        /// Calculates the yearly yield and gross sales at full production.
        /// <para>Annual yield = tree count x yield per tree per year.</para>
        /// <para>Gross sales = annual yield x price per unit.</para>
        /// Example: 25 Carabao Mango trees x 300 kg = 7,500 kg; 7,500 kg x PHP 80 = PHP 600,000.
        /// </summary>
        /// <param name="species">The species to calculate. Must not be null.</param>
        /// <param name="treeCount">Number of trees, from <see cref="MinTreeCount"/> to <see cref="MaxTreeCount"/>.</param>
        /// <exception cref="ArgumentNullException">species is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">treeCount is outside the allowed range.</exception>
        public static YieldResult Calculate(TreeSpecies species, int treeCount)
        {
            EnsureSpecies(species);
            EnsureTreeCount(treeCount);

            decimal annualYield = treeCount * species.YieldPerTreePerYear;
            var result = new YieldResult
            {
                Species = species,
                TreeCount = treeCount,
                AnnualYield = annualYield,
                GrossSales = annualYield * species.PricePerUnit
            };

            if (species.HarvestType == HarvestType.Rotation)
            {
                // Timber is sold only once, at rotation age, so the "annual" numbers above
                // are averages (mean annual increment). The real cash event is the harvest:
                // everything the trees grew during the whole rotation is cut at once.
                int rotationAge = GetRotationAge(species);
                result.IsAnnualized = true;
                result.HarvestYear = rotationAge;
                result.HarvestYield = annualYield * rotationAge;
                result.HarvestSales = result.HarvestYield * species.PricePerUnit;
            }

            return result;
        }

        /// <summary>
        /// Returns how much of full production an annual species reaches in a given year
        /// after planting, as a fraction from 0 to 1.
        /// <list type="bullet">
        /// <item>Before the first harvest year the trees are still growing: 0.</item>
        /// <item>From the first harvest to full production the yield rises in equal steps (linear ramp).</item>
        /// <item>From the full-production year onward: 1 (100%).</item>
        /// </list>
        /// Example: first harvest in year 5, full production in year 8 gives
        /// 25%, 50%, 75% and 100% for years 5, 6, 7 and 8.
        /// </summary>
        /// <param name="species">The species. Must not be null.</param>
        /// <param name="year">Year after planting, starting at 1.</param>
        public static decimal GetProductionFactor(TreeSpecies species, int year)
        {
            EnsureSpecies(species);
            if (year < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(year), "The year must be 1 or later.");
            }

            // A value of 0 years means "already bearing", which is the same as year 1.
            int firstYear = Math.Max(1, species.YearsToFirstHarvest);
            int fullYear = Math.Max(firstYear, species.YearsToFullProduction);

            if (year < firstYear)
            {
                return 0m;
            }

            if (year >= fullYear)
            {
                return 1m;
            }

            // Number of bearing years so far divided by the length of the ramp-up period.
            // The cast to decimal makes this a decimal division instead of integer division.
            return (decimal)(year - firstYear + 1) / (fullYear - firstYear + 1);
        }

        /// <summary>
        /// Builds a year-by-year projection of yield and gross sales.
        /// Annual species follow <see cref="GetProductionFactor"/>. Rotation species
        /// earn nothing until rotation age and then the whole stand is cut; the projection
        /// assumes the land is replanted at once, so harvests repeat every rotation.
        /// </summary>
        /// <param name="species">The species. Must not be null.</param>
        /// <param name="treeCount">Number of trees.</param>
        /// <param name="years">Projection horizon, from <see cref="MinProjectionYears"/> to <see cref="MaxProjectionYears"/>.</param>
        public static YieldProjection Project(TreeSpecies species, int treeCount, int years)
        {
            EnsureSpecies(species);
            EnsureTreeCount(treeCount);
            if (years < MinProjectionYears || years > MaxProjectionYears)
            {
                throw new ArgumentOutOfRangeException(nameof(years),
                    string.Format("The projection must cover {0} to {1} years.", MinProjectionYears, MaxProjectionYears));
            }

            decimal fullAnnualYield = treeCount * species.YieldPerTreePerYear;
            int rotationAge = GetRotationAge(species);
            bool isRotation = species.HarvestType == HarvestType.Rotation;

            var rows = new List<ProjectionYear>(years);
            decimal cumulativeYield = 0m;
            decimal cumulativeSales = 0m;

            for (int year = 1; year <= years; year++)
            {
                decimal factor;
                decimal yield;

                if (isRotation)
                {
                    // Harvest happens in years that are multiples of the rotation age.
                    bool harvestYear = year % rotationAge == 0;
                    factor = harvestYear ? 1m : 0m;
                    yield = harvestYear ? fullAnnualYield * rotationAge : 0m;
                }
                else
                {
                    factor = GetProductionFactor(species, year);
                    yield = fullAnnualYield * factor;
                }

                decimal sales = yield * species.PricePerUnit;
                cumulativeYield += yield;
                cumulativeSales += sales;

                rows.Add(new ProjectionYear
                {
                    Year = year,
                    ProductionFactor = factor,
                    IsHarvestYear = factor > 0m,
                    Yield = yield,
                    GrossSales = sales,
                    CumulativeYield = cumulativeYield,
                    CumulativeSales = cumulativeSales
                });
            }

            int firstHarvestYear = isRotation ? rotationAge : Math.Max(1, species.YearsToFirstHarvest);

            return new YieldProjection
            {
                Species = species,
                TreeCount = treeCount,
                Years = years,
                Rows = rows,
                TotalYield = cumulativeYield,
                TotalSales = cumulativeSales,
                FirstHarvestYear = firstHarvestYear,
                FirstHarvestBeyondHorizon = firstHarvestYear > years
            };
        }

        /// <summary>Rotation age in years; never less than 1 so the modulo above is safe.</summary>
        private static int GetRotationAge(TreeSpecies species)
        {
            return Math.Max(1, species.YearsToFirstHarvest);
        }

        private static void EnsureSpecies(TreeSpecies species)
        {
            if (species == null)
            {
                throw new ArgumentNullException(nameof(species));
            }

            // The database CHECK constraints already block these values; this guard
            // protects the calculator if it is ever called with unsaved data.
            if (species.YieldPerTreePerYear < 0m || species.PricePerUnit < 0m)
            {
                throw new ArgumentException("Yield and price cannot be negative.", nameof(species));
            }
        }

        private static void EnsureTreeCount(int treeCount)
        {
            if (treeCount < MinTreeCount || treeCount > MaxTreeCount)
            {
                throw new ArgumentOutOfRangeException(nameof(treeCount),
                    string.Format("Tree count must be from {0:N0} to {1:N0}.", MinTreeCount, MaxTreeCount));
            }
        }
    }
}
