using MediatR;

namespace DatingApp.Application.UseCases.Admin.Commands.EditRoles
{
    public sealed record EditRolesCommand(string Username, string Roles) : IRequest<(string?, List<string>?)>;
}
