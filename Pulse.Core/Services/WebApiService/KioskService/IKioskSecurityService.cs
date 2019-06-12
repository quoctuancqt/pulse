namespace Pulse.Core.Services
{
    using Domain;
    using Dto.Entity;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IKioskSecurityService: IServiceBase<KioskSecurity, KioskSecurityDto>
    {
        Task<IEnumerable<string>> GenerateLicenseKeyAsync(string clientId, int number);
        Task<KioskSecurityDto> CheckLicenseKeyAsync(string key);
        Task<KioskSecurityDto> UpdateByLicenseKeyAsync(string key, KioskSecurityDto kioskSecurityDto);
    }
}
