using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace TreeYield.Core.Data
{
    /// <summary>
    /// Shared plumbing for every repository: opening a connection, translating SQL
    /// errors, and adding typed parameters. Every query in this application uses
    /// parameters (@Name) instead of joining user input into the SQL text, which is
    /// what prevents SQL injection.
    /// </summary>
    public abstract class RepositoryBase
    {
        private readonly SqlConnectionFactory _connectionFactory;

        /// <summary>Creates the repository.</summary>
        protected RepositoryBase(SqlConnectionFactory connectionFactory)
        {
            if (connectionFactory == null)
            {
                throw new ArgumentNullException(nameof(connectionFactory));
            }

            _connectionFactory = connectionFactory;
        }

        /// <summary>
        /// Opens a connection, runs <paramref name="work"/>, closes the connection and
        /// converts any <see cref="SqlException"/> into a <see cref="DataAccessException"/>.
        /// </summary>
        /// <param name="operation">Short description used in error messages, for example "save the species".</param>
        /// <param name="work">The database work to run on the open connection.</param>
        protected async Task<T> WithConnectionAsync<T>(string operation, Func<SqlConnection, Task<T>> work)
        {
            try
            {
                using (SqlConnection connection = await _connectionFactory.OpenAsync().ConfigureAwait(false))
                {
                    return await work(connection).ConfigureAwait(false);
                }
            }
            catch (SqlException ex)
            {
                throw SqlErrorTranslator.Translate(operation, ex);
            }
        }

        /// <summary>Same as <see cref="WithConnectionAsync{T}"/> for work that returns no value.</summary>
        protected Task WithConnectionAsync(string operation, Func<SqlConnection, Task> work)
        {
            return WithConnectionAsync(operation, async connection =>
            {
                await work(connection).ConfigureAwait(false);
                return true;
            });
        }

        /// <summary>Creates a text command on an open connection.</summary>
        protected static SqlCommand CreateCommand(SqlConnection connection, string sql)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 30;
            return command;
        }

        /// <summary>Adds a typed parameter. A null value is sent as SQL NULL.</summary>
        protected static void AddParameter(SqlCommand command, string name, SqlDbType type, object value)
        {
            command.Parameters.Add(name, type).Value = value ?? DBNull.Value;
        }

        /// <summary>Adds a typed string parameter with a maximum length.</summary>
        protected static void AddParameter(SqlCommand command, string name, SqlDbType type, int size, object value)
        {
            command.Parameters.Add(name, type, size).Value = value ?? DBNull.Value;
        }

        /// <summary>Adds a DECIMAL(12,2) parameter, matching the money and quantity columns.</summary>
        protected static void AddDecimal(SqlCommand command, string name, decimal value)
        {
            SqlParameter parameter = command.Parameters.Add(name, SqlDbType.Decimal);
            parameter.Precision = 12;
            parameter.Scale = 2;
            parameter.Value = value;
        }

        /// <summary>Reads a nullable string column.</summary>
        protected static string ReadString(SqlDataReader reader, string column)
        {
            int ordinal = reader.GetOrdinal(column);
            return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
        }

        /// <summary>Reads a nullable INT column.</summary>
        protected static int? ReadNullableInt(SqlDataReader reader, string column)
        {
            int ordinal = reader.GetOrdinal(column);
            return reader.IsDBNull(ordinal) ? (int?)null : reader.GetInt32(ordinal);
        }

        /// <summary>Reads a nullable DATETIME2 column.</summary>
        protected static DateTime? ReadNullableDateTime(SqlDataReader reader, string column)
        {
            int ordinal = reader.GetOrdinal(column);
            return reader.IsDBNull(ordinal) ? (DateTime?)null : reader.GetDateTime(ordinal);
        }
    }
}
