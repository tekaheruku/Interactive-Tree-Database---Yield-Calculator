using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TreeYield.WinForms.UI;

namespace TreeYield.WinForms.Controls
{
    /// <summary>
    /// A Panel with rounded corners and an optional 1-pixel border. BackColor is the
    /// fill color; the area outside the corners shows the parent's background.
    /// </summary>
    [ToolboxItem(true)]
    [Description("Panel with rounded corners and an optional border.")]
    public class RoundedPanel : Panel
    {
        private int _cornerRadius = 14;
        private Color _borderColor = Color.Empty;

        /// <summary>Creates the panel.</summary>
        public RoundedPanel()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }

        /// <summary>Radius of the corners in pixels.</summary>
        [Category("Appearance")]
        [DefaultValue(14)]
        [Description("Radius of the rounded corners, in pixels.")]
        public int CornerRadius
        {
            get { return _cornerRadius; }
            set
            {
                _cornerRadius = value < 0 ? 0 : value;
                Invalidate();
            }
        }

        /// <summary>Border color. Color.Empty means no border.</summary>
        [Category("Appearance")]
        [Description("Color of the 1-pixel border. Leave empty for no border.")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        // Tells the designer when BorderColor differs from its default (so it is saved).
        private bool ShouldSerializeBorderColor()
        {
            return !_borderColor.IsEmpty;
        }

        private void ResetBorderColor()
        {
            BorderColor = Color.Empty;
        }

        /// <summary>Paints the parent color, then the rounded fill and border.</summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Drawing.GetParentBackColor(this));
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var bounds = new RectangleF(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = Drawing.RoundedRectangle(bounds, _cornerRadius))
            using (var fill = new SolidBrush(BackColor))
            {
                g.FillPath(fill, path);
                if (!_borderColor.IsEmpty)
                {
                    using (var pen = new Pen(_borderColor))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }
        }

        /// <summary>Scales the corner radius together with the rest of the form on high-DPI screens.</summary>
        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);
            _cornerRadius = (int)System.Math.Round(_cornerRadius * factor.Height);
        }
    }
}
