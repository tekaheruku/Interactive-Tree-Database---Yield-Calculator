Namespace Services

    ''' <summary>Rules a new administrator password must follow.</summary>
    Public NotInheritable Class PasswordPolicy

        ''' <summary>Minimum password length.</summary>
        Public Const MinLength As Integer = 8

        ''' <summary>Maximum password length (keeps hashing time bounded).</summary>
        Public Const MaxLength As Integer = 128

        ''' <summary>Text shown under the password box.</summary>
        Public Const Description As String = "At least 8 characters, with at least one letter and one number."

        Private Sub New()
            ' Only Shared members.
        End Sub

        ''' <summary>
        ''' Checks a proposed password. Returns Nothing when it is acceptable,
        ''' otherwise a message explaining what to change.
        ''' </summary>
        ''' <param name="password">The proposed password.</param>
        ''' <param name="username">The account name; the password may not equal it.</param>
        Public Shared Function Validate(password As String, username As String) As String
            If String.IsNullOrEmpty(password) Then
                Return "Enter a new password."
            End If

            If password.Length < MinLength Then
                Return String.Format("The password must be at least {0} characters long.", MinLength)
            End If

            If password.Length > MaxLength Then
                Return String.Format("The password must be {0} characters or fewer.", MaxLength)
            End If

            If Not password.Any(AddressOf Char.IsLetter) OrElse Not password.Any(AddressOf Char.IsDigit) Then
                Return "The password must contain at least one letter and one number."
            End If

            If Not String.IsNullOrEmpty(username) AndAlso String.Equals(password, username, StringComparison.OrdinalIgnoreCase) Then
                Return "The password must not be the same as the username."
            End If

            If String.Equals(password, AuthService.DefaultAdminPassword, StringComparison.Ordinal) Then
                Return "Choose a password other than the default password."
            End If

            Return Nothing
        End Function

    End Class

End Namespace
