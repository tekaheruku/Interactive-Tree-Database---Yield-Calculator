Imports System.ComponentModel
Imports System.Drawing.Drawing2D

''' <summary>
''' A Panel with rounded corners and an optional 1-pixel border. BackColor is the
''' fill color; the area outside the corners shows the parent's background.
''' </summary>
<ToolboxItem(True)>
<Description("Panel with rounded corners and an optional border.")>
Public Class RoundedPanel
    Inherits Panel

    Private _cornerRadius As Integer = 14
    Private _borderColor As Color = Color.Empty

    ''' <summary>Creates the panel.</summary>
    Public Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or
                 ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)
    End Sub

    ''' <summary>Radius of the corners in pixels.</summary>
    <Category("Appearance")>
    <DefaultValue(14)>
    <Description("Radius of the rounded corners, in pixels.")>
    Public Property CornerRadius As Integer
        Get
            Return _cornerRadius
        End Get
        Set(value As Integer)
            _cornerRadius = If(value < 0, 0, value)
            Invalidate()
        End Set
    End Property

    ''' <summary>Border color. Color.Empty means no border.</summary>
    <Category("Appearance")>
    <Description("Color of the 1-pixel border. Leave empty for no border.")>
    Public Property BorderColor As Color
        Get
            Return _borderColor
        End Get
        Set(value As Color)
            _borderColor = value
            Invalidate()
        End Set
    End Property

    ' Tells the designer when BorderColor differs from its default (so it is saved).
    Private Function ShouldSerializeBorderColor() As Boolean
        Return Not _borderColor.IsEmpty
    End Function

    Private Sub ResetBorderColor()
        BorderColor = Color.Empty
    End Sub

    ''' <summary>Paints the parent color, then the rounded fill and border.</summary>
    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
        Dim g As Graphics = e.Graphics
        g.Clear(DrawingHelpers.GetParentBackColor(Me))
        g.SmoothingMode = SmoothingMode.AntiAlias

        Dim bounds As New RectangleF(0, 0, Width - 1, Height - 1)
        Using path As GraphicsPath = DrawingHelpers.RoundedRectangle(bounds, _cornerRadius),
              fill As New SolidBrush(BackColor)
            g.FillPath(fill, path)
            If Not _borderColor.IsEmpty Then
                Using pen As New Pen(_borderColor)
                    g.DrawPath(pen, path)
                End Using
            End If
        End Using
    End Sub

    ''' <summary>Scales the corner radius together with the rest of the form on high-DPI screens.</summary>
    Protected Overrides Sub ScaleControl(factor As SizeF, specified As BoundsSpecified)
        MyBase.ScaleControl(factor, specified)
        _cornerRadius = CInt(Math.Round(_cornerRadius * factor.Height))
    End Sub

End Class
