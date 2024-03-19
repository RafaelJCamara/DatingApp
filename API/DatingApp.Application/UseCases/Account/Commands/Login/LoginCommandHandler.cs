using DatingApp.Application.Dtos;
using DatingApp.Application.Interfaces.Services;
using DatingApp.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Application.UseCases.Account.Commands.Login;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, UserDto?>
{

    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(UserManager<AppUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<UserDto?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager
                            .Users
                            .Include(p => p.Photos)
                            .SingleOrDefaultAsync(x => x.UserName.Equals(request.LoginCredentials.Username));

        if (user is null) return null;

        var result = await _userManager.CheckPasswordAsync(user, request.LoginCredentials.Password);

        if (!result) return null;

        return new UserDto
        {
            Username = user.UserName,
            Token = await _tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };
    }
}
