namespace Pulse.Core.Services
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain;
    using Dto.Entity;

    public class RefreshTokenService : ServiceBase<RefreshToken, RefreshTokenDto>, IRefreshTokenService
    {
        public RefreshTokenService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _repository = unitOfWork.RefreshTokens;
        }

        public async Task<RefreshTokenDto> FindAllRefreshTokensAsync(string refreshTokenId)
        {
            var result = await _repository.FindAll(r => r.RefreshTokenId.Equals(refreshTokenId)).FirstOrDefaultAsync();

            return EntityToDto(result);
        }
    }
}
