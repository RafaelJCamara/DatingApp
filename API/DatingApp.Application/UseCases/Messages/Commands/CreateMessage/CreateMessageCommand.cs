using DatingApp.Application.Dtos;
using MediatR;

namespace DatingApp.Application.UseCases.Messages.Commands.CreateMessage
{
    public sealed record CreateMessageCommand(CreateMessageDto MessageToCreate) : IRequest<(string?, MessageDto?)>;
}
