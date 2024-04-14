using DatingApp.Common.Extensions;
using DatingApp.Domain.Models;
using DatingApp.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        //for asp.net identity
        services
            .AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<AppRole>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddEntityFrameworkStores<DataContext>(); //creates all the identity tables

        services.AddSecurityExtensions(config);

        //configuring our policies
        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
            opt.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));
        });


        return services;
    }
}