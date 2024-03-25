using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Common.Models;
using DatingApp.Application.Dtos;
using DatingApp.Domain.Common.Response;
using MediatR;

namespace DatingApp.Application.UseCases.Likes.Queries.GetUserLikes
{
    public sealed class GetUserLikesQueryHandler : IRequestHandler<GetUserLikesQuery, Result<PagedList<LikeDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUser _currentUser;

        public GetUserLikesQueryHandler(IUnitOfWork unitOfWork, IUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<PagedList<LikeDto>>> Handle(GetUserLikesQuery request, CancellationToken cancellationToken)
        {
            request.LikesParams.UserId = _currentUser.Id.Value;
  
            return Result<PagedList<LikeDto>>.Success(await _unitOfWork.LikesRepository.GetUserLikes(request.LikesParams));
        }
    }
}
