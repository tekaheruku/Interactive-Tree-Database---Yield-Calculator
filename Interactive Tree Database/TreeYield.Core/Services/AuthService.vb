Imports TreeYield.Core.Data
Imports TreeYield.Core.Models

Namespace Services

    ''' <summary>
    ''' Administrator authentication: login with account lockout, password changes,
    ''' and creation of the default account on first run.
    ''' </summary>
    Public Class AuthService

        ''' <summary>Username of the account created on first run.</summary>
        Public Const DefaultAdminUsername As String = "admin"

        ''' <summary>
        ''' Password of the account created on first run. The account is flagged
        ''' "must change password", so this value only works for the very first login.
        ''' </summary>
        Public Const DefaultAdminPassword As String = "Admin@123"

        ''' <summary>Failed attempts allowed before the account is locked.</summary>
        Public Const MaxFailedAttempts As Integer = 5

        ''' <summary>How long a locked account stays locked.</summary>
        Public Const LockoutMinutes As Integer = 5

        ' One message for "unknown user" and "wrong password", so an attacker cannot
        ' find out which usernames exist.
        Private Const InvalidCredentialsMessage As String = "Invalid username or password."

        ' Used to spend the same hashing time when the username does not exist,
        ' so response time does not reveal whether a username is valid.
        Private Shared ReadOnly DummySalt(PasswordHasher.SaltSize - 1) As Byte

        Private ReadOnly _users As UserRepository
        Private ReadOnly _audit As AuditLogRepository

        ''' <summary>Creates the service.</summary>
        Public Sub New(users As UserRepository, audit As AuditLogRepository)
            If users Is Nothing Then
                Throw New ArgumentNullException(NameOf(users))
            End If

            If audit Is Nothing Then
                Throw New ArgumentNullException(NameOf(audit))
            End If

            _users = users
            _audit = audit
        End Sub

        ''' <summary>
        ''' Checks a username and password.
        ''' <list type="number">
        ''' <item>Empty input is rejected.</item>
        ''' <item>An unknown user gets the generic message (after a dummy hash).</item>
        ''' <item>A locked account is refused until the lock expires.</item>
        ''' <item>A wrong password adds a failure; the 5th failure locks the account for 5 minutes.</item>
        ''' <item>A correct password resets the counter.</item>
        ''' </list>
        ''' </summary>
        Public Async Function LoginAsync(username As String, password As String) As Task(Of LoginResult)
            If String.IsNullOrWhiteSpace(username) OrElse String.IsNullOrEmpty(password) Then
                Return LoginResult.Fail(LoginStatus.MissingInput, "Enter your username and password.")
            End If

            Dim name As String = username.Trim()
            Dim user As AppUser = Await _users.GetByUsernameAsync(name).ConfigureAwait(False)

            If user Is Nothing Then
                Await Task.Run(Function() PasswordHasher.Hash(password, DummySalt, PasswordHasher.DefaultIterations)).ConfigureAwait(False)
                Await TryAuditAsync(Nothing, AuditActions.LoginFailed, "Unknown username: " & Shorten(name, 50)).ConfigureAwait(False)
                Return LoginResult.Fail(LoginStatus.InvalidCredentials, InvalidCredentialsMessage)
            End If

            If user.LockoutUntilUtc.HasValue Then
                If user.LockoutUntilUtc.Value > DateTime.UtcNow Then
                    Return LoginResult.Fail(LoginStatus.LockedOut, LockedMessage(user.LockoutUntilUtc.Value))
                End If

                ' The lock has expired: start again with a fresh set of attempts.
                Await _users.ClearLockoutAsync(user.UserId).ConfigureAwait(False)
            End If

            ' PBKDF2 is slow on purpose (about 0.1 s), so it runs on a background thread.
            Dim passwordOk As Boolean = Await Task.Run(Function() PasswordHasher.Verify(password, user.PasswordSalt, user.Iterations, user.PasswordHash)).ConfigureAwait(False)

            If Not passwordOk Then
                Dim lockedUntil As DateTime? = Await _users.RecordFailedLoginAsync(user.UserId, MaxFailedAttempts, LockoutMinutes).ConfigureAwait(False)

                If lockedUntil.HasValue Then
                    Await TryAuditAsync(user.UserId, AuditActions.Lockout,
                        String.Format("Account locked for {0} minutes after {1} failed attempts.", LockoutMinutes, MaxFailedAttempts)).ConfigureAwait(False)
                    Return LoginResult.Fail(LoginStatus.LockedOut, LockedMessage(lockedUntil.Value))
                End If

                Await TryAuditAsync(user.UserId, AuditActions.LoginFailed, "Wrong password.").ConfigureAwait(False)
                Return LoginResult.Fail(LoginStatus.InvalidCredentials, InvalidCredentialsMessage)
            End If

            Await _users.RecordSuccessfulLoginAsync(user.UserId).ConfigureAwait(False)
            Await TryAuditAsync(user.UserId, AuditActions.Login, "Signed in.").ConfigureAwait(False)

            user.FailedLoginCount = 0
            user.LockoutUntilUtc = Nothing
            user.LastLoginAtUtc = DateTime.UtcNow
            Return LoginResult.Success(user)
        End Function

        ''' <summary>
        ''' Changes the password of a signed-in user. Returns Nothing on success,
        ''' otherwise a message explaining why the password was not changed.
        ''' </summary>
        Public Async Function ChangePasswordAsync(user As AppUser, currentPassword As String, newPassword As String, confirmPassword As String) As Task(Of String)
            If user Is Nothing Then
                Throw New ArgumentNullException(NameOf(user))
            End If

            Dim currentOk As Boolean = Await Task.Run(Function() PasswordHasher.Verify(If(currentPassword, String.Empty), user.PasswordSalt, user.Iterations, user.PasswordHash)).ConfigureAwait(False)

            If Not currentOk Then
                Return "The current password is incorrect."
            End If

            If Not String.Equals(newPassword, confirmPassword, StringComparison.Ordinal) Then
                Return "The new password and the confirmation do not match."
            End If

            If String.Equals(newPassword, currentPassword, StringComparison.Ordinal) Then
                Return "The new password must be different from the current password."
            End If

            Dim policyError As String = PasswordPolicy.Validate(newPassword, user.Username)
            If policyError IsNot Nothing Then
                Return policyError
            End If

            Dim salt As Byte() = PasswordHasher.GenerateSalt()
            Dim hash As Byte() = Await Task.Run(Function() PasswordHasher.Hash(newPassword, salt, PasswordHasher.DefaultIterations)).ConfigureAwait(False)

            Await _users.UpdatePasswordAsync(user.UserId, hash, salt, PasswordHasher.DefaultIterations).ConfigureAwait(False)
            Await TryAuditAsync(user.UserId, AuditActions.PasswordChange, "Password changed.").ConfigureAwait(False)

            user.PasswordHash = hash
            user.PasswordSalt = salt
            user.Iterations = PasswordHasher.DefaultIterations
            user.MustChangePassword = False
            Return Nothing
        End Function

        ''' <summary>Writes a LOGOUT entry to the audit log.</summary>
        Public Function LogoutAsync(user As AppUser) As Task
            If user Is Nothing Then
                Return Task.FromResult(0)
            End If

            Return TryAuditAsync(user.UserId, AuditActions.Logout, "Signed out.")
        End Function

        ''' <summary>
        ''' Creates the default administrator account if the Users table is empty.
        ''' The password is hashed in code because PBKDF2 cannot be computed in SQL.
        ''' </summary>
        Public Async Function EnsureDefaultAdminAsync() As Task
            Dim userCount As Integer = Await _users.CountAsync().ConfigureAwait(False)
            If userCount > 0 Then
                Return
            End If

            Dim salt As Byte() = PasswordHasher.GenerateSalt()
            Dim hash As Byte() = Await Task.Run(Function() PasswordHasher.Hash(DefaultAdminPassword, salt, PasswordHasher.DefaultIterations)).ConfigureAwait(False)

            Await _users.CreateAsync(DefaultAdminUsername, hash, salt, PasswordHasher.DefaultIterations, True).ConfigureAwait(False)
        End Function

        Private Shared Function LockedMessage(lockedUntilUtc As DateTime) As String
            Dim minutes As Integer = CInt(Math.Ceiling((lockedUntilUtc - DateTime.UtcNow).TotalMinutes))
            If minutes < 1 Then
                minutes = 1
            End If

            Return String.Format(
                "This account is locked because of too many failed sign-in attempts. Try again in {0} minute{1}.",
                minutes, If(minutes = 1, String.Empty, "s"))
        End Function

        Private Shared Function Shorten(text As String, maxLength As Integer) As String
            Return If(text.Length <= maxLength, text, text.Substring(0, maxLength) & "...")
        End Function

        ''' <summary>
        ''' Audit logging is "best effort": if writing the log fails, the login itself
        ''' should still work, so the error is only traced instead of shown to the user.
        ''' </summary>
        Private Async Function TryAuditAsync(userId As Integer?, action As String, details As String) As Task
            Try
                Await _audit.AddAsync(userId, action, "User", userId, details).ConfigureAwait(False)
            Catch ex As DataAccessException
                Trace.TraceWarning("Audit log write failed: " & ex.ToString())
            End Try
        End Function

    End Class

End Namespace
