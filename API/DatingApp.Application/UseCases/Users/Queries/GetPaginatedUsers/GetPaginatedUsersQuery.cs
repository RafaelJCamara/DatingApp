using DatingApp.Application.Common.Models;
using DatingApp.Application.Dtos;
using DatingApp.Application.UseCases.Users.Common;
using MediatR;

namespace DatingApp.Application.UseCases.Users.Queries.GetPaginatedUsers
{
    public sealed record GetPaginatedUsersQuery(UserParamsDto UserParams) : IRequest<PagedList<MemberDto>>, ILoggableCommand;
}
