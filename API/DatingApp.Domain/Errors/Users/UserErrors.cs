using DatingApp.Domain.Common.Errors;

namespace DatingApp.Domain.Errors.Users
{
    public static class UserErrors
    {
        public static Error UserUpdateFailed(string username) => new("User.UserUpdateFailed", $"Failed to update the user with username {username}.");

        public static Error UserNotFound(string username) => new("User.UserNotFound", $"The user with username {username} was not found.");
    }
}
