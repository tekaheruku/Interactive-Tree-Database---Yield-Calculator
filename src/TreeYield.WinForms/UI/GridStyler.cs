using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace TreeYield.WinForms.UI
{
    /// <summary>
    /// Gives every DataGridView the same flat, read-only look: no row headers, thin
    /// horizontal lines, roomy rows and a soft blue selection.
    /// </summary>
    internal static class GridStyler
    {
        /// <summary>Applies the shared style to a grid.</summary>
        public static void Apply(DataGridView grid)
        {
            grid.BorderStyle = BorderStyle.None;
            grid.BackgroundColor = Theme.Surface;
            grid.GridColor = Theme.GridLine;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.RowHeadersVisible = false;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersHeight = 38;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.RowTemplate.Height = 34;
            grid.ShowCellToolTips = true;

            // Tab moves to the next control instead of the next cell, so keyboard users
            // are never trapped inside the grid (accessibility).
            grid.StandardTab = true;

            DataGridViewCellStyle header = grid.ColumnHeadersDefaultCellStyle;
            header.BackColor = Theme.Surface;
            header.ForeColor = Theme.TextSecondary;
            header.SelectionBackColor = Theme.Surface;
            header.SelectionForeColor = Theme.TextSecondary;
            header.Font = new Font(Theme.SemiboldFontFamily, 9F);
            header.Padding = new Padding(6, 0, 6, 0);
            header.Alignment = DataGridViewContentAlignment.MiddleLeft;

            DataGridViewCellStyle cells = grid.DefaultCellStyle;
            cells.BackColor = Theme.Surface;
            cells.ForeColor = Theme.TextPrimary;
            cells.SelectionBackColor = Theme.Selection;
            cells.SelectionForeColor = Theme.TextPrimary;
            cells.Font = new Font(Theme.FontFamily, 9.75F);
            cells.Padding = new Padding(6, 0, 6, 0);

            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(251, 251, 251);

            // DataGridView hides its DoubleBuffered property; turning it on removes flicker
            // while the grid is refilled during slider dragging.
            PropertyInfo doubleBuffered = typeof(DataGridView).GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (doubleBuffered != null)
            {
                doubleBuffered.SetValue(grid, true, null);
            }
        }

        /// <summary>Right-aligns a numeric column (cells and header), as numbers should be.</summary>
        public static void AlignRight(DataGridViewColumn column, string format)
        {
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            if (format != null)
            {
                column.DefaultCellStyle.Format = format;
            }
        }
    }
}
