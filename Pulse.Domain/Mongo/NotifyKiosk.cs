namespace Pulse.Domain.Mongo
{
    using System;
    using MongoDB.Bson;
    
    public class NotifyKiosk : IMongo
    {
        public ObjectId Id { get; set; }
        public string MachineId { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public string UserId { get; set; }
        public bool IsRead { get; set; }
        public string Status { get; set; }
        public string Content { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
