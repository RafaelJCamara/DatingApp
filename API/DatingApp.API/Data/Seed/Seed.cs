using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using DatingApp.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data.Seed;

public class Seed
{
    public static async Task SeedUsers(DataContext context)
    {
        if(await context.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("Data/Seed/Data/UserSeedData.json");

        //we don't care about the casing
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};

        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);
        
        foreach (var user in users)
        {
            //generate password
            using var hmac = new HMACSHA512();
            user.UserName = user.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
            user.PasswordSalt = hmac.Key;

            context.Users.Add(user);
        }

        await context.SaveChangesAsync();
    }
}