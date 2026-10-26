Imports System.Security.Cryptography

Namespace Services

    ''' <summary>
    ''' Hashes and verifies passwords with PBKDF2 (RFC 2898). Passwords are never stored;
    ''' only a random salt and the derived hash. Many iterations make each guess slow,
    ''' which protects the passwords even if the database file is copied.
    ''' </summary>
    Public NotInheritable Class PasswordHasher

        ''' <summary>Salt length in bytes (128 bits).</summary>
        Public Const SaltSize As Integer = 16

        ''' <summary>
        ''' Hash length in bytes. 20 bytes is the output size of HMAC-SHA1, the function
        ''' .NET Framework 4.5.2 uses inside PBKDF2, so one block is computed per iteration.
        ''' </summary>
        Public Const HashSize As Integer = 20

        ''' <summary>Iteration count for new hashes. Stored per user so it can be raised later.</summary>
        Public Const DefaultIterations As Integer = 100000

        Private Sub New()
            ' Only Shared members.
        End Sub

        ''' <summary>Creates a new cryptographically random salt.</summary>
        Public Shared Function GenerateSalt() As Byte()
            Dim salt(SaltSize - 1) As Byte
            Using random As New RNGCryptoServiceProvider()
                random.GetBytes(salt)
            End Using

            Return salt
        End Function

        ''' <summary>Derives the PBKDF2 hash of a password.</summary>
        ''' <param name="password">The plain-text password.</param>
        ''' <param name="salt">The salt (at least 8 bytes).</param>
        ''' <param name="iterations">Number of PBKDF2 iterations.</param>
        Public Shared Function Hash(password As String, salt As Byte(), iterations As Integer) As Byte()
            If password Is Nothing Then
                Throw New ArgumentNullException(NameOf(password))
            End If

            If salt Is Nothing OrElse salt.Length < 8 Then
                Throw New ArgumentException("The salt must be at least 8 bytes long.", NameOf(salt))
            End If

            If iterations < 1 Then
                Throw New ArgumentOutOfRangeException(NameOf(iterations))
            End If

            Using pbkdf2 As New Rfc2898DeriveBytes(password, salt, iterations)
                Return pbkdf2.GetBytes(HashSize)
            End Using
        End Function

        ''' <summary>Returns True when the password produces the expected hash.</summary>
        Public Shared Function Verify(password As String, salt As Byte(), iterations As Integer, expectedHash As Byte()) As Boolean
            If password Is Nothing OrElse salt Is Nothing OrElse expectedHash Is Nothing Then
                Return False
            End If

            Dim actualHash As Byte() = Hash(password, salt, iterations)
            Return FixedTimeEquals(actualHash, expectedHash)
        End Function

        ''' <summary>
        ''' Compares two byte arrays in constant time. A normal comparison stops at the first
        ''' different byte, and an attacker could measure that timing difference.
        ''' </summary>
        Public Shared Function FixedTimeEquals(left As Byte(), right As Byte()) As Boolean
            If left Is Nothing OrElse right Is Nothing OrElse left.Length <> right.Length Then
                Return False
            End If

            Dim difference As Integer = 0
            For i As Integer = 0 To left.Length - 1
                difference = difference Or (CInt(left(i)) Xor CInt(right(i)))
            Next

            Return difference = 0
        End Function

    End Class

End Namespace
