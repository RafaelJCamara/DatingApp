using DatingApp.Common.Events;
using DatingApp.Domain.Models;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Infrastructure.Events.Handlers
{
    public class EmailValidatedEventHandler : IConsumer<EmailValidatedEvent>
    {
        private readonly UserManager<AppUser> _userManager;

        public EmailValidatedEventHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task Consume(ConsumeContext<EmailValidatedEvent> context)
        {
            var user = await _userManager
                    .Users
                    .SingleOrDefaultAsync(x => x.UserName.Equals(context.Message.Username) && x.Email.Equals(context.Message.Email));

            if(user is not null)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
            }
        }
    }
}
