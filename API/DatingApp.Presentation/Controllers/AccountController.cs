using DatingApp.Application.Dtos;
using DatingApp.Application.UseCases.Account.Commands.Login;
using DatingApp.Application.UseCases.Account.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        (UserDto? registerResult, object errors) = await _mediator.Send(new RegisterCommand(registerDto));

        return registerResult is null ? BadRequest(errors) : registerResult;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
    {
        var loginResult = await _mediator.Send(new LoginCommand(loginDto));

        return loginResult is null ? Unauthorized("Username or password are incorrect") : loginResult;
    }
}