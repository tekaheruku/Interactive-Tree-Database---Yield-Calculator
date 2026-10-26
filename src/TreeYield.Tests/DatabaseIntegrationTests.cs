using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TreeYield.Core.Data;
using TreeYield.Core.Models;
using TreeYield.Core.Services;
using TreeYield.Tests.Framework;

namespace TreeYield.Tests
{
    /// <summary>
    /// End-to-end tests against a real LocalDB database (TreeYieldDb_Test).
    /// They prove that the SQL, constraints, cascade rules and security features work,
    /// not just the C# code.
    /// </summary>
    public class DatabaseIntegrationTests
    {
        /// <summary>Number of species inserted by 002_SeedData.sql.</summary>
        private const int SeedSpeciesCount = 22;

        // Low iteration count for throw-away test accounts keeps the run fast.
        private const int TestIterations = 1000;

        private readonly SqlConnectionFactory _factory;
        private readonly SpeciesRepository _speciesRepository;
        private readonly UserRepository _users;
        private readonly AuditLogRepository _audit;
        private readonly SpeciesService _speciesService;
        private readonly ScenarioService _scenarioService;
        private readonly AuthService _auth;

        public DatabaseIntegrationTests()
        {
            _factory = TestDatabase.Factory;
            _speciesRepository = new SpeciesRepository(_factory);
            _users = new UserRepository(_factory);
            _audit = new AuditLogRepository(_factory);
            var scenarios = new ScenarioRepository(_factory);
            _speciesService = new SpeciesService(_speciesRepository, new CategoryRepository(_factory), scenarios, _audit,
                new DatabaseInitializer(_factory));
            _scenarioService = new ScenarioService(scenarios);
            _auth = new AuthService(_users, _audit);
        }

        [IntegrationTest]
        public async Task Initialize_CreatesCategoriesAndSampleSpecies()
        {
            List<Category> categories = await _speciesService.GetCategoriesAsync();
            List<TreeSpecies> species = await _speciesService.GetAllAsync();

            Assert.AreEqual(2, categories.Count);
            Assert.IsTrue(species.Count >= SeedSpeciesCount, "at least the seeded species");

            TreeSpecies mango = species.Single(s => s.CommonName == "Carabao Mango");
            Assert.AreEqual(300m, mango.YieldPerTreePerYear);
            Assert.AreEqual(80m, mango.PricePerUnit);
            Assert.AreEqual(Category.FruitBearing, mango.CategoryName);
        }

        [IntegrationTest]
        public async Task Initialize_RunTwice_DoesNotDuplicateData()
        {
            await new DatabaseInitializer(_factory).InitializeAsync();

            List<TreeSpecies> species = await _speciesService.GetAllAsync();
            Assert.AreEqual(1, species.Count(s => s.CommonName == "Carabao Mango"));
        }

        [IntegrationTest]
        public async Task DefaultAdmin_IsCreated_AndMustChangePassword()
        {
            AppUser admin = await _users.GetByUsernameAsync(AuthService.DefaultAdminUsername);

            Assert.IsNotNull(admin);
            Assert.IsTrue(admin.MustChangePassword);
            Assert.IsFalse(admin.PasswordHash.SequenceEqual(System.Text.Encoding.UTF8.GetBytes(AuthService.DefaultAdminPassword)),
                "the password must never be stored as plain text");
        }

        [IntegrationTest]
        public async Task Search_FindsPartialNameIgnoringCase()
        {
            List<TreeSpecies> results = await _speciesService.SearchAsync("mAnGo", null);
            Assert.IsTrue(results.Any(s => s.CommonName == "Carabao Mango"));
        }

        [IntegrationTest]
        public async Task Search_ByCategory_ReturnsOnlyThatCategory()
        {
            Category nonFruit = (await _speciesService.GetCategoriesAsync()).Single(c => c.Name == Category.NonFruitBearing);

            List<TreeSpecies> results = await _speciesService.SearchAsync(null, nonFruit.CategoryId);

            Assert.IsTrue(results.Count > 0);
            Assert.IsTrue(results.All(s => s.CategoryId == nonFruit.CategoryId));
            Assert.IsTrue(results.Any(s => s.CommonName == "Mahogany"));
        }

        [IntegrationTest]
        public async Task Search_SqlInjectionText_IsTreatedAsPlainText()
        {
            List<TreeSpecies> orTrick = await _speciesService.SearchAsync("' OR 1=1 --", null);
            List<TreeSpecies> dropTrick = await _speciesService.SearchAsync("'; DROP TABLE dbo.TreeSpecies; --", null);

            Assert.AreEqual(0, orTrick.Count, "the OR 1=1 trick must not return every row");
            Assert.AreEqual(0, dropTrick.Count);
            Assert.IsTrue((await _speciesService.GetAllAsync()).Count >= SeedSpeciesCount, "the table must still exist");
        }

