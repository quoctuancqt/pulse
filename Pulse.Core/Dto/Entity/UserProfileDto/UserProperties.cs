namespace Pulse.Core.Dto.Entity
{ 
    public class UserProperties
    {
        public string UserName { get; set; }

        public string ClientId { get; set; }

        public string FullName { get; set; }

        public string ClientName { get; set; }

        public string Role { get; set; }
        public string AvatarPath { get; set; }
        public bool EmailConfirm { get; set; }
    }
}
