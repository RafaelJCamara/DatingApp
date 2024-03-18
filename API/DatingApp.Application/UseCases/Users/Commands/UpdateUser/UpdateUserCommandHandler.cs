using AutoMapper;
using DatingApp.Application.Common.Interfaces;
using MediatR;

namespace DatingApp.Application.UseCases.Users.Commands.UpdateUser
{
    public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, (bool, string?)>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUser _currentUser;

        public UpdateUserCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IUser currentUser)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<(bool, string?)> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(_currentUser.Username);

            if (user is null) return (false, "User not found.");

            _mapper.Map(request.UserToUpdate, user);

            if (await _unitOfWork.Complete()) return (true, null);

            return (false, "Failed to update the user.");
        }
    }
}
