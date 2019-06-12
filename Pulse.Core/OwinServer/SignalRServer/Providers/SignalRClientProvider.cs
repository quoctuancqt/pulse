namespace Pulse.Core.OwinServer.SignalRServer.Providers
{
    using Dto.Entity;
    using Microsoft.AspNet.SignalR.Client;
    using Services;
    using Settings;
    using SignalR.Client;
    using Common.Helpers;
    using System;
    using System.Threading.Tasks;
    using log4net;
    using Domain.Enum;
    using Common.GeoLocation;

    public abstract class SignalRClientProvider
    {
        private const string LICENSE_KEY = "LicenseKey";

        private const string CLIENT_ID = "ClientId";

        private const string MACHINE_ID = "MachineId";

        protected readonly ICollectDeviceService _collectDeviceService;

        protected readonly PulseSignalRClient _signalR;

        private static bool _isStartCollectData = false;

        protected readonly IApiService _apiService;

        private readonly ILog _log = LogManager.GetLogger(typeof(SignalRClientProvider));

        private static KioskDto _kioskDto;

        private static ClientDto _clientDto;

        public SignalRClientProvider(ICollectDeviceService collectDeviceService, PulseSignalRClient signalR,
            IApiService apiService)
        {
            _log.Debug("Begin SignalRClientProvider");
            _collectDeviceService = collectDeviceService;
            _apiService = apiService;
            _signalR = signalR;
        }

        public virtual void Start()
        {
            _log.Debug("Begin Start");

            _log.Debug("ProcessKioskAsync");

            _kioskDto = AsyncHelper.RunSync(() => ProcessKioskAsync());

            _log.Debug("GetClientByClientId");

            _clientDto = AsyncHelper.RunSync(() => _apiService.GetClientByClientId(KioskConfigurationHepler.GetValueFromSecurity(CLIENT_ID)));

            _log.Debug("Begin connect signalR server");

            _signalR.OnSuccess += OnSuccess;
            _signalR.OnReconnect += OnReconnect;
            _signalR.OnConnectionSlow += OnConnectionSlow;
            _signalR.OnReconnecting += OnReconnecting;
            _signalR.OnBeforeStop += OnBeforeStop;

            _signalR.Run(new ObjectState(_kioskDto, _clientDto));

            _log.Debug("End Start");
        }

        public virtual void Stop()
        {
            _log.Debug("Begin Stop");
            _isStartCollectData = false;
            _signalR.StopConnection();
            _log.Debug("End Stop");
        }

        public virtual async Task DoWork(object obj)
        {
            _log.Debug("Begin DoWork");
            var result = await _collectDeviceService.CollectDeviceInfoAsync();
            _log.Debug("result :" + result);
            Console.WriteLine(result);
            _signalR.SendPerfStatusUpdateToGroups(result);
            GC.Collect();
        }

        #region Handler SignalR
        public void OnSuccess(IHubProxy hubProxy)
        {
            _log.Debug("Connected signalr OnSuccess");
            do
            {
                if (!_isStartCollectData)
                {
                    _apiService.UpdateConnectionIdAsync(KioskConfigurationHepler.GetValueFromSecurity(MACHINE_ID), _signalR.ConnectionId);
                    _isStartCollectData = true;
                }

                AsyncHelper.RunSync(() => DoWork(null));

            } while (_isStartCollectData);
        }

        public void OnConnectionSlow(IHubProxy hubProxy)
        {
            _log.Debug("OnConnectionSlow");
            StopTimer(hubProxy);
        }

        public void OnReconnecting(IHubProxy hubProxy)
        {
            _log.Debug("OnReconnecting");
            StopTimer(hubProxy);
        }

        public void OnReconnect(IHubProxy hubProxy)
        {
            _log.Debug("OnReconnect");
            OnSuccess(hubProxy);
        }

        public void OnBeforeStop(IHubProxy hubProxy)
        {
            //Todo something
        }

        private void StopTimer(IHubProxy hubProxy)
        {
            _log.Debug("Stop Signalr");
            _isStartCollectData = false;
        }
        #endregion

        #region  Private Method

        private async Task<KioskDto> ProcessKioskAsync()
        {
            try
            {
                _log.Debug("Begin ProcessKioskAsync");

                var kiosk = await _apiService.FindKioskByMachineIdAsync(KioskConfigurationHepler.GetValueFromSecurity(MACHINE_ID));

                if (kiosk == null)
                {
                    CountryDto countryDto = await _apiService.FindCountryByNameAsync(GeoLocaltion.GetCountryName());

                    GroupDto groupDto = await _apiService.FindGroupByNameAsync("Unknow");

                    return await CreateKioskAsync(countryDto, groupDto);
                }
                else
                {
                    return await UpdateKioskAsync(kiosk, kiosk.GroupId, kiosk.CountryId);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw new Exception("Please check log with error from SignalRClientProvider ==> ProcessKioskAsync: " + ex.Message, ex);
            }

        }
        private async Task<KioskDto> CreateKioskAsync(CountryDto countryDto, GroupDto groupDto)
        {
            _log.Debug("Begin ProcessKioskAsync Create");

            var defaultValue = await _collectDeviceService.CollectDeviceInfoAsync();

            _log.Debug("defaultValue: " + defaultValue);

           var kiosk = await _apiService.CreateKioskAsync(new KioskDto
            {
                MachineId = KioskConfigurationHepler.GetValueFromSecurity(MACHINE_ID),
                Name = SettingsConfigurationCommon.MACHINE_NAME,
                GroupId = groupDto.Id,
                CountryId = countryDto.Id,
                IpAddress = UnitHelper.GetLocalIPAddress(),
                Address = GeoLocaltion.GetCountryName(),
                DefaultValue = defaultValue,
                Status = KioskStatus.online,
                Long = GeoLocaltion.GetLongitude(),
                Lat = GeoLocaltion.GetLatitude()
           });

            _log.Debug("UnitHelper.GetMacAddress() " + UnitHelper.GetMacAddress());

            await _apiService.UpdateKioskSecurityByKeyAsync(KioskConfigurationHepler.GetValueFromSecurity(LICENSE_KEY), new KioskSecurityDto
            {
                MacAddress = UnitHelper.GetMacAddress(),
                IsActive = true
            });

            return kiosk;
        }
        private async Task<KioskDto> UpdateKioskAsync(KioskDto kiosk, int groupId, int countryId)
        {
            _log.Debug("Begin ProcessKioskAsync Update");
            var defaultValue = await _collectDeviceService.CollectDeviceInfoAsync();
            _log.Debug("defaultValue: " + defaultValue);
            kiosk.Name = SettingsConfigurationCommon.MACHINE_NAME;
            kiosk.GroupId = groupId;
            kiosk.CountryId = countryId;
            kiosk.IpAddress = UnitHelper.GetLocalIPAddress();
            kiosk.DefaultValue = defaultValue;
            kiosk.Status = KioskStatus.online;
            kiosk = await _apiService.UpdateKioskAsync(kiosk);
            return kiosk;
        }

        #endregion
    }
}
