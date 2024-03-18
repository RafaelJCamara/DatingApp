using MediatR;

namespace DatingApp.Application.UseCases.Messages.Commands.DeleteMessage
{
    public sealed record DeleteMessageCommand(int MessageId) : IRequest<string?>;
}
