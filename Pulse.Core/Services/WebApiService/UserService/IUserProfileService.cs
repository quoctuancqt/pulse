namespace Pulse.Core.Services
{
    using Domain;
    using Dto.Entity;
    using Microsoft.AspNet.Identity;
    using Security.Identity.IdentityModels;
    using System.Threading.Tasks;

    public interface IUserProfileService : IServiceBase<UserProfile, UserProfileDto>
    {
        Task<UserProfileDto> CreateAsync(string username, string password, string role = "User");

        Task AddUserToClient(string clientId, string userId);

        Task AddClientUserAsync(string userId, string clientId, string secretKey = "");

        Task<PulseIdentityUser> FindByUsernameAsync(string userName);

        Task<UserProfileDto> FindByUserIdAsync(string userId);

        Task<string> FindUserNameByRefreshIdAsync(string refreshId);

        Task<IdentityResult> ActiveUserAsync(string userId);

        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    }
}
