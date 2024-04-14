using DatingApp.Application.Common.Interfaces;
using DatingApp.Common.Helpers.User;
using DatingApp.Domain.Models;
using MediatR;

namespace DatingApp.Application.UseCases.Likes.Commands.AddLike
{
    public sealed class AddLikeCommandHandler : IRequestHandler<AddLikeCommand, string?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUser _currentUser;

        public AddLikeCommandHandler(IUnitOfWork unitOfWork, IUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<string?> Handle(AddLikeCommand request, CancellationToken cancellationToken)
        {
            var sourceUserId = _currentUser.Id;
            var likedUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(request.TargetUsername);
            var sourceUser = await _unitOfWork.LikesRepository.GetUserWithLikes(sourceUserId.Value);

            if (likedUser == null) return "Target user not found";

            if (sourceUser.UserName == request.TargetUsername) return "You cannot like yourself";

            var userLike = await _unitOfWork.LikesRepository.GetUserLike(sourceUserId.Value, likedUser.Id);

            if (userLike != null) return "You already like this user";

            userLike = new UserLike
            {
                SourceUserId = sourceUserId.Value,
                TargetUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);

            if (await _unitOfWork.Complete()) return null;

            return "Failed to like user";
        }
    }
}
