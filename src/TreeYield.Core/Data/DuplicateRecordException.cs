using System;

namespace TreeYield.Core.Data
{
    /// <summary>
    /// Thrown when an INSERT or UPDATE breaks a UNIQUE constraint, for example when
    /// two species would get the same common name.
    /// </summary>
    public class DuplicateRecordException : DataAccessException
    {
        /// <summary>Creates the exception.</summary>
        public DuplicateRecordException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
