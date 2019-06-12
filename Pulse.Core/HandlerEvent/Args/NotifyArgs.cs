namespace Pulse.Core.HandlerEvent.Args
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class NotifyArgs
    {
        public string MachineId { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public string UserId { get; set; }
        public bool IsRead { get; set; }
        public string Status { get; set; }
        public string Content { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
