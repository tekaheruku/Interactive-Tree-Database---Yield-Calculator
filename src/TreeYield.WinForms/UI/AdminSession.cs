using System;
using TreeYield.Core.Models;

namespace TreeYield.WinForms.UI
{
    /// <summary>
    /// Remembers which administrator is signed in. The session ends automatically after
    /// <see cref="IdleTimeout"/> without admin activity, so an unattended PC does not
    /// stay open for editing.
    /// </summary>
    internal sealed class AdminSession
    {
        /// <summary>How long an unused session stays valid.</summary>
        public static readonly TimeSpan IdleTimeout = TimeSpan.FromMinutes(15);

        private DateTime _lastActivityUtc;

        /// <summary>The signed-in administrator, or null for a guest.</summary>
        public AppUser CurrentUser { get; private set; }

        /// <summary>True when an administrator is signed in and the session has not expired.</summary>
        public bool IsSignedIn
        {
            get { return CurrentUser != null && !IsExpired; }
        }

        /// <summary>True when someone signed in but has been idle longer than <see cref="IdleTimeout"/>.</summary>
        public bool IsExpired
        {
            get { return CurrentUser != null && DateTime.UtcNow - _lastActivityUtc > IdleTimeout; }
        }

        /// <summary>Starts a session.</summary>
        public void SignIn(AppUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            CurrentUser = user;
            Touch();
        }

        /// <summary>Records admin activity, restarting the idle timer.</summary>
        public void Touch()
        {
            _lastActivityUtc = DateTime.UtcNow;
        }

        /// <summary>Ends the session.</summary>
        public void SignOut()
        {
            CurrentUser = null;
        }
    }
}
