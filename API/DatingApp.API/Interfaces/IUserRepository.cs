using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using DatingApp.API.Helpers;

namespace DatingApp.API.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser> GetUserByIdAsync(int id);
    Task<AppUser> GetUserByUsernameAsync(string username);
    Task<bool> SaveAllAsync();
    Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
    Task<MemberDto> GetMemberByUsernameAsync(string username);
}