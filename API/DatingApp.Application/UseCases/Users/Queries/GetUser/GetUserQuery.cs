using DatingApp.Application.Dtos;
using DatingApp.Application.UseCases.Users.Common;
using DatingApp.Domain.Common.Response;
using MediatR;

namespace DatingApp.Application.UseCases.Users.Queries.GetUser
{
    public sealed record GetUserQuery(string Username) : IRequest<Result<MemberDto?>>, ILoggableCommand;
}
