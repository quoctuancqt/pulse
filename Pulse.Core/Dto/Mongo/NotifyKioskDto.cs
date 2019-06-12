namespace Pulse.Core.Dto.Mongo
{
    using System;

    public class NotifyKioskDto
    {
        public string MachineId { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public string UserId { get; set; }
        public string ClientId { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public bool IsRead { get; set; }
        public string CountDate { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
