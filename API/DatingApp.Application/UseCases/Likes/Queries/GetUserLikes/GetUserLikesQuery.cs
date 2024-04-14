using DatingApp.Application.Common.Models;
using DatingApp.Application.Dtos;
using MediatR;

namespace DatingApp.Application.UseCases.Likes.Queries.GetUserLikes
{
    public sealed record GetUserLikesQuery(LikesParamsDto LikesParams) : IRequest<PagedList<LikeDto>>;
}
