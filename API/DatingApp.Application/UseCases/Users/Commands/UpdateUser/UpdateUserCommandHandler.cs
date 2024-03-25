using AutoMapper;
using DatingApp.Application.Common.Interfaces;
using DatingApp.Domain.Common.Response;
using DatingApp.Domain.Errors.Users;
using MediatR;

namespace DatingApp.Application.UseCases.Users.Commands.UpdateUser
{
    public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
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

        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(_currentUser.Username);

            if (user is null) return Result.Failure(UserErrors.UserNotFound(_currentUser.Username));

            _mapper.Map(request.UserToUpdate, user);

            if (await _unitOfWork.Complete()) return Result.Success();

            return Result.Failure(UserErrors.UserUpdateFailed(_currentUser.Username));
        }
    }
}
