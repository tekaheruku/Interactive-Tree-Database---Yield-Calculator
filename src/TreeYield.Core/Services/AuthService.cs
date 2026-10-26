using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TreeYield.Core.Data;
using TreeYield.Core.Models;

namespace TreeYield.Core.Services
{
    /// <summary>
    /// Administrator authentication: login with account lockout, password changes,
    /// and creation of the default account on first run.
    /// </summary>
    public class AuthService
    {
        /// <summary>Username of the account created on first run.</summary>
        public const string DefaultAdminUsername = "admin";

        /// <summary>
        /// Password of the account created on first run. The account is flagged
        /// "must change password", so this value only works for the very first login.
        /// </summary>
        public const string DefaultAdminPassword = "Admin@123";

        /// <summary>Failed attempts allowed before the account is locked.</summary>
        public const int MaxFailedAttempts = 5;

        /// <summary>How long a locked account stays locked.</summary>
        public const int LockoutMinutes = 5;

        // One message for "unknown user" and "wrong password", so an attacker cannot
        // find out which usernames exist.
        private const string InvalidCredentialsMessage = "Invalid username or password.";

        // Used to spend the same hashing time when the username does not exist,
        // so response time does not reveal whether a username is valid.
        private static readonly byte[] DummySalt = new byte[PasswordHasher.SaltSize];

        private readonly UserRepository _users;
        private readonly AuditLogRepository _audit;

        /// <summary>Creates the service.</summary>
        public AuthService(UserRepository users, AuditLogRepository audit)
        {
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }

            if (audit == null)
            {
                throw new ArgumentNullException(nameof(audit));
            }

            _users = users;
            _audit = audit;
        }

        /// <summary>
        /// Checks a username and password.
        /// <list type="number">
        /// <item>Empty input is rejected.</item>
        /// <item>An unknown user gets the generic message (after a dummy hash).</item>
        /// <item>A locked account is refused until the lock expires.</item>
        /// <item>A wrong password adds a failure; the 5th failure locks the account for 5 minutes.</item>
        /// <item>A correct password resets the counter.</item>
        /// </list>
        /// </summary>
        public async Task<LoginResult> LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
            {
                return LoginResult.Fail(LoginStatus.MissingInput, "Enter your username and password.");
            }

            string name = username.Trim();
            AppUser user = await _users.GetByUsernameAsync(name).ConfigureAwait(false);

            if (user == null)
            {
                await Task.Run(() => PasswordHasher.Hash(password, DummySalt, PasswordHasher.DefaultIterations))
                    .ConfigureAwait(false);
                await TryAuditAsync(null, AuditActions.LoginFailed, "Unknown username: " + Shorten(name, 50))
                    .ConfigureAwait(false);
                return LoginResult.Fail(LoginStatus.InvalidCredentials, InvalidCredentialsMessage);
            }

            if (user.LockoutUntilUtc.HasValue)
            {
                if (user.LockoutUntilUtc.Value > DateTime.UtcNow)
                {
                    return LoginResult.Fail(LoginStatus.LockedOut, LockedMessage(user.LockoutUntilUtc.Value));
                }

                // The lock has expired: start again with a fresh set of attempts.
                await _users.ClearLockoutAsync(user.UserId).ConfigureAwait(false);
            }

            // PBKDF2 is slow on purpose (about 0.1 s), so it runs on a background thread.
            bool passwordOk = await Task.Run(() =>
                PasswordHasher.Verify(password, user.PasswordSalt, user.Iterations, user.PasswordHash)).ConfigureAwait(false);

            if (!passwordOk)
            {
                DateTime? lockedUntil = await _users.RecordFailedLoginAsync(user.UserId, MaxFailedAttempts, LockoutMinutes)
                    .ConfigureAwait(false);

                if (lockedUntil.HasValue)
                {
                    await TryAuditAsync(user.UserId, AuditActions.Lockout,
                        string.Format("Account locked for {0} minutes after {1} failed attempts.", LockoutMinutes, MaxFailedAttempts))
                        .ConfigureAwait(false);
                    return LoginResult.Fail(LoginStatus.LockedOut, LockedMessage(lockedUntil.Value));
                }

                await TryAuditAsync(user.UserId, AuditActions.LoginFailed, "Wrong password.").ConfigureAwait(false);
                return LoginResult.Fail(LoginStatus.InvalidCredentials, InvalidCredentialsMessage);
            }

