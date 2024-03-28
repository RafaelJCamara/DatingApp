using DatingApp.Application.Dtos;
using DatingApp.Domain.Common.Response;
using MediatR;

namespace DatingApp.Application.UseCases.Account.Commands.Login;

public sealed record LoginCommand(LoginDto LoginCredentials) : IRequest<Result<UserDto?>>;
