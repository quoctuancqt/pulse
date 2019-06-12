namespace Pulse.Core.Services
{
    using Domain.Enum;
    using Dto.Entity;
    using Dto.Mongo;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IApiService
    {
        Task<ClientDto> GetClientByClientId(string clientId);

        Task<IDictionary<string, string>> GetToken(string username, string password);

        Task<IDictionary<string, string>> RefreshToken(string refreshId);

        Task<KioskDto> FindKioskByMachineIdAsync(string machineId);

        Task<KioskDto> CreateKioskAsync(KioskDto kioskDto);

        Task<KioskDto> UpdateKioskAsync(KioskDto kioskDto);

        Task<KioskDto> UpdateStatusByMachineIdAsync(string machineId, KioskStatus kioskStatus);
        
        Task UpdateKioskSecurityByKeyAsync(string key, KioskSecurityDto kioskSecurityDto);

        Task<KioskSecurityDto> CheckLicenseKeyAsync(string key);

        Task<CountryDto> FindCountryByNameAsync(string countryName);

        Task<IEnumerable<CountryDto>> FindAllCountryAsync();

        Task<IEnumerable<GroupDto>> FindAllGroupAsync();

        Task<GroupDto> FindGroupByNameAsync(string groupName);

        Task<HistoryDto> CreateHistoryAsync(HistoryDto historyDto);

        Task UpdateConnectionIdAsync(string machineId, string connectionId);

        Task<NotifyKioskDto> AddNotifyAsync(NotifyKioskDto dto);

        Task<UserActivitiesDto> AddUserActivitiesAsync(UserActivitiesDto dto);

        Task<SystemEventDto> AddSystemEventAsync(SystemEventDto dto);

        Task AddjsonToMongoKiosk(MongoKioskDto dto);
    }
}
