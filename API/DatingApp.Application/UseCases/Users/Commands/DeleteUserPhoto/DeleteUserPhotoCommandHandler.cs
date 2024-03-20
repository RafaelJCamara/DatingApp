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

            var publicId = user.RemovePhoto(request.PhotoId);

            if (publicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(publicId);
                if (result.Error != null) return (false, result.Error.Message);
            }

            if (await _unitOfWork.Complete()) return (true, null);

            return (false, "Problem deleting photo");
        }
    }
}
