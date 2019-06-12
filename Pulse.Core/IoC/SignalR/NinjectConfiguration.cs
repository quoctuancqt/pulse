namespace Pulse.Core.IoC.SignalR
{
    using Ninject;
    using Services;
    using System.Reflection;
    using Common.ResolverFactories;
    using Core.SignalR.Client;
    using HandlerEvent;

    public static class NinjectConfiguration
    {
        public static void Config()
        {
            IKernel kernel = new StandardKernel();
            kernel.Bind<PulseSignalRClient>().To<PulseSignalRClient>();
            kernel.Bind<IDeviceCtrlService>().To<DeviceCtrlService>();
            kernel.Bind<ICollectDeviceService>().To<CollectDeviceService>();
            kernel.Bind<IWMIService>().To<MemoryService>();
            kernel.Bind<IWMIService>().To<NetWorkService>();
            kernel.Bind<IWMIService>().To<DriveService>();
            kernel.Bind<IWMIService>().To<PhysicalDiskService>();
            kernel.Bind<IWMIService>().To<ProcessorService>();
            kernel.Bind<IWMIService>().To<TemperatureService>();
            kernel.Bind<IProxyService>().To<ProxyService>();
            kernel.Bind<IApiService>().To<KioskApiService>();
            kernel.Load(Assembly.GetExecutingAssembly());
            ResolverFactory.SetKernel(kernel);
        }
    }
}
