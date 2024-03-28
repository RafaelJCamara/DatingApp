using DatingApp.Application.Dtos;
using DatingApp.Domain.Common.Response;
using MediatR;

namespace DatingApp.Application.UseCases.Account.Commands.Register;

public sealed record RegisterCommand(RegisterDto RegisterInformation) : IRequest<Result<UserDto?>>;
