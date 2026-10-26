using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using TreeYield.Core.Data;
using TreeYield.Core.Services;

namespace TreeYield.WinForms.UI
{
    /// <summary>
    /// Central error handling for the UI: writes full technical details to a log file
    /// and shows the user a short, friendly message.
    /// </summary>
    internal static class ErrorReporter
    {
        private const string Caption = "Tree Yield & Revenue Calculator";

        /// <summary>Folder for log files: %LOCALAPPDATA%\TreeYieldCalculator\logs.</summary>
        public static string LogDirectory
        {
            get
            {
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "TreeYieldCalculator", "logs");
            }
        }

        /// <summary>Appends an exception to today's log file. Never throws.</summary>
        public static void Log(Exception exception)
        {
            if (exception == null)
            {
                return;
            }

            try
            {
                Directory.CreateDirectory(LogDirectory);
                string file = Path.Combine(LogDirectory, "app-" + DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + ".log");
                string entry = string.Format(CultureInfo.InvariantCulture, "[{0:yyyy-MM-dd HH:mm:ss}] {1}{2}{2}",
                    DateTime.Now, exception, Environment.NewLine);
                File.AppendAllText(file, entry);
            }
            catch (Exception)
            {
                // Logging must never cause a second error (for example when the disk is full).
            }
        }

        /// <summary>
        /// Returns a message that is safe and useful to show. Our own exceptions already
        /// carry friendly text; anything else is described generically.
        /// </summary>
        public static string GetFriendlyMessage(Exception exception)
        {
            if (exception is DataAccessException || exception is ValidationException
                || exception is UnauthorizedAccessException || exception is IOException)
            {
                return exception.Message;
            }

            return "An unexpected error occurred. Technical details were saved to the log file in:\n" + LogDirectory;
        }

        /// <summary>Logs an error and shows "Could not {action}." with the friendly message.</summary>
        /// <param name="owner">Window that owns the message box.</param>
        /// <param name="action">What was being done, for example "save the species".</param>
        /// <param name="exception">The error.</param>
        public static void ShowError(IWin32Window owner, string action, Exception exception)
        {
            Log(exception);
            MessageBox.Show(owner, "Could not " + action + ".\n\n" + GetFriendlyMessage(exception),
                Caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>Called by the global handler in Program.cs for errors nobody caught.</summary>
        public static void HandleUnexpected(Exception exception)
        {
            Log(exception);
            MessageBox.Show(
                "Something went wrong, but the program can keep running.\n\n" + GetFriendlyMessage(exception),
                Caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>Shows an information message.</summary>
        public static void ShowInfo(IWin32Window owner, string message)
        {
            MessageBox.Show(owner, message, Caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>Asks a Yes/No question with "No" as the safe default. Returns true for Yes.</summary>
        public static bool Confirm(IWin32Window owner, string message, string title)
        {
            return MessageBox.Show(owner, message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }
    }
}
