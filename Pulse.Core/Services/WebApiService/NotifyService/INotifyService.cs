namespace Pulse.Core.Services
{
    using Dto.Entity;
    using Dto.Mongo;
    using System.Threading.Tasks;

    public interface INotifyService
    {
        Task<PageResultDto<NotifyKioskDto>> FindByUserIdAsync(string userId);

        Task UpdateNotifyByUserIdAync(string userId);
    }
}
