namespace Pulse.Core.Services
{
    public class KioskApiService : BaseApiService
    {
        public KioskApiService()
            : base(new ProxyService())
        {
            
        }
    }
}
