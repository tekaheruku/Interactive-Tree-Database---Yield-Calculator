using System;
using System.Linq;
using TreeYield.Core.Models;

namespace TreeYield.Core.Services
{
    /// <summary>
    /// Business rules for a tree species record. These checks run before anything is
    /// sent to the database. The database repeats the most important rules as CHECK
    /// and UNIQUE constraints, so bad data is blocked even if this code is bypassed
    /// (defense in depth).
    /// </summary>
    public static class SpeciesValidator
    {
        /// <summary>Maximum length of <see cref="TreeSpecies.CommonName"/> (matches NVARCHAR(100)).</summary>
        public const int MaxCommonNameLength = 100;

        /// <summary>Maximum length of <see cref="TreeSpecies.ScientificName"/>.</summary>
        public const int MaxScientificNameLength = 150;

        /// <summary>Maximum length of <see cref="TreeSpecies.ProductType"/>.</summary>
        public const int MaxProductTypeLength = 60;

        /// <summary>Maximum length of <see cref="TreeSpecies.YieldUnit"/>.</summary>
        public const int MaxYieldUnitLength = 20;

        /// <summary>Maximum length of <see cref="TreeSpecies.HarvestSeason"/>.</summary>
        public const int MaxHarvestSeasonLength = 100;

        /// <summary>Maximum length of <see cref="TreeSpecies.Notes"/>.</summary>
        public const int MaxNotesLength = 1000;

        /// <summary>Largest yield per tree per year accepted.</summary>
        public const decimal MaxYieldPerTree = 100000m;

        /// <summary>Largest price per unit accepted, in pesos.</summary>
        public const decimal MaxPricePerUnit = 1000000m;

        /// <summary>Largest number of years accepted for first harvest / full production.</summary>
        public const int MaxYears = 60;

        /// <summary>Checks every field and returns all problems found.</summary>
        /// <param name="species">The record to check.</param>
        public static ValidationResult Validate(TreeSpecies species)
        {
            var result = new ValidationResult();
            if (species == null)
            {
                result.Add("Species", "No species data was provided.");
                return result;
            }

            RequireText(result, nameof(TreeSpecies.CommonName), "Common name", species.CommonName, MaxCommonNameLength);
            OptionalText(result, nameof(TreeSpecies.ScientificName), "Scientific name", species.ScientificName, MaxScientificNameLength);

            if (species.CategoryId <= 0)
            {
                result.Add(nameof(TreeSpecies.CategoryId), "Select a category.");
            }

            RequireText(result, nameof(TreeSpecies.ProductType), "Product", species.ProductType, MaxProductTypeLength);
            RequireText(result, nameof(TreeSpecies.YieldUnit), "Yield unit", species.YieldUnit, MaxYieldUnitLength);

            if (species.YieldPerTreePerYear <= 0m)
            {
                result.Add(nameof(TreeSpecies.YieldPerTreePerYear), "Yield per tree must be greater than zero.");
            }
            else if (species.YieldPerTreePerYear > MaxYieldPerTree)
            {
                result.Add(nameof(TreeSpecies.YieldPerTreePerYear),
                    string.Format("Yield per tree cannot be more than {0:N0}.", MaxYieldPerTree));
            }
            else if (HasMoreThanTwoDecimals(species.YieldPerTreePerYear))
            {
                result.Add(nameof(TreeSpecies.YieldPerTreePerYear), "Yield per tree can have at most 2 decimal places.");
            }

            if (species.PricePerUnit < 0m)
            {
                result.Add(nameof(TreeSpecies.PricePerUnit), "Price cannot be negative.");
            }
            else if (species.PricePerUnit > MaxPricePerUnit)
            {
                result.Add(nameof(TreeSpecies.PricePerUnit),
                    string.Format("Price cannot be more than {0:N0}.", MaxPricePerUnit));
            }
            else if (HasMoreThanTwoDecimals(species.PricePerUnit))
            {
                result.Add(nameof(TreeSpecies.PricePerUnit), "Price can have at most 2 decimal places.");
            }

            if (!Enum.IsDefined(typeof(HarvestType), species.HarvestType))
            {
                result.Add(nameof(TreeSpecies.HarvestType), "Select a harvest type.");
            }

            ValidateYears(result, species);

            OptionalText(result, nameof(TreeSpecies.HarvestSeason), "Harvest season", species.HarvestSeason, MaxHarvestSeasonLength);
            OptionalText(result, nameof(TreeSpecies.Notes), "Notes", species.Notes, MaxNotesLength);

            return result;
        }

        private static void ValidateYears(ValidationResult result, TreeSpecies species)
        {
            bool isRotation = species.HarvestType == HarvestType.Rotation;
            string firstLabel = isRotation ? "Rotation age" : "Years to first harvest";

            if (species.YearsToFirstHarvest < 0 || species.YearsToFirstHarvest > MaxYears)
            {
                result.Add(nameof(TreeSpecies.YearsToFirstHarvest),
                    string.Format("{0} must be from 0 to {1}.", firstLabel, MaxYears));
            }
            else if (isRotation && species.YearsToFirstHarvest < 1)
            {
                result.Add(nameof(TreeSpecies.YearsToFirstHarvest), "Rotation age must be at least 1 year.");
            }

            if (species.YearsToFullProduction < 0 || species.YearsToFullProduction > MaxYears)
            {
                result.Add(nameof(TreeSpecies.YearsToFullProduction),
                    string.Format("Years to full production must be from 0 to {0}.", MaxYears));
            }
            else if (species.YearsToFullProduction < species.YearsToFirstHarvest)
            {
                result.Add(nameof(TreeSpecies.YearsToFullProduction),
                    "Full production cannot come before the first harvest.");
            }
        }

        private static void RequireText(ValidationResult result, string field, string label, string value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result.Add(field, label + " is required.");
                return;
            }

            OptionalText(result, field, label, value, maxLength);
        }

        private static void OptionalText(ValidationResult result, string field, string label, string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.Trim().Length > maxLength)
            {
                result.Add(field, string.Format("{0} must be {1} characters or fewer.", label, maxLength));
            }
            else if (value.Any(c => char.IsControl(c) && c != '\r' && c != '\n' && c != '\t'))
            {
                // Control characters (other than line breaks and tabs) are never typed on
                // purpose; they usually come from pasted binary data.
                result.Add(field, label + " contains characters that are not allowed.");
            }
        }

        private static bool HasMoreThanTwoDecimals(decimal value)
        {
            // The database columns are DECIMAL(12,2); extra decimals would be rounded silently.
            return decimal.Round(value, 2) != value;
        }
    }
}