        [IntegrationTest]
        public async Task Search_WildcardCharacters_AreMatchedLiterally()
        {
            Assert.AreEqual(0, (await _speciesService.SearchAsync("%", null)).Count);
            Assert.AreEqual(0, (await _speciesService.SearchAsync("_", null)).Count);
        }

        [IntegrationTest]
        public async Task Save_WithoutAdministrator_IsRefused()
        {
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _speciesService.SaveAsync(NewSpecies("Guest Tree"), null));
        }

        [IntegrationTest]
        public async Task Save_InvalidSpecies_ThrowsValidationException()
        {
            AppUser admin = await Admin();
            TreeSpecies bad = NewSpecies("Invalid Tree");
            bad.PricePerUnit = -10m;

            ValidationException ex = await Assert.ThrowsAsync<ValidationException>(() => _speciesService.SaveAsync(bad, admin));
            Assert.IsNotNull(ex.Result.GetError(nameof(TreeSpecies.PricePerUnit)));
        }

        [IntegrationTest]
        public async Task Save_InsertUpdateDelete_RoundTripAndAudit()
        {
            AppUser admin = await Admin();
            TreeSpecies species = NewSpecies("Test Santol " + Guid.NewGuid().ToString("N").Substring(0, 6));

            int id = await _speciesService.SaveAsync(species, admin);
            TreeSpecies saved = await _speciesRepository.GetByIdAsync(id);
            Assert.AreEqual(species.CommonName, saved.CommonName);
            Assert.AreEqual(55.25m, saved.YieldPerTreePerYear);

            saved.PricePerUnit = 99.5m;
            await _speciesService.SaveAsync(saved, admin);
            Assert.AreEqual(99.5m, (await _speciesRepository.GetByIdAsync(id)).PricePerUnit);

            await _speciesService.DeleteAsync(saved, admin);
            Assert.IsNull(await _speciesRepository.GetByIdAsync(id));

            List<AuditEntry> log = await _audit.GetRecentAsync(20);
            Assert.IsTrue(log.Any(e => e.Action == AuditActions.Create && e.EntityId == id));
            Assert.IsTrue(log.Any(e => e.Action == AuditActions.Update && e.EntityId == id));
            Assert.IsTrue(log.Any(e => e.Action == AuditActions.Delete && e.EntityId == id));
        }

        [IntegrationTest]
        public async Task Save_DuplicateName_ThrowsDuplicateRecordException()
        {
            await Assert.ThrowsAsync<DuplicateRecordException>(async () =>
                await _speciesService.SaveAsync(NewSpecies("Carabao Mango"), await Admin()));
        }

        [IntegrationTest]
        public async Task Database_CheckConstraint_BlocksBadValueEvenWithoutValidator()
        {
            // Goes straight to the repository, skipping the C# validator, to prove the
            // database itself rejects the value (defense in depth).
            TreeSpecies bad = NewSpecies("Constraint Test");
            bad.YieldPerTreePerYear = -1m;

            DataAccessException ex = await Assert.ThrowsAsync<DataAccessException>(() => _speciesRepository.InsertAsync(bad));
            Assert.IsFalse(ex is DuplicateRecordException);
            Assert.AreEqual(SqlErrorTranslator.ConstraintConflict, ((SqlException)ex.InnerException).Number);
        }

        [IntegrationTest]
        public async Task DeleteSpecies_AlsoDeletesItsScenarios()
        {
            AppUser admin = await Admin();
            TreeSpecies species = NewSpecies("Cascade Test " + Guid.NewGuid().ToString("N").Substring(0, 6));
            species.SpeciesId = await _speciesService.SaveAsync(species, admin);

            await _scenarioService.SaveAsync(new Scenario { Name = "Plan A", SpeciesId = species.SpeciesId, TreeCount = 10, ProjectionYears = 10 });
            await _scenarioService.SaveAsync(new Scenario { Name = "Plan B", SpeciesId = species.SpeciesId, TreeCount = 20, ProjectionYears = 5 });
            Assert.AreEqual(2, await _speciesService.CountScenariosAsync(species.SpeciesId));

            await _speciesService.DeleteAsync(species, admin);

            Assert.AreEqual(0, await _speciesService.CountScenariosAsync(species.SpeciesId));
            Assert.IsFalse((await _scenarioService.GetAllAsync()).Any(s => s.SpeciesId == species.SpeciesId));
        }

        [IntegrationTest]
        public async Task Scenario_SaveAndList_IncludesSpeciesName()
        {
            TreeSpecies mango = (await _speciesService.SearchAsync("Carabao Mango", null)).Single();
            int id = await _scenarioService.SaveAsync(new Scenario
            {
                Name = "  Backyard mango  ",
                SpeciesId = mango.SpeciesId,
                TreeCount = 25,
                ProjectionYears = 10
            });

            Scenario saved = (await _scenarioService.GetAllAsync()).Single(s => s.ScenarioId == id);
            Assert.AreEqual("Backyard mango", saved.Name);
            Assert.AreEqual("Carabao Mango", saved.SpeciesName);

            await _scenarioService.DeleteAsync(id);
            Assert.IsFalse((await _scenarioService.GetAllAsync()).Any(s => s.ScenarioId == id));
        }

