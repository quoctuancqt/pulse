namespace Pulse.Domain
{
    using System;

    public interface IAudits
    {
        DateTime? CreatedAt { get; set; }

        DateTime? UpdatedAt { get; set; }

        string CreatedBy { get; set; }

        string UpdatedBy { get; set; }
    }
}
