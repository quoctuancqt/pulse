namespace Pulse.Scheduler.Factories
{
    using Model;

    public interface ISchedulerManager
    {
        string Create(ScheduleModel scheduler);
    }
}
