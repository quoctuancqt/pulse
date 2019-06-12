namespace Pulse.Core.Services
{
    using Domain;
    using Domain.Entity;
    using Repository.Entity;
    using System;
    using System.Security.Principal;
    using System.Threading.Tasks;

    public interface IUnitOfWork: IDisposable
    {
        IRepository<Kiosk> Kiosks { get; }
        IRepository<KioskSecurity> KioskSecurities { get; }
        IRepository<Country> Countries { get; }
        IRepository<Group> Groups { get; }
        IRepository<Client> Clients { get; }
        IRepository<ClientsCountries> ClientsCountries { get; }
        IRepository<History> Histories { get; }
        IRepository<RefreshToken> RefreshTokens { get; }
        IRepository<UserProfile> UserProfiles { get; }
        IRepository<EncryptionUser> EncryptionUsers { get; }
        void Commit();
        Task CommitAsync();
        IPrincipal PrincipalUser { get; set; }
    }
}
