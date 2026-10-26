using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeYield.Core.Services
{
    /// <summary>One validation problem tied to a field (property name) of the object being checked.</summary>
    public class ValidationError
    {
        /// <summary>Creates an error for a field.</summary>
        public ValidationError(string field, string message)
        {
            Field = field;
            Message = message;
        }

        /// <summary>Property name, for example "CommonName". The UI uses it to find the input box.</summary>
        public string Field { get; private set; }

        /// <summary>Message for the user, for example "Common name is required."</summary>
        public string Message { get; private set; }
    }

    /// <summary>
    /// A list of validation errors. Validators return this instead of throwing on the
    /// first problem, so the form can highlight every invalid field at once.
    /// </summary>
    public class ValidationResult
    {
        private readonly List<ValidationError> _errors = new List<ValidationError>();

        /// <summary>True when no errors were added.</summary>
        public bool IsValid
        {
            get { return _errors.Count == 0; }
        }

        /// <summary>All errors in the order they were found.</summary>
        public IReadOnlyList<ValidationError> Errors
        {
            get { return _errors; }
        }

        /// <summary>Adds an error for a field.</summary>
        public void Add(string field, string message)
        {
            _errors.Add(new ValidationError(field, message));
        }

        /// <summary>Returns the first error message for a field, or null if the field is valid.</summary>
        public string GetError(string field)
        {
            ValidationError error = _errors.FirstOrDefault(e => e.Field == field);
            return error == null ? null : error.Message;
        }

        /// <summary>All messages, one per line.</summary>
        public override string ToString()
        {
            return string.Join(Environment.NewLine, _errors.Select(e => e.Message));
        }
    }
}
