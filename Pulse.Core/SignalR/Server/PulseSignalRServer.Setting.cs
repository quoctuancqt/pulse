namespace Pulse.Core.SignalR.Server
{
    using Microsoft.AspNet.SignalR;
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNet.SignalR.Hubs;
    using System.Threading;
    using System.Collections.Concurrent;
    using Common.ResolverFactories;
    using Dto.SignalR;
    using log4net;
    using Domain.Enum;
    using Common.Helpers;
    using HandlerEvent;
    using Settings;
    using HandlerEvent.Args;
    using Dto.Mongo;
    using Domain.Mongo.Enum;

    [HubName("PulseHub")]
    public partial class PulseSignalRServer : Hub
    {
        private static bool IS_START_COLLECT_DATA_SERVER = false;
        private static ConcurrentDictionary<string, UserDataDto> _users = new ConcurrentDictionary<string, UserDataDto>();
        private static int _usersCount = 0;
        private readonly ILog _log = LogManager.GetLogger(typeof(PulseSignalRServer));

        public string GetName { get; private set; }

        public PulseSignalRServer()
        {
           
        }

        #region Publish method
        public override Task OnReconnected()
        {
            _log.Debug("OnReconnected: " + Context.ConnectionId);
            return base.OnReconnected();
        }

        public override Task OnConnected()
        {
            _log.Debug("OnConnected: " + Context.ConnectionId);  

            Interlocked.Increment(ref _usersCount);

            var user = new UserDataDto()
            {
                ConnectionId = Context.ConnectionId,
                ClientId = Context.QueryString["clientId"],
                GroupName = Context.QueryString["groupName"],
                Token = Context.QueryString["access_token"],
                RefreshId = Context.QueryString["refresh_token"],
                MachineId = Context.QueryString["machineId"],
                SystemName = Context.QueryString["systemName"],
                ConnectedAt = DateTime.Now
            };

            _log.Debug("clientId: " + Context.QueryString["clientId"]);

            _log.Debug("groupName: " + Context.QueryString["groupName"]);

            _log.Debug("systemName: " + Context.QueryString["systemName"]);

            _log.Debug("machineId: " + Context.QueryString["machineId"]);

            _users[Context.ConnectionId] = user;

            _log.Debug("AddGroup: AddGroup");

            AddGroup(Context.ConnectionId, Context.QueryString["systemName"], Context.QueryString["groupName"], Context.QueryString["machineId"]);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stop)
        {
            UserDataDto user;
            var events = new SignalREventHandlers(Context.QueryString["access_token"]);
            if (_users.Keys.Contains(Context.ConnectionId))
            {
                Groups.Remove(Context.ConnectionId, _users[Context.ConnectionId].GroupName);

                if (_users.TryRemove(Context.ConnectionId, out user))
                {
                    if (!user.GroupName.Contains("PulseServer#"))
                    {
                        ProcessNotification(Context, ProcessType.Disconnected, KioskStatus.offline, user.MachineId, user.SystemName, user.GroupName);
                        Clients.Group(user.GroupName).OnDisconnected(user.MachineId);
                        AsyncHelper.RunSync(() => events.TriggerSystemEventAsync(new SystemEventArgs {
                            Action = ActionType.Disconnected,
                            Description = "Disconnected",
                            MachineId = user.MachineId,
                            MachineName = user.SystemName,
                            Status = SystemEventStatus.Hight,
                        }));
                    }
                }
            }

            return base.OnDisconnected(false);
        }

        #endregion

        #region Private method
        private UserDataDto FindUserDataByGroupName(string groupName)
        {
            UserDataDto userData = null;
            foreach (var item in _users.Values)
            {
                if (item.GroupName == groupName)
                {
                    userData = item;
                    break;
                }
            }
            return userData;
        }

        private UserDataDto FindUserDataByMachineId(string machineId)
        {
            UserDataDto userData = null;
            foreach (var item in _users.Values)
            {
                if (item.MachineId.ToLower() == machineId)
                {
                    userData = item;
                    break;
                }
            }
            return userData;
        }

        private void ProcessServer(HubCallerContext context)
        {
            if (ProcessParameter(context) && !IS_START_COLLECT_DATA_SERVER)
            {
                Process process = ResolverFactory.GetService<Process>();
                process._context = context;
                process.OnHandleTimer += SendPerfStatusUpdateToClient;
                process.Start();
                IS_START_COLLECT_DATA_SERVER = true;
            }

        }

        private void ProcessNotification(HubCallerContext context, ProcessType processType, KioskStatus kioskStatus, string machineId, string systemName, string groupName)
        {
            if (!ProcessParameter(context) && !string.IsNullOrEmpty(machineId))
            {
                var kiosk = AddNotification(machineId, processType, kioskStatus, systemName, groupName);
   
                var output = $"{{ \"machineId\" : \"{machineId}\" , \"name\" : \"{systemName}\", \"content\" : \"{kiosk.Content}\", "
                             + $" \"status\" : \"{kiosk.Status}\" , \"countDate\" : \"{ kiosk.CreateAt.ToLocalTime().CountDay() }\", \"isRead\" : \"false\" }}";

                SendNotifyToPulseServer(Context.ConnectionId, output);
            }
        }

        private bool ProcessParameter(HubCallerContext context)
        {
            return (string.IsNullOrEmpty(context.QueryString["server"]) ? false : Convert.ToBoolean(context.QueryString["server"]));
        }

        private string GetGroupPulseServer(string connectionId)
        {
            return $"PulseServer#{_users[connectionId].ClientId}";
        }

        private void AddGroup(string connectionId, string systemName, string groupName, string machineId)
        {
            Groups.Add(Context.ConnectionId, groupName);
            _log.Debug("AddGroup1: AddGroup");
            ProcessNotification(Context, ProcessType.Connected, KioskStatus.online, Context.QueryString["machineId"], Context.QueryString["systemName"], Context.QueryString["groupName"]);
            _log.Debug("ProcessNotification");
        }
        #endregion

        #region Process Mongo
        private NotifyKioskDto AddNotification(string machineId, ProcessType processType, KioskStatus kioskStatus, string systemName, string groupName)
        {

            if (SettingsConfigurationSignalR.IS_PULSE_SERVER)
            {
                var events = new SignalREventHandlers(Context.QueryString["access_token"]);

                AsyncHelper.RunSync(() => events.TriggerUpdateStatusAsync(new KioskArgs
                {
                    MachineId = machineId,
                    kioskStatus = kioskStatus
                }));

                var document = AsyncHelper.RunSync(() => events.TriggerNofityAsync(new NotifyKioskArgs
                {
                    MachineId = machineId,
                    Name = systemName,
                    GroupName = groupName,
                    Status = Enum.GetName(typeof(KioskStatus), kioskStatus),
                    Content = string.Format("Kiosk {0} {1} to Pulse Server.", systemName, Enum.GetName(typeof(ProcessType), processType)),
                    CreateAt = DateTime.UtcNow
                }));

                return document;
            }

            return null;

        }
        private void TriggerMongoKiosk(string machineId, string json)
        {
            var events = new SignalREventHandlers(Context.QueryString["access_token"]);

            AsyncHelper.RunAsync(() => events.TriggerMongoKioskAsync(new MongoKioskArgs
            {
                MachineId = machineId,
                Json = json
            })); 

        }
        #endregion
    }
}