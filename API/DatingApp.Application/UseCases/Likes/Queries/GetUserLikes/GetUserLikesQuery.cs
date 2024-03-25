using DatingApp.Application.Common.Models;
using DatingApp.Application.Dtos;
using DatingApp.Domain.Common.Response;
using MediatR;

namespace DatingApp.Application.UseCases.Likes.Queries.GetUserLikes
{
    public sealed record GetUserLikesQuery(LikesParamsDto LikesParams) : IRequest<Result<PagedList<LikeDto>>>;
}
