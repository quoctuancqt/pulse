namespace Pulse.Kiosk
{
    using System;
    using Common.ResolverFactories;
    using System.ServiceProcess;
    using log4net;
    using Common.Helpers;

    public class SelfHostService : ServiceBase
    {
        private Process _process = null;
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
                if (KioskConfigurationHepler.GetValueFromSecurity("LicenseKey") == null && KioskConfigurationHepler.GetValueFromSecurity("LicenseKey") == "")
                {
                    _log.Error("Can't start service with LicenseKey is empty please input LicenseKey before start service thanks");
                    throw new Exception("Can't start service with LicenseKey is empty please input LicenseKey before start service thanks");
                }

                _log.Debug("OnStart Client");
                _process = ResolverFactory.GetService<Process>();
                _process.Start();
                _log.Debug("OnStarted");
            }
            catch(Exception ex)
            {
                _log.Error(ex);
                OnStop();
            }
            
        }

        protected override void OnStop()
        {
            if (_process != null) _process.Stop();

            base.OnStop();
        }
    }
}
