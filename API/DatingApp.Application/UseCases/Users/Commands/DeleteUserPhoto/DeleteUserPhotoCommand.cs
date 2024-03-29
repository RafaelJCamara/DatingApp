﻿using DatingApp.Application.UseCases.Users.Common;
using MediatR;

namespace DatingApp.Application.UseCases.Users.Commands.DeleteUserPhoto
{
    public sealed record DeleteUserPhotoCommand(int PhotoId) : IRequest<(bool, string?)>, ILoggableCommand;
}
