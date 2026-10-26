Imports TreeYield.Core.Models
Imports TreeYield.Core.Services

''' <summary>
''' Lets the signed-in administrator choose a new password. In "forced" mode (first
''' login with the default password) the dialog explains why it appeared.
''' </summary>
Public Class ChangePasswordForm

    Private ReadOnly _auth As AuthService
    Private ReadOnly _user As AppUser

    ''' <summary>Creates the dialog.</summary>
    ''' <param name="auth">Authentication service.</param>
    ''' <param name="user">The signed-in user.</param>
    ''' <param name="forced">True when the password must be changed before continuing.</param>
    Public Sub New(auth As AuthService, user As AppUser, forced As Boolean)
        If auth Is Nothing Then
            Throw New ArgumentNullException(NameOf(auth))
        End If

        If user Is Nothing Then
            Throw New ArgumentNullException(NameOf(user))
        End If

        _auth = auth
        _user = user

        ' This call is required by the designer.
        InitializeComponent()

        lblPolicy.Text = PasswordPolicy.Description
        If forced Then
            lblIntro.Text = "For security, replace the default password before managing records. " &
                            "Type the current (default) password, then your new password twice."
            btnCancel.Text = "Sign Out"
        End If
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        lblError.Text = String.Empty
        SetBusy(True)
        Try
            Dim errorMessage As String = Await _auth.ChangePasswordAsync(_user, txtCurrent.Text, txtNew.Text, txtConfirm.Text)
            If errorMessage Is Nothing Then
                ErrorReporter.ShowInfo(Me, "Your password was changed. Use the new password the next time you sign in.")
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Return
            End If

            lblError.Text = errorMessage
            txtNew.Clear()
            txtConfirm.Clear()
            If errorMessage.StartsWith("The current", StringComparison.Ordinal) Then
                txtCurrent.SelectAll()
                txtCurrent.Focus()
            Else
                txtNew.Focus()
            End If
        Catch ex As Exception
            ErrorReporter.ShowError(Me, "change the password", ex)
        Finally
            SetBusy(False)
        End Try
    End Sub

    Private Sub SetBusy(busy As Boolean)
        txtCurrent.Enabled = Not busy
        txtNew.Enabled = Not busy
        txtConfirm.Enabled = Not busy
        btnSave.Enabled = Not busy
        UseWaitCursor = busy
    End Sub

End Class
