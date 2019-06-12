namespace Pulse.Core.Services
{
    using Domain.Mongo;
    using Dto.Mongo;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMongoService : IBaseMongoService
    {
        Task<MongoState> CreateDatabaseAsync();

        Task ReadNotifyAsync(string userId);

        Task ReadNotifyAsync();

        Task<NotifyKioskDto> CreateNotifyKioskAsync(NotifyKioskDto dto);

        Task<IEnumerable<NotifyKioskDto>> GetNotifyAsync();

        Task<ClientUserDto> CreateClientUserAsync(ClientUserDto dto);

        Task<UserActivitiesDto> CreateUserActivitiesAsync(UserActivitiesDto dto);

        Task<IEnumerable<UserActivitiesDto>> GetUserActivitiesAsync();

        Task<SystemEventDto> CreateSystemEventAsync(SystemEventDto dto);

        Task<IEnumerable<SystemEventDto>> GetSystemEventAsync();

    }
}
