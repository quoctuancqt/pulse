namespace Pulse.Scheduler.Events
{
    using Pulse.Scheduler.Model;

    public delegate T HandlerEvent<T>(T obj,ScheduleModel agrs);
}
