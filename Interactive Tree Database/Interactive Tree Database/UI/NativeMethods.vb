Imports System.Runtime.InteropServices

''' <summary>Small Windows API calls that .NET Framework 4.5.2 does not wrap.</summary>
Friend NotInheritable Class NativeMethods

    ' Edit-control message that shows gray "placeholder" text in an empty TextBox.
    Private Const EM_SETCUEBANNER As Integer = &H1501

    Private Sub New()
    End Sub

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function SendMessage(hWnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As String) As IntPtr
    End Function

    ''' <summary>
    ''' Shows placeholder text in a single-line TextBox while it is empty
    ''' (like the HTML placeholder attribute). Works once the handle exists.
    ''' </summary>
    Public Shared Sub SetCueBanner(textBox As TextBox, text As String)
        If textBox.IsHandleCreated Then
            ' wParam = 1 keeps the hint visible even while the box has focus.
            SendMessage(textBox.Handle, EM_SETCUEBANNER, New IntPtr(1), text)
        Else
            AddHandler textBox.HandleCreated, Sub(sender, e) SetCueBanner(textBox, text)
        End If
    End Sub

End Class
