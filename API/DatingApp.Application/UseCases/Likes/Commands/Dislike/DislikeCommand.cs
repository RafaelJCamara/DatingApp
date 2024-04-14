using MediatR;

namespace DatingApp.Application.UseCases.Likes.Commands.Dislike
{
    public sealed record DislikeCommand(string TargetUsername) : IRequest<string?>;
}
