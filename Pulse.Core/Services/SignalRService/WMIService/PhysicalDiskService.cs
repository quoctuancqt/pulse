namespace Pulse.Core.Services
{
    using WMI;
    using Settings;
    using System;
    using System.Threading.Tasks;

    public sealed class PhysicalDiskService : BaseService
    {
        private const string QUERY = "SELECT * FROM Win32_PerfFormattedData_PerfDisk_PhysicalDisk";
        private const string CLASS_NAME = "Win32_PerfFormattedData_PerfDisk_PhysicalDisk";
        private const string KEY = "PercentIdleTime";

        public PhysicalDiskService() 
            : base(new WMIConnection(null, null, null, SettingsConfigurationCommon.MACHINE_NAME, SettingsConfigurationCommon.CONNECTION_CIMV2))
        {}

        public override async Task<string> GetValueAsync()
        {
            var propertyDataCollection = await GetPropertyValuesAsync(QUERY, CLASS_NAME);
            return $"\"disk\" : {{ \"value\" : \"{(100 - int.Parse(propertyDataCollection[KEY].Value.ToString())).ToString()}\" }}";
        }
    }
}
