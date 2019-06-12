namespace Pulse.Core.Services
{
    using Common.Helpers;
    using Domain.Mongo;
    using Mongo.Factories;
    using Mongo.Repository;
    using System;
    using System.Security.Principal;
    using System.Threading.Tasks;

    public abstract class BaseMongoService: IBaseMongoService
    {
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger
(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Setting

        protected readonly IClientService _clientService;

        private IPrincipal _principalUser;

        public IPrincipal PrincipalUser {
            get
            {
                return _principalUser;
            }
            set
            {
                _clientService.PrincipalUser = value;
                _principalUser = value;
            }
        }

        public BaseMongoService(IClientService clientService)
        {
            _clientService = clientService;
        }

        protected TDocument DtoToDocument<Dto, TDocument>(Dto dto)
        {
            return AutoMapper.Mapper.Map<TDocument>(dto);
        }

        protected Dto DocumentToDto<TDocument, Dto>(TDocument doc)
        {
            return AutoMapper.Mapper.Map<Dto>(doc);
        }
       
        protected int GetToTalPage(int totalRecord, int take)
        {
            if (take > 0)
            {
                return (int)Math.Ceiling((double)((double)totalRecord / (double)take));
            }
            throw new Exception("Require 'Take' larger than zero.");
        }

        #endregion

        private string _mongoConnectionString;

        public string MongoConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_mongoConnectionString))
                {
                    var client = AsyncHelper.RunSync(() => _clientService.FindByClientIdAsync(_clientService.ClientId));

                    _mongoConnectionString = client.MongoConnectionString;
                }

                return _mongoConnectionString;
            }
        }
        protected IMongoContextFactory Context
        {
            get
            {
                return new MongoDbContextFactory(MongoConnectionString);
            }
        }
    }
}
