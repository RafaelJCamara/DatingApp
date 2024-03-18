using DatingApp.API.Extensions;
using DatingApp.API.Helpers;
using DatingApp.Application.Common.Models;
using DatingApp.Application.Dtos;
using DatingApp.Application.UseCases.Likes.Commands.AddLike;
using DatingApp.Application.UseCases.Likes.Commands.Dislike;
using DatingApp.Application.UseCases.Likes.Queries.GetUserLikes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LikesController : ControllerBase
{
    private readonly IMediator _mediator;

    public LikesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{username}")]
    public async Task<ActionResult> AddLike(string targetUsername)
    {
        string? addLikeValidationResult = await _mediator.Send(new AddLikeCommand(targetUsername));
        return addLikeValidationResult is null ? NoContent() : BadRequest(addLikeValidationResult);
    }

    [HttpPost("dislike/{username}")]
    public async Task<ActionResult> Dislike(string targetUsername)
    {
        string? dislikeValidationResult = await _mediator.Send(new DislikeCommand(targetUsername));
        return dislikeValidationResult is null ? NoContent() : BadRequest(dislikeValidationResult);
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery] LikesParamsDto likesParams)
    {
        var userLikes = await _mediator.Send(new GetUserLikesCommand(likesParams));
        
        Response.AddPaginationHeader(new PaginationHeader(userLikes.CurrentPage, userLikes.PageSize, userLikes.TotalCount, userLikes.TotalPages));
        
        return Ok(userLikes);
    }

}