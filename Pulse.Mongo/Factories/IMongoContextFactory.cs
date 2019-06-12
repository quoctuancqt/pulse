using Pulse.Domain.Mongo;
using Pulse.Domain.Mongo.Enum;
using Pulse.Mongo.Repository;
using System.Threading.Tasks;

namespace Pulse.Mongo.Factories
{
    public interface IMongoContextFactory
    {
        IMongoContext GetMongoContext();

        IMongoContext GetMongoContext(string connectionString);

        Task<MongoState> CreateDatabaseAsync(IMongoContext mongoContext);

        IMongoRepository<NotifyKiosk> NotifyKioskRepository { get; }

        IMongoRepository<ClientUser> ClientUserRepository { get; }

        IMongoRepository<SystemEvent> SystemEventRepository { get; }

        IMongoRepository<UserActivities> UserActivitiesRepository { get; }
    }
}
