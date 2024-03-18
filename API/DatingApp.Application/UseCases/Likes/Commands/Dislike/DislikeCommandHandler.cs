using DatingApp.Application.Common.Interfaces;
using MediatR;

namespace DatingApp.Application.UseCases.Likes.Commands.Dislike
{
    public sealed class DislikeCommandHandler : IRequestHandler<DislikeCommand, string?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUser _currentUser;

        public DislikeCommandHandler(IUnitOfWork unitOfWork, IUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<string?> Handle(DislikeCommand request, CancellationToken cancellationToken)
        {
            var sourceUserId = _currentUser.Id;
            var disliked = await _unitOfWork.UserRepository.GetUserByUsernameAsync(request.TargetUsername);
            var sourceUser = await _unitOfWork.LikesRepository.GetUserWithLikes(sourceUserId.Value);

            if (disliked == null) return "Disliked user not found.";

            if (sourceUser.UserName == request.TargetUsername) return "You cannot dislike yourself";

            var userLike = await _unitOfWork.LikesRepository.GetUserLike(sourceUserId.Value, disliked.Id);

            if (userLike == null) return "You can't dislike this user";

            sourceUser.LikedUsers.Remove(userLike);

            if (await _unitOfWork.Complete()) return null;

            return "Failed to dislike user";
        }
    }
}
