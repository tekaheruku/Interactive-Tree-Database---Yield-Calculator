''' <summary>
''' The color palette and font names used by the custom controls and code-built
''' UI, so every screen looks consistent. Text colors were checked for at least
''' 4.5:1 contrast on their background (WCAG 2.1 AA).
''' </summary>
Friend NotInheritable Class Theme

    ''' <summary>Window background (light gray).</summary>
    Public Shared ReadOnly Background As Color = Color.FromArgb(247, 247, 248)

    ''' <summary>Background of the calculator panel.</summary>
    Public Shared ReadOnly Panel As Color = Color.FromArgb(241, 239, 239)

    ''' <summary>Cards, grids and dialogs.</summary>
    Public Shared ReadOnly Surface As Color = Color.White

    ''' <summary>Thin borders around cards and inputs.</summary>
    Public Shared ReadOnly Border As Color = Color.FromArgb(226, 226, 226)

    ''' <summary>Main text.</summary>
    Public Shared ReadOnly TextPrimary As Color = Color.FromArgb(31, 31, 31)

    ''' <summary>Labels, captions and hints.</summary>
    Public Shared ReadOnly TextSecondary As Color = Color.FromArgb(95, 99, 104)

    ''' <summary>Disabled text.</summary>
    Public Shared ReadOnly TextDisabled As Color = Color.FromArgb(160, 160, 160)

    ''' <summary>Primary buttons and focus rings.</summary>
    Public Shared ReadOnly Accent As Color = Color.FromArgb(26, 115, 232)

    ''' <summary>Primary button while the mouse is over it.</summary>
    Public Shared ReadOnly AccentHover As Color = Color.FromArgb(23, 101, 204)

    ''' <summary>Background of the selected filter chip.</summary>
    Public Shared ReadOnly AccentSoft As Color = Color.FromArgb(220, 235, 254)

    ''' <summary>Text on <see cref="AccentSoft"/> (darker blue for readable contrast).</summary>
    Public Shared ReadOnly AccentText As Color = Color.FromArgb(21, 87, 176)

    ''' <summary>Unselected chip background.</summary>
    Public Shared ReadOnly Chip As Color = Color.FromArgb(236, 236, 236)

    ''' <summary>Unselected chip while the mouse is over it.</summary>
    Public Shared ReadOnly ChipHover As Color = Color.FromArgb(226, 226, 226)

    ''' <summary>Money amounts (green, as in the design mock-up).</summary>
    Public Shared ReadOnly Money As Color = Color.FromArgb(11, 128, 67)

    ''' <summary>Errors and destructive actions.</summary>
    Public Shared ReadOnly Danger As Color = Color.FromArgb(197, 34, 31)

    ''' <summary>Danger button while the mouse is over it.</summary>
    Public Shared ReadOnly DangerSoft As Color = Color.FromArgb(252, 232, 230)

    ''' <summary>Horizontal lines between grid rows.</summary>
    Public Shared ReadOnly GridLine As Color = Color.FromArgb(236, 236, 236)

    ''' <summary>Selected grid row.</summary>
    Public Shared ReadOnly Selection As Color = Color.FromArgb(232, 240, 254)

    ''' <summary>Filled part of the slider track and the thumb.</summary>
    Public Shared ReadOnly SliderTrack As Color = Color.FromArgb(31, 31, 31)

    ''' <summary>Unfilled part of the slider track.</summary>
    Public Shared ReadOnly SliderRail As Color = Color.FromArgb(214, 214, 214)

    ''' <summary>Chart columns (first color of the validated data-visualization palette).</summary>
    Public Shared ReadOnly ChartSeries As Color = Color.FromArgb(42, 120, 214)

    ''' <summary>Main UI font family (ships with every Windows version since Vista).</summary>
    Public Const FontFamily As String = "Segoe UI"

    ''' <summary>Semibold variant used for buttons and headings.</summary>
    Public Const SemiboldFontFamily As String = "Segoe UI Semibold"

    Private Sub New()
    End Sub

End Class
