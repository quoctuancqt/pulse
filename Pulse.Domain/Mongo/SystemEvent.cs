namespace Pulse.Domain.Mongo
{
    using System;
    using MongoDB.Bson;
    using Enum;

    public class SystemEvent : IMongo
    {
        public ObjectId Id { get; set; }
        public string MachineName { get; set; }
        public string MachineId { get; set; }
        public string Description { get; set; }
        public ActionType Action { get; set; }
        public SystemEventStatus Status { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
