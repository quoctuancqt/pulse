namespace Pulse.Domain
{
    using System;
    public class RefreshToken: IEntity
    {
        public int Id { get; set; }
        public string RefreshTokenId { get; set; }
        public string Subject { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public string ProtectedTicket { get; set; }
    }
}
