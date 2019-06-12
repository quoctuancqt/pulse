namespace Pulse.Domain.Mongo
{
    using Enum;
    using MongoDB.Bson;
    using System;

    public class UserActivities: IMongo
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public ActionType Action { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
