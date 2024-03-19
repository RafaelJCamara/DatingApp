using DatingApp.Application.UseCases.Admin.Queries.GetUsersWithRoles.Dto;
using MediatR;

namespace DatingApp.Application.UseCases.Admin.Queries.GetUsersWithRoles
{
    public sealed record GetUsersWithRolesQuery() : IRequest<List<UserDto>>;
}
