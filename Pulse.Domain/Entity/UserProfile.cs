namespace Pulse.Domain
{
    using System;
    using Enum;

    public class UserProfile : IEntity, IAudits
    {
        
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string AvatarPath { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string Address { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string ClientId { get; set; }

    }
}
