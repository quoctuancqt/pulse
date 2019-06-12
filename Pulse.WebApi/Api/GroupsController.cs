namespace Pulse.WebApi.Api
{
    using Core.Dto.Entity;
    using Core.Services;
    using Domain;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System;
    [RoutePrefix("api/groups")]
    public class GroupsController : BaseApiController<Group, GroupDto, IGroupService>
    {
        public GroupsController(IGroupService service) : base(service)
        {
        }

        [Route("byname"), HttpGet]
        public async Task<IHttpActionResult> GetByName(string name)
        {
            var result = await _service.FindNameAsync(name);

            return Ok(result);
        }

        [Route("search"), HttpGet]
        public async Task<IHttpActionResult> SearchGroup(string name = "", int skip = 0, int take = 10)
        {
            var result = await _service.SearchAsync(i => (string.IsNullOrEmpty(name) ? !i.Name.Equals(name) : i.Name.Contains(name)), skip, take);

            return Ok(result);
        }
    }
}
