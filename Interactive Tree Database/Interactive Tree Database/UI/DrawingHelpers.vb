Imports System.Drawing.Drawing2D

''' <summary>Drawing helpers shared by the owner-drawn controls.</summary>
Friend NotInheritable Class DrawingHelpers

    Private Shared _appIcon As Icon

    Private Sub New()
    End Sub

    ''' <summary>
    ''' The tree icon embedded in the .exe (set by ApplicationIcon in the project file),
    ''' loaded once and shared by every window that shows an icon.
    ''' </summary>
    Public Shared ReadOnly Property AppIcon As Icon
        Get
            If _appIcon Is Nothing Then
                Try
                    _appIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath)
                Catch ex As ArgumentException
                    _appIcon = SystemIcons.Application
                End Try
            End If

            Return _appIcon
        End Get
    End Property

    ''' <summary>
    ''' Builds a rectangle with rounded corners. The caller must dispose the path.
    ''' </summary>
    ''' <param name="bounds">Outer rectangle.</param>
    ''' <param name="radius">Corner radius in pixels; limited to half the height/width.</param>
    Public Shared Function RoundedRectangle(bounds As RectangleF, radius As Single) As GraphicsPath
        Dim r As Single = Math.Min(radius, Math.Min(bounds.Width, bounds.Height) / 2.0F)
        Dim path As New GraphicsPath()
        If r <= 0.5F Then
            path.AddRectangle(bounds)
            Return path
        End If

        Dim d As Single = r * 2.0F
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90)
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90)
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90)
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90)
        path.CloseFigure()
        Return path
    End Function

    ''' <summary>
    ''' Returns the first solid background color among the control's parents.
    ''' Owner-drawn controls paint this color behind their rounded corners so the
    ''' corners blend into whatever they sit on.
    ''' </summary>
    Public Shared Function GetParentBackColor(control As Control) As Color
        Dim current As Control = control.Parent
        While current IsNot Nothing
            If current.BackColor.A = 255 Then
                Return current.BackColor
            End If

            current = current.Parent
        End While

        Return SystemColors.Control
    End Function

End Class
