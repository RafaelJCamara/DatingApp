using DatingApp.API.Extensions;
using DatingApp.API.Helpers;
using DatingApp.Application.Common.Models;
using DatingApp.Application.Dtos;
using DatingApp.Application.UseCases.Users.Commands.AddPhotoToUser;
using DatingApp.Application.UseCases.Users.Commands.DeleteUserPhoto;
using DatingApp.Application.UseCases.Users.Commands.SetUserMainPhoto;
using DatingApp.Application.UseCases.Users.Commands.UpdateUser;
using DatingApp.Application.UseCases.Users.Queries.GetPaginatedUsers;
using DatingApp.Application.UseCases.Users.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery] UserParamsDto userParams)
    {
        var users = await _mediator.Send(new GetPaginatedUsersQuery(userParams));
        
        Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
        
        return Ok(users);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto?>> GetUser(string username)
    {
        return await _mediator.Send(new GetUserQuery(username));
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateUser(UserUpdateDto userUpdateDto)
    {
        (bool updateUserResult, string? errorMessage) = await _mediator.Send(new UpdateUserCommand(userUpdateDto));

        return updateUserResult ? NoContent() : BadRequest(errorMessage!);
    }
    
    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile newPhoto)
    {
        (PhotoDto? addPhotoResult, string? errorMessage) = await _mediator.Send(new AddPhotoToUserCommand(newPhoto));

        return errorMessage is null ? addPhotoResult : BadRequest(errorMessage!);
    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        (bool setMainPhotoResult, string? errorMessage) = await _mediator.Send(new SetUserMainPhotoCommand(photoId));

        return setMainPhotoResult ? NoContent() : BadRequest(errorMessage!);
    }
    
    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        (bool photoDeletionResult, string? errorMessage) = await _mediator.Send(new DeleteUserPhotoCommand(photoId));

        return photoDeletionResult ? NoContent() : BadRequest(errorMessage!);
    }
    
}
