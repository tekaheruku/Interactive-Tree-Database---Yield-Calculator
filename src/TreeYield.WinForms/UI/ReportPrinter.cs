using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Windows.Forms;
using TreeYield.Core.Models;
using TreeYield.Core.Services;

namespace TreeYield.WinForms.UI
{
    /// <summary>
    /// Draws the printable projection report with System.Drawing.Printing and shows it
    /// in a print preview window (from there it can be printed or saved as PDF with
    /// "Microsoft Print to PDF"). Long tables continue on extra pages.
    /// </summary>
    internal sealed class ReportPrinter : IDisposable
    {
        // Relative widths of the projection table columns.
        private static readonly float[] ColumnWeights = { 0.10f, 0.18f, 0.22f, 0.25f, 0.25f };

        private readonly PrintDocument _document = new PrintDocument();
        private readonly YieldResult _result;
        private readonly YieldProjection _projection;
        private int _nextRow;
        private int _pageNumber;

        /// <summary>Prepares a report for a calculation and its projection.</summary>
        public ReportPrinter(YieldResult result, YieldProjection projection)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (projection == null)
            {
                throw new ArgumentNullException(nameof(projection));
            }

            _result = result;
            _projection = projection;
            _document.DocumentName = "Tree Yield Report - " + result.Species.CommonName;
            _document.BeginPrint += (sender, e) =>
            {
                // Printing can run more than once (preview, then print), so start over each time.
                _nextRow = 0;
                _pageNumber = 0;
            };
            _document.PrintPage += OnPrintPage;
        }

        /// <summary>Opens the print preview window.</summary>
        /// <exception cref="InvalidPrinterException">No printer is installed.</exception>
        public void ShowPreview(IWin32Window owner)
        {
            using (var preview = new PrintPreviewDialog())
            {
                preview.Document = _document;
                preview.UseAntiAlias = true;
                preview.Width = 900;
                preview.Height = 1000;
                preview.Text = "Print Preview - " + _document.DocumentName;
                preview.ShowDialog(owner);
            }
        }

        /// <summary>Releases the print document.</summary>
        public void Dispose()
        {
            _document.Dispose();
        }

        private void OnPrintPage(object sender, PrintPageEventArgs e)
        {
            _pageNumber++;
            Graphics g = e.Graphics;
            Rectangle area = e.MarginBounds;
            float y = area.Top;

            using (var titleFont = new Font(Theme.FontFamily, 18f, FontStyle.Bold))
            using (var headingFont = new Font(Theme.SemiboldFontFamily, 11f))
            using (var bodyFont = new Font(Theme.FontFamily, 10f))
            using (var boldFont = new Font(Theme.SemiboldFontFamily, 10f))
            using (var smallFont = new Font(Theme.FontFamily, 8.5f))
            using (var textBrush = new SolidBrush(Theme.TextPrimary))
            using (var mutedBrush = new SolidBrush(Theme.TextSecondary))
            using (var linePen = new Pen(Theme.Border))
            {
                TreeSpecies s = _result.Species;

                if (_pageNumber == 1)
                {
                    g.DrawString("Tree Yield & Revenue Projection Report", titleFont, textBrush, area.Left, y);
                    y += titleFont.GetHeight(g) + 2;
                    g.DrawString("Generated " + DateTime.Now.ToString("MMMM d, yyyy h:mm tt", CultureInfo.InvariantCulture),
                        smallFont, mutedBrush, area.Left, y);
                    y += smallFont.GetHeight(g) + 14;

                    y = DrawHeading(g, "Species", headingFont, textBrush, linePen, area, y);
                    bool rotation = s.HarvestType == HarvestType.Rotation;
                    var details = new List<KeyValuePair<string, string>>
                    {
                        Pair("Species", s.CommonName + (string.IsNullOrEmpty(s.ScientificName) ? string.Empty : " (" + s.ScientificName + ")")),
                        Pair("Category / product", s.CategoryName + " / " + s.ProductType),
                        Pair("Yield per tree per year", Formatters.Quantity(s.YieldPerTreePerYear, s.YieldUnit) + (rotation ? " (mean annual increment)" : string.Empty)),
                        Pair("Price per unit", Formatters.PesoWithCentavos(s.PricePerUnit) + " per " + s.YieldUnit),
                        Pair("Harvest type", rotation ? "Rotation - cut once at " + Formatters.Years(s.YearsToFirstHarvest) : "Annual - first harvest in year " + Math.Max(1, s.YearsToFirstHarvest) + ", full production from year " + Math.Max(1, s.YearsToFullProduction))
                    };
                    y = DrawPairs(g, details, bodyFont, boldFont, mutedBrush, textBrush, area, y);

                    y += 8;
                    y = DrawHeading(g, "Results for " + _result.TreeCount.ToString("N0", CultureInfo.InvariantCulture) + " trees", headingFont, textBrush, linePen, area, y);
                    var results = new List<KeyValuePair<string, string>>
                    {
                        Pair(rotation ? "Annualized yield" : "Annual yield (full production)", Formatters.Quantity(_result.AnnualYield, s.YieldUnit)),
                        Pair(rotation ? "Annualized gross sales" : "Estimated annual gross sales", Formatters.Peso(_result.GrossSales)),
                        Pair(_projection.Years + "-year total", Formatters.Quantity(_projection.TotalYield, s.YieldUnit) + "  /  " + Formatters.Peso(_projection.TotalSales))
                    };
                    if (rotation)
                    {
                        results.Add(Pair("Harvest at year " + _result.HarvestYear, Formatters.Quantity(_result.HarvestYield, s.YieldUnit) + "  /  " + Formatters.Peso(_result.HarvestSales)));
                    }

                    y = DrawPairs(g, results, bodyFont, boldFont, mutedBrush, textBrush, area, y);
                    y += 8;
                    y = DrawHeading(g, _projection.Years + "-year projection", headingFont, textBrush, linePen, area, y);
                }
                else
                {
                    g.DrawString(_projection.Years + "-year projection (continued) - " + s.CommonName, headingFont, textBrush, area.Left, y);
                    y += headingFont.GetHeight(g) + 8;
                }

                y = DrawTable(g, bodyFont, boldFont, textBrush, linePen, area, y);

                bool finished = _nextRow >= _projection.Rows.Count;
                float footerTop = area.Bottom - smallFont.GetHeight(g) * 3;

                if (finished)
                {
                    var disclaimerBox = new RectangleF(area.Left, Math.Max(y + 12, footerTop - 34), area.Width, 40);
                    g.DrawString(YieldCalculator.EstimateDisclaimer, smallFont, mutedBrush, disclaimerBox);
                }

                g.DrawString("Page " + _pageNumber, smallFont, mutedBrush, area.Right - 60, area.Bottom + 10);
                e.HasMorePages = !finished;
            }
        }

