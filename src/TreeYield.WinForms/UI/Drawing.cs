using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TreeYield.WinForms.UI
{
    /// <summary>Drawing helpers shared by the owner-drawn controls.</summary>
    internal static class Drawing
    {
        /// <summary>
        /// Builds a rectangle with rounded corners. The caller must dispose the path.
        /// </summary>
        /// <param name="bounds">Outer rectangle.</param>
        /// <param name="radius">Corner radius in pixels; limited to half the height/width.</param>
        public static GraphicsPath RoundedRectangle(RectangleF bounds, float radius)
        {
            float r = System.Math.Min(radius, System.Math.Min(bounds.Width, bounds.Height) / 2f);
            var path = new GraphicsPath();
            if (r <= 0.5f)
            {
                path.AddRectangle(bounds);
                return path;
            }

            float d = r * 2f;
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Returns the first solid background color among the control's parents.
        /// Owner-drawn controls paint this color behind their rounded corners so the
        /// corners blend into whatever they sit on.
        /// </summary>
        public static Color GetParentBackColor(Control control)
        {
            Control current = control.Parent;
            while (current != null)
            {
                if (current.BackColor.A == 255)
                {
                    return current.BackColor;
                }

                current = current.Parent;
            }

            return SystemColors.Control;
        }
    }
}
