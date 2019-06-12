namespace Pulse.Domain.Mongo
{
    using System;
    using MongoDB.Bson;

    public class MongoKiosk : IMongo
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string MachineId { get; set; }
        public string Json { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
