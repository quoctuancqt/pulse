namespace Pulse.Core.SignalR.Server
{
    using Common.Helpers;
    using Domain.Mongo.Enum;
    using HandlerEvent;
    using HandlerEvent.Args;
    using Microsoft.AspNet.SignalR;
    using System;
    public partial class PulseSignalRServer : Hub
    {
        public void StartConnection()
        {
            if(Context.QueryString["role"] == "Administrator") ProcessServer(Context);
        }

        public void SendDeviceControl(string machineId, string controlType)
        {
            var userData = FindUserDataByMachineId(machineId);
            var events = new SignalREventHandlers(Context.QueryString["access_token"]);
            if (userData != null)
            {
                ProcessDevicesControlMessage(userData.ConnectionId, controlType);
                AsyncHelper.RunSync(() => events.TriggerSystemEventAsync(new SystemEventArgs
                {
                    Action = controlType == "shutdown" ? ActionType.ShutDown : ActionType.Restart,
                    Description = controlType == "shutdown" ? Enum.GetName(typeof(ActionType), 1) : Enum.GetName(typeof(ActionType), 0),
                    MachineId = machineId,
                    MachineName = userData.SystemName,
                    Status = SystemEventStatus.Critical
                }));
            }
        }

        public void SendUpdateUserData(string connectionId, string systemName)
        {
            if (_users.ContainsKey(connectionId))
            {
                var newValue = _users[connectionId];

                newValue.SystemName = systemName;

                var comparisonValue = _users[connectionId];

                _users.TryUpdate(connectionId, newValue, comparisonValue);
            }
        }

    }
}
