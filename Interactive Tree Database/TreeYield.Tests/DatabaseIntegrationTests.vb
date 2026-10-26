Imports System.Data
Imports System.Data.OleDb
Imports TreeYield.Core.Data
Imports TreeYield.Core.Models
Imports TreeYield.Core.Services
Imports TreeYield.Tests.Framework

''' <summary>
''' End-to-end tests against a real Microsoft Access database file (a throw-away .mdb
''' in the temp folder). They prove that the SQL, constraints, relationships, cascade
''' rules and security features work, not just the VB code.
''' </summary>
Public Class DatabaseIntegrationTests

    ''' <summary>Number of species in SeedData.</summary>
    Private Const SeedSpeciesCount As Integer = 22

    ' Low iteration count for throw-away test accounts keeps the run fast.
    Private Const TestIterations As Integer = 1000

    Private ReadOnly _factory As AccessConnectionFactory
    Private ReadOnly _speciesRepository As SpeciesRepository
    Private ReadOnly _users As UserRepository
    Private ReadOnly _audit As AuditLogRepository
    Private ReadOnly _speciesService As SpeciesService
    Private ReadOnly _scenarioService As ScenarioService
    Private ReadOnly _auth As AuthService

    Public Sub New()
        _factory = TestDatabase.Factory
        _speciesRepository = New SpeciesRepository(_factory)
        _users = New UserRepository(_factory)
        _audit = New AuditLogRepository(_factory)
        Dim scenarios As New ScenarioRepository(_factory)
        _speciesService = New SpeciesService(_speciesRepository, New CategoryRepository(_factory), scenarios, _audit,
                                             New DatabaseInitializer(_factory))
        _scenarioService = New ScenarioService(scenarios)
        _auth = New AuthService(_users, _audit)
    End Sub

    <IntegrationTest>
    Public Sub NewDatabase_IsAnAccessFileWithTablesAndRelationships()
        Assert.IsTrue(_factory.DatabasePath.EndsWith(".mdb", StringComparison.OrdinalIgnoreCase))

        Using connection As OleDbConnection = _factory.Open()
            Dim tables As DataTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})
            Dim names As List(Of String) = tables.Rows.Cast(Of DataRow)().Select(Function(r) CStr(r("TABLE_NAME"))).ToList()
            For Each table As String In {"SchemaVersion", "Categories", "TreeSpecies", "Users", "Scenarios", "AuditLog"}
                Assert.IsTrue(names.Contains(table), "table " & table)
            Next

            ' The three foreign keys are real Access relationships (visible in Database Tools > Relationships).
            Dim keys As DataTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, Nothing)
            Assert.AreEqual(3, keys.Rows.Count, "relationships")
        End Using
    End Sub

    <IntegrationTest>
    Public Async Function Initialize_CreatesCategoriesAndSampleSpecies() As Task
        Dim categories As List(Of Category) = Await _speciesService.GetCategoriesAsync()
        Dim species As List(Of TreeSpecies) = Await _speciesService.GetAllAsync()

        Assert.AreEqual(2, categories.Count)
        Assert.IsTrue(species.Count >= SeedSpeciesCount, "at least the seeded species")

        Dim mango As TreeSpecies = species.Single(Function(s) s.CommonName = "Carabao Mango")
        Assert.AreEqual(300D, mango.YieldPerTreePerYear)
        Assert.AreEqual(80D, mango.PricePerUnit)
        Assert.AreEqual(Category.FruitBearing, mango.CategoryName)
    End Function

    <IntegrationTest>
    Public Async Function Initialize_RunTwice_DoesNotDuplicateData() As Task
        Dim initializer As New DatabaseInitializer(_factory)
        Await initializer.InitializeAsync()

        Dim species As List(Of TreeSpecies) = Await _speciesService.GetAllAsync()
        Assert.AreEqual(1, species.Where(Function(s) s.CommonName = "Carabao Mango").Count())
    End Function

    <IntegrationTest>
    Public Async Function DefaultAdmin_IsCreated_AndMustChangePassword() As Task
        Dim admin As AppUser = Await _users.GetByUsernameAsync(AuthService.DefaultAdminUsername)

        Assert.IsNotNull(admin)
        Assert.IsTrue(admin.MustChangePassword)
        Assert.IsFalse(admin.PasswordHash.SequenceEqual(Text.Encoding.UTF8.GetBytes(AuthService.DefaultAdminPassword)),
                       "the password must never be stored as plain text")
    End Function

    <IntegrationTest>
    Public Async Function Search_FindsPartialNameIgnoringCase() As Task
        Dim results As List(Of TreeSpecies) = Await _speciesService.SearchAsync("mAnGo", Nothing)
        Assert.IsTrue(results.Any(Function(s) s.CommonName = "Carabao Mango"))
    End Function

    <IntegrationTest>
    Public Async Function Search_ByCategory_ReturnsOnlyThatCategory() As Task
        Dim nonFruit As Category = (Await _speciesService.GetCategoriesAsync()).Single(Function(c) c.Name = Category.NonFruitBearing)

        Dim results As List(Of TreeSpecies) = Await _speciesService.SearchAsync(Nothing, nonFruit.CategoryId)

        Assert.IsTrue(results.Count > 0)
        Assert.IsTrue(results.All(Function(s) s.CategoryId = nonFruit.CategoryId))
        Assert.IsTrue(results.Any(Function(s) s.CommonName = "Mahogany"))
    End Function

    <IntegrationTest>
    Public Async Function Search_SqlInjectionText_IsTreatedAsPlainText() As Task
        Dim orTrick As List(Of TreeSpecies) = Await _speciesService.SearchAsync("' OR 1=1 --", Nothing)
        Dim dropTrick As List(Of TreeSpecies) = Await _speciesService.SearchAsync("'; DROP TABLE TreeSpecies; --", Nothing)

        Assert.AreEqual(0, orTrick.Count, "the OR 1=1 trick must not return every row")
        Assert.AreEqual(0, dropTrick.Count)
        Assert.IsTrue((Await _speciesService.GetAllAsync()).Count >= SeedSpeciesCount, "the table must still exist")
    End Function

    <IntegrationTest>
    Public Async Function Search_WildcardCharacters_AreMatchedLiterally() As Task
        Assert.AreEqual(0, (Await _speciesService.SearchAsync("%", Nothing)).Count)
        Assert.AreEqual(0, (Await _speciesService.SearchAsync("_", Nothing)).Count)
        Assert.AreEqual(0, (Await _speciesService.SearchAsync("[", Nothing)).Count)
    End Function

    <IntegrationTest>
    Public Async Function Save_WithoutAdministrator_IsRefused() As Task
        Await Assert.ThrowsAsync(Of UnauthorizedAccessException)(Function() _speciesService.SaveAsync(NewSpecies("Guest Tree"), Nothing))
    End Function

    <IntegrationTest>
    Public Async Function Save_InvalidSpecies_ThrowsValidationException() As Task
        Dim admin As AppUser = Await GetAdminAsync()
        Dim bad As TreeSpecies = NewSpecies("Invalid Tree")
        bad.PricePerUnit = -10D

        Dim ex As ValidationException = Await Assert.ThrowsAsync(Of ValidationException)(Function() _speciesService.SaveAsync(bad, admin))
        Assert.IsNotNull(ex.Result.GetError(NameOf(TreeSpecies.PricePerUnit)))
    End Function

    <IntegrationTest>
    Public Async Function Save_InsertUpdateDelete_RoundTripAndAudit() As Task
        Dim admin As AppUser = Await GetAdminAsync()
        Dim species As TreeSpecies = NewSpecies("Test Santol " & Guid.NewGuid().ToString("N").Substring(0, 6))

        Dim id As Integer = Await _speciesService.SaveAsync(species, admin)
        Dim saved As TreeSpecies = Await _speciesRepository.GetByIdAsync(id)
        Assert.AreEqual(species.CommonName, saved.CommonName)
        Assert.AreEqual(55.25D, saved.YieldPerTreePerYear)

        saved.PricePerUnit = 99.5D
        Await _speciesService.SaveAsync(saved, admin)
        Assert.AreEqual(99.5D, (Await _speciesRepository.GetByIdAsync(id)).PricePerUnit)

        Await _speciesService.DeleteAsync(saved, admin)
        Assert.IsNull(Await _speciesRepository.GetByIdAsync(id))

        Dim log As List(Of AuditEntry) = Await _audit.GetRecentAsync(20)
        Assert.IsTrue(log.Any(Function(e) e.Action = AuditActions.Create AndAlso e.EntityId.GetValueOrDefault() = id))
        Assert.IsTrue(log.Any(Function(e) e.Action = AuditActions.Update AndAlso e.EntityId.GetValueOrDefault() = id))
        Assert.IsTrue(log.Any(Function(e) e.Action = AuditActions.Delete AndAlso e.EntityId.GetValueOrDefault() = id))
    End Function

    <IntegrationTest>
    Public Async Function Save_DuplicateName_ThrowsDuplicateRecordException() As Task
        Dim admin As AppUser = Await GetAdminAsync()
        ' Access compares text case-insensitively, so a different case is still a duplicate.
        Await Assert.ThrowsAsync(Of DuplicateRecordException)(Function() _speciesService.SaveAsync(NewSpecies("CARABAO MANGO"), admin))
    End Function

    <IntegrationTest>
    Public Async Function Database_CheckConstraint_BlocksBadValueEvenWithoutValidator() As Task
        ' Goes straight to the repository, skipping the VB validator, to prove the
        ' Access file itself rejects the value (defense in depth).
        Dim bad As TreeSpecies = NewSpecies("Constraint Test")
        bad.YieldPerTreePerYear = -1D

        Dim ex As DataAccessException = Await Assert.ThrowsAsync(Of DataAccessException)(Function() _speciesRepository.InsertAsync(bad))
        Assert.IsFalse(TypeOf ex Is DuplicateRecordException)
        Assert.AreEqual(AccessErrorTranslator.ValidationRuleBroken, DirectCast(ex.InnerException, OleDbException).Errors(0).SQLState)
    End Function

    <IntegrationTest>
    Public Async Function DeleteSpecies_AlsoDeletesItsScenarios() As Task
        Dim admin As AppUser = Await GetAdminAsync()
        Dim species As TreeSpecies = NewSpecies("Cascade Test " & Guid.NewGuid().ToString("N").Substring(0, 6))
        species.SpeciesId = Await _speciesService.SaveAsync(species, admin)

        Await _scenarioService.SaveAsync(New Scenario With {.Name = "Plan A", .SpeciesId = species.SpeciesId, .TreeCount = 10, .ProjectionYears = 10})
        Await _scenarioService.SaveAsync(New Scenario With {.Name = "Plan B", .SpeciesId = species.SpeciesId, .TreeCount = 20, .ProjectionYears = 5})
        Assert.AreEqual(2, Await _speciesService.CountScenariosAsync(species.SpeciesId))

        Await _speciesService.DeleteAsync(species, admin)

        Assert.AreEqual(0, Await _speciesService.CountScenariosAsync(species.SpeciesId))
        Assert.IsFalse((Await _scenarioService.GetAllAsync()).Any(Function(s) s.SpeciesId = species.SpeciesId))
    End Function

    <IntegrationTest>
    Public Async Function Scenario_SaveAndList_IncludesSpeciesName() As Task
        Dim mango As TreeSpecies = (Await _speciesService.SearchAsync("Carabao Mango", Nothing)).Single()
        Dim id As Integer = Await _scenarioService.SaveAsync(New Scenario With {
            .Name = "  Backyard mango  ",
            .SpeciesId = mango.SpeciesId,
            .TreeCount = 25,
            .ProjectionYears = 10
        })

        Dim saved As Scenario = (Await _scenarioService.GetAllAsync()).Single(Function(s) s.ScenarioId = id)
        Assert.AreEqual("Backyard mango", saved.Name)
        Assert.AreEqual("Carabao Mango", saved.SpeciesName)

        Await _scenarioService.DeleteAsync(id)
        Assert.IsFalse((Await _scenarioService.GetAllAsync()).Any(Function(s) s.ScenarioId = id))
    End Function

    <IntegrationTest>
    Public Async Function Login_UnknownUser_GetsGenericMessage() As Task
        Dim result As LoginResult = Await _auth.LoginAsync("nobody_" & Guid.NewGuid().ToString("N"), "whatever1")

        Assert.AreEqual(LoginStatus.InvalidCredentials, result.Status)
        Assert.AreEqual("Invalid username or password.", result.Message)
    End Function

    <IntegrationTest>
    Public Async Function Login_EmptyInput_IsRejected() As Task
        Assert.AreEqual(LoginStatus.MissingInput, (Await _auth.LoginAsync("", "")).Status)
    End Function

    <IntegrationTest>
    Public Async Function Login_CorrectPassword_Succeeds() As Task
        Dim username As String = Await CreateUserAsync("Correct123")

        Dim result As LoginResult = Await _auth.LoginAsync(username, "Correct123")

        Assert.IsTrue(result.Succeeded, result.Message)
        Assert.AreEqual(username, result.User.Username)
    End Function

    <IntegrationTest>
    Public Async Function Login_FiveWrongPasswords_LocksAccountUntilExpiry() As Task
        Dim username As String = Await CreateUserAsync("Correct123")

        For attempt As Integer = 1 To AuthService.MaxFailedAttempts - 1
            Assert.AreEqual(LoginStatus.InvalidCredentials, (Await _auth.LoginAsync(username, "wrong" & attempt.ToString())).Status)
        Next

        Assert.AreEqual(LoginStatus.LockedOut, (Await _auth.LoginAsync(username, "wrong5")).Status, "5th failure locks")
        Assert.AreEqual(LoginStatus.LockedOut, (Await _auth.LoginAsync(username, "Correct123")).Status,
                        "even the right password is refused while locked")

        ' Simulate the 5 minutes passing by moving the lock end time into the past.
        Using connection As OleDbConnection = _factory.Open()
            Using command As New OleDbCommand("UPDATE Users SET LockoutUntil = ? WHERE Username = ?", connection)
                command.Parameters.Add("?", OleDbType.Date).Value = DateTime.UtcNow.AddMinutes(-1)
                command.Parameters.Add("?", OleDbType.VarWChar, 50).Value = username
                command.ExecuteNonQuery()
            End Using
        End Using

        Assert.IsTrue((Await _auth.LoginAsync(username, "Correct123")).Succeeded, "works again after the lock expires")
        Assert.AreEqual(0, (Await _users.GetByUsernameAsync(username)).FailedLoginCount)
    End Function

    <IntegrationTest>
    Public Async Function ChangePassword_EnforcesRulesAndStoresNewHash() As Task
        Dim username As String = Await CreateUserAsync("Correct123")
        Dim user As AppUser = (Await _auth.LoginAsync(username, "Correct123")).User

        Assert.AreEqual("The current password is incorrect.", Await _auth.ChangePasswordAsync(user, "nope", "Newpass2026", "Newpass2026"))
        Assert.IsNotNull(Await _auth.ChangePasswordAsync(user, "Correct123", "short1", "short1"))
        Assert.IsNotNull(Await _auth.ChangePasswordAsync(user, "Correct123", "Newpass2026", "Different2026"))
        Assert.IsNull(Await _auth.ChangePasswordAsync(user, "Correct123", "Newpass2026", "Newpass2026"))

        Assert.IsFalse((Await _auth.LoginAsync(username, "Correct123")).Succeeded, "old password no longer works")
        Assert.IsTrue((Await _auth.LoginAsync(username, "Newpass2026")).Succeeded)
    End Function

    <IntegrationTest>
    Public Async Function FileOpenExclusivelyElsewhere_GivesFriendlyMessage() As Task
        ' Simulates Microsoft Access holding the file open in exclusive mode.
        OleDbConnection.ReleaseObjectPool()
        Using exclusive As New OleDbConnection(_factory.ConnectionString & ";Mode=Share Exclusive")
            exclusive.Open()
            Dim ex As DataAccessException = Await Assert.ThrowsAsync(Of DataAccessException)(Function() _speciesService.GetAllAsync())
            Assert.IsTrue(ex.Message.Contains("in use"), ex.Message)
        End Using

        OleDbConnection.ReleaseObjectPool()
        Assert.IsTrue((Await _speciesService.GetAllAsync()).Count > 0, "works again once the file is closed")
    End Function

    <IntegrationTest>
    Public Async Function ResetSampleData_RestoresOriginalSpecies() As Task
        Dim admin As AppUser = Await GetAdminAsync()
        Await _speciesService.SaveAsync(NewSpecies("Temporary Tree"), admin)

        Await _speciesService.ResetSampleDataAsync(admin)

        Dim species As List(Of TreeSpecies) = Await _speciesService.GetAllAsync()
        Assert.AreEqual(SeedSpeciesCount, species.Count)
        Assert.IsFalse(species.Any(Function(s) s.CommonName = "Temporary Tree"))
        Assert.AreEqual(0, (Await _scenarioService.GetAllAsync()).Count)
    End Function

    Private Function GetAdminAsync() As Task(Of AppUser)
        Return _users.GetByUsernameAsync(AuthService.DefaultAdminUsername)
    End Function

    Private Async Function CreateUserAsync(password As String) As Task(Of String)
        Dim username As String = "user_" & Guid.NewGuid().ToString("N").Substring(0, 10)
        Dim salt As Byte() = PasswordHasher.GenerateSalt()
        Await _users.CreateAsync(username, PasswordHasher.Hash(password, salt, TestIterations), salt, TestIterations, False)
        Return username
    End Function

    Private Shared Function NewSpecies(name As String) As TreeSpecies
        Return New TreeSpecies With {
            .CommonName = name,
            .ScientificName = "Testus arboris",
            .CategoryId = 1,
            .ProductType = "Fresh fruit",
            .YieldUnit = "kg",
            .YieldPerTreePerYear = 55.25D,
            .PricePerUnit = 40D,
            .HarvestType = HarvestType.Annual,
            .YearsToFirstHarvest = 3,
            .YearsToFullProduction = 6
        }
    End Function

End Class
