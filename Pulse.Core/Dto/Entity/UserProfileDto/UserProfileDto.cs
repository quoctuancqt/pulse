namespace Pulse.Core.Dto.Entity
{
    using Domain.Enum;
    using Security.Identity.IdentityModels;
    using System;
    public class UserProfileDto: IDto
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
        public string ClientId { get; set; }
    }
}
