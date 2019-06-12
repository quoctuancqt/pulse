namespace Pulse.WebApi.Api
{
    using Core.Dto.Mongo;
    using Core.Services;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix("api/useractivities")]
    [Authorize]
    public class UserActivitiesController : ApiController
    {
        private readonly IMongoService _mongoService;

        public UserActivitiesController(IMongoService mongoService)
        {
            _mongoService = mongoService;

            _mongoService.PrincipalUser = User;
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Post(UserActivitiesDto dto)
        {
            return Ok(await _mongoService.CreateUserActivitiesAsync(dto));
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await _mongoService.GetUserActivitiesAsync());
        }
    }
}
