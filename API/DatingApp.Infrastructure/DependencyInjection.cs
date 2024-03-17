using DatingApp.Application.Common.Interfaces;
using DatingApp.Application.Interfaces.Services;
using DatingApp.Infrastructure.BlobStorage;
using DatingApp.Infrastructure.BlobStorage.Settings;
using DatingApp.Infrastructure.Database;
using DatingApp.Infrastructure.Database.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IPhotoService, PhotoService>();

        services.Configure<CloudinarySettings>(configuration.GetSection(nameof(CloudinarySettings)));

        return services;
    }
}