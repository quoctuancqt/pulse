namespace Pulse.WebApi.Api
{
    using Core.Dto.Entity;
    using Core.Services;
    using Domain;
    using System.Web.Http;
    using System.Threading.Tasks;
    using Domain.Enum;
    using System;
    using System.Data.Entity;

    [RoutePrefix("api/histories")]
    public class HistoriesController : BaseApiController<History, HistoryDto, IHistoryService>
    {
        public HistoriesController(IHistoryService service) : base(service)
        {
        }

        public override async Task<IHttpActionResult> Get(int id)
        {
            return Forbidden();
        }

        public override async Task<IHttpActionResult> GetAll()
        {
            return Forbidden();
        }

        [Route("search"), HttpGet]
        public async Task<IHttpActionResult> Search(HistoryType historyType = HistoryType.PulseServer, string machineId="", DateTime? date = null, int skip = 0, int take = 10)
        {
            var searchResult = await _service.SearchAsync(h => h.HistoryType == historyType &&
            (!date.HasValue  ? DbFunctions.TruncateTime(h.CreatedDate) == DateTime.Today : DbFunctions.TruncateTime(h.CreatedDate) == DbFunctions.TruncateTime(date.Value)) && (string.IsNullOrEmpty(machineId) ? true : h.MachineId.ToLower().Equals(machineId.ToLower())), skip, take);
            return Ok(searchResult);
        }
    }
}
