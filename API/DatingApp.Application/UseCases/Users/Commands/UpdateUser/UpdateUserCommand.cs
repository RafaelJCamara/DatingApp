using DatingApp.Application.Dtos;
using DatingApp.Application.UseCases.Users.Common;
using DatingApp.Domain.Common.Response;
using MediatR;

namespace DatingApp.Application.UseCases.Users.Commands.UpdateUser
{
    public sealed record UpdateUserCommand(UserUpdateDto UserToUpdate) : IRequest<Result>, ILoggableCommand;
}
