using DatingApp.Application.Common.Models;
using DatingApp.Application.Dtos;
using DatingApp.Domain.Models;

namespace DatingApp.Application.Interfaces.Repository;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser> GetUserByIdAsync(int id);
    Task<AppUser> GetUserByUsernameAsync(string username);
    Task<PagedList<MemberDto>> GetMembersAsync(UserParamsDto userParams);
    Task<MemberDto> GetMemberByUsernameAsync(string username);
    Task<string> GetUserGender(string username);
}