namespace Pulse.Core.Dto.Entity
{
    public interface IDto<T>
    {
        T Id { get; set; }
    }

    public interface IDto : IDto<int> { }
}
