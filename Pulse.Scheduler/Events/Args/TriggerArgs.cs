namespace Pulse.Scheduler.Events.Args
{
    using Enum;
    using System;

    public class TriggerArgs
    {
        public string TriggerName { get; set; }
        public string TriggerGroup { get; set; }
        public TriggerType TriggerType { get; set; }
        public DateTime StarTime { get; set; }
    }
}
