namespace Pulse.Core.Security.Identity.IdentityModels
{
    using Microsoft.AspNet.Identity.EntityFramework;
    public class PulseIdentityUser : IdentityUser
    {
        public string ClientId { get; set; }
        public string SecretKey { get; set; }
    }
}
