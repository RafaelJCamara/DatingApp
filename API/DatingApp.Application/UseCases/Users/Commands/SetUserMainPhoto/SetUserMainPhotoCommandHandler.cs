using DatingApp.Application.Common.Interfaces;
using DatingApp.Common.Helpers.User;
using MediatR;

namespace DatingApp.Application.UseCases.Users.Commands.SetUserMainPhoto
{
    public sealed class SetUserMainPhotoCommandHandler : IRequestHandler<SetUserMainPhotoCommand, (bool, string?)>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUser _currentUser;

        public SetUserMainPhotoCommandHandler(IUnitOfWork unitOfWork, IUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<(bool, string?)> Handle(SetUserMainPhotoCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(_currentUser.Username);

            if (user == null) return (false, "User not found.");

            var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);

            if (photo == null) return (false, "Photo not found.");

            if (photo.IsMain) return (false, "This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _unitOfWork.Complete()) return (true, null);

            return (false, "Problem setting the main photo");
        }
    }
}
