Imports TreeYield.Core.Models

''' <summary>
''' Remembers which administrator is signed in. The session ends automatically after
''' <see cref="IdleTimeout"/> without admin activity, so an unattended PC does not
''' stay open for editing.
''' </summary>
Friend NotInheritable Class AdminSession

    ''' <summary>How long an unused session stays valid.</summary>
    Public Shared ReadOnly IdleTimeout As TimeSpan = TimeSpan.FromMinutes(15)

    Private _currentUser As AppUser
    Private _lastActivityUtc As DateTime

    ''' <summary>The signed-in administrator, or Nothing for a guest.</summary>
    Public ReadOnly Property CurrentUser As AppUser
        Get
            Return _currentUser
        End Get
    End Property

    ''' <summary>True when an administrator is signed in and the session has not expired.</summary>
    Public ReadOnly Property IsSignedIn As Boolean
        Get
            Return CurrentUser IsNot Nothing AndAlso Not IsExpired
        End Get
    End Property

    ''' <summary>True when someone signed in but has been idle longer than <see cref="IdleTimeout"/>.</summary>
    Public ReadOnly Property IsExpired As Boolean
        Get
            Return CurrentUser IsNot Nothing AndAlso DateTime.UtcNow - _lastActivityUtc > IdleTimeout
        End Get
    End Property

    ''' <summary>Starts a session.</summary>
    Public Sub SignIn(user As AppUser)
        If user Is Nothing Then
            Throw New ArgumentNullException(NameOf(user))
        End If

        _currentUser = user
        Touch()
    End Sub

    ''' <summary>Records admin activity, restarting the idle timer.</summary>
    Public Sub Touch()
        _lastActivityUtc = DateTime.UtcNow
    End Sub

    ''' <summary>Ends the session.</summary>
    Public Sub SignOut()
        _currentUser = Nothing
    End Sub

End Class
