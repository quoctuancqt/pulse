namespace Pulse.Core.Security.Identity.IdentityModels
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;

    public class PulseIdentityRole : IdentityRole
    {
        public static readonly string Administrator = "Administrator";
        public static readonly string ClientAdmin = "ClientAdmin";
        public static readonly string Kiosk = "Kiosk";
        public static readonly string User = "User";

        public static IEnumerable<string> FindAll()
        {
            yield return PulseIdentityRole.Administrator;
            yield return PulseIdentityRole.ClientAdmin;
            yield return PulseIdentityRole.Kiosk;
            yield return PulseIdentityRole.User;
        }
    }
}
