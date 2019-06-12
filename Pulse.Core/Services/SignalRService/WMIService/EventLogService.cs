namespace Pulse.Core.Services
{
    using System;
    using WMI;
    using Settings;
    using System.Management;
    using System.Linq;
    using System.Threading.Tasks;

    public class EventLogService : BaseService
    {
        private const string QUERY = "SELECT * FROM Win32_NTLogEvent Where Logfile = 'System' and TimeGenerated > ";
        private const string CLASS_NAME_EVENT_LOG = "Win32_NTLogEvent";
        private const string TYPE = "Type";
        private const string INFORMATION = "Information";
        private const string WAWRNING = "Warning";
        private const string ERROR = "Error";
        private static int MILLISECOND_EVENT_LOG = 0;
        private static string JSON_RESULT = string.Empty;

        public EventLogService() : base(new WMIConnection(null, null, null, SettingsConfigurationCommon.MACHINE_NAME, SettingsConfigurationCommon.CONNECTION_CIMV2))
        {
        }

        public override async Task<string> GetValueAsync()
        {
            return await GetEventLogSummaryAsync();
        }

        private async Task<string> GetEventLogSummaryAsync()
        {
            if (MILLISECOND_EVENT_LOG == 0)
            {
                MILLISECOND_EVENT_LOG = MILLISECOND_EVENT_LOG + 1000;

                var startTime = ManagementDateTimeConverter.ToDmtfDateTime(DateTime.UtcNow.AddDays(-1));

                var eventLogs = await GetAllInstancesAsync($"{QUERY} '{startTime}'", CLASS_NAME_EVENT_LOG);

                var eventLogInformation = eventLogs.Where(t => t.Properties[TYPE].Value.ToString().Equals(INFORMATION)).ToList();
                var eventLogWarning = eventLogs.Where(t => t.Properties[TYPE].Value.ToString().Equals(WAWRNING)).ToList();
                var eventLogError = eventLogs.Where(t => t.Properties[TYPE].Value.ToString().Equals(ERROR)).ToList();

                JSON_RESULT = $"\"eventLog\" : {{ \"Information\" : \"{GetPercentValueOfEventLog(eventLogInformation.Count, eventLogs.Count)}\", \"Warning\" : \"{GetPercentValueOfEventLog(eventLogWarning.Count, eventLogs.Count)}\", \"Error\" : \"{GetPercentValueOfEventLog(eventLogError.Count, eventLogs.Count)}\"}}";

                return JSON_RESULT;
            }
            else
            {
                MILLISECOND_EVENT_LOG = MILLISECOND_EVENT_LOG + 1000;

                if (MILLISECOND_EVENT_LOG == SettingsConfigurationCommon.MILLISECOND_EVENT_LOG)
                {
                    MILLISECOND_EVENT_LOG = 0;
                }

                return JSON_RESULT;
            }
        }

        private string GetPercentValueOfEventLog(double value, double total)
        {
            return Math.Round((value / total) * 100).ToString();
        }
    }
}
