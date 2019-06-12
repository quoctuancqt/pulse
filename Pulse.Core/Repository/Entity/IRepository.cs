namespace Pulse.Core.Repository.Entity
{
    using Domain;
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> pression = null);

        TEntity FindBy(int id);

        Task<TEntity> FindByAsync(int id);

        void Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

    }
}
