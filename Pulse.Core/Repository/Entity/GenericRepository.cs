namespace Pulse.Core.Repository.Entity
{
    using Connection.Entity;
    using Domain;

    public sealed class GenericRepository<TEntity, TContext> : Repository<TEntity, TContext>, IRepository<TEntity>
        where TEntity : class, IEntity
        where TContext : PulseContext, new()
    {
        public GenericRepository() : base(new TContext()) { }
        public GenericRepository(TContext context) : base(context) { }

    }
}
