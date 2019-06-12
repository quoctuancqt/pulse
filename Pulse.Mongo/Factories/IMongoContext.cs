namespace Pulse.Mongo.Factories
{
    using MongoDB.Driver;
    using System.Threading.Tasks;

    public interface IMongoContext
    {
        IMongoDatabase Database { get; }

        IMongoCollection<T> GetCollection<T>();

        IMongoCollection<T> GetCollection<T>(string name, IndexKeysDefinition<T> keys, CreateIndexOptions options);
    }
}
