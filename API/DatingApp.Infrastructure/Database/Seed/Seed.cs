using DatingApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DatingApp.Infrastructure.Database.Seed;

public class Seed
{
    public static async Task SeedUsers(UserManager<AppUser> userManager,
         RoleManager<AppRole> roleManager)
    {
        if (await userManager.Users.AnyAsync()) return;

        var dir = typeof(Seed).Assembly.GetName().Name;

        var path = @"..\DatingApp.Infrastructure\Database\Seed\Data\UserSeedData.json";

        var userData = await File.ReadAllTextAsync(path);

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        var roles = new List<AppRole>
        {
            new AppRole{Name = "Member"},
            new AppRole{Name = "Admin"},
            new AppRole{Name = "Moderator"},
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        foreach (var user in users)
        {
            user.UserName = user.UserName.ToLower();
            user.Created = DateTime.SpecifyKind(user.Created, DateTimeKind.Utc);
            user.LastActive = DateTime.SpecifyKind(user.Created, DateTimeKind.Utc);
            user.EmailConfirmed = true;
            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Member");
        }

        var admin = new AppUser
        {
            UserName = "admin"
        };

        await userManager.CreateAsync(admin, "Pa$$w0rd");
        await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
    }
}