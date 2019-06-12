namespace Pulse.Domain
{
    using Enum;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Kiosk : IEntity, IAudits
    {
        [Key]
        public int Id { get; set; }
        public string MachineId { get; set; }
        public string ClientId { get; set; }
        public string ConnectionId { get; set; }
        public string IpAddress { get; set; }
        public KioskStatus Status { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Long { get; set; }
        public string Lat { get; set; }
        public string DefaultValue { get; set; }
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

    }
}
