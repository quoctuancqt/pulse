namespace Pulse.Core.HandlerEvent
{
    using Args;
    using Services;
    using System.Threading.Tasks;
    using System;
    using Dto.Mongo;

    public class SignalREventHandlers : EventsBase
    {
        private readonly string _bearer;

        public SignalREventHandlers(string bearer)
        {
            _bearer = bearer;
            _events = RegisterEvent();
        }

        public override async Task TriggerMongoKioskAsync(MongoKioskArgs args)
        {
            IApiService apiService = new SignalRServerApiService(_bearer);

            await apiService.AddjsonToMongoKiosk(new MongoKioskDto
            {
                Name = args.Name,
                MachineId = args.MachineId,
                Json = args.Json,
            });
        }

        public override async Task<NotifyKioskDto> TriggerNofityAsync(NotifyKioskArgs args)
        {
            IApiService apiService = new SignalRServerApiService(_bearer);

           var result = await apiService.AddNotifyAsync(new NotifyKioskDto
            {
                MachineId = args.MachineId,
                Name = args.Name,
                GroupName = args.GroupName,
                Status = args.Status,
                Content = args.Content,
                CreateAt = DateTime.UtcNow
            });

            return result;

        }

        public override async Task<SystemEventDto> TriggerSystemEventAsync(SystemEventArgs args)
        {
            IApiService apiService = new SignalRServerApiService(_bearer);

            var result = await apiService.AddSystemEventAsync(new SystemEventDto
            {
                MachineName = args.MachineName,
                MachineId = args.MachineId,
                Action = args.Action,
                Description = args.Description,
                Status = args.Status,
                CreateAt = DateTime.UtcNow
            });

            return result;
        }

        public override async Task TriggerUpdateStatusAsync(KioskArgs args)
        {
            IApiService apiService = new SignalRServerApiService(_bearer);

            await apiService.UpdateStatusByMachineIdAsync(args.MachineId, args.kioskStatus);
        }

        public override async Task<UserActivitiesDto> TriggerUserActivitiesAsync(UserActivitiesArgs args)
        {
            IApiService apiService = new SignalRServerApiService(_bearer);

            var result = await apiService.AddUserActivitiesAsync(new UserActivitiesDto
            {
                Name = args.Name,
                Action = args.Action,
                UserId = args.UserId
            });

            return result;
        }
    }
}
