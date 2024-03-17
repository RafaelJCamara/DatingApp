using AutoMapper;
using DatingApp.Application.Dtos;
using DatingApp.Application.Interfaces.Services;
using DatingApp.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Application.UseCases.Account.Commands.Register;

public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, (UserDto?, object)>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public RegisterCommandHandler(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<(UserDto?, object?)> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userManager.Users.AnyAsync(user => user.UserName.Equals(request.RegisterInformation.Username.ToLower())))
            return (null, "User is already taken");

        var user = _mapper.Map<AppUser>(request.RegisterInformation);

        var result = await _userManager.CreateAsync(user, request.RegisterInformation.Password);

        var roleResult = await _userManager.AddToRoleAsync(user, "Member");

        if (!result.Succeeded) return (null, result.Errors);

        return (new UserDto
        {
            Username = user.UserName,
            Token = await _tokenService.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender
        }, null);
    }
}
