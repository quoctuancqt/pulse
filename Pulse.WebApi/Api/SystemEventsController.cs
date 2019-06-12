namespace Pulse.WebApi.Api
{
    using Core.Dto.Mongo;
    using Core.Services;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix("api/systemevents")]
    [Authorize]
    public class SystemEventsController : ApiController
    {
        private readonly IMongoService _mongoService;

        public SystemEventsController(IMongoService mongoService)
        {
            _mongoService = mongoService;

            _mongoService.PrincipalUser = User;
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await _mongoService.GetSystemEventAsync());
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Post(SystemEventDto dto)
        {
            return Ok(await _mongoService.CreateSystemEventAsync(dto));
        }
    }
}
