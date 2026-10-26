Namespace Models

    ''' <summary>Outcome of a login attempt.</summary>
    Public Enum LoginStatus

        ''' <summary>Username and password were correct.</summary>
        Success

        ''' <summary>The username or password box was empty.</summary>
        MissingInput

        ''' <summary>Unknown username or wrong password (deliberately not told apart).</summary>
        InvalidCredentials

        ''' <summary>Too many failed attempts; the account is temporarily locked.</summary>
        LockedOut

    End Enum

    ''' <summary>Result returned by <see cref="Services.AuthService.LoginAsync"/>.</summary>
    Public Class LoginResult

        Private Sub New(status As LoginStatus, message As String, user As AppUser)
            Me.Status = status
            Me.Message = message
            Me.User = user
        End Sub

        ''' <summary>What happened.</summary>
        Public ReadOnly Property Status As LoginStatus

        ''' <summary>Message to show the user. Nothing on success.</summary>
        Public ReadOnly Property Message As String

        ''' <summary>The signed-in user. Only set when <see cref="Status"/> is Success.</summary>
        Public ReadOnly Property User As AppUser

        ''' <summary>True when the login succeeded.</summary>
        Public ReadOnly Property Succeeded As Boolean
            Get
                Return Status = LoginStatus.Success
            End Get
        End Property

        ''' <summary>Creates a successful result.</summary>
        Public Shared Function Success(user As AppUser) As LoginResult
            Return New LoginResult(LoginStatus.Success, Nothing, user)
        End Function

        ''' <summary>Creates a failed result with a message for the user.</summary>
        Public Shared Function Fail(status As LoginStatus, message As String) As LoginResult
            Return New LoginResult(status, message, Nothing)
        End Function

    End Class

End Namespace
