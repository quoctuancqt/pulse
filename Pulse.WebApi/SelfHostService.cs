namespace Pulse.WebApi
{
    using Core.Settings;
    using log4net;
    using System;

    public class SelfHostService: System.ServiceProcess.ServiceBase
    {
        private IDisposable _server = null;
        private readonly ILog _log = LogManager.GetLogger(typeof(SelfHostService));
        protected override void OnStart(string[] args)
        {
            try
            {
                _log.Debug("Start");
                _server = Microsoft.Owin.Hosting.WebApp.Start<Core.WeApiServer.WebApiConfiguration>(SettingsConfigurationWebApi.DOMAIN_SERVER);
                _log.Debug("Started");
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
            
        }

        protected override void OnStop()
        {
            if (_server != null)
            {
                _server.Dispose();
            }
            
            base.OnStop();
        }
    }
}
