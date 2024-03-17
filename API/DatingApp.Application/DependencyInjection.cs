using DatingApp.Application.Interfaces.Services;
using DatingApp.Application.SignalR;
using DatingApp.Application.UseCases.Account.Services;
using DatingApp.Application.UseCases.Users.Common.Behaviours;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DatingApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LogUserActivityBehaviour<,>));
        });

        services.AddScoped<ITokenService, TokenService>();

        services.AddSingleton<PresenceTracker>();

        services.AddSignalR();

        return services;
    }

    public static WebApplication MapSignalRHubs(this WebApplication app)
    {
        app.MapHub<PresenceHub>("hubs/presence");
        app.MapHub<MessageHub>("hubs/message");

        return app;
    }

}