using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Interfaces.Services;
using DatingApp.Common.Extensions;
using DatingApp.Domain.Models;
using DatingApp.Infrastructure.BlobStorage;
using DatingApp.Infrastructure.BlobStorage.Settings;
using DatingApp.Infrastructure.Database;
using DatingApp.Infrastructure.Database.Seed;
using DatingApp.Infrastructure.Database.UoW;
using DatingApp.Infrastructure.Events.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace DatingApp.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            })
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IPhotoService, PhotoService>()
            .AddRabbitMQ(configuration, new List<Assembly> { Assembly.GetAssembly(typeof(EmailValidatedEventHandler)) });

        services.Configure<CloudinarySettings>(configuration.GetSection(nameof(CloudinarySettings)));

        return services;
    }

    public static async Task<WebApplication> MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;

        try
        {
            var dbContext = services.GetRequiredService<DataContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
            await dbContext.Database.MigrateAsync();
            await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM [Connections]");
            await Seed.SeedUsers(userManager, roleManager);
        }
        catch (Exception ex)
        {
            var logger = services.GetService<ILogger<WebApplication>>();
            logger.LogError(ex, "An error occured during the database migration.");
        }

        return app;
    }

}