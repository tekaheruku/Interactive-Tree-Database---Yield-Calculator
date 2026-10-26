Imports System.ComponentModel
Imports System.Drawing.Drawing2D

''' <summary>
''' A white rounded card that shows a small title, one large value and an optional
''' caption, like "Annual Yield / 7,500 kg". The value font shrinks automatically
''' when a very large number would not fit.
''' </summary>
<ToolboxItem(True)>
<Description("Card that shows a title, a large value and a caption.")>
Public Class StatCard
    Inherits Control

    Private Const Inset As Integer = 16
    Private Const MinimumValueFontSize As Single = 9.0F

    Private _title As String = "Title"
    Private _value As String = "-"
    Private _caption As String = String.Empty
    Private _valueColor As Color = Color.FromArgb(31, 31, 31)
    Private _fittedValueFont As Font

    ''' <summary>Creates the card.</summary>
    Public Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or
                 ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)
        SetStyle(ControlStyles.Selectable, False)
        TabStop = False
        Size = New Size(260, 96)
        AccessibleRole = AccessibleRole.StaticText
    End Sub

    ''' <summary>Small gray heading, for example "Annual Yield".</summary>
    <Category("Appearance")>
    <DefaultValue("Title")>
    Public Property Title As String
        Get
            Return _title
        End Get
        Set(value As String)
            _title = If(value, String.Empty)
            UpdateAccessibleName()
            Invalidate()
        End Set
    End Property

    ''' <summary>The large value, for example "7,500 kg".</summary>
    <Category("Appearance")>
    <DefaultValue("-")>
    Public Property Value As String
        Get
            Return _value
        End Get
        Set(value As String)
            Dim newValue As String = If(value, String.Empty)
            If newValue = _value Then
                Return
            End If

            _value = newValue
            RefitValueFont()
            UpdateAccessibleName()
            Invalidate()
        End Set
    End Property

    ''' <summary>Optional small text under the value.</summary>
    <Category("Appearance")>
    <DefaultValue("")>
    Public Property Caption As String
        Get
            Return _caption
        End Get
        Set(value As String)
            _caption = If(value, String.Empty)
            UpdateAccessibleName()
            Invalidate()
        End Set
    End Property

    ''' <summary>Color of the large value (green for money).</summary>
    <Category("Appearance")>
    Public Property ValueColor As Color
        Get
            Return _valueColor
        End Get
        Set(value As Color)
            _valueColor = value
            Invalidate()
        End Set
    End Property

    Private Function ShouldSerializeValueColor() As Boolean
        Return _valueColor <> Color.FromArgb(31, 31, 31)
    End Function

    Private Sub ResetValueColor()
        ValueColor = Color.FromArgb(31, 31, 31)
    End Sub

    Protected Overrides Sub OnFontChanged(e As EventArgs)
        MyBase.OnFontChanged(e)
        RefitValueFont()
    End Sub

    Protected Overrides Sub OnSizeChanged(e As EventArgs)
        MyBase.OnSizeChanged(e)
        RefitValueFont()
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            DisposeFittedFont()
        End If

        MyBase.Dispose(disposing)
    End Sub

    ''' <summary>Draws the card background, title, value and caption.</summary>
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g As Graphics = e.Graphics
        g.Clear(DrawingHelpers.GetParentBackColor(Me))
        g.SmoothingMode = SmoothingMode.AntiAlias

        Dim bounds As New RectangleF(0.5F, 0.5F, Width - 2.0F, Height - 2.0F)
        Using path As GraphicsPath = DrawingHelpers.RoundedRectangle(bounds, 10.0F)
            Using fill As New SolidBrush(Theme.Surface)
                g.FillPath(fill, path)
            End Using

            Using pen As New Pen(Theme.Border)
                g.DrawPath(pen, path)
            End Using
        End Using

        Const flags As TextFormatFlags = TextFormatFlags.Left Or TextFormatFlags.Top Or TextFormatFlags.SingleLine Or
                                         TextFormatFlags.EndEllipsis Or TextFormatFlags.NoPrefix
        Dim textWidth As Integer = Width - 2 * Inset

        Using titleFont As New Font(Theme.FontFamily, 9.75F),
              captionFont As New Font(Theme.FontFamily, 8.25F)
            Dim valueFont As Font = If(_fittedValueFont, Font)
            Dim titleHeight As Integer = TextRenderer.MeasureText(g, "Ag", titleFont).Height
            Dim valueHeight As Integer = TextRenderer.MeasureText(g, "Ag", valueFont).Height

            Dim y As Integer = Inset - 2
            TextRenderer.DrawText(g, _title, titleFont, New Rectangle(Inset, y, textWidth, titleHeight), Theme.TextSecondary, flags)

            y += titleHeight + 2
            TextRenderer.DrawText(g, _value, valueFont, New Rectangle(Inset, y, textWidth, valueHeight), _valueColor, flags)

            If _caption.Length > 0 Then
                y += valueHeight
                Dim captionHeight As Integer = TextRenderer.MeasureText(g, "Ag", captionFont).Height
                TextRenderer.DrawText(g, _caption, captionFont, New Rectangle(Inset, y, textWidth, captionHeight), Theme.TextSecondary, flags)
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Finds the largest font size (down to 9 pt) at which the value fits on one line.
    ''' Done when the value, font or size changes, not on every paint.
    ''' </summary>
    Private Sub RefitValueFont()
        DisposeFittedFont()
        Dim available As Integer = Width - 2 * Inset
        If available <= 0 OrElse String.IsNullOrEmpty(_value) Then
            Return
        End If

        If TextRenderer.MeasureText(_value, Font).Width <= available Then
            Return
        End If

        Dim size As Single = Font.SizeInPoints - 1.0F
        While size >= MinimumValueFontSize
            Dim candidate As New Font(Font.FontFamily, size, Font.Style)
            If TextRenderer.MeasureText(_value, candidate).Width <= available OrElse size - 1.0F < MinimumValueFontSize Then
                _fittedValueFont = candidate
                Return
            End If

            candidate.Dispose()
            size -= 1.0F
        End While
    End Sub

    Private Sub DisposeFittedFont()
        If _fittedValueFont IsNot Nothing Then
            _fittedValueFont.Dispose()
            _fittedValueFont = Nothing
        End If
    End Sub

    Private Sub UpdateAccessibleName()
        ' Screen readers read the whole card as one sentence, e.g. "Annual Yield: 7,500 kg".
        AccessibleName = If(_caption.Length > 0,
                            _title & ": " & _value & ". " & _caption,
                            _title & ": " & _value)
    End Sub

End Class
