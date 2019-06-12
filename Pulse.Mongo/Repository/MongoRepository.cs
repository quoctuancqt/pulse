namespace Pulse.Mongo.Repository
{
    using Common.Helpers;
    using Domain.Mongo;
    using Factories;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class MongoRepository<T> : IMongoRepository<T> where T : IMongo<ObjectId>
    {
        private readonly IMongoContext _mongoContext;

        public MongoRepository(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public IMongoCollection<T> Collection
        {
            get { return _mongoContext.GetCollection<T>(); }
        }

        public void Add(T entity)
        {
            AsyncHelper.RunSync(() => InsertOneAsync(entity));
        }

        public IEnumerable<T> AddAll(IEnumerable<T> entities)
        {
            if (!entities.Any())
            {
                return entities;
            }

            AsyncHelper.RunSync(() => InsertManyAsync(entities));
            return entities;
        }

        public T Update(T entity)
        {
            AsyncHelper.RunSync(() => UpdateAsync(entity));
            return entity;
        }

        public IEnumerable<T> UpdateAll(IEnumerable<T> entities)
        {
            if (!entities.Any())
            {
                return entities;
            }

            AsyncHelper.RunSync(() => ReplaceAllAsync(entities));
            return entities;
        }

        public async Task<IEnumerable<T>> UpdateAllAsync(IEnumerable<T> entities)
        {
            if (!entities.Any())
            {
                return entities;
            }

            await ReplaceAllAsync(entities);

            return entities;
        }

        public void Delete(T entity)
        {
            AsyncHelper.RunSync(() => DeleteAllAsync(e => e.Id == entity.Id));
        }

        public void DeleteById(ObjectId id)
        {
            AsyncHelper.RunSync(() => DeleteAllAsync(e => e.Id == id));
        }

        public void DeleteAll()
        {
            AsyncHelper.RunSync(() => DeleteAllAsync(e => true));
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            AsyncHelper.RunSync(() => DeleteAllAsync(predicate));
        }

        public T FindById(ObjectId id)
        {
            return AsyncHelper.RunSync(() => FindAsync(x => x.Id == id));
        }

        public void DeleteAll(IEnumerable<T> entities)
        {
            if (!entities.Any())
            {
                return;
            }

            AsyncHelper.RunSync(() => DeleteAllAsync(e => entities.Select(x => x.Id).Contains(e.Id)));
        }

        public IList<T> FindAll()
        {
            return AsyncHelper.RunSync(() => SearchAsync(t => true).ToListAsync());
        }

        public IList<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return AsyncHelper.RunSync(() => SearchAsync(predicate).ToListAsync());
        }

        public IList<T> SearchFor(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> sortByExpression, bool isAscending = true)
        {
            return AsyncHelper.RunSync(() => SearchAsync(predicate, sortByExpression, isAscending).ToListAsync());
        }

        public IList<T> SearchForPaging(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> sortByExpression, int pageIndex, int pageSize)
        {
            return AsyncHelper.RunSync(() => SearchForPagingAsync(whereExpression, sortByExpression, pageIndex, pageSize));
        }

        public T FindOne(Expression<Func<T, bool>> predicate)
        {
            return AsyncHelper.RunSync(() => FindAsync(predicate));
        }

        public async Task<IList<T>> FindAllAsync()
        {
            return await this.SearchAsync(t => true).ToListAsync();
        }

        public async Task<IList<T>> SearchForAsync(Expression<Func<T, bool>> whereExpression)
        {
            return await this.SearchAsync(whereExpression).ToListAsync();
        }

        public async Task<IList<T>> SearchForPagingAsync(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> sortByExpression, int pageIndex, int pageSize)
        {
            if (whereExpression == null)
            {
                throw new ArgumentNullException(nameof(whereExpression));
            }

            var findExpression = this.SearchAsync(whereExpression);
            if (sortByExpression != null)
            {
                findExpression = findExpression.SortBy(sortByExpression);
            }

            findExpression.Skip(pageIndex * pageSize).Limit(pageSize);

            return await findExpression.ToListAsync();
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> findExpression)
        {
            return await this.SearchAsync(findExpression).FirstOrDefaultAsync();
        }

        public async Task InsertOneAsync(T entity)
        {
            await Collection.InsertOneAsync(entity);
        }

        public async Task<BulkWriteResult<T>> InsertManyAsync(IEnumerable<T> entities)
        {
            if (!entities.Any())
            {
                return null;
            }

            var requests = new List<WriteModel<T>>();
            requests.AddRange(entities.Select(x => new InsertOneModel<T>(x)));
           return await Collection.BulkWriteAsync(requests);
        }

        public async Task UpdateAsync(T entity)
        {
            var filter = Builders<T>.Filter.Eq(u => u.Id, entity.Id);
            await Collection.ReplaceOneAsync(filter, entity);
        }

        public async Task ReplaceAllAsync(IEnumerable<T> entities)
        {
            var writeModels = entities
                .Select(entity => new ReplaceOneModel<T>(Builders<T>.Filter.Eq(e => e.Id, entity.Id), entity))
                .Cast<WriteModel<T>>()
                .ToList();

            await Collection.BulkWriteAsync(writeModels);
        }

        public async Task DeleteAllAsync(Expression<Func<T, bool>> predicate)
        {
            await Collection.DeleteManyAsync(predicate);
        }

        public IFindFluent<T, T> SearchAsync(Expression<Func<T, bool>> whereExpression)
        {
            return this.Collection.Find(Builders<T>.Filter.Where(whereExpression));
        }

        public IFindFluent<T, T> SearchAsync(Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> sortExpression, bool isAscending = true)
        {
            return this.Collection.Find(Builders<T>.Filter.Where(whereExpression)).Sort(isAscending ? Builders<T>.Sort.Ascending(sortExpression) : Builders<T>.Sort.Descending(sortExpression));
        }
    }
}
