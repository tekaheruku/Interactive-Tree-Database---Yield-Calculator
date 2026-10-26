using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TreeYield.Tests.Framework
{
    /// <summary>
    /// Finds every method marked [Test] or [IntegrationTest] in this assembly using
    /// reflection, runs it, and prints PASS or FAIL. A tiny stand-in for NUnit/MSTest
    /// so the solution builds in Visual Studio 2015 without downloading packages.
    /// </summary>
    public class TestRunner
    {
        private readonly bool _includeIntegrationTests;

        /// <summary>Creates the runner.</summary>
        /// <param name="includeIntegrationTests">False to skip tests that need the database.</param>
        public TestRunner(bool includeIntegrationTests)
        {
            _includeIntegrationTests = includeIntegrationTests;
        }

        /// <summary>Number of tests that passed in the last run.</summary>
        public int Passed { get; private set; }

        /// <summary>Number of tests that failed in the last run.</summary>
        public int Failed { get; private set; }

        /// <summary>Number of tests skipped in the last run.</summary>
        public int Skipped { get; private set; }

        /// <summary>Runs all tests and prints the results to the console.</summary>
        public void RunAll()
        {
            var tests = Discover();
            Console.WriteLine("Running {0} tests...", tests.Count);
            Console.WriteLine();

            foreach (var test in tests)
            {
                bool isIntegration = test.GetCustomAttribute<IntegrationTestAttribute>() != null;
                string name = test.DeclaringType.Name + "." + test.Name;

                if (isIntegration && !_includeIntegrationTests)
                {
                    Skipped++;
                    Write(ConsoleColor.DarkGray, "SKIP ", name);
                    continue;
                }

                var stopwatch = Stopwatch.StartNew();
                try
                {
                    object instance = Activator.CreateInstance(test.DeclaringType);
                    object returned = test.Invoke(instance, null);

                    // Async tests return a Task; wait for it so failures are caught here.
                    var task = returned as Task;
                    if (task != null)
                    {
                        task.GetAwaiter().GetResult();
                    }

                    Passed++;
                    Write(ConsoleColor.Green, "PASS ", string.Format("{0} ({1} ms)", name, stopwatch.ElapsedMilliseconds));
                }
                catch (Exception ex)
                {
                    // Reflection wraps exceptions thrown by the test in TargetInvocationException.
                    Exception actual = ex is TargetInvocationException && ex.InnerException != null ? ex.InnerException : ex;
                    Failed++;
                    Write(ConsoleColor.Red, "FAIL ", name);
                    Console.WriteLine("       " + actual.GetType().Name + ": " + actual.Message);
                }
            }

            Console.WriteLine();
            Write(Failed == 0 ? ConsoleColor.Green : ConsoleColor.Red, string.Empty,
                string.Format("{0} passed, {1} failed, {2} skipped.", Passed, Failed, Skipped));
        }

        private static List<MethodInfo> Discover()
        {
            // Classes in name order, methods in the order they are written in the file.
            return typeof(TestRunner).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .OrderBy(t => t.Name, StringComparer.Ordinal)
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m => m.GetCustomAttribute<TestAttribute>() != null || m.GetCustomAttribute<IntegrationTestAttribute>() != null)
                    .OrderBy(m => m.MetadataToken))
                .ToList();
        }

        private static void Write(ConsoleColor color, string prefix, string text)
        {
            ConsoleColor previous = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(prefix);
            Console.ForegroundColor = previous;
            Console.WriteLine(text);
        }
    }
}
