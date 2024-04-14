using DatingApp.API.Extensions;
using DatingApp.API.Middleware;
using DatingApp.Application.Extensions;
using DatingApp.Common.Extensions;
using DatingApp.Infrastructure.Extensions;

namespace DatingApp.Presentation.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddIdentityServices(configuration)
            .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
            .AddCommonHelpers()
            .AddCors();

        return services;
    }

    public static async Task<WebApplication> RunDatingApp(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("https://localhost:4200"));

        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapControllers();

        app.MapSignalRHubs();

        await app.MigrateDatabase();

        app.Run();

        return app;
    }
}
