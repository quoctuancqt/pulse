namespace Pulse.Core.Services
{
    using Domain;
    using Dto.Entity;
    using System.Threading.Tasks;

    public interface IRefreshTokenService: IServiceBase<RefreshToken, RefreshTokenDto>
    {
        Task<RefreshTokenDto> FindAllRefreshTokensAsync(string refreshTokenId);
    }
}
