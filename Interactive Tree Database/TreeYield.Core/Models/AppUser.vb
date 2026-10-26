Namespace Models

    ''' <summary>
    ''' An administrator account (row of the Users table). The password itself is
    ''' never stored; only a salted PBKDF2 hash.
    ''' </summary>
    Public Class AppUser

        ''' <summary>Primary key.</summary>
        Public Property UserId As Integer

        ''' <summary>Login name. Unique.</summary>
        Public Property Username As String

        ''' <summary>PBKDF2 hash of the password.</summary>
        Public Property PasswordHash As Byte()

        ''' <summary>Random salt that was mixed into <see cref="PasswordHash"/>.</summary>
        Public Property PasswordSalt As Byte()

        ''' <summary>PBKDF2 iteration count used for this hash.</summary>
        Public Property Iterations As Integer

        ''' <summary>True when the user must choose a new password at the next login.</summary>
        Public Property MustChangePassword As Boolean

        ''' <summary>Consecutive failed logins since the last successful one.</summary>
        Public Property FailedLoginCount As Integer

        ''' <summary>If set and in the future, the account is locked until this time (UTC).</summary>
        Public Property LockoutUntilUtc As DateTime?

        ''' <summary>Time of the last successful login (UTC).</summary>
        Public Property LastLoginAtUtc As DateTime?

    End Class

End Namespace
