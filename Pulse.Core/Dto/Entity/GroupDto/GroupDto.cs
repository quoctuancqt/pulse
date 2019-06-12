namespace Pulse.Core.Dto.Entity
{
    using FluentValidation.Attributes;
    using System;
    using System.Collections.Generic;

    [Validator(typeof(GroupDtoValidator))]
    public class GroupDto: IDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? CreatedAt { get; set; }

        public IEnumerable<KioskDto> Kiosks { get; set; }
    }
}
