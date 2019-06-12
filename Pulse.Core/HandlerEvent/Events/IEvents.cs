namespace Pulse.Core.HandlerEvent
{
    using Args;
    using Dto.Mongo;
    using System.Threading.Tasks;

    public interface IEvents
    {
        Task<NotifyKioskDto> TriggerNofityAsync(NotifyKioskArgs args);

        Task TriggerUpdateStatusAsync(KioskArgs args);

        Task<UserActivitiesDto> TriggerUserActivitiesAsync(UserActivitiesArgs args);

        Task<SystemEventDto> TriggerSystemEventAsync(SystemEventArgs args);

        Task TriggerMongoKioskAsync(MongoKioskArgs args);
    }
}
