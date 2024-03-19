using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Interfaces.Services;
using MediatR;

namespace DatingApp.Application.UseCases.Users.Commands.DeleteUserPhoto
{
    public sealed class DeleteUserPhotoCommandHandler : IRequestHandler<DeleteUserPhotoCommand, (bool, string?)>
    {
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUser _currentUser;

        public DeleteUserPhotoCommandHandler(IPhotoService photoService, IUnitOfWork unitOfWork, IUser currentUser)
        {
            _photoService = photoService;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<(bool, string?)> Handle(DeleteUserPhotoCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(_currentUser.Username);

            var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);

            if (photo == null) return (false, "Photo not found.");

            if (photo.IsMain) return (false, "You can't delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return (false, result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await _unitOfWork.Complete()) return (true, null);

            return (false, "Problem deleting photo");
        }
    }
}
