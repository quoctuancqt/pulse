namespace Pulse.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Dto.Entity;
    using Domain.Enum;
    using Domain;
    using AutoMapper;
    using Repository.Entity;
    using System.Data.Entity;
    using System.Linq;
    using System.Security.Principal;
    using Microsoft.AspNet.Identity;
    using System.Security.Claims;
    using Common.Helpers;

    public abstract class ServiceBase<TEntity, TDto> : IServiceBase<TEntity,TDto>
        where TEntity : class, IEntity
        where TDto : class, IDto
    {

        #region Setting ServiceBase
        
        protected readonly IUnitOfWork _unitOfWork;

        protected IRepository<TEntity> _repository;

        private IPrincipal _principalUser;

        public IPrincipal PrincipalUser
        {
           get
            {
                return _principalUser;
            }
            set
            {
                _unitOfWork.PrincipalUser = value;
                _principalUser = value;
            }
        }

        public ClaimsIdentity claimsIdentity
        {
            get
            {
                if (_principalUser != null) return (ClaimsIdentity)_principalUser.Identity;
                return null;
            }
        }

        public string ClientId
        {
            get
            {
                if (claimsIdentity != null) return claimsIdentity.FindFirstValue("ClientId");
                return null;
            }
        }
        
        public OAuthGrant AllowedGrant
        {
            get
            {
                if (claimsIdentity != null)
                {
                    string allowedGrant = (claimsIdentity.FindFirstValue("AllowedGrant") == null ? "Anonymous" : claimsIdentity.FindFirstValue("AllowedGrant"));

                    return (OAuthGrant)Enum.Parse(typeof(OAuthGrant), allowedGrant, true);
                }

                return OAuthGrant.Anonymous;
            }
        }

        public string CurrentUserId
        {
            get
            {
                if (_principalUser != null) return _principalUser.Identity.GetUserId();
                return null;
            }
        }

        private const string CLIENT_ID = "ClientId";

        public ServiceBase() : this(new UnitOfWork())
        {

        }

        public ServiceBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Method

        public virtual async Task<TDto> CreateAsync(TDto model)
        {
            var entity = DtoToEntity(model);

            _repository.Add(MapClientId(entity));

            await _unitOfWork.CommitAsync();

            return EntityToDto(entity);
        }

        public virtual async Task<TDto> UpdateAsync(TDto model)
        {
            var entity = await _repository.FindByAsync(model.Id);

            if (entity != null)
            {
                entity = DtoToEntity(model, entity);

                _repository.Update(MapClientId(entity));

                await _unitOfWork.CommitAsync();

                return EntityToDto(entity);
            }

            throw new Exception("Not found entity object with id: " + model.Id);
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await _repository.FindByAsync(id);

            if (entity == null) throw new Exception("Not found entity object with id: " + id);

            _repository.Delete(entity);

            await _unitOfWork.CommitAsync();
        }

        public virtual async Task<IEnumerable<TDto>> FindAllAsync(Expression<Func<TEntity, bool>> pression = null)
        {
            return EntityToDto(await FindAll(pression).ToListAsync());
        }

        public virtual async Task<IEnumerable<TDto>> FindAllAsync<TOrderBy>(Expression<Func<TEntity, bool>> pression = null,
            Expression<Func<TEntity, TOrderBy>> orderBy = null, OrderType orderType = OrderType.Descending)
        {
            var query = FindAll(pression);

            query = BuildOrderBy(query, orderBy, orderType);

            return EntityToDto(await query.ToListAsync());
        }

        public virtual async Task<TDto> FindByIdAsync(int id)
        {
            return EntityToDto(await _repository.FindByAsync(id));
        }

        public virtual async Task<PageResultDto<TDto>> SearchAsync(Expression<Func<TEntity, bool>> pression = null,
            int skip = 0, int take = 10)
        {
            var query = FindAll(pression);

            IEnumerable<TEntity> entities = await query.OrderByDescending(x => x.Id).Skip((skip * take)).Take(take).ToListAsync();

            return new PageResultDto<TDto>(await query.CountAsync(), GetToTalPage(await query.CountAsync(), take), EntityToDto(entities));
        }

        public virtual async Task<PageResultDto<TDto>> SearchAsync<TOrderBy>(Expression<Func<TEntity, bool>> pression = null,
            Expression<Func<TEntity, TOrderBy>> orderBy = null, OrderType orderType = OrderType.Descending,
            int skip = 0, int take = 10)
        {
            var query = FindAll(pression);

            query = BuildOrderBy(query, orderBy, orderType);

            IEnumerable<TEntity> entities = await query.Skip(skip).Take(take).ToListAsync();

            return new PageResultDto<TDto>(await query.CountAsync(), GetToTalPage(await query.CountAsync(), take), EntityToDto(entities));
        }

        #endregion

        #region Protected Method

        protected IQueryable<TEntity> BuildOrderBy<TOrderBy>(IQueryable<TEntity> query,
          Expression<Func<TEntity, TOrderBy>> orderBy,
          OrderType orderType = OrderType.Descending)
        {
            if (orderBy != null)
            {
                query = (orderType == OrderType.Ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy));
            }
            else
            {
                query = query.OrderByDescending(x => x.Id);
            }

            return query;
        }

        protected IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> pression)
        {
            var property = typeof(TEntity).GetProperty(CLIENT_ID);

            IQueryable<TEntity> query = _repository.FindAll(pression);

            if (property != null && AllowedGrant == OAuthGrant.Client)
            {
                query = query.Where(LambdaHelper.BuildQuery<TEntity>("ClientId == " + ClientId));
            }

            return query;
        }

        protected TEntity MapClientId(TEntity entity)
        {
            var property = typeof(TEntity).GetProperty(CLIENT_ID);

            if (property != null)
            {
                entity.GetType().GetProperty(CLIENT_ID).SetValue(entity, ClientId);
            }

            return entity;
        }

        protected TDto EntityToDto(TEntity entity)
        {
            return Mapper.Map<TDto>(entity);
        }

        protected TEntity DtoToEntity(TDto dto)
        {
            return Mapper.Map<TEntity>(dto);
        }

        protected TEntity DtoToEntity(TDto dto, TEntity entity)
        {
            return Mapper.Map(dto, entity);
        }

        protected IEnumerable<TDto> EntityToDto(IEnumerable<TEntity> entities)
        {
            return Mapper.Map<IEnumerable<TDto>>(entities);
        }

        protected IEnumerable<TEntity> DtoToEntity(IEnumerable<TDto> dto)
        {
            return Mapper.Map<IEnumerable<TEntity>>(dto);
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

    }
}
