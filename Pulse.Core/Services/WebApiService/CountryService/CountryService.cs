namespace Pulse.Core.Services
{
    using Dto.Entity;
    using Domain;
    using System.Data.Entity;
    using System.Threading.Tasks;
    
    public class CountryService : ServiceBase<Country, CountryDto>, ICountryService
    {

        private IClientsCountriesService _clientsCountriesService;

        public CountryService(IUnitOfWork unitOfWork, IClientsCountriesService clientsCountriesService) : base(unitOfWork)
        {
            _repository = unitOfWork.Countries;
            _clientsCountriesService = clientsCountriesService;
        }

        public async Task<CountryDto> FindByNameAsync(string countryName)
        {
            var result = await _repository.FindAll(c => c.Name.Equals(countryName)).FirstOrDefaultAsync();

            var clientCountry = await _clientsCountriesService.FindByCountryIdAsync(result.Id);

            if (clientCountry == null)
            {
                await _clientsCountriesService.AddCountryAsync(result.Id, ClientId);
            }
            
            return EntityToDto(result);
        }
    }
}
