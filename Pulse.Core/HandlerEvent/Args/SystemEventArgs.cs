namespace Pulse.Core.HandlerEvent.Args
{
    using Pulse.Domain.Mongo.Enum;

    public class SystemEventArgs
    {
        public string MachineName { get; set; }
        public string MachineId { get; set; }
        public string Description { get; set; }
        public ActionType Action { get; set; }
        public SystemEventStatus Status { get; set; }
    }
}
