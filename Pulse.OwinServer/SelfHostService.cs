using System;
using Microsoft.Owin.Hosting;
using Pulse.Core.Settings;
using System.ServiceProcess;
using log4net;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Pulse.Core.SignalRServer.SignalRConfiguration))]
namespace Pulse.OwinServer
{
    public class SelfHostService : ServiceBase
    {
        private IDisposable _server = null;
        private readonly ILog _log = LogManager.GetLogger(typeof(SelfHostService));
        public SelfHostService()
        {
            //InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _log.Debug("Begin OnStart");
                _log.Debug("OnStart Server: " + SettingsConfigurationSignalR.DOMAIN_SERVER);
                _server = WebApp.Start(SettingsConfigurationSignalR.DOMAIN_SERVER);
            }
            catch(Exception ex)
            {
                _log.Error(ex);
                OnStop();
            }
            
        }

        protected override void OnStop()
        {
            if(_server != null) _server.Dispose();

            base.OnStop();
        }
    }
}
