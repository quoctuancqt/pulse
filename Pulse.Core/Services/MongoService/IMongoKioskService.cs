namespace Pulse.Core.Services
{
    using Domain.Mongo;
    using Dto.Entity;
    using Dto.Mongo;
    using MongoDB.Driver;
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IMongoKioskService: IBaseMongoService
    {
        Task<MongoKioskDto> CreateAsync(MongoKioskDto dto);

        Task<PageResultDto<MongoKioskDto>> SearchAsync(string machineId, Expression<Func<MongoKiosk, bool>> pression = null, int skip = 0, int take = 10);
    }
}
