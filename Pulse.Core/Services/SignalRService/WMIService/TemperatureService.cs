namespace Pulse.Core.Services
{
    using System;
    using System.Threading.Tasks;
    using Settings;
    using WMI;
    public class TemperatureService : BaseService
    {
        private const string QUERY = "SELECT * FROM MSAcpi_ThermalZoneTemperature";
        private const string CLASS_NAME = "MSAcpi_ThermalZoneTemperature";
        private const string KEY = "CurrentTemperature";

        public TemperatureService() :
            base(new WMIConnection(null, null, null, SettingsConfigurationCommon.MACHINE_NAME, SettingsConfigurationCommon.CONNECTION_WMI))
        {}

        public override async Task<string> GetValueAsync()
        {
            var propertyDataCollection = await GetPropertyValuesAsync(QUERY, CLASS_NAME);

            string temperature = string.Empty;

            if (propertyDataCollection != null)
            {
                temperature = ConvertFtoC(double.Parse(propertyDataCollection[KEY].Value.ToString())).ToString();
            }

            return $"\"temperature\" : {{ \"value\" : \"{temperature}\" }}";

        }

        private double ConvertFtoC(double fValue)
        {
            return (fValue - 2732) / 10.0;
        }

    }
}
