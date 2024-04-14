using DatingApp.Application.UseCases.Admin.Commands.EditRoles;
using DatingApp.Application.UseCases.Admin.Queries.GetUsersWithRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("users-with-roles")]
    public async Task<ActionResult> GetUsersWithRoles()
    {
        return Ok(await _mediator.Send(new GetUsersWithRolesQuery()));
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("edit-roles/{username}")]
    public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
    {
        (string? editRolesValidationResult, List<string> newUserRoles) = await _mediator.Send(new EditRolesCommand(username, roles));

        return editRolesValidationResult is null ? Ok(newUserRoles) : BadRequest(editRolesValidationResult);
    }

}
