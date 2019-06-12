namespace Pulse.Core.Services
{
    using System;
    using System.Threading.Tasks;
    using Domain;
    using Dto.Entity;
    using Security.Identity.IdentityModels;
    using Security.Identity;
    using Common.Helpers;
    using System.Linq;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class KioskSecurityService: ServiceBase<KioskSecurity, KioskSecurityDto>, IKioskSecurityService
    {
        private readonly IEncryptionUserService _encryptionUserService;

        private readonly IUserProfileService _userProfileService;

        private const string VIKEY = "@1B2c3D4e5F6g7H8";

        public KioskSecurityService(IUnitOfWork unitOfWork, 
            IEncryptionUserService encryptionUserService,
            IUserProfileService userProfileService) : base(unitOfWork)
        {
            _repository = unitOfWork.KioskSecurities;
            _encryptionUserService = encryptionUserService;
            _userProfileService = userProfileService;
        }

        public async Task<IEnumerable<string>> GenerateLicenseKeyAsync(string clientId, int number)
        {
            IList<string> licenses = new List<string>();

            for (int i = 0; i < number; i++)
            {
                var username = UnitHelper.RandomString();
                var password = UnitHelper.RandomString();

                var userProfile = await _userProfileService.CreateAsync(username, password, PulseIdentityRole.Kiosk);

                await _userProfileService.AddClientUserAsync(userProfile.UserId, clientId);

                string passwordHash = password.GeneratePasswordHash();

                var encryptionUserDto = await CreateEncryptionUserAsync(username, password, passwordHash, userProfile.UserId);

                _repository.Add(new KioskSecurity
                {
                    MachineId = userProfile.UserId,
                    LicenseKey = userProfile.UserId,
                    IsActive = false,
                    EncryptionUserId = encryptionUserDto.Id,
                    ClientId = clientId,
                });

                await _unitOfWork.CommitAsync();

                licenses.Add(userProfile.UserId);
            }

            return licenses;
        }

        public async Task<KioskSecurityDto> CheckLicenseKeyAsync(string key)
        {
            var result = await FindAllAsync(k => k.LicenseKey.Equals(key) && k.IsActive == false && k.MacAddress == null);
            return result.FirstOrDefault();
        }

        public async Task<KioskSecurityDto> UpdateByLicenseKeyAsync(string key, KioskSecurityDto kioskSecurityDto)
        {
            var kioskSecurity = await FindAll(k => k.LicenseKey.Equals(key)).FirstOrDefaultAsync();

            if (kioskSecurity == null) throw new NotImplementedException("Not Found KioskSecurity");

            kioskSecurity.MacAddress = kioskSecurityDto.MacAddress;

            kioskSecurity.IsActive = kioskSecurityDto.IsActive;

            _unitOfWork.KioskSecurities.Update(kioskSecurity);

            await _unitOfWork.CommitAsync();

            return EntityToDto(kioskSecurity);

        }

        private async Task<EncryptionUserDto> CreateEncryptionUserAsync(string username, string password, string passwordHash, string userId)
        {
            string saltKey = UnitHelper.CreateSalt();

            return await _encryptionUserService.CreateAsync(new EncryptionUserDto
            {
                UserEncrypted = UnitHelper.Encrypt(string.Format("{0}&{1}", username, password), passwordHash, saltKey, VIKEY),
                PasswordHash = passwordHash,
                SaltKey = saltKey,
                VIKey = VIKEY,
                UserId = userId
            });
        }

    }
}
