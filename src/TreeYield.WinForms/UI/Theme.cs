using System.Drawing;

namespace TreeYield.WinForms.UI
{
    /// <summary>
    /// The color palette and font names used by the custom controls and code-built
    /// UI, so every screen looks consistent. Text colors were checked for at least
    /// 4.5:1 contrast on their background (WCAG 2.1 AA).
    /// </summary>
    internal static class Theme
    {
        /// <summary>Window background (light gray).</summary>
        public static readonly Color Background = Color.FromArgb(247, 247, 248);

        /// <summary>Background of the calculator panel.</summary>
        public static readonly Color Panel = Color.FromArgb(241, 239, 239);

        /// <summary>Cards, grids and dialogs.</summary>
        public static readonly Color Surface = Color.White;

        /// <summary>Thin borders around cards and inputs.</summary>
        public static readonly Color Border = Color.FromArgb(226, 226, 226);

        /// <summary>Main text.</summary>
        public static readonly Color TextPrimary = Color.FromArgb(31, 31, 31);

        /// <summary>Labels, captions and hints.</summary>
        public static readonly Color TextSecondary = Color.FromArgb(95, 99, 104);

        /// <summary>Disabled text.</summary>
        public static readonly Color TextDisabled = Color.FromArgb(160, 160, 160);

        /// <summary>Primary buttons and focus rings.</summary>
        public static readonly Color Accent = Color.FromArgb(26, 115, 232);

        /// <summary>Primary button while the mouse is over it.</summary>
        public static readonly Color AccentHover = Color.FromArgb(23, 101, 204);

        /// <summary>Background of the selected filter chip.</summary>
        public static readonly Color AccentSoft = Color.FromArgb(220, 235, 254);

        /// <summary>Text on <see cref="AccentSoft"/> (darker blue for readable contrast).</summary>
        public static readonly Color AccentText = Color.FromArgb(21, 87, 176);

        /// <summary>Unselected chip background.</summary>
        public static readonly Color Chip = Color.FromArgb(236, 236, 236);

        /// <summary>Unselected chip while the mouse is over it.</summary>
        public static readonly Color ChipHover = Color.FromArgb(226, 226, 226);

        /// <summary>Money amounts (green, as in the design mock-up).</summary>
        public static readonly Color Money = Color.FromArgb(11, 128, 67);

        /// <summary>Errors and destructive actions.</summary>
        public static readonly Color Danger = Color.FromArgb(197, 34, 31);

        /// <summary>Danger button while the mouse is over it.</summary>
        public static readonly Color DangerSoft = Color.FromArgb(252, 232, 230);

        /// <summary>Horizontal lines between grid rows.</summary>
        public static readonly Color GridLine = Color.FromArgb(236, 236, 236);

        /// <summary>Selected grid row.</summary>
        public static readonly Color Selection = Color.FromArgb(232, 240, 254);

        /// <summary>Filled part of the slider track and the thumb.</summary>
        public static readonly Color SliderTrack = Color.FromArgb(31, 31, 31);

        /// <summary>Unfilled part of the slider track.</summary>
        public static readonly Color SliderRail = Color.FromArgb(214, 214, 214);

        /// <summary>Chart columns (first color of the validated data-visualization palette).</summary>
        public static readonly Color ChartSeries = Color.FromArgb(42, 120, 214);

        /// <summary>Main UI font family (ships with every Windows version since Vista).</summary>
        public const string FontFamily = "Segoe UI";

        /// <summary>Semibold variant used for buttons and headings.</summary>
        public const string SemiboldFontFamily = "Segoe UI Semibold";
    }
}
