namespace Pulse.Domain.Mongo
{
    using MongoDB.Bson;

    public interface IMongo<IdT>
    {
        IdT Id { get; set; }
    }

    public interface IMongo : IMongo<ObjectId>
    {

    }
}
