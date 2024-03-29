using DatingApp.Application.Dtos;
using DatingApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using DatingApp.Application.Extensions;
using DatingApp.Application.Interfaces.Repository;
using DatingApp.Application.Common.Models;

namespace DatingApp.Infrastructure.Database.Repository;

public class LikesRepository : ILikesRepository
{
    private readonly DataContext _context;
    public LikesRepository(DataContext context)
    {
        _context = context;

    }

    public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
    {
        return await _context.Likes.FindAsync(sourceUserId, likedUserId);
    }

    public async Task<AppUser> GetUserWithLikes(int userId)
    {
        return await _context.Users
            .Include(x => x.LikedUsers)
            .FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<PagedList<LikeDto>> GetUserLikes(LikesParamsDto likesParams)
    {
        var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
        var likes = _context.Likes.AsQueryable();

        //users that the userId liked
        if (likesParams.Predicate == "liked")
        {
            likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
            users = likes.Select(like => like.TargetUser);
        }

        //users that liked userId
        if (likesParams.Predicate == "likedBy")
        {
            likes = likes.Where(like => like.TargetUserId == likesParams.UserId);
            users = likes.Select(like => like.SourceUser);
        }

        var likedUsers = users.Select(user => new LikeDto
        {
            UserName = user.UserName,
            KnownAs = user.KnownAs,
            Age = user.DateOfBirth.CalculateAge(),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
            City = user.City,
            Id = user.Id,
            IsLikedByCurrentUser = likes.Any(like => like.TargetUserId == user.Id) || _context.Likes.AsQueryable().Any(like => like.SourceUserId == likesParams.UserId && like.TargetUserId == user.Id)
        });

        return await PagedList<LikeDto>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
    }

    public async Task<bool> DoesCurrentUserLikeTargetUser(int currentUserId, int targetUserId)
    {
        return await _context.Likes.FindAsync(currentUserId, targetUserId) != null;
    }
}