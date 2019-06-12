namespace Pulse.Kiosk
{
    using Core.OwinServer.SignalRServer.Providers;
    using Core.Services;
    using Core.SignalR.Client;
    using Core.HandlerEvent;

    public class Process : SignalRClientProvider
    {
        public Process(ICollectDeviceService collectDeviceService, PulseSignalRClient signalR, IApiService apiService) 
            : base(collectDeviceService, signalR, apiService)
        {
        }
    }
}
