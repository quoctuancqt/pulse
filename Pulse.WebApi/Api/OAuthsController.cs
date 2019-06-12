namespace Pulse.WebApi.Api
{
    using Core.Dto.Entity;
    using Core.HandlerEvent;
    using Core.HandlerEvent.Args;
    using Core.Services;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Security.Claims;
    using Microsoft.AspNet.Identity;
    using Domain.Mongo.Enum;

    [RoutePrefix("api/oauths")]
    [Authorize]
    public class OAuthsController : ApiController
    {
        private readonly IProxyService _proxyService;

        private readonly IEncryptionUserService _encryptionUserService;

        private readonly IUserProfileService _userProfileService;

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger
  (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ClaimsIdentity claimsIdentity
        {
            get
            {
                if (User != null) return (ClaimsIdentity)User.Identity;
                return null;
            }
        }

        private string _clientId
        {
            get
            {
                if (claimsIdentity == null) return string.Empty;

                return claimsIdentity.FindFirstValue("clientId");
            }
        }

        public OAuthsController(IProxyService proxyService, IEncryptionUserService encryptionUserService,
            IUserProfileService userProfileService)
        {
            _proxyService = proxyService;

            _encryptionUserService = encryptionUserService;

            _userProfileService = userProfileService;

            _userProfileService.PrincipalUser = User;

        }

        [AllowAnonymous, HttpPost]
        public async Task<IHttpActionResult> Post([FromBody]Dictionary<string, string> @param)
        {
            var result = await _proxyService.EnsureApiTokenAsync(@param["username"], @param["password"]);

            if (result.ContainsKey("fullName"))
            {
                var events = new SignalREventHandlers(result["access_token"]);
                await events.TriggerUserActivitiesAsync(new UserActivitiesArgs
                {
                    Action = ActionType.Login,
                    Name = result["fullName"],
                });
            }
 
            return Ok(result);
        }

        [HttpPost, Route("register")]
        public async Task<IHttpActionResult> Register([FromBody]Dictionary<string, string> @param)
        {
            var result = await _userProfileService.CreateAsync(@param["username"], @param["password"], @param["role"]);

            await _userProfileService.AddUserToClient(_userProfileService.ClientId, result.UserId);

            return Ok(result);
        }

        [AllowAnonymous, HttpGet]
        public async Task<IHttpActionResult> Get(string refreshId)
        {
            return Ok(await _proxyService.RefreshApiTokenAsync(refreshId));
        }

        [AllowAnonymous, HttpGet, Route("getusernamebyrefreshId/{refreshId}")]
        public async Task<IHttpActionResult> GetUserNameByRefreshId(string refreshId)
        {
            return Ok(await _userProfileService.FindUserNameByRefreshIdAsync(refreshId));
        }

        [HttpGet, Route("checktoken")]
        public IHttpActionResult CheckToken()
        {
            return Content(System.Net.HttpStatusCode.OK, "Successful");
        }

        [HttpPost, Route("logout")]
        public async Task<IHttpActionResult> Logout([FromBody]Dictionary<string, string> @param)
        {
            var user = await _userProfileService.FindByUserIdAsync(_userProfileService.CurrentUserId);
            var events = new SignalREventHandlers(@param["token"]);
            await events.TriggerUserActivitiesAsync(new UserActivitiesArgs
            {
                Action = ActionType.Logout,
                Name = user.FullName
            });

            return Content(System.Net.HttpStatusCode.OK, "Successful");
        }

        [HttpGet, Route("search")]
        public async Task<IHttpActionResult> Search(string name = "", int skip = 0, int take = 10)
        {
            var result = await _userProfileService.SearchAsync(u => (string.IsNullOrEmpty(name) ? true : u.FullName.ToLower().Contains(name.ToLower())), skip, take);

            return Ok(result);
        }

        [HttpGet, Route("getuserprofile")]
        public async Task<IHttpActionResult> GetUserProfile()
        {
            var result = await _userProfileService.FindByUserIdAsync(_userProfileService.CurrentUserId);

            return Ok(result);
        }

        [HttpPut, Route("updateuserprofile")]
        public async Task<IHttpActionResult> UpdateUserProfile(UserProfileDto model)
        {
            var result = await _userProfileService.UpdateAsync(model);

            await _userProfileService.ActiveUserAsync(model.UserId);

            return Content(System.Net.HttpStatusCode.OK, "Successful");
        }

        [HttpPost, Route("changepassword")]
        public async Task<IHttpActionResult> ChangePassword([FromBody]Dictionary<string, string> @param)
        {
            var result = await _userProfileService.ChangePasswordAsync(_userProfileService.CurrentUserId, @param["currentPassword"], @param["newPassword"]);

            return Ok(result);
        }
    }
}
