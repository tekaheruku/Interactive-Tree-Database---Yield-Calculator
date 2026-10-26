using System;
using System.Threading.Tasks;

namespace TreeYield.Tests.Framework
{
    /// <summary>Raised by <see cref="Assert"/> when a check fails.</summary>
    public class AssertionException : Exception
    {
        /// <summary>Creates the exception.</summary>
        public AssertionException(string message)
            : base(message)
        {
        }
    }

    /// <summary>Minimal assertion helpers, so the tests need no external framework (no NuGet).</summary>
    public static class Assert
    {
        /// <summary>Fails unless the two values are equal.</summary>
        public static void AreEqual<T>(T expected, T actual, string message = null)
        {
            if (!Equals(expected, actual))
            {
                Fail(string.Format("Expected <{0}> but was <{1}>. {2}", expected, actual, message));
            }
        }

        /// <summary>Fails unless the condition is true.</summary>
        public static void IsTrue(bool condition, string message = null)
        {
            if (!condition)
            {
                Fail("Expected true. " + message);
            }
        }

        /// <summary>Fails unless the condition is false.</summary>
        public static void IsFalse(bool condition, string message = null)
        {
            if (condition)
            {
                Fail("Expected false. " + message);
            }
        }

        /// <summary>Fails unless the value is null.</summary>
        public static void IsNull(object value, string message = null)
        {
            if (value != null)
            {
                Fail(string.Format("Expected null but was <{0}>. {1}", value, message));
            }
        }

        /// <summary>Fails if the value is null.</summary>
        public static void IsNotNull(object value, string message = null)
        {
            if (value == null)
            {
                Fail("Expected a value but was null. " + message);
            }
        }

        /// <summary>Fails unless the action throws <typeparamref name="TException"/> (or a subclass).</summary>
        public static TException Throws<TException>(Action action) where TException : Exception
        {
            try
            {
                action();
            }
            catch (TException ex)
            {
                return ex;
            }
            catch (Exception ex)
            {
                Fail(string.Format("Expected {0} but {1} was thrown: {2}", typeof(TException).Name, ex.GetType().Name, ex.Message));
            }

            Fail(string.Format("Expected {0} but nothing was thrown.", typeof(TException).Name));
            return null;
        }

        /// <summary>Async version of <see cref="Throws{TException}"/>.</summary>
        public static async Task<TException> ThrowsAsync<TException>(Func<Task> action) where TException : Exception
        {
            try
            {
                await action();
            }
            catch (TException ex)
            {
                return ex;
            }
            catch (Exception ex)
            {
                Fail(string.Format("Expected {0} but {1} was thrown: {2}", typeof(TException).Name, ex.GetType().Name, ex.Message));
            }

            Fail(string.Format("Expected {0} but nothing was thrown.", typeof(TException).Name));
            return null;
        }

        /// <summary>Fails the test with a message.</summary>
        public static void Fail(string message)
        {
            throw new AssertionException(message);
        }
    }
}
