namespace Pulse.Core.Services
{
    using Dto.Entity;
    using log4net;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Enum;
    using System.Net.Http;
    using System;
    using Dto.Mongo;
    using Domain.Mongo.Enum;

    public abstract class BaseApiService : IApiService
    {
        protected readonly IProxyService _proxyService;

        protected readonly ILog _log = LogManager.GetLogger(typeof(KioskApiService));

        public BaseApiService(IProxyService proxyService)
        {
            _proxyService = proxyService;
        }

        #region Client
        public virtual async Task<ClientDto> GetClientByClientId(string clientId)
        {
            HttpResponseMessage response = await _proxyService.GetAsync("/api/clients/byclientid/" + clientId);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<ClientDto>();
        }
        #endregion

        #region Oauth
        public virtual async Task<IDictionary<string, string>> GetToken(string username, string password)
        {
            return await _proxyService.EnsureApiTokenAsync(username, password);
        }

        public virtual async Task<IDictionary<string, string>> RefreshToken(string refreshId)
        {
            return await _proxyService.RefreshApiTokenAsync(refreshId);
        }
        #endregion

        #region kiosks

        public virtual async Task<KioskSecurityDto> CheckLicenseKeyAsync(string key)
        {
            _log.Debug("CheckLicenseKeyAsync");
            HttpResponseMessage response = await _proxyService.GetAsync("/api/kiosks/checklicensekey/" + key);
            _log.Debug(response);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<KioskSecurityDto>();
        }

        public virtual async Task<KioskDto> FindKioskByMachineIdAsync(string machineId)
        {
            HttpResponseMessage response = await _proxyService.GetAsync("/api/kiosks/bymachineid/" + machineId);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<KioskDto>();
        }

        public virtual async Task<KioskDto> CreateKioskAsync(KioskDto kioskDto)
        {
            HttpResponseMessage response = await _proxyService.PostJsonAsync("/api/kiosks", new
            {
                machineId = kioskDto.MachineId,
                name = kioskDto.Name,
                countryId = kioskDto.CountryId,
                countryName = kioskDto.CountryName,
                address = kioskDto.Address,
                groupId = kioskDto.GroupId,
                groupName = kioskDto.GroupName,
                ipAddress = kioskDto.IpAddress,
                defaultValue = kioskDto.DefaultValue,
                status = kioskDto.Status,
                @Lat = kioskDto.Lat,
                @long = kioskDto.Long,
            });

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<KioskDto>();
        }

        public virtual async Task<KioskDto> UpdateKioskAsync(KioskDto kioskDto)
        {
            HttpResponseMessage response = await _proxyService.PutJsonAsync("/api/kiosks", new
            {
                id = kioskDto.Id,
                machineId = kioskDto.MachineId,
                name = kioskDto.Name,
                countryId = kioskDto.CountryId,
                countryName = kioskDto.CountryName,
                groupId = kioskDto.GroupId,
                groupName = kioskDto.GroupName,
                ipAddress = kioskDto.IpAddress,
                defaultValue = kioskDto.DefaultValue,
                status = kioskDto.Status,
                address = kioskDto.Address,
                @long = kioskDto.Long,
                @Lat = kioskDto.Lat,
            });

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<KioskDto>();
        }

        public virtual async Task<KioskDto> UpdateStatusByMachineIdAsync(string machineId, KioskStatus kioskStatus)
        {
            HttpResponseMessage response = await _proxyService.PutJsonAsync(string.Format("/api/kiosks/{0}/updatestatus", machineId), new
            {
                status = kioskStatus,
            });

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<KioskDto>();
        }

        public virtual async Task UpdateKioskSecurityByKeyAsync(string key, KioskSecurityDto kioskSecurityDto)
        {
            HttpResponseMessage response = await _proxyService.PutJsonAsync("/api/kiosks/" + key + "/updateKiosksecuritybykey", new
            {
                macAddress = kioskSecurityDto.MacAddress,
                isActive = kioskSecurityDto.IsActive
            });
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateConnectionIdAsync(string machineId, string connectionId)
        {
            HttpResponseMessage response = await _proxyService.PutJsonAsync("/api/kiosks/" + machineId + "/updateconnectionid", new
            {
                connectionId = connectionId,
            });
            response.EnsureSuccessStatusCode();
        }

        #endregion

        #region countries
        public virtual async Task<CountryDto> FindCountryByNameAsync(string countryName)
        {
            HttpResponseMessage response = await _proxyService.GetAsync("/api/countries/byname?name=" + countryName);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<CountryDto>();
        }

        public virtual async Task<IEnumerable<CountryDto>> FindAllCountryAsync()
        {
            HttpResponseMessage response = await _proxyService.GetAsync("/api/countries");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<IEnumerable<CountryDto>>();
        }
        #endregion

        #region Groups
        public virtual async Task<GroupDto> FindGroupByNameAsync(string groupName)
        {
            HttpResponseMessage response = await _proxyService.GetAsync("/api/groups/byname?name=" + groupName);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<GroupDto>();
        }

        public virtual async Task<IEnumerable<GroupDto>> FindAllGroupAsync()
        {
            HttpResponseMessage response = await _proxyService.GetAsync("/api/groups");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<IEnumerable<GroupDto>>();
        }

        #endregion

        #region History
        public virtual async Task<HistoryDto> CreateHistoryAsync(HistoryDto historyDto)
        {
            HttpResponseMessage response = await _proxyService.PostJsonAsync("/api/histories", new
            {
                processType = historyDto.ProcessType,
                comment = historyDto.Comment,
                historyId = historyDto.HistoryId,
                historyType = historyDto.HistoryType,
                historyName = historyDto.HistoryName,
                machineId = historyDto.MachineId,
            });

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<HistoryDto>();

        }
        #endregion

        #region Mongo
        public async Task<NotifyKioskDto> AddNotifyAsync(NotifyKioskDto dto)
        {
            HttpResponseMessage response = await _proxyService.PostJsonAsync("/api/notify", new
            {
                machineId = dto.MachineId,
                name = dto.Name,
                groupName = GroupType.ALL,
                clientId = dto.ClientId,
                content = dto.Content,
                status = dto.Status,
                isRead = false
            });

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<NotifyKioskDto>();
        }

        public async Task<UserActivitiesDto> AddUserActivitiesAsync(UserActivitiesDto dto)
        {
            HttpResponseMessage response = await _proxyService.PostJsonAsync("/api/useractivities", new
            {
                name = dto.Name,
                action = dto.Action
            });

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<UserActivitiesDto>();
        }

        public async Task<SystemEventDto> AddSystemEventAsync(SystemEventDto dto)
        {
            HttpResponseMessage response = await _proxyService.PostJsonAsync("/api/systemevents", new
            {
                machineName = dto.MachineName,
                machineId = dto.MachineId,
                description = dto.Description,
                action = dto.Action,
                status = dto.Status
            });

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<SystemEventDto>();
        }

        public async Task AddjsonToMongoKiosk(MongoKioskDto dto)
        {
            HttpResponseMessage response = await _proxyService.PostJsonAsync("/api/kiosks/addjsonkiosk", new
            {
                machineId = dto.MachineId,
                name = dto.Name,
                json = dto.Json
            });

            response.EnsureSuccessStatusCode();
        }

        #endregion
    }
}
