namespace Pulse.Core.Dto.Entity
{
    using System.Collections.Generic;   
    public class EncryptionUserDto: IDto
    {
        public int Id { get; set; }

        public string UserEncrypted { get; set; }

        public string UserId { get; set; }

        public string ClientId { get; set; }

        public string PasswordHash { get; set; }

        public string SaltKey { get; set; }

        public string VIKey { get; set; }

    }
}
