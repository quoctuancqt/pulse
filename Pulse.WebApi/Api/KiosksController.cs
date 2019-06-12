namespace Pulse.WebApi.Api
{
    using Core.Dto.Entity;
    using Core.Dto.Mongo;
    using Core.Services;
    using Domain;
    using Domain.Mongo;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix("api/kiosks")]
    public class KiosksController : BaseApiController<Kiosk, KioskDto, IKioskService>
    {
        private readonly IKioskSecurityService _kioskSecurityService;

        private readonly IMongoKioskService _mongoKioskService;

        public KiosksController(IKioskService service, IKioskSecurityService kioskSecurityService, IMongoKioskService mongoKioskService)
            : base(service)
        {
            _kioskSecurityService = kioskSecurityService;
            _mongoKioskService = mongoKioskService;
            _mongoKioskService.PrincipalUser = User;
        }

        [Route("{machineId}/updateconnectionid"), HttpPut]
        public async Task<IHttpActionResult> UpdateConnectionId(string machineId, KioskDto model)
        {
            await _service.UpdateConnectionIdAsync(machineId, model.ConnectionId);

            return Success();
        }

        [Route("{machineId}/updatestatus"), HttpPut]
        public async Task<IHttpActionResult> UpdateStatusByMachineId(string machineId, KioskDto model)
        {
            return Ok(await _service.UpdateStatusByMachineIdAsync(machineId, model.Status));
        }

        [Route("bymachineid/{machineId}"), HttpGet]
        public async Task<IHttpActionResult> GetByMachineId(string machineId)
        {
            return Ok(await _service.FindByMachineIdAsync(machineId));
        }

        [Route("search"), HttpGet]
        public async Task<IHttpActionResult> Search(string name = "", string address = "",
            int countryId = -1, int groupId = -1, int skip = 0, int take = 10)
        {
            var result = await _service.SearchAsync(k =>
                (string.IsNullOrEmpty(name) ? true : k.Name.ToLower().Contains(name.ToLower()))
            && (string.IsNullOrEmpty(address) ? true : k.Address.ToLower().Contains(address.ToLower()))
            && (countryId == -1 ? true : k.CountryId == countryId)
            && (k.ClientId.Equals(_service.ClientId))
            , skip, take);

            return Ok(result);
        }

        #region KiosksSecurity

        [Route("generatelicensekey"), HttpPost]
        public async Task<IHttpActionResult> GenerateLicenseKey(IDictionary<string, string> dic)
        {
            return Ok(await _kioskSecurityService.GenerateLicenseKeyAsync(dic["clientId"], Convert.ToInt32(dic["number"])));
        }

        [Route("checklicensekey/{key}"), HttpGet, AllowAnonymous]
        public async Task<IHttpActionResult> CheckLicenseKey(string key)
        {
            var result = await _kioskSecurityService.CheckLicenseKeyAsync(key);

            return Ok(result);
        }

        [Route("{key}/updateKiosksecuritybykey"), HttpPut]
        public async Task<IHttpActionResult> UpdateKioskSecurityByKey(string key, KioskSecurityDto kioskSecurityDto)
        {
            return Ok(await _kioskSecurityService.UpdateByLicenseKeyAsync(key, kioskSecurityDto));
        }

        [Route("searchLicense"), HttpGet]
        public async Task<IHttpActionResult> SearchLicense(string licenseKey = "", string clientId = "", bool isActive = false, int skip = 0, int take = 10)
        {
            var result = await _kioskSecurityService.SearchAsync(l =>
            (string.IsNullOrEmpty(licenseKey) ? !l.LicenseKey.Equals(licenseKey) : l.LicenseKey.Equals(licenseKey))
            && l.IsActive == isActive && (string.IsNullOrEmpty(clientId) ? l.ClientId == _service.ClientId : l.ClientId == clientId), skip, take);

            return Ok(result);
        }

        #endregion

        #region Mongo
        [HttpPost, Route("addjsonkiosk")]
        public async Task<IHttpActionResult> AddJsonMongo(MongoKioskDto dto)
        {
            return Ok(await _mongoKioskService.CreateAsync(dto));
        }

        [HttpGet, Route("{machineId}/search")]
        public async Task<IHttpActionResult> searchJsonMongo(string machineId, DateTime startDate, DateTime endDate, int skip = 0, int take = 10)
        {
            return Ok(await _mongoKioskService.SearchAsync(machineId, (x => x.CreateAt >= startDate.Date && x.CreateAt <= endDate.Date), skip, take));
        }

        #endregion
    }
}
