using System;

namespace TreeYield.Tests.Framework
{
    /// <summary>
    /// Marks a public method as a unit test (no database needed). The method must take
    /// no parameters and return void or Task.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TestAttribute : Attribute
    {
    }

    /// <summary>
    /// Marks a test that needs SQL Server LocalDB. Skipped when the runner is started
    /// with --unit-only.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class IntegrationTestAttribute : Attribute
    {
    }
}
