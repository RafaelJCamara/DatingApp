
using DatingApp.Domain.Common.Errors;

namespace DatingApp.Domain.Errors.Account
{
    public static class AccountErrors
    {
        public static Error UserNotFound(string username) => new("Account.UserNotFound", $"The user with username {username} was not found.");

        public static Error LoginFailed => new("Account.LoginFailed", "Username and password don't match.");

        public static Error UsernameAlreadyTaken(string username) => new("Account.UsernameAlreadyTaken", $"The username {username} is already taken.");

        public static Error RegistrationFailed(string[] registrationErrors)
            => new("Account.RegistrationFailed", $"The following registration errors happened: {string.Join(", ", registrationErrors)}");
    }
}
