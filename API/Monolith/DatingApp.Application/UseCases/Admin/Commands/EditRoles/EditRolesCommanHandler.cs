using DatingApp.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.Application.UseCases.Admin.Commands.EditRoles
{
    public sealed class EditRolesCommanHandler : IRequestHandler<EditRolesCommand, (string?, List<string>?)>
    {
        private readonly UserManager<AppUser> _userManager;

        public EditRolesCommanHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<(string?, List<string>?)> Handle(EditRolesCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Roles)) return ("You must select at least one role", null);

            var selectedRoles = request.Roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(request.Username);

            if (user is null) return ("User not found", null);

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return ("Failed to add to roles", null);

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return ("Failed to remove from roles", null);

            return (null, (await _userManager.GetRolesAsync(user)).ToList());
        }
    }
}
