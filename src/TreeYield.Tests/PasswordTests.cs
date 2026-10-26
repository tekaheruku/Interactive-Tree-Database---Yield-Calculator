using TreeYield.Core.Services;
using TreeYield.Tests.Framework;

namespace TreeYield.Tests
{
    /// <summary>Tests for <see cref="PasswordHasher"/> and <see cref="PasswordPolicy"/>.</summary>
    public class PasswordTests
    {
        // A low iteration count keeps the unit tests fast; the algorithm is the same.
        private const int TestIterations = 1000;

        [Test]
        public void Hash_ThenVerify_Succeeds()
        {
            byte[] salt = PasswordHasher.GenerateSalt();
            byte[] hash = PasswordHasher.Hash("Mango2026", salt, TestIterations);

            Assert.IsTrue(PasswordHasher.Verify("Mango2026", salt, TestIterations, hash));
        }

        [Test]
        public void Verify_WrongPassword_Fails()
        {
            byte[] salt = PasswordHasher.GenerateSalt();
            byte[] hash = PasswordHasher.Hash("Mango2026", salt, TestIterations);

            Assert.IsFalse(PasswordHasher.Verify("mango2026", salt, TestIterations, hash), "passwords are case-sensitive");
            Assert.IsFalse(PasswordHasher.Verify("", salt, TestIterations, hash));
        }

        [Test]
        public void SamePassword_DifferentSalt_GivesDifferentHash()
        {
            byte[] first = PasswordHasher.Hash("Mango2026", PasswordHasher.GenerateSalt(), TestIterations);
            byte[] second = PasswordHasher.Hash("Mango2026", PasswordHasher.GenerateSalt(), TestIterations);

            Assert.IsFalse(PasswordHasher.FixedTimeEquals(first, second));
        }

        [Test]
        public void Hash_HasExpectedSizes()
        {
            byte[] salt = PasswordHasher.GenerateSalt();
            Assert.AreEqual(PasswordHasher.SaltSize, salt.Length);
            Assert.AreEqual(PasswordHasher.HashSize, PasswordHasher.Hash("x", salt, TestIterations).Length);
        }

        [Test]
        public void FixedTimeEquals_HandlesDifferentLengthsAndNull()
        {
            Assert.IsFalse(PasswordHasher.FixedTimeEquals(new byte[] { 1, 2 }, new byte[] { 1, 2, 3 }));
            Assert.IsFalse(PasswordHasher.FixedTimeEquals(null, new byte[] { 1 }));
            Assert.IsTrue(PasswordHasher.FixedTimeEquals(new byte[] { 1, 2, 3 }, new byte[] { 1, 2, 3 }));
        }

        [Test]
        public void Policy_RejectsShortPassword()
        {
            Assert.IsNotNull(PasswordPolicy.Validate("Ab1", "admin"));
        }

        [Test]
        public void Policy_RejectsPasswordWithoutDigit()
        {
            Assert.IsNotNull(PasswordPolicy.Validate("OnlyLetters", "admin"));
        }

        [Test]
        public void Policy_RejectsPasswordWithoutLetter()
        {
            Assert.IsNotNull(PasswordPolicy.Validate("12345678", "admin"));
        }

        [Test]
        public void Policy_RejectsUsernameAsPassword()
        {
            Assert.IsNotNull(PasswordPolicy.Validate("Treeadmin1", "treeadmin1"));
        }

        [Test]
        public void Policy_RejectsDefaultPassword()
        {
            Assert.IsNotNull(PasswordPolicy.Validate(AuthService.DefaultAdminPassword, "admin"));
        }

        [Test]
        public void Policy_AcceptsGoodPassword()
        {
            Assert.IsNull(PasswordPolicy.Validate("Narra2026", "admin"));
        }
    }
}
