using Microsoft.AspNetCore.Identity;

namespace DatingApp.Domain.Models;

public class AppUserRole : IdentityUserRole<int>
{
    public AppUser User { get; set; }
    public AppRole Role { get; set; }
}
