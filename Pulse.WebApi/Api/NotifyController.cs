namespace Pulse.WebApi.Api
{
    using Core.Dto.Entity;
    using Core.Dto.Mongo;
    using Core.Services;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix("api/notify")]
    [Authorize]
    public class NotifyController : ApiController
    {
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger
(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IMongoService _mongoService;

        public NotifyController(IMongoService mongoService)
        {
            _mongoService = mongoService;

            _mongoService.PrincipalUser = User;
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var result = await _mongoService.GetNotifyAsync();

            return Ok(new PageResultDto<NotifyKioskDto>(result.Count(), result));
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Post(NotifyKioskDto dto)
        {
            try
            {
                return Ok(await _mongoService.CreateNotifyKioskAsync(dto));
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }

            return Ok();
        }

        [HttpPut, Route("")]
        public async Task<IHttpActionResult> Put()
        {
            await _mongoService.ReadNotifyAsync();

            return Content(System.Net.HttpStatusCode.OK, string.Empty);
        }
    }
}
