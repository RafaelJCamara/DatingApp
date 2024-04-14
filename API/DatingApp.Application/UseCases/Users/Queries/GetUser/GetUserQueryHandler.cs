using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Dtos;
using DatingApp.Common.Helpers.User;
using MediatR;

namespace DatingApp.Application.UseCases.Users.Queries.GetUser
{
    public sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, MemberDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUser _currentUser;

        public GetUserQueryHandler(IUnitOfWork unitOfWork, IUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<MemberDto?> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var currentMember = await _unitOfWork.UserRepository.GetMemberByUsernameAsync(request.Username);

            currentMember.IsLikedByCurrentUser = await _unitOfWork.LikesRepository.DoesCurrentUserLikeTargetUser(_currentUser.Id.Value, currentMember.Id);

            return currentMember;
        }
    }
}
