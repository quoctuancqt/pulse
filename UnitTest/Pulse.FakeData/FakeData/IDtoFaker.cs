namespace Pulse.FakeData.FakeData
{
    using Pulse.Core.Dto.Entity;

    public interface IDtoFaker<TDto> where TDto: class, IDto
    {
        TDto CreateDto();

        TDto CreateInvalidDto();
    }
}
