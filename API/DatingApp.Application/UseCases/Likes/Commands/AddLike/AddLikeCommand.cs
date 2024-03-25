using DatingApp.Domain.Common.Response;
using MediatR;

namespace DatingApp.Application.UseCases.Likes.Commands.AddLike
{
    public sealed record AddLikeCommand(string TargetUsername) : IRequest<Result>;
}
