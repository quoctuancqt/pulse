namespace Pulse.Mongo.Repository
{
    using MongoDB.Bson;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IMongoRepository<T>
    {
        IMongoCollection<T> Collection { get; }

        void Add(T entity);

        IEnumerable<T> AddAll(IEnumerable<T> entities);

        T Update(T entity);

        IEnumerable<T> UpdateAll(IEnumerable<T> entities);

        Task<IEnumerable<T>> UpdateAllAsync(IEnumerable<T> entities);

        void Delete(T entity);

        void DeleteById(ObjectId id);

        void DeleteAll();

        void Delete(Expression<Func<T, bool>> predicate);

        T FindById(ObjectId id);

        void DeleteAll(IEnumerable<T> entities);

        IList<T> FindAll();

        IList<T> SearchFor(Expression<Func<T, bool>> predicate);

        IList<T> SearchFor(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> sortByExpression, bool isAscending = true);

        IList<T> SearchForPaging(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> sortByExpression, int pageIndex, int pageSize);

        T FindOne(Expression<Func<T, bool>> predicate);

        Task<IList<T>> FindAllAsync();

        Task<IList<T>> SearchForAsync(Expression<Func<T, bool>> whereExpression);

        Task<IList<T>> SearchForPagingAsync(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> sortByExpression, int pageIndex, int pageSize);

        Task<T> FindAsync(Expression<Func<T, bool>> findExpression);

        Task InsertOneAsync(T entity);

        Task<BulkWriteResult<T>> InsertManyAsync(IEnumerable<T> entities);

        Task UpdateAsync(T entity);

        Task ReplaceAllAsync(IEnumerable<T> entities);

        Task DeleteAllAsync(Expression<Func<T, bool>> predicate);

        IFindFluent<T, T> SearchAsync(Expression<Func<T, bool>> whereExpression);

        IFindFluent<T, T> SearchAsync(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> sortExpression, bool isAscending = true);
    }
}
