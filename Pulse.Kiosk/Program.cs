[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4Net/log4net.config", Watch = true)]
namespace Pulse.Kiosk
{
    using System;
    using Core.SignalRServer;
    using Core.Settings;
    using Core.Mapper.SignalR;
    using Common.ResolverFactories;
    using System.ServiceProcess;
    using Core.IoC.SignalR;
    using Common.Helpers;
    using Core.Services;
    using System.Threading;
    using Core.Dto.Entity;
    using System.Net.NetworkInformation;
    using System.Linq;
    using Microsoft.Practices.ServiceLocation;
    using System.Threading.Tasks;
    using CrossPlatformLibrary.Tracing;
    using Common.GeoLocation;
    using log4net;

    public class Program
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main(string[] args)
        {
            AutoMapperConfiguration.Config();
            NinjectConfiguration.Config();
#if DEBUG
            Process process = ResolverFactory.GetService<Process>();
            process.Start();
            Console.ReadKey();
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { new SelfHostService() };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }

}
