Imports TreeYield.Core.Services
Imports TreeYield.Tests.Framework

''' <summary>Tests for <see cref="PasswordHasher"/> and <see cref="PasswordPolicy"/>.</summary>
Public Class PasswordTests

    ' A low iteration count keeps the unit tests fast; the algorithm is the same.
    Private Const TestIterations As Integer = 1000

    <Test>
    Public Sub Hash_ThenVerify_Succeeds()
        Dim salt As Byte() = PasswordHasher.GenerateSalt()
        Dim hash As Byte() = PasswordHasher.Hash("Mango2026", salt, TestIterations)

        Assert.IsTrue(PasswordHasher.Verify("Mango2026", salt, TestIterations, hash))
    End Sub

    <Test>
    Public Sub Verify_WrongPassword_Fails()
        Dim salt As Byte() = PasswordHasher.GenerateSalt()
        Dim hash As Byte() = PasswordHasher.Hash("Mango2026", salt, TestIterations)

        Assert.IsFalse(PasswordHasher.Verify("mango2026", salt, TestIterations, hash), "passwords are case-sensitive")
        Assert.IsFalse(PasswordHasher.Verify("", salt, TestIterations, hash))
    End Sub

    <Test>
    Public Sub SamePassword_DifferentSalt_GivesDifferentHash()
        Dim first As Byte() = PasswordHasher.Hash("Mango2026", PasswordHasher.GenerateSalt(), TestIterations)
        Dim second As Byte() = PasswordHasher.Hash("Mango2026", PasswordHasher.GenerateSalt(), TestIterations)

        Assert.IsFalse(PasswordHasher.FixedTimeEquals(first, second))
    End Sub

    <Test>
    Public Sub Hash_HasExpectedSizes()
        Dim salt As Byte() = PasswordHasher.GenerateSalt()
        Assert.AreEqual(PasswordHasher.SaltSize, salt.Length)
        Assert.AreEqual(PasswordHasher.HashSize, PasswordHasher.Hash("x", salt, TestIterations).Length)
    End Sub

    <Test>
    Public Sub FixedTimeEquals_HandlesDifferentLengthsAndNothing()
        Assert.IsFalse(PasswordHasher.FixedTimeEquals(New Byte() {1, 2}, New Byte() {1, 2, 3}))
        Assert.IsFalse(PasswordHasher.FixedTimeEquals(Nothing, New Byte() {1}))
        Assert.IsTrue(PasswordHasher.FixedTimeEquals(New Byte() {1, 2, 3}, New Byte() {1, 2, 3}))
    End Sub

    <Test>
    Public Sub Policy_RejectsShortPassword()
        Assert.IsNotNull(PasswordPolicy.Validate("Ab1", "admin"))
    End Sub

    <Test>
    Public Sub Policy_RejectsPasswordWithoutDigit()
        Assert.IsNotNull(PasswordPolicy.Validate("OnlyLetters", "admin"))
    End Sub

    <Test>
    Public Sub Policy_RejectsPasswordWithoutLetter()
        Assert.IsNotNull(PasswordPolicy.Validate("12345678", "admin"))
    End Sub

    <Test>
    Public Sub Policy_RejectsUsernameAsPassword()
        Assert.IsNotNull(PasswordPolicy.Validate("Treeadmin1", "treeadmin1"))
    End Sub

    <Test>
    Public Sub Policy_RejectsDefaultPassword()
        Assert.IsNotNull(PasswordPolicy.Validate(AuthService.DefaultAdminPassword, "admin"))
    End Sub

    <Test>
    Public Sub Policy_AcceptsGoodPassword()
        Assert.IsNull(PasswordPolicy.Validate("Narra2026", "admin"))
    End Sub

End Class
