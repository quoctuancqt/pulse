namespace Pulse.Core.SignalR.Client
{
    using System;
    using Microsoft.AspNet.SignalR.Client;
    using Services;
    using Settings;
    using System.Threading.Tasks;
    using Enum;
    using Dto.Entity;
    using OwinServer.SignalRServer.Providers;
    using System.Collections.Generic;
    using Common.Helpers;

    public partial class PulseSignalRClient
    {
        public delegate void Error(Exception ex);
        public event Error OnError;
        public delegate void HandleSignalR(IHubProxy hubProxy);
        public event HandleSignalR OnSuccess;
        public event HandleSignalR OnReconnect;
        public event HandleSignalR OnConnectionSlow;
        public event HandleSignalR OnReconnecting;
        public event HandleSignalR OnBeforeStop;
        public string ConnectionId { get; private set; }
        private HubConnection _connection;
        private IHubProxy _hubProxy;
        private readonly IDeviceCtrlService _deviceCtrlService;
        private static ObjectState _objectState;

        public PulseSignalRClient(DeviceCtrlService deviceCtrlService)
        {
            _deviceCtrlService = deviceCtrlService;
        }

        public virtual void Run(ObjectState objectState)
        {
            _objectState = objectState;
            var querystringData = new Dictionary<string, string>();
            querystringData.Add("access_token", KioskConfigurationHepler.GetValueFromSecurity("Token"));
            querystringData.Add("refresh_token", KioskConfigurationHepler.GetValueFromSecurity("RefreshToken"));
            querystringData.Add("clientId", objectState.ClientId);
            querystringData.Add("groupName", objectState.GroupName);
            querystringData.Add("systemName", objectState.MachineName);
            querystringData.Add("machineId", objectState.MachineId);
            querystringData.Add("role", "");
            
            _connection = new HubConnection(_objectState.SignalrUrl, querystringData);
            _hubProxy = _connection.CreateHubProxy(SettingsConfigurationKiosks.HUB_NAME);
            _connection.Reconnecting += () => OnReconnecting(_hubProxy);
            _connection.Reconnected += () => OnReconnect(_hubProxy);
            _connection.ConnectionSlow += () => OnConnectionSlow(_hubProxy);

            _connection.Start().ContinueWith(OnComplete);
        }

        public virtual void StopConnection()
        {
            if (OnBeforeStop != null) OnBeforeStop(_hubProxy);

            _connection.Stop();
        }

        private void OnComplete(Task task)
        {
            if (task.IsFaulted)
            {
                if (this.OnError != null)
                {
                    OnError(task.Exception.GetBaseException());
                }
            }
            else
            {
                InitClientMethod();
                ConnectionId = _connection.ConnectionId;
                OnSuccess(_hubProxy);
            }
        }

    }
}
