using DatingApp.Application.Dtos;
using MediatR;

namespace DatingApp.Application.UseCases.Users.Queries.GetUser
{
    public sealed record GetUserQuery(string Username) : IRequest<MemberDto?>;
}
