namespace Pulse.Mongo.Factories
{
    using MongoDB.Driver;
    using System;
    using System.Threading.Tasks;
    using Domain.Mongo;
    using Domain.Mongo.Enum;
    using Repository;

    public class MongoDbContextFactory : IMongoContextFactory
    {
        private readonly string _connectionString;
        private const int ExpireDay = 7;

        public MongoDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<MongoState> CreateDatabaseAsync(IMongoContext mongoContext)
        {
            try
            {
                await CreateCollectionNotifyKioskAsync(mongoContext);

                await CreateCollectionClientUserAsync(mongoContext);

                await CreateCollectionUserActivitiesAsync(mongoContext);

                await CreateCollectionSystemEventAsync(mongoContext);

                return new MongoState(MongoStatus.Successful, null);
            }
            catch(Exception ex)
            {
                return new MongoState(MongoStatus.Fail, ex);
            }
        }

        public IMongoContext GetMongoContext()
        {
            return GetMongoContext(_connectionString);
        }

        public IMongoContext GetMongoContext(string connectionString)
        {
            var mongoUrlBuilder = new MongoUrlBuilder(connectionString);

            var mongoClient = new MongoClient(connectionString);

            return new MongoContext(mongoClient, mongoUrlBuilder.DatabaseName);
        }

        #region private method
        private async Task CreateCollectionNotifyKioskAsync(IMongoContext mongoContext)
        {
            var notification = mongoContext.GetCollection<NotifyKiosk>();

            var options = new CreateIndexOptions { ExpireAfter = TimeSpan.FromDays(ExpireDay) };

            await notification.Indexes.CreateOneAsync(Builders<NotifyKiosk>.IndexKeys.Descending(x => x.CreateAt), options);
        }

        private async Task CreateCollectionClientUserAsync(IMongoContext mongoContext)
        {
            var clientUser = mongoContext.GetCollection<ClientUser>();

            await clientUser.Indexes.CreateOneAsync(Builders<ClientUser>.IndexKeys.Ascending(x => x.GroupName));
        }

        private async Task CreateCollectionUserActivitiesAsync(IMongoContext mongoContext)
        {
            var userActivities = mongoContext.GetCollection<UserActivities>();

            var options = new CreateIndexOptions { ExpireAfter = TimeSpan.FromDays(ExpireDay) };

            await userActivities.Indexes.CreateOneAsync(Builders<UserActivities>.IndexKeys.Ascending(x => x.CreateAt), options);
        }

        private async Task CreateCollectionSystemEventAsync(IMongoContext mongoContext)
        {
            var systemEvent = mongoContext.GetCollection<SystemEvent>();

            var options = new CreateIndexOptions { ExpireAfter = TimeSpan.FromDays(ExpireDay) };

            await systemEvent.Indexes.CreateOneAsync(Builders<SystemEvent>.IndexKeys.Ascending(x => x.CreateAt), options);
        }
        #endregion

        #region Repository

        public IMongoRepository<NotifyKiosk> NotifyKioskRepository
        {
            get
            {
                return new MongoRepository<NotifyKiosk>(GetMongoContext());
            }
        }

        public IMongoRepository<ClientUser> ClientUserRepository
        {
            get
            {
                return new MongoRepository<ClientUser>(GetMongoContext());
            }
        }

        public IMongoRepository<SystemEvent> SystemEventRepository
        {
            get
            {
                return new MongoRepository<SystemEvent>(GetMongoContext());
            }
        }

        public IMongoRepository<UserActivities> UserActivitiesRepository
        {
            get
            {
                return new MongoRepository<UserActivities>(GetMongoContext());
            }
        }

        #endregion
    }
}
