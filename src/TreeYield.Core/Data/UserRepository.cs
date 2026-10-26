using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TreeYield.Core.Models;

namespace TreeYield.Core.Data
{
    /// <summary>Reads and writes the Users table (administrator accounts).</summary>
    public class UserRepository : RepositoryBase
    {
        /// <summary>Creates the repository.</summary>
        public UserRepository(SqlConnectionFactory connectionFactory)
            : base(connectionFactory)
        {
        }

        /// <summary>Finds a user by name (case-insensitive under the default collation). Returns null if not found.</summary>
        public Task<AppUser> GetByUsernameAsync(string username)
        {
            const string sql = @"
SELECT UserId, Username, PasswordHash, PasswordSalt, Iterations, MustChangePassword,
       FailedLoginCount, LockoutUntil, LastLoginAt
FROM dbo.Users
WHERE Username = @Username;";

            return WithConnectionAsync("load the user account", async connection =>
            {
                using (SqlCommand command = CreateCommand(connection, sql))
                {
                    AddParameter(command, "@Username", SqlDbType.NVarChar, 50, username);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        if (!await reader.ReadAsync().ConfigureAwait(false))
                        {
                            return null;
                        }

                        return new AppUser
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            Username = ReadString(reader, "Username"),
                            PasswordHash = (byte[])reader["PasswordHash"],
                            PasswordSalt = (byte[])reader["PasswordSalt"],
                            Iterations = reader.GetInt32(reader.GetOrdinal("Iterations")),
                            MustChangePassword = reader.GetBoolean(reader.GetOrdinal("MustChangePassword")),
                            FailedLoginCount = reader.GetInt32(reader.GetOrdinal("FailedLoginCount")),
                            LockoutUntilUtc = ReadNullableDateTime(reader, "LockoutUntil"),
                            LastLoginAtUtc = ReadNullableDateTime(reader, "LastLoginAt")
                        };
                    }
                }
            });
        }

        /// <summary>Returns the number of user accounts.</summary>
        public Task<int> CountAsync()
        {
            return WithConnectionAsync("count the user accounts", async connection =>
            {
                using (SqlCommand command = CreateCommand(connection, "SELECT COUNT(*) FROM dbo.Users;"))
                {
                    return Convert.ToInt32(await command.ExecuteScalarAsync().ConfigureAwait(false));
                }
            });
        }

        /// <summary>Creates a user and returns the generated ID.</summary>
        public Task<int> CreateAsync(string username, byte[] passwordHash, byte[] passwordSalt, int iterations, bool mustChangePassword)
        {
            const string sql = @"
INSERT INTO dbo.Users (Username, PasswordHash, PasswordSalt, Iterations, MustChangePassword)
OUTPUT INSERTED.UserId
VALUES (@Username, @PasswordHash, @PasswordSalt, @Iterations, @MustChangePassword);";

            return WithConnectionAsync("create the user account", async connection =>
            {
                using (SqlCommand command = CreateCommand(connection, sql))
                {
                    AddParameter(command, "@Username", SqlDbType.NVarChar, 50, username);
                    AddParameter(command, "@PasswordHash", SqlDbType.VarBinary, 64, passwordHash);
                    AddParameter(command, "@PasswordSalt", SqlDbType.VarBinary, 32, passwordSalt);
                    AddParameter(command, "@Iterations", SqlDbType.Int, iterations);
                    AddParameter(command, "@MustChangePassword", SqlDbType.Bit, mustChangePassword);
                    return Convert.ToInt32(await command.ExecuteScalarAsync().ConfigureAwait(false));
                }
            });
        }

        /// <summary>
        /// Adds one failed attempt and locks the account when the limit is reached.
        /// It is done in one UPDATE statement so two quick attempts cannot both read the
        /// old count (no race condition). Returns the lockout end time, or null if not locked.
        /// </summary>
        /// <param name="userId">The account.</param>
        /// <param name="maxAttempts">Failures allowed before locking.</param>
        /// <param name="lockoutMinutes">How long the lock lasts.</param>
        public Task<DateTime?> RecordFailedLoginAsync(int userId, int maxAttempts, int lockoutMinutes)
        {
            // In an UPDATE, every expression on the right of SET sees the values from
            // before the update, so "FailedLoginCount + 1" is the new count in both places.
            const string sql = @"
UPDATE dbo.Users
SET FailedLoginCount = FailedLoginCount + 1,
    LockoutUntil = CASE WHEN FailedLoginCount + 1 >= @MaxAttempts
                        THEN DATEADD(MINUTE, @LockoutMinutes, SYSUTCDATETIME())
                        ELSE NULL END
OUTPUT INSERTED.LockoutUntil
WHERE UserId = @UserId;";

            return WithConnectionAsync("record the failed login", async connection =>
            {
                using (SqlCommand command = CreateCommand(connection, sql))
                {
                    AddParameter(command, "@UserId", SqlDbType.Int, userId);
                    AddParameter(command, "@MaxAttempts", SqlDbType.Int, maxAttempts);
                    AddParameter(command, "@LockoutMinutes", SqlDbType.Int, lockoutMinutes);
                    object value = await command.ExecuteScalarAsync().ConfigureAwait(false);
                    return value == null || value == DBNull.Value ? (DateTime?)null : (DateTime)value;
                }
            });
        }

        /// <summary>Clears the failure counter and lock after a successful login, and stamps the login time.</summary>
        public Task RecordSuccessfulLoginAsync(int userId)
        {
            return WithConnectionAsync("record the login", async connection =>
            {
                using (SqlCommand command = CreateCommand(connection,
                    "UPDATE dbo.Users SET FailedLoginCount = 0, LockoutUntil = NULL, LastLoginAt = SYSUTCDATETIME() WHERE UserId = @UserId;"))
                {
                    AddParameter(command, "@UserId", SqlDbType.Int, userId);
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            });
        }

        /// <summary>Clears an expired lock so the user gets a fresh set of attempts.</summary>
        public Task ClearLockoutAsync(int userId)
        {
            return WithConnectionAsync("clear the account lock", async connection =>
            {
                using (SqlCommand command = CreateCommand(connection,
                    "UPDATE dbo.Users SET FailedLoginCount = 0, LockoutUntil = NULL WHERE UserId = @UserId;"))
                {
                    AddParameter(command, "@UserId", SqlDbType.Int, userId);
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            });
        }

        /// <summary>Stores a new password hash and clears the "must change password" flag.</summary>
        public Task UpdatePasswordAsync(int userId, byte[] passwordHash, byte[] passwordSalt, int iterations)
        {
            const string sql = @"
UPDATE dbo.Users
SET PasswordHash = @PasswordHash, PasswordSalt = @PasswordSalt,
    Iterations = @Iterations, MustChangePassword = 0
WHERE UserId = @UserId;";

            return WithConnectionAsync("change the password", async connection =>
            {
                using (SqlCommand command = CreateCommand(connection, sql))
                {
                    AddParameter(command, "@UserId", SqlDbType.Int, userId);
                    AddParameter(command, "@PasswordHash", SqlDbType.VarBinary, 64, passwordHash);
                    AddParameter(command, "@PasswordSalt", SqlDbType.VarBinary, 32, passwordSalt);
                    AddParameter(command, "@Iterations", SqlDbType.Int, iterations);
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            });
        }
    }
}
