namespace Pulse.Core.Services
{
    using System.Threading.Tasks;
    using Domain.Mongo;
    using System;
    using Dto.Mongo;
    using Common.Helpers;
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity;
    using MongoDB.Driver;
    using System.Data.Entity;
    using System.Linq;
    using Mongo.Repository;

    public class MongoService : BaseMongoService, IMongoService
    {
        public MongoService(IClientService clientService) : base(clientService)
        {
        }

        #region Notify

        public async Task<NotifyKioskDto> CreateNotifyKioskAsync(NotifyKioskDto dto)
        {
            _log.Debug("CreateNotifyKioskAsync: "+ MongoConnectionString);

            var clientUsers = await Context.ClientUserRepository.FindAllAsync();

            IList<NotifyKiosk> NotifyKiosks = new List<NotifyKiosk>();

            _log.Debug("clientUsers: " + clientUsers.Count());

            var createAt = DateTime.UtcNow;

            foreach (var clientUser in clientUsers)
            {
                var notifyKiosk = DtoToDocument<NotifyKioskDto, NotifyKiosk>(dto);

                notifyKiosk.CreateAt = createAt;
                
                notifyKiosk.Id = MongoHelper.GenerateId();

                notifyKiosk.UserId = clientUser.UserId;

                notifyKiosk.IsRead = false;

                NotifyKiosks.Add(notifyKiosk);
            }

            await Context.NotifyKioskRepository.InsertManyAsync(NotifyKiosks);

            return dto;
        }

        public async Task<IEnumerable<NotifyKioskDto>> GetNotifyAsync()
        {
            var result = await Context.NotifyKioskRepository.SearchForPagingAsync(x => x.UserId.Equals(_clientService.CurrentUserId), x => x.CreateAt, 0, 50);

            return result.Select(x => DocumentToDto<NotifyKiosk, NotifyKioskDto>(x));
        }

        public async Task ReadNotifyAsync(string userId)
        {
            var notifyKiosks = await Context.NotifyKioskRepository.SearchAsync(n => n.UserId.Equals(userId)).ToListAsync();

            foreach (var notify in notifyKiosks)
            {
                notify.IsRead = true;
            }

            await Context.NotifyKioskRepository.UpdateAllAsync(notifyKiosks);
        }

        public async Task ReadNotifyAsync()
        {
            await ReadNotifyAsync(_clientService.CurrentUserId);
        }

        #endregion

        #region ClientUser

        public async Task<ClientUserDto> CreateClientUserAsync(ClientUserDto dto)
        {
            var document = DtoToDocument<ClientUserDto, ClientUser>(dto);

            document.Id = MongoHelper.GenerateId();

            await Context.ClientUserRepository.InsertOneAsync(document);

            return DocumentToDto<ClientUser, ClientUserDto>(document);
        }

        #endregion

        #region UserActivities

        public async Task<UserActivitiesDto> CreateUserActivitiesAsync(UserActivitiesDto dto)
        {
            var document = DtoToDocument<UserActivitiesDto, UserActivities>(dto);

            document.Id = MongoHelper.GenerateId();

            document.CreateAt = DateTime.UtcNow;

            if (PrincipalUser != null)
            {
                document.UserId = PrincipalUser.Identity.GetUserId();
            }

            await Context.UserActivitiesRepository.InsertOneAsync(document);

            return DocumentToDto<UserActivities, UserActivitiesDto>(document);
        }

        public async Task<IEnumerable<UserActivitiesDto>> GetUserActivitiesAsync()
        {
            var result = await Context.UserActivitiesRepository.SearchForPagingAsync(x => x.UserId.Equals(_clientService.CurrentUserId) && (x.CreateAt >= DateTime.Today.Date && x.CreateAt <= DateTime.Today.AddDays(1).Date), x => x.CreateAt, 0, 50);

            return result.Select(x => DocumentToDto<UserActivities, UserActivitiesDto>(x));
        }

        #endregion

        #region SystemEvent

        public async Task<SystemEventDto> CreateSystemEventAsync(SystemEventDto dto)
        {
            var document = DtoToDocument<SystemEventDto, SystemEvent>(dto);

            document.Id = MongoHelper.GenerateId();

            document.CreateAt = DateTime.UtcNow;

            await Context.SystemEventRepository.InsertOneAsync(document);

            return DocumentToDto<SystemEvent, SystemEventDto>(document);
        }

        public async Task<IEnumerable<SystemEventDto>> GetSystemEventAsync()
        {
            var result = await Context.SystemEventRepository.SearchForPagingAsync(x => x.CreateAt >= DateTime.Today.Date && x.CreateAt <= DateTime.Today.AddDays(1).Date, x => x.CreateAt, 0, 50);

            return result.Select(x => DocumentToDto<SystemEvent, SystemEventDto>(x));
        }

        #endregion

        public async Task<MongoState> CreateDatabaseAsync()
        {
            return await Context.CreateDatabaseAsync(Context.GetMongoContext());
        }

    }
}
