namespace Pulse.Common.Helpers
{
    using MongoDB.Bson;

    public static class MongoHelper
    {
        public static ObjectId GenerateId()
        {
            return ObjectId.GenerateNewId();
        }
    }
}
