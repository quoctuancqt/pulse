namespace Pulse.Domain
{
    using Enum;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class History : IEntity
    {
        [Key]
        public int Id { get; set; }

        public ProcessType ProcessType { get; set; }

        public string Comment { get; set; }

        public int HistoryId { get; set; }

        public string HistoryName { get; set; }

        public HistoryType HistoryType { get; set; }

        public string UserId { get; set; }

        public string MachineId { get; set; }

        public string FullName { get; set; }

        public string ClientId { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
