using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Common.Models;
using DatingApp.Application.Dtos;
using DatingApp.Domain.Common.Response;
using MediatR;

namespace DatingApp.Application.UseCases.Messages.Queries.GetMessagesForUser
{
    public sealed class GetMessagesForUserQueryHandler : IRequestHandler<GetMessagesForUserQuery, Result<PagedList<MessageDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUser _currentUser;

        public GetMessagesForUserQueryHandler(IUnitOfWork unitOfWork, IUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<Result<PagedList<MessageDto>>> Handle(GetMessagesForUserQuery request, CancellationToken cancellationToken)
        {
            request.MessageParams.Username = _currentUser.Username;

            return Result<PagedList<MessageDto>>.Success(await _unitOfWork.MessageRepository.GetMessagesForUser(request.MessageParams));
        }
    }
}
