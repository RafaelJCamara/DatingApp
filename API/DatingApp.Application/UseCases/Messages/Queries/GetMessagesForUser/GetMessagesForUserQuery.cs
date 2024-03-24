using DatingApp.Application.Common.Models;
using DatingApp.Application.Dtos;
using DatingApp.Domain.Common.Response;
using MediatR;

namespace DatingApp.Application.UseCases.Messages.Queries.GetMessagesForUser
{
    public sealed record GetMessagesForUserQuery(MessageParamsDto MessageParams) : IRequest<Result<PagedList<MessageDto>>>;
}
