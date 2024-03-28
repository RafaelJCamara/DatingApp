using DatingApp.Domain.Common.Response;
using DatingApp.Domain.Errors.Admin;
using DatingApp.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.Application.UseCases.Admin.Commands.EditRoles
{
    public sealed class EditRolesCommanHandler : IRequestHandler<EditRolesCommand, Result<List<string>?>>
    {
        private readonly UserManager<AppUser> _userManager;

        public EditRolesCommanHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<List<string>?>> Handle(EditRolesCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Roles)) return Result<List<string>?>.Failure(AdminErrors.NoRolesSelected);

            var selectedRoles = request.Roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(request.Username);

            if (user is null) return Result<List<string>?>.Failure(AdminErrors.UserNotFound(request.Username));

            var userRoles = await _userManager.GetRolesAsync(user);

            var newRoles = selectedRoles.Except(userRoles);

            var result = await _userManager.AddToRolesAsync(user, newRoles);

            if (!result.Succeeded) return Result<List<string>?>.Failure(AdminErrors.FailToAddRolesToUser(request.Username, newRoles.ToArray()));

            var rolesToDelete = userRoles.Except(selectedRoles);

            result = await _userManager.RemoveFromRolesAsync(user, rolesToDelete);

            if (!result.Succeeded) return Result<List<string>?>.Failure(AdminErrors.FailToRemoveRolesFromUser(request.Username, rolesToDelete.ToArray()));

            return Result<List<string>?>.Success((await _userManager.GetRolesAsync(user)).ToList());
        }
    }
}
