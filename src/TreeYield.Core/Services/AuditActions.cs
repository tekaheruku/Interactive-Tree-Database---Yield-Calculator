namespace TreeYield.Core.Services
{
    /// <summary>Action codes written to the AuditLog table.</summary>
    public static class AuditActions
    {
        /// <summary>Successful administrator login.</summary>
        public const string Login = "LOGIN";

        /// <summary>Failed login (wrong password or unknown username).</summary>
        public const string LoginFailed = "LOGIN_FAILED";

        /// <summary>Account locked after too many failed logins.</summary>
        public const string Lockout = "LOCKOUT";

        /// <summary>Administrator signed out.</summary>
        public const string Logout = "LOGOUT";

        /// <summary>Password changed.</summary>
        public const string PasswordChange = "PASSWORD_CHANGE";

        /// <summary>A record was added.</summary>
        public const string Create = "CREATE";

        /// <summary>A record was edited.</summary>
        public const string Update = "UPDATE";

        /// <summary>A record was deleted.</summary>
        public const string Delete = "DELETE";

        /// <summary>The sample species data was restored.</summary>
        public const string ResetData = "RESET_DATA";
    }
}
