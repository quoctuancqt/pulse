namespace Pulse.Core.SignalR.Server
{
    using OwinServer.SignalRServer.Providers;
    using Services;
    using System.Threading.Tasks;
    using System;

    public class Process : SignalRServerProvider
    {
        public Process(CollectDeviceService collectDeviceService) : base(collectDeviceService)
        {
            
        }

    }
}
