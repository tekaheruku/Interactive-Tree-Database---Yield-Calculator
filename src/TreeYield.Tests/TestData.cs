using TreeYield.Core.Models;

namespace TreeYield.Tests
{
    /// <summary>Ready-made species used by several test classes.</summary>
    internal static class TestData
    {
        /// <summary>Same figures as the seeded Carabao Mango: 300 kg/tree, PHP 80/kg, bears at year 5, full at year 10.</summary>
        public static TreeSpecies Mango()
        {
            return new TreeSpecies
            {
                SpeciesId = 1,
                CommonName = "Carabao Mango",
                ScientificName = "Mangifera indica",
                CategoryId = 1,
                CategoryName = Category.FruitBearing,
                ProductType = "Fresh fruit",
                YieldUnit = "kg",
                YieldPerTreePerYear = 300m,
                PricePerUnit = 80m,
                HarvestType = HarvestType.Annual,
                YearsToFirstHarvest = 5,
                YearsToFullProduction = 10
            };
        }

        /// <summary>Rotation species like the seeded Gmelina: 15 board ft/tree/yr, PHP 35, cut at year 10.</summary>
        public static TreeSpecies Gmelina()
        {
            return new TreeSpecies
            {
                SpeciesId = 2,
                CommonName = "Gmelina",
                ScientificName = "Gmelina arborea",
                CategoryId = 2,
                CategoryName = Category.NonFruitBearing,
                ProductType = "Timber",
                YieldUnit = "board ft",
                YieldPerTreePerYear = 15m,
                PricePerUnit = 35m,
                HarvestType = HarvestType.Rotation,
                YearsToFirstHarvest = 10,
                YearsToFullProduction = 10
            };
        }
    }
}
