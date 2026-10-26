namespace TreeYield.Core.Models
{
    /// <summary>One row of the multi-year projection table (one year after planting).</summary>
    public class ProjectionYear
    {
        /// <summary>Year number, starting at 1 for the first year after planting.</summary>
        public int Year { get; set; }

        /// <summary>
        /// Share of full production reached in this year, from 0 to 1.
        /// For rotation species it is 1 in a harvest year and 0 otherwise.
        /// </summary>
        public decimal ProductionFactor { get; set; }

        /// <summary>True when something is harvested in this year.</summary>
        public bool IsHarvestYear { get; set; }

        /// <summary>Yield harvested in this year, in the species' yield unit.</summary>
        public decimal Yield { get; set; }

        /// <summary>Gross sales earned in this year, in pesos.</summary>
        public decimal GrossSales { get; set; }

        /// <summary>Running total of <see cref="Yield"/> from year 1 to this year.</summary>
        public decimal CumulativeYield { get; set; }

        /// <summary>Running total of <see cref="GrossSales"/> from year 1 to this year.</summary>
        public decimal CumulativeSales { get; set; }
    }
}
