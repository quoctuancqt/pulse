namespace Pulse.Core.Services
{
    using Domain.Entity;
    using Dto.Entity;
    using System.Threading.Tasks;

    public interface IClientService: IServiceBase<Client, ClientDto>
    {
        Task<ClientDto> FindByClientIdAsync(string clientId);

        Task<string> GenerateHeaderAsync(string userName);

        Task<ClientDto> FindByNameAsync(string name);
    }
}
