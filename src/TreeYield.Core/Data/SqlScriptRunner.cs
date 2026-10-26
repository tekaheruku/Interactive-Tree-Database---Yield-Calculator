using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TreeYield.Core.Data
{
    /// <summary>Loads the .sql files embedded in this assembly and runs them batch by batch.</summary>
    public static class SqlScriptRunner
    {
        // "GO" is not a T-SQL statement: tools like SSMS and sqlcmd use it to split a script
        // into batches. SqlCommand does not understand it, so the script is split here on
        // lines that contain only GO (optionally followed by a -- comment). The group is
        // non-capturing because Regex.Split would otherwise return the comment as a batch.
        // [ \t] is used instead of \s so the pattern never runs across line breaks.
        private static readonly Regex BatchSeparator = new Regex(@"^[ \t]*GO[ \t]*(?:--[^\r\n]*)?\r?$",
            RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        /// <summary>Splits a script into batches on GO lines and drops batches that are empty or only comments.</summary>
        public static IList<string> SplitBatches(string script)
        {
            if (string.IsNullOrWhiteSpace(script))
            {
                return new List<string>();
            }

            return BatchSeparator.Split(script)
                .Select(batch => batch.Trim())
                .Where(ContainsCode)
                .ToList();
        }

        /// <summary>Reads an embedded script, for example "001_Schema.sql".</summary>
        public static string LoadEmbeddedScript(string fileName)
        {
            Assembly assembly = typeof(SqlScriptRunner).Assembly;

            // Resource names look like "TreeYield.Core.Data.Scripts.001_Schema.sql".
            string resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(name => name.EndsWith("." + fileName, StringComparison.OrdinalIgnoreCase));

            if (resourceName == null)
            {
                throw new InvalidOperationException("The embedded SQL script '" + fileName + "' was not found.");
            }

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>Runs every batch of a script on an open connection inside the given transaction.</summary>
        public static async Task ExecuteAsync(SqlConnection connection, SqlTransaction transaction, string script)
        {
            foreach (string batch in SplitBatches(script))
            {
                using (var command = new SqlCommand(batch, connection, transaction))
                {
                    command.CommandTimeout = 120;
                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
            }
        }

        /// <summary>True when a batch has at least one line that is not a comment.</summary>
        private static bool ContainsCode(string batch)
        {
            return batch
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Trim())
                .Any(line => line.Length > 0 && !line.StartsWith("--", StringComparison.Ordinal));
        }
    }
}
