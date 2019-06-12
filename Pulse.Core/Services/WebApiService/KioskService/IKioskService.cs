namespace Pulse.Core.Services
{
    using Domain;
    using Domain.Enum;
    using Dto.Entity;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IKioskService: IServiceBase<Kiosk, KioskDto>
    {
        Task<KioskDto> FindByMachineIdAsync(string machineId);
        Task<IEnumerable<KioskDto>> FindByCountryIdAsync(int countryId);
        Task<KioskDto> UpdateStatusByMachineIdAsync(string machineId, KioskStatus kioskStatus);
        Task UpdateConnectionIdAsync(string machineId, string connectionId);
    }
}
