namespace Pulse.Core.Services
{
    using System.Threading.Tasks;
    using Domain;
    using Dto.Entity;
    using Microsoft.AspNet.Identity;
    using Security.Identity;
    using Security.Identity.IdentityModels;
    using System.Linq;
    using System;
    using System.Data.Entity;
    using Common.Helpers;
    using EmailTemplete;
    using EmailTemplete.Model;

    public class UserProfileService : ServiceBase<UserProfile, UserProfileDto>, IUserProfileService
    {
        private readonly PulseUserManager _userManager;

        private readonly IRefreshTokenService _refreshTokenService;

        private readonly IProcessEmailTemplate _processEmailTemplate;

        private const string SUBJECT = "PULSE TEKCENT";

        public UserProfileService(IUnitOfWork unitOfWork, PulseUserManager userManager,
            IRefreshTokenService refreshTokenService,
            SimpleEmailTemplate processEmailTemplate)
            : base(unitOfWork)
        {
            _repository = unitOfWork.UserProfiles;
            _userManager = userManager;
            _refreshTokenService = refreshTokenService;
            _processEmailTemplate = processEmailTemplate;
        }

        public async Task<UserProfileDto> CreateAsync(string username, string password, string role = "User")
        {
            string passwordHash = password.GeneratePasswordHash();

            var pulseIdentityUser = new PulseIdentityUser
            {
                UserName = username,
                Email = (username.IsEmail() ? username : null),
                PasswordHash = passwordHash,
                SecurityStamp = passwordHash,
                ClientId = ClientId
            };

            IdentityResult result = await _userManager.CreateAsync(pulseIdentityUser);

            if (!result.Succeeded) throw new Exception(result.Errors.FirstOrDefault());

            await _userManager.AddToRoleAsync(pulseIdentityUser.Id, role);

            var userProfileDto = new UserProfileDto
            {
                UserId = pulseIdentityUser.Id,
                Email = pulseIdentityUser.Email,
                FullName = "Unknown",
                Password = password,
            };

            if (role != PulseIdentityRole.Kiosk)
            {
                var body = _processEmailTemplate.GenerateEmailTemplate(new LoginModel
                {
                    UserName = username,
                    PassWord = password,
                });

                await _userManager.SendEmailAsync(pulseIdentityUser.Id, SUBJECT, body);

                return await CreateAsync(userProfileDto);
            }

            return userProfileDto;
        }

        public async Task<UserProfileDto> FindByUserIdAsync(string userId)
        {
            var result = await _unitOfWork.UserProfiles.FindAll(u => u.UserId.Equals(userId)).FirstOrDefaultAsync();

            if (result == null) return null;

            return EntityToDto(result);
        }

        public async Task AddClientUserAsync(string userId, string clientId, string secretKey = "")
        {
            if (string.IsNullOrEmpty(secretKey))
            {
                var client = await _unitOfWork.Clients.FindAll(c => c.ClientId.Equals(clientId)).FirstOrDefaultAsync();
                secretKey = client.SecretKey;
            }

            var userProfile = await _repository.FindAll(u => u.UserId.Equals(userId)).FirstOrDefaultAsync();

            if (userProfile != null)
            {
                userProfile.ClientId = clientId;

                _unitOfWork.UserProfiles.Update(userProfile);

                await _unitOfWork.CommitAsync();
            }

            await _userManager.UpdateByClientIdAsync(userId, clientId, secretKey);
        }

        public async Task<PulseIdentityUser> FindByUsernameAsync(string userName)
        {
            var result = await _userManager.FindByNameAsync(userName);

            if (result == null) throw new Exception("Can not found user with username: " + userName);

            return result;
        }

        public async Task<string> FindUserNameByRefreshIdAsync(string refreshId)
        {
            var result = await _refreshTokenService.FindAllRefreshTokensAsync(UnitHelper.GetHash(refreshId));

            if (result == null) throw new Exception("Not found user with refreshId: " + refreshId);

            return result.Subject;
        }

        public async Task AddUserToClient(string clientId, string userId)
        {
            var client = await _unitOfWork.Clients.FindAll(c => c.ClientId.Equals(clientId)).FirstOrDefaultAsync();

           var result = await _userManager.UpdateByClientIdAsync(userId, clientId, client.SecretKey);

            if (!result.Succeeded) throw new Exception(result.Errors.FirstOrDefault());

        }

        public async Task<IdentityResult> ActiveUserAsync(string userId)
        {
            var result = await _userManager.ActiveUserAsync(userId);

            return result;
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(userId, currentPassword, newPassword);

            //Todo send email alert

            return result;
        }
    }
}
