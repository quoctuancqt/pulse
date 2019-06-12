namespace Pulse.Core.SignalR.Server
{
    using HandlerEvent;
    using Microsoft.AspNet.SignalR;

    public partial class PulseSignalRServer : Hub
    {
        public void SendPerfStatusUpdateToGroups(string groupName, string machineId, string json)
        {
            if (FindUserDataByGroupName(groupName) != null)
            {
                _log.Debug(string.Format("GroupName: {0} --- MachineId: {1} ---- Json: {2} :", groupName, machineId, json));
                Clients.Group(groupName).ProcessPerfStatusMessageToKiosk(machineId, json);

                TriggerMongoKiosk(machineId, json);
            }
            else
            {
                //TODO alert for client know groupName incorrect
            }
        }

        public void SendPerfStatusUpdateToClient(string connectionId, string json)
        {
            Clients.Group(GetGroupPulseServer(connectionId)).ProcessPerfStatusMessageToPulseServer(json);
        }

        public void ProcessDevicesControlMessage(string connectionId, string controlType)
        {
            Clients.Client(connectionId).ProcessDevicesControlMessage(controlType);
        }

        public void SendNotifyToPulseServer(string connectionId, string json)
        {
            Clients.Group(GetGroupPulseServer(connectionId)).ReceiveNotifyToPulseServer(json);
        }

    }
}