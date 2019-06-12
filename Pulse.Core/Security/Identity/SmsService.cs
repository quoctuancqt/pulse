namespace Pulse.Core.Security.Identity
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
