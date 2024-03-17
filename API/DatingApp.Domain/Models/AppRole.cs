using Microsoft.AspNetCore.Identity;

namespace DatingApp.Domain.Models;

public class AppRole : IdentityRole<int>
{
    public ICollection<AppUserRole> UserRoles { get; set; }
}
