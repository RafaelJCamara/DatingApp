using DatingApp.Application.Dtos;
using MediatR;

namespace DatingApp.Application.UseCases.Users.Commands.UpdateUser
{
    public sealed record UpdateUserCommand(UserUpdateDto UserToUpdate) : IRequest<(bool, string?)>;
}
