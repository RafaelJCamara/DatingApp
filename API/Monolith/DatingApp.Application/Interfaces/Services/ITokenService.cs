using DatingApp.Domain.Models;

namespace DatingApp.Application.Interfaces.Services;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
}