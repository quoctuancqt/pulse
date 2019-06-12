namespace Pulse.Domain.Mongo
{
    using Enum;
    using System;
    using System.Collections.Generic;

    public class MongoState
    {
        public MongoState(MongoStatus mongoStatus, Exception ex)
        {
            mongoStatus = MongoStatus;
            SetException(ex);
        }

        public MongoStatus MongoStatus { get; private set; }

        public IDictionary<string, object> Errors { get; private set; }

        private void SetException(Exception ex)
        {
            if (ex != null)
            {
                Errors.Add("Message", ex.Message);
                Errors.Add("StackTrace", ex.StackTrace);
            }
        }
    }
}
