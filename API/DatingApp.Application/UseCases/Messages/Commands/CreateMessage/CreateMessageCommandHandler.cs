using AutoMapper;
using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Dtos;
using DatingApp.Common.Helpers.User;
using DatingApp.Domain.Models;
using MediatR;

namespace DatingApp.Application.UseCases.Messages.Commands.CreateMessage
{
    public sealed class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, (string?, MessageDto?)>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUser _currentUser;

        public CreateMessageCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IUser currentUser)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<(string?, MessageDto?)> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            var username = _currentUser.Username;

            if (username == request.MessageToCreate.RecipientUsername.ToLower())
                return ("You cannot send messages to yourself", null);

            var sender = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var recipient = await _unitOfWork.UserRepository.GetUserByUsernameAsync(request.MessageToCreate.RecipientUsername);

            if (recipient == null) return ("Recipient not found", null);

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = request.MessageToCreate.Content
            };
            _unitOfWork.MessageRepository.AddMessage(message);

            if (await _unitOfWork.Complete()) return (null, _mapper.Map<MessageDto>(message));

            return ("Failed to send message", null);
        }
    }
}
