using System;
using System.Linq;
using TreeYield.Core.Models;
using TreeYield.Core.Services;
using TreeYield.Tests.Framework;

namespace TreeYield.Tests
{
    /// <summary>Tests for CSV quoting and formula-injection protection in <see cref="CsvExporter"/>.</summary>
    public class CsvExporterTests
    {
        [Test]
        public void PlainText_IsUnchanged()
        {
            Assert.AreEqual("Carabao Mango", CsvExporter.EscapeField("Carabao Mango"));
        }

        [Test]
        public void TextWithComma_IsQuoted()
        {
            Assert.AreEqual("\"Mango, Carabao\"", CsvExporter.EscapeField("Mango, Carabao"));
        }

        [Test]
        public void InnerQuotes_AreDoubled()
        {
            Assert.AreEqual("\"Mangifera indica \"\"Carabao\"\"\"", CsvExporter.EscapeField("Mangifera indica \"Carabao\""));
        }

        [Test]
        public void LineBreak_IsQuoted()
        {
            Assert.AreEqual("\"line1\nline2\"", CsvExporter.EscapeField("line1\nline2"));
        }

        [Test]
        public void FormulaCharacters_ArePrefixedWithApostrophe()
        {
            Assert.AreEqual("'=1+1", CsvExporter.EscapeField("=1+1"));
            Assert.AreEqual("'+63 912", CsvExporter.EscapeField("+63 912"));
            Assert.AreEqual("'-2", CsvExporter.EscapeField("-2"));
            Assert.AreEqual("'@SUM(A1)", CsvExporter.EscapeField("@SUM(A1)"));
        }

        [Test]
        public void FormulaWithComma_IsPrefixedAndQuoted()
        {
            Assert.AreEqual("\"'=HYPERLINK(\"\"http://x\"\",\"\"click\"\")\"",
                CsvExporter.EscapeField("=HYPERLINK(\"http://x\",\"click\")"));
        }

        [Test]
        public void NullOrEmpty_BecomesEmptyCell()
        {
            Assert.AreEqual(string.Empty, CsvExporter.EscapeField(null));
            Assert.AreEqual(string.Empty, CsvExporter.EscapeField(string.Empty));
        }

        [Test]
        public void BuildReport_ContainsInputsResultsAndEveryYear()
        {
            TreeSpecies mango = TestData.Mango();
            YieldResult result = YieldCalculator.Calculate(mango, 25);
            YieldProjection projection = YieldCalculator.Project(mango, 25, 10);

            string csv = CsvExporter.BuildReport(result, projection, new DateTime(2026, 10, 26, 9, 30, 0));
            string[] lines = csv.Split(new[] { "\r\n" }, StringSplitOptions.None);

            Assert.IsTrue(lines.Contains("Species,Carabao Mango"));
            Assert.IsTrue(lines.Contains("Tree count,25"));
            Assert.IsTrue(lines.Contains("Estimated annual gross sales (PHP),600000.00"));
            Assert.IsTrue(lines.Contains("Generated,2026-10-26 09:30"));
            // Year rows look like "5,16.7%,1250.00,..." - digits followed by a comma.
            Assert.AreEqual(10, lines.Count(l => System.Text.RegularExpressions.Regex.IsMatch(l, @"^\d+,")), "one row per projected year");
        }
    }
}
