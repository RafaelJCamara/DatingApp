using DatingApp.Domain.Common.Response;
using MediatR;

namespace DatingApp.Application.UseCases.Admin.Commands.EditRoles
{
    public sealed record EditRolesCommand(string Username, string Roles) : IRequest<Result<List<string>?>>;
}
