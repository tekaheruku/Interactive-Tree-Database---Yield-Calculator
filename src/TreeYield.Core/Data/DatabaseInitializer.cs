using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace TreeYield.Core.Data
{
    /// <summary>
    /// Creates the database on first run and keeps its schema up to date.
    /// <para>
    /// No .mdf file is shipped with the application. Instead the database is built from
    /// the embedded scripts, so it always matches the SQL Server version installed on
    /// the PC (a .mdf made by a newer LocalDB cannot be opened by an older one).
    /// </para>
    /// It is safe to call <see cref="InitializeAsync"/> every time the program starts.
    /// </summary>
    public class DatabaseInitializer
    {
        /// <summary>Schema version this build of the program expects.</summary>
        public const int CurrentSchemaVersion = 1;

        /// <summary>Embedded script that creates the tables.</summary>
        public const string SchemaScriptName = "001_Schema.sql";

        /// <summary>Embedded script that inserts the categories and sample species.</summary>
        public const string SeedScriptName = "002_SeedData.sql";

        private readonly SqlConnectionFactory _connectionFactory;

        /// <summary>Creates the initializer.</summary>
        public DatabaseInitializer(SqlConnectionFactory connectionFactory)
        {
            if (connectionFactory == null)
            {
                throw new ArgumentNullException(nameof(connectionFactory));
            }

            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Makes sure the database exists and has the current schema and sample data.
        /// The schema and seed scripts run in one transaction: if anything fails,
        /// nothing is half-created.
        /// </summary>
        public async Task InitializeAsync()
        {
            try
            {
                await EnsureDatabaseExistsAsync().ConfigureAwait(false);

                using (SqlConnection connection = await _connectionFactory.OpenAsync().ConfigureAwait(false))
                {
                    int version = await GetSchemaVersionAsync(connection).ConfigureAwait(false);
                    if (version >= CurrentSchemaVersion)
                    {
                        return;
                    }

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        await SqlScriptRunner.ExecuteAsync(connection, transaction,
                            SqlScriptRunner.LoadEmbeddedScript(SchemaScriptName)).ConfigureAwait(false);
                        await SqlScriptRunner.ExecuteAsync(connection, transaction,
                            SqlScriptRunner.LoadEmbeddedScript(SeedScriptName)).ConfigureAwait(false);

                        using (var command = new SqlCommand(
                            "INSERT INTO dbo.SchemaVersion (Version) VALUES (@Version);", connection, transaction))
                        {
                            command.Parameters.Add("@Version", SqlDbType.Int).Value = CurrentSchemaVersion;
                            await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                        }

                        transaction.Commit();
                    }
                }
            }
            catch (SqlException ex)
            {
                throw SqlErrorTranslator.Translate("prepare the database", ex);
            }
        }

        /// <summary>
        /// Deletes all species and saved scenarios and inserts the original sample species
        /// again. User accounts and the audit log are kept.
        /// </summary>
        public async Task ResetSampleDataAsync()
        {
            try
            {
                using (SqlConnection connection = await _connectionFactory.OpenAsync().ConfigureAwait(false))
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    using (var command = new SqlCommand(
                        "DELETE FROM dbo.Scenarios; DELETE FROM dbo.TreeSpecies;", connection, transaction))
                    {
                        await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                    }

                    await SqlScriptRunner.ExecuteAsync(connection, transaction,
                        SqlScriptRunner.LoadEmbeddedScript(SeedScriptName)).ConfigureAwait(false);

                    transaction.Commit();
                }
            }
            catch (SqlException ex)
            {
                throw SqlErrorTranslator.Translate("restore the sample data", ex);
            }
        }

        private async Task EnsureDatabaseExistsAsync()
        {
            using (SqlConnection master = await _connectionFactory.OpenMasterAsync().ConfigureAwait(false))
            {
                using (var check = new SqlCommand("SELECT DB_ID(@Name);", master))
                {
                    check.Parameters.Add("@Name", SqlDbType.NVarChar, 128).Value = _connectionFactory.DatabaseName;
                    object id = await check.ExecuteScalarAsync().ConfigureAwait(false);
                    if (id != null && id != DBNull.Value)
                    {
                        return;
                    }
                }

                // CREATE DATABASE cannot use a parameter for the name, so the name is
                // bracket-quoted by SqlText.QuoteIdentifier to keep it from being run as SQL.
                string sql = "CREATE DATABASE " + SqlText.QuoteIdentifier(_connectionFactory.DatabaseName) + ";";
                using (var create = new SqlCommand(sql, master))
                {
                    create.CommandTimeout = 120;
                    await create.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
        }

        private static async Task<int> GetSchemaVersionAsync(SqlConnection connection)
        {
            const string sql = @"
IF OBJECT_ID(N'dbo.SchemaVersion', N'U') IS NULL
    SELECT 0;
ELSE
    SELECT ISNULL(MAX(Version), 0) FROM dbo.SchemaVersion;";

            using (var command = new SqlCommand(sql, connection))
            {
                return Convert.ToInt32(await command.ExecuteScalarAsync().ConfigureAwait(false));
            }
        }
    }
}
