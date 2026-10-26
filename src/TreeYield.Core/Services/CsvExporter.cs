using System;
using System.Globalization;
using System.Linq;
using System.Text;
using TreeYield.Core.Models;

namespace TreeYield.Core.Services
{
    /// <summary>
    /// Builds the CSV (comma-separated values) report that opens in Excel.
    /// Follows RFC 4180 quoting and blocks "CSV formula injection".
    /// </summary>
    public static class CsvExporter
    {
        // Characters that make Excel treat a cell as a formula, e.g. =HYPERLINK("http://evil").
        private const string FormulaTriggers = "=+-@\t\r";

        private static readonly char[] CharactersNeedingQuotes = { ',', '"', '\r', '\n' };

        /// <summary>
        /// Makes one value safe for a CSV cell:
        /// <list type="number">
        /// <item>If it starts with a formula character, a leading apostrophe is added so Excel shows it as text.</item>
        /// <item>If it contains a comma, quote or line break, it is wrapped in quotes and inner quotes are doubled.</item>
        /// </list>
        /// </summary>
        public static string EscapeField(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            string safe = FormulaTriggers.IndexOf(value[0]) >= 0 ? "'" + value : value;

            if (safe.IndexOfAny(CharactersNeedingQuotes) >= 0)
            {
                safe = "\"" + safe.Replace("\"", "\"\"") + "\"";
            }

            return safe;
        }

        /// <summary>Joins escaped values into one CSV line (without the line break).</summary>
        public static string BuildRow(params string[] fields)
        {
            if (fields == null)
            {
                return string.Empty;
            }

            return string.Join(",", fields.Select(EscapeField));
        }

        /// <summary>Builds the full report: species details, inputs, results and the projection table.</summary>
        /// <param name="result">Snapshot from <see cref="YieldCalculator.Calculate"/>.</param>
        /// <param name="projection">Projection from <see cref="YieldCalculator.Project"/>.</param>
        /// <param name="generatedAt">Time written in the report header (local time).</param>
        public static string BuildReport(YieldResult result, YieldProjection projection, DateTime generatedAt)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (projection == null)
            {
                throw new ArgumentNullException(nameof(projection));
            }

            TreeSpecies s = result.Species;
            string unit = s.YieldUnit;
            bool isRotation = s.HarvestType == HarvestType.Rotation;
            var csv = new StringBuilder();

            AppendRow(csv, "Tree Yield & Revenue Projection Report");
            AppendRow(csv, "Generated", generatedAt.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));
            AppendRow(csv);

            AppendRow(csv, "Field", "Value");
            AppendRow(csv, "Species", s.CommonName);
            AppendRow(csv, "Scientific name", s.ScientificName);
            AppendRow(csv, "Category", s.CategoryName);
            AppendRow(csv, "Product", s.ProductType);
            AppendRow(csv, "Harvest type", isRotation ? "Rotation (one-time harvest)" : "Annual");
            AppendRow(csv, "Yield per tree per year (" + unit + ")", Number(s.YieldPerTreePerYear));
            AppendRow(csv, "Price per " + unit + " (PHP)", Number(s.PricePerUnit));
            AppendRow(csv, isRotation ? "Rotation age (years)" : "Years to first harvest", Whole(s.YearsToFirstHarvest));
            if (!isRotation)
            {
                AppendRow(csv, "Years to full production", Whole(s.YearsToFullProduction));
            }

            AppendRow(csv, "Tree count", Whole(result.TreeCount));
            AppendRow(csv, (isRotation ? "Annualized yield (" : "Annual yield at full production (") + unit + ")", Number(result.AnnualYield));
            AppendRow(csv, isRotation ? "Annualized gross sales (PHP)" : "Estimated annual gross sales (PHP)", Number(result.GrossSales));
            if (isRotation)
            {
                AppendRow(csv, "Harvest year", Whole(result.HarvestYear));
                AppendRow(csv, "Harvest yield (" + unit + ")", Number(result.HarvestYield));
                AppendRow(csv, "Harvest gross sales (PHP)", Number(result.HarvestSales));
            }

            AppendRow(csv);
            AppendRow(csv, Whole(projection.Years) + "-year projection");
            AppendRow(csv, "Year", "Production", "Yield (" + unit + ")", "Gross sales (PHP)",
                "Cumulative yield (" + unit + ")", "Cumulative gross sales (PHP)");

            foreach (ProjectionYear row in projection.Rows)
            {
                AppendRow(csv,
                    Whole(row.Year),
                    Formatters.ProductionStage(row, s.HarvestType),
                    Number(row.Yield),
                    Number(row.GrossSales),
                    Number(row.CumulativeYield),
                    Number(row.CumulativeSales));
            }

            AppendRow(csv, "Total", string.Empty, Number(projection.TotalYield), Number(projection.TotalSales));
            AppendRow(csv);
            AppendRow(csv, "Note", YieldCalculator.EstimateDisclaimer);

            return csv.ToString();
        }

        private static void AppendRow(StringBuilder csv, params string[] fields)
        {
            // RFC 4180 uses CRLF line endings.
            csv.Append(BuildRow(fields)).Append("\r\n");
        }

        private static string Number(decimal value)
        {
            // No thousands separator, so Excel reads the cell as a number.
            return value.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private static string Whole(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
