using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sports_hub.Services.Interfaces
{
    public interface IEmailService
    {
        public Task SendConfirmationEmailAsync(string email, string callbackUrl, string language);

        public Task SendEmailPasswordResetAsync(string email, string callbackUrl, string language);
    }
}
