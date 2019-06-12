namespace Pulse.WebApi.Api
{
    using Common.ResolverFactories;
    using Core.Dto.Entity;
    using Core.Services;
    using Domain;
    using FluentValidation;
    using FluentValidation.Attributes;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Net;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Results;

    [Authorize]
    public abstract class BaseApiController<TEntity, TDto, TService> : ApiController
          where TDto : class, IDto
          where TEntity: class, IEntity 
          where TService : IServiceBase<TEntity, TDto>
    {

        protected readonly TService _service;

        protected readonly string _userId;

        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger
(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public BaseApiController(TService service)
        {
            _service = service;
            _service.PrincipalUser = User;
            _userId = User.Identity.GetUserId();
        }

        [HttpGet, Route("{id:int}")]
        public virtual async Task<IHttpActionResult> Get(int id)
        {
            var result = await _service.FindByIdAsync(id);

            if(result == null) return NotFound();

            return Ok(result);
        }

        [HttpGet, Route("")]
        public virtual async Task<IHttpActionResult> GetAll()
        {
            return Ok(await _service.FindAllAsync());
        }

        [HttpPost]
        public virtual async Task<IHttpActionResult> Post([FromBody] TDto model)
        {
            var Validation = CheckValidation(model);

            if (!Validation.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, Validation.Errors);
            }
            var result = await _service.CreateAsync(model);
            return Ok(result);
        }

        [HttpPut]
        public virtual async Task<IHttpActionResult> Put([FromBody] TDto model)
        {
            var Validation = CheckValidation(model);

            if (!Validation.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, Validation.Errors);
            }

            return Ok(await _service.UpdateAsync(model));
        }

        [HttpDelete]
        public virtual async Task<IHttpActionResult> Delete([FromBody] TDto model)
        {
            await _service.DeleteAsync(model.Id);

            return Ok("Success");
        }

        private ValidationDto CheckValidation(TDto model)
        {
            var attrs = typeof(TDto).GetCustomAttributes(true);
            ValidatorAttribute attr = (ValidatorAttribute)Attribute.GetCustomAttribute(typeof(TDto), typeof(ValidatorAttribute));
            if (attr != null)
            {
                IValidator validator = ResolverFactory.CreateInstance<IValidator>(attr.ValidatorType.AssemblyQualifiedName);
                return new ValidationDto(validator.Validate(model));
            }
            else
            {
                return new ValidationDto();
            }
        }

        protected NegotiatedContentResult<string> Forbidden()
        {
            return Content(HttpStatusCode.Forbidden, "Forbidden");
        }

        protected NegotiatedContentResult<string> Success()
        {
            return Content(HttpStatusCode.OK, "Success");
        }

    }

}
