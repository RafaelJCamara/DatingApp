using DatingApp.Application.Common.Models;
using DatingApp.Application.Dtos;
using MediatR;

namespace DatingApp.Application.UseCases.Users.Queries.GetPaginatedUsers
{
    public sealed record GetPaginatedUsersQuery(UserParamsDto UserParams) : IRequest<PagedList<MemberDto>>;
}
