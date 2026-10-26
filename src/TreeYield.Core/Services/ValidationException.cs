using System;

namespace TreeYield.Core.Services
{
    /// <summary>
    /// Thrown by a service when it is asked to save data that fails validation.
    /// The UI catches it and shows <see cref="Result"/> next to the input fields.
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>Creates the exception from a failed validation result.</summary>
        public ValidationException(ValidationResult result)
            : base(result == null ? "The data is not valid." : result.ToString())
        {
            Result = result ?? new ValidationResult();
        }

        /// <summary>The individual field errors.</summary>
        public ValidationResult Result { get; private set; }
    }
}
