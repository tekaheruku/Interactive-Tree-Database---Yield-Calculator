Imports TreeYield.Core.Models
Imports TreeYield.Core.Services

''' <summary>Administrator sign-in dialog. Returns OK and <see cref="SignedInUser"/> on success.</summary>
Public Class LoginForm

    Private ReadOnly _auth As AuthService
    Private _signedInUser As AppUser

    ''' <summary>Creates the dialog.</summary>
    Public Sub New(auth As AuthService)
        If auth Is Nothing Then
            Throw New ArgumentNullException(NameOf(auth))
        End If

        _auth = auth

        ' This call is required by the designer.
        InitializeComponent()
    End Sub

    ''' <summary>The administrator who signed in (set when the dialog returns OK).</summary>
    Public ReadOnly Property SignedInUser As AppUser
        Get
            Return _signedInUser
        End Get
    End Property

    Private Async Sub btnSignIn_Click(sender As Object, e As EventArgs) Handles btnSignIn.Click
        lblError.Text = String.Empty
        SetBusy(True)
        Try
            Dim result As LoginResult = Await _auth.LoginAsync(txtUsername.Text, txtPassword.Text)
            If result.Succeeded Then
                _signedInUser = result.User
                Me.DialogResult = System.Windows.Forms.DialogResult.OK ' closes the dialog
                Return
            End If

            lblError.Text = result.Message
            txtPassword.Clear()
            If String.IsNullOrWhiteSpace(txtUsername.Text) Then
                txtUsername.Focus()
            Else
                txtPassword.Focus()
            End If
        Catch ex As Exception
            ErrorReporter.ShowError(Me, "sign in", ex)
        Finally
            SetBusy(False)
        End Try
    End Sub

    Private Sub SetBusy(busy As Boolean)
        txtUsername.Enabled = Not busy
        txtPassword.Enabled = Not busy
        btnSignIn.Enabled = Not busy
        btnSignIn.Text = If(busy, "Signing in...", "Sign In")
        UseWaitCursor = busy
    End Sub

    Private Sub chkShowPassword_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowPassword.CheckedChanged
        txtPassword.UseSystemPasswordChar = Not chkShowPassword.Checked
    End Sub

    Private Sub txtPassword_Enter(sender As Object, e As EventArgs) Handles txtPassword.Enter
        UpdateCapsLockWarning()
    End Sub

    Private Sub txtPassword_KeyUp(sender As Object, e As KeyEventArgs) Handles txtPassword.KeyUp
        UpdateCapsLockWarning()
    End Sub

    Private Sub UpdateCapsLockWarning()
        ' Passwords are case-sensitive; warning about Caps Lock prevents needless lockouts.
        lblCapsLock.Visible = Control.IsKeyLocked(Keys.CapsLock)
    End Sub

End Class
