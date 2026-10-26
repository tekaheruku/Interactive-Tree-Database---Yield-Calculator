namespace TreeYield.Core.Models
{
    /// <summary>
    /// A tree classification (row of the Categories table) used by the
    /// "All Species / Fruit-Bearing / Non-Fruit-Bearing" filter.
    /// </summary>
    public class Category
    {
        /// <summary>Name of the fruit-bearing category as stored in the database.</summary>
        public const string FruitBearing = "Fruit-Bearing";

        /// <summary>Name of the non-fruit-bearing category as stored in the database.</summary>
        public const string NonFruitBearing = "Non-Fruit-Bearing";

        /// <summary>Primary key.</summary>
        public int CategoryId { get; set; }

        /// <summary>Display name, for example "Fruit-Bearing". Unique.</summary>
        public string Name { get; set; }

        /// <summary>Optional longer explanation of the category.</summary>
        public string Description { get; set; }

        /// <summary>Returns the category name so the object can be shown directly in a ComboBox.</summary>
        public override string ToString()
        {
            return Name ?? string.Empty;
        }
    }
}
