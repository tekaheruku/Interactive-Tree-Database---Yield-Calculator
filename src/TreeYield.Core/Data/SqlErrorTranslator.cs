using System.Data.SqlClient;

namespace TreeYield.Core.Data
{
    /// <summary>
    /// Converts SQL Server error numbers into friendly <see cref="DataAccessException"/>s,
    /// so users never see raw database messages (which can also leak internal details).
    /// </summary>
    public static class SqlErrorTranslator
    {
        /// <summary>Violation of a UNIQUE constraint.</summary>
        public const int UniqueConstraintViolation = 2627;

        /// <summary>Duplicate key in a UNIQUE index.</summary>
        public const int UniqueIndexViolation = 2601;

        /// <summary>CHECK or FOREIGN KEY constraint conflict.</summary>
        public const int ConstraintConflict = 547;

        /// <summary>Command timeout.</summary>
        public const int Timeout = -2;

        private const string ConnectionHelp =
            "Cannot connect to the database. Make sure SQL Server Express LocalDB is installed " +
            "and that the connection string in TreeYield.WinForms.exe.config is correct.";

        /// <summary>Returns the exception that should be thrown instead of <paramref name="exception"/>.</summary>
        /// <param name="operation">What was being done, for example "load the species list".</param>
        /// <param name="exception">The original SQL exception.</param>
        public static DataAccessException Translate(string operation, SqlException exception)
        {
            switch (exception.Number)
            {
                case UniqueConstraintViolation:
                case UniqueIndexViolation:
                    return new DuplicateRecordException("A record with the same name already exists.", exception);

                case ConstraintConflict:
                    return new DataAccessException(
                        "The database rejected the change because a value is out of range " +
                        "or the record is still linked to other data.", exception);

                case Timeout:
                    return new DataAccessException("The database took too long to respond. Please try again.", exception);

                case -1:    // general network error
                case 2:     // server not found
                case 53:    // network path not found
                case 50:    // LocalDB runtime error
                case 4060:  // cannot open database
                case 18456: // login failed
                    return new DataAccessException(ConnectionHelp, exception);

                default:
                    return new DataAccessException(
                        string.Format("A database error occurred while trying to {0}. (SQL error {1})",
                            operation, exception.Number), exception);
            }
        }
    }
}
