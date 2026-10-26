using System;

namespace TreeYield.Core.Models
{
    /// <summary>
    /// One tree species record from the TreeSpecies table: the agronomic and
    /// market figures that the yield and revenue calculator needs.
    /// </summary>
    public class TreeSpecies
    {
        /// <summary>Primary key. 0 means the record has not been saved yet.</summary>
        public int SpeciesId { get; set; }

        /// <summary>Common or local name shown in the UI, for example "Carabao Mango". Unique.</summary>
        public string CommonName { get; set; }

        /// <summary>Botanical name, for example "Mangifera indica". Optional.</summary>
        public string ScientificName { get; set; }

        /// <summary>Foreign key to the Categories table.</summary>
        public int CategoryId { get; set; }

        /// <summary>Category name loaded through a JOIN. Display only; not saved.</summary>
        public string CategoryName { get; set; }

        /// <summary>What is sold, for example "Fresh fruit", "Timber" or "Cup lump".</summary>
        public string ProductType { get; set; }

        /// <summary>Unit the yield is measured in, for example "kg", "pcs" or "board ft".</summary>
        public string YieldUnit { get; set; }

        /// <summary>
        /// Annual species: harvest of one mature tree in one year.
        /// Rotation species: mean annual increment (growth per tree per year) that is
        /// realized when the tree is harvested at rotation age.
        /// </summary>
        public decimal YieldPerTreePerYear { get; set; }

        /// <summary>Estimated farm-gate price in Philippine pesos for one <see cref="YieldUnit"/>.</summary>
        public decimal PricePerUnit { get; set; }

        /// <summary>Whether the species is harvested every year or once at rotation age.</summary>
        public HarvestType HarvestType { get; set; }

        /// <summary>
        /// Annual: number of years after planting until the first harvest.
        /// Rotation: rotation age, the number of years until the one-time harvest.
        /// </summary>
        public int YearsToFirstHarvest { get; set; }

        /// <summary>
        /// Annual: year in which the tree reaches full production.
        /// Rotation: always equal to <see cref="YearsToFirstHarvest"/>.
        /// </summary>
        public int YearsToFullProduction { get; set; }

        /// <summary>Usual harvest months, for example "March - June". Optional.</summary>
        public string HarvestSeason { get; set; }

        /// <summary>Free-text remarks such as data sources or growing tips. Optional.</summary>
        public string Notes { get; set; }

        /// <summary>When the record was created (UTC).</summary>
        public DateTime CreatedAtUtc { get; set; }

        /// <summary>When the record was last changed (UTC).</summary>
        public DateTime UpdatedAtUtc { get; set; }

        /// <summary>
        /// Creates a shallow copy. The editor form works on a copy so that pressing
        /// Cancel never changes the original object.
        /// </summary>
        public TreeSpecies Clone()
        {
            return (TreeSpecies)MemberwiseClone();
        }

        /// <summary>Returns the common name so the object can be shown directly in a ComboBox.</summary>
        public override string ToString()
        {
            return CommonName ?? string.Empty;
        }
    }
}
