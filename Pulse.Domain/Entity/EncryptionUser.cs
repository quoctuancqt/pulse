namespace Pulse.Domain
{
    using System.ComponentModel.DataAnnotations;

    public class EncryptionUser : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string UserEncrypted { get; set; }
        public string UserId { get; set; }
        public string PasswordHash { get; set; }
        public string SaltKey { get; set; }
        public string VIKey { get; set; }
    }
}
