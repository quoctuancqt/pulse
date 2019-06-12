namespace Pulse.Core.HandlerEvent
{
    using Args;
    using Common.Helpers;
    using Common.ResolverFactories;
    using Dto.Mongo;
    using Services;
    using System;
    using System.Threading.Tasks;

    public class WebApiEventHandlers : EventsBase
    {
        private IMongoService _mongoService;

        public WebApiEventHandlers(IMongoService mongoService)
        {
            _events = RegisterEvent();
            _mongoService = mongoService;
        }

        public override Task TriggerMongoKioskAsync(MongoKioskArgs args)
        {
            throw new NotImplementedException();
        }

        public override Task<NotifyKioskDto> TriggerNofityAsync(NotifyKioskArgs args)
        {
            throw new NotImplementedException();
        }

        public override Task<SystemEventDto> TriggerSystemEventAsync(SystemEventArgs args)
        {
            throw new NotImplementedException();
        }

        public override Task TriggerUpdateStatusAsync(KioskArgs args)
        {
            throw new NotImplementedException();
        }

        public override async Task<UserActivitiesDto> TriggerUserActivitiesAsync(UserActivitiesArgs args)
        {
            throw new NotImplementedException();
        }
    }
}
