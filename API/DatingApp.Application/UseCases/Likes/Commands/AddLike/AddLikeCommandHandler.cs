using DatingApp.Application.Common.Interfaces;
using DatingApp.Domain.Common.Response;
using DatingApp.Domain.Errors.Likes;
using DatingApp.Domain.Models;
using MediatR;

namespace DatingApp.Application.UseCases.Likes.Commands.AddLike
{
    public sealed class AddLikeCommandHandler : IRequestHandler<AddLikeCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUser _currentUser;

        public AddLikeCommandHandler(IUnitOfWork unitOfWork, IUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result> Handle(AddLikeCommand request, CancellationToken cancellationToken)
        {
            var sourceUserId = _currentUser.Id;
            var likedUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(request.TargetUsername);
            var sourceUser = await _unitOfWork.LikesRepository.GetUserWithLikes(sourceUserId.Value);

            if (likedUser is null) return Result.Failure(LikeErrors.TargetUserNotFound(request.TargetUsername));

            if (sourceUser.UserName == request.TargetUsername) return Result.Failure(LikeErrors.TargetLikeUserIsSelf);

            var userLike = await _unitOfWork.LikesRepository.GetUserLike(sourceUserId.Value, likedUser.Id);

            if (userLike is not null) return Result.Failure(LikeErrors.TargetUserAlreadyLiked(request.TargetUsername));

            userLike = new UserLike
            {
                SourceUserId = sourceUserId.Value,
                TargetUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);

            if (await _unitOfWork.Complete()) return Result.Success();

            return Result.Failure(LikeErrors.LikeFailed(request.TargetUsername));
        }
    }
}
