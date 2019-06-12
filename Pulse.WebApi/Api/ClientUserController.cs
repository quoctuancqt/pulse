namespace Pulse.WebApi.Api
{
    using Core.Dto.Mongo;
    using Core.Services;
    using Domain.Mongo.Enum;
    using Microsoft.AspNet.Identity;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix("api/clientuser")]
    [Authorize]
    public class ClientUserController : ApiController
    {
        private readonly IMongoService _mongoService;

        public ClientUserController(IMongoService mongoService)
        {
            _mongoService = mongoService;

            _mongoService.PrincipalUser = User;
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Post()
        {
            var result = await _mongoService.CreateClientUserAsync(new ClientUserDto
            {
                UserId = User.Identity.GetUserId(),
                GroupName = GroupType.ALL
            });

            return Ok(result);
        }
    }
}
