using DatingApp.Application.Dtos;
using DatingApp.Application.UseCases.Users.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Application.UseCases.Users.Commands.AddPhotoToUser
{
    public sealed record AddPhotoToUserCommand(IFormFile NewUserPhoto) : IRequest<(PhotoDto?, string?)>, ILoggableCommand;
}
