namespace Pulse.Core.Services
{
    using Domain.Enum;
    using Dto.Entity;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;

    public interface IServiceBase<TEntity, TDto>
    {
        Task<PageResultDto<TDto>> SearchAsync(Expression<Func<TEntity, bool>> pression = null, int skip = 0, int take = 10);

        Task<PageResultDto<TDto>> SearchAsync<TOrderBy>(Expression<Func<TEntity, bool>> pression = null,
            Expression<Func<TEntity, TOrderBy>> orderBy = null, OrderType orderType = OrderType.Descending,
            int skip = 0, int take = 10);

        Task<IEnumerable<TDto>> FindAllAsync<TOrderBy>(Expression<Func<TEntity, bool>> pression = null,
            Expression<Func<TEntity, TOrderBy>> orderBy = null, OrderType orderType = OrderType.Descending);

       Task<IEnumerable<TDto>> FindAllAsync(Expression<Func<TEntity, bool>> pression = null);

        Task<TDto> FindByIdAsync(int id);

        Task<TDto> CreateAsync(TDto model);

        Task<TDto> UpdateAsync(TDto model);

        Task DeleteAsync(int id);

        IPrincipal PrincipalUser { get; set; }

        ClaimsIdentity claimsIdentity { get; } 

        string CurrentUserId { get; }

        string ClientId { get; }

        OAuthGrant AllowedGrant { get; }
    }
}
