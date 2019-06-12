namespace Pulse.WebApi.Api
{
    using Core.Dto.Entity;
    using Core.Services;
    using Domain;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix("api/countries")]
    public class CountriesController : BaseApiController<Country, CountryDto, ICountryService>
    {
        public CountriesController(ICountryService service) : base(service)
        {
        }

        [AllowAnonymous, HttpGet]
        public override Task<IHttpActionResult> GetAll()
        {
            return base.GetAll();
        }

        [Route("byname"), HttpGet]
        public async Task<IHttpActionResult> GetByName(string name)
        {
            var result = await _service.FindByNameAsync(name);

            return Ok(result);
        }

        [Route("search"), HttpGet]
        public async Task<IHttpActionResult> SearchCountry(string name = "", int skip = 0, int take = 10)
        {
            var result = await _service.SearchAsync(i => (string.IsNullOrEmpty(name) ? !i.Name.Equals(name) : i.Name.Contains(name)), skip, take);

            return Ok(result);
        }
    }
}