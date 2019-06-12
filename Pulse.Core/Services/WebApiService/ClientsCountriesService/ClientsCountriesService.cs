namespace Pulse.Core.Services
{
    using Dto.Entity;
    using Domain.Entity;
    using System.Threading.Tasks;
    using System;
    using System.Linq.Expressions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data.Entity;

    public class ClientsCountriesService : ServiceBase<ClientsCountries, ClientsCountriesDto>, IClientsCountriesService
    {
        public ClientsCountriesService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _repository = _unitOfWork.ClientsCountries;
        }

        public async Task AddCountryAsync(int countryId, string clientId)
        {
            _unitOfWork.ClientsCountries.Add(new ClientsCountries
            {
                ClientId = clientId,
                CountryId = countryId
            });

            await _unitOfWork.CommitAsync();
        }

        public async Task<ClientsCountriesDto> FindByCountryIdAsync(int countryId)
        {
            var entity = await FindAll(c => c.CountryId == countryId).FirstOrDefaultAsync();

            if (entity == null) return null;

            return EntityToDto(entity);
        }

        public async override Task<PageResultDto<ClientsCountriesDto>> SearchAsync(Expression<Func<ClientsCountries, bool>> pression = null, int skip = 0, int take = 10)
        {
            var query = FindAll(pression);

            IEnumerable<ClientsCountries> entities = await query
                .OrderByDescending(x => x.Id).Skip((skip * take))
                .Take(take)
                .ToListAsync();

            return new PageResultDto<ClientsCountriesDto>(await query.CountAsync(), GetToTalPage(await query.CountAsync(), take), EntityToDto(entities));
        }
    }
}
