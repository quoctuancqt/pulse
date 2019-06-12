namespace Pulse.Core.Security.Identity
{
    using Connection.Entity;
    using IdentityModels;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    public class PulseUserStore : UserStore<PulseIdentityUser, PulseIdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>,
        IUserLoginStore<PulseIdentityUser>, IUserClaimStore<PulseIdentityUser>,
        IUserPasswordStore<PulseIdentityUser>, IUserSecurityStampStore<PulseIdentityUser>,
        IUserStore<PulseIdentityUser>, IUserEmailStore<PulseIdentityUser>, IUserLockoutStore<PulseIdentityUser, string>, IDisposable
    {
        
        public PulseUserStore(PulseContext context) : base(context)
        {
        }

    }
}
