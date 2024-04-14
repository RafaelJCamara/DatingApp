using AutoMapper;
using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Dtos;
using DatingApp.Application.Interfaces.Services;
using DatingApp.Common.Helpers.User;
using DatingApp.Domain.Models;
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

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                IsMain = user.Photos.Count == 0
            };

            user.Photos.Add(photo);

            if (await _unitOfWork.Complete())
                return (_mapper.Map<PhotoDto>(photo), null);

            return (null, "Problem adding photo");
        }
    }
}
