namespace Pulse.Core.Services
{
    using Settings;
    using System;
    using System.Management;
    using WMI;
    using System.Threading.Tasks;

    public sealed class MemoryService : BaseService
    {
        private const string QUERY_COMPUTER_SYSTEM = "SELECT * FROM Win32_ComputerSystem";
        private const string CLASS_NAME_COMPUTER_SYSTEM = "Win32_ComputerSystem";
        private const string TOTAL_PHYSICAL_MEMORY = "TotalPhysicalMemory";
        private const string QUERY_OPERATING_SYSTEM = "SELECT * FROM Win32_OperatingSystem";
        private const string CLASS_NAME_OPERATING_SYSTEM = "Win32_OperatingSystem";
        private const string FREE_PHYSICAL_MEMORY = "FreePhysicalMemory";

        public MemoryService() 
            : base(new WMIConnection(null, null, null, SettingsConfigurationCommon.MACHINE_NAME, SettingsConfigurationCommon.CONNECTION_CIMV2))
        {
        }

        public override async Task<string> GetValueAsync()
        {
            return $"\"memory\" : {{ \"value\" : \"{await CalculateMemory()}\" }}";
        }

        private async Task<string> CalculateMemory()
        {
            PropertyDataCollection computerSystem = await GetPropertyValuesAsync(QUERY_COMPUTER_SYSTEM, CLASS_NAME_COMPUTER_SYSTEM);
            PropertyDataCollection operatingSystem = await GetPropertyValuesAsync(QUERY_OPERATING_SYSTEM, CLASS_NAME_OPERATING_SYSTEM);

            var totalPhysicalMemory = double.Parse(computerSystem[TOTAL_PHYSICAL_MEMORY].Value.ToString());
            var freePhysicalMemory = double.Parse(operatingSystem[FREE_PHYSICAL_MEMORY].Value.ToString());

            var delta = 1 - ((freePhysicalMemory * 1024) / totalPhysicalMemory);

            return Math.Ceiling((100 * delta) + 0.5).ToString();
        }

    }
}
