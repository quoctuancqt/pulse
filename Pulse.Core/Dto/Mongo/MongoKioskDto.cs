namespace Pulse.Core.Dto.Mongo
{
    using MongoDB.Bson;
    using System;
    
    public class MongoKioskDto
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string MachineId { get; set; }
        public string Json { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
