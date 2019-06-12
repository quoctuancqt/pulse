namespace Pulse.Core.SignalR.Client
{
    using Microsoft.AspNet.SignalR.Client;
    public partial class PulseSignalRClient
    {
        private void InitClientMethod()
        {
            _hubProxy.On("ProcessDevicesControlMessage", _deviceCtrlService.CallBack);

            _hubProxy.On("OnDisconnected", OnDisConnected);
        }

        private void ProcessDevicesControlMessage(object controlType)
        {
             StopConnection();
            _deviceCtrlService.CallBack(controlType);
        }
        private void OnDisConnected(object ConnectionId)
        {
            //_hubStatus = HubStatus.DisConnected;
        }

    }
}
