using DatingApp.API.Extensions;
using DatingApp.API.Helpers;
using DatingApp.Application.Dtos;
using DatingApp.Application.UseCases.Messages.Commands.CreateMessage;
using DatingApp.Application.UseCases.Messages.Commands.DeleteMessage;
using DatingApp.Application.UseCases.Messages.Queries.GetMessagesForUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MessagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto messageToCreate)
    {
        (string sendCreateMessageValidationResult, MessageDto? newMessage) = await _mediator.Send(new CreateMessageCommand(messageToCreate));
        return sendCreateMessageValidationResult is null ? newMessage : BadRequest(sendCreateMessageValidationResult);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery] MessageParamsDto messageParams)
    {
        var messages = await _mediator.Send(new GetMessagesForUserQuery(messageParams));

        Response.AddPaginationHeader(new PaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages));

        return Ok(messages);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(int id)
    {
        string? deleteMessageValidationResult = await _mediator.Send(new DeleteMessageCommand(id));
        return deleteMessageValidationResult is null ? NoContent() : BadRequest(deleteMessageValidationResult);
    }

}