[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4Net/log4net.config", Watch = true)]
namespace Pulse.OwinServer
{
    using System;
    using Core.SignalRServer;
    using Core.Settings;
    using Core.Mapper.SignalR;
    using Common.ResolverFactories;
    using System.ServiceProcess;
    using Core.IoC.SignalR;
    using Common.Helpers;
    using log4net;
    using Mongo.Factories;

    public class Program
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger
   (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void Main(string[] args)
        {
            AutoMapperConfiguration.Config();
            NinjectConfiguration.Config();
#if DEBUG
            using (Microsoft.Owin.Hosting.WebApp.Start<SignalRConfiguration>(SettingsConfigurationSignalR.DOMAIN_SERVER))
            {
                Console.WriteLine("OwinServer Started....");
                Console.ReadKey();
            }
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { new SelfHostService() };
            ServiceBase.Run(ServicesToRun);
#endif

        }

    }
}
