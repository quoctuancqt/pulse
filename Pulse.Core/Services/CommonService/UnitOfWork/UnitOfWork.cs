namespace Pulse.Core.Services
{
    using System;
    using System.Threading.Tasks;
    using Repository.Entity;
    using Domain;
    using Connection.Entity;
    using System.Security.Principal;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity;
    using Microsoft.AspNet.Identity;
    using System.Linq;
    using Domain.Entity;

    public class UnitOfWork : IUnitOfWork
    {
        private PulseContext _context;
        public IPrincipal PrincipalUser { get; set; }
        private string CurrentUserId
        {
            get
            {
                if (PrincipalUser != null)
                {
                    return PrincipalUser.Identity.GetUserId();
                }

                return null;
            }
        }

        #region Repository
        public IRepository<Kiosk> Kiosks { get; private set; }
        public IRepository<KioskSecurity> KioskSecurities { get; private set; }
        public IRepository<Group> Groups { get; private set; }
        public IRepository<Client> Clients { get; private set; }
        public IRepository<ClientsCountries> ClientsCountries { get; private set; }
        public IRepository<History> Histories { get; private set; }
        public IRepository<Country> Countries { get; private set; }
        public IRepository<RefreshToken> RefreshTokens { get; private set; }
        public IRepository<UserProfile> UserProfiles { get; private set; }
        public IRepository<EncryptionUser> EncryptionUsers { get; private set; }
        #endregion

        #region Setting UnitOfWork

        public UnitOfWork(): this(new PulseContext()) { }

        public UnitOfWork(PulseContext context)
        {
            _context = context;
            InitRepositories();

        }

        private void InitRepositories()
        {
            Kiosks = new GenericRepository<Kiosk, PulseContext>(_context);
            KioskSecurities = new GenericRepository<KioskSecurity, PulseContext>(_context);
            Groups = new GenericRepository<Group, PulseContext>(_context);
            Histories = new GenericRepository<History, PulseContext>(_context);
            Countries = new GenericRepository<Country, PulseContext>(_context);
            RefreshTokens = new GenericRepository<RefreshToken, PulseContext>(_context);
            UserProfiles = new GenericRepository<UserProfile, PulseContext>(_context);
            EncryptionUsers = new GenericRepository<EncryptionUser, PulseContext>(_context);
            Clients = new GenericRepository<Client, PulseContext>(_context);
            ClientsCountries = new GenericRepository<ClientsCountries, PulseContext>(_context);
        }
        #endregion

        #region Commit transaction
        public void Commit()
        {
            BeforeCommit();
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            BeforeCommit();
            await _context.SaveChangesAsync();
        }

        private void BeforeCommit()
        {
            var manager = ((IObjectContextAdapter)_context).ObjectContext.ObjectStateManager;

            foreach (var e in manager.GetObjectStateEntries(EntityState.Added).Where(e => e.Entity is IAudits).Select(e => e.Entity as IAudits))
            {
                e.CreatedBy = CurrentUserId;
                e.CreatedAt = DateTime.Now;
            }

            foreach (var e in manager.GetObjectStateEntries(EntityState.Modified).Where(e => e.Entity is IAudits).Select(e => e.Entity as IAudits))
            {
                e.UpdatedBy = CurrentUserId;
                e.UpdatedAt = DateTime.Now;
            }
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing == true)
            {
                _context = null;
            }
        }

        #endregion
    }
}
