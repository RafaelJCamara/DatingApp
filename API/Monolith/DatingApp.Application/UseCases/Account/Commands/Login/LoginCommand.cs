using DatingApp.Application.Dtos;
using MediatR;

namespace DatingApp.Application.UseCases.Account.Commands.Login;

public sealed record LoginCommand(LoginDto LoginCredentials) : IRequest<UserDto?>;
