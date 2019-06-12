namespace Pulse.Setting
{
    using Entity;
    using System;
    using System.Configuration;
    public class HelperConfiguration
    {
        private ExeConfigurationFileMap _map = new ExeConfigurationFileMap();

        private Configuration _config = null;

        public HelperConfiguration()
        {
            _map.ExeConfigFilename = string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, "Pulse.Kiosk.exe.config");
            _config = ConfigurationManager.OpenMappedExeConfiguration(_map, ConfigurationUserLevel.None);
        }

        public PulseSetting ReadData()
        {
            return GetOwinConfiguration();
        }

        public void WriteData(PulseSetting obj)
        {
            _config.AppSettings.Settings[ConfigurationKey.MACHINE_ID].Value = obj.MachineId;
            _config.AppSettings.Settings[ConfigurationKey.MACHINE_NAME].Value = obj.MachineName;
            _config.AppSettings.Settings[ConfigurationKey.SERVER_URI].Value = obj.ServerUri;
            _config.AppSettings.Settings[ConfigurationKey.WEBAPI_URI].Value = obj.WebApiUri;
            _config.AppSettings.Settings[ConfigurationKey.COUNTRY].Value = obj.CountryName;
            _config.AppSettings.Settings[ConfigurationKey.GROUP_NAME].Value = obj.GroupName;
            _config.Save(ConfigurationSaveMode.Modified);
        }

        private PulseSetting GetOwinConfiguration()
        {
            var settings = _config.GetSection("appSettings") as AppSettingsSection;

            var obj = new PulseSetting
            {
                ServerUri = settings.Settings[ConfigurationKey.SERVER_URI].Value,
                WebApiUri = settings.Settings[ConfigurationKey.WEBAPI_URI].Value,
                MachineId = settings.Settings[ConfigurationKey.MACHINE_ID].Value,
                MachineName = settings.Settings[ConfigurationKey.MACHINE_NAME].Value,
                CountryName = settings.Settings[ConfigurationKey.COUNTRY].Value,
                GroupName = settings.Settings[ConfigurationKey.GROUP_NAME].Value,

            };

            return obj;
        }

    }
}
