using DatingApp.Application.Interfaces.Services;
using DatingApp.Application.SignalR;
using DatingApp.Application.UseCases.Account.Common.Services;
using DatingApp.Application.UseCases.Users.Common.Behaviours;
using DatingApp.Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DatingApp.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddScoped<ITokenService, TokenService>()
            .AddMediatrFromAssemblyContaining(Assembly.GetExecutingAssembly(), new()
            {
                { typeof(IPipelineBehavior<,>), typeof(LogUserActivityBehaviour<,>) }
            })
            .AddSingleton<PresenceTracker>()
            .AddSignalR();

        return services;
    }

    public static WebApplication MapSignalRHubs(this WebApplication app)
    {
        app.MapHub<PresenceHub>("hubs/presence");
        app.MapHub<MessageHub>("hubs/message");

        return app;
    }

}