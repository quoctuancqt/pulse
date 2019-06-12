namespace Pulse.Core.SignalRServer
{
    using Owin;
    using Microsoft.AspNet.SignalR;
    using Microsoft.Owin.Cors;
    using SignalR.Pipeline;
    using log4net;

    public class SignalRConfiguration
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(SignalRConfiguration));
        public void Configuration(IAppBuilder app)
        {
            //GlobalHost.HubPipeline.AddModule(new PulseHubPipelineModule());
            _log.Debug("Configuration");
            app.UseCors(CorsOptions.AllowAll);
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);

                var hubConfiguration = new HubConfiguration
                {
                    //EnableJSONP = true,
                };

                map.RunSignalR(hubConfiguration);
            });
            _log.Debug("Done Configuration");

        }
    }
}
