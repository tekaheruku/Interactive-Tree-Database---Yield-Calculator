namespace TreeYield.Core.Models
{
    /// <summary>
    /// The numbers shown on the three stat cards: the yearly yield and gross
    /// sales for a number of trees of one species at full production.
    /// </summary>
    public class YieldResult
    {
        /// <summary>The species that was calculated.</summary>
        public TreeSpecies Species { get; set; }

        /// <summary>How many trees were entered.</summary>
        public int TreeCount { get; set; }

        /// <summary>Trees x yield per tree per year, in the species' yield unit.</summary>
        public decimal AnnualYield { get; set; }

        /// <summary>Annual yield x price per unit, in Philippine pesos.</summary>
        public decimal GrossSales { get; set; }

        /// <summary>
        /// True for rotation (timber) species: the annual figures are averages,
        /// because the money is earned only once, at harvest.
        /// </summary>
        public bool IsAnnualized { get; set; }

        /// <summary>Rotation species only: the year of the one-time harvest. 0 otherwise.</summary>
        public int HarvestYear { get; set; }

        /// <summary>Rotation species only: total volume cut at harvest (annual yield x rotation age).</summary>
        public decimal HarvestYield { get; set; }

        /// <summary>Rotation species only: gross sales from the one-time harvest.</summary>
        public decimal HarvestSales { get; set; }
    }
}
