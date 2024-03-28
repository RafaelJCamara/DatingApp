using DatingApp.Application.Dtos;
using MediatR;

namespace DatingApp.Application.UseCases.Account.Commands.Register;

public sealed record RegisterCommand(RegisterDto RegisterInformation) : IRequest<(UserDto?, object?)>;
