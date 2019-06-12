namespace Pulse.Core.HandlerEvent.Args
{
    using Domain.Enum;
    using Dto.Entity;

    public class HistoryArgs
    {
        public ProcessType ProcessType { get; set; }
        public HistoryType HistoryType { get; set; }
        public string HistoryName { get; set; }
        public string Comment { get; set; }
        public string UserId { get; set; }
        public string MachineId { get; set; }
        public int HistoryId { get; set; }
        public string FullName { get; set; }
        public string ClientId { get; set; }
        public HistoryDto HistoryDto
        {
            get
            {
                return MapToHistoryDto();
            }
        }
        private HistoryDto MapToHistoryDto()
        {
            return new HistoryDto
            {
                ProcessType = this.ProcessType,
                HistoryType = this.HistoryType,
                Comment = this.Comment,
                HistoryId = this.HistoryId,
                UserId = this.UserId,
                HistoryName = this.HistoryName,
                MachineId = this.MachineId,
                FullName = this.FullName,
                ClientId = this.ClientId
            };
        }
    }
}
