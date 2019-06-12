namespace Pulse.Mongo.Factories
{
    using MongoDB.Driver;
    using MongoDB.Bson.Serialization.Conventions;
    using MongoDB.Bson;
    using System;
    using System.Threading.Tasks;
    using Common.Helpers;

    public class MongoContext : IMongoContext
    {
        public IMongoDatabase Database { get; private set; }

        public MongoContext(IMongoClient mongoClient, string dbName)
        {
            var pack = new ConventionPack()
            {
                new CamelCaseElementNameConvention(),
                new EnumRepresentationConvention(BsonType.String)
            };

            ConventionRegistry.Register("CamelCaseConvensions", pack, t => true);
            Database = mongoClient.GetDatabase(dbName);
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            return Database.GetCollection<T>(typeof(T).Name);
        }

        public IMongoCollection<T> GetCollection<T>(string name, IndexKeysDefinition<T> keys, CreateIndexOptions options)
        {
            var collectionName = string.Format("{0}_{1}", typeof(T).Name, name);

            var collectionsOptions = new ListCollectionsOptions
            {
                Filter = Builders<BsonDocument>.Filter.Eq("name", collectionName)
            };

            var result = this.Database.ListCollections(collectionsOptions);

            if (!result.Any())
            {
                return CreateOneAsync<T>(collectionName, keys, options);
            }
            else
            {
                return Database.GetCollection<T>(collectionName);
            }

        }

        private IMongoCollection<T> CreateOneAsync<T>(string collectionName, IndexKeysDefinition<T> keys, CreateIndexOptions options)
        {
            var collection = this.Database.GetCollection<T>(collectionName);

            AsyncHelper.RunSync(() => collection.Indexes.CreateOneAsync(keys, options));
              
            return collection;
        }
    }
}
