namespace Pulse.WebApi.Api
{
    using Core.Dto.Entity;
    using Core.Services;
    using Domain.Entity;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix("api/clientscountries")]
    public class ClientsCountriesController : BaseApiController<ClientsCountries, ClientsCountriesDto, IClientsCountriesService>
    {
        public ClientsCountriesController(IClientsCountriesService service) : base(service)
        {
            
        }

        [Route("addcountry"), HttpPost]
        public async Task<IHttpActionResult> AddCountry([FromBody]IDictionary<string, string> @param)
        {
            await _service.AddCountryAsync(int.Parse(@param["countryId"]), _service.ClientId);

            return Success();
        }

        [Route("search"), HttpGet]
        public async Task<IHttpActionResult> Search(string countryName = "", int skip = 0, int take = 10)
        {
            var result = await _service.SearchAsync(c=> (string.IsNullOrEmpty(countryName) ? true : c.Country.Name.Equals(countryName)) , skip, take);

            return Ok(result);
        }
    }
}
