using System;

namespace TreeYield.Core.Models
{
    /// <summary>
    /// An administrator account (row of the Users table). The password itself is
    /// never stored; only a salted PBKDF2 hash.
    /// </summary>
    public class AppUser
    {
        /// <summary>Primary key.</summary>
        public int UserId { get; set; }

        /// <summary>Login name. Unique.</summary>
        public string Username { get; set; }

        /// <summary>PBKDF2 hash of the password.</summary>
        public byte[] PasswordHash { get; set; }

        /// <summary>Random salt that was mixed into <see cref="PasswordHash"/>.</summary>
        public byte[] PasswordSalt { get; set; }

        /// <summary>PBKDF2 iteration count used for this hash.</summary>
        public int Iterations { get; set; }

        /// <summary>True when the user must choose a new password at the next login.</summary>
        public bool MustChangePassword { get; set; }

        /// <summary>Consecutive failed logins since the last successful one.</summary>
        public int FailedLoginCount { get; set; }

        /// <summary>If set and in the future, the account is locked until this time (UTC).</summary>
        public DateTime? LockoutUntilUtc { get; set; }

        /// <summary>Time of the last successful login (UTC).</summary>
        public DateTime? LastLoginAtUtc { get; set; }
    }
}
