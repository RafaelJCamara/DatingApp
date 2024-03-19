using DatingApp.Application.Common.Models;
using DatingApp.Application.Dtos;
using MediatR;

namespace DatingApp.Application.UseCases.Messages.Queries.GetMessagesForUser
{
    public sealed record GetMessagesForUserQuery(MessageParamsDto MessageParams) : IRequest<PagedList<MessageDto>>;
}
