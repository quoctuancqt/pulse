namespace Pulse.Core.Dto.Mongo
{
    using Domain.Mongo.Enum;
    using MongoDB.Bson;

    public class ClientUserDto
    {
        public string UserId { get; set; }
        public GroupType GroupName { get; set; }
    }
}
