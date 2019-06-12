namespace Pulse.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Group : IEntity, IAudits
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string ClientId { get; set; }

        [Index]
        public virtual ICollection<Kiosk> Kiosks { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
