namespace Pulse.Core.Services
{
    using System;
    using System.Threading.Tasks;
    using Dto.Entity;
    using Dto.Mongo;
    using Mongo.Factories;
    using MongoDB.Driver;
    using Domain.Mongo;
    using System.Linq;
    using AutoMapper;
    public class NotifyService : INotifyService
    {
        private readonly IMongoCollection<NotifyKiosk> _collection;

        public NotifyService(IMongoContext mongoContext)
        {
            _collection = mongoContext.GetCollection<NotifyKiosk>();
        }

        public async Task<PageResultDto<NotifyKioskDto>> FindByUserIdAsync(string userId)
        {
            var result = await  _collection.Find(Builders<NotifyKiosk>.Filter.Where(n => n.UserId.Equals(userId))).ToListAsync();

            return new PageResultDto<NotifyKioskDto>(
                    result.Count(),
                    result.Select(x => Mapper.Map<NotifyKioskDto>(x)).ToArray()
                );
        }

        public async Task UpdateNotifyByUserIdAync(string userId)
        {
            var builder = Builders<NotifyKiosk>.Filter;
            var filter = builder.Eq("UserId", userId) & builder.Eq("IsRead", false);
            var update = Builders<NotifyKiosk>.Update
                .Set("IsRead", true)
                .CurrentDate("lastModified");

            await _collection.UpdateManyAsync(filter, update);

        }

    }
}
