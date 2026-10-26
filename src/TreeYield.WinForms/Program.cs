using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;
using TreeYield.WinForms.Forms;
using TreeYield.WinForms.UI;

namespace TreeYield.WinForms
{
    /// <summary>Application entry point.</summary>
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Last line of defense: any error that no other code caught is logged and
            // shown as a friendly message instead of crashing the program.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, e) => ErrorReporter.HandleUnexpected(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => ErrorReporter.Log(e.ExceptionObject as Exception);
            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                ErrorReporter.Log(e.Exception);
                e.SetObserved();
            };

            AppServices services;
            try
            {
                services = AppServices.FromConfiguration();
            }
            catch (Exception ex) when (ex is ConfigurationErrorsException || ex is ArgumentException)
            {
                ErrorReporter.Log(ex);
                MessageBox.Show(
                    "The program settings could not be read.\n\n" + ex.Message +
                    "\n\nCheck the TreeYieldDb connection string in TreeYield.WinForms.exe.config.",
                    "Tree Yield & Revenue Calculator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new MainForm(services));
        }
    }
}
