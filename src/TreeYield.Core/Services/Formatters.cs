using System;
using System.Globalization;
using TreeYield.Core.Models;

namespace TreeYield.Core.Services
{
    /// <summary>
    /// Turns numbers into the text shown on screen. Keeping all formatting in one
    /// place makes the cards, grids, CSV file and printed report consistent.
    /// </summary>
    public static class Formatters
    {
        /// <summary>The Philippine peso sign. Written as an escape so the source file stays plain ASCII.</summary>
        public const string PesoSign = "₱";

        // Invariant culture gives "1,234.5" on every PC, whatever its regional settings are.
        private static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

        /// <summary>Formats whole pesos, for example 600000 becomes "PHP-sign 600,000".</summary>
        public static string Peso(decimal amount)
        {
            return PesoSign + RoundToWholePeso(amount).ToString("#,##0", Culture);
        }

        /// <summary>Formats pesos with centavos, for example 80 becomes "PHP-sign 80.00".</summary>
        public static string PesoWithCentavos(decimal amount)
        {
            return PesoSign + amount.ToString("#,##0.00", Culture);
        }

        /// <summary>Rounds to the nearest peso (0.5 rounds up, like a cashier would).</summary>
        public static decimal RoundToWholePeso(decimal amount)
        {
            return Math.Round(amount, 0, MidpointRounding.AwayFromZero);
        }

        /// <summary>Formats a quantity with up to 2 decimals, for example 7500 becomes "7,500" and 37.5 becomes "37.5".</summary>
        public static string Quantity(decimal value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero).ToString("#,##0.##", Culture);
        }

        /// <summary>Formats a quantity followed by its unit, for example "7,500 kg".</summary>
        public static string Quantity(decimal value, string unit)
        {
            string number = Quantity(value);
            return string.IsNullOrWhiteSpace(unit) ? number : number + " " + unit.Trim();
        }

        /// <summary>Formats a 0-1 fraction as a percentage, for example 0.1667 becomes "16.7%".</summary>
        public static string Percent(decimal fraction)
        {
            return Math.Round(fraction * 100m, 1, MidpointRounding.AwayFromZero).ToString("0.#", Culture) + "%";
        }

        /// <summary>Text for the "Production" column of the projection table.</summary>
        public static string ProductionStage(ProjectionYear row, HarvestType harvestType)
        {
            if (row == null)
            {
                return string.Empty;
            }

            if (harvestType == HarvestType.Rotation)
            {
                return row.IsHarvestYear ? "Harvest" : "Growing";
            }

            return row.ProductionFactor <= 0m ? "Not bearing yet" : Percent(row.ProductionFactor);
        }

        /// <summary>Formats a number of years, for example "1 year" or "10 years".</summary>
        public static string Years(int years)
        {
            return years == 1 ? "1 year" : years.ToString(Culture) + " years";
        }

        /// <summary>Converts a UTC time from the database to local time text, for example "Oct 26, 2026 3:45 PM".</summary>
        public static string LocalDateTime(DateTime utc)
        {
            return DateTime.SpecifyKind(utc, DateTimeKind.Utc).ToLocalTime().ToString("MMM d, yyyy h:mm tt", Culture);
        }
    }
}
