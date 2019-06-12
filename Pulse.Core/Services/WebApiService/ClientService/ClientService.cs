namespace Pulse.Core.Services
{
    using System;
    using System.Threading.Tasks;
    using Domain.Entity;
    using Dto.Entity;
    using Security.Identity;
    using System.Data.Entity;
    using System.Text;
    using Common.Helpers;
    using Microsoft.AspNet.Identity;
    using Security.Identity.IdentityModels;
    using System.Linq.Expressions;
    using System.Linq;

    public class ClientService : ServiceBase<Client, ClientDto>, IClientService
    {
        private readonly PulseUserManager _userManager;

        private readonly IUserProfileService _userProfileService;

        private const string MONGO_PREFIX = "mongodb://";

        private const string REST_MONGO_NAME = "PULSE";

        public ClientService(IUnitOfWork unitOfWork, PulseUserManager userManager, IUserProfileService userProfileService) : base(unitOfWork)
        {
            _userManager = userManager;
            _repository = unitOfWork.Clients;
            _userProfileService = userProfileService;
        }

        public async Task<string> GenerateHeaderAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return Convert.ToBase64String(Encoding.ASCII.GetBytes(user.ClientId + ":" + user.SecretKey));
        }

        public async Task<ClientDto> FindByClientIdAsync(string clientId)
        {
            var result = await _repository.FindAll(c => c.ClientId.Equals(clientId)).FirstOrDefaultAsync();

            if (result == null) throw new Exception("Not Found with clientId: "+ clientId);

            return EntityToDto(result);
        }

        public override async Task<ClientDto> CreateAsync(ClientDto model)
        {
            PasswordHasher passwordHasher = new PasswordHasher();

            Client client = DtoToEntity(model);

            client.ClientId = UnitHelper.GenerateNewGuid();

            client.SecretKey = UnitHelper.GenerateNewGuid();

            client.Secret = passwordHasher.HashPassword(client.SecretKey);

            client.MongoName = (REST_MONGO_NAME + "_" + client.ClientId);

            client.MongoConnectionString = (MONGO_PREFIX + model.MongoConnectionString + "/" + client.MongoName);

            _repository.Add(client);

            await _unitOfWork.CommitAsync();

            var username = model.Email;

            var password = UnitHelper.RandomString("");

            var userProfile = await _userProfileService.CreateAsync(username, password, PulseIdentityRole.ClientAdmin);

            await _userProfileService.AddClientUserAsync(userProfile.UserId, client.ClientId, client.SecretKey);

            return EntityToDto(client);
        }

        public async Task<ClientDto> FindByNameAsync(string name)
        {
            var result = await _repository.FindAll(c => c.Name.Equals(name)).FirstOrDefaultAsync();

            if (result == null) return null;

            return EntityToDto(result);
        }

        public override async Task<PageResultDto<ClientDto>> SearchAsync(Expression<Func<Client, bool>> pression = null, int skip = 0, int take = 10)
        {
            var query = _repository.FindAll(pression);

            var entities = await query.OrderByDescending(x => x.Id).Skip((skip * take)).Take(take).ToListAsync();

            return new PageResultDto<ClientDto>(await query.CountAsync(), GetToTalPage(await query.CountAsync(), take), EntityToDto(entities));
        }

        public override async Task<ClientDto> UpdateAsync(ClientDto model)
        {
            Client client = DtoToEntity(model);

            _repository.Update(client);

            await _unitOfWork.CommitAsync();

            return EntityToDto(client);
        }
    }
}
