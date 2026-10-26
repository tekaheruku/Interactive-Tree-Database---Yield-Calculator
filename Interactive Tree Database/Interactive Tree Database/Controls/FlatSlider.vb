Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports System.Globalization

''' <summary>
''' A flat slider in the style of the design mock-up: a thick dark track up to the
''' thumb, a thin gray rail after it, and a narrow vertical thumb. Works with the
''' mouse (click, drag, wheel) and the keyboard (arrows, Page Up/Down, Home/End).
''' </summary>
<ToolboxItem(True)>
<DefaultEvent("ValueChanged")>
<DefaultProperty("Value")>
<Description("Flat slider for choosing a whole number.")>
Public Class FlatSlider
    Inherits Control

    Private Const EdgePadding As Integer = 8

    Private _minimum As Integer = 1
    Private _maximum As Integer = 1000
    Private _value As Integer = 25
    Private _smallChange As Integer = 1
    Private _largeChange As Integer = 10
    Private _dragging As Boolean

    ''' <summary>Raised whenever <see cref="Value"/> changes.</summary>
    <Category("Action")>
    <Description("Occurs when the value changes.")>
    Public Event ValueChanged As EventHandler

    ''' <summary>Creates the slider.</summary>
    Public Sub New()
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.ResizeRedraw Or ControlStyles.Selectable Or ControlStyles.StandardClick, True)
        TabStop = True
        Cursor = Cursors.Hand
        Size = New Size(240, 32)
        AccessibleRole = AccessibleRole.Slider
    End Sub

    ''' <summary>Lowest value.</summary>
    <Category("Behavior")>
    <DefaultValue(1)>
    Public Property Minimum As Integer
        Get
            Return _minimum
        End Get
        Set(value As Integer)
            _minimum = value
            If _maximum < _minimum Then
                _maximum = _minimum
            End If

            Me.Value = _value
            Invalidate()
        End Set
    End Property

    ''' <summary>Highest value.</summary>
    <Category("Behavior")>
    <DefaultValue(1000)>
    Public Property Maximum As Integer
        Get
            Return _maximum
        End Get
        Set(value As Integer)
            _maximum = value
            If _minimum > _maximum Then
                _minimum = _maximum
            End If

            Me.Value = _value
            Invalidate()
        End Set
    End Property

    ''' <summary>Current value, always kept between <see cref="Minimum"/> and <see cref="Maximum"/>.</summary>
    <Category("Behavior")>
    <DefaultValue(25)>
    Public Property Value As Integer
        Get
            Return _value
        End Get
        Set(value As Integer)
            Dim clamped As Integer = Math.Max(_minimum, Math.Min(_maximum, value))
            If clamped = _value Then
                Return
            End If

            _value = clamped
            Invalidate()
            AccessibilityNotifyClients(AccessibleEvents.ValueChange, -1)
            OnValueChanged(EventArgs.Empty)
        End Set
    End Property

    ''' <summary>Step for arrow keys and the mouse wheel.</summary>
    <Category("Behavior")>
    <DefaultValue(1)>
    Public Property SmallChange As Integer
        Get
            Return _smallChange
        End Get
        Set(value As Integer)
            _smallChange = Math.Max(1, value)
        End Set
    End Property

    ''' <summary>Step for Page Up / Page Down.</summary>
    <Category("Behavior")>
    <DefaultValue(10)>
    Public Property LargeChange As Integer
        Get
            Return _largeChange
        End Get
        Set(value As Integer)
            _largeChange = Math.Max(1, value)
        End Set
    End Property

    ''' <summary>Raises <see cref="ValueChanged"/>.</summary>
    Protected Overridable Sub OnValueChanged(e As EventArgs)
        RaiseEvent ValueChanged(Me, e)
    End Sub

    ''' <summary>Arrow keys normally move focus between controls; here they move the slider.</summary>
    Protected Overrides Function IsInputKey(keyData As Keys) As Boolean
        Select Case keyData And Keys.KeyCode
            Case Keys.Left, Keys.Right, Keys.Up, Keys.Down
                Return True
            Case Else
                Return MyBase.IsInputKey(keyData)
        End Select
    End Function

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        Select Case e.KeyCode
            Case Keys.Left, Keys.Down
                Value -= _smallChange
            Case Keys.Right, Keys.Up
                Value += _smallChange
            Case Keys.PageDown
                Value -= _largeChange
            Case Keys.PageUp
                Value += _largeChange
            Case Keys.Home
                Value = _minimum
            Case Keys.End
                Value = _maximum
            Case Else
                MyBase.OnKeyDown(e)
                Return
        End Select

        e.Handled = True
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If e.Button <> MouseButtons.Left Then
            Return
        End If

        Focus()
        _dragging = True
        Capture = True
        Value = ValueFromX(e.X)
    End Sub

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MyBase.OnMouseMove(e)
        If _dragging Then
            Value = ValueFromX(e.X)
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        _dragging = False
        Capture = False
    End Sub

    Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)
        MyBase.OnMouseWheel(e)
        Value += Math.Sign(e.Delta) * _smallChange

        ' Stop the wheel from also scrolling the parent.
        Dim handled As HandledMouseEventArgs = TryCast(e, HandledMouseEventArgs)
        If handled IsNot Nothing Then
            handled.Handled = True
        End If
    End Sub

    Protected Overrides Sub OnGotFocus(e As EventArgs)
        MyBase.OnGotFocus(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnLostFocus(e As EventArgs)
        MyBase.OnLostFocus(e)
        Invalidate()
    End Sub

    Protected Overrides Sub OnEnabledChanged(e As EventArgs)
        MyBase.OnEnabledChanged(e)
        Invalidate()
    End Sub

    ''' <summary>Draws the track, rail, end dot and thumb.</summary>
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g As Graphics = e.Graphics
        g.Clear(DrawingHelpers.GetParentBackColor(Me))
        g.SmoothingMode = SmoothingMode.AntiAlias

        Dim trackColor As Color = If(Enabled, Theme.SliderTrack, Theme.TextDisabled)
        Dim left As Single = EdgePadding
        Dim right As Single = Width - EdgePadding
        Dim centerY As Single = Height / 2.0F
        Dim thumbX As Single = GetThumbX()

        Using rail As New Pen(Theme.SliderRail, 2.0F)
            rail.StartCap = LineCap.Round
            rail.EndCap = LineCap.Round
            g.DrawLine(rail, thumbX, centerY, right, centerY)
        End Using

        Using track As New Pen(trackColor, 4.0F)
            track.StartCap = LineCap.Round
            track.EndCap = LineCap.Round
            g.DrawLine(track, left, centerY, thumbX, centerY)
        End Using

        ' Small dot that marks the end of the range.
        Using dot As New SolidBrush(trackColor)
            g.FillEllipse(dot, right - 2.0F, centerY - 2.0F, 4.0F, 4.0F)
        End Using

        Dim thumbHeight As Single = Math.Min(22.0F, Height - 6.0F)
        Dim thumb As New RectangleF(thumbX - 2.5F, centerY - thumbHeight / 2.0F, 5.0F, thumbHeight)

        If Focused Then
            ' Soft halo shows which control has keyboard focus.
            Dim halo As RectangleF = RectangleF.Inflate(thumb, 5.0F, 3.0F)
            Using path As GraphicsPath = DrawingHelpers.RoundedRectangle(halo, 5.0F),
                  brush As New SolidBrush(Theme.AccentSoft)
                g.FillPath(brush, path)
            End Using
        End If

        Using path As GraphicsPath = DrawingHelpers.RoundedRectangle(thumb, 2.5F),
              brush As New SolidBrush(trackColor)
            g.FillPath(brush, path)
        End Using
    End Sub

    ''' <summary>Gives screen readers the current value.</summary>
    Protected Overrides Function CreateAccessibilityInstance() As AccessibleObject
        Return New SliderAccessibleObject(Me)
    End Function

    Private Function GetThumbX() As Single
        Dim usable As Single = Width - 2 * EdgePadding
        If _maximum = _minimum OrElse usable <= 0 Then
            Return EdgePadding
        End If

        Return EdgePadding + usable * (_value - _minimum) / CSng(_maximum - _minimum)
    End Function

    Private Function ValueFromX(x As Integer) As Integer
        Dim usable As Single = Width - 2 * EdgePadding
        If usable <= 0 Then
            Return _minimum
        End If

        Dim fraction As Single = Math.Max(0.0F, Math.Min(1.0F, (x - EdgePadding) / usable))
        Return _minimum + CInt(Math.Round(fraction * (_maximum - _minimum)))
    End Function

    Private NotInheritable Class SliderAccessibleObject
        Inherits ControlAccessibleObject

        Public Sub New(owner As FlatSlider)
            MyBase.New(owner)
        End Sub

        Public Overrides Property Value As String
            Get
                Return DirectCast(Owner, FlatSlider).Value.ToString(CultureInfo.CurrentCulture)
            End Get
            Set(value As String)
                Dim parsed As Integer
                If Integer.TryParse(value, NumberStyles.Integer, CultureInfo.CurrentCulture, parsed) Then
                    DirectCast(Owner, FlatSlider).Value = parsed
                End If
            End Set
        End Property

        Public Overrides ReadOnly Property Role As AccessibleRole
            Get
                Return AccessibleRole.Slider
            End Get
        End Property
    End Class

End Class
