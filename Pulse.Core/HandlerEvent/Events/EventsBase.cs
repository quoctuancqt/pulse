namespace Pulse.Core.HandlerEvent
{
    using Args;
    using Dto.Mongo;
    using Events;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class EventsBase : IEvents
    {
        protected event HandlerHistory OnHandlerHistory;

        protected static IDictionary<string, HandlerHistory> _events;

        protected IDictionary<string, HandlerHistory> RegisterEvent()
        {
            if (_events == null)
            {
                _events = new Dictionary<string, HandlerHistory>();
                _events.Add(GetKey(EventType.history), OnHandlerHistory);
                return _events;
            }

            return _events;
        }

        protected string GetKey(EventType eventType)
        {
            return Enum.GetName(typeof(EventType), eventType);
        }

        public abstract Task<NotifyKioskDto> TriggerNofityAsync(NotifyKioskArgs args);

        public abstract Task TriggerUpdateStatusAsync(KioskArgs args);

        public abstract Task<UserActivitiesDto> TriggerUserActivitiesAsync(UserActivitiesArgs args);

        public abstract Task<SystemEventDto> TriggerSystemEventAsync(SystemEventArgs args);

        public abstract Task TriggerMongoKioskAsync(MongoKioskArgs args);
    }
}
