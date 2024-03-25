using DatingApp.Application.UseCases.Admin.Queries.GetUsersWithRoles.Dto;
using DatingApp.Domain.Common.Response;
using MediatR;

namespace DatingApp.Application.UseCases.Admin.Queries.GetUsersWithRoles
{
    public sealed record GetUsersWithRolesQuery() : IRequest<Result<IEnumerable<UserDto>>>;
}
