Imports System.Data.OleDb
Imports TreeYield.Core.Models

Namespace Data

    ''' <summary>
    ''' Reads and writes the Users table (administrator accounts). The hash and salt
    ''' are byte arrays in the program and Base64 text in the database, which keeps
    ''' the table readable in Microsoft Access.
    ''' </summary>
    Public Class UserRepository
        Inherits RepositoryBase

        ''' <summary>Creates the repository.</summary>
        Public Sub New(connectionFactory As AccessConnectionFactory)
            MyBase.New(connectionFactory)
        End Sub

        ''' <summary>Finds a user by name (Access compares text case-insensitively). Returns Nothing if not found.</summary>
        Public Function GetByUsernameAsync(username As String) As Task(Of AppUser)
            Const sql As String =
                "SELECT UserId, Username, PasswordHash, PasswordSalt, Iterations, MustChangePassword, " &
                "FailedLoginCount, LockoutUntil, LastLoginAt FROM Users WHERE Username = ?"

            Return QueryAsync("load the user account", Function(connection)
                Using command As OleDbCommand = CreateCommand(connection, sql)
                    AddText(command, username, 50)
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        If Not reader.Read() Then
                            Return Nothing
                        End If

                        Return New AppUser With {
                            .UserId = ReadInteger(reader, "UserId"),
                            .Username = ReadString(reader, "Username"),
                            .PasswordHash = Convert.FromBase64String(ReadString(reader, "PasswordHash")),
                            .PasswordSalt = Convert.FromBase64String(ReadString(reader, "PasswordSalt")),
                            .Iterations = ReadInteger(reader, "Iterations"),
                            .MustChangePassword = ReadBoolean(reader, "MustChangePassword"),
                            .FailedLoginCount = ReadInteger(reader, "FailedLoginCount"),
                            .LockoutUntilUtc = ReadNullableDate(reader, "LockoutUntil"),
                            .LastLoginAtUtc = ReadNullableDate(reader, "LastLoginAt")
                        }
                    End Using
                End Using
            End Function)
        End Function

        ''' <summary>Returns the number of user accounts.</summary>
        Public Function CountAsync() As Task(Of Integer)
            Return QueryAsync("count the user accounts", Function(connection)
                Using command As OleDbCommand = CreateCommand(connection, "SELECT COUNT(*) FROM Users")
                    Return Convert.ToInt32(command.ExecuteScalar())
                End Using
            End Function)
        End Function

        ''' <summary>Creates a user and returns the generated ID.</summary>
        Public Function CreateAsync(username As String, passwordHash As Byte(), passwordSalt As Byte(),
                                    iterations As Integer, mustChangePassword As Boolean) As Task(Of Integer)
            Const sql As String =
                "INSERT INTO Users (Username, PasswordHash, PasswordSalt, Iterations, MustChangePassword, " &
                "FailedLoginCount, CreatedAt) VALUES (?, ?, ?, ?, ?, 0, ?)"

            Return QueryAsync("create the user account", Function(connection)
                Using command As OleDbCommand = CreateCommand(connection, sql)
                    AddText(command, username, 50)
                    AddText(command, Convert.ToBase64String(passwordHash), 100)
                    AddText(command, Convert.ToBase64String(passwordSalt), 50)
                    AddInteger(command, iterations)
                    AddBoolean(command, mustChangePassword)
                    AddDate(command, UtcNow())
                    command.ExecuteNonQuery()
                End Using

                Return LastInsertedId(connection)
            End Function)
        End Function

        ''' <summary>
        ''' Adds one failed attempt and locks the account when the limit is reached.
        ''' The counter and the lock are changed by ONE UPDATE statement (inside a
        ''' transaction with the read-back), so two quick attempts cannot both read the
        ''' old count. Returns the lockout end time, or Nothing if the account is not locked.
        ''' </summary>
        ''' <param name="userId">The account.</param>
        ''' <param name="maxAttempts">Failures allowed before locking.</param>
        ''' <param name="lockoutMinutes">How long the lock lasts.</param>
        Public Function RecordFailedLoginAsync(userId As Integer, maxAttempts As Integer, lockoutMinutes As Integer) As Task(Of DateTime?)
            ' LockoutUntil is set first, from the count as it was BEFORE this failure,
            ' so the result is the same whichever order Access evaluates the SET list in.
            Const sql As String =
                "UPDATE Users SET LockoutUntil = IIf(FailedLoginCount + 1 >= ?, ?, Null), " &
                "FailedLoginCount = FailedLoginCount + 1 WHERE UserId = ?"

            Return QueryAsync("record the failed login", Function(connection)
                Using transaction As OleDbTransaction = connection.BeginTransaction()
                    Using update As OleDbCommand = CreateCommand(connection, sql, transaction)
                        AddInteger(update, maxAttempts)
                        AddDate(update, UtcNow().AddMinutes(lockoutMinutes))
                        AddInteger(update, userId)
                        update.ExecuteNonQuery()
                    End Using

                    Dim lockedUntil As DateTime?
                    Using read As OleDbCommand = CreateCommand(connection, "SELECT LockoutUntil FROM Users WHERE UserId = ?", transaction)
                        AddInteger(read, userId)
                        Dim value As Object = read.ExecuteScalar()
                        lockedUntil = If(value Is Nothing OrElse value Is DBNull.Value, CType(Nothing, DateTime?), Convert.ToDateTime(value))
                    End Using

                    transaction.Commit()
                    Return lockedUntil
                End Using
            End Function)
        End Function

        ''' <summary>Clears the failure counter and lock after a successful login, and stamps the login time.</summary>
        Public Function RecordSuccessfulLoginAsync(userId As Integer) As Task
            Return ExecuteAsync("record the login", Sub(connection)
                Using command As OleDbCommand = CreateCommand(connection,
                    "UPDATE Users SET FailedLoginCount = 0, LockoutUntil = Null, LastLoginAt = ? WHERE UserId = ?")
                    AddDate(command, UtcNow())
                    AddInteger(command, userId)
                    command.ExecuteNonQuery()
                End Using
            End Sub)
        End Function

        ''' <summary>Clears an expired lock so the user gets a fresh set of attempts.</summary>
        Public Function ClearLockoutAsync(userId As Integer) As Task
            Return ExecuteAsync("clear the account lock", Sub(connection)
                Using command As OleDbCommand = CreateCommand(connection,
                    "UPDATE Users SET FailedLoginCount = 0, LockoutUntil = Null WHERE UserId = ?")
                    AddInteger(command, userId)
                    command.ExecuteNonQuery()
                End Using
            End Sub)
        End Function

        ''' <summary>Stores a new password hash and clears the "must change password" flag.</summary>
        Public Function UpdatePasswordAsync(userId As Integer, passwordHash As Byte(), passwordSalt As Byte(), iterations As Integer) As Task
            Const sql As String =
                "UPDATE Users SET PasswordHash = ?, PasswordSalt = ?, Iterations = ?, MustChangePassword = False " &
                "WHERE UserId = ?"

            Return ExecuteAsync("change the password", Sub(connection)
                Using command As OleDbCommand = CreateCommand(connection, sql)
                    AddText(command, Convert.ToBase64String(passwordHash), 100)
                    AddText(command, Convert.ToBase64String(passwordSalt), 50)
                    AddInteger(command, iterations)
                    AddInteger(command, userId)
                    command.ExecuteNonQuery()
                End Using
            End Sub)
        End Function

    End Class

End Namespace
