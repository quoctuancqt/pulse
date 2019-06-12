namespace Pulse.Core.Services
{
    using Domain;
    using Dto.Entity;
    using System.Threading.Tasks;

    public interface IGroupService: IServiceBase<Group, GroupDto>
    {
        Task<GroupDto> FindNameAsync(string groupName);
    }
}
