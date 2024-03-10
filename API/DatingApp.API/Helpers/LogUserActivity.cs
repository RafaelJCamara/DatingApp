using DatingApp.API.Extensions;
using DatingApp.API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DatingApp.API.Helpers;

public class LogUserActivity : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var result = await next();

        if (!result.HttpContext.User.Identity.IsAuthenticated) return;

        var userId = result.HttpContext.User.GetUserId();

        var unitOfWork = result.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

        var user = await unitOfWork.UserRepository.GetUserByIdAsync(userId);
        user.LastActive = DateTime.UtcNow;

        await unitOfWork.Complete();
    }
}