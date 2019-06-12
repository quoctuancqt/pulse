namespace Pulse.Core.Connection.Entity
{
    using Domain;
    using Domain.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Security.Identity.IdentityModels;
    using System.Data.Entity;
    public class PulseContext: IdentityDbContext<PulseIdentityUser, PulseIdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public PulseContext()
            : base("name=Pulse")
        {
        }

        public static PulseContext Create()
        {
            return new PulseContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PulseIdentityUser>().ToTable("PulseIdentityUsers", "dbo");
            modelBuilder.Entity<PulseIdentityRole>().ToTable("PulseIdentityRoles", "dbo");
            modelBuilder.Entity<IdentityUserRole>().ToTable("PulseUserRoles", "dbo");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("PulseUserClaims", "dbo");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("PulseUserLogins", "dbo");
        }

        public virtual IDbSet<UserProfile> UserProfiles { get; set; }
        public virtual IDbSet<Kiosk> Kiosks { get; set; }
        public virtual IDbSet<Country> Countries { get; set; }
        public virtual IDbSet<Group> Groups { get; set; }
        public virtual IDbSet<History> Histories { get; set; }
        public virtual IDbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual IDbSet<KioskSecurity> KioskSecuritys { get; set; }
        public virtual IDbSet<EncryptionUser> EncryptionUsers { get; set; }
        public virtual IDbSet<Client> Clients { get; set; }
        public virtual IDbSet<ClientsCountries> ClientsCountries { get; set; }
    }
}
