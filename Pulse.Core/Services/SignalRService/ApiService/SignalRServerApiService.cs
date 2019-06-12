namespace Pulse.Core.Services
{
    public class SignalRServerApiService : BaseApiService
    {
        public SignalRServerApiService(string bearer)
            : base(new SignalRServerProxy())
        {
            _proxyService.Bearer = bearer;
        }
    }
}
