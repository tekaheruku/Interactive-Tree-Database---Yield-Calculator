using System.Collections.Generic;

namespace TreeYield.Core.Models
{
    /// <summary>The year-by-year projection produced by <see cref="Services.YieldCalculator.Project"/>.</summary>
    public class YieldProjection
    {
        /// <summary>The species that was projected.</summary>
        public TreeSpecies Species { get; set; }

        /// <summary>How many trees were entered.</summary>
        public int TreeCount { get; set; }

        /// <summary>Number of years in the projection (the horizon).</summary>
        public int Years { get; set; }

        /// <summary>One row per year, from year 1 to <see cref="Years"/>.</summary>
        public IList<ProjectionYear> Rows { get; set; }

        /// <summary>Total yield over the whole horizon.</summary>
        public decimal TotalYield { get; set; }

        /// <summary>Total gross sales over the whole horizon, in pesos.</summary>
        public decimal TotalSales { get; set; }

        /// <summary>Year of the first harvest (first bearing year, or rotation age).</summary>
        public int FirstHarvestYear { get; set; }

        /// <summary>True when the first harvest happens after the last projected year.</summary>
        public bool FirstHarvestBeyondHorizon { get; set; }
    }
}
