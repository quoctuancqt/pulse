namespace Pulse.Core.Security.Identity
{
    using IdentityModels;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using System;

    public class PulseRoleManager : RoleManager<PulseIdentityRole>, IDisposable
    {
        public PulseRoleManager(IRoleStore<PulseIdentityRole, string> store) : base(store)
        {
        }

        public static RoleManager<PulseIdentityRole> Create(IdentityFactoryOptions<RoleManager<PulseIdentityRole>> options, IOwinContext context)
        {
            var roleStore = context.Get<IRoleStore<PulseIdentityRole>>();
            return new PulseRoleManager(roleStore);
        }
    }
}
