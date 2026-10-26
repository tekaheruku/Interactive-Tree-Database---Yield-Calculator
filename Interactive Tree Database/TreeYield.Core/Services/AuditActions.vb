Namespace Services

    ''' <summary>Action codes written to the AuditLog table.</summary>
    Public NotInheritable Class AuditActions

        ''' <summary>Successful administrator login.</summary>
        Public Const Login As String = "LOGIN"

        ''' <summary>Failed login (wrong password or unknown username).</summary>
        Public Const LoginFailed As String = "LOGIN_FAILED"

        ''' <summary>Account locked after too many failed logins.</summary>
        Public Const Lockout As String = "LOCKOUT"

        ''' <summary>Administrator signed out.</summary>
        Public Const Logout As String = "LOGOUT"

        ''' <summary>Password changed.</summary>
        Public Const PasswordChange As String = "PASSWORD_CHANGE"

        ''' <summary>A record was added.</summary>
        Public Const Create As String = "CREATE"

        ''' <summary>A record was edited.</summary>
        Public Const Update As String = "UPDATE"

        ''' <summary>A record was deleted.</summary>
        Public Const Delete As String = "DELETE"

        ''' <summary>The sample species data was restored.</summary>
        Public Const ResetData As String = "RESET_DATA"

        Private Sub New()
            ' Only constants.
        End Sub

    End Class

End Namespace
