namespace TreeYield.Core.Models
{
    /// <summary>Outcome of a login attempt.</summary>
    public enum LoginStatus
    {
        /// <summary>Username and password were correct.</summary>
        Success,

        /// <summary>The username or password box was empty.</summary>
        MissingInput,

        /// <summary>Unknown username or wrong password (deliberately not told apart).</summary>
        InvalidCredentials,

        /// <summary>Too many failed attempts; the account is temporarily locked.</summary>
        LockedOut
    }

    /// <summary>Result returned by <see cref="Services.AuthService.LoginAsync"/>.</summary>
    public class LoginResult
    {
        private LoginResult(LoginStatus status, string message, AppUser user)
        {
            Status = status;
            Message = message;
            User = user;
        }

        /// <summary>What happened.</summary>
        public LoginStatus Status { get; private set; }

        /// <summary>Message to show the user. Null on success.</summary>
        public string Message { get; private set; }

        /// <summary>The signed-in user. Only set when <see cref="Status"/> is Success.</summary>
        public AppUser User { get; private set; }

        /// <summary>True when the login succeeded.</summary>
        public bool Succeeded
        {
            get { return Status == LoginStatus.Success; }
        }

        /// <summary>Creates a successful result.</summary>
        public static LoginResult Success(AppUser user)
        {
            return new LoginResult(LoginStatus.Success, null, user);
        }

        /// <summary>Creates a failed result with a message for the user.</summary>
        public static LoginResult Fail(LoginStatus status, string message)
        {
            return new LoginResult(status, message, null);
        }
    }
}
