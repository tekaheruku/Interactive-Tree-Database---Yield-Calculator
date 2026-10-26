using System;
using System.Linq;
using TreeYield.Tests.Framework;

namespace TreeYield.Tests
{
    /// <summary>
    /// Entry point of the test runner.
    /// <para>Usage: TreeYield.Tests.exe [--unit-only] [--no-pause]</para>
    /// The exit code is 0 when every test passed and 1 otherwise.
    /// </summary>
    internal static class Program
    {
        private static int Main(string[] args)
        {
            bool unitOnly = args.Contains("--unit-only", StringComparer.OrdinalIgnoreCase);
            bool noPause = args.Contains("--no-pause", StringComparer.OrdinalIgnoreCase);

            Console.WriteLine("Interactive Tree Database & Yield Calculator - automated tests");
            Console.WriteLine(unitOnly ? "Mode: unit tests only" : "Mode: unit + database integration tests (LocalDB)");
            Console.WriteLine();

            var runner = new TestRunner(!unitOnly);
            try
            {
                runner.RunAll();
            }
            finally
            {
                // Remove the throw-away test database so it does not linger in LocalDB.
                TestDatabase.DropIfCreated();
            }

            // Keep the console window open when started with F5 from Visual Studio.
            if (!noPause && Environment.UserInteractive && !Console.IsInputRedirected)
            {
                Console.WriteLine();
                Console.WriteLine("Press any key to close...");
                Console.ReadKey(true);
            }

            return runner.Failed == 0 ? 0 : 1;
        }
    }
}
