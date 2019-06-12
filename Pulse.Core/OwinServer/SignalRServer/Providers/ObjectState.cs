namespace Pulse.Core.OwinServer.SignalRServer.Providers
{
    using Dto.Entity;

    public class ObjectState
    {
        public ObjectState(KioskDto kioskDto, ClientDto clientDto)
        {
            KioskDto = kioskDto;
            ClientDto = clientDto;
        }

        private KioskDto KioskDto { get; set; }
        private ClientDto ClientDto { get; set; }

        public string MachineId
        {
            get
            {
                return KioskDto.MachineId;
            }
        }

        public string MachineName
        {
            get
            {
                return KioskDto.Name;
            }
        }

        public string GroupName
        {
            get
            {
                return string.Format("{0}{1}", ClientDto.Name.Replace(" ","").ToLower(), ClientDto.ClientId);
            }
        }

        public string SignalrUrl
        {
            get
            {
                return ClientDto.SignalrUrl;
            }
        }

        public string ClientId
        {
            get
            {
                return ClientDto.ClientId;
            }
        }
    }
}
