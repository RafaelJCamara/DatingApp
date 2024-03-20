using AutoMapper;
using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Dtos;
using DatingApp.Application.Interfaces.Services;
using MediatR;

namespace DatingApp.Application.UseCases.Users.Commands.AddPhotoToUser
{
    public sealed class AddPhotoToUserCommandHandler : IRequestHandler<AddPhotoToUserCommand, (PhotoDto?, string?)>
    {
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUser _currentUser;

        public AddPhotoToUserCommandHandler(IMapper mapper, IPhotoService photoService, IUnitOfWork unitOfWork, IUser currentUser)
        {
            _mapper = mapper;
            _photoService = photoService;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<(PhotoDto?, string?)> Handle(AddPhotoToUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(_currentUser.Username);

            var result = await _photoService.AddPhotoAsync(request.NewUserPhoto);

            if (result.Error != null) return (null, result.Error.Message);

            var newPhoto = user.AddPhoto(result.SecureUri.AbsoluteUri, result.PublicId);

            if (await _unitOfWork.Complete())
                return (_mapper.Map<PhotoDto>(newPhoto), null);

            return (null, "Problem adding photo");
        }
    }
}
