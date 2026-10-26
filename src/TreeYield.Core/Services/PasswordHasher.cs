using System;
using System.Security.Cryptography;

namespace TreeYield.Core.Services
{
    /// <summary>
    /// Hashes and verifies passwords with PBKDF2 (RFC 2898). Passwords are never stored;
    /// only a random salt and the derived hash. Many iterations make each guess slow,
    /// which protects the passwords even if the database file is stolen.
    /// </summary>
    public static class PasswordHasher
    {
        /// <summary>Salt length in bytes (128 bits).</summary>
        public const int SaltSize = 16;

        /// <summary>
        /// Hash length in bytes. 20 bytes is the output size of HMAC-SHA1, the function
        /// .NET Framework 4.5.2 uses inside PBKDF2, so one block is computed per iteration.
        /// </summary>
        public const int HashSize = 20;

        /// <summary>Iteration count for new hashes. Stored per user so it can be raised later.</summary>
        public const int DefaultIterations = 100000;

        /// <summary>Creates a new cryptographically random salt.</summary>
        public static byte[] GenerateSalt()
        {
            var salt = new byte[SaltSize];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetBytes(salt);
            }

            return salt;
        }

        /// <summary>Derives the PBKDF2 hash of a password.</summary>
        /// <param name="password">The plain-text password.</param>
        /// <param name="salt">The salt (at least 8 bytes).</param>
        /// <param name="iterations">Number of PBKDF2 iterations.</param>
        public static byte[] Hash(string password, byte[] salt, int iterations)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (salt == null || salt.Length < 8)
            {
                throw new ArgumentException("The salt must be at least 8 bytes long.", nameof(salt));
            }

            if (iterations < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(iterations));
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return pbkdf2.GetBytes(HashSize);
            }
        }

        /// <summary>Returns true when the password produces the expected hash.</summary>
        public static bool Verify(string password, byte[] salt, int iterations, byte[] expectedHash)
        {
            if (password == null || salt == null || expectedHash == null)
            {
                return false;
            }

            byte[] actualHash = Hash(password, salt, iterations);
            return FixedTimeEquals(actualHash, expectedHash);
        }

        /// <summary>
        /// Compares two byte arrays in constant time. A normal comparison stops at the first
        /// different byte, and an attacker could measure that timing difference.
        /// </summary>
        public static bool FixedTimeEquals(byte[] left, byte[] right)
        {
            if (left == null || right == null || left.Length != right.Length)
            {
                return false;
            }

            int difference = 0;
            for (int i = 0; i < left.Length; i++)
            {
                difference |= left[i] ^ right[i];
            }

            return difference == 0;
        }
    }
}
