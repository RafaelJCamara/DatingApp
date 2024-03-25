using DatingApp.Application.Common.Interfaces;
using DatingApp.Domain.Common.Response;
using DatingApp.Domain.Errors.Likes;
using MediatR;

namespace DatingApp.Application.UseCases.Likes.Commands.Dislike
{
    public sealed class DislikeCommandHandler : IRequestHandler<DislikeCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUser _currentUser;

        public DislikeCommandHandler(IUnitOfWork unitOfWork, IUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(DislikeCommand request, CancellationToken cancellationToken)
        {
            var sourceUserId = _currentUser.Id;
            var disliked = await _unitOfWork.UserRepository.GetUserByUsernameAsync(request.TargetUsername);
            var sourceUser = await _unitOfWork.LikesRepository.GetUserWithLikes(sourceUserId.Value);

            if (disliked is null) return Result.Failure(LikeErrors.TargetUserNotFound(request.TargetUsername));

            if (sourceUser.UserName == request.TargetUsername) return Result.Failure(LikeErrors.TargetDislikeUserIsSelf);

            var userLike = await _unitOfWork.LikesRepository.GetUserLike(sourceUserId.Value, disliked.Id);

            if (userLike is null) return Result.Failure(LikeErrors.TargetUserHasNotBeenLikedBeforeDislike);

            sourceUser.LikedUsers.Remove(userLike);

            if (await _unitOfWork.Complete()) return Result.Success();

            return Result.Failure(LikeErrors.DislikeFailed(request.TargetUsername));
        }
    }
}
