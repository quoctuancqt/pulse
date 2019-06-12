namespace Pulse.Core.Settings
{
    using System;
    using System.Configuration;
    public class SettingsConfigurationSignalR
    {
        public static string DOMAIN_SERVER = ConfigurationManager.AppSettings["DOMAIN_SERVER"];
        public static string WEBAPI_URI = ConfigurationManager.AppSettings["WEBAPI_URI"];
        public static bool IS_PULSE_SERVER = Convert.ToBoolean(ConfigurationManager.AppSettings["IS_PULSE_SERVER"]);
    }

    public class SettingsConfigurationWebApi
    {
        public static string DOMAIN_SERVER = ConfigurationManager.AppSettings["DOMAIN_SERVER"];
        public static string WEBAPI_URI = ConfigurationManager.AppSettings["WEBAPI_URI"];
    }

    public class SettingsConfigurationKiosks
    {
        public static string WEBAPI_URI = ConfigurationManager.AppSettings["WEBAPI_URI"];
        public static string HUB_NAME = ConfigurationManager.AppSettings["HUB_NAME"];
        //public static string MACHINE_ID = ConfigurationManager.AppSettings["MACHINE_ID"];
        //public static string GROUP_NAME = ConfigurationManager.AppSettings["GROUP_NAME"];
        //public static string MACHINE_NAME = ConfigurationManager.AppSettings["MACHINE_NAME"];
        //public static string SERVER_URI = ConfigurationManager.AppSettings["SERVER_URI"];
        //public static string COUNTRY = ConfigurationManager.AppSettings["COUNTRY"];
    }

    public class SettingsConfigurationCommon
    {
        public static string MACHINE_NAME = Environment.MachineName;
        public static string CONNECTION_CIMV2 = "CIMV2";
        public static string CONNECTION_WMI = "WMI";
        public static int MILLISECOND = 1000;
        public static int MILLISECOND_EVENT_LOG = 600000;
        public static bool IS_AUTHENTICATION = Convert.ToBoolean(ConfigurationManager.AppSettings["IS_AUTHENTICATION"]);
        public static string WEBAPI_URI = ConfigurationManager.AppSettings["WEBAPI_URI"];
    }
}
