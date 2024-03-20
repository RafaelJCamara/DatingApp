﻿using DatingApp.Application.Common.Interfaces;
using MediatR;

namespace DatingApp.Application.UseCases.Messages.Commands.DeleteMessage
{
    public sealed class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, string?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUser _currentUser;

        public DeleteMessageCommandHandler(IUnitOfWork unitOfWork, IUser currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<string?> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var username = _currentUser.Username;

            var message = await _unitOfWork.MessageRepository.GetMessage(request.MessageId);

            if (!message.BelongsToUser(username))
                return "Unauthorized.";

            message.SetMessageAsDeleted(username);

            if (message.CanMessageBeFullyDeleted())
                _unitOfWork.MessageRepository.DeleteMessage(message);

            if (await _unitOfWork.Complete()) return null;

            return "Problem deleting the message";
        }
    }
}
