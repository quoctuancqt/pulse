namespace Pulse.Core.Dto.Entity
{
    using Domain.Enum;
    using System;

    public class HistoryDto : IDto
    {
        public int Id { get; set; }

        public ProcessType ProcessType { get; set; }

        public string ProcessTypeValue { get; set; }

        public string Comment { get; set; }

        public int HistoryId { get; set; }

        public string HistoryName { get; set; }

        public HistoryType HistoryType { get; set; }

        public string HistoryTypeValue { get; set; }

        public string UserId { get; set; }

        public string MachineId { get; set; }

        public string FullName { get; set; }
        public string ClientId { get; set; }

        public string DateFormat { get; set; }

        public string CountDate { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
