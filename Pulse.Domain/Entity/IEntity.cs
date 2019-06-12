using System.ComponentModel.DataAnnotations;

namespace Pulse.Domain
{
    public interface IEntity : IEntity<int>
    {
    }

    public interface IEntity<T>
    {
        [Key]
        T Id { get; set; }
    }
}
