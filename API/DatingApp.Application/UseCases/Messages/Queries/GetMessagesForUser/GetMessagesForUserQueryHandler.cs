using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Common.Models;
using DatingApp.Application.Dtos;
using DatingApp.Common.Helpers.User;
using MediatR;

namespace DatingApp.Application.UseCases.Messages.Queries.GetMessagesForUser
{
    public sealed class GetMessagesForUserQueryHandler : IRequestHandler<GetMessagesForUserQuery, PagedList<MessageDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUser _currentUser;

        public GetMessagesForUserQueryHandler(IUnitOfWork unitOfWork, IUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<PagedList<MessageDto>> Handle(GetMessagesForUserQuery request, CancellationToken cancellationToken)
        {
            request.MessageParams.Username = _currentUser.Username;

            return await _unitOfWork.MessageRepository.GetMessagesForUser(request.MessageParams);
        }
    }
}
