namespace Pulse.Core.OwinServer.SignalRServer.Providers
{
    using Microsoft.AspNet.SignalR.Hubs;
    using Services;
    using Settings;
    using System.Threading;
    using System;
    using System.Threading.Tasks;
    using log4net;
    using Common.Helpers;

    public abstract class SignalRServerProvider
    {
        protected readonly ICollectDeviceService _collectDeviceService;
        public delegate void HandleTimer(string connectionId, string json);
        public event HandleTimer OnHandleTimer;
        private Timer _timer;
        public HubCallerContext _context;
        private readonly ILog _log = LogManager.GetLogger(typeof(SignalRClientProvider));

        public SignalRServerProvider(CollectDeviceService collectDeviceService)
        {
            _log.Debug("SignalRServerProvider");
            _collectDeviceService = collectDeviceService;
        }

        public virtual void Start()
        {
            _log.Debug("Begin Start");
            while (true)
            {
                AsyncHelper.RunSync(() => DoWork(null));
                Thread.Sleep(500);
            }
        }

        public virtual async Task DoWork(object obj)
        {
            _log.Debug("Begin DoWork");
            var result = await _collectDeviceService.CollectDeviceInfoAsync();

            OnHandleTimer(_context.ConnectionId, result);
#if DEBUG
            Console.WriteLine(result);
#endif
            GC.Collect();
            _log.Debug("End DoWork");
        }

    }
}
