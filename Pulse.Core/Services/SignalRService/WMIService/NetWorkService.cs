namespace Pulse.Core.Services
{
    using WMI;
    using Settings;
    using System.Threading.Tasks;
    using System.Management;
    using System.Collections;
    using System.Linq;
    using System;
    using Common.Helpers;

    public class NetWorkService : BaseService
    {
        private const string QUERY_NET_WORK_INTERFACE = "SELECT * FROM Win32_PerfFormattedData_Tcpip_NetworkInterface";
        private const string QUERY_NET_WORK_ADAPTER = "SELECT * FROM Win32_NetworkAdapter";
        private const string QUERY_NET_WORK_ADAPTER_CONFIGURATION = "SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'True'";
        private const string CLASS_NAME_NET_WORK_INTERFACE = "Win32_PerfFormattedData_Tcpip_NetworkInterface";
        private const string CLASS_NAME_NET_WORK_ADAPTER = "Win32_NetworkAdapter";
        private const string CLASS_NET_WORK_ADAPTER_CONFIGURATION = "Win32_NetworkAdapterConfiguration";
        private const string IP_ADDRESS = "IPAddress";
        private const string NETENABLES = "NetEnabled";
        private const string NAME = "Name";
        private const string BYTES_TOTAL_PERSEC = "BytesTotalPersec";
        private const string BYTES_SENT_PERSEC = "BytesSentPersec";
        private const string BYTES_RECEIVED_PERSEC = "BytesReceivedPersec";
        private const string CURRENT_BAND_WIDTH = "CurrentBandwidth";

        public NetWorkService()
            : base(new WMIConnection(null, null, null, SettingsConfigurationCommon.MACHINE_NAME, SettingsConfigurationCommon.CONNECTION_CIMV2))
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

        private async Task<string> GetIpAddressAsync()
        {
            var activeNetworkAdapter = await GetActiveNetworkAdapterAsync();
            PropertyDataCollection IpAddress = await GetPropertyValuesAsync(QUERY_NET_WORK_ADAPTER_CONFIGURATION, CLASS_NET_WORK_ADAPTER_CONFIGURATION);
            string[] output = ((IEnumerable)IpAddress[IP_ADDRESS].Value).Cast<object>().Select(x => x.ToString()).ToArray();
            return output[0];
        }

        private async Task<string> GetCurrentBandwidthAsync()
        {
            var output = string.Empty;
            var stringValue = string.Empty;
            var activeNetworkAdapter = await GetActiveNetworkAdapterAsync();
            var ipAddress = await GetIpAddressAsync();
            var instances = await GetAllInstancesAsync(QUERY_NET_WORK_INTERFACE, CLASS_NAME_NET_WORK_INTERFACE);
            var enabledInstances = instances.Where(x => x.Properties[NAME].Value.ReplaceString().Contains(activeNetworkAdapter.ReplaceString()));

            foreach (var instance in enabledInstances)
            {
                if (instance.Properties[BYTES_TOTAL_PERSEC].Value.ToString() == "0") continue;

                var total = int.Parse(instance.Properties[BYTES_TOTAL_PERSEC].Value.ToString());
                var sent = (int.Parse(instance.Properties[BYTES_SENT_PERSEC].Value.ToString()) / 1024).ToString();
                var received = (int.Parse(instance.Properties[BYTES_RECEIVED_PERSEC].Value.ToString()) / 1024).ToString();
                var currentBandwidth = int.Parse(instance.Properties[CURRENT_BAND_WIDTH].Value.ToString());
                var percentUsaged = ((total * 8) / (currentBandwidth == 0 ? 1 : currentBandwidth)) / 100;
                stringValue += $"\"usaged\" : \"{percentUsaged}\",\"total\" : \"{total}\", \"sent\" : \"{sent}\", \"received\" : \"{received}\", \"ipAddress\" : \"{ipAddress}\"}}";
                break;
            }

            if (!string.IsNullOrEmpty(stringValue))
            {
                output = "\"network\" : {" + stringValue;
            }
            else
            {
                output = "\"network\" : {" + $"\"usaged\" : \"0\",\"total\" : \"0\", \"sent\" : \"0\", \"received\" : \"0\", \"ipAddress\" : \"0\"}}";
            }

            return output;
        }

    }
}
