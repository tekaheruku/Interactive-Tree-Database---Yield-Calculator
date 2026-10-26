using System;
using System.Linq;

namespace TreeYield.Core.Services
{
    /// <summary>Rules a new administrator password must follow.</summary>
    public static class PasswordPolicy
    {
        /// <summary>Minimum password length.</summary>
        public const int MinLength = 8;

        /// <summary>Maximum password length (keeps hashing time bounded).</summary>
        public const int MaxLength = 128;

        /// <summary>Text shown under the password box.</summary>
        public const string Description = "At least 8 characters, with at least one letter and one number.";

        /// <summary>
        /// Checks a proposed password. Returns null when it is acceptable,
        /// otherwise a message explaining what to change.
        /// </summary>
        /// <param name="password">The proposed password.</param>
        /// <param name="username">The account name; the password may not equal it.</param>
        public static string Validate(string password, string username)
        {
            if (string.IsNullOrEmpty(password))
            {
                return "Enter a new password.";
            }

            if (password.Length < MinLength)
            {
                return string.Format("The password must be at least {0} characters long.", MinLength);
            }

            if (password.Length > MaxLength)
            {
                return string.Format("The password must be {0} characters or fewer.", MaxLength);
            }

            if (!password.Any(char.IsLetter) || !password.Any(char.IsDigit))
            {
                return "The password must contain at least one letter and one number.";
            }

            if (!string.IsNullOrEmpty(username) && string.Equals(password, username, StringComparison.OrdinalIgnoreCase))
            {
                return "The password must not be the same as the username.";
            }

            if (string.Equals(password, AuthService.DefaultAdminPassword, StringComparison.Ordinal))
            {
                return "Choose a password other than the default password.";
            }

            return null;
        }
    }
}
