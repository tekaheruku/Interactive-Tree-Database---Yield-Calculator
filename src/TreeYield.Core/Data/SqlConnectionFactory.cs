using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace TreeYield.Core.Data
{
    /// <summary>
    /// Creates database connections from one connection string. Connections are opened
    /// late and closed early (inside "using" blocks); ADO.NET connection pooling makes
    /// re-opening them cheap.
    /// </summary>
    public class SqlConnectionFactory
    {
        private readonly string _connectionString;

        /// <summary>Creates the factory.</summary>
        /// <param name="connectionString">A SQL Server connection string that names an Initial Catalog.</param>
        public SqlConnectionFactory(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("A connection string is required.", nameof(connectionString));
            }

            // Parsing here fails fast with a clear error if the string is malformed.
            var builder = new SqlConnectionStringBuilder(connectionString);
            if (string.IsNullOrWhiteSpace(builder.InitialCatalog))
            {
                throw new ArgumentException("The connection string must include an Initial Catalog (database name).",
                    nameof(connectionString));
            }

            _connectionString = builder.ConnectionString;
            DatabaseName = builder.InitialCatalog;
            ServerName = builder.DataSource;
        }

        /// <summary>Database name from the connection string, for example "TreeYieldDb".</summary>
        public string DatabaseName { get; private set; }

        /// <summary>Server name from the connection string, for example "(localdb)\MSSQLLocalDB".</summary>
        public string ServerName { get; private set; }

        /// <summary>Opens a connection to the application database.</summary>
        public Task<SqlConnection> OpenAsync()
        {
            return OpenAsync(_connectionString);
        }

        /// <summary>
        /// Opens a connection to the "master" database of the same server. Needed to
        /// create the application database when it does not exist yet.
        /// </summary>
        public Task<SqlConnection> OpenMasterAsync()
        {
            var builder = new SqlConnectionStringBuilder(_connectionString) { InitialCatalog = "master" };
            return OpenAsync(builder.ConnectionString);
        }

        private static async Task<SqlConnection> OpenAsync(string connectionString)
        {
            var connection = new SqlConnection(connectionString);
            try
            {
                await connection.OpenAsync().ConfigureAwait(false);
                return connection;
            }
            catch
            {
                // Do not leak the connection object if opening fails.
                connection.Dispose();
                throw;
            }
        }
    }
}
