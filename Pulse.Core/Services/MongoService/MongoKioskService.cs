namespace Pulse.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dto.Mongo;
    using Domain.Mongo;
    using MongoDB.Driver;
    using Common.Helpers;
    using Dto.Entity;
    using System.Linq.Expressions;
    using System.Data.Entity;

    public class MongoKioskService : BaseMongoService, IMongoKioskService
    {
        public MongoKioskService(IClientService clientService) : base(clientService)
        {
        }

        public async Task<MongoKioskDto> CreateAsync(MongoKioskDto dto)
        {
            var collection = await GetCollectionAsync(dto.MachineId);

            var document = DtoToDocument<MongoKioskDto, MongoKiosk>(dto);

            document.Id = MongoHelper.GenerateId();
            
            document.CreateAt = DateTime.UtcNow;

            collection.InsertOne(document);

            return DocumentToDto<MongoKiosk, MongoKioskDto>(document);
        }

        public async Task<PageResultDto<MongoKioskDto>> SearchAsync(string machineId, Expression<Func<MongoKiosk, bool>> pression = null, int skip = 0, int take = 10)
        {
            var collection = await GetCollectionAsync(machineId);

            var filter = Builders<MongoKiosk>.Filter.Empty;

            if (pression != null) filter = Builders<MongoKiosk>.Filter.Where(pression);

            var query = collection.Find(filter);
                

            var totalRecord = await query.CountAsync();

            int totalPage = GetToTalPage((int)totalRecord, take);

            var result = await query.SortBy(x => x.CreateAt)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();

            return new PageResultDto<MongoKioskDto>((int)totalRecord, totalPage, result.Select(x => DocumentToDto<MongoKiosk, MongoKioskDto>(x)));
        }

        private async Task<IMongoCollection<MongoKiosk>> GetCollectionAsync(string name)
        {
            var options = new CreateIndexOptions { ExpireAfter = TimeSpan.FromDays(7) };

            var keys = Builders<MongoKiosk>.IndexKeys.Descending(x => x.CreateAt);

            return await AsyncHelper.RunAsync(() => Context.GetMongoContext().GetCollection<MongoKiosk>(name, keys, options));
        }
    }
}
