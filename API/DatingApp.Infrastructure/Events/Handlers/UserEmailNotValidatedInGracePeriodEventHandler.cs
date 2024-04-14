using DatingApp.Common.Events;
using DatingApp.Domain.Models;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Events.Handlers
{
    public class UserEmailNotValidatedInGracePeriodEventHandler : IConsumer<UserEmailNotValidatedInGracePeriodEvent>
    {
        private readonly UserManager<AppUser> _userManager;

        public UserEmailNotValidatedInGracePeriodEventHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Consume(ConsumeContext<UserEmailNotValidatedInGracePeriodEvent> context)
        {
            var user = await _userManager
                    .Users
                    .SingleOrDefaultAsync(x => x.UserName.Equals(context.Message.Username));

            if(user is not null)
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTime.MaxValue;
                await _userManager.UpdateAsync(user);
            }
        }
    }
}
