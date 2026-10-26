using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TreeYield.WinForms.UI;

namespace TreeYield.WinForms.Controls
{
    /// <summary>
    /// A white rounded card that shows a small title, one large value and an optional
    /// caption, like "Annual Yield / 7,500 kg". The value font shrinks automatically
    /// when a very large number would not fit.
    /// </summary>
    [ToolboxItem(true)]
    [Description("Card that shows a title, a large value and a caption.")]
    public class StatCard : Control
    {
        private const int Inset = 16;
        private const float MinimumValueFontSize = 9f;

        private string _title = "Title";
        private string _value = "-";
        private string _caption = string.Empty;
        private Color _valueColor = Color.FromArgb(31, 31, 31);
        private Font _fittedValueFont;

        /// <summary>Creates the card.</summary>
        public StatCard()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, false);
            TabStop = false;
            Size = new Size(260, 96);
            AccessibleRole = AccessibleRole.StaticText;
        }

        /// <summary>Small gray heading, for example "Annual Yield".</summary>
        [Category("Appearance")]
        [DefaultValue("Title")]
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value ?? string.Empty;
                UpdateAccessibleName();
                Invalidate();
            }
        }

        /// <summary>The large value, for example "7,500 kg".</summary>
        [Category("Appearance")]
        [DefaultValue("-")]
        public string Value
        {
            get { return _value; }
            set
            {
                string newValue = value ?? string.Empty;
                if (newValue == _value)
                {
                    return;
                }

                _value = newValue;
                RefitValueFont();
                UpdateAccessibleName();
                Invalidate();
            }
        }

        /// <summary>Optional small text under the value.</summary>
        [Category("Appearance")]
        [DefaultValue("")]
        public string Caption
        {
            get { return _caption; }
            set
            {
                _caption = value ?? string.Empty;
                UpdateAccessibleName();
                Invalidate();
            }
        }

        /// <summary>Color of the large value (green for money).</summary>
        [Category("Appearance")]
        public Color ValueColor
        {
            get { return _valueColor; }
            set
            {
                _valueColor = value;
                Invalidate();
            }
        }

        private bool ShouldSerializeValueColor()
        {
            return _valueColor != Color.FromArgb(31, 31, 31);
        }

        private void ResetValueColor()
        {
            ValueColor = Color.FromArgb(31, 31, 31);
        }

        /// <inheritdoc />
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            RefitValueFont();
        }

        /// <inheritdoc />
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            RefitValueFont();
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeFittedFont();
            }

            base.Dispose(disposing);
        }

        /// <summary>Draws the card background, title, value and caption.</summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Drawing.GetParentBackColor(this));
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var bounds = new RectangleF(0.5f, 0.5f, Width - 2f, Height - 2f);
            using (GraphicsPath path = Drawing.RoundedRectangle(bounds, 10f))
            {
                using (var fill = new SolidBrush(Theme.Surface))
                {
                    g.FillPath(fill, path);
                }

                using (var pen = new Pen(Theme.Border))
                {
                    g.DrawPath(pen, path);
                }
            }

            const TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.SingleLine
                | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix;
            int width = Width - 2 * Inset;

            using (var titleFont = new Font(Theme.FontFamily, 9.75f))
            using (var captionFont = new Font(Theme.FontFamily, 8.25f))
            {
                Font valueFont = _fittedValueFont ?? Font;
                int titleHeight = TextRenderer.MeasureText(g, "Ag", titleFont).Height;
                int valueHeight = TextRenderer.MeasureText(g, "Ag", valueFont).Height;

                int y = Inset - 2;
                TextRenderer.DrawText(g, _title, titleFont, new Rectangle(Inset, y, width, titleHeight), Theme.TextSecondary, flags);

                y += titleHeight + 2;
                TextRenderer.DrawText(g, _value, valueFont, new Rectangle(Inset, y, width, valueHeight), _valueColor, flags);

                if (_caption.Length > 0)
                {
                    y += valueHeight;
                    int captionHeight = TextRenderer.MeasureText(g, "Ag", captionFont).Height;
                    TextRenderer.DrawText(g, _caption, captionFont, new Rectangle(Inset, y, width, captionHeight), Theme.TextSecondary, flags);
                }
            }
        }

        /// <summary>
        /// Finds the largest font size (down to 9 pt) at which the value fits on one line.
        /// Done when the value, font or size changes, not on every paint.
        /// </summary>
        private void RefitValueFont()
        {
            DisposeFittedFont();
            int available = Width - 2 * Inset;
            if (available <= 0 || string.IsNullOrEmpty(_value))
            {
                return;
            }

            if (TextRenderer.MeasureText(_value, Font).Width <= available)
            {
                return;
            }

            for (float size = Font.SizeInPoints - 1f; size >= MinimumValueFontSize; size -= 1f)
            {
                var candidate = new Font(Font.FontFamily, size, Font.Style);
                if (TextRenderer.MeasureText(_value, candidate).Width <= available || size - 1f < MinimumValueFontSize)
                {
                    _fittedValueFont = candidate;
                    return;
                }

                candidate.Dispose();
            }
        }

        private void DisposeFittedFont()
        {
            if (_fittedValueFont != null)
            {
                _fittedValueFont.Dispose();
                _fittedValueFont = null;
            }
        }

        private void UpdateAccessibleName()
        {
            // Screen readers read the whole card as one sentence, e.g. "Annual Yield: 7,500 kg".
            AccessibleName = _caption.Length > 0
                ? _title + ": " + _value + ". " + _caption
                : _title + ": " + _value;
        }
    }
}
