using DatingApp.Application.UseCases.Admin.Queries.GetUsersWithRoles.Dto;
using DatingApp.Domain.Common.Response;
using DatingApp.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Application.UseCases.Admin.Queries.GetUsersWithRoles
{
    public class GetUsersWithRolesQueryHandler : IRequestHandler<GetUsersWithRolesQuery, Result<IEnumerable<UserDto>>>
    {
        private readonly UserManager<AppUser> _userManager;

        public GetUsersWithRolesQueryHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<IEnumerable<UserDto>>> Handle(GetUsersWithRolesQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users
             .OrderBy(u => u.UserName)
             .Select(u => new UserDto
             {
                 Id = u.Id,
                 Username = u.UserName,
                 Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
             })
             .ToListAsync();
             
            return Result<IEnumerable<UserDto>>.Success(users);
        }
    }
}
