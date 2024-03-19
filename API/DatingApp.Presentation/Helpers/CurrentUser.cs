using DatingApp.Application.Common.Extensions;
using DatingApp.Application.Common.Interfaces;

namespace DatingApp.Presentation.Helpers;

public class CurrentUser : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? Id => _httpContextAccessor?.HttpContext?.User.GetUserId();

    public string? Username => _httpContextAccessor?.HttpContext?.User.GetUsername();

    public bool? IsAuthenticated => _httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated;
}
