namespace Pulse.Core.Dto.Mongo
{
    using Domain.Mongo.Enum;
    using System;

    public class SystemEventDto
    {
        public string MachineName { get; set; }
        public string MachineId { get; set; }
        public string Description { get; set; }
        public ActionType Action { get; set; }
        public string ActionName { get; set; }
        public SystemEventStatus Status { get; set; }
        public string StatusName { get; set; }
        public string CountDate { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
