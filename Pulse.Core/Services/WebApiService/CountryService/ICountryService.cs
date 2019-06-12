namespace Pulse.Core.Services
{
    using Domain;
    using Dto.Entity;
    using System.Threading.Tasks;

    public interface ICountryService: IServiceBase<Country, CountryDto>
    {
        Task<CountryDto> FindByNameAsync(string countryName);
    }
}
