namespace Pulse.Core.Services
{
    using Dto.Entity;
    using Domain.Entity;
    using System.Threading.Tasks;

    public interface IClientsCountriesService : IServiceBase<ClientsCountries, ClientsCountriesDto>
    {
        Task AddCountryAsync(int countryId, string clientId);

        Task<ClientsCountriesDto> FindByCountryIdAsync(int countryId);
    }
}
