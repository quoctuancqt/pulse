namespace Pulse.Core.Services
{
    using System;
    using System.ComponentModel;
    public interface IDeviceCtrlService: IDisposable
    {
        void CallBack(object arg);
    }
}
