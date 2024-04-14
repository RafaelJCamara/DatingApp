using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Common.Models;
using DatingApp.Application.Dtos;
using DatingApp.Common.Helpers.User;
using MediatR;

namespace DatingApp.Application.UseCases.Users.Queries.GetPaginatedUsers
{
    public sealed class GetPaginatedUsersQueryHandler : IRequestHandler<GetPaginatedUsersQuery, PagedList<MemberDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUser _currentUser;

        public GetPaginatedUsersQueryHandler(IUnitOfWork unitOfWork, IUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<PagedList<MemberDto>> Handle(GetPaginatedUsersQuery request, CancellationToken cancellationToken)
        {
            var gender = await _unitOfWork.UserRepository.GetUserGender(_currentUser.Username);

            request.UserParams.CurrentUsername = _currentUser.Username;

            if (string.IsNullOrEmpty(request.UserParams.Gender))
            {
                request.UserParams.Gender = gender == "male" ? "female" : "male";
            }

            var users = await _unitOfWork.UserRepository.GetMembersAsync(request.UserParams);

            foreach (var member in users)
            {
                var currentMember = await _unitOfWork.UserRepository.GetMemberByUsernameAsync(member.UserName);

                member.IsLikedByCurrentUser = await _unitOfWork.LikesRepository.DoesCurrentUserLikeTargetUser(_currentUser.Id.Value, currentMember.Id);
            }

            return users;
        }
    }
}
