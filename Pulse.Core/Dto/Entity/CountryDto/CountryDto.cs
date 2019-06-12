namespace Pulse.Core.Dto.Entity
{
    using FluentValidation.Attributes;
    using System.Collections.Generic;

    [Validator(typeof(CountryDtoValidator))]
    public class CountryDto: IDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public IEnumerable<KioskDto> Kiosks { get; set; }
    }
}
