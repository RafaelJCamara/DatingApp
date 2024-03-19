using DatingApp.Application.Dtos;
using DatingApp.Application.UseCases.Users.Common;
using MediatR;

namespace DatingApp.Application.UseCases.Users.Commands.UpdateUser
{
    public sealed record UpdateUserCommand(UserUpdateDto UserToUpdate) : IRequest<(bool, string?)>, ILoggableCommand;
}
