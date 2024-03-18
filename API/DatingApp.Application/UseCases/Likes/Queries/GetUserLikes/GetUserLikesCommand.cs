using DatingApp.Application.Common.Models;
using DatingApp.Application.Dtos;
using MediatR;

namespace DatingApp.Application.UseCases.Likes.Queries.GetUserLikes
{
    public sealed record GetUserLikesCommand(LikesParamsDto LikesParams) : IRequest<PagedList<LikeDto>>;
}
