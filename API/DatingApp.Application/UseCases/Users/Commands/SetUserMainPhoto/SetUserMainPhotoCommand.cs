﻿using DatingApp.Application.UseCases.Users.Common;
using MediatR;

namespace DatingApp.Application.UseCases.Users.Commands.SetUserMainPhoto
{
    public sealed record SetUserMainPhotoCommand(int PhotoId) : IRequest<(bool, string?)>, ILoggableCommand;
}
