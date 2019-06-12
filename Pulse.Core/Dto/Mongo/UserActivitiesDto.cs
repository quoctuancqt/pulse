namespace Pulse.Core.Dto.Mongo
{
    using Domain.Mongo.Enum;
    using System;

    public class UserActivitiesDto
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public ActionType Action { get; set; }
        public string ActionName { get; set; }
        public string CountDate { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
