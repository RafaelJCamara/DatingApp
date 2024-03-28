using DatingApp.Domain.Common.Errors;

namespace DatingApp.Domain.Errors.Admin
{
    public static class AdminErrors
    {
        public static Error NoRolesSelected = new("Admin.NoRolesSelected", "You must select at least one role.");

        public static Error UserNotFound(string username)
            => new("Admin.UserNotFound", $"The user with username {username} was not found.");

        public static Error FailToAddRolesToUser(string username, string[] newRoles)
            => new("Admin.FailToAddRolesToUser", $"Failed to add roles {string.Join(", ", newRoles)} to user {username}.");

        public static Error FailToRemoveRolesFromUser(string username, string[] rolesToDelete)
            => new("Admin.FailToAddRolesToUser", $"Failed to delete roles {string.Join(", ", rolesToDelete)} from user {username}.");
    }
}
