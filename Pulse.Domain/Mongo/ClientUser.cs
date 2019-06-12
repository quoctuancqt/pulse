namespace Pulse.Domain.Mongo
{
    using Enum;
    using MongoDB.Bson;

    public class ClientUser : IMongo
    {
        public ObjectId Id { get; set; }
        public string UserId { get; set; }
        public GroupType GroupName { get; set; }
    }
}
