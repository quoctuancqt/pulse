namespace Pulse.Scheduler.Model
{
    using System;
    using Enum;
    using Quartz;

    public class ScheduleModel
    {
        public string Name { get; set; }

        public string JobName { get; set; }

        public TriggerType TriggerType { get; set; }

        public DateTime StarTime { get; set; }
    }
}
