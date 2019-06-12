namespace Pulse.Scheduler.Factories
{
    using System;
    using System.Collections.Generic;
    using Quartz;
    using Quartz.Impl;
    using Events;
    using Enum;
    using Model;
    using Jobs;

    public abstract class SchedulerBase
    {
        protected IScheduler Scheduler;

        private IDictionary<string, HandlerEvent<TriggerBuilder>> scheduleMethodDic;

        public SchedulerBase()
        {
            InitializeSchedulerFactory();

            InitializeScheduleMethodDic();
        }

        protected IJobDetail CreateJob(string jobName, string jobGroup)
        {
            IJobDetail jobDetail = JobBuilder.Create<RunScriptJob>()
                    .WithIdentity(jobName, jobGroup)
                    .Build();

            return jobDetail;
        }

        protected ITrigger CreateTrigger(string triggerName, ScheduleModel model)
        {
            var triggerBuilder = TriggerBuilder.Create().WithIdentity(triggerName, model.Name);

            triggerBuilder = scheduleMethodDic[Enum.GetName(typeof(TriggerType), model.TriggerType)].Invoke(triggerBuilder, model);

            return triggerBuilder.Build();
        }

        #region Private Methods

        private void InitializeSchedulerFactory()
        {
            var factory = new StdSchedulerFactory();

            Scheduler = factory.GetScheduler();
        }

        private void InitializeScheduleMethodDic()
        {
            scheduleMethodDic = new Dictionary<string, HandlerEvent<TriggerBuilder>>();

            scheduleMethodDic.Add(Enum.GetName(typeof(TriggerType), TriggerType.OneTime), AddOneTime);

            scheduleMethodDic.Add(Enum.GetName(typeof(TriggerType), TriggerType.Daily), AddDaily);

            scheduleMethodDic.Add(Enum.GetName(typeof(TriggerType), TriggerType.Weekly), AddWeekly);

            scheduleMethodDic.Add(Enum.GetName(typeof(TriggerType), TriggerType.Monthly), AddMonthly);
        }

        private void ScheduleJob(IJobDetail job, ITrigger trigger)
        {
            Scheduler.ScheduleJob(job, trigger);
        }

        private void ScheduleJob(ITrigger trigger)
        {
            Scheduler.ScheduleJob(trigger);
        }

        private TriggerBuilder AddOneTime(TriggerBuilder triggerBuilder, ScheduleModel scheduler)
        {
            return triggerBuilder.StartAt(scheduler.StarTime);
        }
        
        private TriggerBuilder AddDaily(TriggerBuilder triggerBuilder, ScheduleModel scheduler)
        {
            return triggerBuilder.WithSchedule(
                CronScheduleBuilder.DailyAtHourAndMinute(scheduler.StarTime.Hour, scheduler.StarTime.Minute));
        }

        private TriggerBuilder AddWeekly(TriggerBuilder triggerBuilder, ScheduleModel scheduler)
        {
            var startTime = scheduler.StarTime;
            return triggerBuilder.WithSchedule(
                CronScheduleBuilder.WeeklyOnDayAndHourAndMinute(startTime.DayOfWeek, startTime.Hour, startTime.Minute));
        }

        private TriggerBuilder AddMonthly(TriggerBuilder triggerBuilder, ScheduleModel scheduler)
        {
            var startTime = scheduler.StarTime;
            return triggerBuilder.WithSchedule(
                CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(startTime.Day, startTime.Hour, startTime.Minute));
        }

        #endregion
        
    }
}
