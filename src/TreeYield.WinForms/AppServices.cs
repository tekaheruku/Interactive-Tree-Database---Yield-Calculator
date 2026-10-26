using System;
using System.Configuration;
using System.Threading.Tasks;
using TreeYield.Core.Data;
using TreeYield.Core.Services;
using TreeYield.WinForms.UI;

namespace TreeYield.WinForms
{
    /// <summary>
    /// The "composition root": creates every repository and service once and hands
    /// them to the forms. Forms never create database objects themselves, which keeps
    /// the presentation layer separate from the data layer.
    /// </summary>
    internal sealed class AppServices
    {
        /// <summary>Name of the connection string in App.config.</summary>
        public const string ConnectionStringName = "TreeYieldDb";

        /// <summary>Builds all services for one connection string.</summary>
        public AppServices(string connectionString)
        {
            ConnectionFactory = new SqlConnectionFactory(connectionString);

            var speciesRepository = new SpeciesRepository(ConnectionFactory);
            var categoryRepository = new CategoryRepository(ConnectionFactory);
            var scenarioRepository = new ScenarioRepository(ConnectionFactory);
            var userRepository = new UserRepository(ConnectionFactory);
            var auditRepository = new AuditLogRepository(ConnectionFactory);

            Initializer = new DatabaseInitializer(ConnectionFactory);
            Species = new SpeciesService(speciesRepository, categoryRepository, scenarioRepository, auditRepository, Initializer);
            Scenarios = new ScenarioService(scenarioRepository);
            Auth = new AuthService(userRepository, auditRepository);
            Session = new AdminSession();
        }

        /// <summary>Opens database connections.</summary>
        public SqlConnectionFactory ConnectionFactory { get; private set; }

        /// <summary>Creates and upgrades the database.</summary>
        public DatabaseInitializer Initializer { get; private set; }

        /// <summary>Species records (read for everyone, write for administrators).</summary>
        public SpeciesService Species { get; private set; }

        /// <summary>Saved scenarios.</summary>
        public ScenarioService Scenarios { get; private set; }

        /// <summary>Administrator login and passwords.</summary>
        public AuthService Auth { get; private set; }

        /// <summary>Who is signed in as administrator right now.</summary>
        public AdminSession Session { get; private set; }

        /// <summary>Reads the connection string from TreeYield.WinForms.exe.config.</summary>
        /// <exception cref="ConfigurationErrorsException">The connection string is missing.</exception>
        public static AppServices FromConfiguration()
        {
            ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings[ConnectionStringName];
            if (setting == null || string.IsNullOrWhiteSpace(setting.ConnectionString))
            {
                throw new ConfigurationErrorsException(
                    "The '" + ConnectionStringName + "' connection string is missing.");
            }

            return new AppServices(setting.ConnectionString);
        }

        /// <summary>Creates the database if needed and makes sure an administrator account exists.</summary>
        public async Task InitializeDatabaseAsync()
        {
            await Initializer.InitializeAsync();
            await Auth.EnsureDefaultAdminAsync();
        }
    }
}
