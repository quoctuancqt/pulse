namespace Pulse.Scheduler.Factories
{
    using Model;
    using System;
    using Quartz;
    using System.Collections.Generic;
    using Common.Helpers;

    public class SchedulerManager : SchedulerBase, ISchedulerManager
    {
        public void Start()
        {
            Scheduler.Start();
        }

        public void Stop()
        {
            Scheduler.Shutdown();
        }

        public string Create(ScheduleModel scheduler)
        {
            var jobName = UnitHelper.GenerateNewGuid();

            scheduler.JobName = jobName;

            IJobDetail job = CreateJob(jobName, scheduler.Name);

            var triggerName = UnitHelper.GenerateNewGuid();

            ITrigger trigger = CreateTrigger(triggerName, scheduler);

            Scheduler.ScheduleJob(job, trigger);
            
            return jobName;
        }

        public ScheduleModel GetScheduleByName(string name)
        {
            return null;
        }

        public IEnumerable<ScheduleModel> GetAllSchedules()
        {
            return null;
        }

        public void UpdateSchedule(ScheduleModel scheduleModel)
        {
            var jobName = scheduleModel.JobName;
            
            IJobDetail job = CreateJob(scheduleModel.JobName, scheduleModel.Name);

            var triggerName = UnitHelper.GenerateNewGuid();

            ITrigger trigger = CreateTrigger(triggerName, scheduleModel);

            Scheduler.AddJob(job, true);
        }

    }
}
