namespace Pulse.Core.Services
{
    using Dto.Entity;
    using Domain;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System;

    public class GroupService : ServiceBase<Group, GroupDto>, IGroupService
    {
        public GroupService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _repository = unitOfWork.Groups;
        }

        public async Task<GroupDto> FindNameAsync(string groupName)
        {
            var entity = await _repository.FindAll(g => g.Name.Equals(groupName)).FirstOrDefaultAsync();

            if (entity == null) return null;

            return EntityToDto(entity);
        }
    }
}
