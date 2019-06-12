namespace Pulse.Core.Services
{
    using System.Threading.Tasks;

    public interface ICollectDeviceService
    {
        Task<string> CollectDeviceInfoAsync();
    }
}
