namespace Pulse.Core.Services
{
    using Domain;
    using Dto.Entity;
    public class EncryptionUserService : ServiceBase<EncryptionUser, EncryptionUserDto>, IEncryptionUserService
    {
        public EncryptionUserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _repository = unitOfWork.EncryptionUsers;
        }
    }
}
