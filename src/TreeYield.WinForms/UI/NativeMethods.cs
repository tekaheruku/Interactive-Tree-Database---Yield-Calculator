using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TreeYield.WinForms.UI
{
    /// <summary>Small Windows API calls that .NET Framework 4.5.2 does not wrap.</summary>
    internal static class NativeMethods
    {
        // Edit-control message that shows gray "placeholder" text in an empty TextBox.
        private const int EM_SETCUEBANNER = 0x1501;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        /// <summary>
        /// Shows placeholder text in a single-line TextBox while it is empty
        /// (like the HTML placeholder attribute). Works once the handle exists.
        /// </summary>
        public static void SetCueBanner(TextBox textBox, string text)
        {
            if (textBox.IsHandleCreated)
            {
                // wParam = 1 keeps the hint visible even while the box has focus.
                SendMessage(textBox.Handle, EM_SETCUEBANNER, new IntPtr(1), text);
            }
            else
            {
                textBox.HandleCreated += (sender, e) => SetCueBanner(textBox, text);
            }
        }
    }
}
