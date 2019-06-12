namespace Pulse.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class DeviceCtrlService : IDeviceCtrlService
    {
        private delegate void DevicesCrtl();
        private IDictionary<string, DevicesCrtl> _events = new Dictionary<string, DevicesCrtl>();

        public DeviceCtrlService()
        {
            _events.Add("restart", new DevicesCrtl(Reset));
            _events.Add("shutdown", new DevicesCrtl(ShutDown));
        }

        public void CallBack(object arg)
        {
            string key = arg.ToString();
            if (_events.ContainsKey(key.ToLower()))
            {
                _events[key.ToLower()].Invoke();
            }
            else
            {
                throw new Exception("Not found with key: " + key);
            }
        }

        #region Private Method
        private void Reset()
        {
            System.Diagnostics.Process.Start("shutdown", "/f /r /t 5");
        }

        private void ShutDown()
        {
            System.Diagnostics.Process.Start("shutdown", "/f /s /t 5");
        }

        public void Dispose()
        {
            //TODO something
        }

        #endregion

    }
}
