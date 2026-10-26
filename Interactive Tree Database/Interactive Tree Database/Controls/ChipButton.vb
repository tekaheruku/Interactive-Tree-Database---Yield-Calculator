Imports System.ComponentModel
Imports System.Drawing.Drawing2D

''' <summary>Visual style of a <see cref="ChipButton"/>.</summary>
Public Enum ChipStyle
    ''' <summary>Filter chip: gray, or light blue when <see cref="ChipButton.Selected"/>.</summary>
    Toggle

    ''' <summary>Main action: solid blue with white text.</summary>
    Primary

    ''' <summary>Other actions: white with a gray border.</summary>
    Secondary

    ''' <summary>Destructive action (delete): red text and border.</summary>
    Danger
End Enum

''' <summary>
''' A pill-shaped button used for the filter chips ("All Species", "Fruit-Bearing" ...)
''' and for action buttons. It inherits from Button, so it keeps keyboard support
''' (Tab, Enter, Space), AcceptButton/CancelButton support and screen-reader support;
''' only the painting is custom.
''' </summary>
<ToolboxItem(True)>
<Description("Pill-shaped button for filter chips and actions.")>
Public Class ChipButton
    Inherits Button

    Private _selected As Boolean
    Private _style As ChipStyle = ChipStyle.Toggle
    Private _hovering As Boolean
    Private _pressed As Boolean

    ''' <summary>Creates the button.</summary>
    Public Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or
                 ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)
        FlatStyle = FlatStyle.Flat
        FlatAppearance.BorderSize = 0
        UseVisualStyleBackColor = False
        Cursor = Cursors.Hand
        Padding = New Padding(18, 0, 18, 0)
        Size = New Size(120, 36)
    End Sub

    ''' <summary>For <see cref="ChipStyle.Toggle"/>: whether this chip is the active choice in its group.</summary>
    <Category("Appearance")>
    <DefaultValue(False)>
    <Description("Whether the chip is the active choice in its group.")>
    Public Property Selected As Boolean
        Get
            Return _selected
        End Get
        Set(value As Boolean)
            If _selected = value Then
                Return
            End If

            _selected = value
            Invalidate()
            ' Lets screen readers announce the new checked state.
            AccessibilityNotifyClients(AccessibleEvents.StateChange, -1)
        End Set
    End Property

    ''' <summary>Which color scheme the button uses.</summary>
    <Category("Appearance")>
    <DefaultValue(GetType(ChipStyle), "Toggle")>
    <Description("Toggle (filter chip), Primary, Secondary or Danger.")>
    Public Property ChipStyle As ChipStyle
        Get
            Return _style
        End Get
        Set(value As ChipStyle)
            _style = value
            Invalidate()
        End Set
    End Property

    ''' <summary>Width that fits the text plus padding; used when AutoSize is True.</summary>
    Public Overrides Function GetPreferredSize(proposedSize As Size) As Size
        Dim textSize As Size = TextRenderer.MeasureText(If(Text, String.Empty), Font)
        Return New Size(textSize.Width + Padding.Horizontal, Math.Max(textSize.Height + 16, 32))
    End Function

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        _hovering = True
        Invalidate()
        MyBase.OnMouseEnter(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        _hovering = False
        _pressed = False
        Invalidate()
        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnMouseDown(mevent As MouseEventArgs)
        If mevent.Button = MouseButtons.Left Then
            _pressed = True
            Invalidate()
        End If

        MyBase.OnMouseDown(mevent)
    End Sub

    Protected Overrides Sub OnMouseUp(mevent As MouseEventArgs)
        _pressed = False
        Invalidate()
        MyBase.OnMouseUp(mevent)
    End Sub

    Protected Overrides Sub OnEnabledChanged(e As EventArgs)
        Invalidate()
        MyBase.OnEnabledChanged(e)
    End Sub

    ''' <summary>Draws the pill, the text and (when focused by keyboard) a focus ring.</summary>
    Protected Overrides Sub OnPaint(pevent As PaintEventArgs)
        Dim g As Graphics = pevent.Graphics
        g.Clear(DrawingHelpers.GetParentBackColor(Me))
        g.SmoothingMode = SmoothingMode.AntiAlias

        Dim back As Color
        Dim fore As Color
        Dim border As Color
        GetColors(back, fore, border)

        Dim bounds As New RectangleF(0.5F, 0.5F, Width - 2.0F, Height - 2.0F)
        Using path As GraphicsPath = DrawingHelpers.RoundedRectangle(bounds, bounds.Height / 2.0F)
            Using brush As New SolidBrush(back)
                g.FillPath(brush, path)
            End Using

            If Not border.IsEmpty Then
                Using pen As New Pen(border)
                    g.DrawPath(pen, path)
                End Using
            End If
        End Using

        Dim flags As TextFormatFlags = TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter Or
                                       TextFormatFlags.SingleLine Or TextFormatFlags.EndEllipsis
        If Not ShowKeyboardCues Then
            flags = flags Or TextFormatFlags.HidePrefix
        End If

        Dim textBounds As New Rectangle(Padding.Left \ 2, 0, Width - Padding.Horizontal \ 2, Height)
        TextRenderer.DrawText(g, Text, Font, textBounds, fore, flags)

        ' Visible focus indicator for keyboard users (WCAG 2.4.7).
        If Focused AndAlso ShowFocusCues Then
            Dim ring As New RectangleF(2.0F, 2.0F, Width - 5.0F, Height - 5.0F)
            Using path As GraphicsPath = DrawingHelpers.RoundedRectangle(ring, ring.Height / 2.0F),
                  pen As New Pen(Theme.Accent, 2.0F)
                g.DrawPath(pen, path)
            End Using
        End If
    End Sub

    ''' <summary>Reports "checked" to screen readers when a toggle chip is selected.</summary>
    Protected Overrides Function CreateAccessibilityInstance() As AccessibleObject
        Return New ChipAccessibleObject(Me)
    End Function

    Private Sub GetColors(ByRef back As Color, ByRef fore As Color, ByRef border As Color)
        border = Color.Empty

        If Not Enabled Then
            back = Color.FromArgb(241, 241, 241)
            fore = Theme.TextDisabled
            Return
        End If

        Select Case _style
            Case ChipStyle.Primary
                back = If(_pressed OrElse _hovering, Theme.AccentHover, Theme.Accent)
                fore = Color.White

            Case ChipStyle.Secondary
                back = If(_pressed, Theme.ChipHover, If(_hovering, Color.FromArgb(243, 243, 243), Theme.Surface))
                fore = Theme.TextPrimary
                border = Theme.Border

            Case ChipStyle.Danger
                back = If(_pressed OrElse _hovering, Theme.DangerSoft, Theme.Surface)
                fore = Theme.Danger
                border = Color.FromArgb(240, 180, 176)

            Case Else
                If _selected Then
                    back = If(_hovering, Color.FromArgb(207, 226, 252), Theme.AccentSoft)
                    fore = Theme.AccentText
                Else
                    back = If(_pressed OrElse _hovering, Theme.ChipHover, Theme.Chip)
                    fore = Theme.TextPrimary
                End If
        End Select
    End Sub

    Private NotInheritable Class ChipAccessibleObject
        Inherits ButtonBaseAccessibleObject

        Public Sub New(owner As ChipButton)
            MyBase.New(owner)
        End Sub

        Public Overrides ReadOnly Property State As AccessibleStates
            Get
                Dim chip As ChipButton = DirectCast(Owner, ChipButton)
                Dim baseState As AccessibleStates = MyBase.State
                If chip.ChipStyle = ChipStyle.Toggle AndAlso chip.Selected Then
                    Return baseState Or AccessibleStates.Checked
                End If

                Return baseState
            End Get
        End Property
    End Class

End Class
