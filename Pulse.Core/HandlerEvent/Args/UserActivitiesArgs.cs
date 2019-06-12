namespace Pulse.Core.HandlerEvent.Args
{
    using Domain.Mongo.Enum;

    public class UserActivitiesArgs
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public ActionType Action { get; set; }
    }
}
