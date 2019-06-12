[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4Net/log4net.config", Watch = true)]
namespace Pulse.WebApi
{
    using Core.Settings;
    using System.ServiceProcess;
    using System;
    public class Program
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void Main(string[] args)
      {
#if DEBUG
            using (Microsoft.Owin.Hosting.WebApp.Start<Core.WeApiServer.WebApiConfiguration>(SettingsConfigurationWebApi.DOMAIN_SERVER)) {
                Console.WriteLine("WebApi Started with domain: " + SettingsConfigurationWebApi.DOMAIN_SERVER);
                Console.ReadKey();
            } 
#else
            ServiceBase[] ServicesToRun;

            ServicesToRun = new ServiceBase[]
            {
                new SelfHostService()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
