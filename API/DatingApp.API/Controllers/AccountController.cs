using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using DatingApp.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
    {
        _context = context;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await _context.Users.AnyAsync(user => user.UserName.Equals(registerDto.Username.ToLower())))
            return BadRequest("User is already taken");

        var user = _mapper.Map<AppUser>(registerDto);
        
        using var hmac = new HMACSHA512();

        user.UserName = registerDto.Username.ToLower();
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
        user.PasswordSalt = hmac.Key;
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        return new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
    {
        var user = await _context
            .Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName.Equals(loginDto.Username));

        if (user is null)
            return Unauthorized("User does not exist.");
        
        using var hmac = new HMACSHA512(user.PasswordSalt);

        var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        if (!passwordHash.SequenceEqual(user.PasswordHash))
            return Unauthorized("Invalid password.");
        
        return new UserDto
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };
    }
}