using System;
using System.Configuration;
using System.Data.SqlClient;
using TreeYield.Core.Data;
using TreeYield.Core.Services;

namespace TreeYield.Tests
{
    /// <summary>
    /// Creates a fresh copy of the database (TreeYieldDb_Test) the first time an
    /// integration test needs it, and drops it when the run ends.
    /// </summary>
    internal static class TestDatabase
    {
        private static SqlConnectionFactory _factory;

        /// <summary>Connection factory for the test database (created on first use).</summary>
        public static SqlConnectionFactory Factory
        {
            get
            {
                if (_factory == null)
                {
                    _factory = Create();
                }

                return _factory;
            }
        }

        /// <summary>Drops the test database if this run created it.</summary>
        public static void DropIfCreated()
        {
            if (_factory != null)
            {
                Drop(_factory);
                _factory = null;
            }
        }

        private static SqlConnectionFactory Create()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["TreeYieldDbTest"];
            if (settings == null)
            {
                throw new InvalidOperationException("The TreeYieldDbTest connection string is missing from App.config.");
            }

            var factory = new SqlConnectionFactory(settings.ConnectionString);

            // Safety net: these tests delete data, so never run them against the real database.
            if (!factory.DatabaseName.EndsWith("_Test", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Integration tests only run against a database whose name ends in _Test.");
            }

            Drop(factory);
            new DatabaseInitializer(factory).InitializeAsync().GetAwaiter().GetResult();
            new AuthService(new UserRepository(factory), new AuditLogRepository(factory))
                .EnsureDefaultAdminAsync().GetAwaiter().GetResult();
            return factory;
        }

        private static void Drop(SqlConnectionFactory factory)
        {
            // Pooled connections keep the database "in use"; clear them before dropping.
            SqlConnection.ClearAllPools();

            string name = SqlText.QuoteIdentifier(factory.DatabaseName);
            using (SqlConnection master = factory.OpenMasterAsync().GetAwaiter().GetResult())
            using (SqlCommand command = master.CreateCommand())
            {
                command.CommandText =
                    "IF DB_ID(@Name) IS NOT NULL BEGIN " +
                    "ALTER DATABASE " + name + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE; " +
                    "DROP DATABASE " + name + "; END";
                command.Parameters.AddWithValue("@Name", factory.DatabaseName);
                command.CommandTimeout = 120;
                command.ExecuteNonQuery();
            }
        }
    }
}
