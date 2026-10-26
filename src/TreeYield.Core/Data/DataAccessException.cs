using System;

namespace TreeYield.Core.Data
{
    /// <summary>
    /// A database problem translated into a message the user can understand.
    /// The original <see cref="System.Data.SqlClient.SqlException"/> is kept in
    /// <see cref="Exception.InnerException"/> for the log file.
    /// </summary>
    public class DataAccessException : Exception
    {
        /// <summary>Creates the exception.</summary>
        /// <param name="message">User-friendly message.</param>
        /// <param name="innerException">The original database exception.</param>
        public DataAccessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
