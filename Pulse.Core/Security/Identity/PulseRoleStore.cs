namespace Pulse.Core.Security.Identity
{
    using Connection.Entity;
    using IdentityModels;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;

    public class PulseRoleStore: RoleStore<PulseIdentityRole>, IRoleStore<PulseIdentityRole>, IRoleStore<PulseIdentityRole, string>, IDisposable
    {
        public PulseRoleStore(PulseContext context)
            : base(context)
        {
        }
    }
}
