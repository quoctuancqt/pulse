namespace Pulse.WebApi.Api
{
    using AutoMapper;
    using Core.Dto.Entity;
    using Core.Dto.Mongo;
    using Domain.Mongo;
    using Microsoft.AspNet.Identity;
    using Mongo.Factories;
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix("api/notification")]
    [Authorize]
    public class NotificationController : ApiController
    {
        private readonly IMongoContext _mongoContext;
        private readonly IMongoCollection<NotifyKiosk> _collection;
        private readonly string _userId;
        public NotificationController(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
            _collection = _mongoContext.GetCollection<NotifyKiosk>();
            _userId = User.Identity.GetUserId();
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            var result = await _collection.Find(Builders<NotifyKiosk>.Filter.Where(n => n.UserId.Equals(_userId))).ToListAsync();

            var pageResult = new PageResultDto<NotifyKioskDto>(result.Count(), result.Select(x => Mapper.Map<NotifyKioskDto>(x)).ToArray());

            return Ok(pageResult);
        }

        [HttpPut]
        public async Task<IHttpActionResult> Put(IDictionary<string, string> dic)
        {
            var result = await _collection.Find(Builders<NotifyKiosk>.Filter.Where(n => n.UserId.Equals(_userId) && n.IsRead == false )).ToListAsync();

            return Ok();
        }


    }
}
