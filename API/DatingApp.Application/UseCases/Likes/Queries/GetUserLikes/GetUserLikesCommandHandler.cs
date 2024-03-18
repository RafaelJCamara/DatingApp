using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Common.Models;
using DatingApp.Application.Dtos;
using MediatR;

namespace DatingApp.Application.UseCases.Likes.Queries.GetUserLikes
{
    public sealed class GetUserLikesCommandHandler : IRequestHandler<GetUserLikesCommand, PagedList<LikeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUser _currentUser;

        public GetUserLikesCommandHandler(IUnitOfWork unitOfWork, IUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<PagedList<LikeDto>> Handle(GetUserLikesCommand request, CancellationToken cancellationToken)
        {
            request.LikesParams.UserId = _currentUser.Id.Value;
  
            return await _unitOfWork.LikesRepository.GetUserLikes(request.LikesParams);
        }
    }
}
