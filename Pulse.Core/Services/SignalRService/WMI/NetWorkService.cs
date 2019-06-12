using System.Linq;

namespace Pulse.Core.Services
{
    using WMI;
    using Settings;
    using System;
    using System.Threading.Tasks;

    public class NetWorkService : BaseService
    {
        private const string QUERY_NET_WORK_INTERFACE = "SELECT * FROM Win32_PerfFormattedData_Tcpip_NetworkInterface";
        private const string QUERY_NET_WORK_ADAPTER = "SELECT * FROM Win32_NetworkAdapter";
        private const string CLASS_NAME_NET_WORK_INTERFACE = "Win32_PerfFormattedData_Tcpip_NetworkInterface";
        private const string CLASS_NAME_NET_WORK_ADAPTER = "Win32_NetworkAdapter";
        private const string NETENABLES = "NetEnabled";
        private const string NAME = "Name";
        private const string BYTES_TOTAL_PERSEC = "BytesTotalPersec";
        private const string BYTES_SENT_PERSEC = "BytesSentPersec";
        private const string BYTES_RECEIVED_PERSEC = "BytesReceivedPersec";
        private const string CURRENT_BAND_WIDTH = "CurrentBandwidth";

        public NetWorkService()
            : base(new WMIConnection(null, null, null, SettingsConfigurationSignalR.MACHINE_NAME, SettingsConfigurationSignalR.CONNECTION_CIMV2))
        {
        }

        public override Task<string> GetValueAsync()
        {
            return GetCurrentBandwidthAsync();
        }

        private async Task<string> GetActiveNetworkAdapterAsync()
        {
            var instances = await GetAllInstancesAsync(QUERY_NET_WORK_ADAPTER, CLASS_NAME_NET_WORK_ADAPTER);

            foreach (var instance in instances)
            {
                bool _netEnable = instance.Properties[NETENABLES].Value != null ? (bool)instance.Properties[NETENABLES].Value : false;
                if (_netEnable)
                {
                    return instance.Properties[NAME].Value.ToString();
                }
            }

            return string.Empty;
        }

        private async Task<string> GetCurrentBandwidthAsync()
        {
            var output = string.Empty;
            var stringValue = string.Empty;
            var activeNetworkAdapter = await GetActiveNetworkAdapterAsync();
            var instances = await GetAllInstancesAsync(QUERY_NET_WORK_INTERFACE, CLASS_NAME_NET_WORK_INTERFACE);
            
            var enabledInstances =
                instances.Where(x => x.Properties[NAME].Value.ToString().Contains(activeNetworkAdapter));

            foreach (var instance in enabledInstances)
            {
                if (instance.Properties[BYTES_TOTAL_PERSEC].Value == null)
                {
                    continue;
                }

                var _total = int.Parse(instance.Properties[BYTES_TOTAL_PERSEC].Value.ToString());
                var _sent = (int.Parse(instance.Properties[BYTES_SENT_PERSEC].Value.ToString()) / 1024).ToString();
                var _received = (int.Parse(instance.Properties[BYTES_RECEIVED_PERSEC].Value.ToString()) / 1024).ToString();
                var _currentBandwidth = int.Parse(instance.Properties[CURRENT_BAND_WIDTH].Value.ToString());
                var _percentUsaged = ((_total * 8) / _currentBandwidth) * 100;
                stringValue += $"\"usaged\" : \"{_percentUsaged}\",\"total\" : \"{_total}\", \"sent\" : \"{_sent}\", \"received\" : \"{_received}\"}}";
                break;
            }
            

            return output;
        }

    }
}
