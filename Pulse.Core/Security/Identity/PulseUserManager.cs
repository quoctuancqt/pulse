namespace Pulse.Core.Security.Identity
{
    using Connection.Entity;
    using IdentityModels;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Security.Claims;
    using System.Collections.Generic;
    using Common.ResolverFactories;
    using Services;
    using Domain.Enum;

    public class PulseUserManager : UserManager<PulseIdentityUser>, IDisposable
    {
        public PulseUserManager(IUserStore<PulseIdentityUser> store) : base(store)
        {
            EmailService = ResolverFactory.GetService<IIdentityMessageService>();

            //this.SmsService = new SmsService();
        }

        public static PulseUserManager Create(IdentityFactoryOptions<PulseUserManager> options, IOwinContext context)
        {
            var identityDbContext = context.Get<PulseContext>();
            var userManager = new PulseUserManager(new PulseUserStore(identityDbContext));
            userManager.UserValidator = new UserValidator<PulseIdentityUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false,
            };

            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            return userManager;
        }

        public async Task<PulseIdentityUser> FindFirstByClientIdAsync(string clientId)
        {
            return await Users.Where(u => u.ClientId.Equals(clientId)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PulseIdentityUser>> FindByClientIdAsync(string clientId)
        {
            return await Users.Where(u => u.ClientId.Equals(clientId)).ToArrayAsync();
        }

        public async Task<PulseIdentityUser> FindByClientIdAndNameAsync(string clientId, string userName)
        {
            return await Users.Where(u => u.ClientId.Equals(clientId) && u.UserName.Equals(userName)).FirstOrDefaultAsync();
        }

        public async Task<IdentityResult> UpdateByClientIdAsync(string userId, string clientId, string secretKey)
        {
            var user = await Users.Where(u => u.Id.Equals(userId)).FirstOrDefaultAsync();

            user.ClientId = clientId;
            user.SecretKey = secretKey;

            return await UpdateAsync(user);
        }

        public override async Task<ClaimsIdentity> CreateIdentityAsync(PulseIdentityUser user, string authenticationType)
        {
            IList<Claim> claims = new List<Claim>();
            
            PulseUserManager userManager = ResolverFactory.GetService<PulseUserManager>();

            IClientService clientService = ResolverFactory.GetService<IClientService>();

            var roleName = userManager.GetRoles(user.Id).FirstOrDefault();

            var client = await clientService.FindByClientIdAsync(user.ClientId);

            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id, null, ClaimsIdentity.DefaultIssuer, "Provider"));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName, null, ClaimsIdentity.DefaultIssuer, "Provider"));
            claims.Add(new Claim("ClientId", user.ClientId, null, ClaimsIdentity.DefaultIssuer, "Provider"));
            claims.Add(new Claim("AllowedGrant", Enum.GetName(typeof(OAuthGrant), client.AllowedGrant) , null, ClaimsIdentity.DefaultIssuer, "Provider"));
            claims.Add(new Claim(ClaimTypes.Role, roleName, null, ClaimsIdentity.DefaultIssuer, "Provider"));
            
            var claimsIdentity = new ClaimsIdentity(claims, authenticationType);

            return await Task.FromResult(claimsIdentity);
        }

        public async Task<IdentityResult> ActiveUserAsync(string userId)
        {
            var user = await Users.Where(u => u.Id.Equals(userId)).FirstOrDefaultAsync();

            if (user == null) throw new Exception("Not found user with userId: " + userId);

            user.EmailConfirmed = true;

            return await UpdateAsync(user);
        }

    }
}
