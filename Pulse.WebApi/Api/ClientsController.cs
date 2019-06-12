namespace Pulse.WebApi.Api
{
    using Core.Dto.Entity;
    using Core.Dto.Mongo;
    using Core.Security.Identity;
    using Core.Security.Identity.IdentityModels;
    using Core.Services;
    using Domain.Entity;
    using Domain.Mongo.Enum;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix("api/clients")]
    public class ClientsController : BaseApiController<Client, ClientDto, IClientService>
    {
        private readonly IMongoService _mongoService;

        private readonly PulseUserManager _userManager;

        public ClientsController(IClientService service, IMongoService mongoService, PulseUserManager userManager) : base(service)
        {
            _mongoService = mongoService;
            _userManager = userManager;
        }

        public override async Task<IHttpActionResult> Get(int id)
        {
            return Forbidden();
        }

        public override async Task<IHttpActionResult> GetAll()
        {
            return Forbidden();
        }

        [HttpGet, Route("byclientid/{clientid}")]
        public async Task<IHttpActionResult> GetByClientId(string clientId)
        { 
            var result = await _service.FindByClientIdAsync(clientId);

            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpGet, AllowAnonymous, Route("generateclientheader/{userName}")]
        public async Task<IHttpActionResult> GenerateClientHeader(string userName)
        {
            return Ok(await _service.GenerateHeaderAsync(userName));
        }

        public async override Task<IHttpActionResult> Post([FromBody] ClientDto model)
        {
            if (!CheckUserRole()) return Forbidden();

            var result = await _service.CreateAsync(model);

            if (result.ClientId == null) return BadRequest();

            await _mongoService.CreateDatabaseAsync();

            return Ok(result);
        }

        public async override Task<IHttpActionResult> Put([FromBody] ClientDto model)
        {
            if (!CheckUserRole()) return Forbidden();

            var result = await _service.UpdateAsync(model);

            return Ok(result);
        }

        public async override Task<IHttpActionResult> Delete([FromBody] ClientDto model)
        {
            if (!CheckUserRole()) return Forbidden();

           await _service.DeleteAsync(model.Id);

            return Success();
        }

        [HttpGet, Route("search")]
        public async Task<IHttpActionResult> Search(string name = "", int skip = 0, int take = 10)
        {
            if (!CheckUserRole()) return Forbidden();

            var result = await _service.SearchAsync(u => (string.IsNullOrEmpty(name) ? true : u.Name.ToLower().Contains(name.ToLower())), skip, take);

            return Ok(result);
        }

        private bool CheckUserRole()
        {
            if (!User.IsInRole(PulseIdentityRole.Administrator)) return false;

            return true;
        }

    }
}
