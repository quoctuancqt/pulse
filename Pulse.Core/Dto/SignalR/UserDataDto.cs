namespace Pulse.Core.Dto.SignalR
{
    using System;

    public class UserDataDto
    {
        public string Token { get; set; }
        public string RefreshId { get; set; }
        public string ClientId { get; set; }
        public string MachineId { get; set; }
        public string ConnectionId { get; set; }
        public string SystemName { get; set; }
        public string GroupName { get; set; }
        public DateTime ConnectedAt { get; set; }
    }
}
