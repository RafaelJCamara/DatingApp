using System.Diagnostics;
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
            var activitySource = new ActivitySource("DatingApp");

            using (var activity =
                   activitySource.StartActivity("[Consumer] User email not validated in grace period event", ActivityKind.Consumer))
            {
                activity?.SetTag("messaging.system", "rabbitmq");
                activity?.SetTag("messaging.origin_kind", "queue");
                activity?.SetTag("messaging.rabbitmq.queue", "user-email-not-validated-in-grace-period-event");
                
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
}