            await _users.RecordSuccessfulLoginAsync(user.UserId).ConfigureAwait(false);
            await TryAuditAsync(user.UserId, AuditActions.Login, "Signed in.").ConfigureAwait(false);

            user.FailedLoginCount = 0;
            user.LockoutUntilUtc = null;
            user.LastLoginAtUtc = DateTime.UtcNow;
            return LoginResult.Success(user);
        }

        /// <summary>
        /// Changes the password of a signed-in user. Returns null on success,
        /// otherwise a message explaining why the password was not changed.
        /// </summary>
        public async Task<string> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword, string confirmPassword)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            bool currentOk = await Task.Run(() =>
                PasswordHasher.Verify(currentPassword ?? string.Empty, user.PasswordSalt, user.Iterations, user.PasswordHash))
                .ConfigureAwait(false);

            if (!currentOk)
            {
                return "The current password is incorrect.";
            }

            if (!string.Equals(newPassword, confirmPassword, StringComparison.Ordinal))
            {
                return "The new password and the confirmation do not match.";
            }

            if (string.Equals(newPassword, currentPassword, StringComparison.Ordinal))
            {
                return "The new password must be different from the current password.";
            }

            string policyError = PasswordPolicy.Validate(newPassword, user.Username);
            if (policyError != null)
            {
                return policyError;
            }

            byte[] salt = PasswordHasher.GenerateSalt();
            byte[] hash = await Task.Run(() => PasswordHasher.Hash(newPassword, salt, PasswordHasher.DefaultIterations))
                .ConfigureAwait(false);

            await _users.UpdatePasswordAsync(user.UserId, hash, salt, PasswordHasher.DefaultIterations).ConfigureAwait(false);
            await TryAuditAsync(user.UserId, AuditActions.PasswordChange, "Password changed.").ConfigureAwait(false);

            user.PasswordHash = hash;
            user.PasswordSalt = salt;
            user.Iterations = PasswordHasher.DefaultIterations;
            user.MustChangePassword = false;
            return null;
        }

        /// <summary>Writes a LOGOUT entry to the audit log.</summary>
        public Task LogoutAsync(AppUser user)
        {
            return user == null ? Task.FromResult(0) : TryAuditAsync(user.UserId, AuditActions.Logout, "Signed out.");
        }

        /// <summary>
        /// Creates the default administrator account if the Users table is empty.
        /// The password is hashed here because PBKDF2 cannot be computed in T-SQL.
        /// </summary>
        public async Task EnsureDefaultAdminAsync()
        {
            int userCount = await _users.CountAsync().ConfigureAwait(false);
            if (userCount > 0)
            {
                return;
            }

            byte[] salt = PasswordHasher.GenerateSalt();
            byte[] hash = await Task.Run(() => PasswordHasher.Hash(DefaultAdminPassword, salt, PasswordHasher.DefaultIterations))
                .ConfigureAwait(false);

            await _users.CreateAsync(DefaultAdminUsername, hash, salt, PasswordHasher.DefaultIterations, true)
                .ConfigureAwait(false);
        }

        private static string LockedMessage(DateTime lockedUntilUtc)
        {
            int minutes = (int)Math.Ceiling((lockedUntilUtc - DateTime.UtcNow).TotalMinutes);
            if (minutes < 1)
            {
                minutes = 1;
            }

            return string.Format(
                "This account is locked because of too many failed sign-in attempts. Try again in {0} minute{1}.",
                minutes, minutes == 1 ? string.Empty : "s");
        }

        private static string Shorten(string text, int maxLength)
        {
            return text.Length <= maxLength ? text : text.Substring(0, maxLength) + "...";
        }

        /// <summary>
        /// Audit logging is "best effort": if writing the log fails, the login itself
        /// should still work, so the error is only traced instead of shown to the user.
        /// </summary>
        private async Task TryAuditAsync(int? userId, string action, string details)
        {
            try
            {
                await _audit.AddAsync(userId, action, "User", userId, details).ConfigureAwait(false);
            }
            catch (DataAccessException ex)
            {
                Trace.TraceWarning("Audit log write failed: " + ex);
            }
        }
    }
}