        private float DrawTable(Graphics g, Font bodyFont, Font boldFont, Brush textBrush, Pen linePen, Rectangle area, float y)
        {
            string unit = _result.Species.YieldUnit;
            string[] headers = { "Year", "Production", "Yield (" + unit + ")", "Gross sales", "Cumulative sales" };
            float rowHeight = bodyFont.GetHeight(g) + 8;
            float bottomLimit = area.Bottom - 70;

            DrawRow(g, headers, boldFont, textBrush, area, y, rowHeight);
            y += rowHeight;
            g.DrawLine(linePen, area.Left, y, area.Right, y);

            while (_nextRow < _projection.Rows.Count && y + rowHeight <= bottomLimit)
            {
                ProjectionYear row = _projection.Rows[_nextRow];
                string[] cells =
                {
                    row.Year.ToString(CultureInfo.InvariantCulture),
                    Formatters.ProductionStage(row, _result.Species.HarvestType),
                    Formatters.Quantity(row.Yield),
                    Formatters.Peso(row.GrossSales),
                    Formatters.Peso(row.CumulativeSales)
                };
                DrawRow(g, cells, bodyFont, textBrush, area, y, rowHeight);
                y += rowHeight;
                g.DrawLine(linePen, area.Left, y, area.Right, y);
                _nextRow++;
            }

            return y;
        }

        private static void DrawRow(Graphics g, string[] cells, Font font, Brush brush, Rectangle area, float y, float height)
        {
            float x = area.Left;
            for (int i = 0; i < cells.Length; i++)
            {
                float width = area.Width * ColumnWeights[i];
                using (var format = new StringFormat())
                {
                    // Numbers are right-aligned so digits line up; the first two columns are text.
                    format.Alignment = i >= 2 ? StringAlignment.Far : StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                    format.Trimming = StringTrimming.EllipsisCharacter;
                    format.FormatFlags = StringFormatFlags.NoWrap;
                    g.DrawString(cells[i], font, brush, new RectangleF(x + 4, y, width - 8, height), format);
                }

                x += width;
            }
        }

        private static float DrawHeading(Graphics g, string text, Font font, Brush brush, Pen pen, Rectangle area, float y)
        {
            g.DrawString(text, font, brush, area.Left, y);
            y += font.GetHeight(g) + 3;
            g.DrawLine(pen, area.Left, y, area.Right, y);
            return y + 6;
        }

        private static float DrawPairs(Graphics g, IEnumerable<KeyValuePair<string, string>> pairs, Font labelFont, Font valueFont,
            Brush labelBrush, Brush valueBrush, Rectangle area, float y)
        {
            float labelWidth = area.Width * 0.34f;
            foreach (KeyValuePair<string, string> pair in pairs)
            {
                var valueBox = new RectangleF(area.Left + labelWidth, y, area.Width - labelWidth, 200);
                SizeF valueSize = g.MeasureString(pair.Value, valueFont, (int)valueBox.Width);
                g.DrawString(pair.Key, labelFont, labelBrush, area.Left, y);
                g.DrawString(pair.Value, valueFont, valueBrush, valueBox);
                y += Math.Max(labelFont.GetHeight(g), valueSize.Height) + 4;
            }

            return y;
        }

        private static KeyValuePair<string, string> Pair(string key, string value)
        {
            return new KeyValuePair<string, string>(key, value ?? string.Empty);
        }
    }
}
