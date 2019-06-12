namespace Pulse.Core.Services
{
    using System;
    using System.Threading.Tasks;
    using Settings;
    using WMI;

    public sealed class ProcessorService : BaseService
    {
        private const string QUERY = "SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor";
        private const string CLASS_NAME = "Win32_PerfFormattedData_PerfOS_Processor";
        private const string KEY = "PercentUserTime";

        public ProcessorService() 
            : base(new WMIConnection(null, null, null, SettingsConfigurationCommon.MACHINE_NAME, SettingsConfigurationCommon.CONNECTION_CIMV2))
        {
        }

        public override async Task<string> GetValueAsync()
        {
            var propertyDataCollection = await GetPropertyValuesAsync(QUERY, CLASS_NAME);

            return $"\"cpu\" : {{ \"value\" : \"{propertyDataCollection[KEY].Value.ToString()}\" }}";
            
        }
    }
}
