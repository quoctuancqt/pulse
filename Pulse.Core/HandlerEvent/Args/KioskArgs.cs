namespace Pulse.Core.HandlerEvent.Args
{
    using Domain.Enum;
    
    public class KioskArgs
    {
        public string MachineId { get; set; }
        public KioskStatus kioskStatus { get; set; }
    }
}
