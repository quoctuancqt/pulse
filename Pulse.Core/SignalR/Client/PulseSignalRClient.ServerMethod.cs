namespace Pulse.Core.SignalR.Client
{
    using Settings;
    public partial class PulseSignalRClient
    {
        public void SendPerfStatusUpdateToGroups(string json)
        {
            _hubProxy.Invoke("SendPerfStatusUpdateToGroups",
                      _objectState.GroupName,
                      _objectState.MachineId,
                     json).ContinueWith(task => { });
        }

        public void SendNotifyToPulseServer(string json)
        {   
            _hubProxy.Invoke("SendNotifyToPulseServer", json).ContinueWith(task => { });
        }
    }
}
