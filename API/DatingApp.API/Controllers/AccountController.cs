using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;

namespace DatingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly DataContext _context;

    public AccountController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
    {
        if (await _context.Users.AnyAsync(user => user.UserName.Equals(registerDto.Username.ToLower())))
            return BadRequest("User is already taken");
        
        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = registerDto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        return user;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<AppUser>> Login([FromBody] LoginDto loginDto)
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName.Equals(loginDto.Username));

        if (user is null)
            return Unauthorized("User does not exist.");
        
        using var hmac = new HMACSHA512(user.PasswordSalt);

        var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        if (!passwordHash.SequenceEqual(user.PasswordHash))
            return Unauthorized("Invalid password.");
        
        return user;
    }
}