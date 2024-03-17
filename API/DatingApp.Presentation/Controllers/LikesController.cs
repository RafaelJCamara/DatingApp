using DatingApp.Application.Dtos;
using DatingApp.API.Extensions;
using DatingApp.API.Helpers;
using DatingApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using DatingApp.Application.Common.Models;
using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Common.Extensions;

namespace DatingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LikesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public LikesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    [HttpPost("{username}")]
    public async Task<ActionResult> AddLike(string username)
    {
        var sourceUserId = User.GetUserId();
        var likedUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        var sourceUser = await _unitOfWork.LikesRepository.GetUserWithLikes(sourceUserId);

        if (likedUser == null) return NotFound();

        if (sourceUser.UserName == username) return BadRequest("You cannot like yourself");

        var userLike = await _unitOfWork.LikesRepository.GetUserLike(sourceUserId, likedUser.Id);

        if (userLike != null) return BadRequest("You already like this user");

        userLike = new UserLike
        {
            SourceUserId = sourceUserId,
            TargetUserId = likedUser.Id
        };

        sourceUser.LikedUsers.Add(userLike);

        if (await _unitOfWork.Complete()) return Ok();

        return BadRequest("Failed to like user");
    }

    [HttpPost("dislike/{username}")]
    public async Task<ActionResult> Dislike(string username)
    {
        var sourceUserId = User.GetUserId();
        var disliked = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        var sourceUser = await _unitOfWork.LikesRepository.GetUserWithLikes(sourceUserId);

        if (disliked == null) return NotFound();

        if (sourceUser.UserName == username) return BadRequest("You cannot dislike yourself");

        var userLike = await _unitOfWork.LikesRepository.GetUserLike(sourceUserId, disliked.Id);

        if (userLike == null) return BadRequest("You can't dislike this user");

        sourceUser.LikedUsers.Remove(userLike);

        if (await _unitOfWork.Complete()) return Ok();

        return BadRequest("Failed to dislike user");
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery] LikesParamsDto likesParams)
    {
        likesParams.UserId = User.GetUserId();
        
        var users = await _unitOfWork.LikesRepository.GetUserLikes(likesParams);
        
        Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
        
        return Ok(users);
    }

}