using DatingApp.API.Entities;

namespace DatingApp.API.Interfaces;

public interface ITokenService
{
    string CreateToken(AppUser user);
}