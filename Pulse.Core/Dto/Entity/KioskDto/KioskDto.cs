namespace Pulse.Core.Dto.Entity
{
    using Domain.Enum;
    using System;

    public class KioskDto: IDto
    {
        public int Id { get; set; }
        public string MachineId { get; set; }
        public string ConnectionId { get; set; }
        public string ClientId { get; set; }
        public string IpAddress { get; set; }
        public string Name { get; set; }
        public string DefaultValue { get; set; }
        public KioskStatus Status { get; set; }
        public string StatusValue { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Address { get; set; }
        public string Long { get; set; }
        public string Lat { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string DateFormat { get; set; }
        public string DateFormat1 { get; set; }
        public string CountDate { get; set; }
    }
}
