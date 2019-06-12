using Pulse.Core.Dto.Entity;
using Pulse.FakeData.FakeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pulse.FakeData
{
    public class FakeGroups : IDtoFaker<GroupDto>
    {
        public GroupDto CreateDto()
        {
            const string GROUP_NAME = "myGroup";
            var groupDto = new GroupDto
            {
                CreatedAt = DateTime.Now,
                Id = 1,
                Name = GROUP_NAME
            };

            return groupDto;
        }

        public GroupDto CreateInvalidDto()
        {
            var groupDto = new GroupDto
            {
                CreatedAt = DateTime.Now,
                Id = 1
            };

            return groupDto;
        }
    }
}