        [IntegrationTest]
        public async Task Login_UnknownUser_GetsGenericMessage()
        {
            LoginResult result = await _auth.LoginAsync("nobody_" + Guid.NewGuid().ToString("N"), "whatever1");

            Assert.AreEqual(LoginStatus.InvalidCredentials, result.Status);
            Assert.AreEqual("Invalid username or password.", result.Message);
        }

        [IntegrationTest]
        public async Task Login_EmptyInput_IsRejected()
        {
            Assert.AreEqual(LoginStatus.MissingInput, (await _auth.LoginAsync("", "")).Status);
        }

        [IntegrationTest]
        public async Task Login_CorrectPassword_Succeeds()
        {
            string username = await CreateUser("Correct123");

            LoginResult result = await _auth.LoginAsync(username, "Correct123");

            Assert.IsTrue(result.Succeeded, result.Message);
            Assert.AreEqual(username, result.User.Username);
        }

        [IntegrationTest]
        public async Task Login_FiveWrongPasswords_LocksAccountUntilExpiry()
        {
            string username = await CreateUser("Correct123");

            for (int attempt = 1; attempt <= AuthService.MaxFailedAttempts - 1; attempt++)
            {
                Assert.AreEqual(LoginStatus.InvalidCredentials, (await _auth.LoginAsync(username, "wrong" + attempt)).Status);
            }

            Assert.AreEqual(LoginStatus.LockedOut, (await _auth.LoginAsync(username, "wrong5")).Status, "5th failure locks");
            Assert.AreEqual(LoginStatus.LockedOut, (await _auth.LoginAsync(username, "Correct123")).Status,
                "even the right password is refused while locked");

            // Simulate the 5 minutes passing by moving the lock end time into the past.
            await ExecuteAsync("UPDATE dbo.Users SET LockoutUntil = DATEADD(MINUTE, -1, SYSUTCDATETIME()) WHERE Username = @Name;", username);

            Assert.IsTrue((await _auth.LoginAsync(username, "Correct123")).Succeeded, "works again after the lock expires");
            Assert.AreEqual(0, (await _users.GetByUsernameAsync(username)).FailedLoginCount);
        }

        [IntegrationTest]
        public async Task ChangePassword_EnforcesRulesAndStoresNewHash()
        {
            string username = await CreateUser("Correct123");
            AppUser user = (await _auth.LoginAsync(username, "Correct123")).User;

            Assert.AreEqual("The current password is incorrect.", await _auth.ChangePasswordAsync(user, "nope", "Newpass2026", "Newpass2026"));
            Assert.IsNotNull(await _auth.ChangePasswordAsync(user, "Correct123", "short1", "short1"));
            Assert.IsNotNull(await _auth.ChangePasswordAsync(user, "Correct123", "Newpass2026", "Different2026"));
            Assert.IsNull(await _auth.ChangePasswordAsync(user, "Correct123", "Newpass2026", "Newpass2026"));

            Assert.IsFalse((await _auth.LoginAsync(username, "Correct123")).Succeeded, "old password no longer works");
            Assert.IsTrue((await _auth.LoginAsync(username, "Newpass2026")).Succeeded);
        }

        [IntegrationTest]
        public async Task ResetSampleData_RestoresOriginalSpecies()
        {
            AppUser admin = await Admin();
            await _speciesService.SaveAsync(NewSpecies("Temporary Tree"), admin);

            await _speciesService.ResetSampleDataAsync(admin);

            List<TreeSpecies> species = await _speciesService.GetAllAsync();
            Assert.AreEqual(SeedSpeciesCount, species.Count);
            Assert.IsFalse(species.Any(s => s.CommonName == "Temporary Tree"));
            Assert.AreEqual(0, (await _scenarioService.GetAllAsync()).Count);
        }

        private async Task<AppUser> Admin()
        {
            return await _users.GetByUsernameAsync(AuthService.DefaultAdminUsername);
        }

        private async Task<string> CreateUser(string password)
        {
            string username = "user_" + Guid.NewGuid().ToString("N").Substring(0, 10);
            byte[] salt = PasswordHasher.GenerateSalt();
            await _users.CreateAsync(username, PasswordHasher.Hash(password, salt, TestIterations), salt, TestIterations, false);
            return username;
        }

        private static TreeSpecies NewSpecies(string name)
        {
            return new TreeSpecies
            {
                CommonName = name,
                ScientificName = "Testus arboris",
                CategoryId = 1,
                ProductType = "Fresh fruit",
                YieldUnit = "kg",
                YieldPerTreePerYear = 55.25m,
                PricePerUnit = 40m,
                HarvestType = HarvestType.Annual,
                YearsToFirstHarvest = 3,
                YearsToFullProduction = 6
            };
        }

        private async Task ExecuteAsync(string sql, string name)
        {
            using (SqlConnection connection = await _factory.OpenAsync())
            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
